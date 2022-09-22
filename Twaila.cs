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

        public override void Unload()
        {
            ExtraObjectData.Unload();
        }
    }
}