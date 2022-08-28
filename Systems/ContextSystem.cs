using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Tile_Entities;
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

        public override void Load()
        {
            ContextEntries = new List<ContextEntry>();

            TileEntry = new ContextEntry(CreateTileContext, "Tile");
            TileEntry.ApplicableContexts.Add(CreatePalmTreeContext);
            TileEntry.ApplicableContexts.Add(CreateCactusContext);
            TileEntry.ApplicableContexts.Add(CreateTreeContext);
            TileEntry.ApplicableContexts.Add(CreateSaplingContext);
            TileEntry.ApplicableContexts.Add(CreateFoodPlatterContext);
            TileEntry.ApplicableContexts.Add(CreateItemFrameContext);
            TileEntry.ApplicableContexts.Add(CreateWeaponRackContext);
            TileEntry.ApplicableContexts.Add(CreateHatRackContext);
            ContextEntries.Add(TileEntry);

            WallEntry = new ContextEntry(CreateWallContext, "Wall");
            ContextEntries.Add(WallEntry);

            LiquidEntry = new ContextEntry(CreateLiquidContext, "Liquid");
            ContextEntries.Add(LiquidEntry);

            WireEntry = new ContextEntry(CreateWireContext, "Wire");
            ContextEntries.Add(WireEntry);
        }

        public override void Unload()
        {
            ContextEntries = null;
        }

        public BaseContext CurrentContext(int currIndex, Point pos)
        {
            return ContextEntries[currIndex].Context(pos);
        }

        public BaseContext NextNonNullContext(ref int currIndex, Point pos)
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

        public BaseContext PrevNonNullContext(ref int currIndex, Point pos)
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

            if(tile.HasTile && !TileUtil.IsTileBlockedByAntiCheat(tile, pos))
            {
                return new TileContext(pos);
            }

            return null;
        }

        private static PalmTreeContext CreatePalmTreeContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if (tile.TileType == TileID.PalmTree && !TileUtil.IsTileBlockedByAntiCheat(tile, pos))
            {
                return new PalmTreeContext(pos);
            }

            return null;
        }

        private static CactusContext CreateCactusContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if(tile.TileType == TileID.Cactus && !TileUtil.IsTileBlockedByAntiCheat(tile, pos))
            {
                return new CactusContext(pos);
            }

            return null;
        }

        private static TreeContext CreateTreeContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if((tile.TileType == TileID.Trees || tile.TileType == TileID.MushroomTrees) && !TileUtil.IsTileBlockedByAntiCheat(tile, pos))
            {
                return new TreeContext(pos);
            }

            return null;
        }

        private static SaplingContext CreateSaplingContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);

            if (TileID.Sets.TreeSapling[tile.TileType] && !TileUtil.IsTileBlockedByAntiCheat(tile, pos))
            {
                return new SaplingContext(pos);
            }

            return null;
        }

        private static FoodPlatterContext CreateFoodPlatterContext(Point pos)
        {
            if (TEFoodPlatter.Find(pos.X, pos.Y) != -1)
            {
                return new FoodPlatterContext(pos);
            }

            return null;
        }

        private static ItemFrameContext CreateItemFrameContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);
            if(tile.TileType == TileID.ItemFrame)
            {
                Point targetPos = TileUtil.TileEntityCoordinates(pos.X, pos.Y, width: 2, height: 2);
                if (TEItemFrame.Find(targetPos.X, targetPos.Y) != -1)
                {
                    return new ItemFrameContext(pos);
                }
            }
            return null;
        }

        private static WeaponRackContext CreateWeaponRackContext(Point pos)
        {
            Tile tile = Framing.GetTileSafely(pos);
            if (tile.TileType == TileID.WeaponsRack2 || tile.TileType == TileID.WeaponsRack)
            {
                Point targetPos = TileUtil.TileEntityCoordinates(pos.X, pos.Y, width: 3, height: 3);
                if (TEWeaponsRack.Find(targetPos.X, targetPos.Y) != -1)
                {
                    return new WeaponRackContext(pos);
                }
            }
            return null;
        }

        private static HatRackContext CreateHatRackContext(Point pos)
        {
			Tile tile = Framing.GetTileSafely(pos);
			if (tile.TileType == TileID.HatRack)
			{
				Point targetPos = TileUtil.TileEntityCoordinates(pos.X, pos.Y, width: 3, height: 4);
				if (TEHatRack.Find(targetPos.X, targetPos.Y) != -1)
				{
					return new HatRackContext(pos);
				}
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
            Player player = Main.player[Main.myPlayer];

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
    }

    public delegate BaseContext ContextFetcher(Point pos);

    public class ContextEntry
    {

        public List<ContextFetcher> ApplicableContexts { get; set; }

        public ContextFetcher DefaultContext { get; set; }

        public string Name { get; set; }

        public ContextEntry(ContextFetcher defaultContext, string name)
        {
            DefaultContext = defaultContext;
            ApplicableContexts = new List<ContextFetcher>();
            Name = name;
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
