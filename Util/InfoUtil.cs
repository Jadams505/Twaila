﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Context;

namespace Twaila.Util
{
    public class InfoUtil
    {
        public static bool GetPickInfo(Tile tile, ref int lastIndex, out string text, out string icon, out int pickId)
        {
            int power = GetPickaxePower(tile.TileType);
            text = "";
            icon = "";
            pickId = -1;
            if(power > 0)
            {
                bool canMine = Main.player[Main.myPlayer].HeldItem.pick >= power;
                string greenCheck = "[c/00FF00:\u2713]";
                string redX = "[c/FF0000:\u2717]";
                text = power + "% Pick Power ";
                if (canMine)
                {
                    text += greenCheck;
                }
                else
                {
                    pickId = ItemUtil.GetPickaxeId(power, lastIndex, out lastIndex);
                    icon = $"[i:{pickId}]";
                    text += redX;
                }
                return true;
            }
            return false;
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

        public static bool GetPaintInfo(Tile tile, TileType type, out string text, out string icon)
        {
            byte color = GetPaintColor(tile, type);
            int paintItemId = GetPaintItem(color);
            text = "";
            icon = "";
            if (paintItemId != -1)
            {
                text = NameUtil.GetNameFromItem(paintItemId);
                icon = $"[i:{paintItemId}]";
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

        public static bool GetWireInfo(Tile tile, out string text, out string icon)
        {
            string[] colors = new string[4];
            string[] icons = new string[4];
            if (tile.RedWire)
            {
                colors[0] = "Red";
                icons[0] = $"[i:{ItemID.Wrench}]";
            }
            if (tile.BlueWire)
            {
                colors[1] = "Blue";
                icons[1] = $"[i:{ItemID.BlueWrench}]";
            }
            if (tile.GreenWire)
            {
                colors[2] = "Green";
                icons[2] = $"[i:{ItemID.GreenWrench}]";
            }
            if (tile.YellowWire)
            {
                colors[3] = "Yellow";
                icons[3] = $"[i:{ItemID.YellowWrench}]";
            }
            text = "Wire: " + string.Join(" ", Array.FindAll(colors, (match) => !string.IsNullOrEmpty(match)));
            icon = string.Join("", icons);
            return icon != "";
        }

        public static bool GetActuatorInfo(Tile tile, out string text, out string icon)
        {
            if (tile.HasActuator)
            {
                text = $"{NameUtil.GetNameFromItem(ItemID.Actuator)}";
                icon = $"[i:{ItemID.Actuator}]";
                return true;
            }
            text = "";
            icon = "";
            return false;
        }
    }
}
