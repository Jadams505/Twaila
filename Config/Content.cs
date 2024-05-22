using System;
using System.Collections.Generic;
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

        [SeparatePage]
        public NamingPreferences NamingPreferences = new();

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
            return obj is Content content &&
                   EqualityComparer<ContentToggles>.Default.Equals(EnableContent, content.EnableContent) &&
                   EqualityComparer<Priorities>.Default.Equals(ContentPriorities, content.ContentPriorities) &&
                   EqualityComparer<NpcContent>.Default.Equals(NpcContent, content.NpcContent) &&
                   EqualityComparer<NamingPreferences>.Default.Equals(NamingPreferences, content.NamingPreferences) &&
                   ShowImage == content.ShowImage &&
                   ShowName == content.ShowName &&
                   ShowMod == content.ShowMod &&
                   ShowId == content.ShowId &&
                   ShowPickaxePower == content.ShowPickaxePower &&
                   ShowPickaxe == content.ShowPickaxe &&
                   ShowWire == content.ShowWire &&
                   ShowActuator == content.ShowActuator &&
                   ShowPaint == content.ShowPaint &&
                   ShowCoating == content.ShowCoating &&
                   ShowContainedItems == content.ShowContainedItems &&
                   TextsPerRow == content.TextsPerRow &&
                   IconsPerRow == content.IconsPerRow;
        }

        public override int GetHashCode()
        {
            return new { ShowImage, ShowMod, ShowName, ShowPickaxePower, ShowWire, ShowActuator, ShowPaint, NpcContent }.GetHashCode();
        }
    }
}
