using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Twaila.Util;
using Twaila.Graphics;

namespace Twaila.Context
{
    public class PalmTreeContext : TileContext
    {
        public int PalmTreeSand { get; private set; }

        public PalmTreeContext(Point pos) : base(pos)
        {
            PalmTreeSand = GetPalmTreeSand();
        }

        public override bool ContentChanged(TileContext other)
        {
            if (other is PalmTreeContext otherPalmTreeContext)
            {
                if (PalmTreeSand == otherPalmTreeContext.PalmTreeSand)
                {
                    return false;
                }
            }
            return true;
        }

        protected override TwailaTexture GetTileImage(SpriteBatch spriteBatch)
        {
            if (Tile.TileType == TileID.PalmTree)
            {
                if (TileLoader.CanGrowModPalmTree(PalmTreeSand))
                {
                    return new TwailaTexture(TreeUtil.GetImageForModdedPalmTree(spriteBatch, PalmTreeSand), 0.5f);
                }
                int palmTreeWood = TreeUtil.GetTreeWood(PalmTreeSand);
                if (palmTreeWood != -1)
                {
                    return new TwailaTexture(TreeUtil.GetImageForPalmTree(spriteBatch, palmTreeWood), 0.5f);
                }
            }
            return null;
        }

        protected override TwailaTexture GetTileItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return GetTileImage(spriteBatch);
        }

        protected override string GetTileName(int itemId)
        {
            return NameUtil.GetNameForPalmTree(this) ?? base.GetTileName(itemId);
        }

        private int GetPalmTreeSand()
        {
            if (Tile.TileType != TileID.PalmTree)
            {
                return -1;
            }
            int y = Pos.Y;
            do
            {
                y += 1;
            } while (Main.tile[Pos.X, y].TileType == TileID.PalmTree && Main.tile[Pos.X, y].HasTile);

            if (!Main.tile[Pos.X, y].HasTile)
            {
                return -1;
            }
            return Main.tile[Pos.X, y].TileType;
        }
    }
}
