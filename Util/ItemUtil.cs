using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Context;

namespace Twaila.Util
{
    internal class ItemUtil
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

        public static int GetItemId(TileContext context)
        {
            int id = GetManualItemId(context);
            if(id != -1)
            {
                return id;
            }
            int style = CalculatedPlaceStyle(context.Tile);
            return GetItemId(context.Tile, style, context.TileType);
        }

        internal static void LoadDictionary()
        {
            _tileToItemDictionary = new Dictionary<TileStylePair, int>();
            Populate();
        }

        internal static void UnloadDictionary()
        {
            _tileToItemDictionary = null;
        }

        private static int GetItemId(DummyTile tile, int style, TileType type)
        {
            int id = -1;
            switch (type)
            {
                case TileType.Tile:
                    id = tile.TileId;
                    break;
                case TileType.Wall:
                    id = tile.WallId;
                    break;
                case TileType.Liquid:
                    id = tile.LiquidId;
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
                if (item.createTile != -1)
                {
                    if(item.createTile == TileID.ClosedDoor)
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
            DummyTile dummyTile = new DummyTile();
            for (int i = ItemID.Count; i < ItemLoader.ItemCount; ++i) // modded items
            {
                ModItem mItem = ItemLoader.GetItem(i);
                if(mItem != null)
                {
                    dummyTile.TileId = mItem.Item.createTile;
                    if(mItem.Item.createTile != -1 && TileUtil.GetTileObjectData(dummyTile) != null)
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
            Twaila.Instance.Logger.Info(_tileToItemDictionary.Count + " Pairs Added");
        }

        private static int CalculatedPlaceStyle(DummyTile tile)
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
                if (tile.TileId == TileID.Chandeliers)
                {
                    int row = style % data.StyleWrapLimit;
                    int col = style / data.StyleWrapLimit / 2 * data.StyleWrapLimit;
                    calculatedStyle = row + col;
                }
            }
            return calculatedStyle;
        }

        // Assumes that modded doors do not wrap and that they follow the pattern of vanilla
        private static int GetPlaceStyleForDoor(DummyTile tile)
        {
            TileObjectData data = TileUtil.GetTileObjectData(tile);
            ModTile mTile = TileLoader.GetTile(tile.TileId);

            if ((mTile != null && mTile.OpenDoorID != -1) || tile.TileId == TileID.ClosedDoor)
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / (data.CoordinateFullWidth * 3);

                return row + col * data.StyleWrapLimit;
            }
            else if((mTile != null && mTile.CloseDoorID != -1) || tile.TileId == TileID.OpenDoor)
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / (data.CoordinateFullWidth * 2);
                return row + col * 36;
            }
            return -1;
        }

        private static int GetCustomPlaceStyle(DummyTile tile)
        {
            switch (tile.TileId)
            {
                case TileID.Campfire:
                    int campfireStyle = TileUtil.GetTileStyle(tile);
                    if(tile.TileFrameY >= 54)
                    {
                        return campfireStyle - 14;
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

        private static void GetTileInfo(DummyTile tile, ref int style, ref TileObjectData data)
        {
            data = TileUtil.GetTileObjectData(tile);
            if (data != null)
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / data.CoordinateFullWidth;
                if (data.Direction != Terraria.Enums.TileObjectDirection.None || tile.TileId == TileID.Timers)
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

        private static int GetManualItemId(TileContext context)
        {
            if(context.TileType == TileType.Tile)
            {
                DummyTile tile = context.Tile;
                switch (tile.TileId)
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
                if (context.Tile.Actuator && context.OnlyWire())
                {
                    return ItemID.Actuator;
                }
                if (context.OnlyWire())
                {
                    return ItemID.Wire;
                }
            }
            else if(context.TileType == TileType.Wall)
            {
                switch (context.Tile.WallId)
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
            }
            else if(context.TileType == TileType.Liquid)
            {
                switch (context.Tile.LiquidId)
                {
                    case LiquidID.Water:
                        return ItemID.WaterBucket;
                    case LiquidID.Honey:
                        return ItemID.HoneyBucket;
                    case LiquidID.Lava:
                        return ItemID.LavaBucket;
                }
            }
            return -1;
        }
    }
}
