using System;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.ObjectData;

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
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Grass");
                case TileID.Plants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.Mushroom);
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Plants");
                case TileID.CorruptGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CorruptGrass");
                case TileID.CorruptPlants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.VileMushroom);
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CorruptPlants");
                case TileID.Sunflower:
                    return GetNameFromItem(ItemID.Sunflower);
                case TileID.ClosedDoor:
                    if (tile.TileFrameY >= 594 && tile.TileFrameY <= 630 && tile.TileFrameX <= 36)
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.ClosedDoor.Lihzahrd");
                    break;
                case TileID.Vines:
                    return GetNameFromItem(ItemID.Vine);
                case TileID.JungleGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.JungleGrass");
                case TileID.CrimsonThorns:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.CorruptThorns, 0));
                case TileID.JunglePlants:
                    if(tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.JungleSpores);
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.JunglePlants");
                case TileID.JungleVines:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.JungleVines");
                case TileID.MushroomGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.MushroomGrass");
                case TileID.Plants2:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Plants2");
                case TileID.JunglePlants2:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.JunglePlants2");
                case TileID.HallowedGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.HallowedGrass");
                case TileID.HallowedPlants:
                    if (tile.TileFrameX == 144)
                        return GetNameFromItem(ItemID.Mushroom);
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.HallowedPlants");
                case TileID.HallowedPlants2:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.HallowedPlants2");
                case TileID.HallowedVines:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.HallowedVines");
                case TileID.DiscoBall:
                    return GetNameFromItem(ItemID.DiscoBall);
                case TileID.MagicalIceBlock:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.MagicalIceBlock");
                case TileID.BreakableIce:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.BreakableIce");
                case TileID.Stalactite:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Stalactite");
                case TileID.GreenMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.GreenMoss");
                case TileID.BrownMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.BrownMoss");
                case TileID.RedMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.RedMoss");
                case TileID.BlueMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.BlueMoss");
                case TileID.PurpleMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PurpleMoss");
                case TileID.LavaMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LavaMoss");
                case TileID.KryptonMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.KryptonMoss");
                case TileID.XenonMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.XenonMoss");
                case TileID.ArgonMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.ArgonMoss");
                case TileID.SmallPiles:
                    if(tile.TileFrameY == 18)
                    {
                        if(tile.TileFrameX >= 576 && tile.TileFrameX < 612) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Copper");
                        if (tile.TileFrameX >= 612 && tile.TileFrameX < 648) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Silver");
                        if (tile.TileFrameX >= 648 && tile.TileFrameX < 684) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Gold");
                        if (tile.TileFrameX >= 684 && tile.TileFrameX < 720) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Amethyst");
                        if (tile.TileFrameX >= 720 && tile.TileFrameX < 756) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Topaz");
                        if (tile.TileFrameX >= 756 && tile.TileFrameX < 792) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Sapphire");
                        if (tile.TileFrameX >= 792 && tile.TileFrameX < 828) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Emerald");
                        if (tile.TileFrameX >= 828 && tile.TileFrameX < 864) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Ruby");
                        if (tile.TileFrameX >= 864 && tile.TileFrameX < 900) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Diamond");
                    }
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Default");
                case TileID.SmallPiles1x1Echo:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Default");
                case TileID.SmallPiles2x1Echo:
                    if (tile.TileFrameY == 0)
                    {
                        if (tile.TileFrameX >= 576 && tile.TileFrameX < 612) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Copper");
                        if (tile.TileFrameX >= 612 && tile.TileFrameX < 648) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Silver");
                        if (tile.TileFrameX >= 648 && tile.TileFrameX < 684) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Gold");
                        if (tile.TileFrameX >= 684 && tile.TileFrameX < 720) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Amethyst");
                        if (tile.TileFrameX >= 720 && tile.TileFrameX < 756) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Topaz");
                        if (tile.TileFrameX >= 756 && tile.TileFrameX < 792) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Sapphire");
                        if (tile.TileFrameX >= 792 && tile.TileFrameX < 828) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Emerald");
                        if (tile.TileFrameX >= 828 && tile.TileFrameX < 864) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Ruby");
                        if (tile.TileFrameX >= 864 && tile.TileFrameX < 900) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Diamond");
                    }
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SmallPiles.Default");
                case TileID.LargePiles:
                case TileID.LargePilesEcho:
                    if (tile.TileFrameX >= 868 && tile.TileFrameX < 972) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LargePiles.Copper");
                    if (tile.TileFrameX >= 972 && tile.TileFrameX < 1080) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LargePiles.Silver");
                    if (tile.TileFrameX >= 1080 && tile.TileFrameX < 1188) return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LargePiles.Gold");
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LargePiles.Default");
                case TileID.LargePiles2:
                case TileID.LargePiles2Echo:
                    if (tile.TileFrameY >= 0 && tile.TileFrameY < 36 && tile.TileFrameX >= 918 && tile.TileFrameX < 972)
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LargePiles2.EnchantedSword");
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LargePiles2.Default");
                case TileID.LivingWood:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LivingWood");
                case TileID.LeafBlock:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LeafBlock");
                case TileID.CrimsonGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CrimsonGrass");
                case TileID.CrimsonPlants:
                    if (tile.TileFrameX == 270)
                        return GetNameFromItem(ItemID.ViciousMushroom);
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CrimsonPlants");
                case TileID.CrimsonVines:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CrimsonVines");
                case TileID.Hive:
                    return GetNameFromItem(ItemID.Hive);
                case TileID.PlantDetritus:
                case TileID.PlantDetritus2x2Echo:
                case TileID.PlantDetritus3x2Echo:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PlantDetritus");
                case TileID.WaterDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.WaterDrip, 0));
                case TileID.LavaDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.LavaDrip, 0));
                case TileID.HoneyDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.HoneyDrip, 0));
                case TileID.VineFlowers:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.VineFlowers");
                case TileID.LivingMahogany:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LivingMahogany");
                case TileID.LivingMahoganyLeaves:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LivingMahoganyLeaves");
                case TileID.SandDrip:
                    return Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.SandDrip, 0));
                case TileID.Pumpkins:
                    return GetNameFromItem(ItemID.Pumpkin);
                case TileID.GolfGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.GolfGrass");
                case TileID.GolfGrassHallowed:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.GolfGrassHallowed");
                case TileID.MysticSnakeRope:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.MysticSnakeRope");
                case TileID.GreenMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.GreenMossBrick");
                case TileID.BrownMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.BrownMossBrick");
                case TileID.RedMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.RedMossBrick");
                case TileID.BlueMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.BlueMossBrick");
                case TileID.PurpleMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PurpleMossBrick");
                case TileID.LavaMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LavaMossBrick");
                case TileID.KryptonMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.KryptonMossBrick");
                case TileID.XenonMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.XenonMossBrick");
                case TileID.ArgonMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.ArgonMossBrick");
                case TileID.LilyPad:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.LilyPad");
                case TileID.Cattail:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Cattail");
                case TileID.MushroomVines:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.MushroomVines");
                case TileID.SeaOats:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.SeaOats");
                case TileID.OasisPlants:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.OasisPlants");
                case TileID.Sandcastles:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Sandcastles");
                case TileID.Grate:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Grate");
                case TileID.GrateClosed:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.GrateClosed");
                case TileID.VanityTreeSakura:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.VanityTreeSakura");
                case TileID.VanityTreeYellowWillow:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.VanityTreeYellowWillow");
                case TileID.TreeTopaz:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeTopaz");
                case TileID.TreeAmethyst:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeAmethyst");
                case TileID.TreeSapphire:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeSapphire");
                case TileID.TreeEmerald:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeEmerald");
                case TileID.TreeRuby:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeRuby");
                case TileID.TreeDiamond:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeDiamond");
                case TileID.TreeAmber:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeAmber");
                case TileID.Bamboo:
                    return GetNameFromItem(ItemID.BambooBlock);
                case TileID.Seaweed:
                    return GetNameFromItem(ItemID.Seaweed);
                case TileID.VioletMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.VioletMoss");
                case TileID.VioletMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.VioletMossBrick");
                case TileID.RainbowMoss:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.RainbowMoss");
                case TileID.RainbowMossBrick:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.RainbowMossBrick");
                case TileID.AshGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.AshGrass");
                case TileID.TreeAsh:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.TreeAsh");
                case TileID.CorruptVines:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CorruptVines");
                case TileID.AshPlants:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.AshPlants");
                case TileID.AshVines:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.AshVines");
                case TileID.PlanteraThorns:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PlanteraThorns");
                case TileID.CorruptJungleGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CorruptJungleGrass");
                case TileID.CrimsonJungleGrass:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.CrimsonJungleGrass");
                case TileID.LifeCrystalBoulder:
                    return TwailaConfig.Instance.AntiCheat.HideSuspiciousTiles ? GetNameFromItem(ItemID.LifeCrystal) : GetNameFromItem(ItemID.LifeCrystalBoulder);
                case TileID.DirtiestBlock:
                    return TwailaConfig.Instance.AntiCheat.HideSuspiciousTiles ? GetNameFromItem(ItemID.DirtBlock) : GetNameFromItem(ItemID.DirtiestBlock);
            }
            return null;
        }

        public static string GetNameForLiquids(Tile tile)
        {
            if(tile.LiquidAmount > 0)
            {
                if(tile.LiquidType == LiquidID.Lava)
                {
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Lava");
                }
                if(tile.LiquidType == LiquidID.Honey)
                {
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Honey");
                }
                if(tile.LiquidType == LiquidID.Shimmer)
                {
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Shimmer");
                }
                if(tile.LiquidType == LiquidID.Water)
                {
                    switch (Main.waterStyle)
                    {
                        case WaterStyleID.Purity:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Purity");
                        case WaterStyleID.Lava:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Lava");
                        case WaterStyleID.Corrupt: 
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Corrupt");
                        case WaterStyleID.Jungle:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Jungle");
                        case WaterStyleID.Hallow:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Hallow");
                        case WaterStyleID.Snow:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Snow");
                        case WaterStyleID.Desert:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Desert");
                        case WaterStyleID.Underground:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Underground");
                        case WaterStyleID.Cavern:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Cavern");
                        case WaterStyleID.Bloodmoon:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Bloodmoon");
                        case WaterStyleID.Crimson:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Crimson");
                        case WaterStyleID.Honey:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Honey");
                        case WaterStyleID.UndergroundDesert:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.UndergroundDesert");
                        case 13:
                            return Language.GetTextValue("Mods.Twaila.ManualNames.Liquids.Water.Ocean");
                    }
                    if(Main.waterStyle >= Main.maxLiquidTypes)
                    {
                        return GetInternalLiquidName(Main.waterStyle, false).SplitPascalCase();
                    }
                }
            }
            return null;
        }

        public static string GetNameForTree(int dirtId)
        {
            switch (WorldGen.GetTreeType(dirtId))
            {
                case TreeTypes.Forest:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Trees.Forest");
                case TreeTypes.Corrupt:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Trees.Corrupt");
                case TreeTypes.Mushroom:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Trees.Mushroom");
                case TreeTypes.Crimson:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Trees.Crimson");
                case TreeTypes.Jungle:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Trees.Jungle");
                case TreeTypes.Snow:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Trees.Snow");
                case TreeTypes.Hallowed:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Trees.Hallowed");
            }
            int itemId = TreeUtil.GetTreeWood(dirtId);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + " " + Language.GetTextValue("MapObject.Tree");
            }
            return null;
        }

        public static string GetNameForPalmTree(int sandId)
        {
            switch (WorldGen.GetTreeType(sandId))
            {
                case TreeTypes.Palm:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PalmTree.Default");
                case TreeTypes.PalmCrimson:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PalmTree.PalmCrimson");
                case TreeTypes.PalmCorrupt:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PalmTree.PalmCorrupt");
                case TreeTypes.PalmHallowed:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.PalmTree.PalmHallowed");
            }
            int itemId = TreeUtil.GetTreeWood(sandId);
            if (itemId != -1)
            {
                return Lang.GetItemNameValue(itemId) + " " + Language.GetTextValue("MapObject.PalmTree");
            }
            return null;
        }

        public static string GetNameForSapling(int tileId, int dirtId)
        {
            if (TileID.Sets.TreeSapling[tileId])
            {
                switch (WorldGen.GetTreeType(dirtId))
                {
                    case TreeTypes.Forest:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Forest");
                    case TreeTypes.Corrupt:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Corrupt");
                    case TreeTypes.Crimson:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Crimson");
                    case TreeTypes.Jungle:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Jungle");
                    case TreeTypes.Snow:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Snow");
                    case TreeTypes.Hallowed:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Hallowed");
                    case TreeTypes.Palm:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Palm");
                    case TreeTypes.PalmCrimson:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.PalmCrimson");
                    case TreeTypes.PalmCorrupt:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.PalmCorrupt");
                    case TreeTypes.PalmHallowed:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.PalmHallowed");
                    case TreeTypes.Ash:
                        return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Saplings.Ash");
                }
                int itemId = TreeUtil.GetTreeWood(dirtId);
                if (itemId != -1)
                {
                    string itemName = Lang.GetItemNameValue(itemId);
                    string tree = Language.GetTextValue("MapObject.Tree");
                    string palmTree = Language.GetTextValue("MapObject.PalmTree");
                    string sapling = Language.GetTextValue("MapObject.Sapling");
                    if (TileLoader.CanGrowModTree(dirtId))
                    {
                        return $"{itemName} {tree} {sapling}";
                    }
                    if (TileLoader.CanGrowModPalmTree(dirtId))
                    {
                        return $"{itemName} {palmTree} {sapling}";
                    }
                }
            }
            return null;
        }

        public static string GetNameForCactus(int sandId)
        {
            switch (sandId)
            {
                case TileID.Sand:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Cactus.Default");
                case TileID.Ebonsand:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Cactus.Corrupt");
                case TileID.Crimsand:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Cactus.Crimson");
                case TileID.Pearlsand:
                    return Language.GetTextValue("Mods.Twaila.ManualNames.Tiles.Cactus.Hallowed");
            }
            if (TileLoader.CanGrowModCactus(sandId))
            {
                ModTile mTile = TileLoader.GetTile(sandId);
                if (mTile != null)
                {
                    int tileDrop = TileLoader.GetItemDropFromTypeAndStyle(mTile.Type);
                    string type = GetNameFromItem(tileDrop);
                    if (type != null)
                    {
                        return type + " " + Language.GetTextValue("MapObject.Cactus");
                    }
                }
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

        public static string ToLocalizedString(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            MethodInfo labelMethod = typeof(ConfigManager).GetMethod("GetLocalizedLabel", BindingFlags.Static | BindingFlags.NonPublic);
            PropertyFieldWrapper wrapper = new(field);
            string key = labelMethod?.Invoke(null, new object[] { wrapper }) as string;

            return Language.GetTextValue(key) ?? value.ToString();
        }
    }
}
