using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Twaila.UI;

namespace Twaila
{
    [Label("$Mods.Twaila.ModConfig")]
    public class TwailaConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.Twaila.Features")]

        [DefaultValue(true)]
        [Tooltip("$Mods.Twaila.AntiCheat.Tooltip")]
        [Label("$Mods.Twaila.AntiCheat.Label")]
        public bool AntiCheat;

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
            [Label("$Mods.Twaila.enum.Left")]Left,
            [Label("$Mods.Twaila.enum.Center")]Center,
            [Label("$Mods.Twaila.enum.Right")]Right
        };

        [DrawTicks]
        [DefaultValue(VerticalAnchor.Top)]
        [Label("$Mods.Twaila.AnchorY")]
        public VerticalAnchor AnchorY = VerticalAnchor.Top;
        public enum VerticalAnchor
        {
            [Label("$Mods.Twaila.enum.Bottom")] Bottom,
            [Label("$Mods.Twaila.enum.Center")] Center,
            [Label("$Mods.Twaila.enum.Top")] Top
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

        [DefaultValue(30)]
        [Tooltip("$Mods.Twaila.MaxWidth.Tooltip")]
        [Label("$Mods.Twaila.MaxWidth.Label")]
        public int MaxWidth;

        [DefaultValue(30)]
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
            [Label("$Mods.Twaila.enum.On")] On,
            [Label("$Mods.Twaila.enum.Off")] Off,
            [Label("$Mods.Twaila.enum.Automatic")] Automatic
        }

        public enum ContextUpdateMode
        {
            [Label("$Mods.Twaila.enum.Manual")] Manual,
            [Label("$Mods.Twaila.enum.Automatic")] Automatic
        }

        [DefaultValue(ContextUpdateMode.Automatic)]
        [DrawTicks]
        [Label("$Mods.Twaila.ContextMode")]
        public ContextUpdateMode ContextMode;

        [DefaultValue(true)]
        [Label("$Mods.Twaila.ShowBackground")]
        public bool ShowBackground;

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

        public enum DisplayType
        {
            [Label("$Mods.Twaila.enum.Name")] Name,
            [Label("$Mods.Twaila.enum.Icon")] Icon,
            [Label("$Mods.Twaila.enum.Both")] Both,
            [Label("$Mods.Twaila.enum.Off")] Off
        }

        public enum NameType
        {
            [Label("$Mods.Twaila.enum.DisplayName")] DisplayName,
            [Label("$Mods.Twaila.enum.InternalName")] InternalName,
            [Label("$Mods.Twaila.enum.FullName")] FullName,
            [Label("$Mods.Twaila.enum.Off")] Off
        }

        public class Content
        {
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
            [Label("$Mods.Twaila.Content.ShowContainedItems")]
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
                        && ShowId == other.ShowId && ShowContainedItems == other.ShowContainedItems;
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

        public void Save()
        {
            Directory.CreateDirectory(ConfigManager.ModConfigPath);
            string filename = Mod.Name + "_" + Name + ".json";
            string path = Path.Combine(ConfigManager.ModConfigPath, filename);
            string json = JsonConvert.SerializeObject((object)this, ConfigManager.serializerSettings);
            File.WriteAllText(path, json);
        }

        public static TwailaConfig Get()
        {
            return ModContent.GetInstance<TwailaConfig>();
        }
    }
}