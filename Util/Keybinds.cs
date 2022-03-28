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
            toggleUI = mod.RegisterHotKey("Toggle UI", "Mouse3");
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
                TwailaUI.ToggleVisibility(null);
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
