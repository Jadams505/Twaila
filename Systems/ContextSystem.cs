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

        public BaseContext NextContext(int currIndex, int x, int y)
        {
            for(int i = currIndex + 1; i < ContextConditions.Count; ++i)
            {
                BaseContext context = ContextConditions[i].Invoke(x, y);
                if (context != null)
                {
                    return context;
                }
            }

            for (int i = 0; i < currIndex + 1; ++i)
            {
                BaseContext context = ContextConditions[i].Invoke(x, y);
                if (context != null)
                {
                    return context;
                }
            }
            return null;
        }
    }
}
