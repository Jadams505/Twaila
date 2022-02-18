
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

        public Texture2D Image {get ; private set; }
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
                Texture2D buffer = new Texture2D(spriteBatch.GraphicsDevice, GetSpriteWidth(), GetSpriteHeight());
                Rectangle dim = GetDimensions().ToRectangle();
                int frameX = Tile.frameX / data.CoordinateFullWidth * data.CoordinateFullWidth;
                int frameY = Tile.frameY / data.CoordinateFullHeight * data.CoordinateFullHeight;
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
                for (int row = 0; row < data.Height; ++row)
                {
                    for (int col = 0; col < data.Width; ++col)
                    {
                        int width = data.CoordinateWidth, height = data.CoordinateHeights[row], lastHeight = data.CoordinateHeights[row - 1 > 0 ? row - 1 : 0];
                        Rectangle copyRectangle = new Rectangle(frameX + (width + data.CoordinatePadding) * col,
                            frameY + (lastHeight + data.CoordinatePadding) * row, width, height);
                        Rectangle pasteRectangle = new Rectangle(width * col, lastHeight * row, width, height);
                        if(!PopulateTextureBuffer(texture, copyRectangle, buffer, pasteRectangle))
                        {
                            return false;
                        }
                    }
                }
                Image = buffer;
                spriteBatch.Draw(Image, new Vector2(dim.X, dim.Y), new Rectangle(0, 0, Image.Width, Image.Height), Color.White, 0, Vector2.Zero, Scale, 0, 0);
                return true;
            }
            return false;
        }
        private bool DrawFromTile(SpriteBatch spriteBatch)
        {
            Rectangle dim = GetDimensions().ToRectangle();
            int size = 16;
            int padding = 2;
            Texture2D texture = GetTileTexture();
            
            if (texture != null && !texture.Equals(TextureManager.BlankTexture))
            {
                Texture2D buffer = new Texture2D(spriteBatch.GraphicsDevice, size * 2, size * 2);
                SetSizeFromTile();
                for(int row = 0; row < 2; ++row)
                {
                    for(int col = 0; col < 2; ++col)
                    {
                        Rectangle copyRectangle = new Rectangle(col * (size + padding), 54 + row * (size + padding), size, size);
                        Rectangle pasteRectangle = new Rectangle(size * col, size * row, size, size);
                        if (!PopulateTextureBuffer(texture, copyRectangle, buffer, pasteRectangle))
                        {
                            return false;
                        }
                    }
                }
                Image = buffer;
                spriteBatch.Draw(Image, new Vector2(dim.X, dim.Y), new Rectangle(0, 0, Image.Width, Image.Height), Color.White, 0, Vector2.Zero, Scale, 0, 0);
                return true;
            }
            return false;
        }

        private bool PopulateTextureBuffer(Texture2D fullTexture, Rectangle copyRectangle, Texture2D buffer, Rectangle pasteRectangle)
        {
            Color[] textureAsArray = new Color[copyRectangle.Width * copyRectangle.Height];
            if (copyRectangle.X + copyRectangle.Width > fullTexture.Width || copyRectangle.Y + copyRectangle.Height > fullTexture.Height)
            {
                return false;
            }
            fullTexture.GetData(0, copyRectangle, textureAsArray, 0, textureAsArray.Length);
            buffer.SetData(0, pasteRectangle, textureAsArray, 0, textureAsArray.Length);
            return true;
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
                    spriteBatch.Draw(position: new Vector2((int)GetDimensions().X, (int)GetDimensions().Y) + itemTexture.Size() * (1f - Scale) / 2f, texture: itemTexture, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: Scale, effects: SpriteEffects.None, layerDepth: 0f);
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
                int treeDirt = TwailaUtil.GetTreeDirt(Pos.X, Pos.Y, Tile);
                if(treeDirt == -1)
                {
                    return false;
                } 
                if(TileLoader.CanGrowModTree(treeDirt))
                {
                    DrawModdedTree(spriteBatch, treeDirt);
                    return true;
                }
                int treeWood = TwailaUtil.GetTreeWood(treeDirt);
                if (treeWood != -1)
                {
                    DrawTree(spriteBatch, treeWood);
                    return true;
                }
            }
            else if (Tile.type == TileID.PalmTree) 
            { 
                int palmTreeSand = TwailaUtil.GetPalmTreeSand(Pos.X, Pos.Y, Tile);
                if(palmTreeSand == -1)
                {
                    return false;
                }
                if (TileLoader.CanGrowModPalmTree(palmTreeSand))
                {
                    DrawModdedPalmTree(spriteBatch, palmTreeSand);
                    return true;
                }
                int palmTreeWood = TwailaUtil.GetTreeWood(palmTreeSand);
                if(palmTreeWood != -1)
                {
                    DrawPalmTree(spriteBatch, palmTreeWood);
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

        private void DrawTree(SpriteBatch spriteBatch, int woodType)
        {
            Scale = 0.5f;
            int size = 20;
            
            
            Texture2D topTexture = Main.treeTopTexture[0];
            Texture2D woodTexture = Main.tileTexture[TileID.Trees];
            Texture2D branchTexture = Main.treeBranchTexture[0];
            SetTexturesForTree(woodType, Pos.Y, ref topTexture, ref woodTexture, ref branchTexture);
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

            switch (woodType)
            {
                case ItemID.RichMahogany: 
                    if(Pos.Y <= Main.worldSurface) // underground jungle
                    {
                        Width.Set(56, 0);
                        Height.Set(79, 0);
                        top = new Rectangle(236, 4, 112, 92);
                        topOffsetX = (int)Math.Round(42 * Scale);
                        topOffsetY = (int)Math.Round(90 * Scale);
                        break;
                    }
                    else // jungle
                    {
                        Width.Set(56, 0);
                        Height.Set(80, 0);
                        top = new Rectangle(0, 0, 114, 94);
                        topOffsetX = (int)Math.Round(46 * Scale);
                        topOffsetY = (int)Math.Round(92 * Scale);
                    }
                    break;
                case ItemID.Pearlwood: // hallow
                    Width.Set(40, 0);
                    Height.Set(92, 0);
                    top = new Rectangle(84, 22, 76, 118);
                    topOffsetX = (int)Math.Round(28 * Scale);
                    topOffsetY = (int)Math.Round(116 * Scale);
                    break;
            }

            DrawTree(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, trunk3, leftBranch, rightBranch, bottomMiddle,
                bottomLeft, bottomRight, topTexture, woodTexture, branchTexture);
        }

        private void DrawModdedTree(SpriteBatch spriteBatch, int treeDirt)
        {
            Scale = 0.5f;
            int size = 20;
            int unimplemented = 0;
            int frame = 0, fWidth = 82, fHeight = 80, xOffset = (int)Math.Round(30 * Scale), yOffset = (int)Math.Round(78 * Scale);
            Tile dirtTile = new Tile();
            dirtTile.active(true);
            dirtTile.type = (ushort)treeDirt;
            Texture2D topTexture = TileLoader.GetTreeTopTextures(treeDirt, 82, 0, ref frame, ref fWidth, ref fHeight, ref xOffset, ref yOffset);
            Texture2D woodTexture = TileLoader.GetTreeTexture(dirtTile);
            Texture2D branchTexture = TileLoader.GetTreeBranchTextures(treeDirt, 0, 0, 0, ref unimplemented);
            Rectangle top = new Rectangle(frame * fWidth, 0, fWidth, fHeight);
            Rectangle trunk1 = new Rectangle(44, 108, size, size);
            Rectangle trunk2 = new Rectangle(88, 42, size, size);
            Rectangle trunk3 = new Rectangle(66, 66, size, size);
            Rectangle leftBranch = new Rectangle(0, 42, size * 2, size * 2);
            Rectangle rightBranch = new Rectangle(42, 42, size * 2, size * 2);
            Rectangle bottomMiddle = new Rectangle(88, 154, size, size);
            Rectangle bottomLeft = new Rectangle(44, 176, size, size);
            Rectangle bottomRight = new Rectangle(22, 154, size, size);

            Width.Set(40, 0);
            Height.Set(74, 0);

            DrawTree(spriteBatch, xOffset, yOffset, top, trunk1, trunk2, trunk3, leftBranch, rightBranch, bottomMiddle,
                bottomLeft, bottomRight, topTexture, woodTexture, branchTexture);
        }

        private void DrawTree(SpriteBatch spriteBatch, int topOffsetX, int topOffsetY, Rectangle top, Rectangle trunk1,
            Rectangle trunk2, Rectangle trunk3, Rectangle leftBranch, Rectangle rightBranch, Rectangle bottomMiddle,
            Rectangle bottomLeft, Rectangle bottomRight, Texture2D topTexture, Texture2D woodTexture, Texture2D branchTexture)
        {
            int unit = (int)Math.Round(16 * Scale);
            Rectangle drawPos = GetDimensions().ToRectangle();

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

        private void DrawPalmTree(SpriteBatch spriteBatch, int palmTreeWood)
        {
            Scale = 0.5f;
            int size = 20;
            int palmTreeType = 0;
            switch (palmTreeWood)
            {
                case ItemID.PalmWood:
                    palmTreeType = 0;
                    break;
                case ItemID.Shadewood:
                    palmTreeType = 1;
                    break;
                case ItemID.Pearlwood:
                    palmTreeType = 2;
                    break;
                case ItemID.Ebonwood:
                    palmTreeType = 3;
                    break;
            }
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

            DrawPalmTree(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, bottom, topTexture, woodTexture);
        }

        private void DrawModdedPalmTree(SpriteBatch spriteBatch, int palmTreeSand)
        {
            Scale = 0.5f;
            int size = 20;
            Tile sandTile = new Tile();
            sandTile.active(true);
            sandTile.type = (ushort)palmTreeSand;
            Texture2D woodTexture = TileLoader.GetPalmTreeTexture(sandTile);
            Texture2D topTexture = TileLoader.GetPalmTreeTopTextures(palmTreeSand);

            Rectangle top = new Rectangle(0,0, size * 4, size * 4);
            Rectangle trunk1 = new Rectangle(0, 0, size, size);
            Rectangle trunk2 = new Rectangle(42, 0, size, size);
            Rectangle bottom = new Rectangle(66, 0, size, size);

            int topOffsetX = (int)Math.Round(30 * Scale);
            int topOffsetY = (int)Math.Round(78 * Scale);

            Width.Set((int)(78 * Scale), 0);
            Height.Set((int)(160 * Scale), 0);

            DrawPalmTree(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, bottom, topTexture, woodTexture);
        }

        private void DrawPalmTree(SpriteBatch spriteBatch, int topOffsetX, int topOffsetY, Rectangle top, Rectangle trunk1, 
            Rectangle trunk2, Rectangle bottom, Texture2D topTexture, Texture2D woodTexture)
        {
            int unit = (int)Math.Round(16 * Scale);

            Rectangle drawPos = GetDimensions().ToRectangle();
            spriteBatch.Draw(topTexture, new Vector2(drawPos.X, drawPos.Y), top, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk1, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk2, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            drawPos.X += (int)(2 * Scale);
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk1, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += unit;
            drawPos.X += (int)(2 * Scale);
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

            Width.Set(30, 0);
            Height.Set(66, 0);

            Rectangle drawPos = GetDimensions().ToRectangle();
            spriteBatch.Draw(topTexture, new Vector2(drawPos.X, drawPos.Y), top, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            spriteBatch.Draw(woodTexture, new Vector2(drawPos.X, drawPos.Y), trunk, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            drawPos.Y += (int)(18 * 2 * Scale);
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

        private void SetTexturesForTree(int woodType, int depth, ref Texture2D topTexture, ref Texture2D woodTexture, ref Texture2D branchTexture)
        {
            switch (woodType)
            {
                case ItemID.Ebonwood:
                    topTexture = Main.treeTopTexture[1];
                    woodTexture = Main.woodTexture[0];
                    branchTexture = Main.treeBranchTexture[1];
                    break;
                case ItemID.RichMahogany:
                    if (depth >= Main.worldSurface)
                    {
                        topTexture = Main.treeTopTexture[13];
                        woodTexture = Main.woodTexture[5];
                        branchTexture = Main.treeBranchTexture[13];
                        break;
                    }
                    topTexture = Main.treeTopTexture[2];
                    woodTexture = Main.woodTexture[1];
                    branchTexture = Main.treeBranchTexture[2];
                    break;
                case ItemID.Pearlwood:
                    topTexture = Main.treeTopTexture[3];
                    woodTexture = Main.woodTexture[2];
                    branchTexture = Main.treeBranchTexture[3];
                    break;
                case ItemID.BorealWood:
                    topTexture = Main.treeTopTexture[12];
                    woodTexture = Main.woodTexture[3];
                    branchTexture = Main.treeBranchTexture[12];
                    break;
                case ItemID.Shadewood:
                    topTexture = Main.treeTopTexture[5];
                    woodTexture = Main.woodTexture[4];
                    branchTexture = Main.treeBranchTexture[5];
                    return;
                case ItemID.GlowingMushroom:
                    topTexture = Main.treeTopTexture[14];
                    woodTexture = Main.woodTexture[6];
                    branchTexture = Main.treeBranchTexture[14];
                    break;
                case ItemID.Wood:
                    topTexture = Main.treeTopTexture[0];
                    woodTexture = Main.tileTexture[TileID.Trees];
                    branchTexture = Main.treeBranchTexture[0];
                    break;
            }
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
