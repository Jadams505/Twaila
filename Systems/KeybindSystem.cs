using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Twaila.UI;

namespace Twaila.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind ToggleUI { get; private set; }
        public static ModKeybind PauseCycling { get; private set; }
        public static ModKeybind NextContext { get; private set; }


        public override void Load()
        {
            ToggleUI = KeybindLoader.RegisterKeybind(Mod, "Cycle UI Display Mode", "Mouse3");
            PauseCycling = KeybindLoader.RegisterKeybind(Mod, "Pause Cycling", Keys.F);
            NextContext = KeybindLoader.RegisterKeybind(Mod, "Next Context", Keys.Right);
        }

        public override void Unload()
        {
            ToggleUI = null;
            PauseCycling = null;
            NextContext = null;
        }

        public static void HandleKeys(TwailaPlayer player)
        {
            if (ToggleUI.JustPressed)
            {
                switch (TwailaConfig.Get().UIDisplaySettings.UIDisplay)
                {
                    case TwailaConfig.DisplayMode.On:
                        TwailaConfig.Get().UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.Off;
                        break;
                    case TwailaConfig.DisplayMode.Off:
                        TwailaConfig.Get().UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.Automatic;
                        break;
                    case TwailaConfig.DisplayMode.Automatic:
                        TwailaConfig.Get().UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.On;
                        break;
                }
                Main.NewText("Display Mode: " + TwailaConfig.Get().UIDisplaySettings.UIDisplay);
            }
            if (PauseCycling.Current)
            {
                player.CyclingPaused = true;
            }
            else
            {
                player.CyclingPaused = false;
            }
            if (NextContext.JustPressed)
            {
                TwailaUI.NextContext();
            }
        }
    }
}
