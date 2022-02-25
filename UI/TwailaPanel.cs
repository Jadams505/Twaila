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

        public TwailaPanel()
        {
            _pos = Point.Zero;
            _tile = new Tile();
            _itemId = -1;
            Name = new TwailaText("Default Name");
            
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
            int oldStyle = TileObjectData.GetTileStyle(_tile), newStyle = TileObjectData.GetTileStyle(tile);
            if (tile.active() && (IsTileException(tile) || _tile.type != tile.type) || (oldStyle != -1 && newStyle != -1 && oldStyle != newStyle))
            {
                _itemId = GetItemId(tile);
                NameUtil.UpdateName(this, tile, _pos, itemId: _itemId, name: "Default Name");
                NameUtil.UpdateModName(this, tile);
                Image.SetImage(spriteBatch, tile, _itemId, _pos);
                _tile.CopyFrom(tile);
            }
        }

        public void UpdatePos(Point pos)
        {
            _pos = pos;
        }

        private static int GetItemId(Tile tile)
        {
            ModTile mTile = TileLoader.GetTile(tile.type);
            int style = TileObjectData.GetTileStyle(tile);
            if (mTile == null)
            {
                Item item = new Item();
                for (int i = 0; i < ItemID.Count; ++i)
                {
                    item.SetDefaults(i);
                    if (item.createTile == tile.type && (style == -1 || item.placeStyle == style))
                    {
                        return i;
                    }
                }
                return -1;
            }
            bool multiTile = TileObjectData.GetTileData(tile) != null;
            if (mTile.drop == 0 && multiTile)
            {
                for (int i = ItemID.Count; i < ItemLoader.ItemCount; ++i)
                {
                    ModItem mItem = ItemLoader.GetItem(i);
                    if (mItem != null && mItem.item.createTile == tile.type && (style == -1 || mItem.item.placeStyle == style))
                    {
                        return i;
                    }
                }
            }
            return mTile.drop == 0 ? -1 : mTile.drop;
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
            return  tile.type == TileID.Trees || tile.type == TileID.PalmTree || tile.type == TileID.Cactus;
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
