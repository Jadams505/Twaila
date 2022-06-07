using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class LiquidContext : BaseContext
    {
        private int _liquidId;
        private int _waterStyle;

        public LiquidContext(Point point) : base(point)
        {

        }

        public override bool Applies()
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return tile.LiquidAmount > 0;
        }

        public override void UpdateOnChange(BaseContext prevContext, Layout layout)
        {
            Tile tile = Framing.GetTileSafely(Pos);

            _liquidId = tile.LiquidType;
            _waterStyle = Main.waterStyle;

            layout.Name.SetText(GetName(tile));

            if(!(prevContext is LiquidContext other && other._liquidId == _liquidId && 
                (_liquidId != LiquidID.Water || other._waterStyle == _waterStyle)))
            {
                layout.Image.SetImage(GetImage(Main.spriteBatch, tile));
            }

            TwailaText id = new TwailaText("Id: " + tile.LiquidType);
            layout.InfoBox.AddAndEnable(id);

            layout.Mod.SetText(GetMod());
        }

        private string GetName(Tile tile)
        {
            return NameUtil.GetNameForLiquids(tile) ?? "Default Liquid";
        }

        private TwailaTexture GetImage(SpriteBatch spriteBatch, Tile tile)
        {
            return new TwailaTexture(ImageUtil.GetLiquidImageFromTile(spriteBatch, tile));
        }

        private string GetMod()
        {
            return "Terraria";
        }

    }
}
