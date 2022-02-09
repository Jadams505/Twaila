using System.Collections.Generic;
using Terraria.ID;

namespace Twaila.Util
{
    public class TileDictionary
    {
        public class TileData
        {
            public readonly int drop, pickaxe, axe, hammer;
            public TileData(int drop, int pickaxe, int axe, int hammer)
            {
                this.drop = drop;
                this.pickaxe = pickaxe;
                this.axe = axe;
                this.hammer = hammer;
            }
        }
        public static readonly Dictionary<int, TileData> TILES = Default();

        public static Dictionary<int, TileData> Default()
        {
            Dictionary<int, TileData> dict = new Dictionary<int, TileData>();
            AddEntry(tileType: TileID.Dirt, drop: ItemID.DirtBlock, pickaxePower: 35, axePower: 0,  hammerPower: 0, dict);
            AddEntry(tileType: TileID.Stone, drop: ItemID.StoneBlock, pickaxePower: 35, axePower: 0,  hammerPower: 0, dict);
            AddEntry(tileType: TileID.Grass, drop: -1, pickaxePower: 35, axePower: 0, hammerPower: 0, dict);
            AddEntry(tileType: TileID.Plants, drop: -1, pickaxePower: 35, axePower: 0, hammerPower: 0, dict);
            AddEntry(tileType: TileID.Torches, drop: ItemID.Torch, pickaxePower: 35, axePower: 0, hammerPower: 0, dict);
            return dict;
        }
        
        private static void AddEntry(int tileType, int drop, int pickaxePower, int axePower, int hammerPower, Dictionary<int, TileData> dict)
        {
            dict.Add(tileType, new TileData(drop, pickaxePower, axePower, hammerPower));
        }
    }
}
