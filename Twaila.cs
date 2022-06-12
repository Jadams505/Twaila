using System.Collections.Generic;
using Terraria.ModLoader;
using Twaila.Context;
using Twaila.ObjectData;
using Twaila.Systems;
using Twaila.Util;

namespace Twaila
{
	public class Twaila : Mod
	{
        public static Twaila Instance => ModContent.GetInstance<Twaila>();

        public override void Load()
        {
            ExtraObjectData.Initialize();
        }

        public override void PostAddRecipes()
        {
            ItemUtil.Load(); // loaded at this point so that all items from all mods have been loaded
        }

        public override void Unload()
        {
            ExtraObjectData.Unload();
            ItemUtil.Unload();
        }
    }
}