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
        public static void UpdateUI(TwailaPanel panel, Point pos)
        {
            Tile tile = GetTileCopy(pos.X, pos.Y);
            int itemId = FindItemId(tile);
            UpdateName(panel, pos, itemId: itemId, name: "Default Name");
            UpdateModName(panel, tile);
            panel.Image.Set(pos, tile, itemId);
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
        private static void UpdateName(TwailaPanel panel, Point pos, int itemId = -1, string name = "", bool overrideName = false)
        {
            if (overrideName)
            {
                panel.Name.SetText(name);
                return;
            }
            if(UpdateNameForTrees(panel, pos) || UpdateNameFromItem(itemId, panel) || UpdateNameFromMap(panel, pos))
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

        private static bool UpdateNameFromMap(TwailaPanel panel, Point pos)
        {
            string mapName = Lang.GetMapObjectName(Main.Map[pos.X, pos.Y].Type);
            if (!mapName.Equals(""))
            {
                panel.Name.SetText(mapName);
                return true;
            }
            return false;
        }

        private static bool UpdateNameForTrees(TwailaPanel panel, Point pos)
        {
            Tile tile = Main.tile[pos.X, pos.Y];
            int itemId = -1;
            string toAppend = "";
            if (tile.type == TileID.Trees)
            {
                int? treeType = GetTreeType(pos.X, pos.Y);
                toAppend = " Tree";
                switch (treeType)
                {
                    case 0:
                        itemId = ItemID.Ebonwood;
                        break;
                    case 1:
                    case 5:
                        itemId = ItemID.RichMahogany;
                        break;
                    case 2:
                        itemId = ItemID.Pearlwood;
                        break;
                    case 3:
                        itemId = ItemID.BorealWood;
                        break;
                    case 4:
                        itemId = ItemID.Shadewood;
                        break;
                    case 6:
                        itemId = ItemID.GlowingMushroom;
                        break;
                    case -1:
                        itemId = ItemID.Wood;
                        break;
                }
            }
            else if(tile.type == TileID.PalmTree)
            {
                int? sandType = GetPalmTreeType(pos.X, pos.Y);
                toAppend = " Palm Tree";
                switch (sandType)
                {
                    case 0:
                        itemId = ItemID.PalmWood;
                        toAppend = "";
                        break;
                    case 1:
                        itemId = ItemID.Shadewood;
                        break;
                    case 2:
                        itemId = ItemID.Pearlwood;
                        break;
                    case 3:
                        itemId = ItemID.Ebonwood;
                        break;
                }
            }
            if(itemId != -1)
            {
                panel.Name.SetText(Lang.GetItemNameValue(itemId) + toAppend);
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
        public static Tile GetTileCopy(int x, int y)
        {
            Tile copy = new Tile();
            copy.CopyFrom(Main.tile[x, y]);
            return copy;
        }

        public static int? GetTreeType(int x, int y)
        {
            if(Main.tile[x, y].type != TileID.Trees)
            {
                return null;
            }
            if (Main.tile[x - 1, y].type == TileID.Trees && Main.tile[x, y + 1].type != TileID.Trees && Main.tile[x, y - 1].type != TileID.Trees)
            {
                x--;
            }
            if (Main.tile[x + 1, y].type == TileID.Trees && Main.tile[x, y + 1].type != TileID.Trees && Main.tile[x, y - 1].type != TileID.Trees)
            {
                x++;
            }
            while (Main.tile[x, y].type == TileID.Trees && Main.tile[x, y].active())
            {
                y += 1;
            }
            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return null;
            }
            switch (Main.tile[x, y].type)
            {
                case TileID.CorruptGrass:
                    return 0;
                case TileID.JungleGrass:
                    if (!(y > Main.worldSurface))
                    {
                        return 1;
                    }
                    return 5;
                case TileID.HallowedGrass:
                    return 2;
                case TileID.SnowBlock:
                    return 3;
                case TileID.FleshGrass:
                    return 4;
                case TileID.MushroomGrass:
                    return 6;
                case TileID.Grass:
                    return -1;
                default:
                    return null;
            }
        }
        public static int? GetPalmTreeType(int x, int y)
        {
            if (Main.tile[x, y].type != TileID.PalmTree)
            {
                return null;
            }
            while (Main.tile[x, y].type == TileID.PalmTree && Main.tile[x, y].active())
            {
                y += 1;
            }
            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return null;
            }
            switch (Main.tile[x, y].type)
            {
                case TileID.Sand:
                    return 0;
                case TileID.Crimsand:
                    return 1;
                case TileID.Pearlsand:
                    return 2;
                case TileID.Ebonsand:
                    return 3;
                default:
                    return null;
            }
        }
    }
}
