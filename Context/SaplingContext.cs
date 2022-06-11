using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace Twaila.Context
{
    public class SaplingContext : TileContext
    {
        public int DirtId { get; private set; }

        public SaplingContext(Point pos) : base(pos)
        {
            DirtId = GetSaplingTile();
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(SaplingContext))
            {
                SaplingContext otherContext = (SaplingContext)other;
                return otherContext.DirtId != DirtId || StyleChanged(otherContext);
            }
            return true;
        }

        public override void Update()
        {
            base.Update();
            DirtId = GetSaplingTile();
        }

        protected override TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return new TwailaTexture(ImageUtil.GetImageFromTileDrawing(spriteBatch, tile, Pos.X, Pos.Y));
        }

        protected override string GetName()
        {
            return NameUtil.GetNameForSapling(TileId, DirtId);
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(TileId);
            if (mTile != null)
            {
                return mTile.Mod.DisplayName;
            }
            return "Terraria";
        }

        private int GetSaplingTile()
        {
            int y = Pos.Y;
            do
            {
                y++;
            } while (TileID.Sets.TreeSapling[Main.tile[Pos.X, y].TileType] && Main.tile[Pos.X, y].HasTile);

            if (!Main.tile[Pos.X, y].HasTile)
            {
                return -1;
            }
            return Main.tile[Pos.X, y].TileType;
        }
    }
}
