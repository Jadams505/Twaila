using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Twaila.ObjectData;
using Twaila.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;

namespace Twaila.Util
{
    internal class ImageUtil
    {
        public static Texture2D GetImageFromTile(SpriteBatch spriteBatch, Tile tile)
        {
            int size = 16;
            int padding = 2;
            Texture2D texture = GetTileTexture(tile.TileType);

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
                        texture = TextureAssets.Liquid[WaterStyleID.Lava].Value;
                        break;
                    case LiquidID.Honey:
                        texture = TextureAssets.Liquid[WaterStyleID.Honey].Value;
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
                GetImageForXmasTree(spriteBatch, tile) ?? TreeUtil.GetImageForBamboo(spriteBatch, tile.TileType)
                ?? TreeUtil.GetImageForSeaweed(spriteBatch, tile.TileType) ?? GetImageForMannequins(spriteBatch, tile)
                ?? GetImageForSnakeRope(spriteBatch, tile.TileType) ?? GetImageForCattail(spriteBatch, tile)
                ?? GetImageForRelic(spriteBatch, tile) ?? GetImageForPylon(spriteBatch, tile) ??
                GetImageForVoidVault(spriteBatch, tile);
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
                int mutableFrameY = tile.TileFrameY;
                if (tile.TileFrameY >= data.CoordinateFullHeight)
                {
                    mutableFrameY = 288; // this is the correct frameY for the campfire when it is turned off
                }
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, tile.TileFrameX, mutableFrameY, data);
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
                int mutableId = tile.TileType;
                int style = TileObjectData.GetTileStyle(tile);
                TileObjectData data = TileUtil.GetTileObjectData(tile);
                if (style == 0 && Main.dayTime) // daybloom
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 1 && !Main.dayTime) // moonglow
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 3 && !Main.dayTime && (Main.bloodMoon || Main.moonPhase == 0)) // deathweed
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 4 && (Main.raining || Main.cloudAlpha > 0)) // waterleaf
                {
                    mutableId = TileID.BloomingHerbs;
                }
                else if (style == 5 && !Main.raining && Main.time > 40500) // fireblossom
                {
                    mutableId = TileID.BloomingHerbs;
                }
                return GetImageFromTileObjectData(spriteBatch, mutableId, tile.TileFrameX, tile.TileFrameY, data);
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
                int mutableFrameY = tile.TileFrameY;
                if(tile.TileFrameX >= 10) // the top left tile's frameX is always 10
                {
                    mutableFrameY = 0; // sets the frameY to what it would be if it had no decorations
                }
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, tile.TileFrameX, mutableFrameY, TileUtil.GetTileObjectData(tile));
            }
            return null;
        }

        public static Texture2D GetImageForMannequins(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.DisplayDoll)
            {
                TileObjectData data = TileUtil.GetTileObjectData(tile);
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, tile.TileFrameX + data.CoordinateFullWidth * 4, tile.TileFrameY, TileUtil.GetTileObjectData(tile));
            }
            return null;
        }

        public static Texture2D GetImageForSnakeRope(SpriteBatch spriteBatch, int tileId)
        {
            if(tileId == TileID.MysticSnakeRope)
            {
                TextureBuilder builder = new TextureBuilder();
                Texture2D texture = GetTileTexture(tileId);
                int size = 16;
                int padding = 2;
                for(int i = 0; i < 3; ++i)
                {
                    builder.AddComponent(new Rectangle(size + padding, 0 + i * (size + padding), size, size), texture, 
                        new Point(0, 0 + i * size));
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForCattail(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.Cattail)
            {
                TextureBuilder builder = new TextureBuilder();
                Texture2D texture = GetTileTexture(tile.TileType);
                int size = 16;
                int padding = 2;
                int bottomStyle = 4;
                int middleStyle = 8;
                int topStyle = 11;

                Point drawPos = Point.Zero;
                builder.AddComponent(new Rectangle((size + padding) * topStyle, tile.TileFrameY, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle((size + padding) * middleStyle, tile.TileFrameY, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle((size + padding) * bottomStyle, tile.TileFrameY, size, size), texture, drawPos);
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForRelic(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.MasterTrophyBase)
            {
                Texture2D baseTexture = GetTileTexture(tile.TileType);
                Texture2D relicTexture = TextureAssets.Extra[198].Value;
                int size = 48;
                int padding = 2;
                int frameY = tile.TileFrameX / 54 * (size + padding);

                Point drawPos = Point.Zero;
                TextureBuilder builder = new TextureBuilder();
                builder.AddComponent(new Rectangle(0, frameY, size, size), relicTexture, drawPos);
                drawPos.Y += size;
                for(int i = 0; i < 3; ++i)
                {
                    builder.AddComponent(new Rectangle(i * 18, 54, 16, 16), baseTexture, drawPos);
                    drawPos.X += 16;
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForPylon(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.TeleportationPylon)
            {
                Texture2D baseTexture = GetTileTexture(tile.TileType);
                Texture2D pylonTexture = TextureAssets.Extra[181].Value;
                int pylonWidth = 28, pylonHeight = 44;
                int padding = 2;
                int frameX = 90 + (tile.TileFrameX / 54 * (pylonWidth + padding));

                Point drawPos = Point.Zero;
                TextureBuilder builder = new TextureBuilder();             
                int startY = 18;
                int startX = tile.TileFrameX / 54 * 54;
                for (int row = 0; row < 3; ++row)
                {
                    for (int col = 0; col < 3; ++col)
                    {
                        builder.AddComponent(new Rectangle(startX + (col * 18), startY + (row * 18), 16, 16), baseTexture, new Point(16 * col, row * 16));
                    }
                }
                drawPos.X += 10;
                drawPos.Y -= 16;
                builder.AddComponent(new Rectangle(frameX, 0, pylonWidth, pylonHeight), pylonTexture, drawPos);
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForVoidVault(SpriteBatch spriteBatch, Tile tile)
        {
            if(tile.TileType == TileID.VoidVault)
            {
                return GetImageFromTileObjectData(spriteBatch, tile.TileType, 0, 0, TileUtil.GetTileObjectData(tile));
            }
            return null;
        }

        public static Texture2D GetImageForWireAndActuator(SpriteBatch spriteBatch, Tile tile)
        {
            if(!tile.HasTile && tile.WallType == 0 && tile.LiquidAmount <= 0)
            {
                if (tile.HasActuator)
                {
                    return GetItemTexture(ItemID.Actuator);
                }
                if (tile.YellowWire || tile.GreenWire || tile.BlueWire || tile.RedWire)
                {
                    return GetItemTexture(ItemID.Wire);
                } 
            }
            return null;
        }

        public static Texture2D GetDebugImage(SpriteBatch spriteBatch, Tile tile)
        {
            TextureBuilder builder = new TextureBuilder();
            Texture2D texture = GetTileTexture(tile.TileType);
            builder.AddComponent(new Rectangle(0, 0, texture.Width, texture.Height), texture, Point.Zero);
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        public static Texture2D GetImageFromTileObjectData(SpriteBatch spriteBatch, int tileId, int frameX, int frameY, TileObjectData data)
        {
            if (data == null)
            {
                return null;
            }
            Texture2D texture = GetTileTexture(tileId);
            if (texture != null)
            {
                TextureBuilder builder = new TextureBuilder();

                frameX = frameX / data.CoordinateFullWidth * data.CoordinateFullWidth;
                frameY = frameY / data.CoordinateFullHeight * data.CoordinateFullHeight;

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

        public static Texture2D GetImageFromTileDrawing(SpriteBatch spriteBatch, Tile tile, int posX, int posY)
        {
            TileObjectData data = TileUtil.GetTileObjectData(tile);
            short tileFx = tile.TileFrameX, tileFy = tile.TileFrameY;
            Main.instance.TilesRenderer.GetTileDrawData(posX, posY, tile, tile.TileType,
                ref tileFx, ref tileFy, out int width, out int height, out int top, out int h, out int addX, out int addY,
                out _, out _, out _, out _);
            if (Main.tileFrame[tile.TileType] == 0) // if the tile is not animated
            {
                tileFx += (short)addX;
                tileFy += (short)addY;
            }
            return GetImageFromTileObjectData(spriteBatch, tile.TileType, tileFx, tileFy, data);
        }

        public static Texture2D GetTileTexture(int tileId)
        {
            if(tileId >= 0 && tileId < TextureAssets.Tile.Length)
            {
                Main.instance.LoadTiles(tileId);
                return TextureAssets.Tile[tileId].Value;
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
