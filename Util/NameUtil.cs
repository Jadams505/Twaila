﻿using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Context;

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
            string altMapName = Lang.GetMapObjectName(MapHelper.TileToLookup(context.Tile.TileType, style == -1 ? 0 : style));
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
            switch (tile.TileType)
            {
                case TileID.Grass:
                    return GetNameFromItem(ItemID.GrassSeeds).Replace("Seeds", "Block");
                case TileID.Plants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.Mushroom);
                    return "Plant";
                case TileID.CorruptGrass:
                    return GetNameFromItem(ItemID.CorruptSeeds).Replace("Seeds", "Grass Block");
                case TileID.CorruptPlants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.VileMushroom);
                    return "Corrupt Plant";
                case TileID.Sunflower:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Sunflower, 0));
                case TileID.Vines:
                    return GetNameFromItem(ItemID.Vine);
                case TileID.JungleGrass:
                    return GetNameFromItem(ItemID.JungleGrassSeeds).Replace("Seeds", "Block");
                case TileID.CrimsonThorns:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.CorruptThorns, 0));
                case TileID.JunglePlants:
                    if(tile.TileFrameX == 144)
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
                    if (tile.TileFrameX == 144)
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
                    if (tile.TileFrameX < 22) return "Green Moss";
                    else if (tile.TileFrameX < 44) return "Brown Moss";
                    else if (tile.TileFrameX < 66) return "Red Moss";
                    else if (tile.TileFrameX < 88) return "Blue Moss";
                    else if (tile.TileFrameX < 110) return "Purple Moss";
                    else if (tile.TileFrameX < 132) return "Lava Moss";
                    break;
                case TileID.SmallPiles:
                    if(tile.TileFrameY == 18)
                    {
                        if(tile.TileFrameX >= 576 && tile.TileFrameX < 612) return "Small " + Lang.GetItemNameValue(ItemID.CopperCoin) + " Stash";
                        if(tile.TileFrameX >= 612 && tile.TileFrameX < 648) return "Small " + Lang.GetItemNameValue(ItemID.SilverCoin) + " Stash";
                        if (tile.TileFrameX >= 648 && tile.TileFrameX < 684) return "Small " + Lang.GetItemNameValue(ItemID.GoldCoin) + " Stash";
                        if (tile.TileFrameX >= 684 && tile.TileFrameX < 720) return Lang.GetItemNameValue(ItemID.Amethyst) + " Stash";
                        if (tile.TileFrameX >= 720 && tile.TileFrameX < 756) return Lang.GetItemNameValue(ItemID.Topaz) + " Stash";
                        if (tile.TileFrameX >= 756 && tile.TileFrameX < 792) return Lang.GetItemNameValue(ItemID.Sapphire) + " Stash";
                        if (tile.TileFrameX >= 792 && tile.TileFrameX < 828) return Lang.GetItemNameValue(ItemID.Emerald) + " Stash";
                        if (tile.TileFrameX >= 828 && tile.TileFrameX < 864) return Lang.GetItemNameValue(ItemID.Ruby) + " Stash";
                        if (tile.TileFrameX >= 864 && tile.TileFrameX < 900) return Lang.GetItemNameValue(ItemID.Diamond) + " Stash";
                    }
                    return "Small Debris";
                case TileID.LargePiles:
                    if (tile.TileFrameX >= 868 && tile.TileFrameX < 972) return "Large " + Lang.GetItemNameValue(ItemID.CopperCoin) + " Stash";
                    if (tile.TileFrameX >= 972 && tile.TileFrameX < 1080) return "Large " + Lang.GetItemNameValue(ItemID.SilverCoin) + " Stash";
                    if (tile.TileFrameX >= 1080 && tile.TileFrameX < 1188) return "Large " + Lang.GetItemNameValue(ItemID.GoldCoin) + " Stash";
                    return "Large Debris";
                case TileID.LargePiles2:
                    if (tile.TileFrameY >= 0 && tile.TileFrameY < 36 && tile.TileFrameX >= 918 && tile.TileFrameX < 972) return Lang.GetItemNameValue(ItemID.EnchantedSword) + " Shrine";
                    return "Large Debris";
                case TileID.LivingWood:
                    return GetNameFromItem(ItemID.LivingWoodWand).Replace("Wand", "Block");
                case TileID.LeafBlock:
                    return GetNameFromItem(ItemID.LeafWand).Replace("Wand", "Block");
                case TileID.CrimsonGrass:
                    return GetNameFromItem(ItemID.CrimsonSeeds).Replace("Seeds", "Grass Block");
                case TileID.CrimsonPlants:
                    if (tile.TileFrameX == 270)
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

        public static string GetNameForManualWalls(Tile tile)
        {
            string wall = "Wall";
            string block = "Block";
            switch (tile.WallType)
            {
                case WallID.EbonstoneUnsafe:
                    return GetNameFromItem(ItemID.EbonstoneBlock).Replace(block, wall);
                case WallID.HellstoneBrickUnsafe:
                    return GetNameFromItem(ItemID.HellstoneBrickWall);
                case WallID.ObsidianBrickUnsafe:
                    return GetNameFromItem(ItemID.ObsidianBrickWall);
                case WallID.MudUnsafe:
                    return GetNameFromItem(ItemID.MudBlock).Replace(block, wall);
                case WallID.DirtUnsafe:
                    return GetNameFromItem(ItemID.DirtBlock).Replace(block, wall);
                case WallID.BlueDungeonUnsafe:
                    return GetNameFromItem(ItemID.BlueBrickWall);
                case WallID.GreenDungeonUnsafe:
                    return GetNameFromItem(ItemID.GreenBrickWall);
                case WallID.PinkDungeonUnsafe:
                    return GetNameFromItem(ItemID.PinkBrickWall);
                case WallID.SnowWallUnsafe:
                    return GetNameFromItem(ItemID.SnowBlock).Replace(block, wall);
                case WallID.AmethystUnsafe:
                    return GetNameFromItem(ItemID.Amethyst) + " " + GetNameFromItem(ItemID.StoneWall);
                case WallID.TopazUnsafe:
                    return GetNameFromItem(ItemID.Topaz) + " " + GetNameFromItem(ItemID.StoneWall);
                case WallID.SapphireUnsafe:
                    return GetNameFromItem(ItemID.Sapphire) + " " + GetNameFromItem(ItemID.StoneWall);
                case WallID.EmeraldUnsafe:
                    return GetNameFromItem(ItemID.Emerald) + " " + GetNameFromItem(ItemID.StoneWall);
                case WallID.RubyUnsafe:
                    return GetNameFromItem(ItemID.Ruby) + " " + GetNameFromItem(ItemID.StoneWall);
                case WallID.DiamondUnsafe:
                    return GetNameFromItem(ItemID.Diamond) + " " + GetNameFromItem(ItemID.StoneWall);
                case WallID.CaveUnsafe:
                    return "Green Mossy " + wall;
                case WallID.Cave2Unsafe:
                    return "Brown Mossy " + wall;
                case WallID.Cave3Unsafe:
                    return "Red Mossy " + wall;
                case WallID.Cave4Unsafe:
                    return "Blue Mossy " + wall;
                case WallID.Cave5Unsafe:
                    return "Purple Mossy " + wall;
                case WallID.Cave6Unsafe:
                    return "Rocky " + GetNameFromItem(ItemID.DirtWall);
                case WallID.Cave7Unsafe:
                    return "Old " + GetNameFromItem(ItemID.StoneWall);
                case WallID.SpiderUnsafe:
                    return "Spider " + wall;
                case WallID.GrassUnsafe:
                    return GetNameFromItem(ItemID.GrassWall);
                case WallID.JungleUnsafe:
                    return GetNameFromItem(ItemID.JungleWall);
                case WallID.FlowerUnsafe:
                    return GetNameFromItem(ItemID.FlowerWall);
                case WallID.CorruptGrassUnsafe:
                    return "Corrupt Grass " + wall;
                case WallID.HallowedGrassUnsafe:
                    return "Hallowed Grass " + wall;
                case WallID.IceUnsafe:
                    return GetNameFromItem(ItemID.IceBlock).Replace(block, wall);
                case WallID.ObsidianBackUnsafe:
                    return GetNameFromItem(ItemID.Obsidian) + " Back " + wall;
                case WallID.MushroomUnsafe:
                    return GetNameFromItem(ItemID.MushroomWall);
                case WallID.CrimsonGrassUnsafe:
                    return "Crimson Grass " + wall;
                case WallID.CrimstoneUnsafe:
                    return GetNameFromItem(ItemID.CrimstoneBlock).Replace(block, wall);
                case WallID.HiveUnsafe:
                    return GetNameFromItem(ItemID.HiveWall);
                case WallID.LihzahrdBrickUnsafe:
                    return GetNameFromItem(ItemID.LihzahrdBrickWall);
                case WallID.BlueDungeonSlabUnsafe:
                    return GetNameFromItem(ItemID.BlueSlabWall);
                case WallID.BlueDungeonTileUnsafe:
                    return GetNameFromItem(ItemID.BlueTiledWall);
                case WallID.PinkDungeonSlabUnsafe:
                    return GetNameFromItem(ItemID.PinkSlabWall);
                case WallID.PinkDungeonTileUnsafe:
                    return GetNameFromItem(ItemID.PinkTiledWall);
                case WallID.GreenDungeonSlabUnsafe:
                    return GetNameFromItem(ItemID.GreenSlabWall);
                case WallID.GreenDungeonTileUnsafe:
                    return GetNameFromItem(ItemID.GreenTiledWall);
                case WallID.CaveWall:
                    return "Cave " + GetNameFromItem(ItemID.DirtWall);
                case WallID.CaveWall2:
                    return "Rough " + GetNameFromItem(ItemID.DirtWall);
                case WallID.MarbleUnsafe:
                    return GetNameFromItem(ItemID.MarbleWall);
                case WallID.GraniteUnsafe:
                    return GetNameFromItem(ItemID.GraniteWall);
                case WallID.Cave8Unsafe:
                    return "Craggy " + GetNameFromItem(ItemID.StoneWall);
                case WallID.CorruptionUnsafe1:
                    return "Corrupt Growth " + wall;
                case WallID.CorruptionUnsafe2:
                    return "Corrupt Mass " + wall;
                case WallID.CorruptionUnsafe3:
                    return "Corrupt Pustule " + wall;
                case WallID.CorruptionUnsafe4:
                    return "Corrupt Tendtil " + wall;
                case WallID.CrimsonUnsafe1:
                    return "Crimson Crust " + wall;
                case WallID.CrimsonUnsafe2:
                    return "Crimson Scab " + wall;
                case WallID.CrimsonUnsafe3:
                    return "Crimson Teeth " + wall;
                case WallID.CrimsonUnsafe4:
                    return "Crimson Blister " + wall;
                case WallID.DirtUnsafe1:
                    return "Layered " + GetNameFromItem(ItemID.DirtWall);
                case WallID.DirtUnsafe2:
                    return "Crumbling " + GetNameFromItem(ItemID.DirtWall);
                case WallID.DirtUnsafe3:
                    return "Cracked " + GetNameFromItem(ItemID.DirtWall);
                case WallID.DirtUnsafe4:
                    return "Wavy " + GetNameFromItem(ItemID.DirtWall);
                case WallID.HallowUnsafe1:
                    return "Hallowed Prism " + wall;
                case WallID.HallowUnsafe2:
                    return "Hallowed Cavern " + wall;
                case WallID.HallowUnsafe3:
                    return "Hallowed Shard " + wall;
                case WallID.HallowUnsafe4:
                    return "Hallowed Cystalline " + wall;
                case WallID.JungleUnsafe1:
                    return "Lichen Stone " + wall;
                case WallID.JungleUnsafe2:
                    return "Leafy Jungle " + wall;
                case WallID.JungleUnsafe3:
                    return "Ivy Stone " + wall;
                case WallID.JungleUnsafe4:
                    return "Jungle Vine " + wall;
                case WallID.LavaUnsafe1:
                    return "Ember " + wall;
                case WallID.LavaUnsafe2:
                    return "Cinder " + wall;
                case WallID.LavaUnsafe3:
                    return "Magma " + wall;
                case WallID.LavaUnsafe4:
                    return "Smouldering Stone " + wall;
                case WallID.RocksUnsafe1:
                    return "Worn " + GetNameFromItem(ItemID.StoneWall);
                case WallID.RocksUnsafe2:
                    return "Stalactite " + GetNameFromItem(ItemID.StoneWall);
                case WallID.RocksUnsafe3:
                    return "Mottled " + GetNameFromItem(ItemID.StoneWall);
                case WallID.RocksUnsafe4:
                    return "Fractured " + GetNameFromItem(ItemID.StoneWall);
            }
            return null;
        }

        public static string GetNameForLiquids(Tile tile)
        {
            if(tile.LiquidType > 0)
            {
                if(tile.LiquidType == LiquidID.Lava)
                {
                    return "Lava";
                }
                if(tile.LiquidType == LiquidID.Honey)
                {
                    return "Honey";
                }
                if(tile.LiquidType == LiquidID.Water)
                {
                    const string water = "Water";
                    switch (Main.waterStyle)
                    {
                        case 0:
                            return water;
                        case 1:
                            return "Lava";
                        case 2: 
                            return "Corrupt " + water;
                        case 3:
                            return "Jungle " + water;
                        case 4:
                            return "Hallowed " + water;
                        case 5:
                            return "Tundra " + water;
                        case 6:
                            return "Desert " + water;
                        case 7:
                            return "Underground " + water;
                        case 8:
                            return "Cavern " + water;
                        case 9:
                            return "Blood Moon " + water;
                        case 10:
                            return "Crimson " + water;
                        case 11:
                            return "Honey";
                        case 12:
                            return "Desert " + water;
                    }
                }
            }
            return null;
        }

        public static string GetNameForTree(TreeContext context)
        {
            string tree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Trees, 0));
            string toAppend = "";
            if (context.Tile.TileType == TileID.Trees)
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
            if (context.Tile.TileType == TileID.PalmTree)
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
            if (TileID.Sets.TreeSapling[context.Tile.TileType])
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
            if (context.Tile.TileType == TileID.Cactus)
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
                        int dropId = mTile.ItemDrop;
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
            if(tile.TileType == TileID.Containers)
            {
                int style = tile.TileFrameX / 36;
                if(style < Lang.chestType.Length)
                {
                    return Lang.chestType[style].Value;
                }                
            }
            else if(tile.TileType == TileID.Containers2)
            {
                int style = tile.TileFrameX / 36;
                if (style < Lang.chestType2.Length)
                {
                    return Lang.chestType2[style].Value;
                }
            }
            return null;
        }

        public static string GetModName(TileContext context)
        {
            switch (context.TileType)
            {
                case TileType.Tile:
                    ModTile mTile = TileLoader.GetTile(context.Tile.TileType);
                    if (mTile != null)
                    {
                        return mTile.Mod.DisplayName;
                    }
                    break;
                case TileType.Wall:
                    ModWall mWall = WallLoader.GetWall(context.Tile.WallType);
                    if(mWall != null)
                    {
                        return mWall.Mod.DisplayName;
                    }
                    break;
            }
            return "Terraria";
        }
    }
}
