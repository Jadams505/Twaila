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

        public override bool ContentChanged(TileContext other)
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

        protected override TwailaTexture GetTileImage(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.TileType == TileID.Cactus)
            {
                if (TileLoader.CanGrowModCactus(CactusSand))
                {
                    return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, CactusSand, true));
                }
                return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, CactusSand, false));
            }
            return null;
        }

        protected override TwailaTexture GetTileItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return null;
        }

        protected override string GetTileName(Tile tile, int itemId)
        {
            return NameUtil.GetNameForCactus(this) ?? base.GetTileName(tile, itemId);
        }

        private int GetCactusSand()
        {
            if (Tile.TileId != TileID.Cactus)
            {
                return -1;
            }
            int x = Pos.X, y = Pos.Y;
            do
            {
                if (Main.tile[x, y + 1].TileType == TileID.Cactus)
                {
                    y++;
                }
                else if (Main.tile[x + 1, y].TileType == TileID.Cactus)
                {
                    x++;
                }
                else if (Main.tile[x - 1, y].TileType == TileID.Cactus)
                {
                    x--;
                }
                else
                {
                    y++;
                }
            }
            while (Main.tile[x, y].TileType == TileID.Cactus && Main.tile[x, y].HasTile);
            if (!Main.tile[x, y].HasTile)
            {
                return -1;
            }
            return Main.tile[x, y].TileType;
        }
    }
}
