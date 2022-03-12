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

        public TwailaPanel()
        {
            _context = new TileContext();
            Name = new TwailaText("Default Name", Main.fontCombatText[0], Color.White, 1f);
            Image = new UITwailaImage();
            Image.VAlign = 0.5f;
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
            UpdateSize();
            Drag();
            UpdateAlignment();
        }

        private void UpdateSize()
        {
            float imageHeight = Image.Height.Pixels;
            float textHeight = Mod.Height.Pixels + Name.Height.Pixels;
            float calculatedHeight = imageHeight > textHeight ? imageHeight + PaddingTop + PaddingBottom : textHeight + PaddingTop + PaddingBottom;
            Height.Set(MathHelper.Clamp(calculatedHeight, 0, TwailaConfig.Get().MaxHeight / 100.0f * Parent.GetDimensions().Height), 0);
            float imageWidth = Image.Width.Pixels + Image.MarginRight;
            float textWidth = Name.Width.Pixels > Mod.Width.Pixels ? Name.Width.Pixels : Mod.Width.Pixels;
            float calculatedWidth = textWidth + imageWidth + PaddingLeft + PaddingRight;
            Width.Set(MathHelper.Clamp(calculatedWidth, 0, TwailaConfig.Get().MaxWidth / 100.0f * Parent.GetDimensions().Width), 0);
        }

        private void UpdateAlignment()
        {
            if (TwailaConfig.Get().UseDefaultPosition)
            {
                TwailaConfig.Get().AnchorPosX = (int)Parent.GetDimensions().Width / 2;
                TwailaConfig.Get().AnchorPosY = 0;
            }
            UpdatePos();
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
