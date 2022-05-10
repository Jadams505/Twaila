using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.ObjectData;
using Terraria.ObjectData;
using Twaila.Graphics;
using Terraria.GameContent.Drawing;
using Twaila.UI;
using Terraria.GameContent;

namespace Twaila.Context
{
    public enum TileType
    {
        Tile, Wall, Liquid, Empty
    }

    public class DummyTile
    {
        public int TileId { get; set; }
        public int WallId { get; set; }
        public int LiquidId { get; set; }
        public int LiquidAmount { get; set; }
        public int TileFrameX { get; set; }
        public int TileFrameY { get; set; }
        public bool RedWire { get; set; }
        public bool BlueWire { get; set; }
        public bool GreenWire { get; set; }
        public bool YellowWire { get; set; }
        public bool Actuator { get; set; }
        public bool HasTile { get; set; }

        public DummyTile(Tile tile)
        {
            TileId = tile.TileType;
            WallId = tile.WallType;
            LiquidId = tile.LiquidType;
            LiquidAmount = tile.LiquidAmount;
            TileFrameX = tile.TileFrameX;
            TileFrameY = tile.TileFrameY;
            HasTile = tile.HasTile;
            RedWire = tile.RedWire;
            BlueWire = tile.BlueWire;
            GreenWire = tile.GreenWire;
            YellowWire = tile.YellowWire;
            Actuator = tile.HasActuator;
        }

        public DummyTile()
        {
            TileId = 0;
            WallId = 0;
            LiquidId = 0;
            LiquidAmount = 0;
            TileFrameX = 0;
            TileFrameY = 0;
            HasTile = false;
            RedWire = false;
            BlueWire = false;
            GreenWire = false;
            YellowWire = false;
            Actuator = false;
        }
    }

    public class TileContext
    {
        public DummyTile Tile { get; private set; }

        public Point Pos { get; private set; }

        public TileType TileType { get; private set; }

        public TileContext(Point pos)
        {
            Pos = pos;
            Tile = GetTileCopy();
            if (HasTile() || OnlyWire())
            {
                TileType = TileType.Tile;
            }
            else if (HasWall())
            {
                TileType = TileType.Wall;
            }
            else if (HasLiquid())
            {
                TileType = TileType.Liquid;
            }
            else
            {
                TileType = TileType.Empty;
            }
        }

        public TileContext()
        {
            Pos = Point.Zero;
            Tile = new DummyTile();
            TileType = TileType.Empty;
        }

        private DummyTile GetTileCopy()
        {
            Tile tile = Framing.GetTileSafely(Pos.X, Pos.Y);
            return new DummyTile(tile);
        }

        public bool ContextChanged(TileContext other)
        {
            if (TypeChanged(other))
            {
                return true;
            }
            return ContentChanged(other);
        }

        public bool TypeChanged(TileContext other)
        {
            return TileType != other.TileType;
        }

        public string GetName(Tile tile, int itemId)
        {
            if (TileType == TileType.Tile)
            {
                return GetTileName(tile, itemId);
            }
            if (TileType == TileType.Liquid)
            {
                return GetLiquidName(tile);
            }
            if (TileType == TileType.Wall)
            {
                return GetWallName(tile, itemId);
            }
            return "Default Name";
        }

        public TwailaTexture GetImage(SpriteBatch spriteBatch, Tile tile)
        {
            if (TileType == TileType.Tile)
            {
                return GetTileImage(spriteBatch, tile);
            }
            if (TileType == TileType.Liquid)
            {
                return GetLiquidImage(spriteBatch, tile);
            }
            if (TileType == TileType.Wall)
            {
                return GetWallImage(spriteBatch, tile);
            }
            return GetTileImage(spriteBatch, tile);
        }

        public TwailaTexture GetImage(SpriteBatch spriteBatch, int itemId)
        {
            if (TileType == TileType.Tile)
            {
                return GetTileItemImage(spriteBatch, itemId);
            }
            if (TileType == TileType.Liquid)
            {
                return GetLiquidItemImage(spriteBatch, itemId);
            }
            if (TileType == TileType.Wall)
            {
                return GetWallItemImage(spriteBatch, itemId);
            }
            return GetTileItemImage(spriteBatch, itemId);
        }

        public string GetMod()
        {
            if(TileType == TileType.Tile)
            {
                return GetTileMod();
            }
            if(TileType == TileType.Liquid)
            {
                return GetLiquidMod();
            }
            if(TileType == TileType.Wall)
            {
                return GetWallMod();
            }
            return GetTileMod();
        }

