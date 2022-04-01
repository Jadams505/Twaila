﻿using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.ObjectData;
using Terraria.ObjectData;
using Twaila.Graphics;

namespace Twaila.Context
{
    public enum TileType
    {
        Tile, Wall, Liquid, Empty
    }

    public class TileContext
    {
        public Tile Tile { get; private set; }

        public Point Pos { get; private set; }

        public TileType TileType { get; private set; }

        public TileContext(Point pos)
        {
            Pos = pos;
            Tile = GetTileCopy();
            if (HasTile())
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
            Tile = new Tile();
            TileType = TileType.Empty;
        }

        private Tile GetTileCopy()
        {
            Tile copy = new Tile();
            copy.CopyFrom(Framing.GetTileSafely(Pos.X, Pos.Y));
            return copy;
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

        public string GetName(int itemId)
        {
            if (TileType == TileType.Tile)
            {
                return GetTileName(itemId);
            }
            if (TileType == TileType.Liquid)
            {
                return GetLiquidName();
            }
            if (TileType == TileType.Wall)
            {
                return GetWallName(itemId);
            }
            return "Default Name";
        }

        public TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            if (TileType == TileType.Tile)
            {
                return GetTileImage(spriteBatch);
            }
            if (TileType == TileType.Liquid)
            {
                return GetLiquidImage(spriteBatch);
            }
            if (TileType == TileType.Wall)
            {
                return GetWallImage(spriteBatch);
            }
            return GetTileImage(spriteBatch);
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
            if(!HasTile() && !HasLiquid() && !HasWall())
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
            return Tile.wall > 0;
        }

        public bool HasTile()
        {
            return Tile.active() && Tile.type >= 0;
        }

        public bool HasLiquid()
        {
            return Tile.liquid > 0;
        }

        public virtual bool ContentChanged(TileContext other)
        {
            if (other is TileContext otherTileContext)
            {
                if (TileType == TileType.Tile && Tile.type == otherTileContext.Tile.type)
                {
                    return StyleChanged(other);
                }
                if (TileType == TileType.Wall && Tile.wall == otherTileContext.Tile.wall)
                {
                    return false;
                }
                if (TileType == TileType.Liquid && Tile.liquidType() == otherTileContext.Tile.liquidType())
                {
                    return false;
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

        protected virtual string GetTileName(int itemId)
        {
            return NameUtil.GetNameForManualTiles(Tile) ?? NameUtil.GetNameForChest(Tile) ?? NameUtil.GetNameFromItem(itemId)
                ?? NameUtil.GetNameFromMap(this) ?? "Default Name";
        }

        protected virtual string GetWallName(int itemId)
        {
            return NameUtil.GetNameFromItem(itemId) ?? "Default Wall";
        }

        protected virtual string GetLiquidName()
        {
            return "Default Liquid";
        }

        protected virtual TwailaTexture GetTileImage(SpriteBatch spriteBatch)
        {
            Texture2D texture = ImageUtil.GetImageCustom(spriteBatch, Tile) ?? ImageUtil.GetImageFromTileData(spriteBatch, Tile)
                ?? ImageUtil.GetImageFromTile(spriteBatch, Tile);
            return new TwailaTexture(texture);
        }

        protected virtual TwailaTexture GetWallImage(SpriteBatch spriteBatch)
        {
            return new TwailaTexture(ImageUtil.GetWallImageFromTile(spriteBatch, Tile));
        }

        protected virtual TwailaTexture GetLiquidImage(SpriteBatch spriteBatch)
        {
            return null;
        }

        protected virtual TwailaTexture GetTileItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return new TwailaTexture(ImageUtil.GetImageFromItemData(Tile, itemId));
        }

        protected virtual TwailaTexture GetWallItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return GetWallImage(spriteBatch);
        }

        protected virtual TwailaTexture GetLiquidItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return GetLiquidImage(spriteBatch);
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
