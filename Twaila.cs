using Terraria.ModLoader;
using Twaila.ObjectData;

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