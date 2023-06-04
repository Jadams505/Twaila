using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Twaila.UI;
using Twaila.Systems;
using Terraria;

namespace Twaila
{
    public class TwailaConfig : ModConfig
    {
        public static TwailaConfig Instance => ModContent.GetInstance<TwailaConfig>();

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnChanged()
        {
            ContextSystem.Instance?.SortContexts();

            if (PanelPositionData.SyncPositionData)
            {
                PanelPositionData.ClosedInventory.CopyTo(PanelPositionData.OpenInventory);
                PanelPositionData.ClosedInventory.CopyTo(PanelPositionData.InFullscreenMap);
            }
        }

        [Header("$Mods.Twaila.Features")]

        [SeparatePage]
        public AntiCheatSettings AntiCheat = new AntiCheatSettings();

        [SeparatePage]
        public Content DisplayContent = new Content();


        [Header("UIPosition")]

        [SeparatePage]
        [Label("$Mods.Twaila.PanelPositionData.Label")]
        [Tooltip("$Mods.Twaila.PanelPositionData.Tooltip")]
        public PanelContextData PanelPositionData = new PanelContextData();
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

        [DefaultValue(false)]
        [Tooltip("$Mods.Twaila.UseTextHeightForImage.Tooltip")]
        [Label("$Mods.Twaila.UseTextHeightForImage.Label")]
        public bool UseTextHeightForImage;

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

        public enum NumberType
        {
            [Label("$Mods.Twaila.Enums.Number")] Number,
            [Label("$Mods.Twaila.Enums.Text")] Text,
            [Label("$Mods.Twaila.Enums.Both")] Both,
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
            [Label("$Mods.Twaila.Content.StatsPerRow")]
            public int TextsPerRow;

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
                TextsPerRow = 1;
            }

