using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Context;

namespace Twaila.Util
{
    public class NameUtil
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
                case TileID.ClosedDoor:
                    if (tile.TileFrameY >= 594 && tile.TileFrameY <= 630 && tile.TileFrameX <= 36)
                        return "Locked " + GetNameFromItem(ItemID.LihzahrdDoor);
                    break;
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
				case TileID.VioletMoss:
					return GetNameFromItem(ItemID.VioletMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
				case TileID.VioletMossBrick:
					return GetNameFromItem(ItemID.VioletMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
				case TileID.RainbowMoss:
					return GetNameFromItem(ItemID.RainbowMoss) + " " + GetNameFromItem(ItemID.StoneBlock);
				case TileID.RainbowMossBrick:
					return GetNameFromItem(ItemID.RainbowMoss) + " " + GetNameFromItem(ItemID.GrayBrick);
				case TileID.AshGrass:
					return GetNameFromItem(ItemID.AshGrassSeeds).Replace("Seeds", "Block");
				case TileID.TreeAsh:
					return GetNameFromItem(ItemID.AshWood).Replace("Wood", "Tree");
				case TileID.CorruptVines:
					return "Corrupt " + GetNameFromItem(ItemID.Vine);
				case TileID.AshPlants:
					return "Ash Plant";
				case TileID.AshVines:
					return "Ash " + GetNameFromItem(ItemID.Vine);
				case TileID.PlanteraThorns:
					return "Plantera " + Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.CorruptThorns, 0));
				case TileID.CorruptJungleGrass:
					return "Corrupt " + GetNameFromItem(ItemID.JungleGrassSeeds).Replace("Seeds", "Block");
				case TileID.CrimsonJungleGrass:
					return "Crimson " + GetNameFromItem(ItemID.JungleGrassSeeds).Replace("Seeds", "Block");
				case TileID.LifeCrystalBoulder:
					return TwailaConfig.Get().AntiCheat ? GetNameFromItem(ItemID.LifeCrystal) : GetNameFromItem(ItemID.LifeCrystalBoulder);
				case TileID.DirtiestBlock:
					return TwailaConfig.Get().AntiCheat ? GetNameFromItem(ItemID.DirtBlock) : GetNameFromItem(ItemID.DirtiestBlock);
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
				if(tile.LiquidType == LiquidID.Shimmer)
				{
					return "Shimmer";
				}
                if(tile.LiquidType == LiquidID.Water)
                {
                    const string water = "Water";
                    switch (Main.waterStyle)
                    {
                        case WaterStyleID.Purity:
                            return water;
                        case WaterStyleID.Lava:
                            return "Lava";
                        case WaterStyleID.Corrupt: 
                            return "Corrupt " + water;
                        case WaterStyleID.Jungle:
                            return "Jungle " + water;
                        case WaterStyleID.Hallow:
                            return "Hallowed " + water;
                        case WaterStyleID.Snow:
                            return "Tundra " + water;
                        case WaterStyleID.Desert:
                            return "Desert " + water;
                        case WaterStyleID.Underground:
                            return "Underground " + water;
                        case WaterStyleID.Cavern:
                            return "Cavern " + water;
                        case WaterStyleID.Bloodmoon:
                            return "Blood Moon " + water;
                        case WaterStyleID.Crimson:
                            return "Crimson " + water;
                        case WaterStyleID.Honey:
                            return "Honey";
                        case WaterStyleID.UndergroundDesert:
                            return "Desert " + water;
						case 13:
							return "Ocean " + water;
                    }
                    if(Main.waterStyle >= Main.maxLiquidTypes)
                    {
                        return SplitCamelCase(GetInternalLiquidName(Main.waterStyle, false));
                    }
                }
            }
            return null;
        }

        public static string GetNameForTree(int treeId, int dirtId)
        {
            string tree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Trees, 0));
            string toAppend = "";
            if (treeId == TileID.Trees)
            {
                toAppend = " " + tree;
                if (dirtId == TileID.Grass || dirtId == TileID.GolfGrass)
                {
                    return tree;
                }
            }
            int itemId = TreeUtil.GetTreeWood(dirtId);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + toAppend;
            }
            return null;
        }

        public static string GetNameForPalmTree(int treeId, int sandId)
        {
            string palmTree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.PalmTree, 0));
            string toAppend = "";
            if (treeId == TileID.PalmTree)
            {
                toAppend = " " + palmTree;
                if (sandId == TileID.Sand)
                {
                    return palmTree;
                }
            }
            int itemId = TreeUtil.GetTreeWood(sandId);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + toAppend;
            }
            return null;
        }

        public static string GetNameForSapling(int saplingId, int dirtId)
        {
            string tree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Trees, 0));
            string sapling = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Saplings, 0));
            string palmTree = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.PalmTree, 0));
            string toAppend = "";
            if (TileID.Sets.TreeSapling[saplingId])
            {
                if (TileLoader.CanGrowModPalmTree(dirtId) || dirtId == TileID.Crimsand ||
                    dirtId == TileID.Ebonsand || dirtId == TileID.Pearlsand)
                {
                    toAppend = " " + palmTree + " " + sapling;
                }
                else
                {
                    toAppend = " " + tree + " " + sapling;
                }
                if (dirtId == TileID.Grass)
                {
                    return tree + " " + sapling;
                }
                if (dirtId == TileID.Sand)
                {
                    return palmTree + " " + sapling;
                }
            }
            int itemId = TreeUtil.GetTreeWood(dirtId);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + toAppend;
            }
            return null;
        }

        public static string GetNameForCactus(int cactusId, int sandId)
        {
            string cactus = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Cactus, 0));
            if (cactusId == TileID.Cactus)
            {
                if (sandId == -1)
                {
                    return null;
                }
                if (TileLoader.CanGrowModCactus(sandId))
                {
                    ModTile mTile = TileLoader.GetTile(sandId);
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
                    switch (sandId)
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

        public static string SplitCamelCase(string word)
        {
            StringBuilder builder = new StringBuilder();
			if(word != null)
			{
				foreach (char letter in word)
				{
					if (char.IsUpper(letter) && builder.Length != 0)
					{
						builder.Append(' ');
					}
					builder.Append(letter);
				}
				return builder.ToString();
			}
			return word;
        }

        public static string GetName(TwailaConfig.NameType nameType, string displayName, string internalName, string fullName)
        {
            switch (nameType)
            {
                case TwailaConfig.NameType.DisplayName:
                    return displayName ?? internalName ?? fullName;
                case TwailaConfig.NameType.InternalName:
                    return internalName ?? fullName ?? displayName;
                case TwailaConfig.NameType.FullName:
                    return fullName ?? internalName ?? displayName;
            }
            return null;
        }

        public static string GetInternalTileName(int tileId, bool fullName)
        {
            ModTile mTile = TileLoader.GetTile(tileId);
            return fullName ? mTile?.GetType().FullName : mTile?.Name;
        }

        public static string GetInternalWallName(int wallId, bool fullName)
        {
            ModWall mWall = WallLoader.GetWall(wallId);
            return fullName ? mWall?.GetType().FullName : mWall?.Name;
        }

        public static string GetInternalLiquidName(int waterStyle, bool fullName)
        {
            ModWaterStyle mWater = LoaderManager.Get<WaterStylesLoader>().Get(waterStyle);
            return fullName ? mWater?.GetType().FullName : mWater?.Name;
        }
    }
}
