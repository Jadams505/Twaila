using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;
using Terraria.ObjectData;
using Twaila.ObjectData;
using Twaila.Graphics;

namespace Twaila.Util
{
    internal class ImageUtil
    {
        public static Texture2D GetImageFromTileData(SpriteBatch spriteBatch, Tile tile)
        {
            TileObjectData data = ExtraObjectData.GetData(tile.type) ?? TileObjectData.GetTileData(tile);
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

        public static Texture2D GetImageFromItemData(Tile tile, int itemId)
        {
            if (itemId != -1)
            {
                Texture2D itemTexture = GetItemTexture(tile, itemId);
                if (itemTexture != null)
                {
                    return itemTexture;
                }
            }
            return null;
        }

        public static Texture2D GetImageCustom(SpriteBatch spriteBatch, Tile tile)
        {
            return GetImageForStalactite(spriteBatch, tile) ?? GetImageForPiles(spriteBatch, tile) ??
                GetImageForJungleFoliage(spriteBatch, tile) ?? GetImageForCampfire(spriteBatch, tile);
        }

        public static Texture2D GetImageForStalactite(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.type == TileID.Stalactite)
            {
                TileObjectData data = new TileObjectData();
                if (tile.frameY <= 69)
                {
                    data.CopyFrom(TileObjectData.Style1x2);
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else if (tile.frameY <= 105)
                {
                    data.CopyFrom(TileObjectData.Style1x1);
                    data.CoordinateHeights = new int[] { 16 };
                }
                else
                {
                    data = null;
                }
                return GetImageFromTileObjectData(spriteBatch, tile, data);
            }
            return null;
        }

        public static Texture2D GetImageForPiles(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.type == TileID.SmallPiles)
            {
                TileObjectData data = new TileObjectData();
                if (tile.frameY < 18)
                {
                    data.CopyFrom(TileObjectData.Style1x1);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16 };
                }
                else if (tile.frameY < 52)
                {
                    data.CopyFrom(TileObjectData.Style2x1);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16 };
                }
                else
                {
                    data = null;
                }
                return GetImageFromTileObjectData(spriteBatch, tile, data);
            }
            return null;
        }

        public static Texture2D GetImageForJungleFoliage(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.type == TileID.PlantDetritus)
            {
                TileObjectData data = new TileObjectData();
                if (tile.frameY < 36)
                {
                    data.CopyFrom(TileObjectData.Style3x2);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else if (tile.frameY < 70)
                {
                    data.CopyFrom(TileObjectData.Style2x2);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else
                {
                    data = null;
                }
                return GetImageFromTileObjectData(spriteBatch, tile, data);
            }
            return null;
        }

        public static Texture2D GetImageForCampfire(SpriteBatch spriteBatch, Tile tile)
        {
            if (tile.type == TileID.Campfire && !TwailaConfig.Get().UseItemTextures)
            {
                TileObjectData data = TileObjectData.GetTileData(tile);
                if (tile.frameY >= data.CoordinateFullHeight)
                {
                    tile.frameY = 288;
                }
                return GetImageFromTileObjectData(spriteBatch, tile, data);
            }
            return null;
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

                int frameX = tile.frameX / data.CoordinateFullWidth * data.CoordinateFullWidth;
                int frameY = tile.frameY / data.CoordinateFullHeight * data.CoordinateFullHeight;

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

        private static Texture2D GetTileTexture(Tile tile)
        {
            return Main.tileTexture[tile.type];
        }

        private static Texture2D GetItemTexture(Tile tile, int itemId)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            if (mTile != null)
            {
                return GetModdedItemTexture(itemId);
            }
            return itemId <= -1 ? null : Main.itemTexture[itemId];
        }

        private static Texture2D GetModdedItemTexture(int itemId)
        {
            ModItem mItem = ModContent.GetModItem(itemId);
            if (mItem != null)
            {
                try
                {
                    return ModContent.GetTexture(mItem.Texture);
                }
                catch (MissingResourceException) { }
            }
            return null;
        }
    }
}
