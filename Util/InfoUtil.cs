using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static string GetPaintName(Tile tile, TileType type)
        {
            byte color = GetPaintColor(tile, type);
            switch (color)
            {
                case PaintID.RedPaint:
                    return $"[i:{ItemID.RedPaint}]";
                case PaintID.OrangePaint:
                    return NameUtil.GetNameFromItem(ItemID.OrangePaint);
                case PaintID.YellowPaint:
                    return NameUtil.GetNameFromItem(ItemID.YellowPaint);
                case PaintID.LimePaint:
                    return NameUtil.GetNameFromItem(ItemID.LimePaint);
                case PaintID.GreenPaint:
                    return NameUtil.GetNameFromItem(ItemID.GreenPaint);
                case PaintID.CyanPaint:
                    return NameUtil.GetNameFromItem(ItemID.CyanPaint);
                case PaintID.SkyBluePaint:
                    return NameUtil.GetNameFromItem(ItemID.SkyBluePaint);
                case PaintID.BluePaint:
                    return NameUtil.GetNameFromItem(ItemID.BluePaint);
                case PaintID.PurplePaint:
                    return NameUtil.GetNameFromItem(ItemID.PurplePaint);
                case PaintID.VioletPaint:
                    return NameUtil.GetNameFromItem(ItemID.VioletPaint);
                case PaintID.PinkPaint:
                    return NameUtil.GetNameFromItem(ItemID.PinkPaint);
                case PaintID.BlackPaint:
                    return NameUtil.GetNameFromItem(ItemID.BlackPaint);
                case PaintID.GrayPaint:
                    return NameUtil.GetNameFromItem(ItemID.GrayPaint);
                case PaintID.WhitePaint:
                    return NameUtil.GetNameFromItem(ItemID.WhitePaint);
                case PaintID.BrownPaint:
                    return NameUtil.GetNameFromItem(ItemID.BrownPaint);
                case PaintID.ShadowPaint:
                    return NameUtil.GetNameFromItem(ItemID.ShadowPaint);
                case PaintID.NegativePaint:
                    return NameUtil.GetNameFromItem(ItemID.NegativePaint);
                case PaintID.IlluminantPaint:
                    return NameUtil.GetNameFromItem(ItemID.GlowPaint);
                case PaintID.DeepRedPaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepRedPaint);
                case PaintID.DeepOrangePaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepOrangePaint);
                case PaintID.DeepYellowPaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepYellowPaint);
                case PaintID.DeepLimePaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepLimePaint);
                case PaintID.DeepGreenPaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepGreenPaint);
                case PaintID.DeepTealPaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepTealPaint);
                case PaintID.DeepCyanPaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepCyanPaint);
                case PaintID.DeepSkyBluePaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepSkyBluePaint);
                case PaintID.DeepBluePaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepBluePaint);
                case PaintID.DeepPurplePaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepPurplePaint);
                case PaintID.DeepVioletPaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepVioletPaint);
                case PaintID.DeepPinkPaint:
                    return NameUtil.GetNameFromItem(ItemID.DeepPinkPaint);
            }
            return null;
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
    }
}
