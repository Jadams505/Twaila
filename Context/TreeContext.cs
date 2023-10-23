using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.Util;

namespace Twaila.Context
{
    public class TreeContext : TileContext
    {
        protected int DirtId { get; private set; }

        public TreeContext(TwailaPoint pos) : base(pos)
        {
            DirtId = GetTreeDirt();
        }

        public static TreeContext CreateTreeContext(TwailaPoint pos)
        {
            Point tilePos = pos.BestTilePos();
            Tile tile = Framing.GetTileSafely(tilePos);

            if (!tile.HasTile || tile.TileType >= TileLoader.TileCount)
                return null;

            if (!TileUtil.IsTilePosInBounds(tilePos))
                return null;

            if ((tile.TileType == TileID.Trees || tile.TileType == TileID.MushroomTrees) && !TileUtil.IsTileBlockedByAntiCheat(tile, tilePos))
            {
                return new TreeContext(pos);
            }

            return null;
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
                return otherContext.DirtId != DirtId || (DirtId == TileID.JungleGrass && Math.Sign(BestTilePos.Y - Main.worldSurface) != Math.Sign(otherContext.BestTilePos.Y - Main.worldSurface));
            }
            return true;
        }

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            float scale = 0.5f;
            if (TileId == TileID.Trees)
            {
                if (TileLoader.CanGrowModTree(DirtId))
                {
                    Texture2D treeTexture = TreeUtil.GetImageForModdedTree(spriteBatch, DirtId);
                    return new TwailaRender(treeTexture, scale);
                }
                int treeWood = TreeUtil.GetTreeWood(DirtId);
                if (treeWood != -1)
                {
                    return new TwailaRender(TreeUtil.GetImageForVanillaTree(spriteBatch, treeWood, BestTilePos.Y), scale);
                }
            }
            else if (TileId == TileID.MushroomTrees)
            {
                return new TwailaRender(TreeUtil.GetImageForMushroomTree(spriteBatch), scale);
            }
            return new TwailaRender();
        }

        protected override string GetName()
        {
            string displayName = NameUtil.GetNameForTree(DirtId);
            string internalName = PlantLoader.Get<ModTree>(TileId, DirtId)?.GetType().Name;
            string fullName = PlantLoader.Get<ModTree>(TileId, DirtId)?.GetType().FullName;

            TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? base.GetName();
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(DirtId);
            return NameUtil.GetMod(mTile);
        }

        private int GetTreeDirt()
        {
            Point bestPos = BestTilePos;
            int x = bestPos.X, y = bestPos.Y;

            if (Framing.GetTileSafely(x - 1, y).TileType == TileID.Trees && Framing.GetTileSafely(x, y + 1).TileType != TileID.Trees && Framing.GetTileSafely(x, y - 1).TileType != TileID.Trees)
            {
                x--;
            }
            if (Framing.GetTileSafely(x + 1, y).TileType == TileID.Trees && Framing.GetTileSafely(x, y + 1).TileType != TileID.Trees && Framing.GetTileSafely(x, y - 1).TileType != TileID.Trees)
            {
                x++;
            }
            do
            {
                y += 1;
            } while (WorldGen.InWorld(x, y) && Framing.GetTileSafely(x, y).TileType == TileID.Trees && Framing.GetTileSafely(x, y).HasTile);

            if (!Framing.GetTileSafely(x, y).HasTile)
            {
                return -1;
            }
            return Framing.GetTileSafely(x, y).TileType;
        }
    }
}
