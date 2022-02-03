using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;

namespace Twaila.Util
{
    public class Keybinds
    {
        public static ModHotKey toggleUI;

        public static void RegisterKeybinds(Mod mod)
        {
            toggleUI = mod.RegisterHotKey("Toggle UI", "P");
        }

        public static void Unload()
        {
            toggleUI = null;
        }
    }
}
