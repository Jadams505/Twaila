using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Twaila.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Systems;
using Microsoft.Xna.Framework;
using Twaila.Config;

namespace Twaila.Context
{
    public class SaplingContext : TileContext
    {
        public int DirtId { get; private set; }

        public SaplingContext(TwailaPoint pos) : base(pos)
        {
            DirtId = GetSaplingTile();
        }

        public static SaplingContext CreateSaplingContext(TwailaPoint pos)
        {
            Tile tile = Framing.GetTileSafely(pos.BestPos());

            if (TileID.Sets.TreeSapling[tile.TileType] && !TileUtil.IsTileBlockedByAntiCheat(tile, pos.BestPos()))
            {
                return new SaplingContext(pos);
            }

            return null;
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(SaplingContext))
            {
                SaplingContext otherContext = (SaplingContext)other;
                return otherContext.DirtId != DirtId || StyleChanged(otherContext);
            }
            return true;
        }

        public override void Update()
        {
            base.Update();
            DirtId = GetSaplingTile();
        }

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            return ImageUtil.GetImageFromTileDrawing(spriteBatch, tile, Pos.BestPos().X, Pos.BestPos().Y).ToRender();
        }

        protected override TwailaRender ItemImage(SpriteBatch spriteBatch)
        {
            int itemId = ItemTilePairSystem.GetItemId(Framing.GetTileSafely(Pos.BestPos()), TileType.Tile);
            Texture2D texture = ImageUtil.GetItemTexture(itemId);
            return texture.ToRender();
        }

        protected override string GetName()
        {
            string displayName = NameUtil.GetNameForSapling(TileId, DirtId);
            string internalName = NameUtil.GetInternalTileName(TileId, false);
            string fullName = NameUtil.GetInternalTileName(TileId, true);

            TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? base.GetName();
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(TileId);
            return NameUtil.GetMod(mTile);
        }

        private int GetSaplingTile()
        {
            Point bestPos = Pos.BestPos();
            int y = bestPos.Y;
            do
            {
                y++;
            } while (TileID.Sets.TreeSapling[Main.tile[bestPos.X, y].TileType] && Main.tile[bestPos.X, y].HasTile);

            if (!Main.tile[bestPos.X, y].HasTile)
            {
                return -1;
            }
            return Main.tile[bestPos.X, y].TileType;
        }
    }
}
