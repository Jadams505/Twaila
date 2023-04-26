using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Twaila.UI;
using Twaila.Systems;

namespace Twaila
{
    [Label("$Mods.Twaila.ModConfig")]
    public class TwailaConfig : ModConfig
    {
        public static TwailaConfig Instance => ModContent.GetInstance<TwailaConfig>();

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnChanged()
        {
            ContextSystem.Instance?.SortContexts();
        }

        [Header("$Mods.Twaila.Features")]

        [SeparatePage]
        [Tooltip("$Mods.Twaila.AntiCheatSettings.Tooltip")]
        [Label("$Mods.Twaila.AntiCheatSettings.Label")]
        public AntiCheatSettings AntiCheat = new AntiCheatSettings();

        [SeparatePage]
        [Label("$Mods.Twaila.DisplayContent")]
        public Content DisplayContent = new Content();


        [Header("$Mods.Twaila.UIPosition")]

        [DefaultValue(true)]
        [Tooltip("$Mods.Twaila.UseDefaultPosition.Tooltip")]
        [Label("$Mods.Twaila.UseDefaultPosition.Label")]
        public bool UseDefaultPosition;

        [DefaultValue(true)]
        [Tooltip("$Mods.Twaila.LockPosition.Tooltip")]
        [Label("$Mods.Twaila.LockPosition.Label")]
        public bool LockPosition;

        [DrawTicks]
        [DefaultValue(HorizontalAnchor.Center)]
        [Label("$Mods.Twaila.AnchorX")]
        public HorizontalAnchor AnchorX = HorizontalAnchor.Center;
        public enum HorizontalAnchor
        {
            [Label("$Mods.Twaila.Enums.Left")]Left,
            [Label("$Mods.Twaila.Enums.Center")]Center,
            [Label("$Mods.Twaila.Enums.Right")]Right
        };

        [DrawTicks]
        [DefaultValue(VerticalAnchor.Top)]
        [Label("$Mods.Twaila.AnchorY")]
        public VerticalAnchor AnchorY = VerticalAnchor.Top;
        public enum VerticalAnchor
        {
            [Label("$Mods.Twaila.Enums.Bottom")] Bottom,
            [Label("$Mods.Twaila.Enums.Center")] Center,
            [Label("$Mods.Twaila.Enums.Top")] Top
        };

        [DefaultValue(0)]
        [Range(0, 2000)]
        [Label("$Mods.Twaila.AnchorPosX")]
        public int AnchorPosX;

        [DefaultValue(0)]
        [Range(0, 2000)]
        [Label("$Mods.Twaila.AnchorPosY")]
        public int AnchorPosY;


        [Header("$Mods.Twaila.UIPanel")]

        [DefaultValue(40)]
        [Tooltip("$Mods.Twaila.MaxWidth.Tooltip")]
        [Label("$Mods.Twaila.MaxWidth.Label")]
        public int MaxWidth;

        [DefaultValue(40)]
        [Tooltip("$Mods.Twaila.MaxHeight.Tooltip")]
        [Label("$Mods.Twaila.MaxHeight.Label")]
        public int MaxHeight;

        [DefaultValue(12)]
        [Label("$Mods.Twaila.PanelPadding")]
        public int PanelPadding;

        [DefaultValue(120)]
        [Range(0, 1000)]
        [Tooltip("$Mods.Twaila.CycleDelay.Tooltip")]
        [Label("$Mods.Twaila.CycleDelay.Label")]
        public int CycleDelay;

        [DefaultValue(DrawMode.Shrink)]
        [DrawTicks]
        [Label("$Mods.Twaila.ContentSetting")]
        public DrawMode ContentSetting = DrawMode.Shrink;

        [DefaultValue(0.25f)]
        [Label("$Mods.Twaila.HoverOpacity")]
        public float HoverOpacity;

        public enum DisplayMode
        {
            [Label("$Mods.Twaila.Enums.On")] On,
            [Label("$Mods.Twaila.Enums.Off")] Off,
            [Label("$Mods.Twaila.Enums.Automatic")] Automatic
        }

        public enum ContextUpdateMode
        {
            [Label("$Mods.Twaila.Enums.Manual")] Manual,
            [Label("$Mods.Twaila.Enums.Automatic")] Automatic
        }

        [DefaultValue(ContextUpdateMode.Automatic)]
        [DrawTicks]
        [Label("$Mods.Twaila.ContextMode")]
        public ContextUpdateMode ContextMode;

        [DefaultValue(true)]
        [Label("$Mods.Twaila.ShowBackground")]
        public bool ShowBackground;

        [DefaultValue(true)]
        [Label("$Mods.Twaila.ShowInFullscreenMap")]
        public bool ShowInFullscreenMap;

        [SeparatePage]
        [Label("$Mods.Twaila.UIDisplaySettings")]
        public DisplaySettings UIDisplaySettings = new DisplaySettings();

