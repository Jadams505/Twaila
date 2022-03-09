using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Twaila.Context;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel
    {
        public TwailaText Name, Mod;
        public UITwailaImage Image;
        private bool debugMode;
        private TileContext _context;

        public TwailaPanel()
        {
            debugMode = false;
            _context = new TileContext();
            Name = new TwailaText("Default Name", Main.fontCombatText[0], Color.White, 1f);
            
            Image = new UITwailaImage();
            Image.VAlign = 0.5f;
            Image.MarginRight = 10;
            Mod = new TwailaText("Terraria", Main.fontItemStack, Color.White, 1f);
            
            Width.Set(0, 0);
            Height.Set(0, 0);
            HAlign = 0.5f;
            Top.Set(0, 0);

            Append(Name);
            Append(Mod);
            Append(Image);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float height = GetHeight(Image) > GetHeight(Mod) + GetHeight(Name) ? GetHeight(Image) : GetHeight(Mod) + GetHeight(Name);
            Height.Set(height + PaddingLeft + PaddingRight, 0);
            float width = GetWidth(Name) > GetWidth(Mod) ? GetWidth(Name) : GetWidth(Mod);
            Width.Set(width + GetWidth(Image) + Image.MarginRight + PaddingLeft + PaddingRight, 0);

            Name.Top.Set(Top.Pixels, 0);
            Name.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
            Mod.Top.Set(Name.Top.Pixels + GetHeight(Name), 0);
            Mod.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
            Recalculate();
        }
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
            UpdatePanelContents(spriteBatch);
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

        public void ToggleDebugMode()
        {
            debugMode ^= true;
        }

        private static float GetWidth(UIElement element)
        {
            return element.Width.Pixels;
        }

        private static float GetHeight(UIElement element)
        {
            return element.Height.Pixels;
        }
    }
}
