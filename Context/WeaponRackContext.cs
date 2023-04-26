using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
    public class WeaponRackContext : TileContext
    {
        protected int ItemId { get; set; }
        protected string ItemText { get; set; }

        public WeaponRackContext (TwailaPoint point) : base(point)
        {
            ItemId = 0;
            ItemText = "";
        }

        public static WeaponRackContext CreateWeaponRackContext(TwailaPoint pos)
        {
            Point bestPos = pos.BestPos();
            Tile tile = Framing.GetTileSafely(bestPos);
            if (tile.TileType == TileID.WeaponsRack2 || tile.TileType == TileID.WeaponsRack)
            {
                Point targetPos = TileUtil.TileEntityCoordinates(bestPos.X, bestPos.Y, width: 3, height: 3);
                if (TEWeaponsRack.Find(targetPos.X, targetPos.Y) != -1 && !TileUtil.IsTileBlockedByAntiCheat(tile, bestPos))
                {
                    return new WeaponRackContext(pos);
                }
            }
            return null;
        }

        public override void Update()
        {
            base.Update();
            TwailaConfig.Content content = TwailaConfig.Instance.DisplayContent;

            ItemId = GetItemId();

            if (ItemId > 0)
            {
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    IconGrid.AddIcon(ImageUtil.GetRenderForIconItem(ItemId));
                }
                if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
                {
                    ItemText = Lang.GetItemNameValue(ItemId);
                    TextGrid.Add(new UITwailaText(ItemText));
                }
            }
        }

        protected override TwailaRender TileImage(SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(Pos.BestPos());
            return ImageUtil.GetRenderForWeaponRack(spriteBatch, tile, Pos.BestPos().X, Pos.BestPos().Y, ItemId);
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

        private int GetItemId()
        {
            Point targetPos = TileUtil.TileEntityCoordinates(Pos.BestPos().X, Pos.BestPos().Y, width: 3, height: 3);
            int id = TEWeaponsRack.Find(targetPos.X, targetPos.Y);
            Item item = ((TEWeaponsRack)TileEntity.ByID[id]).item;
            return item.type;
        }
    }
}
