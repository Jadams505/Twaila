using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.Util;

namespace Twaila.Context
{
    public class TreeContext : TileContext
    {
        public int TreeDirt { get; private set; }

        public TreeContext(Point pos) : base(pos)
        {
            TreeDirt = GetTreeDirt();
        }

        public override bool ContextChanged(TileContext other)
        {
            if(other is TreeContext otherTreeContext)
            {
                if(TreeDirt == otherTreeContext.TreeDirt)
                {
                    if(TreeDirt == TileID.JungleGrass)
                    {
                        return Math.Sign(Pos.Y - Main.worldSurface) != Math.Sign(otherTreeContext.Pos.Y - Main.worldSurface);
                    }
                    return false;
                }
            }
            return true;
        }

        public override TwailaTexture GetTileImage(SpriteBatch spriteBatch)
        {
            float scale = 0.5f;
            if (Tile.type == TileID.Trees)
            {
                if (TileLoader.CanGrowModTree(TreeDirt))
                {
                    return new TwailaTexture(TreeUtil.GetImageForModdedTree(spriteBatch, TreeDirt), scale);
                }
                int treeWood = TreeUtil.GetTreeWood(TreeDirt);
                if (treeWood != -1)
                {
                    return new TwailaTexture(TreeUtil.GetImageForVanillaTree(spriteBatch, treeWood, Pos.Y), scale);
                }
            }
            else if (Tile.type == TileID.MushroomTrees)
            {
                return new TwailaTexture(TreeUtil.GetImageForMushroomTree(spriteBatch), scale);
            }
            return null;
        }

        public override TwailaTexture GetItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return GetTileImage(spriteBatch);
        }

        public override string GetName(int itemId)
        {
            return NameUtil.GetNameForTree(this) ?? base.GetName(itemId);
        }
        private int GetTreeDirt()
        {
            if (Tile.type != TileID.Trees)
            {
                return -1;
            }
            int x = Pos.X, y = Pos.Y;
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
    }
}
