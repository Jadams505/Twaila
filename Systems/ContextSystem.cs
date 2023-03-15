using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Twaila.Context;

namespace Twaila.Systems
{
    public class ContextSystem : ModSystem
    {
        public static ContextSystem Instance => ModContent.GetInstance<ContextSystem>();

        public List<ContextEntry> ContextEntries { get; private set; }

        public ContextEntry TileEntry { get; private set; }
        public ContextEntry WallEntry { get; private set; }
        public ContextEntry LiquidEntry { get; private set; }
        public ContextEntry WireEntry { get; private set; }
        public ContextEntry NpcEntry { get; private set; }

        public override void Load()
        {
            ContextEntries = new List<ContextEntry>();

            TileEntry = new ContextEntry(TileContext.CreateTileContext, Language.GetText("Mods.Twaila.Contexts.Tile"));
            TileEntry.ApplicableContexts.Add(PalmTreeContext.CreatePalmTreeContext);
            TileEntry.ApplicableContexts.Add(CactusContext.CreateCactusContext);
            TileEntry.ApplicableContexts.Add(TreeContext.CreateTreeContext);
            TileEntry.ApplicableContexts.Add(SaplingContext.CreateSaplingContext);
            TileEntry.ApplicableContexts.Add(FoodPlatterContext.CreateFoodPlatterContext);
            TileEntry.ApplicableContexts.Add(ItemFrameContext.CreateItemFrameContext);
            TileEntry.ApplicableContexts.Add(WeaponRackContext.CreateWeaponRackContext);
            TileEntry.ApplicableContexts.Add(HatRackContext.CreateHatRackContext);
            TileEntry.ApplicableContexts.Add(DisplayDollContext.CreateDisplayDollContext);
            ContextEntries.Add(TileEntry);

            WallEntry = new ContextEntry(WallContext.CreateWallContext, Language.GetText("Mods.Twaila.Contexts.Wall"));

            ContextEntries.Add(WallEntry);

            LiquidEntry = new ContextEntry(LiquidContext.CreateLiquidContext, Language.GetText("Mods.Twaila.Contexts.Liquid"));
            ContextEntries.Add(LiquidEntry);

            WireEntry = new ContextEntry(WireContext.CreateWireContext, Language.GetText("Mods.Twaila.Contexts.Wire"));
            ContextEntries.Add(WireEntry);

            // Localize this
            NpcEntry = new ContextEntry(NpcContext.CreateNpcContext, Language.GetText("Mods.Twaila.Contexts.Npc"));
            ContextEntries.Add(NpcEntry);
        }

        public override void Unload()
        {
            ContextEntries = null;
        }

        public BaseContext CurrentContext(int currIndex, TwailaPoint pos)
        {
            return ContextEntries[currIndex].Context(pos);
        }

        public BaseContext NextNonNullContext(ref int currIndex, TwailaPoint pos)
        {
            int i = currIndex;
            BaseContext context;
            do
            {
                i = NextContextIndex(i);
                context = CurrentContext(i, pos);
            }
            while (context == null && i != currIndex);

            currIndex = i;
            return context;
        }

        public BaseContext PrevNonNullContext(ref int currIndex, TwailaPoint pos)
        {
            int i = currIndex;
            BaseContext context;
            do
            {
                i = PrevContextIndex(i);
                context = CurrentContext(i, pos);
            }
            while (context == null && i != currIndex);

            currIndex = i;
            return context;
        }

        public int NextContextIndex(int currIndex)
        {
            int nextIndex = currIndex + 1;
            return nextIndex < ContextEntries.Count ? nextIndex : 0;
        }

        public int PrevContextIndex(int currIndex)
        {
            int prevIndex = currIndex - 1;
            return prevIndex >= 0 ? prevIndex : ContextEntries.Count - 1;
        }

        public List<ContextEntry> ContextEntriesAt(TwailaPoint pos)
        {
            return ContextEntries.FindAll(entry => entry.Context(pos) != null);
        }

        public int ContextEntryCountAt(TwailaPoint pos)
        {
            int count = 0;
            foreach(var entry in ContextEntries)
            {
                if (entry.Context(pos) != null)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public struct TwailaPoint
    {
        public Point MousePos;

        public Point TilePos;

        public Point SmartCursorPos;

        public Point MapPos;

        public TwailaPoint(Point mouse, Point tile, Point smart, Point map)
        {
            MousePos = mouse;
            TilePos = tile;
            SmartCursorPos = smart;
            MapPos = map;
        }

        public Point BestPos()
        {
            if (Main.SmartCursorShowing)
            {
                return SmartCursorPos;
            }
            
            if (Main.mapFullscreen)
            {
                return MapPos;
            }
            
            return TilePos;
        }
    }

    public delegate BaseContext ContextFetcher(TwailaPoint pos);

    public class ContextEntry
    {

        public List<ContextFetcher> ApplicableContexts { get; set; }

        public ContextFetcher DefaultContext { get; set; }

        public LocalizedText Name { get; set; }

        public ContextEntry(ContextFetcher defaultContext, LocalizedText name)
        {
            DefaultContext = defaultContext;
            ApplicableContexts = new List<ContextFetcher>();
            Name = name;
        }

        public BaseContext Context(TwailaPoint pos)
        {
            BaseContext foundContext = null;
            foreach(var entry in ApplicableContexts)
            {
                BaseContext context = entry.Invoke(pos);
                if (context != null)
                {
                    foundContext = context;
                    break;
                }
            }
            return foundContext ?? DefaultContext.Invoke(pos);
        }
    }
}
