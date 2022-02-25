﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Twaila.UI;

namespace Twaila.Util
{
    internal class NameUtil
    {
        public static void UpdateName(TwailaPanel panel, Tile tile, Point pos, int itemId = -1, string name = "", bool overrideName = false)
        {
            if (overrideName)
            {
                panel.Name.SetText(name);
                return;
            }
            if (UpdateNameCustom(panel, tile, pos) || UpdateNameFromItem(itemId, panel) || UpdateNameFromMap(panel, pos))
            {
                return;
            }
            panel.Name.SetText(name);
        }

        private static bool UpdateNameFromItem(int itemId, TwailaPanel panel)
        {
            if (itemId == -1)
            {
                return false;
            }
            ModItem item = ModContent.GetModItem(itemId);
            if (item == null) // vanilla
            {
                panel.Name.SetText(Lang.GetItemNameValue(itemId));
                return true;
            }
            string name = item.DisplayName.GetDefault();
            if (name == null || name.Equals(""))
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
            if (UpdateNameForTreesAndSaplings(panel, tile, pos) || UpdateNameForCactus(panel, tile, pos))
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
                treeBottom = TreeUtil.GetSaplingTile(pos.X, pos.Y, tile);
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
                treeBottom = TreeUtil.GetTreeDirt(pos.X, pos.Y, tile);
                if (treeBottom == TileID.Grass)
                {
                    panel.Name.SetText(tree);
                    return true;
                }
            }
            else if (tile.type == TileID.PalmTree)
            {
                toAppend = " " + palmTree;
                treeBottom = TreeUtil.GetPalmTreeSand(pos.X, pos.Y, tile);
                if (treeBottom == TileID.Sand)
                {
                    panel.Name.SetText(palmTree);
                    return true;
                }
            }
            itemId = TreeUtil.GetTreeWood(treeBottom);
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
                int cactusSand = TreeUtil.GetCactusSand(pos.X, pos.Y, tile);
                if (cactusSand == -1)
                {
                    return false;
                }
                if (TileLoader.CanGrowModCactus(cactusSand))
                {
                    ModTile mTile = TileLoader.GetTile(cactusSand);
                    if (mTile != null)
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
                    if (itemId != -1)
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

        public static bool UpdateModName(TwailaPanel panel, Tile tile)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            panel.Mod.SetText(mTile == null ? "Terraria" : mTile.mod.DisplayName);
            return true;
        }
    }
}