        [SeparatePage]
        [Label("$Mods.Twaila.PanelColor")]
        public ColorWrapper PanelColor = new ColorWrapper(44, 57, 105, 178);


        [Header("$Mods.Twaila.UIText")]

        [DefaultValue(true)]
        [Label("$Mods.Twaila.TextShadow")]
        public bool TextShadow;

        [DefaultValue(false)]
        [Tooltip("$Mods.Twaila.OverrideColor.Tooltip")]
        [Label("$Mods.Twaila.OverrideColor.Label")]
        public bool OverrideColor;

        [SeparatePage]
        [Label("$Mods.Twaila.TextColor")]
        public ColorWrapper TextColor = new ColorWrapper(255, 255, 255, 255);


        [Header("$Mods.Twaila.UIImage")]

        [DefaultValue(false)]
        [Tooltip("$Mods.Twaila.UseItemTextures.Tooltip")]
        [Label("$Mods.Twaila.UseItemTextures.Label")]
        public bool UseItemTextures;

        [DefaultValue(25)]
        [Tooltip("$Mods.Twaila.ReservedImageWidth.Tooltip")]
        [Label("$Mods.Twaila.ReservedImageWidth.Label")]
        public int ReservedImageWidth;

        [DefaultValue(false)]
        [Tooltip("$Mods.Twaila.UseTextHeightForImage.Tooltip")]
        [Label("$Mods.Twaila.UseTextHeightForImage.Label")]
        public bool UseTextHeightForImage;

        

        public enum DisplayType
        {
            [Label("$Mods.Twaila.Enums.Name")] Name,
            [Label("$Mods.Twaila.Enums.Icon")] Icon,
            [Label("$Mods.Twaila.Enums.Both")] Both,
            [Label("$Mods.Twaila.Enums.Off")] Off
        }

        public enum NameType
        {
            [Label("$Mods.Twaila.Enums.DisplayName")] DisplayName,
            [Label("$Mods.Twaila.Enums.InternalName")] InternalName,
            [Label("$Mods.Twaila.Enums.FullName")] FullName,
            [Label("$Mods.Twaila.Enums.Off")] Off
        }

        public class Content
        {
            [SeparatePage]
            [Label("$Mods.Twaila.Content.ContentPriorities")]
            public Priorities ContentPriorities = new Priorities();

            [SeparatePage]
            [Label("$Mods.Twaila.Content.NpcContent")]
            public NpcContent NpcContent = new NpcContent();

            [DefaultValue(true)]
            [Label("$Mods.Twaila.Content.ShowImage")]
            public bool ShowImage;

            [DrawTicks]
            [Label("$Mods.Twaila.Content.ShowName")]
            public NameType ShowName;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.Content.ShowMod")]
            public bool ShowMod;

            [DefaultValue(false)]
            [Label("$Mods.Twaila.Content.ShowId")]
            public bool ShowId;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.Content.ShowPickaxePower")]
            public bool ShowPickaxePower;

            [DrawTicks]
            [Label("$Mods.Twaila.Content.ShowPickaxe")]
            public DisplayType ShowPickaxe;

            [DrawTicks]
            [Label("$Mods.Twaila.Content.ShowWire")]
            public DisplayType ShowWire;

            [DrawTicks]
            [Label("$Mods.Twaila.Content.ShowActuator")]
            public DisplayType ShowActuator;

            [DrawTicks]
            [Label("$Mods.Twaila.Content.ShowPaint")]
            public DisplayType ShowPaint;

            [DrawTicks]
            [Label("$Mods.Twaila.Content.ShowCoating")]
            public DisplayType ShowCoating = DisplayType.Off;

            [DrawTicks]
            [Label("$Mods.Twaila.Content.ShowContainedItems")]
            public DisplayType ShowContainedItems;

            [Range(1, 20)]
            [DefaultValue(8)]
            [Label("$Mods.Twaila.Content.IconsPerRow")]
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
            }

            public override bool Equals(object obj)
            {
                if(obj is Content other)
                {
                    return ShowImage == other.ShowImage && ShowMod == other.ShowMod && ShowName == other.ShowName
                        && ShowPickaxePower == other.ShowPickaxePower && ShowWire == other.ShowWire && 
                        ShowActuator == other.ShowActuator && ShowPaint == other.ShowPaint && ShowPickaxe == other.ShowPickaxe
                        && ShowId == other.ShowId && ShowContainedItems == other.ShowContainedItems && ShowCoating == other.ShowCoating
                        && NpcContent == other.NpcContent && IconsPerRow == other.IconsPerRow;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { ShowImage, ShowMod, ShowName, ShowPickaxePower, ShowWire, ShowActuator, ShowPaint, NpcContent}.GetHashCode();
            }
        }

