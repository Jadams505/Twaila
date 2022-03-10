using Terraria.ModLoader;
using Twaila.UI;

namespace Twaila.Util
{
    public class Keybinds
    {
        public static ModHotKey toggleUI;
        public static ModHotKey toggleDebugTextures;
        //public static ModHotKey toggleItemTextures;

        public static void RegisterKeybinds(Mod mod)
        {
            toggleUI = mod.RegisterHotKey("Toggle UI", "Mouse3");
            toggleDebugTextures = mod.RegisterHotKey("Debug", "O");
            //toggleItemTextures = mod.RegisterHotKey("Toggle Item Textures", "I");

        }

        public static void Unload()
        {
            toggleUI = null;
            toggleDebugTextures = null;
            //toggleItemTextures = null;
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
            /*
            if (toggleItemTextures.JustPressed)
            {
                TwailaConfig.Get().UseItemTextures ^= true;
                TwailaUI.UpdateUI(forced: true);
            }
            */
        }
    }
}
