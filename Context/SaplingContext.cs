using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.Graphics;
using Terraria.ID;
using Twaila.UI;
using Terraria.ModLoader;

namespace Twaila.Context
{
    public class SaplingContext : TileContext
    {
        public int DirtId { get; private set; }

        public SaplingContext(Point pos) : base(pos)
        {

        }

        public override bool Applies()
        {
            return TileID.Sets.TreeSapling[TileId];
        }

        public override void UpdateOnChange(BaseContext prevContext, Layout layout)
        {
            Tile tile = Framing.GetTileSafely(Pos);

            TileId = tile.TileType;
            FrameX = tile.TileFrameX;
            FrameY = tile.TileFrameY;
            DirtId = GetSaplingTile();

            layout.Name.SetText(GetName());

            if (!(prevContext is SaplingContext otherContext && otherContext.DirtId == DirtId &&
                !StyleChanged(otherContext)))
            {
                layout.Image.SetImage(GetImage(Main.spriteBatch, tile));
            }

            TwailaText id = new TwailaText("Id: " + tile.TileType);
            layout.InfoBox.AddAndEnable(id);

            layout.Mod.SetText(GetMod());
        }

        private TwailaTexture GetImage(SpriteBatch spriteBatch, Tile tile)
        {
            return new TwailaTexture(ImageUtil.GetImageFromTileDrawing(spriteBatch, tile, Pos.X, Pos.Y));
        }

        private string GetName()
        {
            return NameUtil.GetNameForSapling(TileId, DirtId);
        }

        private string GetMod()
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
