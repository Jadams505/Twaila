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
        public static int GetItemId(TileContext context)
        {
            if (context.TileType == TileType.Empty)
            {
                return -1;
            }
            int id = GetManualItemId(context);
            if (id != -1)
            {
                return id;
            }
            ModTile mTile = TileLoader.GetTile(context.Tile.type);
            int style = CalculatedPlaceStyle(context.Tile);
            ModWall mWall = WallLoader.GetWall(context.Tile.wall);
            if (context.TileType == TileType.Wall && mWall != null)
            {
                return mWall.drop;
            }

            if ((context.TileType == TileType.Tile && mTile == null) || (context.TileType == TileType.Wall && mWall == null))
            {
                Item item = new Item();
                for (int i = 0; i < ItemID.Count; ++i)
                {
                    item.SetDefaults(i);
                    if(context.TileType == TileType.Tile)
                    {
                        if (item.createTile == context.Tile.type || DoorHack(item, context.Tile))
                        {
                            id = item.type;
                            if (style == -1 || item.placeStyle == style)
                            {
                                return i;
                            }
                        }
                    }
                    else if(context.TileType == TileType.Wall)
                    {
                        if(item.createWall == context.Tile.wall)
                        {
                            return i;
                        }
                    }
                }
                return id;
            }
            bool multiTile = TileObjectData.GetTileData(context.Tile) != null;
            if (mTile.drop == 0 && multiTile)
            {
                for (int i = ItemID.Count; i < ItemLoader.ItemCount; ++i)
                {
                    ModItem mItem = ItemLoader.GetItem(i);
                    if (mItem != null && (mItem.item.createTile == context.Tile.type || DoorHack(mItem.item, context.Tile)))
                    {
                        id = mItem.item.type;
                        if (style == -1 || mItem.item.placeStyle == style)
                        {
                            return i;
                        }
                    }
                }
            }
            return mTile.drop == 0 ? id : mTile.drop;
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
                if (tile.type == TileID.Chandeliers)
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
            TileObjectData data = ExtraObjectData.GetData(tile) ?? TileObjectData.GetTileData(tile);
            if (TileLoader.IsClosedDoor(tile))
            {
                int row = tile.frameY / data.CoordinateFullHeight;
                int col = tile.frameX / (data.CoordinateFullWidth * 3);

                return row + col * data.StyleWrapLimit;
            }
            else if(TileLoader.CloseDoorID(tile) > -1) // open door
            {
                int row = tile.frameY / data.CoordinateFullHeight;
                int col = tile.frameX / (data.CoordinateFullWidth * 2);
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

        private static int GetManualItemId(TileContext context)
        {
            if(context.TileType == TileType.Tile)
            {
                Tile tile = context.Tile;
                switch (tile.type)
                {
                    case TileID.Plants:
                        if (tile.frameX == 144) return ItemID.Mushroom;
                        break;
                    case TileID.Heart:
                        return ItemID.LifeCrystal;
                    case TileID.Bottles:
                        if (tile.frameX == 18) return ItemID.LesserHealingPotion;
                        if (tile.frameX == 36) return ItemID.LesserManaPotion;
                        break;
                    case TileID.Saplings:
                        return ItemID.Acorn;
                    case TileID.Sunflower:
                        return ItemID.Sunflower;
                    case TileID.JunglePlants:
                        if (tile.frameX == 144) return ItemID.JungleSpores;
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
                        if (tile.frameX == 144) return ItemID.Mushroom;
                        break;
                    case TileID.HolidayLights:
                        if (tile.frameX == 0 || tile.frameX == 54)
                        {
                            return ItemID.BlueLight;
                        }
                        else if (tile.frameX == 18 || tile.frameX == 72)
                        {
                            return ItemID.RedLight;
                        }
                        else if (tile.frameX == 36 || tile.frameX == 86)
                        {
                            return ItemID.GreenLight;
                        }
                        break;
                    case TileID.FleshWeeds:
                        if (tile.frameX == 270) return ItemID.ViciousMushroom;
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
                        if ((tile.frameX >= 72 && tile.frameX < 180) || (tile.frameX >= 144 && tile.frameX < 108) || (tile.frameX >= 828 && tile.frameX < 1008)) // locked chests
                        {
                            return -2;
                        }
                        break;
                    case TileID.DyePlants:
                        if (tile.frameX >= 204 && tile.frameX < 238) return ItemID.PinkPricklyPear;
                        break;
                    case TileID.SillyBalloonTile:
                        if (tile.frameX >= 0 && tile.frameX < 36) return ItemID.SillyBalloonTiedPurple;
                        if (tile.frameX >= 36 && tile.frameX < 72) return ItemID.SillyBalloonTiedGreen;
                        if (tile.frameX >= 72) return ItemID.SillyBalloonTiedPink;
                        break;
                    case TileID.LifeFruit:
                        return ItemID.LifeFruit;
                }
            }
            else if(context.TileType == TileType.Liquid)
            {
                switch (context.Tile.liquidType())
                {
                    case Tile.Liquid_Water:
                        return ItemID.WaterBucket;
                    case Tile.Liquid_Honey:
                        return ItemID.HoneyBucket;
                    case Tile.Liquid_Lava:
                        return ItemID.LavaBucket;
                }
            }
            return -1;
        }

        private static void GetTileInfo(Tile tile, ref int style, ref TileObjectData data)
        {
            data = ExtraObjectData.GetData(tile) ?? TileObjectData.GetTileData(tile);
            if(data != null)
            {
                int row = tile.frameY / data.CoordinateFullHeight;
                int col = tile.frameX / data.CoordinateFullWidth;
                if (data.Direction != Terraria.Enums.TileObjectDirection.None || tile.type == TileID.Timers)
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
