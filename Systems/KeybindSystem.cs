using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Twaila.UI;

namespace Twaila.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind ToggleUI { get; private set; }
        public static ModKeybind NextContext { get; private set; }
        public static ModKeybind PrevContext { get; private set; }
        public static ModKeybind LockContext { get; private set; }


        public override void Load()
        {
            ToggleUI = KeybindLoader.RegisterKeybind(Mod, "Cycle UI Display Mode", "Mouse3");
            NextContext = KeybindLoader.RegisterKeybind(Mod, "Next Context", Keys.Right);
            PrevContext = KeybindLoader.RegisterKeybind(Mod, "Previous Context", Keys.Left);
            LockContext = KeybindLoader.RegisterKeybind(Mod, "Lock Context", Keys.Up);
        }

        public override void Unload()
        {
            ToggleUI = null;
            NextContext = null;
            PrevContext = null;
            LockContext = null;
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
                Main.NewText("Display Mode: " + TwailaConfig.Get().UIDisplaySettings.UIDisplay);
            }

            if (LockContext.JustPressed)
            {
                if (TwailaConfig.Get().LockContext)
                {
                    TwailaConfig.Get().LockContext = false;
                    Main.NewText("Context Unlocked");
                }
                else
                {
                    TwailaConfig.Get().LockContext = true;
                    Main.NewText("Context Locked");
                }
            }

            if (TwailaConfig.Get().LockContext)
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
            else
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
