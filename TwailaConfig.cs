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

        [DefaultValue(false)]
        public bool UseItemTextures;

        [Header("UI Position")]

        [DefaultValue(true)]
        [Tooltip("Dragging the panel automatically disables this")]
        public bool UseDefaultPosition;

        [DefaultValue(true)]
        public bool LockPosition;

        [DrawTicks]
        [DefaultValue(HorizontalAnchor.Center)]
        public HorizontalAnchor AnchorX;
        public enum HorizontalAnchor
        {
            Left, Center, Right
        };

        [DrawTicks]
        [DefaultValue(VerticalAnchor.Top)]
        public VerticalAnchor AnchorY;
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