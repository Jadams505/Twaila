using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Context;

namespace Twaila.Util
{
    public enum TileType
    {
        Tile, Wall, Liquid, Empty
    }

    public class ItemUtil
    {
        private class TileStylePair
        {
            public int Style { get; set; }
            public int Id { get; set; }
            public TileType Type { get; set; }

            public TileStylePair(int id, int style, TileType type)
            {
                Id = id;
                Style = style;
                Type = type;
            }

            public override string ToString()
            {
                return $"Id: {Id} Style: {Style} Type: {Type}";
            }

            public override bool Equals(object obj)
            {
                return obj is TileStylePair pair &&
                       Style == pair.Style &&
                       Id == pair.Id &&
                       Type == pair.Type;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Style, Id, Type);
            }
        }

        private static Dictionary<TileStylePair, int> _tileToItemDictionary;

        public static int GetItemId(Tile tile, TileType type)
        {
            int id = GetManualItemId(tile, type);
            if(id != -1)
            {
                return id;
            }
            int style = CalculatedPlaceStyle(tile);
            return GetItemId(tile, style, type);
        }

        private class PickPowerPair
        {
            public int power;
            public int id;

            public PickPowerPair(int power, int id)
            {
                this.power = power;
                this.id = id;
            }

        }

        private static List<(int power, int id)> _pickaxes;

        public static int GetPickId(int pickPower, int startIndex, out int foundIndex)
        {
            if (startIndex < 0 || startIndex >= _pickaxes.Count)
            {
                startIndex = 0;
            }
            for (int i = startIndex; i < _pickaxes.Count; ++i)
            {
                (int power, int id) pick = _pickaxes[i];
                if (pick.power >= pickPower)
                {
                    foundIndex = i;
                    return pick.id;
                }
            }
            foundIndex = -1;
            return -1;
        }

        private class PickPowerSorter : IComparer<(int power, int id)>
        {
            public int Compare((int power, int id) x, (int power, int id) y)
            {
                if(x.power < y.power)
                {
                    return -1;
                }
                if(x.power > y.power)
                {
                    return 1;
                }
                return 0;
            }
        }

        internal static void Load()
        {
            _tileToItemDictionary = new Dictionary<TileStylePair, int>();
            _pickaxes = new List<(int power, int id)>();
            Populate();
        }

        internal static void Unload()
        {
            _pickaxes = null;
            _tileToItemDictionary = null;
        }

        private static int GetItemId(Tile tile, int style, TileType type)
        {
            int id = -1;
            switch (type)
            {
                case TileType.Tile:
                    id = tile.TileType;
                    break;
                case TileType.Wall:
                    id = tile.WallType;
                    break;
                case TileType.Liquid:
                    id = tile.LiquidType;
                    break;
                default:
                    return -1;
            }
            TileStylePair pair = new TileStylePair(id, style, type);
            int firstTry = _tileToItemDictionary.GetValueOrDefault(pair, -1);
            if(firstTry == -1 && pair.Style != 0)
            {
                pair.Style = 0;
                int secondTry = _tileToItemDictionary.GetValueOrDefault(pair, -1);
                return secondTry; // this works for a lot of tiles that are directional (improve later)
            }
            return firstTry;
        }

        private static void AddEntry(int id, int style, TileType type, int itemId)
        {
            TileStylePair pair = new TileStylePair(id, style, type);
            if (!_tileToItemDictionary.TryAdd(pair, itemId))
            {
                //Twaila.Instance.Logger.Warn("Cannot use itemId: " + itemId + " becuase " + pair + " already exists!");
            }
        }

