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
        protected int DirtId { get; private set; }

        public TreeContext(Point pos) : base(pos)
        {
            DirtId = GetTreeDirt();
        }

        public override void Update()
        {
            base.Update();
            DirtId = GetTreeDirt();
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(TreeContext))
            {
                TreeContext otherContext = (TreeContext)other;
                return otherContext.DirtId != DirtId || (DirtId == TileID.JungleGrass && Math.Sign(Pos.Y - Main.worldSurface) != Math.Sign(otherContext.Pos.Y - Main.worldSurface));
            }
            return true;
        }

        protected override TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            float scale = 0.5f;
            if (TileId == TileID.Trees)
            {
                if (TileLoader.CanGrowModTree(DirtId))
                {
                    Texture2D treeTexture = TreeUtil.GetImageForModdedTree(spriteBatch, DirtId);
                    return new TwailaTexture(treeTexture, scale);
                }
                int treeWood = TreeUtil.GetTreeWood(DirtId);
                if (treeWood != -1)
                {
                    return new TwailaTexture(TreeUtil.GetImageForVanillaTree(spriteBatch, treeWood, Pos.Y), scale);
                }
            }
            else if (TileId == TileID.MushroomTrees)
            {
                return new TwailaTexture(TreeUtil.GetImageForMushroomTree(spriteBatch), scale);
            }
            return null;
        }

        protected override string GetName()
        {
            return NameUtil.GetNameForTree(TileId, DirtId) ?? base.GetName();
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(DirtId);
            if (mTile != null)
            {
                return mTile.Mod.DisplayName;
            }
            return "Terraria";
        }

        private int GetTreeDirt()
        {
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
