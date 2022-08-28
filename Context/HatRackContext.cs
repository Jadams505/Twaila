using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Twaila.Graphics;
using Twaila.UI;
using System.Reflection;
using Twaila.Util;

namespace Twaila.Context
{
	public class HatRackContext : TileContext
	{
		public const int MAX_ITEM_COUNT = 2;
		protected int[] ItemIds { get; set; }
		protected string[] ItemTexts { get; set; }

		public HatRackContext(Point pos) : base(pos)
		{
			ItemIds = new int[MAX_ITEM_COUNT];
			ItemTexts = new string[MAX_ITEM_COUNT];
		}

		public override bool ContextChanged(BaseContext other)
		{
			if (other?.GetType() == typeof(HatRackContext))
			{
				HatRackContext otherContext = (HatRackContext)other;
				for(int i = 0; i < otherContext.ItemIds.Length; ++i)
				{
					if (otherContext.ItemIds[i] != ItemIds[i])
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		public override void Update()
		{
			base.Update();

			PopulateItems();

			for(int i = 0; i < ItemIds.Length; ++i)
			{
				int id = ItemIds[i];
				if(id > 0)
				{
					Icons.IconImages.Insert(0, ImageUtil.GetRenderForIconItem(id));
				}
			}		
		}

		protected override TwailaRender TileImage(SpriteBatch spriteBatch)
		{
			Tile tile = Framing.GetTileSafely(Pos);
			if (ItemIds[0] > 0 || ItemIds[1] > 0)
			{
				return ImageUtil.GetRenderForHatRack(spriteBatch, tile, Pos.X, Pos.Y, ItemIds[0], ItemIds[1]);
			}
			return base.TileImage(spriteBatch);
		}

		protected override List<UITwailaElement> InfoElements()
		{
			return base.InfoElements();
		}

		private void PopulateItems()
		{
			Point targetPos = TileUtil.TileEntityCoordinates(Pos.X, Pos.Y, width: 3, height: 4);
			int id = TEHatRack.Find(targetPos.X, targetPos.Y);
			TEHatRack instance = (TEHatRack)TileEntity.ByID[id];
			Item[] items = (Item[])instance.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);

			for(int i = 0; i < items.Length; ++i)
			{
				ItemIds[i] = items[i].type;
			}
		}
	}
}
