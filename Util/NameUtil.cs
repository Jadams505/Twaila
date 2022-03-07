using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Twaila.UI;

namespace Twaila.Util
{
    internal class NameUtil
    {
        public static string GetNameForTile(Tile tile, Point pos, int itemId)
        {
            return GetNameCustom(tile, pos) ?? GetNameFromItem(itemId) ?? GetNameFromMap(pos) ?? "Default Name";
        }

        private static string GetNameFromItem(int itemId)
        {
            if (itemId == -1)
            {
                return null;
            }
            ModItem item = ModContent.GetModItem(itemId);
            if (item == null) // vanilla
            {
                return Lang.GetItemNameValue(itemId);
            }
            string name = item.DisplayName.GetDefault();
            if (name == null || name.Equals(""))
            {
                return null;
            }
            return name;
        }

        private static string GetNameFromMap(Point pos)
        {
            string mapName = Lang.GetMapObjectName(Main.Map[pos.X, pos.Y].Type);
            if (!mapName.Equals(""))
            {
                return mapName;
            }
            return null;
        }

        private static string GetNameCustom(Tile tile, Point pos)
        {
            return GetNameForManualTiles(tile) ?? GetNameForTreesAndSaplings(tile, pos) ?? GetNameForCactus(tile, pos) ?? 
                GetNameForChest(tile);
        }

