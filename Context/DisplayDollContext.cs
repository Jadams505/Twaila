using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
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
            Point bestPos = pos.BestPos();
            Tile tile = Framing.GetTileSafely(bestPos);
            if (tile.TileType == TileID.Mannequin || tile.TileType == TileID.Womannequin || tile.TileType == TileID.DisplayDoll)
            {
                Point targetPos = TileUtil.TileEntityCoordinates(bestPos.X, bestPos.Y, width: 2, height: 3);
                if (TEDisplayDoll.Find(targetPos.X, targetPos.Y) != -1 && !TileUtil.IsTileBlockedByAntiCheat(tile, bestPos))
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
            TwailaConfig.Content content = TwailaConfig.Instance.DisplayContent;

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
                    }
                }
            }
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = base.InfoElements();

            foreach (string name in ItemTexts)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    elements.Insert(0, new UITwailaText(name));
                }
            }
            return elements;
        }

        private void PopulateItems()
        {
            Point targetPos = TileUtil.TileEntityCoordinates(Pos.BestPos().X, Pos.BestPos().Y, width: 2, height: 3);
            int id = TEDisplayDoll.Find(targetPos.X, targetPos.Y);
            TEDisplayDoll instance = (TEDisplayDoll)TileEntity.ByID[id];
            Item[] items = (Item[])instance.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);

            for (int i = 0; i < items.Length; ++i)
            {
                ItemIds[i] = items[i].type;
            }
        }
    }
}
