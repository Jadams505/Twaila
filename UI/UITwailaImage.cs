
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;
using Twaila.Util;

namespace Twaila.UI
{
    public class UITwailaImage : UIElement
    {
        public Point Pos { get; private set; }
        public Tile Tile { get; private set; }
        public float Scale;
        public int ItemId { get; private set; }
        public UITwailaImage() : this(Point.Zero, new Tile())
        {
        }
        public UITwailaImage(Point pos, Tile tile, int itemId = -1, float scale = 1)
        {
            Tile = tile;
            Pos = pos;
            ItemId = itemId;
            Scale = scale;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            bool drawSuccess = false;
            if (DrawForTrees(spriteBatch))
            {
                return;
            }
            if (TwailaConfig.Get().UseItemTextures)
            {
                drawSuccess = !DrawFromItemData(spriteBatch) && !DrawFromTileData(spriteBatch) && !DrawFromTile(spriteBatch);
                return;
            }
            drawSuccess = !DrawFromTileData(spriteBatch) && !DrawFromTile(spriteBatch) && !DrawFromItemData(spriteBatch);
        }
        private bool DrawFromTileData(SpriteBatch spriteBatch)
        {
            TileObjectData data = TileObjectData.GetTileData(Tile);
            Texture2D texture = GetTileTexture();
            if (data != null && texture != null && !texture.Equals(TextureManager.BlankTexture))
            {
                SetSizeFromTileData();
                int fullWidth = GetSpriteWidth() + (data.Width * data.CoordinatePadding);
                int fullHeight = GetSpriteHeight() + (data.Height * data.CoordinatePadding);
                CalculatedStyle dim = GetDimensions();
                int frameX = Tile.frameX / fullWidth * fullWidth;
                int frameY = Tile.frameY / fullHeight * fullHeight;
                if (data.Style > data.StyleWrapLimit)
                {
                    if (data.StyleHorizontal)
                    {
                        frameY += fullHeight;
                    }
                    else
                    {
                        frameX += fullWidth;
                    }  
                }
                for (int row = 0; row < data.Height; ++row)
                {
                    for (int col = 0; col < data.Width; ++col)
                    {
                        float drawPosX = (int)Math.Round(dim.X) + col * data.CoordinateWidth;
                        float drawPosY = (int)Math.Round(dim.Y) + row * data.CoordinateHeights[row - 1 >= 0 ? row - 1 : 0];
                        spriteBatch.Draw(texture, new Vector2(drawPosX, drawPosY),
                            new Rectangle(frameX + col * (data.CoordinateWidth + data.CoordinatePadding), 
                            frameY + row * (data.CoordinateHeights[row - 1 >= 0 ? row - 1 : 0] + data.CoordinatePadding),
                            data.CoordinateWidth, data.CoordinateHeights[row]), Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    }
                }
                return true;
            }
            return false;
        }
        private bool DrawFromTile(SpriteBatch spriteBatch)
        {
            
            CalculatedStyle dim = GetDimensions();
            int size = 16;
            int padding = 2;
            Texture2D texture = GetTileTexture();
            if(texture != null && !texture.Equals(TextureManager.BlankTexture))
            {
                SetSizeFromTile();
                for (int row = 0; row < 2; ++row)
                {
                    for (int col = 0; col < 2; ++col)
                    {
                        Vector2 drawPos = new Vector2((int)dim.X + col * size, (int)dim.Y + row * size);
                        Rectangle spriteData = new Rectangle(col * (size + padding), 54 + row * (size + padding), size, size);
                        spriteBatch.Draw(texture, drawPos, spriteData, Color.White, 0, Vector2.Zero, Scale, 0, 0);
                    }
                }
                return true;
            }
            return false;
        }
        private void SetSizeFromTile()
        {
            Width.Set(32, 0);
            Height.Set(32, 0);
            Recalculate();
        }
        private Texture2D GetTileTexture()
        {
            ModTile mTile = TileLoader.GetTile(Tile.type);
            if(mTile != null)
            {
                return GetModdedTileTexture();
            }
            return Main.tileTexture[Tile.type];
        }
        private Texture2D GetModdedTileTexture()
        {
            ModTile mTile = TileLoader.GetTile(Tile.type);
            if(mTile != null)
            {
                string texturePath = mTile.HighlightTexture;
                int index = texturePath.IndexOf("_Highlight");
                if(index != -1)
                {
                    try
                    {
                        return ModContent.GetTexture(texturePath.Substring(0, index));
                    }
                    catch (Exception) { }
                }          
            }
            return TextureManager.BlankTexture;
        }
        private void SetSizeFromTileData()
        {
            Width.Set(GetSpriteWidth(), 0);
            Height.Set(GetSpriteHeight(), 0);
            Recalculate();
        }
        private bool DrawFromItemData(SpriteBatch spriteBatch)
        {
            if(ItemId != -1)
            {    
                Texture2D itemTexture = GetItemTexture();     
                if (itemTexture != null && !itemTexture.Equals(TextureManager.BlankTexture))
                {
                    SetSizeFromItemData(itemTexture);
                    spriteBatch.Draw(position: GetDimensions().Position() + itemTexture.Size() * (1f - Scale) / 2f, texture: itemTexture, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: Scale, effects: SpriteEffects.None, layerDepth: 0f);
                    return true;
                } 
            }
            return false;
        }
        private Texture2D GetItemTexture()
        {
            ModTile mTile = TileLoader.GetTile(Tile.type);
            if (mTile != null)
            {
                return GetModdedItemTexture();
            }
            return ItemId == -1 ? TextureManager.BlankTexture : Main.itemTexture[ItemId];
        }
        private Texture2D GetModdedItemTexture()
        {
            ModItem mItem = ModContent.GetModItem(ItemId);
            if (mItem != null)
            {
                try
                {
                    return ModContent.GetTexture(mItem.Texture);
                }
                catch (Exception) { }
            }
            return TextureManager.BlankTexture;
        }
        private void SetSizeFromItemData(Texture2D itemTexture)
        {
            Width.Set(itemTexture.Width, 0);
            Height.Set(itemTexture.Height, 0);
            Recalculate();
        }

        private bool DrawForTrees(SpriteBatch spriteBatch)
        {
            if (Tile.type == TileID.Trees)
            {
                int? treeType = TwailaUtil.GetTreeType(Pos.X, Pos.Y);
                if (treeType != null)
                {
                    DrawTree(spriteBatch, treeType.Value);
                    return true;
                }
            }
            else if (Tile.type == TileID.PalmTree) 
            { 
                int? palmTreeType = TwailaUtil.GetPalmTreeType(Pos.X, Pos.Y);
                if(palmTreeType != null)
                {
                    DrawPalmTree(spriteBatch, palmTreeType.Value);
                    return true;
                }
            }
            else if(Tile.type == TileID.MushroomTrees)
            {
                DrawMushroomTree(spriteBatch);
                return true;
            }
            return false;
        }

        private void DrawTree(SpriteBatch spriteBatch, int treeType)
        {
            Scale = 0.5f;
            int size = 20;
            int unit = (int)Math.Round(16 * Scale);

            Texture2D topTexture = Main.treeTopTexture[treeType + 1];
            Texture2D woodTexture = treeType == -1 ? Main.tileTexture[TileID.Trees] : Main.woodTexture[treeType];
            Texture2D branchTexture = Main.treeBranchTexture[treeType + 1];
            Rectangle top = new Rectangle(82, 0, size * 4, size * 4);
            Rectangle trunk1 = new Rectangle(44, 108, size, size);
            Rectangle trunk2 = new Rectangle(88, 42, size, size);
            Rectangle trunk3 = new Rectangle(66, 66, size, size);
            Rectangle leftBranch = new Rectangle(0, 42, size * 2, size * 2);
            Rectangle rightBranch = new Rectangle(42, 42, size * 2, size * 2);
            Rectangle bottomMiddle = new Rectangle(88, 154, size, size);
            Rectangle bottomLeft = new Rectangle(44, 176, size, size);
            Rectangle bottomRight = new Rectangle(22, 154, size, size);

            int topOffsetX = (int)Math.Round(30 * Scale);
            int topOffsetY = (int)Math.Round(78 * Scale);

            Width.Set(40, 0);
            Height.Set(74, 0);

            switch (treeType)
            {
                case 1: // jungle
                    Width.Set(56, 0);
                    Height.Set(80, 0);
                    top = new Rectangle(0, 0, 114, 94);
                    topOffsetX = (int)Math.Round(46 * Scale);
                    topOffsetY = (int)Math.Round(92 * Scale);
                    break;
                case 2: // hallow
                    Width.Set(40, 0);
                    Height.Set(92, 0);
                    top = new Rectangle(84, 22, 76, 118);
                    topOffsetX = (int)Math.Round(28 * Scale);
                    topOffsetY = (int)Math.Round(116 * Scale);
                    break;
                case 5: // underground jungle
                    Width.Set(56, 0);
                    Height.Set(79, 0);
                    top = new Rectangle(236, 4, 112, 92);
                    topOffsetX = (int)Math.Round(42 * Scale);
                    topOffsetY = (int)Math.Round(90 * Scale);
                    topTexture = Main.treeTopTexture[13];
                    branchTexture = Main.treeBranchTexture[13];
                    break;
                case 3: // snow
                    topTexture = Main.treeTopTexture[12];
                    branchTexture = Main.treeBranchTexture[12];
                    break;
                case 6: // mushroom
                    topTexture = Main.treeTopTexture[14];
                    branchTexture = Main.treeBranchTexture[14];
                    break;  
            }

            CalculatedStyle drawPos = GetDimensions();
            spriteBatch.Draw(topTexture, new Vector2(drawPos.X, drawPos.Y), top, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk1, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk2, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(branchTexture, new Vector2(drawPos.X - (int)Math.Round(38 * Scale), drawPos.Y - (int)Math.Round(12 * Scale)), leftBranch, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk3, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(branchTexture, new Vector2(drawPos.X + (int)Math.Round(18 * Scale), drawPos.Y - (int)Math.Round(12 * Scale)), rightBranch, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), bottomMiddle, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X - unit, drawPos.Y), bottomLeft, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X + unit, drawPos.Y), bottomRight, Color.White, 0, Vector2.Zero, Scale, 0, 0);
        }

