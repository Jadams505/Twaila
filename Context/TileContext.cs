using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class TileContext : BaseContext
    {
        protected ushort TileId { get; set; }
        protected short FrameX { get; set; }
        protected short FrameY { get; set; }

        public TileContext(Point point) : base(point)
        {
            Tile tile = Framing.GetTileSafely(Pos);

            TileId = tile.TileType;
            FrameX = tile.TileFrameX;
            FrameY = tile.TileFrameY;
        }

        public override bool Applies()
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return tile.HasTile;
        }

        public override void UpdateOnChange(BaseContext prevContext, Layout layout)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            int itemId = ItemUtil.GetItemId(tile, TileType.Tile);

            TileId = tile.TileType;
            FrameX = tile.TileFrameX;
            FrameY = tile.TileFrameY;

            layout.Name.SetText(GetName(tile, itemId));

            if (!(prevContext is TileContext otherContext && otherContext.TileId == TileId && !StyleChanged(otherContext)))
            {
                layout.Image.SetImage(GetTileImage(Main.spriteBatch, tile));
            }

            TwailaText id = new TwailaText("Id: " + tile.TileType);
            layout.InfoBox.AddAndEnable(id);

            layout.Mod.SetText(GetMod());
        }

        protected bool StyleChanged(TileContext other)
        {
            TileObjectData oldData = TileUtil.GetTileObjectData(other.TileId, other.FrameX, other.FrameY),
                newData = TileUtil.GetTileObjectData(TileId, FrameX, FrameY);
            if (newData == null || oldData == null)
            {
                return false;
            }
            int oldRow = other.FrameX / oldData.CoordinateFullWidth;
            int oldCol = other.FrameY / oldData.CoordinateFullHeight;
            int newRow = FrameX / newData.CoordinateFullWidth;
            int newCol = FrameY / newData.CoordinateFullHeight;

            return oldRow != newRow || oldCol != newCol;
        }

        private TwailaTexture GetTileImage(SpriteBatch spriteBatch, Tile tile)
        {
            Texture2D texture = TreeUtil.GetImageForVanityTree(spriteBatch, tile.TileType) ??
                    TreeUtil.GetImageForGemTree(spriteBatch, tile.TileType);
            if (texture != null)
            {
                return new TwailaTexture(texture, 0.5f);
            }
            /*
            if (OnlyWire())
            {
                return new TwailaTexture(ImageUtil.GetImageForWireAndActuator(spriteBatch, tile));
            }
            */
            texture = ImageUtil.GetImageCustom(spriteBatch, tile) ?? ImageUtil.GetImageFromTileDrawing(spriteBatch, tile, Pos.X, Pos.Y) ?? ImageUtil.GetImageFromTile(spriteBatch, tile);
            return new TwailaTexture(texture);
        }

        private string GetName(Tile tile, int itemId)
        {
            return NameUtil.GetNameForManualTiles(tile) ?? NameUtil.GetNameForChest(tile) ?? NameUtil.GetNameFromItem(itemId)
                ?? NameUtil.GetNameFromMap(tile, Pos.X, Pos.Y) ?? "Default Name";
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
    }
}
