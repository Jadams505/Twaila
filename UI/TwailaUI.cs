using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Twaila.Context;

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
                _interface?.Update(time);
            }
        }

        public static Point GetMousePos()
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
            return new Point(targetX, targetY);
        }

        public static TileContext GetContext(Point pos)
        {
            if(!InBounds(pos.X, pos.Y))
            {
                return new TileContext();
            }
            Tile tile = Framing.GetTileSafely(pos);

            if(tile.type == TileID.Trees || tile.type == TileID.MushroomTrees)
            {
                return new TreeContext(pos);
            }
            if(tile.type == TileID.Cactus)
            {
                return new CactusContext(pos);
            }
            if(tile.type == TileID.PalmTree)
            {
                return new PalmTreeContext(pos);
            }
            if (TileLoader.IsSapling(tile.type))
            {
                return new SaplingContext(pos);
            }
            return new TileContext(pos);
        }

        public static void DebugUI()
        {
            _panel?.ToggleDebugMode();
        }

        public static bool InBounds(int targetX, int targetY)
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
