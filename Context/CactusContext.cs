using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Terraria.ModLoader;
using Twaila.Graphics;

namespace Twaila.Context
{
    public class CactusContext : TileContext
    {
        public int CactusSand { get; private set; }

        public CactusContext(Point pos) : base(pos)
        {
           CactusSand = GetCactusSand();
        }

        public override bool ContextChanged(TileContext other)
        {
            if (other is CactusContext otherCactusContext)
            {
                if (CactusSand == otherCactusContext.CactusSand)
                {
                    return false;
                }
            }
            return true;
        }

        public override TwailaTexture GetTileImage(SpriteBatch spriteBatch)
        {
            if (Tile.type == TileID.Cactus)
            {
                if (TileLoader.CanGrowModCactus(CactusSand))
                {
                    return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, CactusSand, true));
                }
                return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, CactusSand, false));
            }
            return null;
        }

        public override TwailaTexture GetItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return GetTileImage(spriteBatch);
        }

        public override string GetName(int itemId)
        {
            return NameUtil.GetNameForCactus(this) ?? base.GetName(itemId);
        }

        private int GetCactusSand()
        {
            if (Tile.type != TileID.Cactus)
            {
                return -1;
            }
            int x = Pos.X, y = Pos.Y;
            do
            {
                if (Main.tile[x, y + 1].type == TileID.Cactus)
                {
                    y++;
                }
                else if (Main.tile[x + 1, y].type == TileID.Cactus)
                {
                    x++;
                }
                else if (Main.tile[x - 1, y].type == TileID.Cactus)
                {
                    x--;
                }
                else
                {
                    y++;
                }
            }
            while (Main.tile[x, y].type == TileID.Cactus && Main.tile[x, y].active());
            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }
    }
}
