using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Twaila.Systems;

namespace Twaila.Util
{
    public class InfoUtil
    {
        public static bool GetPickInfo(Tile tile, ref int lastIndex, out string text, out int pickId)
        {
            int power = GetPickaxePower(tile.TileType);
            text = "";
            pickId = -1;
            if(power > 0)
            {
                bool canMine = Main.player[Main.myPlayer].HeldItem.pick >= power;
                if (canMine)
                {
                    text = Language.GetText("Mods.Twaila.GoodPickPower").WithFormatArgs(power).Value;
                }
                else
                {
                    pickId = ItemTilePairSystem.GetPickId(power, lastIndex, out lastIndex);
                    text = Language.GetText("Mods.Twaila.BadPickPower").WithFormatArgs(power).Value;
                }
                return true;
            }
            return false;
        }

        public static int GetPickPowerForItem(int itemId)
        {
            ModItem mItem = ItemLoader.GetItem(itemId);
            if(mItem != null)
            {
                return mItem.Item.pick;
            }
            Item item = new Item();
            item.SetDefaults(itemId);
            return item.pick;
        }

        public static int GetPickaxePower(int tileId)
        {
            ModTile mTile = TileLoader.GetTile(tileId);
            if(mTile != null)
            {
                return mTile.MinPick;
            }
            switch (tileId)
            {
                case TileID.Meteorite:
                case TileID.Demonite:
                case TileID.Crimtane:
                case TileID.Obsidian:
                    return 55;
                case TileID.Ebonstone:
                case TileID.Pearlstone:
                case TileID.Hellstone:
                case TileID.Crimstone:
                case TileID.BlueDungeonBrick:
                case TileID.GreenDungeonBrick:
                case TileID.PinkDungeonBrick:
                    return 65;
                case TileID.Cobalt:
                case TileID.Palladium:
                    return 100;
                case TileID.Mythril:
                case TileID.Orichalcum:
                    return 110;
                case TileID.Adamantite:
                case TileID.Titanium:
                    return 150;
                case TileID.Chlorophyte:
                    return 200;
                case TileID.LihzahrdBrick:
                case TileID.LihzahrdAltar:
                    return 210;
                default:
                    return 0;
            }
        }

        public static bool GetPaintInfo(Tile tile, TileType type, out string text, out int icon)
        {
            byte color = GetPaintColor(tile, type);
            icon = GetPaintItem(color);
            text = "";
            if (icon != -1)
            {
                text = NameUtil.GetNameFromItem(icon);
                return true;
            }
            return false;
        }

        public static byte GetPaintColor(Tile tile, TileType type)
        {
            if(type == TileType.Tile)
            {
                return tile.TileColor;
            }
            else if(type == TileType.Wall)
            {
                return tile.WallColor;
            }
            return 0;
        }

        public static int GetPaintItem(byte color)
        {
            switch (color)
            {
                case PaintID.RedPaint:
                    return ItemID.RedPaint;
                case PaintID.OrangePaint:
                    return ItemID.OrangePaint;
                case PaintID.YellowPaint:
                    return ItemID.YellowPaint;
                case PaintID.LimePaint:
                    return ItemID.LimePaint;
                case PaintID.GreenPaint:
                    return ItemID.GreenPaint;
                case PaintID.CyanPaint:
                    return ItemID.CyanPaint;
                case PaintID.SkyBluePaint:
                    return ItemID.SkyBluePaint;
                case PaintID.BluePaint:
                    return ItemID.BluePaint;
                case PaintID.PurplePaint:
                    return ItemID.PurplePaint;
                case PaintID.VioletPaint:
                    return ItemID.VioletPaint;
                case PaintID.PinkPaint:
                    return ItemID.PinkPaint;
                case PaintID.BlackPaint:
                    return ItemID.BlackPaint;
                case PaintID.GrayPaint:
                    return ItemID.GrayPaint;
                case PaintID.WhitePaint:
                    return ItemID.WhitePaint;
                case PaintID.BrownPaint:
                    return ItemID.BrownPaint;
                case PaintID.ShadowPaint:
                    return ItemID.ShadowPaint;
                case PaintID.NegativePaint:
                    return ItemID.NegativePaint;
                case PaintID.IlluminantPaint:
                    return ItemID.GlowPaint;
                case PaintID.DeepRedPaint:
                    return ItemID.DeepRedPaint;
                case PaintID.DeepOrangePaint:
                    return ItemID.DeepOrangePaint;
                case PaintID.DeepYellowPaint:
                    return ItemID.DeepYellowPaint;
                case PaintID.DeepLimePaint:
                    return ItemID.DeepLimePaint;
                case PaintID.DeepGreenPaint:
                    return ItemID.DeepGreenPaint;
                case PaintID.DeepTealPaint:
                    return ItemID.DeepTealPaint;
                case PaintID.DeepCyanPaint:
                    return ItemID.DeepCyanPaint;
                case PaintID.DeepSkyBluePaint:
                    return ItemID.DeepSkyBluePaint;
                case PaintID.DeepBluePaint:
                    return ItemID.DeepBluePaint;
                case PaintID.DeepPurplePaint:
                    return ItemID.DeepPurplePaint;
                case PaintID.DeepVioletPaint:
                    return ItemID.DeepVioletPaint;
                case PaintID.DeepPinkPaint:
                    return ItemID.DeepPinkPaint;
            }
            return -1;
        }