        private static void Populate()
        {
            for (int i = 0; i < ItemID.Count; ++i) // vanilla items
            {
                Item item = new Item();
                item.SetDefaults(i);
                if (item.pick > 0)
                {
                    _pickaxes.Add((item.pick, i));
                }
                if (item.createTile != -1)
                {
                    if (item.createTile == TileID.ClosedDoor)
                    {
                        AddOpenDoorEntry(i);
                    } 
                    AddEntry(item.createTile, item.placeStyle, TileType.Tile, i);
                }
                if (item.createWall != -1)
                {
                    AddEntry(item.createWall, 0, TileType.Wall, i);
                }
            }
            
            for (int i = TileID.Count; i < TileLoader.TileCount; ++i) // modded tiles
            {
                ModTile mTile = TileLoader.GetTile(i);
                if(mTile != null && mTile.ItemDrop != 0)
                {
                    AddEntry(i, 0, TileType.Tile, mTile.ItemDrop);
                }
            }
            for(int i = WallID.Count; i < WallLoader.WallCount; ++i) // modded walls
            {
                ModWall mWall = WallLoader.GetWall(i);
                if(mWall != null && mWall.ItemDrop != 0)
                {
                    AddEntry(i, 0, TileType.Wall, mWall.ItemDrop);
                }
            }
            for (int i = ItemID.Count; i < ItemLoader.ItemCount; ++i) // modded items
            {
                ModItem mItem = ItemLoader.GetItem(i);
                if(mItem != null)
                {
                    if (mItem.Item.pick > 0)
                    {
                        _pickaxes.Add((mItem.Item.pick, i));
                    }
                    if(mItem.Item.createTile != -1 && TileUtil.GetTileObjectData(mItem.Item.createTile, 0, 0) != null)
                    {
                        AddOpenDoorEntry(i);
                        AddEntry(mItem.Item.createTile, mItem.Item.placeStyle, TileType.Tile, i);
                    }
                    if(mItem.Item.createWall != -1)
                    {
                        AddEntry(mItem.Item.createWall, 0, TileType.Wall, i);
                    }
                }
            }
            _pickaxes.Sort(new PickPowerSorter());
            Twaila.Instance.Logger.Info(_tileToItemDictionary.Count + " Pairs Added");
        }

        private static int CalculatedPlaceStyle(Tile tile)
        {
            TileObjectData data = null;
            int style = GetCustomPlaceStyle(tile);
            if(style != -1)
            {
                return style;
            }
            GetTileInfo(tile, ref style, ref data);
            int calculatedStyle = style;
            if (data != null)
            {
                int doorStyle = GetPlaceStyleForDoor(tile);
                if (doorStyle != -1)
                {
                    return doorStyle;
                }
                if (tile.TileType == TileID.Chandeliers)
                {
                    int row = style % data.StyleWrapLimit;
                    int col = style / data.StyleWrapLimit / 2 * data.StyleWrapLimit;
                    calculatedStyle = row + col;
                }
            }
            return calculatedStyle;
        }

