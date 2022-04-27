using Terraria;
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

        public static string GetNameFromMap(Tile tile, int x, int y)
        {
            string mapName = Lang.GetMapObjectName(Main.Map[x, y].Type);
            int style = TileObjectData.GetTileStyle(tile);
            string altMapName = Lang.GetMapObjectName(MapHelper.TileToLookup(tile.TileType, style == -1 ? 0 : style));
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
                    return GetNameFromItem(ItemID.GreenMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.BrownMoss:
                    return GetNameFromItem(ItemID.BrownMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.RedMoss:
                    return GetNameFromItem(ItemID.RedMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.BlueMoss:
                    return GetNameFromItem(ItemID.BlueMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.PurpleMoss:
                    return GetNameFromItem(ItemID.PurpleMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.LavaMoss:
                    return GetNameFromItem(ItemID.LavaMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.KryptonMoss:
                    return GetNameFromItem(ItemID.KryptonMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.XenonMoss:
                    return GetNameFromItem(ItemID.XenonMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
                case TileID.ArgonMoss:
                    return GetNameFromItem(ItemID.ArgonMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
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
                case TileID.GolfGrass:
                    return "Mowed Grass";
                case TileID.GolfGrassHallowed:
                    return "Mowed Hallowed Grass";
                case TileID.MysticSnakeRope:
                    return "Snake Rope";
                case TileID.GreenMossBrick:
                    return GetNameFromItem(ItemID.GreenMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.BrownMossBrick:
                    return GetNameFromItem(ItemID.BrownMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.RedMossBrick:
                    return GetNameFromItem(ItemID.RedMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.BlueMossBrick:
                    return GetNameFromItem(ItemID.BlueMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.PurpleMossBrick:
                    return GetNameFromItem(ItemID.PurpleMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.LavaMossBrick:
                    return GetNameFromItem(ItemID.LavaMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.KryptonMossBrick:
                    return GetNameFromItem(ItemID.KryptonMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.XenonMossBrick:
                    return GetNameFromItem(ItemID.XenonMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.ArgonMossBrick:
                    return GetNameFromItem(ItemID.ArgonMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
                case TileID.LilyPad:
                    return "Lily Pad";
                case TileID.Cattail:
                    return "Cattail";
                case TileID.MushroomVines:
                    return "Hanging Mushroom";
                case TileID.SeaOats:
                    return "Sea Oats";
                case TileID.OasisPlants:
                    return "Oasis Plant";
                case TileID.Sandcastles:
                    return "Sandcastle";
                case TileID.Grate:
                    return "Open Grate";
                case TileID.GrateClosed:
                    return "Closed Grate";
                case TileID.VanityTreeSakura:
                    return GetNameFromItem(ItemID.VanityTreeSakuraSeed).Replace("Sapling", "Tree");
                case TileID.VanityTreeYellowWillow:
                    return GetNameFromItem(ItemID.VanityTreeYellowWillowSeed).Replace("Sapling", "Tree");
                case TileID.TreeTopaz:
                    return GetNameFromItem(ItemID.Topaz) + " Tree";
                case TileID.TreeAmethyst:
                    return GetNameFromItem(ItemID.Amethyst) + " Tree";
                case TileID.TreeSapphire:
                    return GetNameFromItem(ItemID.Sapphire) + " Tree";
                case TileID.TreeEmerald:
                    return GetNameFromItem(ItemID.Emerald) + " Tree";
                case TileID.TreeRuby:
                    return GetNameFromItem(ItemID.Ruby) + " Tree";
                case TileID.TreeDiamond:
                    return GetNameFromItem(ItemID.Diamond) + " Tree";
                case TileID.TreeAmber:
                    return GetNameFromItem(ItemID.Amber) + " Tree";
                case TileID.Bamboo:
                    return GetNameFromItem(ItemID.BambooBlock);
                case TileID.Seaweed:
                    return GetNameFromItem(ItemID.Seaweed);
            }
            return null;
        }

        public static string GetNameForLiquids(Tile tile)
        {
            if(tile.LiquidAmount > 0)
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
            if (context.Tile.TileId == TileID.Trees)
            {
                toAppend = " " + tree;
                if (context.TreeDirt == TileID.Grass || context.TreeDirt == TileID.GolfGrass)
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
            if (context.Tile.TileId == TileID.PalmTree)
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
            if (TileID.Sets.TreeSapling[context.Tile.TileId])
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
            if (context.Tile.TileId == TileID.Cactus)
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
                    ModTile mTile = TileLoader.GetTile(context.Tile.TileId);
                    if (mTile != null)
                    {
                        return mTile.Mod.DisplayName;
                    }
                    break;
                case TileType.Wall:
                    ModWall mWall = WallLoader.GetWall(context.Tile.WallId);
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
