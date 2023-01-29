﻿using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Context;

namespace Twaila.Util
{
    public static class NameUtil
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
            string name = item.DisplayName.Value;
            if (string.IsNullOrEmpty(name))
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
            if (!string.IsNullOrEmpty(mapName))
            {
                return mapName;
            }
            if (!string.IsNullOrEmpty(altMapName))
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
					return Language.GetTextValue("Mods.Twaila.Tiles.Grass");
                case TileID.Plants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.Mushroom);
                    return Language.GetTextValue("Mods.Twaila.Tiles.Plants");
				case TileID.CorruptGrass:
                    return Language.GetTextValue("Mods.Twaila.Tiles.CorruptGrass");
				case TileID.CorruptPlants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.VileMushroom);
                    return Language.GetTextValue("Mods.Twaila.Tiles.CorruptPlants");
				case TileID.Sunflower:
					return GetNameFromItem(ItemID.Sunflower);
                case TileID.ClosedDoor:
                    if (tile.TileFrameY >= 594 && tile.TileFrameY <= 630 && tile.TileFrameX <= 36)
                        return Language.GetTextValue("Mods.Twaila.Tiles.ClosedDoor.Lihzahrd");
					break;
                case TileID.Vines:
                    return GetNameFromItem(ItemID.Vine);
                case TileID.JungleGrass:
                    return Language.GetTextValue("Mods.Twaila.Tiles.JungleGrass");
				case TileID.CrimsonThorns:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.CorruptThorns, 0));
                case TileID.JunglePlants:
                    if(tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.JungleSpores);
					return Language.GetTextValue("Mods.Twaila.Tiles.JunglePlants");
				case TileID.JungleVines:
                    return Language.GetTextValue("Mods.Twaila.Tiles.JungleVines");
				case TileID.MushroomGrass:
                    return Language.GetTextValue("Mods.Twaila.Tiles.MushroomGrass");
				case TileID.Plants2:
					return Language.GetTextValue("Mods.Twaila.Tiles.Plants2");
				case TileID.JunglePlants2:
					return Language.GetTextValue("Mods.Twaila.Tiles.JunglePlants2");
				case TileID.HallowedGrass:
                    return Language.GetTextValue("Mods.Twaila.Tiles.HallowedGrass");
				case TileID.HallowedPlants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.Mushroom);
                    return Language.GetTextValue("Mods.Twaila.Tiles.HallowedPlants");
				case TileID.HallowedPlants2:
                    return Language.GetTextValue("Mods.Twaila.Tiles.HallowedPlants2");
				case TileID.HallowedVines:
                    return Language.GetTextValue("Mods.Twaila.Tiles.HallowedVines");
				case TileID.DiscoBall:
                    return GetNameFromItem(ItemID.DiscoBall);
                case TileID.MagicalIceBlock:
                    return Language.GetTextValue("Mods.Twaila.Tiles.MagicalIceBlock");
				case TileID.BreakableIce:
                    return Language.GetTextValue("Mods.Twaila.Tiles.BreakableIce");
                case TileID.Stalactite:
                    return Language.GetTextValue("Mods.Twaila.Tiles.Stalactite");
				case TileID.GreenMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.GreenMoss");
				case TileID.BrownMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.BrownMoss");
                case TileID.RedMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.RedMoss");
                case TileID.BlueMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.BlueMoss");
                case TileID.PurpleMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.PurpleMoss");
                case TileID.LavaMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.LavaMoss");
                case TileID.KryptonMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.KryptonMoss");
                case TileID.XenonMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.XenonMoss");
                case TileID.ArgonMoss:
                    return Language.GetTextValue("Mods.Twaila.Tiles.ArgonMoss");
                case TileID.SmallPiles:
                    if(tile.TileFrameY == 18)
                    {
                        if(tile.TileFrameX >= 576 && tile.TileFrameX < 612) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Copper");
						if (tile.TileFrameX >= 612 && tile.TileFrameX < 648) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Silver");
						if (tile.TileFrameX >= 648 && tile.TileFrameX < 684) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Gold");
                        if (tile.TileFrameX >= 684 && tile.TileFrameX < 720) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Amethyst");
						if (tile.TileFrameX >= 720 && tile.TileFrameX < 756) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Topaz");
                        if (tile.TileFrameX >= 756 && tile.TileFrameX < 792) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Sapphire");
                        if (tile.TileFrameX >= 792 && tile.TileFrameX < 828) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Emerald");
                        if (tile.TileFrameX >= 828 && tile.TileFrameX < 864) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Ruby");
                        if (tile.TileFrameX >= 864 && tile.TileFrameX < 900) return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Diamond");
                    }
                    return Language.GetTextValue("Mods.Twaila.Tiles.SmallPiles.Default");
				case TileID.LargePiles:
                    if (tile.TileFrameX >= 868 && tile.TileFrameX < 972) return Language.GetTextValue("Mods.Twaila.Tiles.LargePiles.Copper");
                    if (tile.TileFrameX >= 972 && tile.TileFrameX < 1080) return Language.GetTextValue("Mods.Twaila.Tiles.LargePiles.Silver");
                    if (tile.TileFrameX >= 1080 && tile.TileFrameX < 1188) return Language.GetTextValue("Mods.Twaila.Tiles.LargePiles.Gold");
                    return Language.GetTextValue("Mods.Twaila.Tiles.LargePiles.Default");
				case TileID.LargePiles2:
                    if (tile.TileFrameY >= 0 && tile.TileFrameY < 36 && tile.TileFrameX >= 918 && tile.TileFrameX < 972)
						return Language.GetTextValue("Mods.Twaila.Tiles.LargePiles2.EnchantedSword");
					return Language.GetTextValue("Mods.Twaila.Tiles.LargePiles2.Default");
				case TileID.LivingWood:
                    return Language.GetTextValue("Mods.Twaila.Tiles.LivingWood");
				case TileID.LeafBlock:
                    return Language.GetTextValue("Mods.Twaila.Tiles.LeafBlock");
				case TileID.CrimsonGrass:
                    return Language.GetTextValue("Mods.Twaila.Tiles.CrimsonGrass");
				case TileID.CrimsonPlants:
                    if (tile.TileFrameX == 270)
                        return GetNameFromItem(ItemID.ViciousMushroom);
                    return Language.GetTextValue("Mods.Twaila.Tiles.CrimsonPlants");
				case TileID.CrimsonVines:
                    return Language.GetTextValue("Mods.Twaila.Tiles.CrimsonVines");
				case TileID.Hive:
                    return GetNameFromItem(ItemID.Hive);
                case TileID.PlantDetritus:
                    return Language.GetTextValue("Mods.Twaila.Tiles.PlantDetritus");
				case TileID.WaterDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.WaterDrip, 0));
                case TileID.LavaDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.LavaDrip, 0));
                case TileID.HoneyDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.HoneyDrip, 0));
                case TileID.VineFlowers:
                    return Language.GetTextValue("Mods.Twaila.Tiles.VineFlowers");
                case TileID.LivingMahogany:
                    return Language.GetTextValue("Mods.Twaila.Tiles.LivingMahogany");
                case TileID.LivingMahoganyLeaves:
                    return Language.GetTextValue("Mods.Twaila.Tiles.LivingMahoganyLeaves");
                case TileID.SandDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.SandDrip, 0));
                case TileID.Pumpkins:
                    return GetNameFromItem(ItemID.Pumpkin);
                case TileID.GolfGrass:
                    return Language.GetTextValue("Mods.Twaila.Tiles.GolfGrass");
				case TileID.GolfGrassHallowed:
                    return Language.GetTextValue("Mods.Twaila.Tiles.GolfGrassHallowed");
				case TileID.MysticSnakeRope:
                    return Language.GetTextValue("Mods.Twaila.Tiles.MysticSnakeRope");
				case TileID.GreenMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.GreenMossBrick");
				case TileID.BrownMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.BrownMossBrick");
				case TileID.RedMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.RedMossBrick");
                case TileID.BlueMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.BlueMossBrick");
                case TileID.PurpleMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.PurpleMossBrick");
                case TileID.LavaMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.LavaMossBrick");
                case TileID.KryptonMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.KryptonMossBrick");
                case TileID.XenonMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.XenonMossBrick");
                case TileID.ArgonMossBrick:
                    return Language.GetTextValue("Mods.Twaila.Tiles.ArgonMossBrick");
                case TileID.LilyPad:
                    return Language.GetTextValue("Mods.Twaila.Tiles.LilyPad");
				case TileID.Cattail:
                    return Language.GetTextValue("Mods.Twaila.Tiles.Cattail");
				case TileID.MushroomVines:
                    return Language.GetTextValue("Mods.Twaila.Tiles.MushroomVines");
				case TileID.SeaOats:
                    return Language.GetTextValue("Mods.Twaila.Tiles.SeaOats");
				case TileID.OasisPlants:
                    return Language.GetTextValue("Mods.Twaila.Tiles.OasisPlants");
				case TileID.Sandcastles:
                    return Language.GetTextValue("Mods.Twaila.Tiles.Sandcastles");
				case TileID.Grate:
                    return Language.GetTextValue("Mods.Twaila.Tiles.Grate");
				case TileID.GrateClosed:
                    return Language.GetTextValue("Mods.Twaila.Tiles.GrateClosed");
				case TileID.VanityTreeSakura:
                    return Language.GetTextValue("Mods.Twaila.Tiles.VanityTreeSakura");
				case TileID.VanityTreeYellowWillow:
                    return Language.GetTextValue("Mods.Twaila.Tiles.VanityTreeYellowWillow");
				case TileID.TreeTopaz:
                    return Language.GetTextValue("Mods.Twaila.Tiles.TreeTopaz");
				case TileID.TreeAmethyst:
                    return Language.GetTextValue("Mods.Twaila.Tiles.TreeAmethyst");
				case TileID.TreeSapphire:
                    return Language.GetTextValue("Mods.Twaila.Tiles.TreeSapphire");
                case TileID.TreeEmerald:
                    return Language.GetTextValue("Mods.Twaila.Tiles.TreeEmerald");
                case TileID.TreeRuby:
                    return Language.GetTextValue("Mods.Twaila.Tiles.TreeRuby");
                case TileID.TreeDiamond:
                    return Language.GetTextValue("Mods.Twaila.Tiles.TreeDiamond");
                case TileID.TreeAmber:
                    return Language.GetTextValue("Mods.Twaila.Tiles.TreeAmber");
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
					return TwailaConfig.Get().AntiCheat.HideSuspiciousTiles ? GetNameFromItem(ItemID.LifeCrystal) : GetNameFromItem(ItemID.LifeCrystalBoulder);
				case TileID.DirtiestBlock:
					return TwailaConfig.Get().AntiCheat.HideSuspiciousTiles ? GetNameFromItem(ItemID.DirtBlock) : GetNameFromItem(ItemID.DirtiestBlock);
			}
            return null;
        }

        public static string GetNameForLiquids(Tile tile)
        {
            if(tile.LiquidAmount > 0)
            {
                if(tile.LiquidType == LiquidID.Lava)
                {
                    return Language.GetTextValue("Mods.Twaila.Liquids.Lava");
				}
                if(tile.LiquidType == LiquidID.Honey)
                {
                    return Language.GetTextValue("Mods.Twaila.Liquids.Honey");
				}
				if(tile.LiquidType == LiquidID.Shimmer)
				{
					return Language.GetTextValue("Mods.Twaila.Liquids.Shimmer");
				}
                if(tile.LiquidType == LiquidID.Water)
                {
                    switch (Main.waterStyle)
                    {
                        case WaterStyleID.Purity:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Purity");
                        case WaterStyleID.Lava:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Lava");
						case WaterStyleID.Corrupt: 
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Corrupt");
						case WaterStyleID.Jungle:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Jungle");
						case WaterStyleID.Hallow:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Hallow");
						case WaterStyleID.Snow:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Snow");
						case WaterStyleID.Desert:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Desert");
						case WaterStyleID.Underground:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Underground");
						case WaterStyleID.Cavern:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Cavern");
						case WaterStyleID.Bloodmoon:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Bloodmoon");
						case WaterStyleID.Crimson:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.Crimson");
						case WaterStyleID.Honey:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Honey");
						case WaterStyleID.UndergroundDesert:
                            return Language.GetTextValue("Mods.Twaila.Liquids.Water.UndergroundDesert");
						case 13:
							return Language.GetTextValue("Mods.Twaila.Liquids.Water.Ocean");
					}
                    if(Main.waterStyle >= Main.maxLiquidTypes)
                    {
                        return GetInternalLiquidName(Main.waterStyle, false).SplitPascalCase();
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
                        return mItem == null ? mTile.Name : mItem.DisplayName.Value + " " + cactus;
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

        public static string SplitPascalCase(this string word)
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
