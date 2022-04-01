using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.Graphics;

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

        protected override TwailaTexture GetTileImage(SpriteBatch spriteBatch)
        {
            return new TwailaTexture(ImageUtil.GetImageFromTileData(spriteBatch, Tile));
        }

        protected override TwailaTexture GetTileItemImage(SpriteBatch spriteBatch, int itemId)
        {
            Texture2D texture = ImageUtil.GetImageFromItemData(Tile, itemId) ?? GetTileImage(spriteBatch).Texture;
            return new TwailaTexture(texture);
        }

        protected override string GetTileName(int itemId)
        {
            return NameUtil.GetNameForSapling(this) ?? base.GetTileName(itemId);
        }

        private int GetSaplingTile()
        {
            if (!TileLoader.IsSapling(Tile.type))
            {
                return -1;
            }
            int y = Pos.Y;
            do
            {
                y++;
            } while (TileLoader.IsSapling(Main.tile[Pos.X, y].type) && Main.tile[Pos.X, y].active());

            if (Main.tile[Pos.X, y] == null || !Main.tile[Pos.X, y].active())
            {
                return -1;
            }
            return Main.tile[Pos.X, y].type;
        }
    }
}
