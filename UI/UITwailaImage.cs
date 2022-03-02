
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;
using Terraria.ObjectData;
using Terraria.UI;
using Twaila.ObjectData;
using Twaila.Util;

namespace Twaila.UI
{
    public class UITwailaImage : UIElement
    {
        public float Scale;
        private Texture2D _image;
        public UITwailaImage()
        {
            Scale = 1;
            _image = Main.buffTexture[BuffID.Confused];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(_image != null)
            {
                Width.Set(_image.Width * Scale, 0);
                Height.Set(_image.Height * Scale, 0);
                Recalculate();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_image != null)
            {
                spriteBatch?.Draw(_image, new Vector2(GetDimensions().ToRectangle().X, GetDimensions().ToRectangle().Y), 
                    new Rectangle(0, 0, _image.Width, _image.Height), Color.White, 0, Vector2.Zero, Scale, 0, 0);
            }
        }

        public void SetImage(SpriteBatch spriteBatch, Tile tile, int itemId, Point pos)
        {
            //_image = GetDebugImage(spriteBatch, tile); return;
            _image = GetImageCustom(spriteBatch, pos, tile);
            if(_image == null)
            {
                if (TwailaConfig.Get().UseItemTextures)
                {
                    _image = GetImageFromItemData(tile, itemId) ?? GetImageFromTileData(spriteBatch, tile) ?? GetImageFromTile(spriteBatch, tile);
                }
                else
                {
                    _image = GetImageFromTileData(spriteBatch, tile) ?? GetImageFromTile(spriteBatch, tile) ?? GetImageFromItemData(tile, itemId);
                }
            }
            if (_image == null)
            {
                _image = Main.buffTexture[BuffID.Confused];
            }
        }

        private Texture2D GetImageFromTileData(SpriteBatch spriteBatch, Tile tile)
        {
            Scale = 1;
            TileObjectData data = ExtraObjectData.GetData(tile.type) ?? TileObjectData.GetTileData(tile);
            if(data == null)
            {
                return null;
            }
            return GetImageFromTileObjectData(spriteBatch, tile, data);
        }

        private Texture2D GetImageFromTile(SpriteBatch spriteBatch, Tile tile)
        {
            Scale = 1;
            int size = 16;
            int padding = 2;
            Texture2D texture = GetTileTexture(tile);
            
            if (texture != null)
            {
                TextureBuilder builder = new TextureBuilder();
                for(int row = 0; row < 2; ++row)
                {
                    for(int col = 0; col < 2; ++col)
                    {
                        Rectangle copyRectangle = new Rectangle(col * (size + padding), 54 + row * (size + padding), size, size);
                        builder.AddComponent(copyRectangle, texture, new Point(size * col, size * row));
                    }
                }
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        private Texture2D GetImageFromItemData(Tile tile, int itemId)
        {
            Scale = 1;
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

        private Texture2D GetImageCustom(SpriteBatch spriteBatch, Point pos, Tile tile)
        {
            return GetImageForTrees(spriteBatch, pos, tile) ?? GetImageForCactus(spriteBatch, pos, tile) ?? 
                GetImageForStalactite(spriteBatch, tile) ?? GetImageForPiles(spriteBatch, tile) ??
                GetImageForJungleFoliage(spriteBatch, tile);
        }

        private Texture2D GetImageForTrees(SpriteBatch spriteBatch, Point pos, Tile tile)
        {
            Scale = 0.5f;
            if (tile.type == TileID.Trees)
            {
                int treeDirt = TreeUtil.GetTreeDirt(pos.X, pos.Y, tile);
                if (treeDirt == -1)
                {
                    return null;
                }
                if (TileLoader.CanGrowModTree(treeDirt))
                {
                    return TreeUtil.GetImageForModdedTree(spriteBatch, treeDirt);
                }
                int treeWood = TreeUtil.GetTreeWood(treeDirt);
                if (treeWood != -1)
                {
                    return TreeUtil.GetImageForVanillaTree(spriteBatch, treeWood, pos.Y);
                }
            }
            else if (tile.type == TileID.PalmTree)
            {
                int palmTreeSand = TreeUtil.GetPalmTreeSand(pos.X, pos.Y, tile);
                if (palmTreeSand == -1)
                {
                    return null;
                }
                if (TileLoader.CanGrowModPalmTree(palmTreeSand))
                {
                    return TreeUtil.GetImageForModdedPalmTree(spriteBatch, palmTreeSand);
                }
                int palmTreeWood = TreeUtil.GetTreeWood(palmTreeSand);
                if (palmTreeWood != -1)
                {
                    return TreeUtil.GetImageForPalmTree(spriteBatch, palmTreeWood);
                }
            }
            else if (tile.type == TileID.MushroomTrees)
            {
                return TreeUtil.GetImageForMushroomTree(spriteBatch);
            }
            return null;
        }

        private Texture2D GetImageForCactus(SpriteBatch spriteBatch, Point pos, Tile tile)
        {
            Scale = 1;
            if (tile.type == TileID.Cactus)
            {
                int cactusSand = TreeUtil.GetCactusSand(pos.X, pos.Y, tile);
                if (cactusSand == -1)
                {
                    return null;
                }
                if (TileLoader.CanGrowModCactus(cactusSand))
                {
                    return TreeUtil.GetImageForCactus(spriteBatch, cactusSand, true);
                }
                return TreeUtil.GetImageForCactus(spriteBatch, cactusSand, false);
            }
            return null;
        }

        private Texture2D GetImageForStalactite(SpriteBatch spriteBatch, Tile tile)
        {
            Scale = 1;
            if (tile.type == TileID.Stalactite)
            {
                TileObjectData data = new TileObjectData();
                if(tile.frameY <= 69)
                {
                    data.CopyFrom(TileObjectData.Style1x2);
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else if(tile.frameY <= 105)
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

        private Texture2D GetImageForPiles(SpriteBatch spriteBatch, Tile tile)
        {
            Scale = 1;
            if(tile.type == TileID.SmallPiles)
            {
                TileObjectData data = new TileObjectData();
                if(tile.frameY < 18)
                {
                    data.CopyFrom(TileObjectData.Style1x1);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16 };
                }
                else if(tile.frameY < 52)
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

        private Texture2D GetImageForJungleFoliage(SpriteBatch spriteBatch, Tile tile)
        {
            Scale = 1;
            if(tile.type == TileID.PlantDetritus)
            {
                TileObjectData data = new TileObjectData();
                if (tile.frameY < 36)
                {
                    data.CopyFrom(TileObjectData.Style3x2);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else if(tile.frameY < 70)
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

        private Texture2D GetDebugImage(SpriteBatch spriteBatch, Tile tile)
        {
            TextureBuilder builder = new TextureBuilder();
            Texture2D texture = GetTileTexture(tile);
            builder.AddComponent(new Rectangle(0, 0, texture.Width, texture.Height), texture, Point.Zero);
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        private static Texture2D GetImageFromTileObjectData(SpriteBatch spriteBatch, Tile tile, TileObjectData data)
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
            return itemId == -1 ? null : Main.itemTexture[itemId];
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