        public void SetTileType(TileType type)
        {
            if(!HasTile() && !HasLiquid() && !HasWall() && !Tile.RedWire && !Tile.BlueWire && !Tile.GreenWire && !Tile.YellowWire && !Tile.Actuator)
            {
                TileType = TileType.Empty;
            }
            else if ((type == TileType.Tile && HasTile()) || (type == TileType.Liquid && HasLiquid()) || (type == TileType.Wall && HasWall()))
            {
                TileType = type;
            }
        }

        public bool HasWall()
        {
            return Tile.WallId > 0;
        }

        public bool HasTile()
        {
            return Tile.HasTile && Tile.TileId >= 0;
        }

        public bool HasLiquid()
        {
            return Tile.LiquidAmount > 0;
        }

        public bool OnlyWire()
        {
            return (Tile.RedWire || Tile.BlueWire || Tile.GreenWire || Tile.YellowWire || Tile.Actuator) &&
                !HasTile() && !HasWall() && !HasLiquid();
        }

        public virtual bool ContentChanged(TileContext other)
        {
            if (other.GetType() == GetType())
            {
                if (TileType == TileType.Tile && Tile.TileId == other.Tile.TileId)
                {
                    if(OnlyWire() && other.OnlyWire() && Tile.Actuator != other.Tile.Actuator)
                    {
                        return true;
                    }
                    return StyleChanged(other);
                }
                if (TileType == TileType.Wall && Tile.WallId == other.Tile.WallId)
                {
                    return false;
                }
                if (TileType == TileType.Liquid && Tile.LiquidId == other.Tile.LiquidId)
                {
                    return false;
                }
            }
            return true;
        }

        public virtual bool StyleChanged(TileContext other)
        {
            TileObjectData oldData = TileUtil.GetTileObjectData(other.Tile), newData = TileUtil.GetTileObjectData(Tile);
            if (newData == null || oldData == null)
            {
                return false;
            }
            int oldRow = other.Tile.TileFrameX / oldData.CoordinateFullWidth;
            int oldCol = other.Tile.TileFrameY / oldData.CoordinateFullHeight;
            int newRow = Tile.TileFrameX / newData.CoordinateFullWidth;
            int newCol = Tile.TileFrameY / newData.CoordinateFullHeight;

            return oldRow != newRow || oldCol != newCol;
        }

        protected virtual string GetTileName(Tile tile, int itemId)
        {
            return NameUtil.GetNameForManualTiles(tile) ?? NameUtil.GetNameForChest(tile) ?? NameUtil.GetNameFromItem(itemId)
                ?? NameUtil.GetNameFromMap(tile, Pos.X, Pos.Y) ?? "Default Name";
        }

        protected virtual string GetWallName(Tile tile, int itemId)
        {
            return NameUtil.GetNameFromItem(itemId) ?? "Default Wall";
        }

        protected virtual string GetLiquidName(Tile tile)
        {
            return NameUtil.GetNameForLiquids(tile) ?? "Default Liquid";
        }

        protected virtual TwailaTexture GetTileImage(SpriteBatch spriteBatch, Tile tile)
        {
            Texture2D texture = TreeUtil.GetImageForVanityTree(spriteBatch, tile.TileType) ??
                    TreeUtil.GetImageForGemTree(spriteBatch, tile.TileType);
            if(texture != null)
            {
                return new TwailaTexture(texture, 0.5f);
            }
            if(OnlyWire())
            {
                return new TwailaTexture(ImageUtil.GetImageForWireAndActuator(spriteBatch, tile));
            }
            texture = ImageUtil.GetImageCustom(spriteBatch, tile) ?? ImageUtil.GetImageFromTileDrawing(spriteBatch, tile, Pos.X, Pos.Y) ?? ImageUtil.GetImageFromTile(spriteBatch, tile);
            return new TwailaTexture(texture);
        }

        protected virtual TwailaTexture GetWallImage(SpriteBatch spriteBatch, Tile tile)
        {
            return new TwailaTexture(ImageUtil.GetWallImageFromTile(spriteBatch, tile));
        }

        protected virtual TwailaTexture GetLiquidImage(SpriteBatch spriteBatch, Tile tile)
        {
            return new TwailaTexture(ImageUtil.GetLiquidImageFromTile(spriteBatch, tile));
        }

        protected virtual TwailaTexture GetTileItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return new TwailaTexture(ImageUtil.GetItemTexture(itemId));
        }

        protected virtual TwailaTexture GetWallItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return new TwailaTexture(ImageUtil.GetItemTexture(itemId));
        }

        protected virtual TwailaTexture GetLiquidItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return new TwailaTexture(ImageUtil.GetItemTexture(itemId));
        }

        protected virtual string GetTileMod()
        {
            return NameUtil.GetModName(this);
        }

        protected virtual string GetWallMod()
        {
            return NameUtil.GetModName(this);
        }

        protected virtual string GetLiquidMod()
        {
            return NameUtil.GetModName(this);
        }

    }
}
