using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Twaila.UI;

namespace Twaila
{
    public class TwailaConfig : ModConfig
    {
        public static TwailaConfig Instance => ModContent.GetInstance<TwailaConfig>();

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Features")]

        [SeparatePage]
        public AntiCheatSettings AntiCheat = new AntiCheatSettings();

        [SeparatePage]
        public Content DisplayContent = new Content();


        [Header("UIPosition")]

        [DefaultValue(true)]
        public bool UseDefaultPosition;

        [DefaultValue(true)]
        public bool LockPosition;

        [DrawTicks]
        [DefaultValue(HorizontalAnchor.Center)]
        public HorizontalAnchor AnchorX = HorizontalAnchor.Center;
        public enum HorizontalAnchor
        {
            Left,
            Center,
            Right
        };

        [DrawTicks]
        [DefaultValue(VerticalAnchor.Top)]
        public VerticalAnchor AnchorY = VerticalAnchor.Top;
        public enum VerticalAnchor
        {
            Bottom,
            Center,
            Top
        };

        [DefaultValue(0)]
        [Range(0, 2000)]
        public int AnchorPosX;

        [DefaultValue(0)]
        [Range(0, 2000)]
        public int AnchorPosY;


        [Header("UIPanel")]

        [DefaultValue(40)]
        public int MaxWidth;

        [DefaultValue(40)]
        public int MaxHeight;

        [DefaultValue(12)]
        public int PanelPadding;

        [DefaultValue(120)]
        [Range(0, 1000)]
        public int CycleDelay;

        [DefaultValue(DrawMode.Shrink)]
        [DrawTicks]
        public DrawMode ContentSetting = DrawMode.Shrink;

        [DefaultValue(0.25f)]
        public float HoverOpacity;

        public enum DisplayMode
        {
            On,
            Off,
            Automatic
        }

        public enum ContextUpdateMode
        {
            Manual,
            Automatic
        }

        [DefaultValue(ContextUpdateMode.Automatic)]
        [DrawTicks]
        public ContextUpdateMode ContextMode;

        [DefaultValue(true)]
        public bool ShowBackground;

        [DefaultValue(true)]
        public bool ShowInFullscreenMap;

        [SeparatePage]
        public DisplaySettings UIDisplaySettings = new DisplaySettings();

        [SeparatePage]
        public ColorWrapper PanelColor = new ColorWrapper(44, 57, 105, 178);


        [Header("UIText")]

        [DefaultValue(true)]
        public bool TextShadow;

        [DefaultValue(false)]
        public bool OverrideColor;

        [SeparatePage]
        public ColorWrapper TextColor = new ColorWrapper(255, 255, 255, 255);


        [Header("UIImage")]

        [DefaultValue(false)]
        public bool UseItemTextures;

        [DefaultValue(25)]
        public int ReservedImageWidth;

        public enum DisplayType
        {
            Name,
            Icon,
            Both,
            Off
        }

        public enum NameType
        {
            DisplayName,
            InternalName,
            FullName,
            Off
        }

        public class Content
        {
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
            }

            public override bool Equals(object obj)
            {
                if(obj is Content other)
                {
                    return ShowImage == other.ShowImage && ShowMod == other.ShowMod && ShowName == other.ShowName
                        && ShowPickaxePower == other.ShowPickaxePower && ShowWire == other.ShowWire && 
                        ShowActuator == other.ShowActuator && ShowPaint == other.ShowPaint && ShowPickaxe == other.ShowPickaxe
                        && ShowId == other.ShowId && ShowContainedItems == other.ShowContainedItems && ShowCoating == other.ShowCoating;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { ShowImage, ShowMod, ShowName, ShowPickaxePower, ShowWire, ShowActuator, ShowPaint }.GetHashCode();
            }
        }

        public class ColorWrapper
        {
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