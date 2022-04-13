using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Twaila.ObjectData;
using Twaila.Graphics;
using Terraria.GameContent;

namespace Twaila.Util
{
    internal class ImageUtil
    {
        public static Texture2D GetImageFromTileData(SpriteBatch spriteBatch, Tile tile)
        {
            TileObjectData data = ExtraObjectData.GetData(tile) ?? TileObjectData.GetTileData(tile);
            if (data == null)
            {
                return null;
            }
            return GetImageFromTileObjectData(spriteBatch, tile, data);
        }

        public static Texture2D GetImageFromTile(SpriteBatch spriteBatch, Tile tile)
        {
            int size = 16;
            int padding = 2;
            Texture2D texture = GetTileTexture(tile);

            if (texture != null)
            {
                TextureBuilder builder = new TextureBuilder();
                for (int row = 0; row < 2; ++row)
                {
                    for (int col = 0; col < 2; ++col)
                    {
                        Rectangle copyRectangle = new Rectangle(col * (size + padding), 54 + row * (size + padding), size, size);
                        builder.AddComponent(copyRectangle, texture, new Point(size * col, size * row));
                    }
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetWallImageFromTile(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.WallType > 0)
            {
                int size = 32;
                int startX = 324, startY = 108;
                Texture2D texture = GetWallTexture(tile);

                if (texture != null)
                {
                    TextureBuilder builder = new TextureBuilder();
                    Rectangle copyRectangle = new Rectangle(startX, startY, size, size);
                    builder.AddComponent(copyRectangle, texture, new Point(0, 0));
                    return builder.Build(spriteBatch.GraphicsDevice);
                }
            }
            return null;
        }

        public static Texture2D GetLiquidImageFromTile(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.LiquidAmount > 0)
            {
                int size = 16;
                int startX = 0, startY = 0;
                Texture2D texture = null;
                switch (tile.LiquidType)
                {
                    case LiquidID.Lava:
                        texture = TextureAssets.Liquid[1].Value;
                        break;
                    case LiquidID.Honey:
                        texture = TextureAssets.Liquid[11].Value;
                        break;
                    case LiquidID.Water:
                        texture = TextureAssets.Liquid[Main.waterStyle].Value;
                        break;
                }

                if(texture != null)
                {
                    TextureBuilder builder = new TextureBuilder();
                    Rectangle copyRectangle = new Rectangle(startX, startY, size, size);
                    builder.AddComponent(copyRectangle, texture, new Point(0, 0));
                    return builder.Build(spriteBatch.GraphicsDevice);
                }
            }
            return null;
        }

        public static Texture2D GetImageCustom(SpriteBatch spriteBatch, Tile tile)
        {
            return GetImageForCampfire(spriteBatch, tile) ?? GetImageForHerbs(spriteBatch, tile) ?? 
                GetImageForXmasTree(spriteBatch, tile);
        }

        /*
            For some reason the frameY for campfires when they are turned off does not match the frameY on
            their spritesheet. It might have something to do with animation frames
        */
        public static Texture2D GetImageForCampfire(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.TileType == TileID.Campfire && !TwailaConfig.Get().UseItemTextures)
            {
                TileObjectData data = TileObjectData.GetTileData(tile);
                if (tile.TileFrameY >= data.CoordinateFullHeight)
                {
                    tile.TileFrameY = 288; // this is the correct frameY for the campfire when it is turned off
                }
                return GetImageFromTileObjectData(spriteBatch, tile, data);
            }
            return null;
        }

        /*
            Becuase herb tiles bloom and unbloom based on various conditions the tileId's do not reflect what texture
            is actually being displayed. Thus those conditions must be explititly checked in order to determine if the tile is
            mature or blooming
            Blinkroot and Shiverthorn are exceptions because once bloomed they never unbloom
        */
        public static Texture2D GetImageForHerbs(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.TileType == TileID.MatureHerbs)
            {
                int tileId = tile.TileType;
                int style = TileObjectData.GetTileStyle(tile);
                if (style == 0 && Main.dayTime) // daybloom
                {
                    tileId = TileID.BloomingHerbs;
                }
                if (style == 1 && !Main.dayTime) // moonglow
                {
                    tileId = TileID.BloomingHerbs;
                }
                if (style == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0)) // deathweed
                {
                    tileId = TileID.BloomingHerbs;
                }
                if (style == 4 && (Main.raining || Main.cloudAlpha > 0)) // waterleaf
                {
                    tileId = TileID.BloomingHerbs;
                }
                if (style == 5 && !Main.raining && Main.time > 40500) // fireblossom
                {
                    tileId = TileID.BloomingHerbs;
                }
                tile.TileType = (ushort)tileId;
                return GetImageFromTileData(spriteBatch, tile);
            }
            return null;
        }

        /*
            Christmas trees store extra data in the top left tile to account for decorations
        */
        public static Texture2D GetImageForXmasTree(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.ChristmasTree)
            {
                if(tile.TileFrameX >= 10) // the top left tile's frameX is always 10
                {
                    tile.TileFrameY = 0; // sets the frameY to what it would be if it had no decorations
                }
            }
            return GetImageFromTileData(spriteBatch, tile);
        }

        public static Texture2D GetDebugImage(SpriteBatch spriteBatch, Tile tile)
        {
            TextureBuilder builder = new TextureBuilder();
            Texture2D texture = GetTileTexture(tile);
            builder.AddComponent(new Rectangle(0, 0, texture.Width, texture.Height), texture, Point.Zero);
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        public static Texture2D GetImageFromTileObjectData(SpriteBatch spriteBatch, Tile tile, TileObjectData data)
        {
            if (data == null)
            {
                return null;
            }
            Texture2D texture = GetTileTexture(tile);
            if (texture != null)
            {
                TextureBuilder builder = new TextureBuilder();

                int frameX = tile.TileFrameX / data.CoordinateFullWidth * data.CoordinateFullWidth;
                int frameY = tile.TileFrameY / data.CoordinateFullHeight * data.CoordinateFullHeight;

                if (data.Style > data.StyleWrapLimit)
                {
                    if (data.StyleHorizontal)
                    {
                        frameY += data.CoordinateFullHeight;
                    }
                    else
                    {
                        frameX += data.CoordinateFullWidth;
                    }
                }
                int height = 0;
                for (int row = 0; row < data.Height; ++row)
                {
                    for (int col = 0; col < data.Width; ++col)
                    {
                        int width = data.CoordinateWidth;
                        Rectangle copyRectangle = new Rectangle(frameX + (width + data.CoordinatePadding) * col,
                            frameY + height + data.CoordinatePadding * row, width, data.CoordinateHeights[row]);
                        builder.AddComponent(copyRectangle, texture, new Point(width * col, height));
                    }
                    height += data.CoordinateHeights[row];
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetTileTexture(Tile tile)
        {
            if(tile.TileType >= 0 && tile.TileType < TextureAssets.Tile.Length)
            {
                Main.instance.LoadTiles(tile.TileType);
                return TextureAssets.Tile[tile.TileType].Value;
            }
            return null;
        }

        public static Texture2D GetWallTexture(Tile tile)
        {
            if (tile.WallType >= 0 && tile.WallType < TextureAssets.Wall.Length)
            {
                Main.instance.LoadWall(tile.WallType);
                return TextureAssets.Wall[tile.WallType].Value;
            }
            return null;
        }

        public static Texture2D GetItemTexture(int itemId)
        {
            if (itemId >= 0 && itemId < TextureAssets.Item.Length)
            {
                Main.instance.LoadItem(itemId);
                return TextureAssets.Item[itemId].Value;
            }
            return null;
        }
    }
}
