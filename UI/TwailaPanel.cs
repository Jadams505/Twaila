using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using Twaila.Config;
using Twaila.Context;
using Twaila.Systems;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel, IDragable
    {
        public Layout Layout { get; set; }
        public BaseContext CurrentContext { get; set; }
        public BaseContext PriorityContext { get; set; }

        public int tick = 0;

        private int pickIndex = 0;
        private bool _dragging;
        private Point _lastMouse;
        

        private Vector2 MaxPanelDimension => new Vector2(TwailaConfig.Instance.MaxWidth / 100.0f * Parent.GetDimensions().Width, TwailaConfig.Instance.MaxHeight / 100.0f * Parent.GetDimensions().Height);
        private Vector2 MaxPanelInnerDimension => new Vector2(MaxPanelDimension.X - PaddingLeft - PaddingRight, MaxPanelDimension.Y - PaddingTop - PaddingLeft);

        private static PositionData CurrentPositionData => TwailaConfig.Instance.PanelPositionData.GetActiveContext();

        public TwailaPanel()
        {
            Layout = new Layout();
            Layout.Image.MarginRight = 10;
            Layout.Apply(this);

            Width.Set(0, 0);
            Height.Set(0, 0);
            Top.Set(0, 0);
            Left.Set(PlayerInput.RealScreenWidth / 2, 0);
        }

        public override void Update(GameTime gameTime)
        {
            // The order of these function calls is VERY important

            // Shouldn't update if it shouldn't be seen
            if (!CurrentPositionData.ShowUI)
                return;

            // Adds elements to the panel
            if (!IsDragging() && !Main.gamePaused)
            {
                UpdatePanelContents();
            }

            // Applies config settings to the new elements (text shadow, opacity, etc)
            UpdateFromConfig();

            // Sets the sizes of the panel and it's children based on their content sizes
            UpdateSize();

            // This calls Update on all the panel's children, important for wrapper elements like the grid element
            // This has to be called after UpdateSize because children elements rely on the parent's dimensions
            // This has to be called before UpdateAlignment because this moves elements around
            base.Update(gameTime);
            
            // Handles changing anchor pos when the panel is being dragged
            Drag();

            // Moves the children elements where they belong
            // Updates the position of the panel to match it's anchor pos
            UpdateAlignment();
        }

        private void UpdateFromConfig()
        {
            TwailaConfig config = TwailaConfig.Instance;

            this.AppendOrRemove(Layout.Image, config.DisplayContent.ShowImage);
            this.AppendOrRemove(Layout.Mod, config.DisplayContent.ShowMod);
            this.AppendOrRemove(Layout.Name, config.DisplayContent.ShowName != TwailaConfig.NameType.Off);

            Layout.ApplyConfigSettings(config);

            BackgroundColor = config.PanelColor.Color;
            BorderColor = Color.Black;
            if (ContainsPoint(Main.MouseScreen) && !IsDragging())
            {
                BackgroundColor *= config.HoverOpacity;
                BorderColor *= config.HoverOpacity;

                Layout.ApplyHoverSettings(config);
            }
        }

        private void UpdateSize()
        {
            SetPadding(TwailaConfig.Instance.PanelPadding);
            Layout.SetInitialSizes();
            if (!HasChild(Layout.InfoBox) && !HasChild(Layout.Name) && !HasChild(Layout.Mod))
            {
                Layout.Image.MarginRight = 0;
            }

            Vector2 imageDimension = Layout.Image.GetSizeIfAppended();
            Vector2 textDimension = Layout.TextColumnSize();

            float imageMarginX = Layout.Image.GetSizeIfAppended(Layout.Image.MarginRight);

            DrawMode drawMode = TwailaConfig.Instance.ContentSetting;
            if (drawMode == DrawMode.Shrink)
            {
                float reservedImageWidth = imageDimension.X * ImageScale(new Vector2(TwailaConfig.Instance.ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));
                
                Vector2 maxSize = new Vector2(MaxPanelInnerDimension.X - reservedImageWidth - imageMarginX, MaxPanelInnerDimension.Y);

                Layout.TextElements.Sort((a, b) => (a.GetContentSize().Y / a.SizePriority().Y).CompareTo(b.GetContentSize().Y / b.SizePriority().Y));

                Vector2 totalSizePriority = Vector2.Zero;
                foreach (var element in Layout.TextElements)
                {
                    var priority = element.SizePriority();
                    totalSizePriority.Y += priority.Y;
                    totalSizePriority.X = Math.Max(priority.X, totalSizePriority.X);
                }
                
                Vector2 totalSpace = new Vector2(maxSize.X, maxSize.Y);
                for (int i = 0; i < Layout.TextElements.Count; ++i)
                {
                    UITwailaElement element = Layout.TextElements[i];
                    Vector2 priority = element.SizePriority();
                    float priorityFactor = priority.Y / totalSizePriority.Y;
                    Vector2 elementSpace = new Vector2(maxSize.X, totalSpace.Y * priorityFactor);
                    

                    element.ScaleElement(elementSpace);
                    Vector2 scaledSize = new Vector2(element.Width.Pixels, element.Height.Pixels);
                    totalSpace.Y -= Math.Min(scaledSize.Y, elementSpace.Y);
                    totalSizePriority -= element.SizePriority();
                }

                float contentWidth = Utils.Max(Layout.Name.Width.Pixels, Layout.InfoBox.Width.Pixels, Layout.Mod.Width.Pixels);

                Layout.Name.Width.Set(contentWidth, 0);
                Layout.InfoBox.Width.Set(contentWidth, 0);
                Layout.Mod.Width.Set(contentWidth, 0);

                textDimension.X = Layout.TextColumnSize().X;
                textDimension.Y = Layout.TextColumnSize().Y;

                Vector2 remainingSpace = new Vector2(MaxPanelInnerDimension.X - textDimension.X - imageMarginX, MaxPanelInnerDimension.Y);

                if (TwailaConfig.Instance.UseTextHeightForImage)
                {
                    remainingSpace.Y = textDimension.Y;
                }

                imageDimension.X *= ImageScale(remainingSpace);
                imageDimension.Y *= ImageScale(remainingSpace);
            }
            else
            {
                if (drawMode == DrawMode.Trim)
                {
                    float height = 0;
                    float width = 0;

                    TrimElementVertically(Layout.Name, ref width, ref height);
                    TrimElementVertically(Layout.InfoBox, ref width, ref height);
                    TrimElementVertically(Layout.Mod, ref width, ref height);

                    textDimension.Y = height;
                    textDimension.X = width;
                }
                imageDimension.Y = Math.Min(MaxPanelInnerDimension.Y, imageDimension.Y);
                textDimension.Y = Math.Min(MaxPanelInnerDimension.Y, textDimension.Y);
                imageDimension.X = Math.Min(TwailaConfig.Instance.ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, imageDimension.X);
                textDimension.X = Math.Min(MaxPanelInnerDimension.X - imageDimension.X - imageMarginX, textDimension.X);

                Layout.Name.Width.Set(textDimension.X, 0);
                Layout.InfoBox.Width.Set(textDimension.X, 0);
                Layout.Mod.Width.Set(textDimension.X, 0);

                if(imageDimension.X == 0 || imageDimension.Y == 0)
                {
                    imageDimension.X = 0;
                    imageDimension.Y = 0;
                }
            }

            if(TwailaConfig.Instance.UseTextHeightForImage && imageDimension.Y > textDimension.Y)
            {
                imageDimension.Y = textDimension.Y;
            }

            float calculatedHeight = Math.Max(imageDimension.Y, textDimension.Y);
            Height.Set(calculatedHeight + PaddingTop + PaddingBottom, 0);
            Layout.Image.Height.Set(calculatedHeight, 0);
            
            float calculatedWidth = textDimension.X + imageDimension.X + imageMarginX + PaddingLeft + PaddingRight;
            Width.Set(calculatedWidth, 0);
            Layout.Image.Width.Set(imageDimension.X, 0);
        }

        private void TrimElementVertically(UITwailaElement element, ref float currentWidth, ref float currentHeight)
        {
            Vector2 elementSize = element.GetSizeIfAppended();
            if (elementSize.Y + currentHeight < MaxPanelInnerDimension.Y)
            {
                currentHeight += elementSize.Y;
                currentWidth = Math.Max(currentWidth, elementSize.X);
            }
            else
            {
                float trimHeight = Math.Max(0, MaxPanelInnerDimension.Y - currentHeight);
                element.Height.Set(trimHeight, 0);
                if (trimHeight > 0)
                {
                    currentHeight += trimHeight;
                    currentWidth = Math.Max(currentWidth, elementSize.X);
                }
            }
        }

        public float ImageScale(Vector2 maxSize)
        {
            float scaleX = 1;
            float width = Layout.Image.GetSizeIfAppended(Layout.Image.Render.Width);
            if (width > 0 && width > maxSize.X)
            {
                scaleX = maxSize.X / width;
            }
            float scaleY = 1;
            float height = Layout.Image.GetSizeIfAppended(Layout.Image.Render.Height);
            if (height > 0 && height > maxSize.Y)
            {
                scaleY = maxSize.Y / height;
            }
            return Math.Min(scaleX, scaleY);
        }

        private void UpdateAlignment()
        {
            var positionData = CurrentPositionData;
            if (positionData.UseDefaultPosition)
            {
                positionData.AnchorPosX = (int)Parent.GetDimensions().Width / 2;
                positionData.AnchorPosY = 0;
            }
            UpdatePos();
            Layout.Image.Top.Set(0, 0);
            float textColX = Layout.Image.GetSizeIfAppended(Layout.Image.Width.Pixels + Layout.Image.MarginRight);
            Layout.Name.Left.Set(textColX, 0);
            Layout.InfoBox.Left.Set(textColX, 0);
            Layout.Mod.Left.Set(textColX, 0);

            Layout.UpdateTextColumnVertically();
            Recalculate();
        }

        private void UpdatePos()
        {
            var positionData = CurrentPositionData;
            float left = 0;
            switch (positionData.AnchorX)
            {
                case TwailaConfig.HorizontalAnchor.Left:
                    left = positionData.AnchorPosX;
                    break;
                case TwailaConfig.HorizontalAnchor.Center:
                    left = positionData.AnchorPosX - Width.Pixels / 2;
                    break;
                case TwailaConfig.HorizontalAnchor.Right:
                    left = positionData.AnchorPosX - Width.Pixels;
                    break;
            }
            float top = 0;
            switch (positionData.AnchorY)
            {
                case TwailaConfig.VerticalAnchor.Top:
                    top = positionData.AnchorPosY;
                    break;
                case TwailaConfig.VerticalAnchor.Center:
                    top = positionData.AnchorPosY - Height.Pixels / 2;
                    break;
                case TwailaConfig.VerticalAnchor.Bottom:
                    top = positionData.AnchorPosY - Height.Pixels;
                    break;
            }
            Left.Set(MathHelper.Clamp(left, 0, Parent.GetDimensions().Width - Width.Pixels), 0);
            Top.Set(MathHelper.Clamp(top, 0, Parent.GetDimensions().Height - Height.Pixels), 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentPositionData.ShowUI)
                base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (TwailaConfig.Instance.ShowBackground)
            {
                base.DrawSelf(spriteBatch);
            }
        }

        private void UpdatePanelContents()
        {
            TwailaPoint mouseInfo = TwailaUI.GetCursorInfo();
            BaseContext context = null;
            BaseContext priorityContext = null;

            if (!TwailaUI.InBounds(mouseInfo.BestPos().X, mouseInfo.BestPos().Y))
            {
                tick = 0;
                return;
            }

            if(TwailaConfig.Instance.ContextMode == TwailaConfig.ContextUpdateMode.Manual)
            {
                context = GetManualContext(ref mouseInfo);
                priorityContext = PriorityContext;
                tick = 0;
            }
            else if(TwailaConfig.Instance.ContextMode == TwailaConfig.ContextUpdateMode.Automatic)
            {
                priorityContext = GetPriorityContext(ref mouseInfo);
                context = GetAutomaticContext(ref mouseInfo);

                if (tick >= TwailaConfig.Instance.CycleDelay)
                {
                    context = CycleContext(ref mouseInfo);
                }
            }

            if(context == null)
            {
                tick = 0;
                return;
            }

            Layout.InfoBox.RemoveAllChildren();
            UpdateCurrentContext(context);

            PriorityContext = priorityContext;
            CurrentContext = context;
        }

        private BaseContext GetManualContext(ref TwailaPoint mouseInfo)
        {
            return ContextSystem.Instance.CurrentContext(TwailaConfig.Instance.CurrentContext.Index, mouseInfo);
        }

        private BaseContext GetAutomaticContext(ref TwailaPoint mouseInfo)
        {
            Player player = Main.LocalPlayer;
            BaseContext context = ContextSystem.Instance.CurrentContext(TwailaConfig.Instance.CurrentContext.Index, mouseInfo) 
                ?? ContextSystem.Instance.NextNonNullContext(ref TwailaConfig.Instance.CurrentContext.Index, mouseInfo);

            if (player.itemAnimation > 0)
            {
                if (player.HeldItem.pick > 0) // swinging a pickaxe
                {
                    context = ContextSystem.Instance.TileEntry.Context(mouseInfo);
                }
                if (player.HeldItem.hammer > 0) // swinging a hammer
                {
                    context = ContextSystem.Instance.WallEntry.Context(mouseInfo);
                }
            }
            return context;
        }

        private BaseContext GetPriorityContext(ref TwailaPoint mouseInfo)
        {
            int first = 0;
            BaseContext priorityContext = ContextSystem.Instance.CurrentContext(0, mouseInfo) ?? ContextSystem.Instance.NextNonNullContext(ref first, mouseInfo);

            if (PriorityContext?.GetType() == priorityContext?.GetType())
            {
                tick++;
            }
            else
            {
                if(priorityContext != null)
                {
                    TwailaConfig.Instance.CurrentContext.SetIndex(first);
                }
                
                tick = 0;
            }

            return priorityContext;
        }

        private BaseContext CycleContext(ref TwailaPoint mouseInfo)
        {
            tick = 0;
            pickIndex++;
            return ContextSystem.Instance.NextNonNullContext(mouseInfo);
        }

        private void UpdateCurrentContext(BaseContext context)
        {
            if (context is TileContext tileContext)
            {
                tileContext.pickIndex = pickIndex;
                context = tileContext;
                context.UpdateOnChange(CurrentContext, Layout);
                if (context.ContextChanged(CurrentContext))
                {
                    tileContext.pickIndex = 0;
                }
                pickIndex = tileContext.pickIndex;
            }
            else
            {
                pickIndex = 0;
                context.UpdateOnChange(CurrentContext, Layout);
            }
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            var positionData = CurrentPositionData;
            _lastMouse = new Point(Main.mouseX, Main.mouseY);
            if (!positionData.LockPosition)
            {
                Main.LocalPlayer.mouseInterface = true;
                positionData.UseDefaultPosition = false;
                _dragging = true;
            }
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            _dragging = false;
        }

        public bool IsDragging()
        {
            return _dragging && !Main.ingameOptionsWindow && !Main.hideUI;
        }

        public void Drag()
        {
            if (IsDragging())
            {
                var positionData = CurrentPositionData;
                int deltaX = Main.mouseX - _lastMouse.X, deltaY = Main.mouseY - _lastMouse.Y;
                positionData.AnchorPosX += deltaX;
                positionData.AnchorPosY += deltaY;
                positionData.AnchorPosX = (int)MathHelper.Clamp(positionData.AnchorPosX, 0, Parent.GetDimensions().Width);
                positionData.AnchorPosY = (int)MathHelper.Clamp(positionData.AnchorPosY, 0, Parent.GetDimensions().Height);
                _lastMouse.X = Main.mouseX;
                _lastMouse.Y = Main.mouseY;
            }
        }
    }
}
