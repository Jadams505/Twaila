using Terraria.ModLoader;
using Twaila.ObjectData;
using Twaila.Util;

namespace Twaila
{
	public class Twaila : Mod
	{
        public static Twaila Instance => ModContent.GetInstance<Twaila>();

        public override void Load()
        {
            Keybinds.RegisterKeybinds(this);
            ExtraObjectData.Initialize();
        }

        public override void PostAddRecipes()
        {
            ItemUtil.Load(); // loaded at this point so that all items from all mods have been loaded
        }

        public override void Unload()
        {
            Keybinds.Unload();
            ExtraObjectData.Unload();
            ItemUtil.Unload();
        }
    }
}