using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
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
			Tile tile = Framing.GetTileSafely(pos.BestPos());

			if (Framing.GetTileSafely(pos.BestPos()).WallType > 0 && !TileUtil.IsWallBlockedByAntiCheat(tile, pos.BestPos()))
			{
				return new WallContext(pos);
			}

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
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            WallId = tile.WallType;

            if (content.ShowId)
            {
				Id = Language.GetText("Mods.Twaila.WallId").WithFormatArgs(WallId).Value;
            }

            if (InfoUtil.GetPaintInfo(tile, TileType.Wall, out string paintText, out int paintIcon))
            {
                if (content.ShowPaint == TwailaConfig.DisplayType.Icon || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    if (paintIcon > 0)
                    {
                        Icons.IconImages.Insert(0, ImageUtil.GetItemTexture(paintIcon).ToRender());
                    }
                }
                if (content.ShowPaint == TwailaConfig.DisplayType.Name || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    PaintText = paintText;
                }
            }

			if (InfoUtil.GetCoatingInfo(tile, TileType.Wall, out string illuminantText, out string echoText,
				out int illuminantIcon, out int echoIcon))
			{
				if (content.ShowCoating == TwailaConfig.DisplayType.Icon || content.ShowCoating == TwailaConfig.DisplayType.Both)
				{
					if (illuminantIcon > 0)
					{
						Icons.IconImages.Insert(0, ImageUtil.GetItemTexture(illuminantIcon).ToRender());
					}
					if (echoIcon > 0)
					{
						Icons.IconImages.Insert(0, ImageUtil.GetItemTexture(echoIcon).ToRender());
					}
				}
				if (content.ShowCoating == TwailaConfig.DisplayType.Name || content.ShowCoating == TwailaConfig.DisplayType.Both)
				{
					IlluminantText = illuminantText;
					EchoText = echoText;
				}
			}
		}

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            if (TwailaConfig.Get().UseItemTextures)
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
            int itemId = ItemUtil.GetItemId(tile, TileType.Wall);
            return ImageUtil.GetItemTexture(itemId).ToRender();
        }

        protected virtual TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            return ImageUtil.GetWallRenderFromTile(tile);
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = base.InfoElements();

            if (!string.IsNullOrEmpty(PaintText))
            {
                elements.Insert(0, new UITwailaText(PaintText));
            }
			if (!string.IsNullOrEmpty(IlluminantText))
			{
				elements.Insert(0, new UITwailaText(IlluminantText));
			}
			if (!string.IsNullOrEmpty(EchoText))
			{
				elements.Insert(0, new UITwailaText(EchoText));
			}
			if (!string.IsNullOrEmpty(Id))
            {
                elements.Insert(0, new UITwailaText(Id));
            }
            

            return elements;
        }

        protected override string GetName()
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());

            string displayName = NameUtil.GetNameFromItem(ItemUtil.GetItemId(tile, TileType.Wall));
            string internalName = NameUtil.GetInternalWallName(WallId, false);
            string fullName = NameUtil.GetInternalWallName(WallId, true);

            TwailaConfig.NameType nameType = TwailaConfig.Get().DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? Language.GetTextValue("Mods.Twaila.Defaults.Name");
        }

        protected override string GetMod()
        {
            ModWall mWall = WallLoader.GetWall(WallId);
            if (mWall != null)
            {
                return mWall.Mod.DisplayName;
            }
            return "Terraria";
        }
    }
}
