using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.UI;
using Twaila.Graphics;

namespace Twaila.UI
{
    public class Layout
    {
        public UITwailaImage Image { get; private set; }

        public TwailaText Name { get; private set; }

        public UIInfoBox InfoBox { get; private set; }

        public TwailaText Mod { get; private set; }

        public Layout()
        {
            Image = new UITwailaImage();
            Name = new TwailaText("Default Name", FontAssets.CombatText[0].Value, Color.White, 1f);
            InfoBox = new UIInfoBox();
            Mod = new TwailaText("Terraria", FontAssets.ItemStack.Value, Color.White, 1f);
        }

        public void Apply(UIElement element)
        {
            element.Append(Image);
            element.Append(InfoBox);
            element.Append(Mod);
            element.Append(Name);
        }

        public void UpdateImage(TwailaTexture image)
        {
            Image.SetImage(image);
        }

        public void UpdateFromConfig()
        {
            TwailaConfig config = TwailaConfig.Get();

            Image.DrawMode = config.ContentSetting;
            Image.Opacity = 1;
            InfoBox.ApplyToAll(ApplyConfig);
            ApplyConfig(Name);
            ApplyConfig(Mod);
        }

        private void ApplyConfig(UITwailaElement element)
        {
            TwailaConfig config = TwailaConfig.Get();
            element.DrawMode = config.ContentSetting;
            element.Opacity = 1;
            if (element is TwailaText text)
            {
                text.OverrideTextColor = config.OverrideColor;
                text.Color = config.TextColor.Color;
                text.TextShadow = config.TextShadow;
            }
        }

        public void SetInitialSizes()
        {
            Name.Width.Set(Name.GetContentSize().X, 0);
            Name.Height.Set(Name.GetContentSize().Y, 0);
            InfoBox.ApplyToAll((element) =>
            {
                element.Width.Set(element.GetContentSize().X, 0);
                element.Height.Set(element.GetContentSize().Y, 0);
            });
            Mod.Width.Set(Mod.GetContentSize().X, 0);
            Mod.Height.Set(Mod.GetContentSize().Y, 0);

            InfoBox.UpdateDimensions();
            Image.Width.Set(Image.image.Width, 0);
            Image.Height.Set(Image.image.Height, 0);
            Image.MarginRight = 10;
        }

        public Vector2 TextColumnSize()
        {
            float width = Math.Max(Name.Width.Pixels, Math.Max(InfoBox.Width.Pixels, Mod.Width.Pixels));
            return new Vector2(width, Name.Height.Pixels + InfoBox.Height.Pixels + Mod.Height.Pixels);
        }

        public void UpdateTextColumnVertically()
        {
            Name.Top.Set(0, 0);
            InfoBox.UpdateVertically();
            InfoBox.Top.Set(Name.Height.Pixels, 0);
            Mod.Top.Set(InfoBox.Height.Pixels + Name.Height.Pixels, 0);
        }
    }
}
