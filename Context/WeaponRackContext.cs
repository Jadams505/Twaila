using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Twaila.Graphics;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class WeaponRackContext : TileContext
    {
        protected int ItemId { get; set; }
        protected string ItemText { get; set; }

        public WeaponRackContext (Point point) : base(point)
        {
            ItemId = 0;
            ItemText = "";
        }

        public override void Update()
        {
            base.Update();
            TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

            ItemId = GetItemId();

            if (ItemId > 0)
            {
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    Icons.IconImages.Insert(0, ImageUtil.GetRenderForIconItem(ItemId));
                }
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    ItemText = Lang.GetItemNameValue(ItemId);
                }
            }
        }

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos);
            return ImageUtil.GetRenderForWeaponRack(spriteBatch, tile, Pos.X, Pos.Y, ItemId);
        }

        public override bool ContextChanged(BaseContext other)
        {
            if (other?.GetType() == typeof(WeaponRackContext))
            {
                WeaponRackContext otherContext = (WeaponRackContext)other;
                return otherContext.ItemId != ItemId;
            }
            return true;
        }

        protected override List<UITwailaElement> InfoElements()
        {
            List<UITwailaElement> elements = base.InfoElements();

            if (!string.IsNullOrEmpty(ItemText))
            {
                elements.Insert(0, new TwailaText(ItemText));
            }
            return elements;
        }

        private int GetItemId()
        {
            Point targetPos = TileUtil.TileEntityCoordinates(Pos.X, Pos.Y, width: 3, height: 3);
            int id = TEWeaponsRack.Find(targetPos.X, targetPos.Y);
            Item item = ((TEWeaponsRack)TileEntity.ByID[id]).item;
            return item.type;
        }
    }
}
