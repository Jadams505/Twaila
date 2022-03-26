using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Twaila.UI;

namespace Twaila
{
    public class TwailaConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Features")]

        [DefaultValue(true)]
        [Tooltip("When this is ON only tiles that are revealed on the map will show on the panel")]
        public bool AntiCheat;

        [SeparatePage]
        [Label("Content Toggles")]
        public Content DisplayContent = new Content();


        [Header("UI Position")]

        [DefaultValue(true)]
        [Tooltip("Dragging the panel automatically disables this")]
        public bool UseDefaultPosition;

        [DefaultValue(true)]
        [Tooltip("When this is OFF you can use the mouse to move the panel")]
        public bool LockPosition;

        [DrawTicks]
        [DefaultValue(HorizontalAnchor.Center)]
        public HorizontalAnchor AnchorX = HorizontalAnchor.Center;
        public enum HorizontalAnchor
        {
            Left, Center, Right
        };

        [DrawTicks]
        [DefaultValue(VerticalAnchor.Top)]
        public VerticalAnchor AnchorY = VerticalAnchor.Top;
        public enum VerticalAnchor
        {
            Bottom, Center, Top
        };

        [DefaultValue(0)]
        [Range(0, 2000)]
        public int AnchorPosX;

        [DefaultValue(0)]
        [Range(0, 2000)]
        public int AnchorPosY;


        [Header("UI Panel")]

        [DefaultValue(30)]
        [Tooltip("Maximum % of the screen's width the panel can be")]
        [Label("MaxWidth (%)")]
        public int MaxWidth;

        [DefaultValue(20)]
        [Tooltip("Maximum % of the screen's height the panel can be")]
        [Label("MaxHeight (%)")]
        public int MaxHeight;

        [DefaultValue(12)]
        public int PanelPadding;

        [DefaultValue(DrawMode.Shrink)]
        [DrawTicks]
        public DrawMode ContentSetting = DrawMode.Shrink;

        [DefaultValue(true)]
        public bool ShowBackground;

        [SeparatePage]
        [Label("Panel Color")]
        public ColorWrapper PanelColor = new ColorWrapper(44, 57, 105, 178);


        [Header("UI Text")]

        [DefaultValue(true)]
        public bool TextShadow;

        [DefaultValue(false)]
        [Tooltip("Turn this on to override any pre-existing text color")]
        public bool OverrideColor;

        [SeparatePage]
        [Label("Text Color")]
        public ColorWrapper TextColor = new ColorWrapper(255, 255, 255, 255);


        [Header("UI Image")]

        [DefaultValue(false)]
        [Tooltip("ON: Uses the texture of the item that place the tile. OFF: Uses the texture of the sprite in the world")]
        public bool UseItemTextures;

        [DefaultValue(25)]
        [Tooltip("The % of the panel's width that is allocated for the image when the text is too long")]
        [Label("ReservedImageWidth (%)")]
        public int ReservedImageWidth;

        public class Content
        {
            [DefaultValue(true)]
            public bool ShowImage;

            [DefaultValue(true)]
            public bool ShowName;

            [DefaultValue(true)]
            public bool ShowMod;

            public Content()
            {
                ShowImage = true;
                ShowName = true;
                ShowMod = true;
            }

            public override bool Equals(object obj)
            {
                if(obj is Content other)
                {
                    return ShowImage == other.ShowImage && ShowMod == other.ShowMod && ShowName == other.ShowName;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { ShowImage, ShowMod, ShowName }.GetHashCode();
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

        public void Save()
        {
            Directory.CreateDirectory(ConfigManager.ModConfigPath);
            string filename = mod.Name + "_" + Name + ".json";
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