        public static bool GetCoatingInfo(Tile tile, TileType type, out string illuminantText, out string echoText,
            out int illuminantIcon, out int echoIcon)
        {
            illuminantText = "";
            echoText = "";
            illuminantIcon = 0;
            echoIcon = 0;
            if(GetCoatingState(tile, type, out bool ill, out bool echo))
            {
                if (ill)
                {
                    illuminantIcon = ItemID.GlowPaint;
                    illuminantText = NameUtil.GetNameFromItem(illuminantIcon);
                }
                if (echo)
                {
                    echoIcon = ItemID.EchoCoating;
                    echoText = NameUtil.GetNameFromItem(echoIcon);
                }
                return true;
            }
            return false;
        }

        public static bool GetCoatingState(Tile tile, TileType type, out bool illuminant, out bool echo)
        {
            illuminant = false;
            echo = false;
            if (type == TileType.Tile)
            {
                illuminant = tile.IsTileFullbright;
                echo = tile.IsTileInvisible;
            }
            else if (type == TileType.Wall)
            {
                illuminant = tile.IsWallFullbright;
                echo = tile.IsWallInvisible;
            }
            return illuminant || echo;
        }

        public static bool GetWireInfo(Tile tile, out string text, out int[] icons)
        {
            //string[] colors = new string[4];
            string colorStr = "";
            icons = new int[4];
            bool hasWire = false;
            if (tile.RedWire)
            {
                colorStr += Language.GetTextValue("Mods.Twaila.WireColor.Red") + " ";
                icons[0] = ItemID.Wrench;
                hasWire = true;
            }
            if (tile.BlueWire)
            {
                colorStr += Language.GetTextValue("Mods.Twaila.WireColor.Blue") + " ";
                icons[1] = ItemID.BlueWrench;
                hasWire = true;
            }
            if (tile.GreenWire)
            {
                colorStr += Language.GetTextValue("Mods.Twaila.WireColor.Green") + " ";
                icons[2] = ItemID.GreenWrench;
                hasWire = true;
            }
            if (tile.YellowWire)
            {
                colorStr += Language.GetTextValue("Mods.Twaila.WireColor.Yellow");
                icons[3] = ItemID.YellowWrench;
                hasWire = true;
            }
            text = Language.GetText("Mods.Twaila.WireText").WithFormatArgs(colorStr).Value;
            return hasWire;
        }

        public static bool GetActuatorInfo(Tile tile, out string text, out int icon)
        {
            if (tile.HasActuator)
            {
                text = $"{NameUtil.GetNameFromItem(ItemID.Actuator)}";
                icon = ItemID.Actuator;
                return true;
            }
            text = "";
            icon = -1;
            return false;
        }

        public static bool GetId(Tile tile, TileType type, out int id)
        {
            switch (type)
            {
                case TileType.Tile:
                    if (tile.HasTile)
                    {
                        id = tile.TileType;
                        return true;
                    }
                    break;
                case TileType.Wall:
                    id = tile.WallType;
                    return true;
                case TileType.Liquid:
                    if(tile.LiquidAmount > 0)
                    {
                        id = Main.waterStyle;
                        return true;
                    }
                    break;
            }
            id = -1;
            return false;
        }
    }
}
