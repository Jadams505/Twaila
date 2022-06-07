using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class BlockContext : BaseContext
    {
        private ushort _tileId;
        private short _frameX;
        private short _frameY;

        public BlockContext(Point point) : base(point)
        {

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

            _tileId = tile.TileType;
            _frameX = tile.TileFrameX;
            _frameY = tile.TileFrameY;

            layout.Name.SetText(GetName(tile, itemId));

            if (!(prevContext is BlockContext otherContext && otherContext._tileId == _tileId && StyleChanged(otherContext)))
            {
                layout.Image.SetImage(GetTileImage(Main.spriteBatch, tile));
            }

            TwailaText id = new TwailaText("Id: " + tile.TileType);
            layout.InfoBox.AddAndEnable(id);

            layout.Mod.SetText(GetMod());
        }

        private bool StyleChanged(BlockContext other)
        {
            TileObjectData oldData = TileUtil.GetTileObjectData(other._tileId, other._frameX, other._frameY),
                newData = TileUtil.GetTileObjectData(_tileId, _frameX, _frameY);
            if (newData == null || oldData == null)
            {
                return false;
            }
            int oldRow = other._frameX / oldData.CoordinateFullWidth;
            int oldCol = other._frameY / oldData.CoordinateFullHeight;
            int newRow = _frameX / newData.CoordinateFullWidth;
            int newCol = _frameY / newData.CoordinateFullHeight;

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
            ModTile mTile = TileLoader.GetTile(_tileId);
            if (mTile != null)
            {
                return mTile.Mod.DisplayName;
            }
            return "Terraria";
        }
    }
}
