using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ObjectData;
using Twaila.Context;
using Twaila.ObjectData;

namespace Twaila.Util
{
    internal class TileUtil
    {
        public static TileObjectData GetTileObjectData(Tile tile)
        {
            TileObjectData data = ExtraObjectData.GetData(tile) ?? TileObjectData.GetTileData(tile);
            return data;
        }

        public static TileObjectData GetTileObjectData(int tileId, int frameX, int frameY, int style = 0)
        {
            TileObjectData data = ExtraObjectData.GetData(tileId, frameY) ??
                TileObjectData.GetTileData(tileId, style);
            return data;
        }

        public static int GetTileStyle(Tile tile)
        {
            TileObjectData data = GetTileObjectData(tile);
            if(data == null)
            {
                return -1;
            }

            int col = tile.TileFrameX / data.CoordinateFullWidth;
            int row = tile.TileFrameY / data.CoordinateFullHeight;
            int swl = data.StyleWrapLimit;
            if (swl == 0)
            {
                swl = 1;
            }

            int style = (!data.StyleHorizontal) ? (col * swl + row) : (row * swl + col);
            style /= data.StyleMultiplier;
            return style;
        }

        public static void GetRealTileFrame(Tile tile, int posX, int posY, out int frameX, out int frameY)
        {
            short originalFrameX = tile.TileFrameX, originalFrameY = tile.TileFrameY;
            Main.instance.TilesRenderer.GetTileDrawData(posX, posY, tile, tile.TileType, ref originalFrameX, ref originalFrameY, out _, out _, out _,
                out _, out int addX, out int addY, out _, out _, out _, out _);
            frameX = originalFrameX + addX;
            frameY = originalFrameY + addY;
        }

        public static bool IsTileBlockedByAntiCheat(Tile tile, Point pos)
        {
            if (TwailaConfig.Get().AntiCheat)
            {
                Player player = Main.player[Main.myPlayer];
                if(tile.TileType == TileID.EchoBlock)
                {
                    return !player.CanSeeInvisibleBlocks;
                }
                return !IsTileRevealedToPlayer(player, tile, pos);
            }
            return false;
        }

        public static bool IsBlockedByAntiCheat(Tile tile, Point pos)
        {
            if (TwailaConfig.Get().AntiCheat)
            {
                Player player = Main.player[Main.myPlayer];
                return !IsTileRevealedToPlayer(player, tile, pos);
            }
            return false;
        }

        public static bool IsTileRevealedToPlayer(Player player, Tile tile, Point tilePos)
        {
            if (player.HasBuff(BuffID.Spelunker) && Main.tileSpelunker[tile.TileType])
            {
                return true;
            }
            if (player.HasBuff(BuffID.Dangersense) && TileDrawing.IsTileDangerous(tilePos.X, tilePos.Y, Main.player[Main.myPlayer]))
            {
                return true;
            }
            return Main.Map.IsRevealed(tilePos.X, tilePos.Y);
        }
    }
}