        // Assumes that modded doors do not wrap and that they follow the pattern of vanilla
        private static int GetPlaceStyleForDoor(Tile tile)
        {
            TileObjectData data = TileUtil.GetTileObjectData(tile);
            ModTile mTile = TileLoader.GetTile(tile.TileType);

            if ((mTile != null && mTile.OpenDoorID != -1) || tile.TileType == TileID.ClosedDoor)
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / (data.CoordinateFullWidth * 3);

                return row + col * data.StyleWrapLimit;
            }
            else if((mTile != null && mTile.CloseDoorID != -1) || tile.TileType == TileID.OpenDoor)
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / (data.CoordinateFullWidth * 2);
                return row + col * 36;
            }
            return -1;
        }

        private static int GetCustomPlaceStyle(Tile tile)
        {
            switch (tile.TileType)
            {
                case TileID.Campfire:
                    int campfireStyle = TileUtil.GetTileStyle(tile);
                    int styleCount = TileObjectData.GetTileData(tile).StyleWrapLimit;
                    if (campfireStyle >= styleCount)
                    {
                        return campfireStyle - styleCount;
                    }
                    return campfireStyle;
                case TileID.Statues:
                    int style = TileUtil.GetTileStyle(tile);
                    if(tile.TileFrameY >= 162)
                    {
                        return style - 165;
                    }
                    return style;
                case TileID.DisplayDoll:
                    if(tile.TileFrameX >= 72 && tile.TileFrameX <= 126)
                    {
                        return 2;
                    }
                    if(tile.TileFrameX >= 0 && tile.TileFrameX < 72)
                    {
                        return 0;
                    }
                    break;
            }
            return -1;
        }

        private static void GetTileInfo(Tile tile, ref int style, ref TileObjectData data)
        {
            data = TileUtil.GetTileObjectData(tile);
            if (data != null)
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / data.CoordinateFullWidth;
                if (data.Direction != Terraria.Enums.TileObjectDirection.None || tile.TileType == TileID.Timers)
                {
                    style = TileUtil.GetTileStyle(tile);
                }
                else if (data.StyleHorizontal)
                {
                    if (data.StyleMultiplier > 1)
                    {
                        style = row + col / data.StyleMultiplier;
                    }
                    else
                    {
                        style = col + row * data.StyleWrapLimit;
                    }
                }
                else
                {
                    if (data.StyleMultiplier > 1)
                    {
                        style = col + row / data.StyleMultiplier;
                    }
                    else
                    {
                        style = row + col * data.StyleWrapLimit;
                    }
                }
            }
            else
            {
                style = 0;
            }
        }
        
        /*
            Uses the item id of a door item to add an entry for the open door tile.
            This is necessary because no item places open door tiles only closed door tiles 
        */
        private static void AddOpenDoorEntry(int doorItemId)
        {
            ModItem mItem = ItemLoader.GetItem(doorItemId);
            if(mItem != null)
            {
                ModTile mTile = TileLoader.GetTile(mItem.Item.createTile);
                if(mTile != null && mTile.OpenDoorID != -1)
                {
                    AddEntry(mTile.OpenDoorID, mItem.Item.placeStyle, TileType.Tile, doorItemId);
                }
            }
            else
            {
                Item item = new Item();
                item.SetDefaults(doorItemId);
                if(item.createTile == TileID.ClosedDoor)
                {
                    AddEntry(TileID.OpenDoor, item.placeStyle, TileType.Tile, doorItemId);
                }
            }
        }

        private static int GetManualItemId(Tile tile, TileType type)
        {
            if (type == TileType.Tile)
            {
                return GetManualTileItemId(tile);
            }
            else if (type == TileType.Wall)
            {
                return GetManualWallItemId(tile.WallType);
            }
            else if(type == TileType.Liquid)
            {
                return GetManualLiquidItemId(tile.LiquidType);
            }
            return -1;
        }

        private static int GetManualTileItemId(Tile tile)
        {
            switch (tile.TileType)
            {
                case TileID.Plants:
                    if (tile.TileFrameX == 144) return ItemID.Mushroom;
                    break;
                case TileID.Heart:
                    return ItemID.LifeCrystal;
                case TileID.Bottles:
                    if (tile.TileFrameX == 18) return ItemID.LesserHealingPotion;
                    if (tile.TileFrameX == 36) return ItemID.LesserManaPotion;
                    break;
                case TileID.Saplings:
                    return ItemID.Acorn;
                case TileID.Sunflower:
                    return ItemID.Sunflower;
                case TileID.JunglePlants:
                    if (tile.TileFrameX == 144) return ItemID.JungleSpores;
                    break;
                case TileID.Sapphire:
                    return ItemID.Sapphire;
                case TileID.Ruby:
                    return ItemID.Ruby;
                case TileID.Emerald:
                    return ItemID.Emerald;
                case TileID.Topaz:
                    return ItemID.Topaz;
                case TileID.Amethyst:
                    return ItemID.Amethyst;
                case TileID.Diamond:
                    return ItemID.Diamond;
                case TileID.MushroomPlants:
                    return ItemID.GlowingMushroom;
                case TileID.HallowedPlants:
                    if (tile.TileFrameX == 144) return ItemID.Mushroom;
                    break;
                case TileID.Crystals:
                    if (tile.TileFrameX >= 324)
                        return ItemID.QueenSlimeCrystal;
                    break;
                case TileID.HolidayLights:
                    if (tile.TileFrameX == 0 || tile.TileFrameX == 54)
                    {
                        return ItemID.BlueLight;
                    }
                    else if (tile.TileFrameX == 18 || tile.TileFrameX == 72)
                    {
                        return ItemID.RedLight;
                    }
                    else if (tile.TileFrameX == 36 || tile.TileFrameX == 86)
                    {
                        return ItemID.GreenLight;
                    }
                    break;
                case TileID.CrimsonPlants:
                    if (tile.TileFrameX == 270) return ItemID.ViciousMushroom;
                    break;
                case TileID.Hive:
                    return ItemID.Hive;
                case TileID.AmethystGemsparkOff:
                    return ItemID.AmethystGemsparkBlock;
                case TileID.TopazGemsparkOff:
                    return ItemID.TopazGemsparkBlock;
                case TileID.SapphireGemsparkOff:
                    return ItemID.SapphireGemsparkBlock;
                case TileID.EmeraldGemsparkOff:
                    return ItemID.EmeraldGemsparkBlock;
                case TileID.RubyGemsparkOff:
                    return ItemID.RubyGemsparkBlock;
                case TileID.DiamondGemsparkOff:
                    return ItemID.DiamondGemsparkBlock;
                case TileID.AmberGemsparkOff:
                    return ItemID.AmberGemsparkBlock;
                case TileID.TrapdoorOpen:
                    return ItemID.Trapdoor;
                case TileID.TallGateOpen:
                    return ItemID.TallGate;
                case TileID.Containers:
                    if ((tile.TileFrameX >= 72 && tile.TileFrameX < 180) || (tile.TileFrameX >= 144 && tile.TileFrameX < 108) || (tile.TileFrameX >= 828 && tile.TileFrameX < 1008)) // locked chests
                    {
                        return -2;
                    }
                    break;
                case TileID.DyePlants:
                    if (tile.TileFrameX >= 204 && tile.TileFrameX < 238) return ItemID.PinkPricklyPear;
                    break;
                case TileID.SillyBalloonTile:
                    if (tile.TileFrameX >= 0 && tile.TileFrameX < 36) return ItemID.SillyBalloonTiedPurple;
                    if (tile.TileFrameX >= 36 && tile.TileFrameX < 72) return ItemID.SillyBalloonTiedGreen;
                    if (tile.TileFrameX >= 72) return ItemID.SillyBalloonTiedPink;
                    break;
                case TileID.LifeFruit:
                    return ItemID.LifeFruit;
                case TileID.LongMoss:
                    if (tile.TileFrameX < 22) return ItemID.GreenMoss;
                    else if (tile.TileFrameX < 44) return ItemID.BrownMoss;
                    else if (tile.TileFrameX < 66) return ItemID.RedMoss;
                    else if (tile.TileFrameX < 88) return ItemID.BlueMoss;
                    else if (tile.TileFrameX < 110) return ItemID.PurpleMoss;
                    else if (tile.TileFrameX < 132) return ItemID.LavaMoss;
                    else if (tile.TileFrameX < 154) return ItemID.KryptonMoss;
                    else if (tile.TileFrameX < 176) return ItemID.XenonMoss;
                    else if (tile.TileFrameX < 198) return ItemID.ArgonMoss;
                    break;
                case TileID.GrateClosed:
                    return ItemID.Grate;
                case TileID.AmberStoneBlock:
                    return ItemID.Amber;
            }
            if (tile.HasActuator && !tile.HasTile)
            {
                return ItemID.Actuator;
            }
            if ((tile.RedWire || tile.BlueWire || tile.GreenWire || tile.YellowWire) && !tile.HasTile)
            {
                return ItemID.Wire;
            }
            return -1;
        }

        private static int GetManualWallItemId(int wallId)
        {
            switch (wallId)
            {
                case WallID.DirtUnsafe:
                    return ItemID.DirtWall;
                case WallID.EbonstoneUnsafe:
                    return ItemID.EbonstoneEcho;
                case WallID.BlueDungeonUnsafe:
                    return ItemID.BlueBrickWall;
                case WallID.GreenDungeonUnsafe:
                    return ItemID.GreenBrickWall;
                case WallID.PinkDungeonUnsafe:
                    return ItemID.PinkBrickWall;
                case WallID.HellstoneBrickUnsafe:
                    return ItemID.HellstoneBrickWall;
                case WallID.ObsidianBrickUnsafe:
                    return ItemID.ObsidianBrickWall;
                case WallID.MudUnsafe:
                    return ItemID.MudWallEcho;
                case WallID.PearlstoneBrickUnsafe:
                    return ItemID.PearlstoneEcho;
                case WallID.SnowWallUnsafe:
                    return ItemID.SnowWallEcho;
                case WallID.AmethystUnsafe:
                    return ItemID.AmethystEcho;
                case WallID.TopazUnsafe:
                    return ItemID.TopazEcho;
                case WallID.SapphireUnsafe:
                    return ItemID.SapphireEcho;
                case WallID.EmeraldUnsafe:
                    return ItemID.EmeraldEcho;
                case WallID.RubyUnsafe:
                    return ItemID.RubyEcho;
                case WallID.DiamondUnsafe:
                    return ItemID.DiamondEcho;
                case WallID.CaveUnsafe:
                    return ItemID.Cave1Echo;
                case WallID.Cave2Unsafe:
                    return ItemID.Cave2Echo;
                case WallID.Cave3Unsafe:
                    return ItemID.Cave3Echo;
                case WallID.Cave4Unsafe:
                    return ItemID.Cave4Echo;
                case WallID.Cave5Unsafe:
                    return ItemID.Cave5Echo;
                case WallID.Cave6Unsafe:
                    return ItemID.Cave6Echo;
                case WallID.LivingLeaf:
                    return ItemID.LivingLeafWall;
                case WallID.Cave7Unsafe:
                    return ItemID.Cave7Echo;
                case WallID.SpiderUnsafe:
                    return ItemID.SpiderEcho;
                case WallID.GrassUnsafe:
                    return ItemID.GrassWall;
                case WallID.JungleUnsafe:
                    return ItemID.JungleWall;
                case WallID.FlowerUnsafe:
                    return ItemID.FlowerWall;
                case WallID.CorruptGrassUnsafe:
                    return ItemID.CorruptGrassEcho;
                case WallID.HallowedGrassUnsafe:
                    return ItemID.HallowedGrassEcho;
                case WallID.IceUnsafe:
                    return ItemID.IceEcho;
                case WallID.ObsidianBackUnsafe:
                    return ItemID.ObsidianBackEcho;
                case WallID.MushroomUnsafe:
                    return ItemID.MushroomWall;
                case WallID.CrimsonGrassUnsafe:
                    return ItemID.CrimsonGrassEcho;
                case WallID.CrimstoneUnsafe:
                    return ItemID.CrimstoneEcho;
                case WallID.HiveUnsafe:
                    return ItemID.HiveWall;
                case WallID.LihzahrdBrickUnsafe:
                    return ItemID.LihzahrdBrickWall;
                case WallID.BlueDungeonSlabUnsafe:
                    return ItemID.BlueSlabWall;
                case WallID.BlueDungeonTileUnsafe:
                    return ItemID.BlueTiledWall;
                case WallID.PinkDungeonSlabUnsafe:
                    return ItemID.PinkSlabWall;
                case WallID.PinkDungeonTileUnsafe:
                    return ItemID.PinkTiledWall;
                case WallID.GreenDungeonSlabUnsafe:
                    return ItemID.GreenSlabWall;
                case WallID.GreenDungeonTileUnsafe:
                    return ItemID.GreenTiledWall;
                case WallID.CaveWall:
                    return ItemID.CaveWall1Echo;
                case WallID.CaveWall2:
                    return ItemID.CaveWall2Echo;
                case WallID.MarbleUnsafe:
                    return ItemID.MarbleWall;
                case WallID.GraniteUnsafe:
                    return ItemID.GraniteWall;
                case WallID.Cave8Unsafe:
                    return ItemID.Cave8Echo;
                case WallID.Sandstone:
                    return ItemID.SandstoneWall;
                case WallID.CorruptionUnsafe1:
                    return ItemID.Corruption1Echo;
                case WallID.CorruptionUnsafe2:
                    return ItemID.Corruption2Echo;
                case WallID.CorruptionUnsafe3:
                    return ItemID.Corruption3Echo;
                case WallID.CorruptionUnsafe4:
                    return ItemID.Corruption4Echo;
                case WallID.CrimsonUnsafe1:
                    return ItemID.Crimson1Echo;
                case WallID.CrimsonUnsafe2:
                    return ItemID.Crimson2Echo;
                case WallID.CrimsonUnsafe3:
                    return ItemID.Crimson3Echo;
                case WallID.CrimsonUnsafe4:
                    return ItemID.Crimson4Echo;
                case WallID.DirtUnsafe1:
                    return ItemID.Dirt1Echo;
                case WallID.DirtUnsafe2:
                    return ItemID.Dirt2Echo;
                case WallID.DirtUnsafe3:
                    return ItemID.Dirt3Echo;
                case WallID.DirtUnsafe4:
                    return ItemID.Dirt4Echo;
                case WallID.HallowUnsafe1:
                    return ItemID.Hallow1Echo;
                case WallID.HallowUnsafe2:
                    return ItemID.Hallow2Echo;
                case WallID.HallowUnsafe3:
                    return ItemID.Hallow3Echo;
                case WallID.HallowUnsafe4:
                    return ItemID.Hallow4Echo;
                case WallID.JungleUnsafe1:
                    return ItemID.Jungle1Echo;
                case WallID.JungleUnsafe2:
                    return ItemID.Jungle2Echo;
                case WallID.JungleUnsafe3:
                    return ItemID.Jungle3Echo;
                case WallID.JungleUnsafe4:
                    return ItemID.Jungle4Echo;
                case WallID.LavaUnsafe1:
                    return ItemID.Lava1Echo;
                case WallID.LavaUnsafe2:
                    return ItemID.Lava2Echo;
                case WallID.LavaUnsafe3:
                    return ItemID.Lava3Echo;
                case WallID.LavaUnsafe4:
                    return ItemID.Lava4Echo;
                case WallID.RocksUnsafe1:
                    return ItemID.Rocks1Echo;
                case WallID.RocksUnsafe2:
                    return ItemID.Rocks2Echo;
                case WallID.RocksUnsafe3:
                    return ItemID.Rocks3Echo;
                case WallID.RocksUnsafe4:
                    return ItemID.Rocks4Echo;
                case WallID.HardenedSand:
                    return ItemID.HardenedSandWall;
                case WallID.CorruptHardenedSand:
                    return ItemID.CorruptHardenedSandWall;
                case WallID.CrimsonHardenedSand:
                    return ItemID.CrimsonHardenedSandWall;
                case WallID.HallowHardenedSand:
                    return ItemID.HallowHardenedSandWall;
                case WallID.CorruptSandstone:
                    return ItemID.CorruptSandstoneWall;
                case WallID.CrimsonSandstone:
                    return ItemID.CrimsonSandstoneWall;
                case WallID.HallowSandstone:
                    return ItemID.HallowSandstoneWall;
                case WallID.LivingWoodUnsafe:
                    return ItemID.LivingWoodWall;
            }
            return -1;
        }

        private static int GetManualLiquidItemId(int liquidId)
        {
            switch (liquidId)
            {
                case LiquidID.Water:
                    return ItemID.WaterBucket;
                case LiquidID.Honey:
                    return ItemID.HoneyBucket;
                case LiquidID.Lava:
                    return ItemID.LavaBucket;
            }
            return -1;
        }
    }
}
