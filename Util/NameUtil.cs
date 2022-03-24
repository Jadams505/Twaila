using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI.Chat;
using Twaila.Context;
using Twaila.ObjectData;

namespace Twaila.Util
{
    internal class NameUtil
    {
        public static string GetNameFromItem(int itemId)
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

        public static string GetNameFromMap(TileContext context)
        {
            string mapName = Lang.GetMapObjectName(Main.Map[context.Pos.X, context.Pos.Y].Type);
            int style = TileObjectData.GetTileStyle(context.Tile);
            string altMapName = Lang.GetMapObjectName(MapHelper.TileToLookup(context.Tile.type, style == -1 ? 0 : style));
            if (mapName != null && !mapName.Equals(""))
            {
                return mapName;
            }
            if (altMapName != null && !altMapName.Equals(""))
            {
                return altMapName;
            }
            return null;
        }

        public static string GetNameForManualTiles(Tile tile)
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

        public static string GetNameForTree(TreeContext context)
        {
            string tree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Trees, 0));
            string toAppend = "";
            if (context.Tile.type == TileID.Trees)
            {
                toAppend = " " + tree;
                if (context.TreeDirt == TileID.Grass)
                {
                    return tree;
                }
            }
            int itemId = TreeUtil.GetTreeWood(context.TreeDirt);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + toAppend;
            }
            return null;
        }

        public static string GetNameForPalmTree(PalmTreeContext context)
        {
            string palmTree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.PalmTree, 0));
            string toAppend = "";
            if (context.Tile.type == TileID.PalmTree)
            {
                toAppend = " " + palmTree;
                if (context.PalmTreeSand == TileID.Sand)
                {
                    return palmTree;
                }
            }
            int itemId = TreeUtil.GetTreeWood(context.PalmTreeSand);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + toAppend;
            }
            return null;
        }

        public static string GetNameForSapling(SaplingContext context)
        {
            string tree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Trees, 0));
            string sapling = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Saplings, 0));
            string palmTree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.PalmTree, 0));
            string toAppend = "";
            if (TileLoader.IsSapling(context.Tile.type))
            {
                if (TileLoader.CanGrowModPalmTree(context.SaplingDirt) || context.SaplingDirt == TileID.Crimsand || 
                    context.SaplingDirt == TileID.Ebonsand || context.SaplingDirt == TileID.Pearlsand)
                {
                    toAppend = " " + palmTree + " " + sapling;
                }
                else
                {
                    toAppend = " " + tree + " " + sapling;
                }
                if (context.SaplingDirt == TileID.Grass)
                {
                    return tree + " " + sapling;
                }
                if (context.SaplingDirt == TileID.Sand)
                {
                    return palmTree + " " + sapling;
                }
            }
            int itemId = TreeUtil.GetTreeWood(context.SaplingDirt);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + toAppend;
            }
            return null;
        }

        public static string GetNameForCactus(CactusContext context)
        {
            string cactus = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Cactus, 0));
            if (context.Tile.type == TileID.Cactus)
            {
                if (context.CactusSand == -1)
                {
                    return null;
                }
                if (TileLoader.CanGrowModCactus(context.CactusSand))
                {
                    ModTile mTile = TileLoader.GetTile(context.CactusSand);
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
                    switch (context.CactusSand)
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

        public static string GetNameForChest(Tile tile)
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
            if(mTile == null)
            {
                return "Terraria";
            }
            return mTile.mod.DisplayName;
        }
    }
}
