using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class WallContext : BaseContext
    {

        private ushort _wallId;

        public WallContext(Point pos) : base(pos)
        {
        }

        public override bool Applies()
        {
            Tile tile = Framing.GetTileSafely(Pos.X, Pos.Y);
            return tile.WallType > WallID.None;
        }

        public override void UpdateOnChange(BaseContext prevContext, Layout layout)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            _wallId = tile.WallType;

            if(!(prevContext is WallContext otherContext && otherContext._wallId == this._wallId))
            {
                layout.Image.SetImage(GetImage(Main.spriteBatch));
            }

            layout.Name.SetText(GetName());

            TwailaText id = new TwailaText("Id: " + tile.WallType);
            layout.InfoBox.AddAndEnable(id);

            TwailaText color = new TwailaText("Color: " + tile.WallColor);
            layout.InfoBox.AddAndEnable(color);

            layout.Mod.SetText(GetMod());
        }

        public TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            return new TwailaTexture(ImageUtil.GetWallImageFromTile(spriteBatch, Framing.GetTileSafely(Pos)));
        }

        public string GetName()
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return NameUtil.GetNameFromItem(ItemUtil.GetItemId(tile, TileType.Wall)) ?? "Default Wall";
        }

        public string GetMod()
        {
            ModWall mWall = WallLoader.GetWall(_wallId);
            if (mWall != null)
            {
                return mWall.Mod.DisplayName;
            }
            return "Terraria";
        }
    }
}
