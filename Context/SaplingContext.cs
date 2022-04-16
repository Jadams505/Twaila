using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.Graphics;
using Terraria.ID;

namespace Twaila.Context
{
    public class SaplingContext : TileContext
    {
        public int SaplingDirt { get; private set; }

        public SaplingContext(Point pos) : base(pos)
        {
            SaplingDirt = GetSaplingTile();
        }

        public override bool ContentChanged(TileContext other)
        {
            if (other is SaplingContext otherSaplingContext)
            {
                if (SaplingDirt == otherSaplingContext.SaplingDirt)
                {
                    return StyleChanged(other);
                }
            }
            return true;
        }

        protected override TwailaTexture GetTileImage(SpriteBatch spriteBatch, Tile tile)
        {
            return new TwailaTexture(ImageUtil.GetImageFromTileData(spriteBatch, tile));
        }

        protected override TwailaTexture GetTileItemImage(SpriteBatch spriteBatch, int itemId)
        {
            Texture2D texture = ImageUtil.GetItemTexture(itemId);
            return new TwailaTexture(texture);
        }

        protected override string GetTileName(Tile tile, int itemId)
        {
            return NameUtil.GetNameForSapling(this) ?? base.GetTileName(tile, itemId);
        }

        private int GetSaplingTile()
        {
            if (!TileID.Sets.TreeSapling[Tile.TileId])
            {
                return -1;
            }
            int y = Pos.Y;
            do
            {
                y++;
            } while (TileID.Sets.TreeSapling[Main.tile[Pos.X, y].TileType] && Main.tile[Pos.X, y].HasTile);

            if (!Main.tile[Pos.X, y].HasTile)
            {
                return -1;
            }
            return Main.tile[Pos.X, y].TileType;
        }
    }
}
