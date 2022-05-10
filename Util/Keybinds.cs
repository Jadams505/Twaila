using Terraria;
using Terraria.ModLoader;
using Twaila.UI;

namespace Twaila.Util
{
    public class Keybinds
    {
        public static ModHotKey toggleUI;
        public static ModHotKey toggleDebugTextures;
        public static ModHotKey info;

        public static void RegisterKeybinds(Mod mod)
        {
            toggleUI = mod.RegisterHotKey("Cycle UI Display Mode", "Mouse3");
            toggleDebugTextures = mod.RegisterHotKey("Debug", "O");
            info = mod.RegisterHotKey("info", "*");
        }

        public static void Unload()
        {
            toggleUI = null;
            toggleDebugTextures = null;
            info = null;
        }

        public static void HandleKeys()
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
            if (toggleDebugTextures.JustPressed)
            {
                TwailaUI.debugMode ^= true;
            }
            
            if (info.JustPressed)
            {
                Main.NewText(Main.tile[Player.tileTargetX, Player.tileTargetY]);
            }
            
        }
    }
}
