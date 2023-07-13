using System.ComponentModel;
using Terraria.ModLoader.Config;
using static Twaila.Config.TwailaConfig;

namespace Twaila.Config
{
    public class DisplaySettings
    {
        [DefaultValue(DisplayMode.Automatic)]
        [DrawTicks]
        public DisplayMode UIDisplay = DisplayMode.Automatic;

        [Header("AutomaticOptions")]
        [DefaultValue(false)]
        public bool HideUIForAir;

        public DisplaySettings()
        {
            UIDisplay = DisplayMode.Automatic;
            HideUIForAir = false;
        }

        public override bool Equals(object obj)
        {
            if (obj is DisplaySettings other)
            {
                return UIDisplay == other.UIDisplay && HideUIForAir == other.HideUIForAir;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { UIDisplay, HideUIForAir }.GetHashCode();
        }
    }
}
