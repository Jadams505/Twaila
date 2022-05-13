using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Context;

namespace Twaila.Util
{
    public class InfoUtil
    {
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

        public static string GetPaintTag(Tile tile, TileType type)
        {
            byte color = GetPaintColor(tile, type);
            int paintItemId = GetPaintItem(color);
            if(paintItemId != -1)
            {
                return $"[i:{paintItemId}]";
            }
            return "";
        }

        public static string GetPaintName(Tile tile, TileType type)
        {
            return NameUtil.GetNameFromItem(GetPaintItem(GetPaintColor(tile, type)));
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

        public static string GetWireTag(Tile tile)
        {
            string tag = "";
            if (tile.RedWire)
            {
                tag += $"[i:{ItemID.Wrench}]";
            }
            if (tile.BlueWire)
            {
                tag += $"[i:{ItemID.BlueWrench}]";
            }
            if (tile.GreenWire)
            {
                tag += $"[i:{ItemID.GreenWrench}]";
            }
            if (tile.YellowWire)
            {
                tag += $"[i:{ItemID.YellowWrench}]";
            }
            return tag;
        }

        public static string GetActuatorTag(Tile tile)
        {
            if (tile.HasActuator)
            {
                return $"[i:{ItemID.Actuator}]";
            }
            return "";
        }
    }
}
