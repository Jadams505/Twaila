using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.Config;
using Microsoft.Xna.Framework;

namespace Twaila.Context
{
    public class CactusContext : TileContext
    {
        protected int SandTileId { get; set; }

        public CactusContext(TwailaPoint pos) : base(pos)
        {
            SandTileId = GetCactusSand();
        }

        public static CactusContext CreateCactusContext(TwailaPoint pos)
        {
            Point tilePos = pos.BestTilePos();
            Tile tile = Framing.GetTileSafely(tilePos);

            if (!tile.HasTile || tile.TileType >= TileLoader.TileCount)
                return null;

            if (!TileUtil.IsTilePosInBounds(tilePos))
                return null;

            if (tile.TileType == TileID.Cactus && !TileUtil.IsTileBlockedByAntiCheat(tile, tilePos))
            {
                return new CactusContext(pos);
            }

            return null;
        }

        public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(CactusContext))
            {
                CactusContext otherContext = (CactusContext)other;
                return otherContext.SandTileId != SandTileId;
            }
            return true;
        }

        public override void Update()
        {
            base.Update();
            SandTileId = GetCactusSand();
        }

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            if (TileLoader.CanGrowModCactus(SandTileId))
            {
                return TreeUtil.GetImageForCactus(spriteBatch, SandTileId, true).ToRender();
            }
            return TreeUtil.GetImageForCactus(spriteBatch, SandTileId, false).ToRender();
        }

        protected override TwailaRender ItemImage(SpriteBatch spriteBatch)
        {
            return new TwailaRender();
        }

        protected override string GetName()
        {
            string displayName = NameUtil.GetNameForCactus(SandTileId);
            string internalName = PlantLoader.Get<ModCactus>(TileId, SandTileId)?.GetType().Name;
            string fullName = PlantLoader.Get<ModCactus>(TileId, SandTileId)?.GetType().FullName;

            TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? base.GetName();
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(SandTileId);
            return NameUtil.GetMod(mTile);
        }

        private int GetCactusSand()
        {
            Point bestPos = BestTilePos;
            int x = bestPos.X, y = bestPos.Y;
            do
            {
                if (Framing.GetTileSafely(x, y + 1).TileType == TileID.Cactus)
                {
                    y++;
                }
                else if (Framing.GetTileSafely(x + 1, y).TileType == TileID.Cactus)
                {
                    x++;
                }
                else if (Framing.GetTileSafely(x - 1, y).TileType == TileID.Cactus)
                {
                    x--;
                }
                else
                {
                    y++;
                }
            }
            while (WorldGen.InWorld(x, y) && Framing.GetTileSafely(x, y).TileType == TileID.Cactus && Framing.GetTileSafely(x, y).HasTile);

            if (!Framing.GetTileSafely(x, y).HasTile)
            {
                return -1;
            }
            return Framing.GetTileSafely(x, y).TileType;
        }
    }
}
