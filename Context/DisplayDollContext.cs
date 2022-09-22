using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Twaila.UI;
using Twaila.Util;

namespace Twaila.Context
{
	public class DisplayDollContext : TileContext
	{
		public const int MAX_ITEM_COUNT = 8;
		protected int[] ItemIds { get; set; }
		protected string[] ItemTexts { get; set; }

		protected TwailaIconLine DisplayContentIcons { get; set; }

		public DisplayDollContext(Point point) : base(point)
		{
			ItemIds = new int[MAX_ITEM_COUNT];
			ItemTexts = new string[MAX_ITEM_COUNT];
			DisplayContentIcons = new TwailaIconLine();
		}

		public override bool ContextChanged(BaseContext other)
		{
			if (other?.GetType() == typeof(DisplayDollContext))
			{
				DisplayDollContext otherContext = (DisplayDollContext)other;
				for (int i = 0; i < otherContext.ItemIds.Length; ++i)
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
			TwailaConfig.Content content = TwailaConfig.Get().DisplayContent;

			PopulateItems();

			for (int i = 0; i < ItemIds.Length; ++i)
			{
				int id = ItemIds[i];
				if (id > 0)
				{
					if (content.ShowContainedItems == TwailaConfig.DisplayType.Icon || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
					{
						if(Icons.IconImages.Count != 0 && Icons.IconImages.Count < 6)
						{
							Icons.IconImages.Add(ImageUtil.GetRenderForIconItem(id));
						}
						else
						{
							DisplayContentIcons.IconImages.Insert(0, ImageUtil.GetRenderForIconItem(id));
						}
					}
					if (content.ShowContainedItems == TwailaConfig.DisplayType.Name || content.ShowContainedItems == TwailaConfig.DisplayType.Both)
					{
						ItemTexts[i] = NameUtil.GetNameFromItem(id);
					}
				}
			}
		}

		protected override List<UITwailaElement> InfoElements()
		{
			List<UITwailaElement> elements = base.InfoElements();

			foreach (string name in ItemTexts)
			{
				if (!string.IsNullOrEmpty(name))
				{
					elements.Insert(0, new TwailaText(name));
				}
			}
			if (DisplayContentIcons.IconImages.Count > 0)
			{
				elements.Add(DisplayContentIcons);
			}
			return elements;
		}

		private void PopulateItems()
		{
			Point targetPos = TileUtil.TileEntityCoordinates(Pos.X, Pos.Y, width: 2, height: 3);
			int id = TEDisplayDoll.Find(targetPos.X, targetPos.Y);
			TEDisplayDoll instance = (TEDisplayDoll)TileEntity.ByID[id];
			Item[] items = (Item[])instance.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);

			for (int i = 0; i < items.Length; ++i)
			{
				ItemIds[i] = items[i].type;
			}
		}
	}
}
