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
            SandTileId = GetCactusSand();
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

        protected override TwailaTexture TileImage(SpriteBatch spriteBatch)
        {
            if (TileLoader.CanGrowModCactus(SandTileId))
            {
                return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, SandTileId, true));
            }
            return new TwailaTexture(TreeUtil.GetImageForCactus(spriteBatch, SandTileId, false));
        }

        protected override TwailaTexture ItemImage(SpriteBatch spriteBatch)
        {
            return null;
        }

        protected override string GetName()
        {
            string displayName = NameUtil.GetNameForCactus(TileId, SandTileId);
            string internalName = PlantLoader.Get<ModCactus>(TileId, SandTileId)?.GetType().Name;
            string fullName = PlantLoader.Get<ModCactus>(TileId, SandTileId)?.GetType().FullName;

            TwailaConfig.NameType nameType = TwailaConfig.Get().DisplayContent.ShowName;

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