        private void DrawPalmTree(SpriteBatch spriteBatch, int palmTreeType)
        {
            Scale = 0.5f;
            int size = 20;
            int unit = (int)Math.Round(16 * Scale);
            Texture2D woodTexture = Main.tileTexture[TileID.PalmTree];
            Texture2D topTexture = Main.treeTopTexture[15];

            Rectangle top = new Rectangle(82, palmTreeType * 82, size * 4, size * 4);
            Rectangle trunk1 = new Rectangle(0, palmTreeType * 22, size, size);
            Rectangle trunk2 = new Rectangle(42, palmTreeType * 22, size, size);
            Rectangle bottom = new Rectangle(66, palmTreeType * 22, size, size);

            int topOffsetX = (int)Math.Round(30 * Scale);
            int topOffsetY = (int)Math.Round(78 * Scale);

            Width.Set((int)(78 * Scale), 0);
            Height.Set((int)(160 * Scale), 0);

            CalculatedStyle drawPos = GetDimensions();
            spriteBatch.Draw(topTexture, new Vector2(drawPos.X, drawPos.Y), top, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk1, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk2, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            drawPos.X += 2 * Scale;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk1, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            drawPos.X += 2 * Scale;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk1, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), bottom, Color.White, 0, Vector2.Zero, Scale, 0, 0);
        }

