using System;
using System.ComponentModel;

namespace Twaila.Config
{
    public class AntiCheatSettings
    {
        [DefaultValue(true)]
        public bool HideUnrevealedTiles;

        [DefaultValue(true)]
        public bool HideWires;

        [DefaultValue(true)]
        public bool HideEchoTiles;

        [DefaultValue(true)]
        public bool HideSuspiciousTiles;

        public AntiCheatSettings()
        {
            HideUnrevealedTiles = true;
            HideWires = true;
            HideEchoTiles = true;
            HideSuspiciousTiles = true;
        }

        public override bool Equals(object obj)
        {
            if (obj is AntiCheatSettings other)
            {
                return HideUnrevealedTiles == other.HideUnrevealedTiles
                    && HideWires == other.HideWires
                    && HideEchoTiles == other.HideEchoTiles
                    && HideSuspiciousTiles == other.HideSuspiciousTiles;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HideUnrevealedTiles, HideWires, HideEchoTiles, HideSuspiciousTiles);
        }
    }
}