        public class NpcContent
        {
            [DefaultValue(true)]
            [Label("$Mods.Twaila.NpcContent.ShowHp")]
            public bool ShowHp;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.NpcContent.ShowDefense")]
            public bool ShowDefense;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.NpcContent.ShowAttack")]
            public bool ShowAttack;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.NpcContent.ShowKnockback")]
            public bool ShowKnockback;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.NpcContent.ShowKills")]
            public bool ShowKills;

            [Range(1, 5)]
            [DefaultValue(3)]
            [Label("$Mods.Twaila.NpcContent.StatElementsPerRow")]
            public int StatElementsPerRow;

            [DrawTicks]
            [Label("$Mods.Twaila.NpcContent.ShowBuffs")]
            public DisplayType ShowBuffs;

            [Range(1, 20)]
            [DefaultValue(10)]
            [Label("$Mods.Twaila.NpcContent.BuffIconsPerRow")]
            public int BuffIconsPerRow;

            [Range(1, 10)]
            [DefaultValue(3)]
            [Label("$Mods.Twaila.NpcContent.BuffTextsPerRow")]
            public int BuffTextsPerRow;

            public NpcContent()
            {
                ShowHp = true;
                ShowDefense = true;
                ShowAttack = true;
                ShowKnockback = true;
                ShowKills = true;
                StatElementsPerRow = 3;
                ShowBuffs = DisplayType.Icon;
                BuffIconsPerRow = 10;
                BuffTextsPerRow = 3;
            }

            public override bool Equals(object obj)
            {
                if (obj is NpcContent other)
                {
                    return ShowHp == other.ShowHp && ShowDefense == other.ShowDefense && ShowAttack == other.ShowAttack
                        && ShowKnockback == other.ShowKnockback && ShowKills == other.ShowKills && StatElementsPerRow == other.StatElementsPerRow
                        && ShowBuffs == other.ShowBuffs && BuffIconsPerRow == other.BuffIconsPerRow && BuffTextsPerRow == other.BuffTextsPerRow;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ShowHp, ShowDefense, ShowAttack, ShowKnockback, ShowKills, StatElementsPerRow, ShowBuffs);
            }
        }

        public class Priorities
        {
            [Header("$Mods.Twaila.Priorities.Header")]

            [DefaultValue(0)]
            [Label("$Mods.Twaila.Priorities.WirePriority")]
            public int WirePriority;

            [DefaultValue(1)]
            [Label("$Mods.Twaila.Priorities.NpcPriority")]
            public int NpcPriority;

            [DefaultValue(2)]
            [Label("$Mods.Twaila.Priorities.TilePrioity")]
            public int TilePrioity;

            [DefaultValue(3)]
            [Label("$Mods.Twaila.Priorities.LiquidPriority")]
            public int LiquidPriority;

            [DefaultValue(4)]
            [Label("$Mods.Twaila.Priorities.WallPriority")]
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

        public class ColorWrapper
        {
            [Label("$Mods.Twaila.Color")]
            public Color Color;

            public ColorWrapper(byte r, byte g, byte b, byte a)
            {
                Color = new Color(r, g, b, a);
            }

            public ColorWrapper()
            {
                Color = new Color(0, 0, 0, 0);
            }

            public override bool Equals(object obj)
            {
                if (obj is Color other)
                {
                    return Color.Equals(other);
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return Color.GetHashCode();
            }
        }

        public class DisplaySettings
        {
            [DefaultValue(DisplayMode.Automatic)]
            [DrawTicks]
            [Label("$Mods.Twaila.UIDisplay")]
            public DisplayMode UIDisplay = DisplayMode.Automatic;

            [Header("$Mods.Twaila.AutomaticOptions")]
            [DefaultValue(false)]
            [Tooltip("$Mods.Twaila.HideUIForAir.Tooltip")]
            [Label("$Mods.Twaila.HideUIForAir.Label")]
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

        public class AntiCheatSettings
        {
            [DefaultValue(true)]
            [Tooltip("$Mods.Twaila.HideUnrevealedTiles.Tooltip")]
            [Label("$Mods.Twaila.HideUnrevealedTiles.Label")]
            public bool HideUnrevealedTiles;

            [DefaultValue(true)]
            [Tooltip("$Mods.Twaila.HideWires.Tooltip")]
            [Label("$Mods.Twaila.HideWires.Label")]
            public bool HideWires;

            [DefaultValue(true)]
            [Tooltip("$Mods.Twaila.HideEchoTiles.Tooltip")]
            [Label("$Mods.Twaila.HideEchoTiles.Label")]
            public bool HideEchoTiles;

            [DefaultValue(true)]
            [Tooltip("$Mods.Twaila.HideSuspiciousTiles.Tooltip")]
            [Label("$Mods.Twaila.HideSuspiciousTiles.Label")]
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

        public void Save()
        {
            try
            {
                typeof(ConfigManager)
                    ?.GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic)
                    ?.Invoke(null, new object[] { this });
            }
            catch { }
        }
    }
}