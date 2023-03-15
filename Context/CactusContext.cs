using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Twaila.Util;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.Systems;

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
            Tile tile = Framing.GetTileSafely(pos.BestPos());

            if (tile.TileType == TileID.Cactus && !TileUtil.IsTileBlockedByAntiCheat(tile, pos.BestPos()))
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
            if(mTile != null)
            {
                return mTile.Mod.DisplayName;
            }
            return "Terraria";
        }

        private int GetCactusSand()
        {
            int x = Pos.BestPos().X, y = Pos.BestPos().Y;
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
