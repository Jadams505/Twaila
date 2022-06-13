using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class LiquidContext : WireContext
    {
        protected int LiquidId { get; set; }
        protected int WaterStyle { get; set; }

        protected string Id { get; set; }

        public LiquidContext(Point point) : base(point)
        {
            Update();
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(LiquidContext))
            {
                LiquidContext otherContext = (LiquidContext)other;
                return otherContext.LiquidId != LiquidId || (LiquidId == LiquidID.Water && otherContext.WaterStyle != WaterStyle);
            }
            return true;
        }

        public override void Update()
        {
            base.Update();
            Tile tile = Framing.GetTileSafely(Pos);
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            LiquidId = tile.LiquidType;
            WaterStyle = Main.waterStyle;

            if (content.ShowId)
            {
                if (LiquidId == LiquidID.Water)
                {
                    Id = $"Water Style: {WaterStyle}";
                }
                else
                {
                    Id = $"Liquid Id: {LiquidId}";
                }
            }
        }

        protected override string GetName()
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return NameUtil.GetNameForLiquids(tile) ?? "Default Liquid";
        }

        protected override TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return new TwailaTexture(ImageUtil.GetLiquidImageFromTile(spriteBatch, tile));
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = base.InfoElements();

            if (!string.IsNullOrEmpty(Id))
            {
                elements.Insert(0, new TwailaText(Id));
            }

            return elements;
        }

        protected override string GetMod()
        {
            return "Terraria";
        }

    }
}
