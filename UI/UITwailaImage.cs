
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
            //if (SetDebugImage(spriteBatch, tile)) return;
            bool drawSuccess;
            if (DrawCustom(spriteBatch, pos, tile))
            {
                drawSuccess = true;
            }
            else if (TwailaConfig.Get().UseItemTextures)
            {
                drawSuccess = SetImageFromItemData(tile, itemId) || SetImageFromTileData(spriteBatch, tile) || SetImageFromTile(spriteBatch, tile);
            }
            else
            {
                drawSuccess = SetImageFromTileData(spriteBatch, tile) || SetImageFromTile(spriteBatch, tile) || SetImageFromItemData(tile, itemId);
            }

            if (!drawSuccess)
            {
                _image = Main.buffTexture[BuffID.Confused];
            }
        }

        private bool SetImageFromTileData(SpriteBatch spriteBatch, Tile tile)
        {
            Scale = 1;
            TileObjectData data = TileObjectData.GetTileData(tile);
            if(data == null)
            {
                return false;
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
                _image = builder.Build(spriteBatch.GraphicsDevice);
                return _image != null;
            }
            return false;
        }

        private bool SetImageFromTile(SpriteBatch spriteBatch, Tile tile)
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
                _image = builder.Build(spriteBatch.GraphicsDevice);
                return _image != null;
            }
            return false;
        }

        private bool SetImageFromItemData(Tile tile, int itemId)
        {
            Scale = 1;
            if (itemId != -1)
            {
                Texture2D itemTexture = GetItemTexture(tile, itemId);
                if (itemTexture != null)
                {
                    _image = itemTexture;
                    return _image != null;
                }
            }
            return false;
        }

        private bool DrawCustom(SpriteBatch spriteBatch, Point pos, Tile tile)
        {
            if (SetImageForTrees(spriteBatch, pos, tile) || SetImageForCactus(spriteBatch, pos, tile))
            {
                return true;
            }
            return false;
        }

        private bool SetImageForTrees(SpriteBatch spriteBatch, Point pos, Tile tile)
        {
            Scale = 0.5f;
            if (tile.type == TileID.Trees)
            {
                int treeDirt = TreeUtil.GetTreeDirt(pos.X, pos.Y, tile);
                if (treeDirt == -1)
                {
                    return false;
                }
                if (TileLoader.CanGrowModTree(treeDirt))
                {
                    _image = TreeUtil.GetImageForModdedTree(spriteBatch, treeDirt);
                    return _image != null;
                }
                int treeWood = TreeUtil.GetTreeWood(treeDirt);
                if (treeWood != -1)
                {
                    _image = TreeUtil.GetImageForVanillaTree(spriteBatch, treeWood, pos.Y);
                    return _image != null;
                }
            }
            else if (tile.type == TileID.PalmTree)
            {
                int palmTreeSand = TreeUtil.GetPalmTreeSand(pos.X, pos.Y, tile);
                if (palmTreeSand == -1)
                {
                    return false;
                }
                if (TileLoader.CanGrowModPalmTree(palmTreeSand))
                {
                    _image = TreeUtil.GetImageForModdedPalmTree(spriteBatch, palmTreeSand);
                    return _image != null;
                }
                int palmTreeWood = TreeUtil.GetTreeWood(palmTreeSand);
                if (palmTreeWood != -1)
                {
                    _image = TreeUtil.GetImageForPalmTree(spriteBatch, palmTreeWood);
                    return _image != null;
                }
            }
            else if (tile.type == TileID.MushroomTrees)
            {
                _image = TreeUtil.GetImageForMushroomTree(spriteBatch);
                return _image != null;
            }
            return false;
        }

        private bool SetImageForCactus(SpriteBatch spriteBatch, Point pos, Tile tile)
        {
            Scale = 1;
            if (tile.type == TileID.Cactus)
            {
                int cactusSand = TreeUtil.GetCactusSand(pos.X, pos.Y, tile);
                if (cactusSand == -1)
                {
                    return false;
                }
                if (TileLoader.CanGrowModCactus(cactusSand))
                {
                    _image = TreeUtil.GetImageForCactus(spriteBatch, cactusSand, true);
                    return _image != null;
                }
                _image = TreeUtil.GetImageForCactus(spriteBatch, cactusSand, false);
                return _image != null;
            }
            return false;
        }

        private bool SetDebugImage(SpriteBatch spriteBatch, Tile tile)
        {
            TextureBuilder builder = new TextureBuilder();
            Texture2D texture = GetTileTexture(tile);
            builder.AddComponent(new Rectangle(0, 0, texture.Width, texture.Height), texture, Point.Zero);
            _image = builder.Build(spriteBatch.GraphicsDevice);
            return _image != null;
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
