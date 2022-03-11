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
        internal Vector2 anchorPos;
        private Point lastMouse;

        public TwailaPanel()
        {
            _context = new TileContext();
            Name = new TwailaText("Default Name", Main.fontCombatText[0], Color.White, 1f);
            anchorPos = Vector2.Zero;
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
            Height.Set(imageHeight > textHeight ? imageHeight + PaddingTop + PaddingBottom : textHeight + PaddingTop + PaddingBottom, 0);
            float imageWidth = Image.Width.Pixels + Image.MarginRight;
            float textWidth = Name.Width.Pixels > Mod.Width.Pixels ? Name.Width.Pixels : Mod.Width.Pixels;
            Width.Set(textWidth + imageWidth + PaddingLeft + PaddingRight, 0);
        }

        private void UpdateAlignment()
        {
            if (TwailaConfig.Get().UseDefaultPosition)
            {
                anchorPos.X = Parent.GetDimensions().Width / 2;
                anchorPos.Y = 0;
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
            switch (TwailaConfig.Get().DrawAnchor)
            {
                case TwailaConfig.Anchor.Left:
                    left = anchorPos.X;
                    break;
                case TwailaConfig.Anchor.Center:
                    left = anchorPos.X - Width.Pixels / 2;
                    break;
                case TwailaConfig.Anchor.Right:
                    left = anchorPos.X - Width.Pixels;
                    break;
            }
            Left.Set(MathHelper.Clamp(left, 0, Parent.GetDimensions().Width - Width.Pixels), 0);
            Top.Set(MathHelper.Clamp(anchorPos.Y, 0, Parent.GetDimensions().Height - Height.Pixels), 0);
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
                anchorPos.X += deltaX;
                anchorPos.Y += deltaY;
                anchorPos.X = MathHelper.Clamp(anchorPos.X, 0, Parent.GetDimensions().Width);
                anchorPos.Y = MathHelper.Clamp(anchorPos.Y, 0, Parent.GetDimensions().Height);
                lastMouse = new Point(Main.mouseX, Main.mouseY);
            }
        }
    }
}
