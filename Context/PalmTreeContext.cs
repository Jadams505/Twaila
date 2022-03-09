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

        public override bool ContextChanged(TileContext other)
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

        public override TwailaTexture GetTileImage(SpriteBatch spriteBatch)
        {
            if (Tile.type == TileID.PalmTree)
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

        public override TwailaTexture GetItemImage(SpriteBatch spriteBatch, int itemId)
        {
            return GetTileImage(spriteBatch);
        }

        public override string GetName(int itemId)
        {
            return NameUtil.GetNameForPalmTree(this);
        }

        private int GetPalmTreeSand()
        {
            if (Tile.type != TileID.PalmTree)
            {
                return -1;
            }
            int y = Pos.Y;
            do
            {
                y += 1;
            } while (Main.tile[Pos.X, y].type == TileID.PalmTree && Main.tile[Pos.X, y].active());

            if (Main.tile[Pos.X, y] == null || !Main.tile[Pos.X, y].active())
            {
                return -1;
            }
            return Main.tile[Pos.X, y].type;
        }
    }
}
