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
        [DefaultValue(Anchor.Center)]
        public Anchor DrawAnchor;
        public enum Anchor
        {
            Left, Center, Right
        };


        public int UIPosX => TwailaUI.panel == null ? 0 : (int)TwailaUI.panel.anchorPos.X;

        public int UIPosY => TwailaUI.panel == null ? 0 : (int)TwailaUI.panel.anchorPos.Y;

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