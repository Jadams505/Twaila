using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Context;
using Twaila.ObjectData;

namespace Twaila.Util
{
    internal class ItemUtil
    {
        public static int GetItemId(Tile tile, TileType type)
        {
            if (type == TileType.Empty)
            {
                return -1;
            }
            int id = GetManualItemId(tile, type);
            if (id != -1)
            {
                return id;
            }
            ModTile mTile = TileLoader.GetTile(tile.TileType);
            int style = CalculatedPlaceStyle(tile);
            ModWall mWall = WallLoader.GetWall(tile.WallType);
            if (type == TileType.Wall && mWall != null)
            {
                return mWall.ItemDrop;
            }

            if ((type == TileType.Tile && mTile == null) || (type == TileType.Wall && mWall == null))
            {
                Item item = new Item();
                for (int i = 0; i < ItemID.Count; ++i)
                {
                    item.SetDefaults(i);
                    if(type == TileType.Tile)
                    {
                        if (item.createTile == tile.TileType || DoorHack(item, tile))
                        {
                            id = item.type;
                            if (style == -1 || item.placeStyle == style)
                            {
                                return i;
                            }
                        }
                    }
                    else if(type == TileType.Wall)
                    {
                        if(item.createWall == tile.WallType)
                        {
                            return i;
                        }
                    }
                }
                return id;
            }
            bool multiTile = TileObjectData.GetTileData(tile) != null;
            if (mTile.ItemDrop == 0 && multiTile)
            {
                for (int i = ItemID.Count; i < ItemLoader.ItemCount; ++i)
                {
                    ModItem mItem = ItemLoader.GetItem(i);
                    if (mItem != null && (mItem.Item.createTile == tile.TileType || DoorHack(mItem.Item, tile)))
                    {
                        id = mItem.Item.type;
                        if (style == -1 || mItem.Item.placeStyle == style)
                        {
                            return i;
                        }
                    }
                }
            }
            return mTile.ItemDrop == 0 ? id : mTile.ItemDrop;
        }

        private static int CalculatedPlaceStyle(Tile tile)
        {
            TileObjectData data = null;
            int style = -1;
            GetTileInfo(tile, ref style, ref data);
            int calculatedStyle = style;
            if (data != null)
            {
                int doorStyle = GetPlaceStyleForDoor(tile);
                if(doorStyle != -1)
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
            if (TileLoader.IsClosedDoor(tile))
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / (data.CoordinateFullWidth * 3);

                return row + col * data.StyleWrapLimit;
            }
            else if(TileLoader.CloseDoorID(tile) > -1) // open door
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / (data.CoordinateFullWidth * 2);
                return row + col * 36;
            }
            return -1;
        }

        private static bool DoorHack(Item item, Tile tile)
        {
            int closeDoorId = TileLoader.CloseDoorID(tile);
            if(closeDoorId > -1) // open door
            {
                return item.createTile == closeDoorId;
            }
            return false;
        }

        private static int GetManualItemId(Tile tile, TileType tileType)
        {
            if(tileType == TileType.Tile)
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
                }
            }
            else if(tileType == TileType.Liquid)
            {
                switch (tile.LiquidType)
                {
                    case LiquidID.Water:
                        return ItemID.WaterBucket;
                    case LiquidID.Lava:
                        return ItemID.LavaBucket;
                    case LiquidID.Honey:
                        return ItemID.HoneyBucket;
                }
            }
            return -1;
        }

        private static void GetTileInfo(Tile tile, ref int style, ref TileObjectData data)
        {
            data = TileUtil.GetTileObjectData(tile);
            if(data != null)
            {
                int row = tile.TileFrameY / data.CoordinateFullHeight;
                int col = tile.TileFrameX / data.CoordinateFullWidth;
                if (data.Direction != Terraria.Enums.TileObjectDirection.None || tile.TileType == TileID.Timers)
                {
                    style = TileObjectData.GetTileStyle(tile);
                }
                else if (data.StyleHorizontal)
                {
                    if(data.StyleMultiplier > 1)
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
                    if(data.StyleMultiplier > 1)
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
                style = -1;
            }
        }
    }
}
