using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Twaila.Context;

namespace Twaila.Systems
{
    public class ContextSystem : ModSystem
    {
        public static ContextSystem Instance => ModContent.GetInstance<ContextSystem>();

        public delegate BaseContext FetchContext(int x, int y);

        public List<FetchContext> ContextConditions { get; private set; }

        public override void Load()
        {
            ContextConditions = new List<FetchContext>();
            RegisterWallContext();
            RegisterTileContext();

            RegisterContext((x, y) => Framing.GetTileSafely(x, y).LiquidAmount > 0 ? new LiquidContext(new Point(x, y)) : null);
        }

        private void RegisterWallContext()
        {
            RegisterContext((x, y) =>
            {
                Tile tile = Framing.GetTileSafely(x, y);

                if (tile.WallType > 0)
                {
                    return new WallContext(new Point(x, y));
                }
                return null;
            });
        }

        private void RegisterTileContext()
        {
            RegisterContext((x, y) =>
            {
                Tile tile = Framing.GetTileSafely(x, y);

                if (tile.HasTile)
                {
                    return new BlockContext(new Point(x, y));
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

        public BaseContext CurrentContext(int currIndex, int x, int y)
        {
            return ContextConditions[currIndex].Invoke(x, y);
        }

        public BaseContext NextContext(ref int currIndex, int x, int y)
        {
            for (int i = currIndex + 1; i < ContextConditions.Count; ++i)
            {
                BaseContext context = CurrentContext(i, x, y);
                if (context != null)
                {
                    currIndex = i;
                    return context;
                }
            }

            for (int i = 0; i < currIndex + 1; ++i)
            {
                BaseContext context = CurrentContext(i, x, y);
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
