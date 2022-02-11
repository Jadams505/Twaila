using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
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
                UpdateText();
                _interface.Update(time);
            }
        }

        private static void UpdateText()
        {
            if(Main.tile[Player.tileTargetX, Player.tileTargetY] != null)
            {
                Tile tile = Main.tile[Player.tileTargetX,Player.tileTargetY];
                if (tile.active() && Enabled)
                {
                    ToggleVisibility(true);
                    TwailaUtil.UpdateUI(_panel, tile);
                    return;
                }
            }
            //_interface?.SetState(null);
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
