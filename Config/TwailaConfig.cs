using System;
using System.Reflection;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Twaila.UI;
using Twaila.Systems;

namespace Twaila.Config
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

        [SeparatePage]
        public PanelContextData PanelPositionData = new PanelContextData();

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

        [DefaultValue(ContextUpdateMode.Automatic)]
        [DrawTicks]
        public ContextUpdateMode ContextMode;

        public ContextIndex CurrentContext = new ContextIndex();

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
        public bool UseTextHeightForImage;

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

        public enum HorizontalAnchor
        {
            Left,
            Center,
            Right
        };

        public enum VerticalAnchor
        {
            Bottom,
            Center,
            Top
        };

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
            Number,
            Text,
            Both,
            Off
        }

        public override void OnChanged()
        {
            ContextSystem.Instance?.SortContexts();

            if (PanelPositionData.SyncPositionData)
            {
                PanelPositionData.ClosedInventory.CopyTo(PanelPositionData.OpenInventory);
                PanelPositionData.ClosedInventory.CopyTo(PanelPositionData.InFullscreenMap);
            }

            if(ContextSystem.Instance != null)
            {
                CurrentContext.SetIndex(CurrentContext.Index);
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