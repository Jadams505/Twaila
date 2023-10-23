using Microsoft.Xna.Framework;
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
    public class WallContext : WireContext
    {
        protected ushort WallId { get; set; }

        protected string Id { get; set; }
        protected string PaintText { get; set; }
        protected string IlluminantText { get; set; }
        protected string EchoText { get; set; }

        public WallContext(TwailaPoint pos) : base(pos)
        {
            Id = "";
            PaintText = "";
            IlluminantText = "";
            EchoText = "";
        }

        public static WallContext CreateWallContext(TwailaPoint pos)
        {
            Point tilePos = pos.BestTilePos();
            Tile tile = Framing.GetTileSafely(tilePos);

            if (tile.WallType == WallID.None || tile.WallType >= WallLoader.WallCount)
                return null;

            if (!TileUtil.IsTilePosInBounds(tilePos))
                return null;

            if (!TileUtil.IsWallBlockedByAntiCheat(tile, tilePos))
                return new WallContext(pos);

            return null;
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(WallContext))
            {
                WallContext otherContext = (WallContext)other;
                return otherContext.WallId != WallId;
            }
            return true;
        }

        public override void Update()
        {
            base.Update();
            Tile tile = Framing.GetTileSafely(BestTilePos);
            Content content = TwailaConfig.Instance.DisplayContent;

            WallId = tile.WallType;

            if (content.ShowId)
            {
                Id = Language.GetText("Mods.Twaila.WallId").WithFormatArgs(WallId).Value;
                TextGrid.Add(new UITwailaText(Id));
            }

            if (InfoUtil.GetPaintInfo(tile, TileType.Wall, out string paintText, out int paintIcon))
            {
                if (content.ShowPaint == TwailaConfig.DisplayType.Icon || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    if (paintIcon > 0)
                    {
                        IconGrid.AddIcon(ImageUtil.GetItemTexture(paintIcon).ToRender());
                    }
                }
                if (content.ShowPaint == TwailaConfig.DisplayType.Name || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    PaintText = paintText;
                    TextGrid.Add(new UITwailaText(PaintText));
                }
            }

            if (InfoUtil.GetCoatingInfo(tile, TileType.Wall, out string illuminantText, out string echoText,
                out int illuminantIcon, out int echoIcon))
            {
                if (content.ShowCoating == TwailaConfig.DisplayType.Icon || content.ShowCoating == TwailaConfig.DisplayType.Both)
                {
                    if (illuminantIcon > 0)
                    {
                        IconGrid.AddIcon(ImageUtil.GetItemTexture(illuminantIcon).ToRender());
                    }
                    if (echoIcon > 0)
                    {
                        IconGrid.AddIcon(ImageUtil.GetItemTexture(echoIcon).ToRender());
                    }
                }
                if (content.ShowCoating == TwailaConfig.DisplayType.Name || content.ShowCoating == TwailaConfig.DisplayType.Both)
                {
                    IlluminantText = illuminantText;
                    EchoText = echoText;
                    TextGrid.Add(new UITwailaText(IlluminantText));
                    TextGrid.Add(new UITwailaText(EchoText));
                }
            }
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
            Tile tile = Framing.GetTileSafely(BestTilePos);
            int itemId = ItemTilePairSystem.GetItemId(tile, TileType.Wall);
            return ImageUtil.GetItemTexture(itemId).ToRender();
        }

        protected virtual TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(BestTilePos);
            return ImageUtil.GetWallRenderFromTile(tile);
        }

        protected override string GetName()
        {
            Tile tile = Framing.GetTileSafely(BestTilePos);

            string displayName = NameUtil.GetNameFromItem(ItemTilePairSystem.GetItemId(tile, TileType.Wall));
            string internalName = NameUtil.GetInternalWallName(WallId, false);
            string fullName = NameUtil.GetInternalWallName(WallId, true);

            TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? Language.GetTextValue("Mods.Twaila.Defaults.Name");
        }

        protected override string GetMod()
        {
            ModWall mWall = WallLoader.GetWall(WallId);
            return NameUtil.GetMod(mWall);
        }
    }
}
