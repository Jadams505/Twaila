using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.UI;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel
    {
        public TwailaText Name, Mod;
        private List<TwailaText> _lines;
        public TwailaImage Image;        

        public TwailaPanel()
        {
            _lines = new List<TwailaText>();
            Name = new TwailaText("Unknown Name");
            
            Image = new TwailaImage();
            Image.VAlign = 0.5f;
            Image.MarginRight = 10;
            Mod = new TwailaText("Terraria", Main.fontItemStack, Color.White, 0.8f);
            
            
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
            float height = GetHeight(Image) > GetHeight(Mod) + GetHeight(Name) ? GetHeight(Image) : GetHeight(Mod) + GetHeight(Name);
            Height.Set(height + PaddingLeft + PaddingRight, 0);
            float width = GetWidth(Name) > GetWidth(Mod) ? GetWidth(Name) : GetWidth(Mod);
            Width.Set(width + GetWidth(Image) + Image.MarginRight + PaddingLeft + PaddingRight, 0);

            Name.Top.Set(Top.Pixels, 0);
            Name.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
            Mod.Top.Set(Name.Top.Pixels + GetHeight(Name), 0);
            Mod.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
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
