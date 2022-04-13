﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Twaila.UI
{
    public class ModInterface : ModSystem
    {
        private static GameTime _lastUpdateTime;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                TwailaUI.Load();
            }
        }

        public override void Unload()
        {
            TwailaUI.Unload();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateTime = gameTime;
            TwailaUI.Update(gameTime);
        }

        private static bool DrawGUI()
        {
            TwailaUI.Draw(_lastUpdateTime);
            return true;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int InventoryLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (InventoryLayer != -1)
            {
                layers.Insert(InventoryLayer, new LegacyGameInterfaceLayer(
                    "Twaila: Panel Layer", DrawGUI, InterfaceScaleType.UI));
            }
        }
    }
}
