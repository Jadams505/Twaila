using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using Twaila.Context;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaUI
    {
        private static UserInterface _interface;
        private static UIState _state;
        private static TwailaPanel _panel;
        public static bool debugMode = false;

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
            TileContext currentContext = GetContext(GetMousePos());
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
                        if ((currentContext.TileType == TileType.Empty) || TileUtil.IsBlockedByAntiCheat(currentContext) 
                            && !_panel.ContainsPoint(Main.mouseX, Main.mouseY) && !Main.SmartCursorShowing && !_panel.IsDragging())
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

            if(tile.TileType == TileID.Trees || tile.TileType == TileID.MushroomTrees)
            {
                return new TreeContext(pos);
            }
            if(tile.TileType == TileID.Cactus)
            {
                return new CactusContext(pos);
            }
            if(tile.TileType == TileID.PalmTree)
            {
                return new PalmTreeContext(pos);
            }
            if (TileID.Sets.TreeSapling[tile.TileType])
            {
                return new SaplingContext(pos);
            }
            return new TileContext(pos);
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
