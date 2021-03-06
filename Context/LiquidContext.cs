using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
            string displayName = NameUtil.GetNameForLiquids(tile);
            string internalName = NameUtil.GetInternalLiquidName(WaterStyle, false);
            string fullName = NameUtil.GetInternalLiquidName(WaterStyle, true);

            TwailaConfig.NameType nameType = TwailaConfig.Get().DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? "Default Liquid";
        }

        protected override TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            if (TwailaConfig.Get().UseItemTextures)
            {
                TwailaTexture itemTexture = ItemImage(spriteBatch);
                if (itemTexture?.Texture != null)
                {
                    return itemTexture;
                }
                return TileImage(spriteBatch);
            }
            TwailaTexture tileTexture = TileImage(spriteBatch);
            if (tileTexture?.Texture != null)
            {
                return tileTexture;
            }
            return ItemImage(spriteBatch);
        }

        protected virtual TwailaTexture ItemImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            int itemId = ItemUtil.GetItemId(tile, TileType.Liquid);
            return new TwailaTexture(ImageUtil.GetItemTexture(itemId));
        }

        protected virtual TwailaTexture TileImage(SpriteBatch spriteBatch)
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
            if(WaterStyle >= WaterStyleID.Count)
            {
                ModWaterStyle mWater = LoaderManager.Get<WaterStylesLoader>().Get(WaterStyle);
                return mWater?.Mod.DisplayName;
            }
            return "Terraria";
        }

    }
}
