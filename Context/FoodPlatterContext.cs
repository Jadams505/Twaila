using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;
using Twaila.Graphics;
using Twaila.Util;

namespace Twaila.Context
{
    public class FoodPlatterContext : TileContext
    {
        protected int FoodItemId { get; private set; }

        public FoodPlatterContext(Point point) : base(point)
        {
            FoodItemId = 0;
        }

        public override void Update()
        {
            base.Update();
            FoodItemId = GetFoodItemId();
        }

        public override bool ContextChanged(BaseContext other)
        {
            if(other?.GetType() == typeof(FoodPlatterContext))
            {
                FoodPlatterContext otherContext = (FoodPlatterContext)other;
                return otherContext.FoodItemId != FoodItemId;
            }
            return true;
        }

        protected override TwailaTexture TileImage(SpriteBatch spriteBatch)
        {
            if(FoodItemId != 0)
            {
                return new TwailaTexture(ImageUtil.GetImageForPlate(spriteBatch, FoodItemId));
            }
            return base.TileImage(spriteBatch);
        }

        protected override string GetName()
        {
            if(FoodItemId != 0)
            {
                return $"{base.GetName()} [{NameUtil.GetNameFromItem(FoodItemId)}]";
            }
            return base.GetName();
        }

        private int GetFoodItemId()
        {
            int id = TEFoodPlatter.Find(Pos.X, Pos.Y);
            Item item = ((TEFoodPlatter)TileEntity.ByID[id]).item;
            return item.type;
        }
    }
}
