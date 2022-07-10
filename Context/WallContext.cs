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
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            WallId = tile.WallType;

            if (content.ShowId)
            {
                Id = $"Wall Id: {WallId}";
            }

            if (InfoUtil.GetPaintInfo(tile, TileType.Wall, out string paintText, out int paintIcon))
            {
                if (content.ShowPaint == TwailaConfig.DisplayType.Icon || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    if (paintIcon > 0)
                    {
                        Icons.IconImages.Insert(0, ImageUtil.GetItemTexture(paintIcon));
                    }
                }
                if (content.ShowPaint == TwailaConfig.DisplayType.Name || content.ShowPaint == TwailaConfig.DisplayType.Both)
                {
                    PaintText = paintText;
                }
            }
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
            int itemId = ItemUtil.GetItemId(tile, TileType.Wall);
            return new TwailaTexture(ImageUtil.GetItemTexture(itemId));
        }

        protected virtual TwailaTexture TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return new TwailaTexture(ImageUtil.GetWallImageFromTile(spriteBatch, tile));
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

            string displayName = NameUtil.GetNameFromItem(ItemUtil.GetItemId(tile, TileType.Wall));
            string internalName = NameUtil.GetInternalWallName(WallId, false);
            string fullName = NameUtil.GetInternalWallName(WallId, true);

            TwailaConfig.NameType nameType = TwailaConfig.Get().DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? "Default Name";
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
