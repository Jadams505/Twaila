using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.UI;
using Twaila.Context;
using Twaila.Systems;
using Twaila.Util;

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
            switch (TwailaConfig.Get().UIDisplaySettings.UIDisplay)
            {
                case TwailaConfig.DisplayMode.On:
                    Enabled = true;
                    break;
                case TwailaConfig.DisplayMode.Off:
                    Enabled = false;
                    break;
                case TwailaConfig.DisplayMode.Automatic:
                    if (TwailaConfig.Get().UIDisplaySettings.HideUIForAir)
                    {
                        if ((ContextSystem.Instance.ContextEntryCountAt(GetCursorInfo()) == 0 ||
                            (TwailaConfig.Get().ContextMode == TwailaConfig.ContextUpdateMode.Manual && ContextSystem.Instance.CurrentContext(_panel.currIndex, GetCursorInfo()) == null))
                            && !_panel.ContainsPoint(Main.MouseScreen) && !Main.SmartCursorShowing && !_panel.IsDragging())
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
			Point mouse = new Point(Main.mouseX, Main.mouseY);
			Point tile = new Point(Player.tileTargetX, Player.tileTargetY);
			Point smart = new Point(Main.SmartCursorX, Main.SmartCursorY);

			float mapSpaceX = Main.mapFullscreenScale * (10 - Main.mapFullscreenPos.X) + (Main.screenWidth / 2.0f);
			float mapSpaceY = Main.mapFullscreenScale * (10 - Main.mapFullscreenPos.Y) + (Main.screenHeight / 2.0f);
			float x = (Main.mouseX - mapSpaceX) / Main.mapFullscreenScale + 10;
			float y = (Main.mouseY - mapSpaceY) / Main.mapFullscreenScale + 10;
			Point map = new Point((int)x, (int)y);

			return new TwailaPoint(mouse, tile, smart, map);
        }

        public static bool InBounds(int targetX, int targetY)
        {
			if (Main.mapFullscreen)
			{
				return targetX >= 0 && targetX < Main.maxTilesX && targetY >= 0 && targetY < Main.maxTilesY;
			}
            if (targetX < (Main.screenPosition.X - 16) / 16) // left
            {
                return false;
            }
            if (16 * targetX > PlayerInput.RealScreenWidth + Main.screenPosition.X) // right
            {
                return false;
            }
            if (targetY < (Main.screenPosition.Y - 16) / 16) // top
            {
                return false;
            }
            if (16 * targetY > PlayerInput.RealScreenHeight + Main.screenPosition.Y) // bottom
            {
                return false;
            }
            return true;
        }

        public static void NextContext()
        {
            _panel.currIndex = ContextSystem.Instance.NextContextIndex(_panel.currIndex);
			Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").WithFormatArgs(ContextSystem.Instance.ContextEntries[_panel.currIndex].Name.Value));
			_panel.tick = 0;
        }

        public static void PrevContext()
        {
            _panel.currIndex = ContextSystem.Instance.PrevContextIndex(_panel.currIndex);
			Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").WithFormatArgs(ContextSystem.Instance.ContextEntries[_panel.currIndex].Name.Value));
			_panel.tick = 0;
        }

        public static void NextNonNullContext()
        {
            ContextSystem.Instance.NextNonNullContext(ref _panel.currIndex, GetCursorInfo());
            Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").WithFormatArgs(ContextSystem.Instance.ContextEntries[_panel.currIndex].Name.Value));
            _panel.tick = 0;
        }

        public static void PrevNonNullContext()
        {
            ContextSystem.Instance.PrevNonNullContext(ref _panel.currIndex, GetCursorInfo());
            Main.NewText(Language.GetText("Mods.Twaila.CurrentContext").WithFormatArgs(ContextSystem.Instance.ContextEntries[_panel.currIndex].Name.Value));
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
