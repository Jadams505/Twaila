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

        public override bool ContentChanged(TileContext other)
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

        protected override TwailaTexture GetTileImage(SpriteBatch spriteBatch, Tile tile)
        {
            float scale = 0.5f;
            if (tile.TileType == TileID.Trees)
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
            else if (tile.TileType == TileID.MushroomTrees)
            {
                return new TwailaTexture(TreeUtil.GetImageForMushroomTree(spriteBatch), scale);
            }
            return null;
        }

        protected override TwailaTexture GetTileItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return null;
        }

        protected override string GetTileName(Tile tile, int itemId)
        {
            return NameUtil.GetNameForTree(this) ?? base.GetTileName(tile, itemId);
        }

        private int GetTreeDirt()
        {
            if (Tile.TileId != TileID.Trees)
            {
                return -1;
            }
            int x = Pos.X, y = Pos.Y;
            if (Main.tile[x - 1, y].TileType == TileID.Trees && Main.tile[x, y + 1].TileType != TileID.Trees && Main.tile[x, y - 1].TileType != TileID.Trees)
            {
                x--;
            }
            if (Main.tile[x + 1, y].TileType == TileID.Trees && Main.tile[x, y + 1].TileType != TileID.Trees && Main.tile[x, y - 1].TileType != TileID.Trees)
            {
                x++;
            }
            do
            {
                y += 1;
            } while (Main.tile[x, y].TileType == TileID.Trees && Main.tile[x, y].HasTile);

            if (!Main.tile[x, y].HasTile)
            {
                return -1;
            }
            return Main.tile[x, y].TileType;
        }
    }
}