        private void DrawMushroomTree(SpriteBatch spriteBatch)
        {
            Scale = 0.5f;
            Texture2D topTexture = Main.shroomCapTexture;
            Texture2D woodTexture = Main.tileTexture[TileID.MushroomTrees];

            Rectangle top = new Rectangle(124, 0, 60, 42);
            Rectangle trunk = new Rectangle(0, 0, 18, 54);

            int topOffsetX = (int)Math.Round(22 * Scale);
            int topOffsetY = (int)Math.Round(42 * Scale);

            Width.Set(60, 0);
            Height.Set(66, 0);

            CalculatedStyle drawPos = GetDimensions();
            spriteBatch.Draw(topTexture, new Vector2(drawPos.X, drawPos.Y), top, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += (18 * 2) * Scale;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk, Color.White, 0, Vector2.Zero, Scale, 0, 0);

        }

        private int GetSpriteHeight()
        {
            TileObjectData data = TileObjectData.GetTileData(Tile);
            if(data != null)
            {
                int height = 0;
                foreach (int i in data.CoordinateHeights)
                {
                    height += i;
                }
                return height;
            }
            return 0;
        }
        private int GetSpriteWidth()
        {
            TileObjectData data = TileObjectData.GetTileData(Tile);
            return data == null ? 0 : data.Width * data.CoordinateWidth;
        }
        public void Set(Point pos, Tile tile, int itemId = -1, float scale = 1)
        {
            Tile = tile;
            Pos = pos;
            ItemId = itemId;
            Scale = scale;
        }

    }
}
