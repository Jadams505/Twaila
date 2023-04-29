using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind ToggleUI { get; private set; }
        public static ModKeybind NextContext { get; private set; }
        public static ModKeybind PrevContext { get; private set; }
        public static ModKeybind CycleContextMode { get; private set; }


        public override void Load()
        {
            ToggleUI = KeybindLoader.RegisterKeybind(Mod, "CycleUIDisplayMode", "Mouse3");
            NextContext = KeybindLoader.RegisterKeybind(Mod, "NextContext", Keys.Right);
            PrevContext = KeybindLoader.RegisterKeybind(Mod, "PreviousContext", Keys.Left);
            CycleContextMode = KeybindLoader.RegisterKeybind(Mod, "CycleContextMode", Keys.Up);
        }

        public override void Unload()
        {
            ToggleUI = null;
            NextContext = null;
            PrevContext = null;
            CycleContextMode = null;
        }

        public static void HandleKeys(TwailaPlayer player)
        {
            if (ToggleUI.JustPressed)
            {
                switch (TwailaConfig.Instance.UIDisplaySettings.UIDisplay)
                {
                    case TwailaConfig.DisplayMode.On:
                        TwailaConfig.Instance.UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.Off;
                        break;
                    case TwailaConfig.DisplayMode.Off:
                        TwailaConfig.Instance.UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.Automatic;
                        break;
                    case TwailaConfig.DisplayMode.Automatic:
                        TwailaConfig.Instance.UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.On;
                        break;
                }
                Main.NewText(Language.GetText("Mods.Twaila.DisplayModeMessage").WithFormatArgs(TwailaConfig.Instance.UIDisplaySettings.UIDisplay.ToLocalizedString()));
            }

            if (CycleContextMode.JustPressed)
            {
                if (TwailaConfig.Instance.ContextMode == TwailaConfig.ContextUpdateMode.Manual)
                {
                    TwailaConfig.Instance.ContextMode = TwailaConfig.ContextUpdateMode.Automatic;
                }
                else if(TwailaConfig.Instance.ContextMode == TwailaConfig.ContextUpdateMode.Automatic)
                {
                    TwailaConfig.Instance.ContextMode = TwailaConfig.ContextUpdateMode.Manual;
                }
                Main.NewText(Language.GetText("Mods.Twaila.ContextModeMessage").WithFormatArgs(TwailaConfig.Instance.ContextMode.ToLocalizedString()));
            }

            if (TwailaConfig.Instance.ContextMode == TwailaConfig.ContextUpdateMode.Manual)
            {
                if (NextContext.JustPressed)
                {
                    TwailaUI.NextContext();
                }

                if (PrevContext.JustPressed)
                {
                    TwailaUI.PrevContext();
                }
            }
            else if(TwailaConfig.Instance.ContextMode == TwailaConfig.ContextUpdateMode.Automatic)
            {
                if (NextContext.JustPressed)
                {
                    if (TwailaConfig.Instance.ContentSetting == DrawMode.Shrink)
                        TwailaConfig.Instance.ContentSetting = DrawMode.Trim;
                    else
                        TwailaConfig.Instance.ContentSetting = DrawMode.Shrink;
                    TwailaUI.NextNonNullContext();
                }

                if (PrevContext.JustPressed)
                {
                    TwailaUI.PrevNonNullContext();
                }
            }
        }
    }
}
