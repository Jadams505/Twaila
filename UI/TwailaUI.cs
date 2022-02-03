using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;

namespace Twaila.UI
{
    public class TwailaUI
    {
        private static UserInterface _interface;
        private static UIState _state;
        private static UIPanel _panel;
        private static UIText _text;
        private static UIImage _image;

        public static bool Enabled { get; private set; }
        public static void Initialize()
        {
            Enabled = true;

            _interface = new UserInterface();
            _panel = new UIPanel();
            _state = new UIState();
            _text = new UIText("Test");
            _image = new UIImage(TextureManager.BlankTexture);
            _image.VAlign = 0.5f;
            _panel.Append(_text);
            _panel.Append(_image);
            _panel.Width.Set(300, 0);
            _panel.Height.Set(100, 0);
            _panel.HAlign = 0.5f;
            _panel.Top.Set(0, 0);

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
            string message = "";
            if(Main.tile[Player.tileTargetX, Player.tileTargetY] != null)
            {
                Tile tile = Main.tile[Player.tileTargetX,Player.tileTargetY];
                ModTile mTile = TileLoader.GetTile(tile.type);
                if (mTile != null)
                {
                    message = mTile.Name;
                }
                else
                {
                    if (tile.active())
                    {
                        for (int i = 0; i < ItemID.Count; ++i)
                        {
                            Item item = new Item();
                            item.SetDefaults(i);
                            if (item.createTile == tile.type)
                            {
                                message = Lang.GetItemNameValue(i);
                                _image.SetImage(Main.itemTexture[i]);
                            }
                        }
                    }
                }
            }
            _text.SetText(message);
            
            
        }

        public static void ToggleVisibility(bool? visible)
        {
            if(visible == null)
            {
                Enabled = !Enabled;
            }
            else
            {
                Enabled = visible.Value;               
            }
            _interface?.SetState(_state);
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
            _text = null;
            _image = null;
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
