using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;

namespace Twaila.ObjectData
{
    internal class ExtraObjectData
    {
        private static Dictionary<int, TileObjectData> _data;

        public static void Initialize()
        {
            _data = new Dictionary<int, TileObjectData>();
            Add1x1();
            Add2x1();
            Add2x2();
            Add2x3();
            Add3x2();
            Add3x3();
        }

        public static void Unload()
        {
            _data = null;
        }

        private static void Add1x1()
        {
            TileObjectData data = new TileObjectData(TileObjectData.Style1x1);
            data.CoordinateHeights = new int[] { 16 };
            AddEntry(TileID.Crystals, data);
            AddEntry(TileID.Traps, data);
            AddEntry(TileID.Explosives, data);
            AddEntry(TileID.Timers, data);
            AddEntry(TileID.HolidayLights, data);
            AddEntry(TileID.MinecartTrack, data);
            AddEntry(TileID.PlanterBox, data);
            AddEntry(TileID.LogicGate, data);
            AddEntry(TileID.LogicSensor, data);
            AddEntry(TileID.LogicGateLamp, data);
            AddEntry(TileID.WireBulb, data);
            AddEntry(TileID.PixelBox, data);

            TileObjectData data2 = new TileObjectData(TileObjectData.Style1x1);
            data2.CoordinateHeights = new int[] { 16 };
            data2.StyleHorizontal = true;
            AddEntry(TileID.ExposedGems, data2);
            AddEntry(TileID.WirePipe, data2);

            TileObjectData data3 = new TileObjectData(TileObjectData.Style1x1);
            data3.CoordinateHeights = new int[] { 16 };
            data3.StyleHorizontal = true;
            data3.CoordinateWidth = 20;
            AddEntry(TileID.LongMoss, data3);

            TileObjectData data4 = new TileObjectData(TileObjectData.Style1x1);
            data4.CoordinateWidth = 16;
            data4.CoordinateHeights = new int[] { 20 };
            data4.StyleHorizontal = true;
            AddEntry(TileID.CorruptPlants, data4);
            AddEntry(TileID.Plants, data4);
            AddEntry(TileID.JunglePlants, data4);
            AddEntry(TileID.MushroomPlants, data4);
            AddEntry(TileID.HallowedPlants, data4);
            AddEntry(TileID.FleshWeeds, data4);

            TileObjectData data5 = new TileObjectData(TileObjectData.Style1x1);
            data5.CoordinateHeights = new int[] { 32 };
            data5.StyleHorizontal = true;
            AddEntry(TileID.Plants2, data5);
            AddEntry(TileID.JunglePlants2, data5);
            AddEntry(TileID.HallowedPlants2, data5);
        }
        private static void Add2x1()
        {
            TileObjectData data = new TileObjectData();
            data.CopyFrom(TileObjectData.Style2x1);
            data.CoordinateHeights = new int[] { 18 };
            data.StyleHorizontal = true;
            AddEntry(TileID.GeyserTrap, data);
        }

        private static void Add2x2()
        {
            TileObjectData data = new TileObjectData();
            data.CopyFrom(TileObjectData.Style2x2);
            AddEntry(TileID.Heart, data);
            AddEntry(TileID.Pots, data);
            AddEntry(TileID.ShadowOrbs, data);
            AddEntry(TileID.LifeFruit, data);
            AddEntry(TileID.PlanteraBulb, data);
        }
        private static void Add2x3()
        {
            TileObjectData data = new TileObjectData();
            data.CopyFrom(TileObjectData.Style2xX);
            data.Height = 3;
            data.CoordinateHeights = new int[] { 16, 16, 18 };
            data.StyleHorizontal = true;
            AddEntry(TileID.LunarMonolith, data);
        }

        private static void Add3x2()
        {
            TileObjectData data = new TileObjectData();
            data.CopyFrom(TileObjectData.Style3x2);
            data.CoordinateHeights = new int[] { 16, 18 };
            AddEntry(TileID.TinkerersWorkbench, data);
        }

        private static void Add3x3()
        {
            TileObjectData data = new TileObjectData();
            data.CopyFrom(TileObjectData.Style3x3);
            data.CoordinateHeights = new int[] { 16, 16, 18 };
            AddEntry(TileID.Chimney, data);
            TileObjectData data2 = new TileObjectData(TileObjectData.Style3x3);
            data2.CoordinateHeights = new int[] { 16, 16, 16 };
            data2.StyleHorizontal = true;
            AddEntry(TileID.GemLocks, data2);
        }

        private static void AddEntry(int tileId, TileObjectData copyFrom)
        {
            _data.Add(tileId, new TileObjectData(copyFrom));
        }

        public static TileObjectData GetData(int type)
        {
            _data.TryGetValue(type, out TileObjectData data);
            return data;
        }

        public static int GetTileStyle(Tile tile)
        {
            TileObjectData data = GetData(tile.type);
            if(data == null)
            {
                return -1;
            }
            if (data.StyleHorizontal)
            {
                return tile.frameX / data.CoordinateFullWidth;
            }
            return tile.frameY / data.CoordinateFullHeight;
        }
    }
}
