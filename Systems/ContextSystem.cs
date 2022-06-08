using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Context;

namespace Twaila.Systems
{
    public class ContextSystem : ModSystem
    {
        public static ContextSystem Instance => ModContent.GetInstance<ContextSystem>();

        public delegate BaseContext FetchContext(Point pos);

        public List<FetchContext> ContextConditions { get; private set; }

        public override void Load()
        {
            ContextConditions = new List<FetchContext>();
            RegisterWallContext();
            RegisterTileContext();

            RegisterContext(pos => Framing.GetTileSafely(pos).LiquidAmount > 0 ? new LiquidContext(pos) : null);
            RegisterContext(pos => Framing.GetTileSafely(pos).TileType == TileID.Cactus ? new CactusContext(pos) : null);
            RegisterContext(pos => Framing.GetTileSafely(pos).TileType == TileID.PalmTree ? new PalmTreeContext(pos) : null);
            RegisterContext(pos => TileID.Sets.TreeSapling[Framing.GetTileSafely(pos).TileType] ? new SaplingContext(pos) : null);
            RegisterContext(pos => Framing.GetTileSafely(pos).TileType == TileID.Trees || Framing.GetTileSafely(pos).TileType == TileID.MushroomTrees ? new TreeContext(pos) : null);
        }

        private void RegisterWallContext()
        {
            RegisterContext(pos =>
            {
                Tile tile = Framing.GetTileSafely(pos);

                if (tile.WallType > 0)
                {
                    return new WallContext(pos);
                }
                return null;
            });
        }

        private void RegisterTileContext()
        {
            RegisterContext(pos =>
            {
                Tile tile = Framing.GetTileSafely(pos);

                if (tile.HasTile)
                {
                    return new TileContext(pos);
                }
                return null;
            });
        }

        public override void Unload()
        {
            ContextConditions = null;
        }

        public void RegisterContext(FetchContext contextCondition)
        {
            ContextConditions.Add(contextCondition);
        }

        public BaseContext CurrentContext(int currIndex, Point pos)
        {
            return ContextConditions[currIndex].Invoke(pos);
        }

        public BaseContext NextContext(ref int currIndex, Point pos)
        {
            for (int i = currIndex + 1; i < ContextConditions.Count; ++i)
            {
                BaseContext context = CurrentContext(i, pos);
                if (context != null)
                {
                    currIndex = i;
                    return context;
                }
            }

            for (int i = 0; i < currIndex + 1; ++i)
            {
                BaseContext context = CurrentContext(i, pos);
                if (context != null)
                {
                    currIndex = i;
                    return context;
                }
            }
            return null;
        }
    }
}
