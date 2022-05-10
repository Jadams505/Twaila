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

        public static string GetPaintTag(Tile tile, TileType type)
        {
            byte color = GetPaintColor(tile, type);
            switch (color)
            {
                case PaintID.RedPaint:
                    return $"[i:{ItemID.RedPaint}]";
                case PaintID.OrangePaint:
                    return $"[i:{ItemID.OrangePaint}]";
                case PaintID.YellowPaint:
                    return $"[i:{ItemID.YellowPaint}]";
                case PaintID.LimePaint:
                    return $"[i:{ItemID.LimePaint}]";
                case PaintID.GreenPaint:
                    return $"[i:{ItemID.GreenPaint}]";
                case PaintID.CyanPaint:
                    return $"[i:{ItemID.CyanPaint}]";
                case PaintID.SkyBluePaint:
                    return $"[i:{ItemID.SkyBluePaint}]";
                case PaintID.BluePaint:
                    return $"[i:{ItemID.BluePaint}]";
                case PaintID.PurplePaint:
                    return $"[i:{ItemID.PurplePaint}]";
                case PaintID.VioletPaint:
                    return $"[i:{ItemID.VioletPaint}]";
                case PaintID.PinkPaint:
                    return $"[i:{ItemID.PinkPaint}]";
                case PaintID.BlackPaint:
                    return $"[i:{ItemID.BlackPaint}]";
                case PaintID.GrayPaint:
                    return $"[i:{ItemID.GrayPaint}]";
                case PaintID.WhitePaint:
                    return $"[i:{ItemID.WhitePaint}]";
                case PaintID.BrownPaint:
                    return $"[i:{ItemID.BrownPaint}]";
                case PaintID.ShadowPaint:
                    return $"[i:{ItemID.ShadowPaint}]";
                case PaintID.NegativePaint:
                    return $"[i:{ItemID.NegativePaint}]";
                case PaintID.IlluminantPaint:
                    return $"[i:{ItemID.GlowPaint}]";
                case PaintID.DeepRedPaint:
                    return $"[i:{ItemID.DeepRedPaint}]";
                case PaintID.DeepOrangePaint:
                    return $"[i:{ItemID.DeepOrangePaint}]";
                case PaintID.DeepYellowPaint:
                    return $"[i:{ItemID.DeepYellowPaint}]";
                case PaintID.DeepLimePaint:
                    return $"[i:{ItemID.DeepLimePaint}]";
                case PaintID.DeepGreenPaint:
                    return $"[i:{ItemID.DeepGreenPaint}]";
                case PaintID.DeepTealPaint:
                    return $"[i:{ItemID.DeepTealPaint}]";
                case PaintID.DeepCyanPaint:
                    return $"[i:{ItemID.DeepCyanPaint}]";
                case PaintID.DeepSkyBluePaint:
                    return $"[i:{ItemID.DeepSkyBluePaint}]";
                case PaintID.DeepBluePaint:
                    return $"[i:{ItemID.DeepBluePaint}]";
                case PaintID.DeepPurplePaint:
                    return $"[i:{ItemID.DeepPurplePaint}]";
                case PaintID.DeepVioletPaint:
                    return $"[i:{ItemID.DeepVioletPaint}]";
                case PaintID.DeepPinkPaint:
                    return $"[i:{ItemID.DeepPinkPaint}]";
            }
            return "";
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
