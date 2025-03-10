using System.ComponentModel;
using Terraria.ModLoader.Config;
using static Twaila.Config.TwailaConfig;

namespace Twaila.Config;

public class DisplaySettings
{
    [DefaultValue(DisplayMode.Automatic)]
    [DrawTicks]
    public DisplayMode UIDisplay = DisplayMode.Automatic;

    [Header("AutomaticOptions")]
    [DefaultValue(false)]
    public bool HideUIForAir;

    [Header("OtherOptions")]
    [DefaultValue(false)]
    public bool HideUIWhenTalkingToNPCs;

    [DefaultValue(false)]
    public bool HideUIWhenEditingSigns;

    public DisplaySettings()
    {
        UIDisplay = DisplayMode.Automatic;
        HideUIForAir = false;
        HideUIWhenTalkingToNPCs = false;
        HideUIWhenEditingSigns = false;
    }

    public override bool Equals(object? obj)
    {
        if (obj is DisplaySettings other)
        {
            return UIDisplay == other.UIDisplay && 
                HideUIForAir == other.HideUIForAir &&
                HideUIWhenTalkingToNPCs == other.HideUIWhenTalkingToNPCs &&
                HideUIWhenEditingSigns == other.HideUIWhenEditingSigns;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return new { UIDisplay, HideUIForAir }.GetHashCode();
    }
}
