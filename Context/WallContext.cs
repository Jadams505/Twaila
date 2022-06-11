using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class WallContext : WireContext
    {
        protected ushort WallId { get; set; }

        protected string Id { get; set; }
        protected string PaintText { get; set; }

        public WallContext(Point pos) : base(pos)
        {
            Update();
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
            Tile tile = Framing.GetTileSafely(Pos);
            WallId = tile.WallType;

            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            string iconText = "";

            if (content.ShowId)
            {
                Id = $"Wall Id: {WallId}";
            }

            if (InfoUtil.GetPaintInfo(tile, TileType.Wall, out string paintText, out string paintIcon))
            {
                if (content.ShowPaint == TwailaConfig.DisplayType.Icon || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    iconText += paintIcon;
                }
                if (content.ShowPaint == TwailaConfig.DisplayType.Name || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    PaintText = paintText;
                }
            }

            InfoIcons = iconText + InfoIcons;
        }

        protected override TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            return new TwailaTexture(ImageUtil.GetWallImageFromTile(spriteBatch, Framing.GetTileSafely(Pos)));
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = base.InfoElements();

            if (!string.IsNullOrEmpty(PaintText))
            {
                elements.Insert(0, new TwailaText(PaintText));
            }
            if (!string.IsNullOrEmpty(Id))
            {
                elements.Insert(0, new TwailaText(Id));
            }
            

            return elements;
        }

        protected override string GetName()
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return NameUtil.GetNameFromItem(ItemUtil.GetItemId(tile, TileType.Wall)) ?? "Default Wall";
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
