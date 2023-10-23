using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class DisplayDollContext : TileContext
    {
        public const int MAX_ITEM_COUNT = 8;
        protected int[] ItemIds { get; set; }
        protected string[] ItemTexts { get; set; }

        public DisplayDollContext(TwailaPoint point) : base(point)
        {
            ItemIds = new int[MAX_ITEM_COUNT];
            ItemTexts = new string[MAX_ITEM_COUNT];
        }

        public static DisplayDollContext CreateDisplayDollContext(TwailaPoint pos)
        {
            Point tilePos = pos.BestTilePos();
            Tile tile = Framing.GetTileSafely(tilePos);

            if (!tile.HasTile || tile.TileType >= TileLoader.TileCount)
                return null;

            if (!TileUtil.IsTilePosInBounds(tilePos))
                return null;

            if (tile.TileType == TileID.Mannequin || tile.TileType == TileID.Womannequin || tile.TileType == TileID.DisplayDoll)
            {
                Point targetPos = TileUtil.TileEntityCoordinates(tilePos.X, tilePos.Y, width: 2, height: 3);
                if (TEDisplayDoll.Find(targetPos.X, targetPos.Y) != -1 && !TileUtil.IsTileBlockedByAntiCheat(tile, tilePos))
                {
                    return new DisplayDollContext(pos);
                }
            }
            return null;
        }

        public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(DisplayDollContext))
            {
                DisplayDollContext otherContext = (DisplayDollContext)other;
                for (int i = 0; i < otherContext.ItemIds.Length; ++i)
                {
                    if (otherContext.ItemIds[i] != ItemIds[i])
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        public override void Update()
        {
            base.Update();
            Content content = TwailaConfig.Instance.DisplayContent;

            PopulateItems();

            for (int i = 0; i < ItemIds.Length; ++i)
            {
                int id = ItemIds[i];
                if (id > 0)
                {
                    if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                    {
                        IconGrid.AddIcon(ImageUtil.GetRenderForIconItem(id));
                    }
                    if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                    {
                        ItemTexts[i] = NameUtil.GetNameFromItem(id);
                        TextGrid.Add(new UITwailaText(ItemTexts[i]));
                    }
                }
            }
        }

        private static readonly FieldInfo TEDisplayDoll_items = typeof(TEDisplayDoll).GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

        private void PopulateItems()
        {
            Point targetPos = TileUtil.TileEntityCoordinates(BestTilePos.X, BestTilePos.Y, width: 2, height: 3);
            int id = TEDisplayDoll.Find(targetPos.X, targetPos.Y);
            TEDisplayDoll instance = (TEDisplayDoll)TileEntity.ByID[id];
            Item[] items = (Item[])TEDisplayDoll_items?.GetValue(instance);

            if (items is null)
                return;

            for (int i = 0; i < items.Length; ++i)
            {
                ItemIds[i] = items[i].type;
            }
        }
    }
}
