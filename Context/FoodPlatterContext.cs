using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class FoodPlatterContext : TileContext
    {
        protected int FoodItemId { get; set; }
        protected string ItemText { get; set; }

        public FoodPlatterContext(TwailaPoint point) : base(point)
        {
            FoodItemId = 0;
            ItemText = "";
        }

		public static FoodPlatterContext CreateFoodPlatterContext(TwailaPoint pos)
		{
			Point tilePos = pos.BestPos();
			Tile tile = Framing.GetTileSafely(tilePos);
			if (TEFoodPlatter.Find(tilePos.X, tilePos.Y) != -1 && !TileUtil.IsTileBlockedByAntiCheat(tile, tilePos))
			{
				return new FoodPlatterContext(pos);
			}

			return null;
		}

		public override void Update()
        {
            base.Update();
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            FoodItemId = GetFoodItemId();

            if (FoodItemId > 0)
            {
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    Icons.IconImages.Insert(0, ImageUtil.GetRenderForIconItem(FoodItemId));
                }
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    ItemText = Lang.GetItemNameValue(FoodItemId);
                }
            }
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

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            if(FoodItemId != 0)
            {
                return ImageUtil.GetRenderForPlate(FoodItemId);
            }
            return base.TileImage(spriteBatch);
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = base.InfoElements();

            if (!string.IsNullOrEmpty(ItemText))
            {
                elements.Insert(0, new UITwailaText(ItemText));
            }
            return elements;
        }

        private int GetFoodItemId()
        {
            int id = TEFoodPlatter.Find(Pos.BestPos().X, Pos.BestPos().Y);
            Item item = ((TEFoodPlatter)TileEntity.ByID[id]).item;
            return item.type;
        }
    }
}
