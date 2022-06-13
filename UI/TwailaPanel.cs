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

            Layout.UpdateFromConfig();

            BackgroundColor = config.PanelColor.Color;
            BorderColor = Color.Black;
            if (IsMouseHovering && !IsDragging())
            {
                BackgroundColor *= config.HoverOpacity;
                BorderColor *= config.HoverOpacity;
                Layout.Image.Opacity = config.HoverOpacity;
                Layout.InfoBox.ApplyToAll(HoverSettings);
                HoverSettings(Layout.Name);
                HoverSettings(Layout.Mod);
            }
        }

        private void HoverSettings(UITwailaElement element)
        {
            TwailaConfig config = TwailaConfig.Get();
            element.Opacity = config.HoverOpacity;
            if (element is TwailaText text)
            {
                text.OverrideTextColor = true;
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
            float imageHeight = GetDimension(Layout.Image, Layout.Image.Height.Pixels);
            float textHeight = Layout.TextColumnSize().Y;
            float imageWidth = GetDimension(Layout.Image, Layout.Image.image.Width);
            float textWidth = Layout.TextColumnSize().X;

            if (Layout.InfoBox.IsEmpty() && !HasChild(Layout.Name) && !HasChild(Layout.Mod))
            {
                Layout.Image.MarginRight = 0;
            }
            DrawMode drawMode = TwailaConfig.Get().ContentSetting;
            if (drawMode == DrawMode.Shrink)
            {
                imageWidth *= ImageScale(new Vector2(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));

                Vector2 maxSize = new Vector2(MaxPanelInnerDimension.X - imageWidth - GetDimension(Layout.Image, Layout.Image.MarginRight), MaxPanelInnerDimension.Y / (Layout.InfoBox.NumberOfAppendedElements() + 2));

                ScaleElement(Layout.Name, maxSize);
                Layout.InfoBox.ApplyToAll(element => ScaleElement(element, maxSize));
                Layout.InfoBox.UpdateDimensionsUI();
                ScaleElement(Layout.Mod, maxSize);

                textWidth = Layout.TextColumnSize().X;
                textHeight = Layout.TextColumnSize().Y;

                Vector2 remainingSpace = new Vector2(MaxPanelInnerDimension.X - textWidth - GetDimension(Layout.Image, Layout.Image.MarginRight), MaxPanelInnerDimension.Y);

                imageWidth = GetDimension(Layout.Image, Layout.Image.image.Width) * ImageScale(remainingSpace);
                imageHeight = GetDimension(Layout.Image, Layout.Image.image.Height) * ImageScale(remainingSpace);
            }
            else
            {
                if (drawMode == DrawMode.Trim)
                {
                    float height = 0;
                    if (Layout.Name.GetContentSize().Y + height < MaxPanelInnerDimension.Y)
                    {
                        height += Layout.Name.GetContentSize().Y;
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
                            if (element.GetContentSize().Y + height < MaxPanelInnerDimension.Y)
                            {
                                height += element.GetContentSize().Y;
                            }
                            else
                            {
                                Layout.InfoBox.RemoveElement(i);
                                i--;
                            }
                        }
                    }
                    if (Layout.Mod.GetContentSize().Y + height < MaxPanelInnerDimension.Y)
                    {
                        height += Layout.Mod.GetContentSize().Y;
                    }
                    else
                    {
                        RemoveChild(Layout.Mod);
                    }
                    textHeight = height;
                }
                imageHeight = Math.Min(MaxPanelInnerDimension.Y, imageHeight);
                textHeight = Math.Min(MaxPanelInnerDimension.Y, textHeight);
                imageWidth = Math.Min(TwailaConfig.Get().ReservedImageWidth / 100.0f * MaxPanelInnerDimension.X, imageWidth);
                textWidth = Math.Min(MaxPanelInnerDimension.X - imageWidth - GetDimension(Layout.Image, Layout.Image.MarginRight), textWidth);

                Layout.Name.Width.Set(textWidth, 0);
                Layout.InfoBox.ApplyToAll(element => element.Width.Set(textWidth, 0));
                Layout.InfoBox.UpdateDimensionsUI();
                Layout.Mod.Width.Set(textWidth, 0);


                Vector2 remainingSpace = new Vector2(MaxPanelInnerDimension.X - textWidth - GetDimension(Layout.Image, Layout.Image.MarginRight), MaxPanelInnerDimension.Y);

                imageWidth = MathHelper.Clamp(GetDimension(Layout.Image, Layout.Image.image.Width), 0, remainingSpace.X);
                imageHeight = MathHelper.Clamp(GetDimension(Layout.Image, Layout.Image.image.Height), 0, remainingSpace.Y);
            }

            float calculatedHeight = imageHeight > textHeight ? imageHeight : textHeight;
            Height.Set(calculatedHeight + PaddingTop + PaddingBottom, 0);
            Layout.Image.Height.Set(Math.Max(imageHeight, textHeight), 0);
            
            float calculatedWidth = textWidth + imageWidth + GetDimension(Layout.Image, Layout.Image.MarginRight) + PaddingLeft + PaddingRight;
            Width.Set(calculatedWidth, 0);
            Layout.Image.Width.Set(imageWidth, 0);
        }

        private void ScaleElement(UITwailaElement element, Vector2 maxSize)
        {
            float scale = element.GetScale(maxSize);
            float width = element.GetContentSize().X * scale;
            float height = element.GetContentSize().Y * scale;

            element.Width.Set(width, 0);
            element.Height.Set(height, 0);
            element.Recalculate();
        }

        public float ImageScale(Vector2 maxSize)
        {
            float scaleX = 1;
            if (GetDimension(Layout.Image, Layout.Image.image.Width) > maxSize.X)
            {
                scaleX = maxSize.X / GetDimension(Layout.Image, Layout.Image.image.Width);
            }
            float scaleY = 1;
            if (GetDimension(Layout.Image, Layout.Image.image.Height) > maxSize.Y)
            {
                scaleY = maxSize.Y / GetDimension(Layout.Image, Layout.Image.image.Height);
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
            Point mousePos = TwailaUI.GetMousePos();
            Player player = Main.player[Main.myPlayer];

            if (!TwailaUI.InBounds(mousePos.X, mousePos.Y))
            {
                tick = 0;
                return;
            }

            BaseContext context = ContextSystem.Instance.CurrentContext(currIndex, mousePos) ??
                ContextSystem.Instance.NextContext(ref currIndex, mousePos);
            if (player.itemAnimation > 0)
            {
                if (player.HeldItem.pick > 0) // swinging a pickaxe
                {
                    context = ContextSystem.Instance.TileEntry.Context(mousePos);
                }
                if (player.HeldItem.hammer > 0) // swinging a hammer
                {
                    context = ContextSystem.Instance.WallEntry.Context(mousePos);
                }
            }
            
            if (context == null)
            {
                tick = 0;
                return;
            }
            
            player.TryGetModPlayer(out TwailaPlayer tPlayer);
            
            if (tick >= TwailaConfig.Get().CycleDelay && !tPlayer.CyclingPaused)
            {
                context = ContextSystem.Instance.NextContext(ref currIndex, mousePos);
                pickIndex++;
                tick = 0;
            }

            if (!HasChild(Layout.Mod))
            {
                Append(Layout.Mod);
            }
            if (!HasChild(Layout.Name))
            {
                Append(Layout.Name);
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

        public override void MouseDown(UIMouseEvent evt)
        {   
            _lastMouse = new Point(Main.mouseX, Main.mouseY);
            if (!TwailaConfig.Get().LockPosition)
            {
                Main.LocalPlayer.mouseInterface = true;
                TwailaConfig.Get().UseDefaultPosition = false;
                _dragging = true;
            }
        }

        public override void MouseUp(UIMouseEvent evt)
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
