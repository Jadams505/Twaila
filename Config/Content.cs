using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using static Twaila.Config.TwailaConfig;

namespace Twaila.Config
{
    public class Content
    {
        [SeparatePage]
        public ContentToggles EnableContent = new ContentToggles();

        [SeparatePage]
        public Priorities ContentPriorities = new Priorities();

        [SeparatePage]
        public NpcContent NpcContent = new NpcContent();

        [DefaultValue(true)]
        public bool ShowImage;

        [DrawTicks]
        public NameType ShowName;

        [DefaultValue(true)]
        public bool ShowMod;

        [DefaultValue(false)]
        public bool ShowId;

        [DefaultValue(true)]
        public bool ShowPickaxePower;

        [DrawTicks]
        public DisplayType ShowPickaxe;

        [DrawTicks]
        public DisplayType ShowWire;

        [DrawTicks]
        public DisplayType ShowActuator;

        [DrawTicks]
        public DisplayType ShowPaint;

        [DrawTicks]
        public DisplayType ShowCoating = DisplayType.Off;

        [DrawTicks]
        public DisplayType ShowContainedItems;

        [Range(1, 20)]
        [DefaultValue(1)]
        public int TextsPerRow;

        [Range(1, 20)]
        [DefaultValue(8)]
        public int IconsPerRow;

        public Content()
        {
            ShowImage = true;
            ShowMod = true;
            ShowId = false;
            ShowPickaxePower = true;
            ShowPickaxe = DisplayType.Icon;
            ShowWire = DisplayType.Icon;
            ShowActuator = DisplayType.Icon;
            ShowPaint = DisplayType.Icon;
            ShowCoating = DisplayType.Icon;
            ShowContainedItems = DisplayType.Icon;
            ShowName = NameType.DisplayName;
            NpcContent = new NpcContent();
            IconsPerRow = 8;
            TextsPerRow = 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is Content other)
            {
                return ShowImage == other.ShowImage && ShowMod == other.ShowMod && ShowName == other.ShowName
                    && ShowPickaxePower == other.ShowPickaxePower && ShowWire == other.ShowWire &&
                    ShowActuator == other.ShowActuator && ShowPaint == other.ShowPaint && ShowPickaxe == other.ShowPickaxe
                    && ShowId == other.ShowId && ShowContainedItems == other.ShowContainedItems && ShowCoating == other.ShowCoating
                    && NpcContent == other.NpcContent && IconsPerRow == other.IconsPerRow && TextsPerRow == other.TextsPerRow;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { ShowImage, ShowMod, ShowName, ShowPickaxePower, ShowWire, ShowActuator, ShowPaint, NpcContent }.GetHashCode();
        }
    }
}
