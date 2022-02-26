using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;
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
            ToggleVisibility(Enabled);
        }

        public static void Update(GameTime time)
        {
            if (Enabled)
            {
                UpdateUI();
                _interface?.Update(time);
            }
        }

        public static void UpdateUI()
        {
            int targetX, targetY;
            if (Main.SmartCursorShowing)
            {
                targetX = Main.SmartCursorX;
                targetY = Main.SmartCursorY;
            }
            else
            {
                targetX = Player.tileTargetX;
                targetY = Player.tileTargetY;
            }
            Tile tile = Main.tile[targetX, targetY];
            if (tile != null && tile.active() && Enabled && InBounds(targetX, targetY))
            {
                _panel?.UpdatePos(new Point(targetX, targetY));
                return;
            }
        }

        private static bool InBounds(int targetX, int targetY)
        {
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

        public static void ToggleVisibility(bool? visible)
        {
            Enabled = visible == null ? !Enabled : visible.Value;
            if (Enabled)
            {
                _interface?.SetState(_state);
            }
            else
            {
                _interface?.SetState(null);
            } 
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
            if(_interface?.CurrentState != null)
            {
                _interface.Draw(Main.spriteBatch, time);
            }
        }

    }
}
