using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Twaila.UI;
using System.Reflection;
using Twaila.Util;
using Twaila.Systems;
using Terraria.ID;

namespace Twaila.Context
{
    public class HatRackContext : TileContext
    {
        public const int MAX_ITEM_COUNT = 2;
        protected int[] ItemIds { get; set; }
        protected string[] ItemTexts { get; set; }

        public HatRackContext(TwailaPoint pos) : base(pos)
        {
            ItemIds = new int[MAX_ITEM_COUNT];
            ItemTexts = new string[MAX_ITEM_COUNT];
        }

        public static HatRackContext CreateHatRackContext(TwailaPoint pos)
        {
            Point bestPos = pos.BestPos();
            Tile tile = Framing.GetTileSafely(bestPos);
            if (tile.TileType == TileID.HatRack)
            {
                Point targetPos = TileUtil.TileEntityCoordinates(bestPos.X, bestPos.Y, width: 3, height: 4);
                if (TEHatRack.Find(targetPos.X, targetPos.Y) != -1 && !TileUtil.IsTileBlockedByAntiCheat(tile, bestPos))
                {
                    return new HatRackContext(pos);
                }
            }
            return null;
        }

        public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(HatRackContext))
            {
                HatRackContext otherContext = (HatRackContext)other;
                for(int i = 0; i < otherContext.ItemIds.Length; ++i)
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

            for(int i = 0; i < ItemIds.Length; ++i)
            {
                int id = ItemIds[i];
                if(id > 0)
                {
                    if(content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                    {
                        IconGrid.AddIcon(ImageUtil.GetRenderForIconItem(id));
                    }
                    if(content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                    {
                        ItemTexts[i] = NameUtil.GetNameFromItem(id);
                        TextGrid.Add(new UITwailaText(ItemTexts[i]));
                    }
                }
            }		
        }

        private void PopulateItems()
        {
            Point targetPos = TileUtil.TileEntityCoordinates(Pos.BestPos().X, Pos.BestPos().Y, width: 3, height: 4);
            int id = TEHatRack.Find(targetPos.X, targetPos.Y);
            TEHatRack instance = (TEHatRack)TileEntity.ByID[id];
            Item[] items = (Item[])instance.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);

            for(int i = 0; i < items.Length; ++i)
            {
                ItemIds[i] = items[i].type;
            }
        }
    }
}
