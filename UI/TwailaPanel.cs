using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;
using Twaila.ObjectData;
using Twaila.Util;

namespace Twaila.UI
{
    public class TwailaPanel : UIPanel
    {
        public TwailaText Name, Mod;
        public UITwailaImage Image;
        private Point _pos;
        private Tile _tile;
        private int _itemId;
        private bool forceUpdate;
        private bool debugMode;

        public TwailaPanel()
        {
            _pos = Point.Zero;
            _tile = new Tile();
            _itemId = -1;
            forceUpdate = false;
            debugMode = false;
            Name = new TwailaText("Default Name", Main.fontCombatText[0], Color.White, 1f);
            
            Image = new UITwailaImage();
            Image.VAlign = 0.5f;
            Image.MarginRight = 10;
            Mod = new TwailaText("Terraria", Main.fontItemStack, Color.White, 1f);
            
            Width.Set(0, 0);
            Height.Set(0, 0);
            HAlign = 0.5f;
            Top.Set(0, 0);

            Append(Name);
            Append(Mod);
            Append(Image);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float height = GetHeight(Image) > GetHeight(Mod) + GetHeight(Name) ? GetHeight(Image) : GetHeight(Mod) + GetHeight(Name);
            Height.Set(height + PaddingLeft + PaddingRight, 0);
            float width = GetWidth(Name) > GetWidth(Mod) ? GetWidth(Name) : GetWidth(Mod);
            Width.Set(width + GetWidth(Image) + Image.MarginRight + PaddingLeft + PaddingRight, 0);

            Name.Top.Set(Top.Pixels, 0);
            Name.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
            Mod.Top.Set(Name.Top.Pixels + GetHeight(Name), 0);
            Mod.Left.Set(Image.Width.Pixels + Image.MarginRight, 0);
            Recalculate();
        }
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
            Tile tile = GetTileCopy(_pos.X, _pos.Y);
            
            if (forceUpdate || (tile.active() && (IsTileException(tile) || IsDifferentTile(tile) || IsDifferentStyle(tile))))
            {
                _itemId = ItemUtil.GetItemId(tile);
                Name.SetText(NameUtil.GetNameForTile(tile, _pos, itemId: _itemId));
                Mod.SetText(NameUtil.GetModName(tile));
                Image.SetImage(spriteBatch, tile, _itemId, _pos, debugMode);
                _tile.CopyFrom(tile);
            }
            forceUpdate = false;
        }

        private bool IsDifferentStyle(Tile tile)
        {
            TileObjectData oldData = ExtraObjectData.GetData(_tile.type), newData = ExtraObjectData.GetData(tile.type);
            if(newData == null)
            {
                oldData = TileObjectData.GetTileData(_tile);
                newData = TileObjectData.GetTileData(tile);
            }
            if(newData == null || oldData == null)
            {
                return false;
            }
            int oldRow = _tile.frameX / oldData.CoordinateFullWidth;
            int oldCol = _tile.frameY / oldData.CoordinateFullHeight;
            int newRow = tile.frameX / newData.CoordinateFullWidth;
            int newCol = tile.frameY / newData.CoordinateFullHeight;

            return oldRow != newRow || oldCol != newCol;
        }

        private bool IsDifferentTile(Tile tile)
        {
            return _tile.type != tile.type;
        }

        public void UpdatePos(Point pos)
        {
            _pos = pos;
        }

        public void ForceUpdate()
        {
            forceUpdate = true;
        }

        public void ToggleDebugMode()
        {
            debugMode ^= true;
        }

        private static Tile GetTileCopy(int x, int y)
        {
            Tile copy = new Tile();
            copy.CopyFrom(Framing.GetTileSafely(x, y));
            return copy;
        }

        // Any tile that has many textures for the same tileid without using TileObjectData
        private static bool IsTileException(Tile tile)
        {
            
            switch (tile.type)
            {
                case TileID.Trees:
                case TileID.PalmTree:
                case TileID.Cactus:
                    return true;
            }
            
            return false;
        }

        private static float GetWidth(UIElement element)
        {
            return element.Width.Pixels;
        }

        private static float GetHeight(UIElement element)
        {
            return element.Height.Pixels;
        }
    }
}
