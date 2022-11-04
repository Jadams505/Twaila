using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;

namespace Twaila.ObjectData
{
    public class ExtraObjectData
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
            Add4x2();
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
            AddEntry(TileID.LilyPad, data2);

            TileObjectData data3 = new TileObjectData(TileObjectData.Style1x1);
            data3.CoordinateHeights = new int[] { 16 };
            data3.StyleHorizontal = true;
            data3.CoordinateWidth = 20;
            AddEntry(TileID.LongMoss, data3);
            AddEntry(TileID.AbigailsFlower, data3);

            TileObjectData data4 = new TileObjectData(TileObjectData.Style1x1);
            data4.CoordinateWidth = 16;
            data4.CoordinateHeights = new int[] { 20 };
            data4.StyleHorizontal = true;
            AddEntry(TileID.CorruptPlants, data4);
            AddEntry(TileID.Plants, data4);
            AddEntry(TileID.JunglePlants, data4);
            AddEntry(TileID.MushroomPlants, data4);
            AddEntry(TileID.HallowedPlants, data4);
            AddEntry(TileID.CrimsonPlants, data4); //FleshWeeds

            TileObjectData data5 = new TileObjectData(TileObjectData.Style1x1);
            data5.CoordinateHeights = new int[] { 32 };
            data5.StyleHorizontal = true;
            AddEntry(TileID.Plants2, data5);
            AddEntry(TileID.JunglePlants2, data5);
            AddEntry(TileID.HallowedPlants2, data5);
            AddEntry(TileID.SeaOats, data5);

            TileObjectData data6 = new TileObjectData(TileObjectData.Style1x1);
            data6.CoordinateHeights = new int[] { 16 };
            data6.CoordinateWidth = 16;
            data6.StyleHorizontal = true;
            data6.StyleMultiplier = 27;
            data6.StyleWrapLimit = 27;
            AddEntry(TileID.Platforms, data6); // the stone platform is messed up in the original TileObjectData and I dont know why
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

            TileObjectData data2 = new TileObjectData(TileObjectData.Style3x2);
            data.CoordinateHeights = new int[] { 16, 16 };
            data.CoordinateWidth = 16;
            data.StyleHorizontal = true;
            AddEntry(TileID.OasisPlants, data2);
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

        private static void Add4x2()
        {
            TileObjectData data = new TileObjectData();
            data.CopyFrom(TileObjectData.Style4x2);
            data.CoordinateHeights = new int[] { 16, 18 };
            data.CoordinatePaddingFix = new Point16(0, -2);
            AddEntry(TileID.Beds, data);
        }

        private static void AddEntry(int tileId, TileObjectData copyFrom)
        {
            _data.Add(tileId, new TileObjectData(copyFrom));
        }

        public static TileObjectData GetData(Tile tile)
        {
            TileObjectData data = GetDataForPiles(tile.TileType, tile.TileFrameY) ?? GetDataForJungleFoilage(tile.TileType, tile.TileFrameY) ?? 
                GetDataForStalactite(tile.TileType, tile.TileFrameY);
            if(data == null)
            {
                _data.TryGetValue(tile.TileType, out data);
            }  
            return data;
        }

        public static TileObjectData GetData(int tileId, int frameY)
        {
            TileObjectData data = GetDataForPiles(tileId, frameY) ?? GetDataForJungleFoilage(tileId, frameY) ?? GetDataForStalactite(tileId, frameY);
            if (data == null)
            {
                _data.TryGetValue(tileId, out data);
            }
            return data;
        }

        /*
            The top row of the spritesheet for piles are 1x1 in size while the
            next two rows are 2x1 in size
        */
        private static TileObjectData GetDataForPiles(int tileId, int frameY)
        {
            if (tileId == TileID.SmallPiles)
            {
                TileObjectData data = new TileObjectData();
                if (frameY < 18)
                {
                    data.CopyFrom(TileObjectData.Style1x1);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16 };
                }
                else if (frameY < 52)
                {
                    data.CopyFrom(TileObjectData.Style2x1);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16 };
                }
                else
                {
                    data = null;
                }
                return data;
            }
            return null;
        }

        /*
            The top half of the spritesheet for jungle foilage are 3x2 in size while the
            bottom half are 2x2 in size
        */
        private static TileObjectData GetDataForJungleFoilage(int tileId, int frameY)
        {
            if (tileId == TileID.PlantDetritus)
            {
                TileObjectData data = new TileObjectData();
                if (frameY < 36)
                {
                    data.CopyFrom(TileObjectData.Style3x2);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else if (frameY < 70)
                {
                    data.CopyFrom(TileObjectData.Style2x2);
                    data.StyleHorizontal = true;
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else
                {
                    data = null;
                }
                return data;
            }
            return null;
        }

        /*
            The top half of the spritesheet for stalactites are 1x2 in size while the
            bottom half are 1x1 in size
        */
        private static TileObjectData GetDataForStalactite(int tileId, int frameY)
        {
            if (tileId == TileID.Stalactite)
            {
                TileObjectData data = new TileObjectData();
                if (frameY <= 69)
                {
                    data.CopyFrom(TileObjectData.Style1x2);
                    data.CoordinateHeights = new int[] { 16, 16 };
                }
                else if (frameY <= 105)
                {
                    data.CopyFrom(TileObjectData.Style1x1);
                    data.CoordinateHeights = new int[] { 16 };
                }
                else
                {
                    data = null;
                }
                return data;
            }
            return null;
        }
    }
}
