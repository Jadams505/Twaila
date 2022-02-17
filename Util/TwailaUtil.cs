using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
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
            UpdateName(panel, tile, pos, itemId: itemId, name: "Default Name");
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
        private static void UpdateName(TwailaPanel panel, Tile tile, Point pos, int itemId = -1, string name = "", bool overrideName = false)
        {
            if (overrideName)
            {
                panel.Name.SetText(name);
                return;
            }
            if(UpdateNameCustom(panel, tile, pos) || UpdateNameFromItem(itemId, panel) || UpdateNameFromMap(panel, pos))
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

        private static bool UpdateNameCustom(TwailaPanel panel, Tile tile, Point pos)
        {
            if(UpdateNameForTreesAndSaplings(panel, tile, pos) || UpdateNameForCactus(panel, tile, pos))
            {
                return true;
            }
            return false;
        }

        private static bool UpdateNameForTreesAndSaplings(TwailaPanel panel, Tile tile, Point pos)
        {
            int itemId = -1;
            string toAppend = "";
            int treeBottom = -1;
            string tree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Trees, 0));
            string sapling = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Saplings, 0));
            string palmTree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.PalmTree, 0));
            if (TileLoader.IsSapling(tile.type))
            {
                treeBottom = GetSaplingTile(pos.X, pos.Y, tile);
                if (TileLoader.CanGrowModPalmTree(treeBottom) || treeBottom == TileID.Crimsand || treeBottom == TileID.Ebonsand || treeBottom == TileID.Pearlsand)
                {
                    toAppend = " " + palmTree + " " + sapling;
                }
                else
                {
                    toAppend = " " + tree + " " + sapling;
                }
                if (treeBottom == TileID.Grass)
                {
                    panel.Name.SetText(tree + " " + sapling);
                    return true;
                }
                if (treeBottom == TileID.Sand)
                {
                    panel.Name.SetText(palmTree + " " + sapling);
                    return true;
                }
                
            }
            else if (tile.type == TileID.Trees)
            {
                toAppend = " " + tree;
                treeBottom = GetTreeDirt(pos.X, pos.Y, tile);
                if(treeBottom == TileID.Grass)
                {
                    panel.Name.SetText(tree);
                    return true;
                }
            }
            else if(tile.type == TileID.PalmTree)
            {
                toAppend = " " + palmTree;
                treeBottom = GetPalmTreeSand(pos.X, pos.Y, tile);
                if(treeBottom == TileID.Sand)
                {
                    panel.Name.SetText(palmTree);
                    return true;
                }
            }
            itemId = GetTreeWood(treeBottom);
            if (itemId != -1)
            {
                panel.Name.SetText(Lang.GetItemNameValue(itemId) + toAppend);
                return true;
            }
            return false;
            
        }

        private static bool UpdateNameForCactus(TwailaPanel panel, Tile tile, Point pos)
        {
            string cactus = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Cactus, 0));
            if (tile.type == TileID.Cactus)
            {
                int cactusSand = GetCactusSand(pos.X, pos.Y, tile);
                if(cactusSand == -1)
                {
                    return false;
                }
                if (TileLoader.CanGrowModCactus(cactusSand))
                {
                    ModTile mTile = TileLoader.GetTile(cactusSand);
                    if(mTile != null)
                    {
                        int dropId = mTile.drop;
                        ModItem mItem = ItemLoader.GetItem(dropId);
                        panel.Name.SetText(mItem == null ? mTile.Name : mItem.DisplayName.GetDefault() + " " + cactus);
                        return true;
                    }
                }
                else
                {
                    int itemId = -1;
                    switch (cactusSand)
                    {
                        case TileID.Crimsand:
                            itemId = ItemID.CrimsandBlock;
                            break;
                        case TileID.Ebonsand:
                            itemId = ItemID.EbonsandBlock;
                            break;
                        case TileID.Pearlsand:
                            itemId = ItemID.PearlsandBlock;
                            break;
                    }
                    if(itemId != -1)
                    {
                        panel.Name.SetText(Lang.GetItemNameValue(itemId) + " " + cactus);
                        return true;
                    }
                }
                panel.Name.SetText(cactus);
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

        public static int GetTreeWood(int treeDirt)
        {
            switch (treeDirt)
            {
                case TileID.CorruptGrass:
                case TileID.Ebonsand:
                    return ItemID.Ebonwood;
                case TileID.JungleGrass:
                    return ItemID.RichMahogany;
                case TileID.HallowedGrass:
                case TileID.Pearlsand:
                    return ItemID.Pearlwood;
                case TileID.SnowBlock:
                    return ItemID.BorealWood;
                case TileID.FleshGrass:
                case TileID.Crimsand:
                    return ItemID.Shadewood;
                case TileID.MushroomGrass:
                    return ItemID.GlowingMushroom;
                case TileID.Grass:
                    return ItemID.Wood;
                case TileID.Sand:
                    return ItemID.PalmWood;
            }
            int wood = -1;
            TileLoader.DropTreeWood(treeDirt, ref wood);
            TileLoader.DropPalmTreeWood(treeDirt, ref wood);
            return wood;
        }

        public static int GetTreeDirt(int x, int y, Tile tile)
        {
            if (tile.type != TileID.Trees)
            {
                return -1;
            }
            if (Main.tile[x - 1, y].type == TileID.Trees && Main.tile[x, y + 1].type != TileID.Trees && Main.tile[x, y - 1].type != TileID.Trees)
            {
                x--;
            }
            if (Main.tile[x + 1, y].type == TileID.Trees && Main.tile[x, y + 1].type != TileID.Trees && Main.tile[x, y - 1].type != TileID.Trees)
            {
                x++;
            }
            do
            {
                y += 1;
            } while (Main.tile[x, y].type == TileID.Trees && Main.tile[x, y].active());

            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }
        public static int GetPalmTreeSand(int x, int y, Tile tile)
        {
            if (tile.type != TileID.PalmTree)
            {
                return -1;
            }
            do
            {
                y += 1;
            } while (Main.tile[x, y].type == TileID.PalmTree && Main.tile[x, y].active());

            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }
        private static int GetSaplingTile(int x, int y, Tile tile)
        {
            if (!TileLoader.IsSapling(tile.type))
            {
                return -1;
            }
            do
            {
                y++;
            } while (TileLoader.IsSapling(Main.tile[x, y].type) && Main.tile[x, y].active());
            
            if (Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }
        public static int GetCactusSand(int x, int y, Tile tile)
        {
            if(tile.type != TileID.Cactus)
            {
                return -1;
            }
            do
            {
                if (Main.tile[x, y + 1].type == TileID.Cactus)
                {
                    y++;
                }
                else if (Main.tile[x + 1, y].type == TileID.Cactus)
                {
                    x++;
                }
                else if (Main.tile[x - 1, y].type == TileID.Cactus)
                {
                    x--;
                }
                else
                {
                    y++;
                }
            }
            while (Main.tile[x, y].type == TileID.Cactus && Main.tile[x, y].active());
            if(Main.tile[x, y] == null || !Main.tile[x, y].active())
            {
                return -1;
            }
            return Main.tile[x, y].type;
        }
    }
}
