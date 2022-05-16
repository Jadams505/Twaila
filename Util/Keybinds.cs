using Terraria;
using Terraria.ModLoader;
using Twaila.UI;
using Microsoft.Xna.Framework.Input;

namespace Twaila.Util
{
    public class Keybinds
    {
        private static ModKeybind toggleUI;
        private static ModKeybind pauseCycling;
        //private static ModKeybind info;

        public static void RegisterKeybinds(Mod mod)
        {
            toggleUI = KeybindLoader.RegisterKeybind(mod, "Cycle UI Display Mode", "Mouse3");
            pauseCycling = KeybindLoader.RegisterKeybind(mod, "Pause Cycling", Keys.F);
            //info = KeybindLoader.RegisterKeybind(mod, "info", "*");
        }

        public static void Unload()
        {
            toggleUI = null;
            pauseCycling = null;
            //info = null;
        }

        public static void HandleKeys(TwailaPlayer player)
        {
            if (toggleUI.JustPressed)
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
            if (pauseCycling.Current)
            {
                player.CyclingPaused = true;
            }
            else
            {
                player.CyclingPaused = false;
            }
            /*
            if (info.JustPressed)
            {
                Main.NewText(Framing.GetTileSafely(TwailaUI.GetMousePos()));
            }
            */
            
        }
    }
}
