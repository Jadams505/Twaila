﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Context;
using Twaila.Util;

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

            TileEntry = new ContextEntry(CreateTileContext);
            TileEntry.ApplicableContexts.Add(CreatePalmTreeContext);
            TileEntry.ApplicableContexts.Add(CreateCactusContext);
            TileEntry.ApplicableContexts.Add(CreateTreeContext);
            TileEntry.ApplicableContexts.Add(CreateSaplingContext);
            ContextEntries.Add(TileEntry);

            WallEntry = new ContextEntry(CreateWallContext);
            ContextEntries.Add(WallEntry);

            LiquidEntry = new ContextEntry(CreateLiquidContext);
            ContextEntries.Add(LiquidEntry);

            WireEntry = new ContextEntry(CreateWireContext);
            ContextEntries.Add(WireEntry);

            NpcEntry = new ContextEntry(CreateNpcContext);
            ContextEntries.Add(NpcEntry);
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

        public List<ContextEntry> ContextEntriesAt(Point pos)
        {
            return ContextEntries.FindAll(entry => entry.Context(pos) != null);
        }

        public int ContextEntryCountAt(Point pos)
        {
            int count = 0;
            ContextEntries.ForEach(entry =>
            {
                if (entry.Context(pos) != null)
                {
                    count++;
                }
            });
            return count;
        }

        private static TileContext CreateTileContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if(tile.HasTile && !TileUtil.IsBlockedByAntiCheat(tile, pos))
            {
                return new TileContext(pos);
            }

            return null;
        }

        private static PalmTreeContext CreatePalmTreeContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if (tile.TileType == TileID.PalmTree && !TileUtil.IsBlockedByAntiCheat(tile, pos))
            {
                return new PalmTreeContext(pos);
            }

            return null;
        }

        private static CactusContext CreateCactusContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if(tile.TileType == TileID.Cactus && !TileUtil.IsBlockedByAntiCheat(tile, pos))
            {
                return new CactusContext(pos);
            }

            return null;
        }

        private static TreeContext CreateTreeContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if((tile.TileType == TileID.Trees || tile.TileType == TileID.MushroomTrees) && !TileUtil.IsBlockedByAntiCheat(tile, pos))
            {
                return new TreeContext(pos);
            }

            return null;
        }

        private static SaplingContext CreateSaplingContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if (TileID.Sets.TreeSapling[tile.TileType] && !TileUtil.IsBlockedByAntiCheat(tile, pos))
            {
                return new SaplingContext(pos);
            }

            return null;
        }

        private static WallContext CreateWallContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if(Framing.GetTileSafely(pos).WallType > 0 && !TileUtil.IsBlockedByAntiCheat(tile, pos))
            {
                return new WallContext(pos);
            }

            return null;
        }

        private static LiquidContext CreateLiquidContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if (tile.LiquidAmount > 0 && !TileUtil.IsBlockedByAntiCheat(tile, pos))
            {
                return new LiquidContext(pos);
            }

            return null;
        }

        private static WireContext CreateWireContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            bool noTile = !tile.HasTile && tile.WallType <= 0 && tile.LiquidAmount <= 0;

            bool hasWire = tile.RedWire || tile.BlueWire || tile.YellowWire || tile.GreenWire;

            bool canSeeWire = WiresUI.Settings.DrawWires && !WiresUI.Settings.HideWires;

            bool canSeeActuator = WiresUI.Settings.HideWires || WiresUI.Settings.DrawWires; // literally only necessary for the actuation rod

            if (noTile)
            {
                if (hasWire && (!TwailaConfig.Get().AntiCheat || canSeeWire))
                {
                    return new WireContext(pos);
                }
                if (tile.HasActuator && (!TwailaConfig.Get().AntiCheat || canSeeActuator))
                {
                    return new WireContext(pos);
                }
            }
            return null;
        }

        private static NpcContext CreateNpcContext(Point pos)
        {
            if (NpcContext.IntersectsNPC(pos))
            {
                return new NpcContext(pos);
            }

            return null;
        }
    }

    public delegate BaseContext ContextFetcher(Point pos);

    public class ContextEntry
    {

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
