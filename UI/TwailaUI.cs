using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Twaila.Config;
using Twaila.Context;
using Twaila.Systems;

namespace Twaila.UI
{
    public class TwailaUI
    {
        private static UserInterface _interface;
        private static UIState _state;
        private static TwailaPanel _panel;

        public static bool Enabled { get; private set; }
        public static void Initialize()
        {
            Enabled = true;
            _interface = new UserInterface();
            _panel = new TwailaPanel();
            _state = new UIState();

            _state.Append(_panel);
            _interface?.SetState(_state);
        }

        public static void Update(GameTime time)
        {
            switch (TwailaConfig.Instance.UIDisplaySettings.UIDisplay)
            {
                case TwailaConfig.DisplayMode.On:
                    Enabled = true;
                    break;
                case TwailaConfig.DisplayMode.Off:
                    Enabled = false;
                    break;
                case TwailaConfig.DisplayMode.Automatic:
                    if (TwailaConfig.Instance.UIDisplaySettings.HideUIForAir)
                    {
                        TwailaPoint cursorInfo = GetCursorInfo();
                        bool panelIsHovered = _panel.ContainsPoint(Main.MouseScreen);
                        bool NoValidContexts(TwailaPoint cursorInfo) => ContextSystem.Instance.ContextEntryCountAt(cursorInfo) == 0;
                        bool ManualContextIsNull(TwailaPoint cursorInfo) => TwailaConfig.Instance.ContextMode == TwailaConfig.ContextUpdateMode.Manual && ContextSystem.Instance.CurrentContext(cursorInfo) == null;
                        
                        if ((!IsMouseOnScreen() || NoValidContexts(cursorInfo) || ManualContextIsNull(cursorInfo))
                            && !panelIsHovered && !_panel.IsDragging())
                        {
                            Enabled = false;
                            break;
                        }
                    }
                    Enabled = true;
                    break;
            }
            _interface?.Update(time);
        }

        public static TwailaPoint GetCursorInfo()
        {
            Point mouse = MouseWorldSafe();
            Point tile = new Point(Player.tileTargetX, Player.tileTargetY);
            Point smart = new Point(Main.SmartCursorX, Main.SmartCursorY);

            float mapSpaceX = Main.mapFullscreenScale * (10 - Main.mapFullscreenPos.X) + (Main.screenWidth / 2.0f);
            float mapSpaceY = Main.mapFullscreenScale * (10 - Main.mapFullscreenPos.Y) + (Main.screenHeight / 2.0f);
            float x = (Main.mouseX - mapSpaceX) / Main.mapFullscreenScale + 10;
            float y = (Main.mouseY - mapSpaceY) / Main.mapFullscreenScale + 10;
            Point map = new Point((int)x, (int)y);

            return new TwailaPoint(mouse, tile, smart, map);
        }

        private static Point MouseWorldSafe()
        {
            int mouseX = Main.mouseX;
            int mouseY = Main.mouseY;
            int lastMouseX = Main.lastMouseX;
            int lastMouseY = Main.lastMouseY;

            PlayerInput.SetZoom_Unscaled();

            // This call has side effects, so I reset mouse positions just in case
            PlayerInput.SetZoom_MouseInWorld(); 
            Point mouse = Main.MouseWorld.ToPoint();

            Main.mouseX = mouseX;
            Main.mouseY = mouseY;
            Main.lastMouseX = lastMouseX;
            Main.lastMouseY = lastMouseY;

            return mouse;
        }

        public static bool IsMouseOnScreen() => _panel.Parent.ContainsPoint(Main.MouseScreen);

        private static string NameOfCurrentContext => ContextSystem.Instance.ContextEntries[TwailaConfig.Instance.CurrentContext.Index].Name.Value;

        public static void NextContext()
        {
            TwailaConfig.Instance.CurrentContext.SetIndex(ContextSystem.Instance.NextContextIndex());
            Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").Format(NameOfCurrentContext));
            _panel.tick = 0;
        }

        public static void PrevContext()
        {
            TwailaConfig.Instance.CurrentContext.SetIndex(ContextSystem.Instance.PrevContextIndex());
            Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").Format(NameOfCurrentContext));
            _panel.tick = 0;
        }

        public static void NextNonNullContext()
        {
            ContextSystem.Instance.NextNonNullContext(GetCursorInfo());
            Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").Format(NameOfCurrentContext));
            _panel.tick = 0;
        }

        public static void PrevNonNullContext()
        {
            ContextSystem.Instance.PrevNonNullContext(GetCursorInfo());
            Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").Format(NameOfCurrentContext));
            _panel.tick = 0;
        }

        public static void Load()
        {
            if (!Main.dedServ)
            {
                Initialize();
            }
        }

        public static void Unload()
        {
            _interface = null;
            _panel = null;
            _state = null;
        }

        public static void Draw(GameTime time)
        {
            if (Enabled)
            {
                _interface?.Draw(Main.spriteBatch, time);
            }
        }
    }
}
