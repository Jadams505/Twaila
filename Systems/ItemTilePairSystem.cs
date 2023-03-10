using Terraria.ModLoader;
using Twaila.Util;

namespace Twaila.Systems
{
    public class ItemTilePairSystem : ModSystem
    {
        public override void PostAddRecipes()
        {
            ItemUtil.Load(); // loaded at this point so that all items from all mods have been loaded
        }

        public override void Unload()
        {
            ItemUtil.Unload();
        }
    }
}
