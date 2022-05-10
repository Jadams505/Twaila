using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Context;

namespace Twaila.Util
{
    internal class TileUtil
    {
        public static bool IsBlockedByAntiCheat(TileContext context)
        {
            if (TwailaConfig.Get().AntiCheat && context.TileType != TileType.Empty)
            {
                Player player = Main.player[Main.myPlayer];
                if (player.HasBuff(BuffID.Spelunker) && Main.tileSpelunker[context.Tile.type])
                {
                    return false;
                }
                if (player.HasBuff(BuffID.Dangersense) && IsDangersenseTile(context.Tile, context.Pos))
                {
                    return false;
                }
                return !Main.Map.IsRevealed(context.Pos.X, context.Pos.Y);
            }
            return false;
        }

        public static bool IsDangersenseTile(Tile tile, Point pos)
        {
            bool dangerTile = tile.type == TileID.PressurePlates || tile.type == TileID.Traps || tile.type == TileID.Boulder ||
                tile.type == TileID.Explosives || tile.type == TileID.LandMine || tile.type == TileID.ProjectilePressurePad ||
                tile.type == TileID.GeyserTrap || tile.type == TileID.BeeHive;
            if (tile.slope() == 0 && !tile.inActive())
            {
                dangerTile = dangerTile || tile.type == TileID.Cobweb || tile.type == TileID.CorruptThorns ||
                    tile.type == TileID.JungleThorns || tile.type == TileID.CrimtaneThorns || tile.type == TileID.Spikes
                    || tile.type == TileID.WoodenSpikes || tile.type == TileID.HoneyBlock;
                if (!Main.player[Main.myPlayer].fireWalk)
                {
                    dangerTile = dangerTile || tile.type == TileID.Meteorite || tile.type == TileID.Hellstone || tile.type == TileID.HellstoneBrick;
                }
                if (!Main.player[Main.myPlayer].iceSkate)
                {
                    dangerTile = dangerTile || tile.type == TileID.BreakableIce;
                }
            }
            return dangerTile || TileLoader.Dangersense(pos.X, pos.Y, tile.type, Main.player[Main.myPlayer]);
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
