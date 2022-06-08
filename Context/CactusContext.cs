using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.UI;
using Terraria.Map;

namespace Twaila.Context
{
    public class CactusContext : TileContext
    {
        protected int SandTileId { get; set; }

        public CactusContext(Point pos) : base(pos)
        {
        }

        public override bool Applies()
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return tile.TileType == TileID.Cactus;
        }

        public override void UpdateOnChange(BaseContext prevContext, Layout layout)
        {
            Tile tile = Framing.GetTileSafely(Pos);

            TileId = tile.TileType;
            FrameX = tile.TileFrameX;
            FrameY = tile.TileFrameY;
            SandTileId = GetCactusSand();

            layout.Name.SetText(GetName());

            if (!(prevContext is CactusContext otherContext && otherContext.SandTileId == SandTileId))
            {
                layout.Image.SetImage(GetImage(Main.spriteBatch));
            }

            TwailaText id = new TwailaText("Id: " + tile.TileType);
            layout.InfoBox.AddAndEnable(id);

            layout.Mod.SetText(GetMod());
        }


        private TwailaTexture GetImage(SpriteBatch spriteBatch)
        {
            if (TileLoader.CanGrowModCactus(SandTileId))
            {
                return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, SandTileId, true));
            }
            return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, SandTileId, false));
        }

        private string GetName()
        {
            string cactus = Lang.GetMapObjectName(MapHelper.TileToLookup(TileID.Cactus, 0));
            if (SandTileId == -1)
            {
                return null;
            }
            if (TileLoader.CanGrowModCactus(SandTileId))
            {
                ModTile mTile = TileLoader.GetTile(SandTileId);
                if (mTile != null)
                {
                    int dropId = mTile.ItemDrop;
                    ModItem mItem = ItemLoader.GetItem(dropId);
                    return mItem == null ? mTile.Name : mItem.DisplayName.GetDefault() + " " + cactus;
                }
            }
            else
            {
                int itemId = -1;
                switch (SandTileId)
                {
                    case TileID.Crimsand:
                        itemId = ItemID.CrimsandBlock;
                        break;
                    case TileID.Ebonsand:
                        itemId = ItemID.EbonsandBlock;
                        break;
                    case TileID.Pearlsand:
                        itemId = ItemID.PearlsandBlock;
                        break;
                }
                if (itemId != -1)
                {
                    return Lang.GetItemNameValue(itemId) + " " + cactus;
                }
            }
            return cactus;
        }

        private string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(SandTileId);
            if(mTile != null)
            {
                return mTile.Mod.DisplayName;
            }
            return "Terraria";
        }

        private int GetCactusSand()
        {
            int x = Pos.X, y = Pos.Y;
            do
            {
                if (Main.tile[x, y + 1].TileType == TileID.Cactus)
                {
                    y++;
                }
                else if (Main.tile[x + 1, y].TileType == TileID.Cactus)
                {
                    x++;
                }
                else if (Main.tile[x - 1, y].TileType == TileID.Cactus)
                {
                    x--;
                }
                else
                {
                    y++;
                }
            }
            while (Main.tile[x, y].TileType == TileID.Cactus && Main.tile[x, y].HasTile);
            if (!Main.tile[x, y].HasTile)
            {
                return -1;
            }
            return Main.tile[x, y].TileType;
        }
    }
}