        private static string GetNameForManualTiles(Tile tile)
        {
            switch (tile.type)
            {
                case TileID.Grass:
                    return GetNameFromItem(ItemID.GrassSeeds).Replace("Seeds", "Block");
                case TileID.Plants:
                    if (tile.frameX == 144)
                        return GetNameFromItem(ItemID.Mushroom);
                    return "Plant";
                case TileID.CorruptGrass:
                    return GetNameFromItem(ItemID.CorruptSeeds).Replace("Seeds", "Grass Block");
                case TileID.CorruptPlants:
                    if (tile.frameX == 144)
                        return GetNameFromItem(ItemID.VileMushroom);
                    return "Corrupt Plant";
                case TileID.Sunflower:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Sunflower, 0));
                case TileID.Vines:
                    return GetNameFromItem(ItemID.Vine);
                case TileID.JungleGrass:
                    return GetNameFromItem(ItemID.JungleGrassSeeds).Replace("Seeds", "Block");
                case TileID.CrimtaneThorns:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.CorruptThorns, 0));
                case TileID.JunglePlants:
                    if(tile.frameX == 144)
                        return GetNameFromItem(ItemID.JungleSpores);
                    return "Jungle Plant";
                case TileID.JungleVines:
                    return "Jungle " + GetNameFromItem(ItemID.Vine);
                case TileID.MushroomGrass:
                    return GetNameFromItem(ItemID.MushroomGrassSeeds).Replace("Seeds", "Block");
                case TileID.Plants2:
                    return "Tall Plant";
                case TileID.JunglePlants2:
                    return "Tall Jungle Plant";
                case TileID.HallowedGrass:
                    return GetNameFromItem(ItemID.HallowedSeeds).Replace("Seeds", "Grass Block");
                case TileID.HallowedPlants:
                    if (tile.frameX == 144)
                        return GetNameFromItem(ItemID.Mushroom);
                    return "Hallowed Plant";
                case TileID.HallowedPlants2:
                    return "Tall Hallowed Plant";
                case TileID.HallowedVines:
                    return "Hallowed " + GetNameFromItem(ItemID.Vine);
                case TileID.DiscoBall:
                    return GetNameFromItem(ItemID.DiscoBall);
                case TileID.MagicalIceBlock:
                    return "Magic " + GetNameFromItem(ItemID.IceBlock);
                case TileID.BreakableIce:
                    return "Thin " + GetNameFromItem(ItemID.IceBlock);
                case TileID.Stalactite:
                    return "Stalactite";
                case TileID.GreenMoss:
                    return "Green Moss";
                case TileID.BrownMoss:
                    return "Brown Moss";
                case TileID.RedMoss:
                    return "Red Moss";
                case TileID.BlueMoss:
                    return "Blue Moss";
                case TileID.PurpleMoss:
                    return "Purple Moss";
                case TileID.LavaMoss:
                    return "Lava Moss";
                case TileID.LongMoss:
                    if (tile.frameX < 22) return "Green Moss";
                    else if (tile.frameX < 44) return "Brown Moss";
                    else if (tile.frameX < 66) return "Red Moss";
                    else if (tile.frameX < 88) return "Blue Moss";
                    else if (tile.frameX < 110) return "Purple Moss";
                    else if (tile.frameX < 132) return "Lava Moss";
                    break;
                case TileID.SmallPiles:
                    if(tile.frameY == 18)
                    {
                        if(tile.frameX >= 576 && tile.frameX < 612) return "Small " + Lang.GetItemNameValue(ItemID.CopperCoin) + " Stash";
                        if(tile.frameX >= 612 && tile.frameX < 648) return "Small " + Lang.GetItemNameValue(ItemID.SilverCoin) + " Stash";
                        if (tile.frameX >= 648 && tile.frameX < 684) return "Small " + Lang.GetItemNameValue(ItemID.GoldCoin) + " Stash";
                        if (tile.frameX >= 684 && tile.frameX < 720) return Lang.GetItemNameValue(ItemID.Amethyst) + " Stash";
                        if (tile.frameX >= 720 && tile.frameX < 756) return Lang.GetItemNameValue(ItemID.Topaz) + " Stash";
                        if (tile.frameX >= 756 && tile.frameX < 792) return Lang.GetItemNameValue(ItemID.Sapphire) + " Stash";
                        if (tile.frameX >= 792 && tile.frameX < 828) return Lang.GetItemNameValue(ItemID.Emerald) + " Stash";
                        if (tile.frameX >= 828 && tile.frameX < 864) return Lang.GetItemNameValue(ItemID.Ruby) + " Stash";
                        if (tile.frameX >= 864 && tile.frameX < 900) return Lang.GetItemNameValue(ItemID.Diamond) + " Stash";
                    }
                    return "Small Debris";
                case TileID.LargePiles:
                    if (tile.frameX >= 868 && tile.frameX < 972) return "Large " + Lang.GetItemNameValue(ItemID.CopperCoin) + " Stash";
                    if (tile.frameX >= 972 && tile.frameX < 1080) return "Large " + Lang.GetItemNameValue(ItemID.SilverCoin) + " Stash";
                    if (tile.frameX >= 1080 && tile.frameX < 1188) return "Large " + Lang.GetItemNameValue(ItemID.GoldCoin) + " Stash";
                    return "Large Debris";
                case TileID.LargePiles2:
                    if (tile.frameY >= 0 && tile.frameY < 36 && tile.frameX >= 918 && tile.frameX < 972) return Lang.GetItemNameValue(ItemID.EnchantedSword) + " Shrine";
                    return "Large Debris";
                case TileID.LivingWood:
                    return GetNameFromItem(ItemID.LivingWoodWand).Replace("Wand", "Block");
                case TileID.LeafBlock:
                    return GetNameFromItem(ItemID.LeafWand).Replace("Wand", "Block");
                case TileID.FleshGrass:
                    return GetNameFromItem(ItemID.CrimsonSeeds).Replace("Seeds", "Grass Block");
                case TileID.FleshWeeds:
                    if (tile.frameX == 270)
                        return GetNameFromItem(ItemID.ViciousMushroom);
                    return "Crimson Plant";
                case TileID.CrimsonVines:
                    return "Crimson " + GetNameFromItem(ItemID.Vine);
                case TileID.Hive:
                    return GetNameFromItem(ItemID.Hive);
                case TileID.PlantDetritus:
                    return "Jungle Foliage";
                case TileID.WaterDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.WaterDrip, 0));
                case TileID.LavaDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.LavaDrip, 0));
                case TileID.HoneyDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.HoneyDrip, 0));
                case TileID.VineFlowers:
                    return "Flower " + GetNameFromItem(ItemID.Vine);
                case TileID.LivingMahogany:
                    return GetNameFromItem(ItemID.LivingMahoganyWand).Replace("Wand", "Block");
                case TileID.LivingMahoganyLeaves:
                    return GetNameFromItem(ItemID.LivingMahoganyLeafWand).Replace("Wand", "Block");
                case TileID.SandDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.SandDrip, 0));
                case TileID.Pumpkins:
                    return Lang.GetItemNameValue(ItemID.Pumpkin);
            }
            return null;
        }

        private static string GetNameForTreesAndSaplings(Tile tile, Point pos)
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
                    return tree + " " + sapling;
                }
                if (treeBottom == TileID.Sand)
                {
                    return palmTree + " " + sapling;
                }
            }
            else if (tile.type == TileID.Trees)
            {
                toAppend = " " + tree;
                treeBottom = TreeUtil.GetTreeDirt(pos.X, pos.Y, tile);
                if (treeBottom == TileID.Grass)
                {
                    return tree;
                }
            }
            else if (tile.type == TileID.PalmTree)
            {
                toAppend = " " + palmTree;
                treeBottom = TreeUtil.GetPalmTreeSand(pos.X, pos.Y, tile);
                if (treeBottom == TileID.Sand)
                {
                    return palmTree;
                }
            }
            itemId = TreeUtil.GetTreeWood(treeBottom);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + toAppend;
            }
            return null;
        }

        private static string GetNameForCactus(Tile tile, Point pos)
        {
            string cactus = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Cactus, 0));
            if (tile.type == TileID.Cactus)
            {
                int cactusSand = TreeUtil.GetCactusSand(pos.X, pos.Y, tile);
                if (cactusSand == -1)
                {
                    return null;
                }
                if (TileLoader.CanGrowModCactus(cactusSand))
                {
                    ModTile mTile = TileLoader.GetTile(cactusSand);
                    if (mTile != null)
                    {
                        int dropId = mTile.drop;
                        ModItem mItem = ItemLoader.GetItem(dropId);
                        return mItem == null ? mTile.Name : mItem.DisplayName.GetDefault() + " " + cactus;
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
                        return Lang.GetItemNameValue(itemId) + " " + cactus;
                    }
                }
                return cactus;
            }
            return null;
        }

        private static string GetNameForChest(Tile tile)
        {
            if(tile.type == TileID.Containers)
            {
                int style = tile.frameX / 36;
                if(style < Lang.chestType.Length)
                {
                    return Lang.chestType[style].Value;
                }                
            }
            else if(tile.type == TileID.Containers2)
            {
                int style = tile.frameX / 36;
                if (style < Lang.chestType2.Length)
                {
                    return Lang.chestType2[style].Value;
                }
            }
            return null;
        }

        public static string GetModName(Tile tile)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            return mTile == null ? "Terraria" : mTile.mod.DisplayName;
        }
    }
}
