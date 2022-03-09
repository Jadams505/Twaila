using Terraria.ModLoader;
using Twaila.UI;

namespace Twaila.Util
{
    public class Keybinds
    {
        public static ModHotKey toggleUI;
        //public static ModHotKey toggleItemTextures;
        //public static ModHotKey toggleDebugTextures;

        public static void RegisterKeybinds(Mod mod)
        {
            toggleUI = mod.RegisterHotKey("Toggle UI", "Mouse3");
            //toggleItemTextures = mod.RegisterHotKey("Toggle Item Textures", "I");
            //toggleDebugTextures = mod.RegisterHotKey("Debug", "O");
        }

        public static void Unload()
        {
            toggleUI = null;
            //toggleItemTextures = null;
            //toggleDebugTextures = null;
        }

        public static void HandleKeys()
        {
            if (toggleUI.JustPressed)
            {
                TwailaUI.ToggleVisibility(null);
            }
            /*
            if (toggleItemTextures.JustPressed)
            {
                TwailaConfig.Get().UseItemTextures ^= true;
                TwailaUI.UpdateUI(forced: true);
            }
            if (toggleDebugTextures.JustPressed)
            {
                TwailaUI.DebugUI();
            }
            */
        }
    }
}
