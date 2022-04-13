using Terraria.ModLoader;
using Twaila.ObjectData;
using Twaila.Util;

namespace Twaila
{
	public class Twaila : Mod
	{
        public override void Load()
        {
            Keybinds.RegisterKeybinds(this);
            ExtraObjectData.Initialize();
        }

        public override void Unload()
        {
            Keybinds.Unload();
            ExtraObjectData.Unload();
        }
    }
}