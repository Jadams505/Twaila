using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using Twaila.Context;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel, IDragable
    {
        public TwailaText Name, Mod;
        public UITwailaImage Image;
        private TileContext _context;
        private bool dragging;
        private Point lastMouse;

        private Vector2 MaxPanelDimension => new Vector2(TwailaConfig.Get().MaxWidth / 100.0f * Parent.GetDimensions().Width, TwailaConfig.Get().MaxHeight / 100.0f * Parent.GetDimensions().Height);
        private Vector2 MaxPanelInnerDimension => new Vector2(MaxPanelDimension.X - PaddingLeft - PaddingRight, MaxPanelDimension.Y - PaddingTop - PaddingLeft);

        public TwailaPanel()
        {
            _context = new TileContext();
            Name = new TwailaText("Default Name", Main.fontCombatText[0], Color.White, 1f);
            Image = new UITwailaImage();
            Image.MarginRight = 10;
            Mod = new TwailaText("Terraria", Main.fontItemStack, Color.White, 1f);
            Width.Set(0, 0);
            Height.Set(0, 0);
            Top.Set(0, 0);
            Left.Set(PlayerInput.RealScreenWidth / 2, 0);

            Append(Name);
            Append(Mod);
            Append(Image);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.drawMode = TwailaConfig.Get().ContentSetting;
            Mod.drawMode = TwailaConfig.Get().ContentSetting;
            Name.drawMode = TwailaConfig.Get().ContentSetting;
            UpdateSize();
            Drag();
            UpdateAlignment();
        }

        private void UpdateSize()
        {
            SetPadding(TwailaConfig.Get().PanelPadding);
            float imageHeight = Image.image.Height();
            float textHeight = Mod.GetTextSize().Y + Name.GetTextSize().Y;
            if(imageHeight > MaxPanelInnerDimension.Y)
            {
                switch (TwailaConfig.Get().ContentSetting)
                {
                    case DrawMode.Shrink:
                        imageHeight *= ImageScale(new Vector2(TwailaConfig.Get().MaxImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));
                        break;
                    default:
                        imageHeight = MaxPanelInnerDimension.Y;
                        break;
                }
            }
            if (textHeight > MaxPanelInnerDimension.Y)
            {
                switch (TwailaConfig.Get().ContentSetting)
                {
                    case DrawMode.Shrink:
                        Vector2 maxSize = new Vector2(MaxPanelInnerDimension.X - (TwailaConfig.Get().MaxImageWidth / 100.0f * MaxPanelInnerDimension.X) - Image.MarginRight, MaxPanelInnerDimension.Y);
                        float nameHeight = Name.GetTextSize().Y * TextScale(Name, maxSize);
                        float modHeight = Mod.GetTextSize().Y * TextScale(Mod, maxSize);
                        textHeight = nameHeight + modHeight;
                        break;
                    default:
                        textHeight = MaxPanelInnerDimension.Y;
                        break;
                }
            }
            float calculatedHeight = imageHeight > textHeight ? imageHeight : textHeight;
            Height.Set(calculatedHeight + PaddingTop + PaddingBottom, 0);
            Image.Height.Set(Math.Max(imageHeight, textHeight), 0);
            
            float imageWidth = Image.image.Width();
            float textWidth = Name.GetTextSize().X > Mod.GetTextSize().X ? Name.GetTextSize().X : Mod.GetTextSize().X;
            if(imageWidth > TwailaConfig.Get().MaxImageWidth / 100.0f * MaxPanelInnerDimension.X || TwailaConfig.Get().ContentSetting == DrawMode.Shrink)
            {
                switch (TwailaConfig.Get().ContentSetting)
                {
                    case DrawMode.Shrink:
                        imageWidth *= ImageScale(new Vector2(TwailaConfig.Get().MaxImageWidth / 100.0f * MaxPanelInnerDimension.X, MaxPanelInnerDimension.Y));
                        break;
                    default:
                        imageWidth = TwailaConfig.Get().MaxImageWidth / 100.0f * MaxPanelInnerDimension.X;
                        break;
                }
            }
            if(textWidth > MaxPanelInnerDimension.X - imageWidth - Image.PaddingRight || TwailaConfig.Get().ContentSetting == DrawMode.Shrink)
            {
                switch (TwailaConfig.Get().ContentSetting)
                {
                    case DrawMode.Shrink:
                        Vector2 maxSize = new Vector2(MaxPanelInnerDimension.X - imageWidth - Image.MarginRight, MaxPanelInnerDimension.Y);
                        float nameWidth = Name.GetTextSize().X * TextScale(Name, maxSize);
                        float modWidth = Mod.GetTextSize().X * TextScale(Mod, maxSize);
                        Name.Width.Set(nameWidth, 0);
                        Mod.Width.Set(modWidth, 0);
                        textWidth = Math.Max(nameWidth, modWidth);
                        break;
                    default:
                        textWidth = MaxPanelInnerDimension.X - imageWidth - Image.PaddingRight;
                        Name.Width.Set(textWidth, 0);
                        Mod.Width.Set(textWidth, 0);
                        break;
                }
            }
            float calculatedWidth = textWidth + imageWidth + Image.MarginRight + PaddingLeft + PaddingRight;
            Width.Set(calculatedWidth, 0);
            Image.Width.Set(imageWidth, 0);
        }

        public float ImageScale(Vector2 maxSize)
        {
            float scaleX = 1;
            if (Image.image.Width() > maxSize.X)
            {
                scaleX = maxSize.X / Image.image.Width();
            }
            float scaleY = 1;
            if (Image.image.Height() > maxSize.Y)
            {
                scaleY = maxSize.Y / Image.image.Height();
            }
            return Math.Min(scaleX, scaleY) * Image.image.Scale;
        }

        public float TextScale(TwailaText text, Vector2 maxSize)
        {
            float scaleX = 1;
            if (text.GetTextSize().X > maxSize.X)
            {
                scaleX = maxSize.X / text.GetTextSize().X;
            }
            float scaleY = 1;
            if (text.GetTextSize().Y > maxSize.Y)
            {
                scaleY = maxSize.Y / text.GetTextSize().Y;
            }
            return Math.Min(scaleX, scaleY) * text.Scale;
        }
        
        private void UpdateAlignment()
        {
            if (TwailaConfig.Get().UseDefaultPosition)
            {
                TwailaConfig.Get().AnchorPosX = (int)Parent.GetDimensions().Width / 2;
                TwailaConfig.Get().AnchorPosY = 0;
            }
            UpdatePos();
            Image.Top.Set(0, 0);
            Name.Top.Set(0, 0);
            Name.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
            Mod.Top.Set(Name.Height.Pixels, 0);
            Mod.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
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

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
            if (!IsDragging())
            {
                UpdatePanelContents(spriteBatch);
            }    
        }

        private void UpdatePanelContents(SpriteBatch spriteBatch)
        {
            TileContext currentContext = TwailaUI.GetContext(TwailaUI.GetMousePos());
            if (currentContext.Tile.active() && currentContext.ContextChanged(_context))
            {
                int itemId = ItemUtil.GetItemId(currentContext.Tile);
                Name.SetText(currentContext.GetName(itemId));
                Mod.SetText(currentContext.GetMod());
                Image.SetImage(spriteBatch, currentContext, itemId);
                _context = currentContext;
            }
        }

        public override void MouseDown(UIMouseEvent evt)
        {           
            lastMouse = new Point(Main.mouseX, Main.mouseY);
            if (!TwailaConfig.Get().LockPosition)
            {
                Main.LocalPlayer.mouseInterface = true;
                TwailaConfig.Get().UseDefaultPosition = false;
                dragging = true;
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            dragging = false;
        }

        public bool IsDragging()
        {
            return dragging;
        }

        public void Drag()
        {
            if (IsDragging())
            {
                int deltaX = Main.mouseX - lastMouse.X, deltaY = Main.mouseY - lastMouse.Y;
                TwailaConfig.Get().AnchorPosX += deltaX;
                TwailaConfig.Get().AnchorPosY += deltaY;
                TwailaConfig.Get().AnchorPosX = (int)MathHelper.Clamp(TwailaConfig.Get().AnchorPosX, 0, Parent.GetDimensions().Width);
                TwailaConfig.Get().AnchorPosY = (int)MathHelper.Clamp(TwailaConfig.Get().AnchorPosY, 0, Parent.GetDimensions().Height);
                lastMouse.X = Main.mouseX;
                lastMouse.Y = Main.mouseY;
            }
        }
    }
}
