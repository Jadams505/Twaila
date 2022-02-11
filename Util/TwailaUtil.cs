using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.UI;
namespace Twaila.Util
{
    internal class TwailaUtil
    {
        public static void UpdateUI(TwailaPanel panel, Tile tile)
        {
            int itemId = FindItemId(tile);
            UpdateName(panel, tile, itemId: itemId, name: "Default Name");
            UpdateModName(panel, tile);
            panel.Image.Set(tile, itemId);
        }

        private static int FindItemId(Tile tile)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            int style = TileObjectData.GetTileStyle(tile);
            if(mTile == null)
            {
                Item item = new Item();
                for (int i = 0; i < ItemID.Count; ++i)
                {
                    item.SetDefaults(i);
                    if (item.createTile == tile.type && (style == -1 || item.placeStyle == style))
                    {
                        return i;
                    }
                }
                return -1;
            }
            bool multiTile = TileObjectData.GetTileData(tile) != null;
            if (mTile.drop == 0 && multiTile)
            {
                for (int i = ItemID.Count; i < ItemLoader.ItemCount; ++i)
                {
                    ModItem mItem = ItemLoader.GetItem(i);
                    if (mItem != null && mItem.item.createTile == tile.type && (style == -1 || mItem.item.placeStyle == style))
                    {
                        return i;
                    }
                }
            }
            return mTile.drop == 0 ? -1 : mTile.drop;    
        }
        private static void UpdateName(TwailaPanel panel, Tile tile, int itemId = -1, string name = "", bool overrideName = false)
        {
            if (overrideName)
            {
                panel.Name.SetText(name);
                return;
            }
            if(UpdateNameFromItem(itemId, panel) || UpdateNameFromMap(panel, tile))
            {
                return;
            }
            panel.Name.SetText(name);
        }

        private static bool UpdateNameFromItem(int itemId, TwailaPanel panel)
        {
            if(itemId == -1)
            {
                return false;
            }
            ModItem item = ModContent.GetModItem(itemId);
            if(item == null) // vanilla
            {
                panel.Name.SetText(Lang.GetItemNameValue(itemId));
                return true;
            }
            string name = item.DisplayName.GetDefault();
            if(name == null || name.Equals(""))
            {
                return false;
            }
            panel.Name.SetText(name);
            return true;
        }

        private static bool UpdateNameFromMap(TwailaPanel panel, Tile tile)
        {
            string mapName = Lang.GetMapObjectName(Main.Map[Player.tileTargetX, Player.tileTargetY].Type);
            if (!mapName.Equals(""))
            {
                panel.Name.SetText(mapName);
                return true;
            }
            return false;
        }

        private static bool UpdateModName(TwailaPanel panel, Tile tile)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            panel.Mod.SetText(mTile == null ? "Terraria" : mTile.mod.DisplayName);
            return true;
        }
    }
}
