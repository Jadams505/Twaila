using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Context;

namespace Twaila.Systems
{
    public class ContextSystem : ModSystem
    {
        public static ContextSystem Instance => ModContent.GetInstance<ContextSystem>();

        public List<ContextEntry> ContextEntries { get; private set; }

        public override void Load()
        {
            ContextEntries = new List<ContextEntry>();

            ContextEntry tileEntry = new ContextEntry(pos => Framing.GetTileSafely(pos).HasTile ? new TileContext(pos) : null);
            tileEntry.ApplicableContexts.Add(pos => Framing.GetTileSafely(pos).TileType == TileID.PalmTree ? new PalmTreeContext(pos) : null);
            tileEntry.ApplicableContexts.Add(pos => Framing.GetTileSafely(pos).TileType == TileID.Cactus ? new CactusContext(pos) : null);
            tileEntry.ApplicableContexts.Add(pos => Framing.GetTileSafely(pos).TileType == TileID.Trees || Framing.GetTileSafely(pos).TileType == TileID.MushroomTrees ? new TreeContext(pos) : null);
            tileEntry.ApplicableContexts.Add(pos => TileID.Sets.TreeSapling[Framing.GetTileSafely(pos).TileType] ? new SaplingContext(pos) : null);
            ContextEntries.Add(tileEntry);

            ContextEntry wallEntry = new ContextEntry(pos => Framing.GetTileSafely(pos).WallType > 0 ? new WallContext(pos) : null);
            ContextEntries.Add(wallEntry);
         
            ContextEntry liquidEntry = new ContextEntry(pos => Framing.GetTileSafely(pos).LiquidAmount > 0 ? new LiquidContext(pos) : null);
            ContextEntries.Add(liquidEntry);
        }

        public override void Unload()
        {
            ContextEntries = null;
        }

        public BaseContext CurrentContext(int currIndex, Point pos)
        {
            return ContextEntries[currIndex].Context(pos);
        }

        public BaseContext NextContext(ref int currIndex, Point pos)
        {
            for (int i = currIndex + 1; i < ContextEntries.Count; ++i)
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

    public class ContextEntry
    {
        public delegate BaseContext ContextFetcher(Point pos);

        public List<ContextFetcher> ApplicableContexts { get; set; }

        public ContextFetcher DefaultContext { get; set; }

        public ContextEntry(ContextFetcher defaultContext)
        {
            DefaultContext = defaultContext;
            ApplicableContexts = new List<ContextFetcher>();
        }

        public BaseContext Context(Point pos)
        {
            BaseContext foundContext = null;
            ApplicableContexts.ForEach(entry =>
            {
                BaseContext context = entry.Invoke(pos);
                if (context != null)
                {
                    foundContext = context;
                }
            });
            return foundContext ?? DefaultContext.Invoke(pos);
        }
    }
}
