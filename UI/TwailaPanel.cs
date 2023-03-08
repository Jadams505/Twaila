using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Twaila.Context;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel, IDragable
    {
        public Layout Layout { get; set; }
        public BaseContext CurrentContext { get; set; }
        public int currIndex = 0;
        public int tick = 0;

        private int pickIndex = 0;
        private bool _dragging;
        private Point _lastMouse;
        

        private Vector2 MaxPanelDimension => new Vector2(TwailaConfig.Get().MaxWidth / 100.0f * Parent.GetDimensions().Width, TwailaConfig.Get().MaxHeight / 100.0f * Parent.GetDimensions().Height);
        private Vector2 MaxPanelInnerDimension => new Vector2(MaxPanelDimension.X - PaddingLeft - PaddingRight, MaxPanelDimension.Y - PaddingTop - PaddingLeft);

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
            base.Update(gameTime);
            UpdateFromConfig();
            UpdateSize();
            Drag();
            UpdateAlignment();
        }

        private void UpdateFromConfig()
        {
            TwailaConfig config = TwailaConfig.Get();

            AppendOrRemove(Layout.Image, config.DisplayContent.ShowImage);
            AppendOrRemove(Layout.Mod, config.DisplayContent.ShowMod);
            AppendOrRemove(Layout.Name, config.DisplayContent.ShowName != TwailaConfig.NameType.Off);

            Layout.ApplyConfigSettings(config);

            BackgroundColor = config.PanelColor.Color;
            BorderColor = Color.Black;
            if (ContainsPoint(Main.mouseX, Main.mouseY) && !IsDragging())
            {
                BackgroundColor *= config.HoverOpacity;
                BorderColor *= config.HoverOpacity;

				Layout.ApplyHoverSettings(config);
            }
        }

        private void AppendOrRemove(UIElement element, bool append)
        {
            if (append)
            {
                if (!HasChild(element))
                {
                    Append(element);
                }
            }
            else if (HasChild(element))
            {
                RemoveChild(element);
            }
        }

        private float GetDimension(UIElement childElement, float dimension)
        {
            return HasChild(childElement) ? dimension : 0;
        }

        private void UpdateSize()
        {
            SetPadding(TwailaConfig.Get().PanelPadding);
            Layout.SetInitialSizes();
            if (Layout.InfoBox.IsEmpty() && !HasChild(Layout.Name) && !HasChild(Layout.Mod))
            {
                Layout.Image.MarginRight = 0;
            }

            Vector2 imageDimension = Layout.Image.GetSizeIfAppended();
            Vector2 textDimension = Layout.TextColumnSize();

            float imageMarginX = Layout.Image.GetSizeIfAppended(Layout.Image.MarginRight);

            DrawMode drawMode = TwailaConfig.Get().ContentSetting;
            if (drawMode == DrawMode.Shrink)
            {
                float reservedImageWidth = imageDimension.X * ImageScale(new Vector2(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));
                
                Vector2 maxSize = new Vector2(MaxPanelInnerDimension.X - reservedImageWidth - imageMarginX, MaxPanelInnerDimension.Y / (Layout.InfoBox.NumberOfAppendedElements() + 2));

                Layout.Name.ScaleElement(maxSize);
                Layout.InfoBox.ApplyToAll(element => element.ScaleElement(maxSize));
                Layout.InfoBox.UpdateDimensionsUI();
                Layout.Mod.ScaleElement(maxSize);

                textDimension.X = Layout.TextColumnSize().X;
                textDimension.Y = Layout.TextColumnSize().Y;

                Vector2 remainingSpace = new Vector2(MaxPanelInnerDimension.X - textDimension.X - imageMarginX, MaxPanelInnerDimension.Y);

                imageDimension.X *= ImageScale(remainingSpace);
                imageDimension.Y *= ImageScale(remainingSpace);
            }
            else
            {
                if (drawMode == DrawMode.Trim)
                {
                    float height = 0;
                    Vector2 nameSize = Layout.Name.GetSizeIfAppended();
                    if (nameSize.Y + height < MaxPanelInnerDimension.Y)
                    {
                        height += nameSize.Y;
                    }
                    else
                    {
                        RemoveChild(Layout.Name);
                    }
                    for (int i = 0; i < Layout.InfoBox.InfoLines.Count; ++i)
                    {
                        if (Layout.InfoBox.Enabled[i])
                        {
                            UITwailaElement element = Layout.InfoBox.InfoLines[i];
                            Vector2 elementSize = element.GetSizeIfAppended();
                            if (elementSize.Y + height < MaxPanelInnerDimension.Y)
                            {
                                height += elementSize.Y;
                            }
                            else
                            {
                                Layout.InfoBox.RemoveElement(i);
                                i--;
                            }
                        }
                    }
                    Vector2 modSize = Layout.Mod.GetSizeIfAppended();
                    if (modSize.Y + height < MaxPanelInnerDimension.Y)
                    {
                        height += modSize.Y;
                    }
                    else
                    {
                        RemoveChild(Layout.Mod);
                    }
                    textDimension.Y = height;
                }
                imageDimension.Y = Math.Min(MaxPanelInnerDimension.Y, imageDimension.Y);
                textDimension.Y = Math.Min(MaxPanelInnerDimension.Y, textDimension.Y);
                imageDimension.X = Math.Min(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, imageDimension.X);
                textDimension.X = Math.Min(MaxPanelInnerDimension.X - imageDimension.X - imageMarginX, textDimension.X);

                Layout.Name.Width.Set(textDimension.X, 0);
                Layout.InfoBox.ApplyToAll(element => element.Width.Set(textDimension.X, 0));
                Layout.InfoBox.UpdateDimensionsUI();
                Layout.Mod.Width.Set(textDimension.X, 0);

                if(imageDimension.X == 0 || imageDimension.Y == 0)
                {
                    imageDimension.X = 0;
                    imageDimension.Y = 0;
                }
            }

            float calculatedHeight = Math.Max(imageDimension.Y, textDimension.Y);
            Height.Set(calculatedHeight + PaddingTop + PaddingBottom, 0);
            Layout.Image.Height.Set(calculatedHeight, 0);
            
            float calculatedWidth = textDimension.X + imageDimension.X + imageMarginX + PaddingLeft + PaddingRight;
            Width.Set(calculatedWidth, 0);
            Layout.Image.Width.Set(imageDimension.X, 0);
        }

        public float ImageScale(Vector2 maxSize)
        {
            float scaleX = 1;
            if (GetDimension(Layout.Image, Layout.Image.Render.Width) > maxSize.X)
            {
                scaleX = maxSize.X / GetDimension(Layout.Image, Layout.Image.Render.Width);
            }
            float scaleY = 1;
            if (GetDimension(Layout.Image, Layout.Image.Render.Height) > maxSize.Y)
            {
                scaleY = maxSize.Y / GetDimension(Layout.Image, Layout.Image.Render.Height);
            }
            return Math.Min(scaleX, scaleY);
        }

        private void UpdateAlignment()
        {
            if (TwailaConfig.Get().UseDefaultPosition)
            {
                TwailaConfig.Get().AnchorPosX = (int)Parent.GetDimensions().Width / 2;
                TwailaConfig.Get().AnchorPosY = 0;
            }
            UpdatePos();
            Layout.Image.Top.Set(0, 0);
            float textColX = GetDimension(Layout.Image, Layout.Image.Width.Pixels + Layout.Image.MarginRight);
            Layout.Name.Left.Set(textColX, 0);
            Layout.InfoBox.Left.Set(textColX, 0);
            Layout.Mod.Left.Set(textColX, 0);

            Layout.UpdateTextColumnVertically();
            Recalculate();
        }

        private void UpdatePos()
        {
            float left = 0;
            switch (TwailaConfig.Get().AnchorX)
            {
                case TwailaConfig.HorizontalAnchor.Left:
                    left = TwailaConfig.Get().AnchorPosX;
                    break;
                case TwailaConfig.HorizontalAnchor.Center:
                    left = TwailaConfig.Get().AnchorPosX - Width.Pixels / 2;
                    break;
                case TwailaConfig.HorizontalAnchor.Right:
                    left = TwailaConfig.Get().AnchorPosX - Width.Pixels;
                    break;
            }
            float top = 0;
            switch (TwailaConfig.Get().AnchorY)
            {
                case TwailaConfig.VerticalAnchor.Top:
                    top = TwailaConfig.Get().AnchorPosY;
                    break;
                case TwailaConfig.VerticalAnchor.Center:
                    top = TwailaConfig.Get().AnchorPosY - Height.Pixels / 2;
                    break;
                case TwailaConfig.VerticalAnchor.Bottom:
                    top = TwailaConfig.Get().AnchorPosY - Height.Pixels;
                    break;
            }
            Left.Set(MathHelper.Clamp(left, 0, Parent.GetDimensions().Width - Width.Pixels), 0);
            Top.Set(MathHelper.Clamp(top, 0, Parent.GetDimensions().Height - Height.Pixels), 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (TwailaConfig.Get().ShowBackground)
            {
                base.DrawSelf(spriteBatch);
            }
        }
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
            if (!IsDragging() && !Main.gamePaused)
            {
                UpdatePanelContents(spriteBatch);
            }    
        }

        private void UpdatePanelContents(SpriteBatch spriteBatch)
        {
            tick++;
            TwailaPoint mouseInfo = TwailaUI.GetCursorInfo();
            Player player = Main.player[Main.myPlayer];

            if (!TwailaUI.InBounds(mouseInfo.BestPos().X, mouseInfo.BestPos().Y))
            {
                tick = 0;
                return;
            }

            BaseContext context = ContextSystem.Instance.CurrentContext(currIndex, mouseInfo);

            if (TwailaConfig.Get().ContextMode == TwailaConfig.ContextUpdateMode.Automatic)
            {
                context ??= ContextSystem.Instance.NextNonNullContext(ref currIndex, mouseInfo);

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
            }

            if (context == null)
            {
                tick = 0;
                return;
            }

            if (tick >= TwailaConfig.Get().CycleDelay)
            {
                if (TwailaConfig.Get().ContextMode == TwailaConfig.ContextUpdateMode.Automatic)
                {
                    context = ContextSystem.Instance.NextNonNullContext(ref currIndex, mouseInfo);
                }
                tick = 0;
                pickIndex++;
            }

            Layout.InfoBox.RemoveAll();
            if (context is TileContext tileContext)
            {
                if (context.ContextChanged(CurrentContext))
                {
                    pickIndex = 0;
                }
                tileContext.pickIndex = pickIndex;
                context = tileContext;
                context.UpdateOnChange(CurrentContext, Layout);
                pickIndex = tileContext.pickIndex;
            }
            else
            {
                pickIndex = 0;
                context.UpdateOnChange(CurrentContext, Layout);
            }
            CurrentContext = context;
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {   
            _lastMouse = new Point(Main.mouseX, Main.mouseY);
            if (!TwailaConfig.Get().LockPosition)
            {
                Main.LocalPlayer.mouseInterface = true;
                TwailaConfig.Get().UseDefaultPosition = false;
                _dragging = true;
            }
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            _dragging = false;
        }

        public bool IsDragging()
        {
            return _dragging && !Main.ingameOptionsWindow && !Main.hideUI && !Main.mapFullscreen;
        }

        public void Drag()
        {
            if (IsDragging())
            {
                int deltaX = Main.mouseX - _lastMouse.X, deltaY = Main.mouseY - _lastMouse.Y;
                TwailaConfig.Get().AnchorPosX += deltaX;
                TwailaConfig.Get().AnchorPosY += deltaY;
                TwailaConfig.Get().AnchorPosX = (int)MathHelper.Clamp(TwailaConfig.Get().AnchorPosX, 0, Parent.GetDimensions().Width);
                TwailaConfig.Get().AnchorPosY = (int)MathHelper.Clamp(TwailaConfig.Get().AnchorPosY, 0, Parent.GetDimensions().Height);
                _lastMouse.X = Main.mouseX;
                _lastMouse.Y = Main.mouseY;
            }
        }

        public bool ContainsPoint(int x, int y)
        {
            return GetDimensions().ToRectangle().Intersects(new Rectangle(x, y, 0, 0));
        }
    }
}
