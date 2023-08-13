using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Twaila.Config
{
    public class Priorities
    {
        [Header("Priorities")]

        [DefaultValue(0)]
        public int WirePriority;

        [DefaultValue(1)]
        public int NpcPriority;

        [DefaultValue(2)]
        public int TilePrioity;

        [DefaultValue(3)]
        public int LiquidPriority;

        [DefaultValue(4)]
        public int WallPriority;

        public Priorities()
        {
            WirePriority = 0;
            NpcPriority = 1;
            TilePrioity = 2;
            LiquidPriority = 3;
            WallPriority = 4;
        }

        public override bool Equals(object obj)
        {
            return obj is Priorities priorities &&
                   NpcPriority == priorities.NpcPriority &&
                   WallPriority == priorities.WallPriority &&
                   TilePrioity == priorities.TilePrioity &&
                   LiquidPriority == priorities.LiquidPriority &&
                   WirePriority == priorities.WirePriority;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NpcPriority, WallPriority, TilePrioity, LiquidPriority, WirePriority);
        }
    }
}
