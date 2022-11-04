using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class TileContext : WireContext
    {
        public int pickIndex;

        protected ushort TileId { get; set; }
        protected short FrameX { get; set; }
        protected short FrameY { get; set; }

        protected string Id { get; set; }
        protected string PickPower { get; set; }
        protected string RecommendedPickaxe { get; set; }
        protected string PaintText { get; set; }


        public TileContext(Point point) : base(point)
        {
            Id = "";
            PickPower = "";
            RecommendedPickaxe = "";
            PaintText = "";
        }

        public override void Update()
        {
            base.Update();
            Tile tile = Framing.GetTileSafely(Pos);
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            TileId = tile.TileType;
            FrameX = tile.TileFrameX;
            FrameY = tile.TileFrameY;

            if (content.ShowId)
            {
                Id = $"Tile Id: {TileId}";
            }

            if (InfoUtil.GetPaintInfo(tile, TileType.Tile, out string paintText, out int paintIcon))
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

            if (InfoUtil.GetPickInfo(tile, ref pickIndex, out string pickText, out int pickId))
            {
                if (pickId != -1)
                {
                    if (content.ShowPickaxe == TwailaConfig.DisplayType.Icon || content.ShowPickaxe == TwailaConfig.DisplayType.Both)
                    {
                        Icons.IconImages.Insert(0, ImageUtil.GetItemTexture(pickId).ToRender());
                    }
                    if (content.ShowPickaxe == TwailaConfig.DisplayType.Name || content.ShowPickaxe == TwailaConfig.DisplayType.Both)
                    {
                        RecommendedPickaxe = $"{NameUtil.GetNameFromItem(pickId)} {InfoUtil.GetPickPowerForItem(pickId)}%";
                    }
                }

                if (content.ShowPickaxePower)
                {
                    PickPower = pickText;
                }
            }
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(TileContext))
            {
                TileContext otherContext = (TileContext)other;
                return otherContext.TileId != TileId || StyleChanged(otherContext);
            }
            return true;
        }

        protected bool StyleChanged(TileContext other)
        {
            TileObjectData oldData = TileUtil.GetTileObjectData(other.TileId, other.FrameX, other.FrameY),
                newData = TileUtil.GetTileObjectData(TileId, FrameX, FrameY);
            if (newData == null || oldData == null)
            {
                return false;
            }
            int oldRow = other.FrameX / oldData.CoordinateFullWidth;
            int oldCol = other.FrameY / oldData.CoordinateFullHeight;
            int newRow = FrameX / newData.CoordinateFullWidth;
            int newCol = FrameY / newData.CoordinateFullHeight;

            return oldRow != newRow || oldCol != newCol;
        }

        protected override TwailaRender GetImage(SpriteBatch spriteBatch)
        {
            if (TwailaConfig.Get().UseItemTextures)
            {
                TwailaRender itemTexture = ItemImage(spriteBatch);
                if(itemTexture != null && itemTexture.CanDraw())
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
            int itemId = ItemUtil.GetItemId(Framing.GetTileSafely(Pos), TileType.Tile);
            Texture2D texture = ImageUtil.GetItemTexture(itemId);
            return texture.ToRender();
        }

        protected virtual TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            Texture2D texture = TreeUtil.GetImageForVanityTree(spriteBatch, tile.TileType) ??
                    TreeUtil.GetImageForGemTree(spriteBatch, tile.TileType) ?? TreeUtil.GetImageForAshTree(spriteBatch, tile.TileType);
            if (texture != null)
            {
                return new TwailaRender(texture, 0.5f);
            }
            texture = ImageUtil.GetImageCustom(spriteBatch, tile) ?? ImageUtil.GetImageFromTileDrawing(spriteBatch, tile, Pos.X, Pos.Y) ?? ImageUtil.GetImageFromTile(spriteBatch, tile);
            return texture.ToRender();
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = base.InfoElements();

            if (!string.IsNullOrEmpty(PaintText))
            {
                elements.Insert(0, new TwailaText(PaintText));
            }
            if (!string.IsNullOrEmpty(RecommendedPickaxe))
            {
                elements.Insert(0, new TwailaText(RecommendedPickaxe));
            }
            if (!string.IsNullOrEmpty(PickPower))
            {
                elements.Insert(0, new TwailaText(PickPower));
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
            int itemId = ItemUtil.GetItemId(tile, TileType.Tile);

            string displayName = NameUtil.GetNameForManualTiles(tile) ?? NameUtil.GetNameForChest(tile) ?? NameUtil.GetNameFromItem(itemId)
                ?? NameUtil.GetNameFromMap(tile, Pos.X, Pos.Y);
            string internalName = NameUtil.GetInternalTileName(TileId, false);
            string fullName = NameUtil.GetInternalTileName(TileId, true);

            TwailaConfig.NameType nameType = TwailaConfig.Get().DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? "Default Name";
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(TileId);
            if (mTile != null)
            {
                return mTile.Mod.DisplayName;
            }
            return "Terraria";
        }
    }
}
