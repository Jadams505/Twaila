using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Twaila.Config;
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
            Content content = TwailaConfig.Instance.DisplayContent;

            FoodItemId = GetFoodItemId();

            if (FoodItemId > 0)
            {
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    IconGrid.AddIcon(ImageUtil.GetRenderForIconItem(FoodItemId));
                }
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    ItemText = Lang.GetItemNameValue(FoodItemId);
                    TextGrid.Add(new UITwailaText(ItemText));
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

        private int GetFoodItemId()
        {
            int id = TEFoodPlatter.Find(Pos.BestPos().X, Pos.BestPos().Y);
            Item item = ((TEFoodPlatter)TileEntity.ByID[id]).item;
            return item.type;
        }
    }
}
