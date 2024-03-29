﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Twaila.Config;
using Twaila.ObjectData;

namespace Twaila.Util
{
    public class TileUtil
    {
        public static TileObjectData GetTileObjectData(Tile tile)
        {
            TileObjectData data = ExtraObjectData.GetData(tile) ?? GetTileDataSafe(tile);
            return data;
        }

        public static TileObjectData GetTileObjectData(int tileId, int frameX, int frameY, int style = 0)
        {
            TileObjectData data = ExtraObjectData.GetData(tileId, frameY) ??
                TileObjectData.GetTileData(tileId, style);
            return IsValidTileObjectData(data) ? data : null;
        }

        public static bool IsValidTileObjectData(TileObjectData data)
        {
            return data != null && data.CoordinateFullWidth > 0 && data.CoordinateFullHeight > 0 && data.StyleMultiplier > 0;
        }

        public static TileObjectData GetTileDataSafe(Tile tile)
        {
            TileObjectData data = GetRawData(tile);
            return IsValidTileObjectData(data) ? TileObjectData.GetTileData(tile) : null;
        }

        private static readonly FieldInfo _tileObjectDataValues = typeof(TileObjectData).GetField("_data", BindingFlags.Static | BindingFlags.NonPublic);
        private static TileObjectData GetRawData(Tile tile)
        {
            List<TileObjectData> data = (List<TileObjectData>)_tileObjectDataValues.GetValue(null);
            return data[tile.TileType];
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
            if (TwailaConfig.Instance.AntiCheat.HideUnrevealedTiles)
            {
                Player player = Main.player[Main.myPlayer];
                if(TwailaConfig.Instance.AntiCheat.HideEchoTiles && (tile.TileType == TileID.EchoBlock || (tile.TileType == TileID.Platforms && tile.TileFrameY == 864) || tile.IsTileInvisible))
                {
                    return !player.CanSeeInvisibleBlocks && !Main.SceneMetrics.EchoMonolith;
                }
                return !IsTileRevealedToPlayer(player, tile, pos);
            }
            return false;
        }

        public static bool IsWallBlockedByAntiCheat(Tile tile, Point pos)
        {
            if (TwailaConfig.Instance.AntiCheat.HideUnrevealedTiles)
            {
                Player player = Main.LocalPlayer;
                if (TwailaConfig.Instance.AntiCheat.HideEchoTiles && (tile.WallType == WallID.EchoWall || tile.IsWallInvisible))
                {
                    return !player.CanSeeInvisibleBlocks && !Main.SceneMetrics.EchoMonolith;
                }
                return !IsTileRevealedToPlayer(player, tile, pos);
            }
            return false;
        }

        public static bool IsBlockedByAntiCheat(Tile tile, Point pos)
        {
            if (TwailaConfig.Instance.AntiCheat.HideUnrevealedTiles)
            {
                Player player = Main.player[Main.myPlayer];
                return !IsTileRevealedToPlayer(player, tile, pos);
            }
            return false;
        }

        public static bool IsTileRevealedToPlayer(Player player, Tile tile, Point tilePos)
        {
            if (tile.TileType >= TileLoader.TileCount)
                return false;

            if (tilePos.X < 0 || tilePos.X >= Main.Map.MaxWidth)
                return false;

            if (tilePos.Y < 0 || tilePos.Y >= Main.Map.MaxHeight)
                return false;

            if (!WorldGen.InWorld(tilePos.X, tilePos.Y))
                return false;

            if(!Main.mapFullscreen)
            {
                if (player.HasBuff(BuffID.Spelunker))
                {
                    if (tile.TileType >= Main.tileSpelunker.Length)
                        return false;

                    if (Main.tileSpelunker[tile.TileType])
                        return true;
                }

                if (player.HasBuff(BuffID.Dangersense) && TileDrawing.IsTileDangerous(tilePos.X, tilePos.Y, player))
                    return true;
            }

            return Main.Map.IsRevealed(tilePos.X, tilePos.Y);
        }

        public static Point TileEntityCoordinates(int tileCoordX, int tileCoordY, int size = 18, int width = 1, int height = 1)
        {
            Tile tile = Framing.GetTileSafely(tileCoordX, tileCoordY);
            int posXAdjusted = tileCoordX - tile.TileFrameX / size % width;
            int posYAdjusted = tileCoordY - tile.TileFrameY / size % height;

             return new Point(posXAdjusted, posYAdjusted);
        }

        public static bool IsTileValid(Tile tile) => tile.HasTile && tile.TileType < TileLoader.TileCount;

        public static bool IsTilePosInBounds(Point tilePos)
        {
            if (!WorldGen.InWorld(tilePos.X, tilePos.Y))
                return false;

            if (!Main.mapFullscreen)
                return IsTilePosOnScreen(tilePos);

            return true;
        }

        public static bool IsTilePosOnScreen(Point tilePos)
        {
            int targetX = tilePos.X, targetY = tilePos.Y;
            if (targetX < (Main.screenPosition.X - 16) / 16) // left
            {
                return false;
            }
            if (16 * targetX > PlayerInput.RealScreenWidth + Main.screenPosition.X) // right
            {
                return false;
            }
            if (targetY < (Main.screenPosition.Y - 16) / 16) // top
            {
                return false;
            }
            if (16 * targetY > PlayerInput.RealScreenHeight + Main.screenPosition.Y) // bottom
            {
                return false;
            }
            return true;
        }
    }
}
