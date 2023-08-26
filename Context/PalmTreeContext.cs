using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Twaila.Util;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.Config;

namespace Twaila.Context
{
    public class PalmTreeContext : TileContext
    {
        protected int SandId { get; set; }

        public PalmTreeContext(TwailaPoint pos) : base(pos)
        {
            SandId = GetPalmTreeSand();
        }

        public static PalmTreeContext CreatePalmTreeContext(TwailaPoint pos)
        {
            Tile tile = Framing.GetTileSafely(pos.BestPos());

            if (tile.TileType == TileID.PalmTree && !TileUtil.IsTileBlockedByAntiCheat(tile, pos.BestPos()))
            {
                return new PalmTreeContext(pos);
            }

            return null;
        }

        public override void Update()
        {
            base.Update();
            SandId = GetPalmTreeSand();
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(PalmTreeContext))
            {
                PalmTreeContext otherContext = (PalmTreeContext)other;
                return otherContext.SandId != SandId;
            }
            return true;
        }

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            if (TileLoader.CanGrowModPalmTree(SandId))
            {
                return new TwailaRender(TreeUtil.GetImageForModdedPalmTree(spriteBatch, SandId), 0.5f);
            }
            int palmTreeWood = TreeUtil.GetTreeWood(SandId);
            if (palmTreeWood != -1)
            {
                return new TwailaRender(TreeUtil.GetImageForPalmTree(spriteBatch, palmTreeWood), 0.5f);
            }
            return new TwailaRender();
        }

        protected override string GetName()
        {
            string displayName = NameUtil.GetNameForPalmTree(SandId);
            string internalName = PlantLoader.Get<ModPalmTree>(TileId, SandId)?.GetType().Name;
            string fullName = PlantLoader.Get<ModPalmTree>(TileId, SandId)?.GetType().FullName;

            TwailaConfig.NameType nameType = TwailaConfig.Instance.DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? base.GetName();
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(SandId);
            return NameUtil.GetMod(mTile);
        }

        private int GetPalmTreeSand()
        {
            int x = Pos.BestPos().X;
            int y = Pos.BestPos().Y;
            do
            {
                y += 1;
            } while (WorldGen.InWorld(x, y) && Framing.GetTileSafely(x, y).TileType == TileID.PalmTree && Framing.GetTileSafely(x, y).HasTile);

            if (!Framing.GetTileSafely(x, y).HasTile)
            {
                return -1;
            }
            return Framing.GetTileSafely(x, y).TileType;
        }
    }
}
