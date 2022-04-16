using Terraria;
using Terraria.GameContent.Drawing;
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

        public static TileObjectData GetTileObjectData(DummyTile tile, int style = 0)
        {
            TileObjectData data = ExtraObjectData.GetData(tile.TileId, tile.TileFrameY) ?? 
                TileObjectData.GetTileData(tile.TileId, style);
            return data;
        }

        public static bool IsBlockedByAntiCheat(TileContext context)
        {
            if (TwailaConfig.Get().AntiCheat && context.TileType != TileType.Empty)
            {
                Player player = Main.player[Main.myPlayer];
                if (player.HasBuff(BuffID.Spelunker) && Main.tileSpelunker[context.Tile.TileId])
                {
                    return false;
                }
                if (player.HasBuff(BuffID.Dangersense) && TileDrawing.IsTileDangerous(context.Pos.X, context.Pos.Y, Main.player[Main.myPlayer]))
                {
                    return false;
                }
                return !Main.Map.IsRevealed(context.Pos.X, context.Pos.Y);
            }
            return false;
        }

        // tile -> wall -> liquid -> tile
        public static void CycleType(TileContext context)
        {
            if (context.TileType == TileType.Empty || (!context.HasTile() && !context.HasLiquid() && !context.HasWall()))
            {
                return;
            }

            TileType type = context.TileType;
            do
            {
                type = Cycle(type);
                context.SetTileType(type);
            } while (context.TileType != type);
        }

        private static TileType Cycle(TileType type)
        {
            if (type == TileType.Tile)
            {
                return TileType.Wall;
            }
            if (type == TileType.Wall)
            {
                return TileType.Liquid;
            }
            if (type == TileType.Liquid)
            {
                return TileType.Tile;
            }
            return TileType.Empty;
        }

    }
}
