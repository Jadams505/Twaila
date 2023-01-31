using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                switch (TwailaConfig.Get().UIDisplaySettings.UIDisplay)
                {
                    case TwailaConfig.DisplayMode.On:
                        TwailaConfig.Get().UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.Off;
                        break;
                    case TwailaConfig.DisplayMode.Off:
                        TwailaConfig.Get().UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.Automatic;
                        break;
                    case TwailaConfig.DisplayMode.Automatic:
                        TwailaConfig.Get().UIDisplaySettings.UIDisplay = TwailaConfig.DisplayMode.On;
                        break;
                }
				Main.NewText(Language.GetText("Mods.Twaila.DisplayModeMessage").WithFormatArgs(TwailaConfig.Get().UIDisplaySettings.UIDisplay.ToLocalizedString()));
			}

            if (CycleContextMode.JustPressed)
            {
                if (TwailaConfig.Get().ContextMode == TwailaConfig.ContextUpdateMode.Manual)
                {
                    TwailaConfig.Get().ContextMode = TwailaConfig.ContextUpdateMode.Automatic;
                }
                else if(TwailaConfig.Get().ContextMode == TwailaConfig.ContextUpdateMode.Automatic)
                {
                    TwailaConfig.Get().ContextMode = TwailaConfig.ContextUpdateMode.Manual;
                }
				Main.NewText(Language.GetText("Mods.Twaila.ContextModeMessage").WithFormatArgs(TwailaConfig.Get().ContextMode.ToLocalizedString()));
            }

            if (TwailaConfig.Get().ContextMode == TwailaConfig.ContextUpdateMode.Manual)
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
            else if(TwailaConfig.Get().ContextMode == TwailaConfig.ContextUpdateMode.Automatic)
            {
                if (NextContext.JustPressed)
                {
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