            public override bool Equals(object obj)
            {
                if(obj is Content other)
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

            [DrawTicks]
            [Label("$Mods.Twaila.NpcContent.ShowBuffs")]
            public DisplayType ShowBuffs;

            [DrawTicks]
            [Label("$Mods.Twaila.NpcContent.ShowHappiness")]
            public NumberType ShowHappiness;

            [DrawTicks]
            [Label("$Mods.Twaila.NpcContent.ShowNpcPreferences")]
            public DisplayType ShowNpcPreferences;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.NpcContent.ShowBiomePreferences")]
            public bool ShowBiomePreferences;

            [Range(1, 20)]
            [DefaultValue(3)]
            [Label("$Mods.Twaila.NpcContent.StatsPerRow")]
            public int StatsPerRow;

            [Range(1, 20)]
            [DefaultValue(8)]
            [Label("$Mods.Twaila.NpcContent.IconsPerRow")]
            public int IconsPerRow;

            [SeparatePage]
            [Label("$Mods.Twaila.NpcContent.HappinessColors")]
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

        public class HappinessColors
        {
            [Label("$Mods.Twaila.LoveColor")]
            public ColorWrapper LoveColor;

            [Label("$Mods.Twaila.LikeColor")]
            public ColorWrapper LikeColor;

            [Label("$Mods.Twaila.DislikeColor")]
            public ColorWrapper DislikeColor;

            [Label("$Mods.Twaila.HateColor")]
            public ColorWrapper HateColor;

            public HappinessColors()
            {
                LoveColor = new (73, 215, 43, 255);
                LikeColor = new (220, 215, 31, 255);
                DislikeColor = new (224, 111, 28, 255);
                HateColor = new (232, 56, 31, 255);
            }

            public override bool Equals(object obj)
            {
                return obj is HappinessColors other &&
                       LoveColor == other.LoveColor &&
                       LikeColor == other.LikeColor &&
                       DislikeColor == other.DislikeColor &&
                       HateColor == other.HateColor;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(LoveColor, LikeColor, DislikeColor, HateColor);
            }
        }

        public class PositionData
        {
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

            [DrawTicks]
            [DefaultValue(VerticalAnchor.Top)]
            [Label("$Mods.Twaila.AnchorY")]
            public VerticalAnchor AnchorY = VerticalAnchor.Top;

            [DefaultValue(0)]
            [Range(0, 2000)]
            [Label("$Mods.Twaila.AnchorPosX")]
            public int AnchorPosX;

            [DefaultValue(0)]
            [Range(0, 2000)]
            [Label("$Mods.Twaila.AnchorPosY")]
            public int AnchorPosY;

            [DefaultValue(true)]
            [Label("$Mods.Twaila.ShowUI.Label")]
            [Tooltip("$Mods.Twaila.ShowUI.Tooltip")]
            public bool ShowUI;

            public PositionData()
            {
                UseDefaultPosition = true;
                LockPosition = true;
                AnchorX = HorizontalAnchor.Center;
                AnchorY = VerticalAnchor.Top;
                AnchorPosX = 0;
                AnchorPosY = 0;
                ShowUI = true;
            }

            public void CopyTo(PositionData other)
            {
                other.UseDefaultPosition = UseDefaultPosition;
                other.LockPosition = LockPosition;
                other.AnchorX = AnchorX;
                other.AnchorY = AnchorY;
                other.AnchorPosX = AnchorPosX;
                other.AnchorPosY = AnchorPosY;
                other.ShowUI = ShowUI;
            }

            public override bool Equals(object obj)
            {
                return obj is PositionData data &&
                       AnchorX == data.AnchorX &&
                       AnchorY == data.AnchorY &&
                       AnchorPosX == data.AnchorPosX &&
                       AnchorPosY == data.AnchorPosY &&
                       ShowUI == data.ShowUI &&
                       LockPosition == data.LockPosition &&
                       UseDefaultPosition == data.UseDefaultPosition;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(AnchorX, AnchorY, AnchorPosX, AnchorPosY);
            }
        }

        public class PanelContextData
        {
            [DefaultValue(true)]
            [Label("$Mods.Twaila.SyncPositionData.Label")]
            [Tooltip("$Mods.Twaila.SyncPositionData.Tooltip")]
            public bool SyncPositionData = true;

            [SeparatePage]
            [Label("$Mods.Twaila.ClosedInventory.Label")]
            [Tooltip("$Mods.Twaila.ClosedInventory.Tooltip")]
            public PositionData ClosedInventory = new PositionData()
            {
                UseDefaultPosition = true,
                LockPosition = true,
                AnchorX = HorizontalAnchor.Center,
                AnchorY = VerticalAnchor.Top,
                AnchorPosX = 0,
                AnchorPosY = 0,
                ShowUI = true,
            };

            [SeparatePage]
            [Label("$Mods.Twaila.OpenInventory.Label")]
            [Tooltip("$Mods.Twaila.OpenInventory.Tooltip")]
            public PositionData OpenInventory = new PositionData()
            {
                UseDefaultPosition = true,
                LockPosition = true,
                AnchorX = HorizontalAnchor.Center,
                AnchorY = VerticalAnchor.Top,
                AnchorPosX = 0,
                AnchorPosY = 0,
                ShowUI = true,
            };

            [SeparatePage]
            [Label("$Mods.Twaila.InFullscreenMap.Label")]
            [Tooltip("$Mods.Twaila.InFullscreenMap.Tooltip")]
            public PositionData InFullscreenMap = new PositionData()
            {
                UseDefaultPosition = true,
                LockPosition = true,
                AnchorX = HorizontalAnchor.Center,
                AnchorY = VerticalAnchor.Top,
                AnchorPosX = 0,
                AnchorPosY = 0,
                ShowUI = true,
            };

            public PanelContextData()
            {
                SyncPositionData = true;
            }

            public PositionData GetActiveContext()
            {
                if (SyncPositionData)
                    return ClosedInventory;

                if (Main.mapFullscreen)
                    return InFullscreenMap;

                if (Main.playerInventory)
                    return OpenInventory;

                return ClosedInventory;
            }

            public override bool Equals(object obj)
            {
                return obj is PanelContextData data &&
                       ClosedInventory == data.ClosedInventory &&
                       OpenInventory == data.OpenInventory &&
                       InFullscreenMap == data.InFullscreenMap &&
                       SyncPositionData == data.SyncPositionData;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ClosedInventory, OpenInventory, InFullscreenMap, SyncPositionData);
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