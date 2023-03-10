using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Graphics;

namespace Twaila.Util
{
    public class TreeUtil
    {
        public static Texture2D GetImageForVanillaTree(SpriteBatch spriteBatch, int woodType, int depth)
        {
            int size = 20;
            Texture2D topTexture = TextureAssets.TreeTop[0].ForceVanillaLoad();
            Texture2D woodTexture = ImageUtil.GetTileTexture(TileID.Trees);
            Texture2D branchTexture = TextureAssets.TreeBranch[0].ForceVanillaLoad();
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
                    if (depth <= Main.worldSurface) // jungle
                    {
                        top = new Rectangle(236, 4, 112, 92);
                        topOffsetX = 42;
                        topOffsetY = 90;
                        break;
                    }
                    else // underground jungle
                    {
                        top = new Rectangle(0, 2, 114, 94);
                        topOffsetX = 48;
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

        public static Texture2D GetImageForVanityTree(SpriteBatch spriteBatch, int tileId)
        {
            int size = 20;
            Texture2D topTexture;
            Texture2D woodTexture = ImageUtil.GetTileTexture(tileId);
            Texture2D branchTexture;
            Rectangle top;
            Rectangle trunk1 = new Rectangle(44, 108, size, size);
            Rectangle trunk2 = new Rectangle(88, 42, size, size);
            Rectangle trunk3 = new Rectangle(66, 66, size, size);
            Rectangle leftBranch = new Rectangle(0, 42, size * 2, size * 2);
            Rectangle rightBranch = new Rectangle(42, 42, size * 2, size * 2);
            Rectangle bottomMiddle = new Rectangle(88, 154, size, size);
            Rectangle bottomLeft = new Rectangle(44, 176, size, size);
            Rectangle bottomRight = new Rectangle(22, 154, size, size);
            int topOffsetX;
            int topOffsetY;

            switch (tileId)
            {
                case TileID.VanityTreeSakura:
                    top = new Rectangle(125, 16, 106, 86);
                    topTexture = TextureAssets.TreeTop[29].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[29].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 78;
                    break;
                case TileID.VanityTreeYellowWillow:
                    top = new Rectangle(124, 0, 116, 94);
                    topTexture = TextureAssets.TreeTop[30].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[30].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 92;
                    break;
                default:
                    return null;
            }

            return BuildImageForTrees(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, trunk3, leftBranch, rightBranch, bottomMiddle,
                bottomLeft, bottomRight, topTexture, woodTexture, branchTexture);
        }

        public static Texture2D GetImageForAshTree(SpriteBatch spriteBatch, int tileId)
        {
            if(tileId == TileID.TreeAsh)
            {
                int size = 20;
                Texture2D topTexture = TextureAssets.TreeTop[31].ForceVanillaLoad();
                Texture2D woodTexture = ImageUtil.GetTileTexture(tileId);
                Texture2D branchTexture = TextureAssets.TreeBranch[31].ForceVanillaLoad();
                Rectangle top = new Rectangle(120, 0, 114, 96);
                Rectangle trunk1 = new Rectangle(44, 108, size, size);
                Rectangle trunk2 = new Rectangle(88, 42, size, size);
                Rectangle trunk3 = new Rectangle(66, 66, size, size);
                Rectangle leftBranch = new Rectangle(0, 42, size * 2, size * 2);
                Rectangle rightBranch = new Rectangle(42, 42, size * 2, size * 2);
                Rectangle bottomMiddle = new Rectangle(88, 154, size, size);
                Rectangle bottomLeft = new Rectangle(44, 176, size, size);
                Rectangle bottomRight = new Rectangle(22, 154, size, size);
                int topOffsetX = 46;
                int topOffsetY = 94;

                return BuildImageForTrees(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, trunk3, leftBranch, rightBranch, bottomMiddle,
                    bottomLeft, bottomRight, topTexture, woodTexture, branchTexture);
            }
            return null;
        }

        public static Texture2D GetImageForGemTree(SpriteBatch spriteBatch, int tileId)
        {
            int size = 20;
            Texture2D topTexture;
            Texture2D woodTexture = ImageUtil.GetTileTexture(tileId);
            Texture2D branchTexture;
            Rectangle top;
            Rectangle trunk1 = new Rectangle(44, 108, size, size);
            Rectangle trunk2 = new Rectangle(88, 42, size, size);
            Rectangle trunk3 = new Rectangle(66, 66, size, size);
            Rectangle leftBranch = new Rectangle(0, 42, size * 2, size * 2);
            Rectangle rightBranch = new Rectangle(42, 42, size * 2, size * 2);
            Rectangle bottomMiddle = new Rectangle(88, 154, size, size);
            int topOffsetX;
            int topOffsetY;
            switch (tileId)
            {
                case TileID.TreeTopaz:
                    top = new Rectangle(2, 0, 114, 96);
                    topTexture = TextureAssets.TreeTop[22].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[22].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 94;
                    break;
                case TileID.TreeAmethyst:
                    top = new Rectangle(2, 0, 114, 96);
                    topTexture = TextureAssets.TreeTop[23].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[23].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 94;
                    break;
                case TileID.TreeSapphire:
                    top = new Rectangle(2, 0, 114, 96);
                    topTexture = TextureAssets.TreeTop[24].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[24].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 94;
                    break;
                case TileID.TreeEmerald:
                    top = new Rectangle(2, 0, 114, 96);
                    topTexture = TextureAssets.TreeTop[25].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[25].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 94;
                    break;
                case TileID.TreeRuby:
                    top = new Rectangle(2, 0, 114, 96);
                    topTexture = TextureAssets.TreeTop[26].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[26].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 94;
                    break;
                case TileID.TreeDiamond:
                    top = new Rectangle(2, 0, 114, 96);
                    topTexture = TextureAssets.TreeTop[27].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[27].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 94;
                    break;
                case TileID.TreeAmber:
                    top = new Rectangle(2, 0, 114, 96);
                    topTexture = TextureAssets.TreeTop[28].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[28].ForceVanillaLoad();
                    topOffsetX = 44;
                    topOffsetY = 94;
                    break;
                default:
                    return null;
            }
            return BuildImageForGemTree(spriteBatch, topOffsetX, topOffsetY, top, trunk1, trunk2, trunk3, leftBranch, rightBranch, 
                bottomMiddle, topTexture, woodTexture, branchTexture);
        }

        public static Texture2D GetImageForModdedTree(SpriteBatch spriteBatch, int treeDirt)
        {
            int size = 20;
            int frame = 0, fWidth = 80, fHeight = 80, xOffset = 30, yOffset = 78;

            ModTree mTree = PlantLoader.Get<ModTree>(TileID.Trees, treeDirt);

            if(mTree == null)
            {
                return null;
            }

            Texture2D topTexture = mTree.GetTopTextures().Value;
            Texture2D woodTexture = mTree.GetTexture().Value;
            Texture2D branchTexture = mTree.GetBranchTextures().Value;

            Rectangle top = new Rectangle(frame * fWidth, 0, fWidth, fHeight);
            Rectangle trunk1 = new Rectangle(44, 110, size, size);
            Rectangle trunk2 = new Rectangle(88, 44, size, size);
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
            Texture2D woodTexture = ImageUtil.GetTileTexture(TileID.PalmTree);
            Texture2D topTexture = TextureAssets.TreeTop[15].ForceVanillaLoad();

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

            ModPalmTree mPalmTree = PlantLoader.Get<ModPalmTree>(TileID.PalmTree, palmTreeSand);

            if(mPalmTree == null)
            {
                return null;
            }

            Texture2D woodTexture = mPalmTree.GetTexture().Value;
            Texture2D topTexture = mPalmTree.GetTopTextures().Value;

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
            Texture2D topTexture = TextureAssets.ShroomCap.ForceVanillaLoad();
            Texture2D woodTexture = ImageUtil.GetTileTexture(TileID.MushroomTrees);

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

        public static Texture2D GetImageForCactus(SpriteBatch spriteBatch, int cactusSand, bool modded)
        {
            int size = 16;
            int padding = 2;
            Texture2D cactusTexture = modded ? TileLoader.GetCactusTexture(cactusSand) : ImageUtil.GetTileTexture(TileID.Cactus);
            if (!modded)
            {
                switch (cactusSand)
                {
                    case TileID.Ebonsand:
                        cactusTexture = TextureAssets.EvilCactus.ForceVanillaLoad();
                        break;
                    case TileID.Pearlsand:
                        cactusTexture = TextureAssets.GoodCactus.ForceVanillaLoad();
                        break;
                    case TileID.Crimsand:
                        cactusTexture = TextureAssets.CrimsonCactus.ForceVanillaLoad();
                        break;
                    default:
                        cactusTexture = ImageUtil.GetTileTexture(TileID.Cactus);
                        break;
                }
            }

            Rectangle stemTop = new Rectangle(0, 0, size, size);
            Rectangle stemMiddle = new Rectangle(90, 36, size, size);
            Rectangle left = new Rectangle(54, 36, size, size);
            Rectangle topLeft = new Rectangle(54, 0, size, size);
            Rectangle right = new Rectangle(36, 36, size, size);
            Rectangle topRight = new Rectangle(36, 0, size, size);
            Rectangle stemBottom = new Rectangle(0, 2 * (size + padding), size, size);

            Point drawPos = Point.Zero;
            TextureBuilder builder = new TextureBuilder();
            drawPos.X += size / 2;
            builder.AddComponent(stemTop, cactusTexture, drawPos);
            drawPos.Y += size;
            builder.AddComponent(stemMiddle, cactusTexture, drawPos);
            drawPos.X -= size;
            builder.AddComponent(left, cactusTexture, drawPos);
            drawPos.Y -= size;
            builder.AddComponent(topLeft, cactusTexture, drawPos);
            drawPos.X += size * 2;
            builder.AddComponent(topRight, cactusTexture, drawPos);
            drawPos.Y += size;
            builder.AddComponent(right, cactusTexture, drawPos);
            drawPos.Y += size;
            drawPos.X -= size;
            builder.AddComponent(stemBottom, cactusTexture, drawPos);
            return builder.Build(spriteBatch.GraphicsDevice);
        }

        public static Texture2D GetImageForBamboo(SpriteBatch spriteBatch, int tileId)
        {
            if(tileId == TileID.Bamboo)
            {
                Texture2D texture = ImageUtil.GetTileTexture(tileId);
                int size = 16;
                int padding = 2;

                int bottomStyle = 1;
                int middle1Style = 12;
                int middle2Style = 10;
                int topStyle = 17;

                Point drawPos = Point.Zero;

                TextureBuilder builder = new TextureBuilder();
                builder.AddComponent(new Rectangle(topStyle * (size + padding), 0, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle(middle1Style * (size + padding), 0, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle(middle2Style * (size + padding), 0, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle(bottomStyle * (size + padding), 0, size, size), texture, drawPos);
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static Texture2D GetImageForSeaweed(SpriteBatch spriteBatch, int tileId)
        {
            if (tileId == TileID.Seaweed)
            {
                Texture2D texture = ImageUtil.GetTileTexture(tileId);
                int size = 16;
                int padding = 2;

                int bottomStyle = 1;
                int middle1Style = 5;
                int middle2Style = 4;
                int topStyle = 12;

                Point drawPos = Point.Zero;

                TextureBuilder builder = new TextureBuilder();
                builder.AddComponent(new Rectangle(topStyle * (size + padding), 0, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle(middle1Style * (size + padding), 0, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle(middle2Style * (size + padding), 0, size, size), texture, drawPos);
                drawPos.Y += size;
                builder.AddComponent(new Rectangle(bottomStyle * (size + padding), 0, size, size), texture, drawPos);
                return builder.Build(spriteBatch.GraphicsDevice);
            }
            return null;
        }

        public static int GetTreeWood(int treeDirt)
        {
            switch (treeDirt)
            {
                case TileID.CorruptGrass:
                case TileID.Ebonsand:
                case TileID.CorruptJungleGrass:
                    return ItemID.Ebonwood;
                case TileID.JungleGrass:
                    return ItemID.RichMahogany;
                case TileID.HallowedGrass:
                case TileID.Pearlsand:
                case TileID.GolfGrassHallowed:
                    return ItemID.Pearlwood;
                case TileID.SnowBlock:
                    return ItemID.BorealWood;
                case TileID.CrimsonGrass:
                case TileID.Crimsand:
                case TileID.CrimsonJungleGrass:
                    return ItemID.Shadewood;
                case TileID.MushroomGrass:
                    return ItemID.GlowingMushroom;
                case TileID.Grass:
                case TileID.GolfGrass:
                    return ItemID.Wood;
                case TileID.Sand:
                    return ItemID.PalmWood;
                case TileID.AshGrass:
                    return ItemID.AshWood;
            }
            int wood = -1;
            TileLoader.DropTreeWood(treeDirt, ref wood);
            TileLoader.DropPalmTreeWood(treeDirt, ref wood);
            return wood;
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

        private static Texture2D BuildImageForGemTree(SpriteBatch spriteBatch, int topOffsetX, int topOffsetY, Rectangle top, Rectangle trunk1,
            Rectangle trunk2, Rectangle trunk3, Rectangle leftBranch, Rectangle rightBranch, Rectangle bottomMiddle, Texture2D topTexture, Texture2D woodTexture, Texture2D branchTexture)
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
                    topTexture = TextureAssets.TreeTop[1].ForceVanillaLoad();
                    woodTexture = TextureAssets.Wood[0].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[1].ForceVanillaLoad();
                    break;
                case ItemID.RichMahogany:
                    if (depth >= Main.worldSurface)
                    {
                        topTexture = TextureAssets.TreeTop[13].ForceVanillaLoad();
                        woodTexture = TextureAssets.Wood[5].ForceVanillaLoad();
                        branchTexture = TextureAssets.TreeBranch[13].ForceVanillaLoad();
                        break;
                    }
                    topTexture = TextureAssets.TreeTop[2].ForceVanillaLoad();
                    woodTexture = TextureAssets.Wood[1].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[2].ForceVanillaLoad();
                    break;
                case ItemID.Pearlwood:
                    topTexture = TextureAssets.TreeTop[3].ForceVanillaLoad();
                    woodTexture = TextureAssets.Wood[2].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[3].ForceVanillaLoad();
                    break;
                case ItemID.BorealWood:
                    topTexture = TextureAssets.TreeTop[12].ForceVanillaLoad();
                    woodTexture = TextureAssets.Wood[3].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[12].ForceVanillaLoad();
                    break;
                case ItemID.Shadewood:
                    topTexture = TextureAssets.TreeTop[5].ForceVanillaLoad();
                    woodTexture = TextureAssets.Wood[4].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[5].ForceVanillaLoad();
                    return;
                case ItemID.GlowingMushroom:
                    topTexture = TextureAssets.TreeTop[14].ForceVanillaLoad();
                    woodTexture = TextureAssets.Wood[6].ForceVanillaLoad();
                    branchTexture = TextureAssets.TreeBranch[14].ForceVanillaLoad();
                    break;
                case ItemID.Wood:
                    topTexture = TextureAssets.TreeTop[0].ForceVanillaLoad();
                    woodTexture = ImageUtil.GetTileTexture(TileID.Trees);
                    branchTexture = TextureAssets.TreeBranch[0].ForceVanillaLoad();
                    break;
            }
        }
    }
}
