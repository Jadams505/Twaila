using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.ObjectData;
using Terraria.ObjectData;
using Twaila.Graphics;

namespace Twaila.Context
{
    public class TileContext
    {
        public Tile Tile { get; private set; }
        public Point Pos { get; private set; }

        public TileContext(Point pos)
        {
            Pos = pos;
            Tile = GetTileCopy();
        }

        public TileContext()
        {
            Pos = Point.Zero;
            Tile = new Tile();
        }

        private Tile GetTileCopy()
        {
            Tile copy = new Tile();
            copy.CopyFrom(Framing.GetTileSafely(Pos.X, Pos.Y));
            return copy;
        }

        public virtual bool ContextChanged(TileContext other)
        {
            if (other is TileContext otherTileContext)
            {
                if (Tile.type == otherTileContext.Tile.type)
                {
                    return StyleChanged(other);
                }
            }
            return true;
        }

        public virtual bool StyleChanged(TileContext other)
        {
            TileObjectData oldData = ExtraObjectData.GetData(other.Tile.type), newData = ExtraObjectData.GetData(Tile.type);
            if (newData == null)
            {
                oldData = TileObjectData.GetTileData(other.Tile);
                newData = TileObjectData.GetTileData(Tile);
            }
            if (newData == null || oldData == null)
            {
                return false;
            }
            int oldRow = other.Tile.frameX / oldData.CoordinateFullWidth;
            int oldCol = other.Tile.frameY / oldData.CoordinateFullHeight;
            int newRow = Tile.frameX / newData.CoordinateFullWidth;
            int newCol = Tile.frameY / newData.CoordinateFullHeight;

            return oldRow != newRow || oldCol != newCol;
        }

        public virtual string GetName(int itemId)
        {
            return NameUtil.GetNameForManualTiles(Tile) ?? NameUtil.GetNameForChest(Tile) ?? NameUtil.GetNameFromItem(itemId) 
                ?? NameUtil.GetNameFromMap(Pos) ?? "Default Name";
        }

        public virtual TwailaTexture GetTileImage(SpriteBatch spriteBatch)
        {
            Texture2D texture = ImageUtil.GetImageCustom(spriteBatch, Tile) ?? ImageUtil.GetImageFromTileData(spriteBatch, Tile)
                ?? ImageUtil.GetImageFromTile(spriteBatch, Tile);
            return new TwailaTexture(texture);
        }

        public virtual TwailaTexture GetItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return new TwailaTexture(ImageUtil.GetImageFromItemData(Tile, itemId));
        }

        public virtual string GetMod()
        {
            return NameUtil.GetModName(Tile) ?? "Default Mod";
        }
    }
}
