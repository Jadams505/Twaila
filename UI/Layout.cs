﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Twaila.Config;
using Twaila.Util;

namespace Twaila.UI
{
    public class Layout
    {
        public UITwailaImage Image { get; private set; }

        public UITwailaText Name { get; private set; }

        public UITwailaGrid InfoBox { get; private set; }

        public UITwailaText Mod { get; private set; }

        public List<UITwailaElement> TextElements { get; private set; }

        public Layout()
        {
            Image = new UITwailaImage();
            //string localized = Language.GetTextValue("Mods.Twaila.Defaults.Name"); fix later
            Name = new UITwailaText("Default Name", FontAssets.CombatText[0].Value, Color.White, 1f);
            InfoBox = new UITwailaGrid(width: 1)
            {
                SmartRows = false,
            };
            Mod = new UITwailaText("Terraria", FontAssets.ItemStack.Value, Color.White, 1f);

            TextElements = new List<UITwailaElement>()
            {
                Mod,
                Name,
                InfoBox,
            };
        }

        public void Apply(UIElement element)
        {
            element.Append(Image);
            element.Append(InfoBox);
            element.Append(Mod);
            element.Append(Name);
        }

        public void ApplyConfigSettings(TwailaConfig config)
        {
            Image.ApplyConfigSettings(config);
            InfoBox.ApplyConfigSettings(config);
            Name.ApplyConfigSettings(config);
            Mod.ApplyConfigSettings(config);
        }

        public void ApplyHoverSettings(TwailaConfig config)
        {
            Image.ApplyHoverSettings(config);
            InfoBox.ApplyHoverSettings(config);
            Name.ApplyHoverSettings(config);
            Mod.ApplyHoverSettings(config);
        }

        public void SetInitialSizes()
        {
            var contentSize = Name.GetContentSize();
            Name.Width.Set(contentSize.X, 0);
            Name.Height.Set(contentSize.Y, 0);

            contentSize = InfoBox.GetContentSize();
            InfoBox.Width.Set(contentSize.X, 0);
            InfoBox.Height.Set(contentSize.Y, 0);

            contentSize = Mod.GetContentSize();
            Mod.Width.Set(contentSize.X, 0);
            Mod.Height.Set(contentSize.Y, 0);

            contentSize = Image.GetContentSize();
            Image.Width.Set(contentSize.X, 0);
            Image.Height.Set(contentSize.Y, 0);
            Image.MarginRight = 10;
        }

        public Vector2 TextColumnSize()
        {
            Vector2 nameDim = Name.GetSizeIfAppended();
            Vector2 infoDim = InfoBox.GetSizeIfAppended();
            Vector2 modDim = Mod.GetSizeIfAppended();

            float width = Math.Max(nameDim.X, Math.Max(infoDim.X, modDim.X));
            return new Vector2(width, nameDim.Y + infoDim.Y + modDim.Y);
        }

        public void UpdateTextColumnVertically()
        {
            Name.Top.Set(0, 0);

            var infoHeight = InfoBox.GetSizeIfAppended().Y;
            var nameHeight = Name.GetSizeIfAppended().Y;
            InfoBox.Top.Set(nameHeight, 0);
            Mod.Top.Set(infoHeight + nameHeight, 0);
        }
    }
}
