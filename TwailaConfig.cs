using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Twaila
{
    public class TwailaConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        public bool UseItemTextures;

        public static TwailaConfig Get()
        {
            return ModContent.GetInstance<TwailaConfig>();
        }
    }
}