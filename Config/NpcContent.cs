using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using static Twaila.Config.TwailaConfig;

namespace Twaila.Config
{
    public class NpcContent
    {
        [DefaultValue(true)]
        public bool ShowHp;

        [DefaultValue(true)]
        public bool ShowDefense;

        [DefaultValue(true)]
        public bool ShowAttack;

        [DefaultValue(true)]
        public bool ShowKnockback;

        [DefaultValue(true)]
        public bool ShowKills;

        [DrawTicks]
        public DisplayType ShowBuffs;

        [DrawTicks]
        public NumberType ShowHappiness;

        [DrawTicks]
        public DisplayType ShowNpcPreferences;

        [DefaultValue(true)]
        public bool ShowBiomePreferences;

        [Range(1, 20)]
        [DefaultValue(3)]
        public int StatsPerRow;

        [Range(1, 20)]
        [DefaultValue(8)]
        public int IconsPerRow;

        [SeparatePage]
        public HappinessColors HappinessColors = new();

        public NpcContent()
        {
            ShowHp = true;
            ShowDefense = true;
            ShowAttack = true;
            ShowKnockback = true;
            ShowKills = true;
            StatsPerRow = 3;
            ShowBiomePreferences = true;
            ShowHappiness = NumberType.Number;
            ShowBuffs = DisplayType.Icon;
            ShowNpcPreferences = DisplayType.Icon;
            IconsPerRow = 8;
            HappinessColors = new();
        }

        public override bool Equals(object obj)
        {
            if (obj is NpcContent other)
            {
                return ShowHp == other.ShowHp && ShowDefense == other.ShowDefense && ShowAttack == other.ShowAttack
                    && ShowKnockback == other.ShowKnockback && ShowKills == other.ShowKills
                    && ShowBuffs == other.ShowBuffs && IconsPerRow == other.IconsPerRow && StatsPerRow == other.StatsPerRow
                    && ShowNpcPreferences == other.ShowNpcPreferences && ShowBiomePreferences == other.ShowBiomePreferences
                    && HappinessColors == other.HappinessColors && ShowHappiness == other.ShowHappiness;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShowHp, ShowDefense, ShowAttack, ShowKnockback, ShowKills, StatsPerRow, ShowBuffs);
        }
    }
}
