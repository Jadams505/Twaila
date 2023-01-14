﻿using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;
using Terraria.UI;

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
            element.ApplyConfigSettings(config);
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
            Image.Width.Set(Image.Render.Width, 0);
            Image.Height.Set(Image.Render.Height, 0);
            Image.MarginRight = 10;
        }

        public Vector2 TextColumnSize()
        {
            Vector2 nameDim = GetDimension(Name);
            Vector2 infoDim = GetDimension(InfoBox);
            Vector2 modDim = GetDimension(Mod);

            float width = Math.Max(nameDim.X, Math.Max(infoDim.X, modDim.X));
            return new Vector2(width, nameDim.Y + infoDim.Y + modDim.Y);
        }

        public void UpdateTextColumnVertically()
        {
            Name.Top.Set(0, 0);
            InfoBox.UpdateVertically();
            InfoBox.Top.Set(GetDimension(Name).Y, 0);
            Mod.Top.Set(InfoBox.Height.Pixels + GetDimension(Name).Y, 0);
        }

        private Vector2 GetDimension(UIElement element)
        {
            if (element.Parent != null && element.Parent.HasChild(element))
            {
                return new Vector2(element.Width.Pixels, element.Height.Pixels);
            }
            return Vector2.Zero;
        }
    }
}
