using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Twaila.Util;
using Twaila.Graphics;

namespace Twaila.Context
{
    public class PalmTreeContext : TileContext
    {
        protected int SandId { get; set; }

        public PalmTreeContext(Point pos) : base(pos)
        {
            SandId = GetPalmTreeSand();
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
            return null;
        }

        protected override string GetName()
        {
            string displayName = NameUtil.GetNameForPalmTree(TileId, SandId);
            string internalName = PlantLoader.Get<ModPalmTree>(TileId, SandId)?.GetType().Name;
            string fullName = PlantLoader.Get<ModPalmTree>(TileId, SandId)?.GetType().FullName;

            TwailaConfig.NameType nameType = TwailaConfig.Get().DisplayContent.ShowName;

            return NameUtil.GetName(nameType, displayName, internalName, fullName) ?? base.GetName();
        }

        protected override string GetMod()
        {
            ModTile mTile = TileLoader.GetTile(SandId);
            if (mTile != null)
            {
                return mTile.Mod.DisplayName;
            }
            return "Terraria";
        }

        private int GetPalmTreeSand()
        {
            int y = Pos.Y;
            do
            {
                y += 1;
            } while (Main.tile[Pos.X, y].TileType == TileID.PalmTree && Main.tile[Pos.X, y].HasTile);

            if (!Main.tile[Pos.X, y].HasTile)
            {
                return -1;
            }
            return Main.tile[Pos.X, y].TileType;
        }
    }
}
