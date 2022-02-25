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

namespace Twaila.Util
{
    internal class TreeUtil
    {
        public static Texture2D GetImageForVanillaTree(SpriteBatch spriteBatch, int woodType, int depth)
        {
            int size = 20;
            Texture2D topTexture = Main.treeTopTexture[0];
            Texture2D woodTexture = Main.tileTexture[TileID.Trees];
            Texture2D branchTexture = Main.treeBranchTexture[0];
            SetTexturesForTrees(woodType, depth, ref topTexture, ref woodTexture, ref branchTexture);
            Rectangle top = new Rectangle(82, 0, size * 4, size * 4);
            Rectangle trunk1 = new Rectangle(44, 108, size, size);
            Rectangle trunk2 = new Rectangle(88, 42, size, size);
            Rectangle trunk3 = new Rectangle(66, 66, size, size);
            Rectangle leftBranch = new Rectangle(0, 42, size * 2, size * 2);
            Rectangle rightBranch = new Rectangle(42, 42, size * 2, size * 2);
            Rectangle bottomMiddle = new Rectangle(88, 154, size, size);
            Rectangle bottomLeft = new Rectangle(44, 176, size, size);
            Rectangle bottomRight = new Rectangle(22, 154, size, size);
            int topOffsetX = 30;
            int topOffsetY = 78;

            switch (woodType)
            {
                case ItemID.RichMahogany:
                    if (depth <= Main.worldSurface) // underground jungle
                    {
                        top = new Rectangle(236, 4, 112, 92);
                        topOffsetX = 42;
                        topOffsetY = 90;
                        break;
                    }
                    else // jungle
                    {
                        top = new Rectangle(0, 0, 114, 94);
                        topOffsetX = 46;
                        topOffsetY = 92;
                    }
                    break;
                case ItemID.Pearlwood: // hallow
                    top = new Rectangle(84, 22, 76, 118);
                    topOffsetX = 28;
                    topOffsetY = 116;
                    break;
            }

            return BuildImageForTrees(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, trunk3, leftBranch, rightBranch, bottomMiddle,
                bottomLeft, bottomRight, topTexture, woodTexture, branchTexture);
        }

        public static Texture2D GetImageForModdedTree(SpriteBatch spriteBatch, int treeDirt)
        {
            int size = 20;
            int unimplemented = 0;
            int frame = 0, fWidth = 82, fHeight = 80, xOffset = 30, yOffset = 78;
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

            return BuildImageForTrees(spriteBatch, xOffset, yOffset, top, trunk1, trunk2, trunk3, leftBranch, rightBranch, bottomMiddle,
                bottomLeft, bottomRight, topTexture, woodTexture, branchTexture);
        }

        public static Texture2D GetImageForPalmTree(SpriteBatch spriteBatch, int palmTreeWood)
        {
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

            int topOffsetX = 30;
            int topOffsetY = 78;

            return BuildImageForPalmTrees(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, bottom, topTexture, woodTexture);
        }

        public static Texture2D GetImageForModdedPalmTree(SpriteBatch spriteBatch, int palmTreeSand)
        {
            int size = 20;
            Tile sandTile = new Tile();
            sandTile.active(true);
            sandTile.type = (ushort)palmTreeSand;
            Texture2D woodTexture = TileLoader.GetPalmTreeTexture(sandTile);
            Texture2D topTexture = TileLoader.GetPalmTreeTopTextures(palmTreeSand);

            Rectangle top = new Rectangle(0, 0, size * 4, size * 4);
            Rectangle trunk1 = new Rectangle(0, 0, size, size);
            Rectangle trunk2 = new Rectangle(42, 0, size, size);
            Rectangle bottom = new Rectangle(66, 0, size, size);

            int topOffsetX = 30;
            int topOffsetY = 78;

            return BuildImageForPalmTrees(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, bottom, topTexture, woodTexture);
        }

