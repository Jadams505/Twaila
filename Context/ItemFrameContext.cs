using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class ItemFrameContext : TileContext
    {
        protected int ItemId { get; set; }
        protected string ItemText { get; set; }

        public ItemFrameContext(TwailaPoint point) : base(point)
        {
            ItemId = 0;
            ItemText = "";
        }

        public static ItemFrameContext CreateItemFrameContext(TwailaPoint pos)
        {
            Point tilePos = pos.BestTilePos();
            Tile tile = Framing.GetTileSafely(tilePos);

            if (!tile.HasTile || tile.TileType >= TileLoader.TileCount)
                return null;

            if (!TileUtil.IsTilePosInBounds(tilePos))
                return null;

            if (tile.TileType == TileID.ItemFrame)
            {
                Point targetPos = TileUtil.TileEntityCoordinates(tilePos.X, tilePos.Y, width: 2, height: 2);
                if (TEItemFrame.Find(targetPos.X, targetPos.Y) != -1 && !TileUtil.IsTileBlockedByAntiCheat(tile, tilePos))
                {
                    return new ItemFrameContext(pos);
                }
            }
            return null;
        }

        public override void Update()
        {
            base.Update();
            Content content = TwailaConfig.Instance.DisplayContent;

            ItemId = GetItemId();

            if (ItemId > 0)
            {
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    IconGrid.AddIcon(ImageUtil.GetRenderForIconItem(ItemId));
                }
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    ItemText = Lang.GetItemNameValue(ItemId);
                    TextGrid.Add(new UITwailaText(ItemText));
                }
            }
        }

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(BestTilePos);
            return ImageUtil.GetRenderForItemFrame(spriteBatch, tile, BestTilePos.X, BestTilePos.Y, ItemId);
        }

        public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(ItemFrameContext))
            {
                ItemFrameContext otherContext = (ItemFrameContext)other;
                return otherContext.ItemId != ItemId;
            }
            return true;
        }

        private int GetItemId()
        {
            Point targetPos = TileUtil.TileEntityCoordinates(BestTilePos.X, BestTilePos.Y, width: 2, height: 2);
            int id = TEItemFrame.Find(targetPos.X, targetPos.Y);
            Item item = ((TEItemFrame)TileEntity.ByID[id]).item;
            return item.type;
        }
    }
}
