using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class LiquidContext : WireContext
    {
        protected int LiquidId { get; set; }
        protected int WaterStyle { get; set; }

        protected string Id { get; set; }

        public LiquidContext(TwailaPoint point) : base(point)
        {
            Id = "";
        }

        public static LiquidContext CreateLiquidContext(TwailaPoint pos)
        {
            Tile tile = Framing.GetTileSafely(pos.BestPos());

            if (tile.LiquidAmount > 0 && !TileUtil.IsBlockedByAntiCheat(tile, pos.BestPos()))
            {
                return new LiquidContext(pos);
            }

            return null;
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
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            Content content = TwailaConfig.Instance.DisplayContent;

            LiquidId = tile.LiquidType;
            WaterStyle = Main.waterStyle;

            if (content.ShowId)
            {
                if (LiquidId == LiquidID.Water)
                {
                    Id = Language.GetText("Mods.Twaila.WaterStyle").WithFormatArgs(WaterStyle).Value;
                }
                else
                {
                    Id = Language.GetText("Mods.Twaila.LiquidId").WithFormatArgs(LiquidId).Value;
                }
                TextGrid.Add(new UITwailaText(Id));
            }
        }

        protected override string GetName()
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            string displayName = NameUtil.GetNameForLiquids(tile);
            string internalName = NameUtil.GetInternalLiquidName(WaterStyle, false);
            string fullName = NameUtil.GetInternalLiquidName(WaterStyle, true);

            TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? Language.GetTextValue("Mods.Twaila.Defaults.Liquid");
        }

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            if (TwailaConfig.Instance.UseItemTextures)
            {
                TwailaRender itemTexture = ItemImage(spriteBatch);
                if (itemTexture != null && itemTexture.CanDraw())
                {
                    return itemTexture;
                }
                return TileImage(spriteBatch);
            }
            TwailaRender tileTexture = TileImage(spriteBatch);
            if (tileTexture != null && tileTexture.CanDraw())
            {
                return tileTexture;
            }
            return ItemImage(spriteBatch);
        }

        protected virtual TwailaRender ItemImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            int itemId = ItemTilePairSystem.GetItemId(tile, TileType.Liquid);
            return ImageUtil.GetItemTexture(itemId).ToRender();
        }

        protected virtual TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            return ImageUtil.GetLiquidRenderFromTile(tile);
        }

        protected override string GetMod()
        {
            ModWaterStyle mWater = LoaderManager.Get<WaterStylesLoader>().Get(WaterStyle);
            return NameUtil.GetMod(mWater);
        }

    }
}