        public static Texture2D GetImageForMushroomTree(SpriteBatch spriteBatch)
        {
            Texture2D topTexture = Main.shroomCapTexture;
            Texture2D woodTexture = Main.tileTexture[TileID.MushroomTrees];

            Rectangle top = new Rectangle(124, 0, 60, 42);
            Rectangle trunk = new Rectangle(0, 0, 18, 54);

            int topOffsetX = 22;
            int topOffsetY = 42;

            Point drawPos = Point.Zero;
            TextureBuilder builder = new TextureBuilder();
            builder.AddComponent(top, topTexture, drawPos);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            builder.AddComponent(trunk, woodTexture, drawPos);
            drawPos.Y += 36;
            builder.AddComponent(trunk, woodTexture, drawPos);
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        public static int GetTreeWood(int treeDirt)
        {
            switch (treeDirt)
            {
                case TileID.CorruptGrass:
                case TileID.Ebonsand:
                    return ItemID.Ebonwood;
                case TileID.JungleGrass:
                    return ItemID.RichMahogany;
                case TileID.HallowedGrass:
                case TileID.Pearlsand:
                    return ItemID.Pearlwood;
                case TileID.SnowBlock:
                    return ItemID.BorealWood;
                case TileID.FleshGrass:
                case TileID.Crimsand:
                    return ItemID.Shadewood;
                case TileID.MushroomGrass:
                    return ItemID.GlowingMushroom;
                case TileID.Grass:
                    return ItemID.Wood;
                case TileID.Sand:
                    return ItemID.PalmWood;
            }
            int wood = -1;
            TileLoader.DropTreeWood(treeDirt, ref wood);
            TileLoader.DropPalmTreeWood(treeDirt, ref wood);
            return wood;
        }
        public static int GetTreeDirt(int x, int y, Tile tile)
        {
            if (tile.type != TileID.Trees)
            {
                return -1;
            }
            if (Main.tile[x - 1, y].type == TileID.Trees && Main.tile[x, y + 1].type != TileID.Trees && Main.tile[x, y - 1].type != TileID.Trees)
            {
                x--;
            }
            if (Main.tile[x + 1, y].type == TileID.Trees && Main.tile[x, y + 1].type != TileID.Trees && Main.tile[x, y - 1].type != TileID.Trees)
            {
                x++;
            }
            do
            {
                y += 1;
            } while (Main.tile[x, y].type == TileID.Trees && Main.tile[x, y].active());

            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }
        public static int GetPalmTreeSand(int x, int y, Tile tile)
        {
            if (tile.type != TileID.PalmTree)
            {
                return -1;
            }
            do
            {
                y += 1;
            } while (Main.tile[x, y].type == TileID.PalmTree && Main.tile[x, y].active());

            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }
        public static int GetSaplingTile(int x, int y, Tile tile)
        {
            if (!TileLoader.IsSapling(tile.type))
            {
                return -1;
            }
            do
            {
                y++;
            } while (TileLoader.IsSapling(Main.tile[x, y].type) && Main.tile[x, y].active());

            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }

        private static Texture2D BuildImageForTrees(SpriteBatch spriteBatch, int topOffsetX, int topOffsetY, Rectangle top, Rectangle trunk1,
            Rectangle trunk2, Rectangle trunk3, Rectangle leftBranch, Rectangle rightBranch, Rectangle bottomMiddle,
            Rectangle bottomLeft, Rectangle bottomRight, Texture2D topTexture, Texture2D woodTexture, Texture2D branchTexture)
        {
            int unit = 16;
            TextureBuilder builder = new TextureBuilder();
            Point drawPos = Point.Zero;
            builder.AddComponent(top, topTexture, drawPos);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            builder.AddComponent(trunk1, woodTexture, drawPos);
            drawPos.Y += unit;
            builder.AddComponent(trunk2, woodTexture, drawPos);
            builder.AddComponent(leftBranch, branchTexture, new Point(drawPos.X - 38, drawPos.Y - 12));
            drawPos.Y += unit;
            builder.AddComponent(trunk3, woodTexture, drawPos);
            builder.AddComponent(rightBranch, branchTexture, new Point(drawPos.X + 18, drawPos.Y - 12));
            drawPos.Y += unit;
            builder.AddComponent(bottomMiddle, woodTexture, drawPos);
            builder.AddComponent(bottomLeft, woodTexture, new Point(drawPos.X - unit, drawPos.Y));
            builder.AddComponent(bottomRight, woodTexture, new Point(drawPos.X + unit, drawPos.Y));
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        private static Texture2D BuildImageForPalmTrees(SpriteBatch spriteBatch, int topOffsetX, int topOffsetY, Rectangle top, Rectangle trunk1,
            Rectangle trunk2, Rectangle bottom, Texture2D topTexture, Texture2D woodTexture)
        {
            int unit = 16;
            TextureBuilder builder = new TextureBuilder();

            Point drawPos = Point.Zero;
            builder.AddComponent(top, topTexture, drawPos);
            drawPos.X += topOffsetX;
            drawPos.Y += topOffsetY;
            builder.AddComponent(trunk1, woodTexture, drawPos);
            drawPos.Y += unit;
            builder.AddComponent(trunk2, woodTexture, drawPos);
            drawPos.Y += unit;
            drawPos.X += 2;
            builder.AddComponent(trunk1, woodTexture, drawPos);
            drawPos.Y += unit;
            drawPos.X += 2;
            builder.AddComponent(trunk1, woodTexture, drawPos);
            drawPos.Y += unit;
            builder.AddComponent(bottom, woodTexture, drawPos);
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        private static void SetTexturesForTrees(int woodType, int depth, ref Texture2D topTexture, ref Texture2D woodTexture, ref Texture2D branchTexture)
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

        
    }
}
