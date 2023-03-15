using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Twaila.UI
{
	public class UIStatGrid : UITwailaElement
	{
		public List<UIStatElement> GridElements { get; set; }

		public int GridWidth { get; set; }

		public int GridHeight => (int)Math.Ceiling((double)GridElements.Count / GridWidth);

		public UIStatGrid(List<UIStatElement> elements, int width) : base()
		{
			GridElements = elements;
			GridWidth = width;
			float top = 0;
			float left = 0;
			float rowTop = 0;
			for (int i = 0; i < GridElements.Count; ++i)
			{
				UIStatElement curr = GridElements[i];
				Vector2 size = curr.GetContentSize();

				if(i % GridWidth == 0)
				{
					left = 0;
					top += rowTop;
				}

				curr.Left.Set(left, 0);
				curr.Top.Set(top, 0);
				left += size.X;
				rowTop = MathHelper.Max(rowTop, size.Y);
				Append(curr);
			}

			Width.Set(GetContentSize().X, 0);
			Height.Set(GetContentSize().Y, 0);
		}

		public override Vector2 GetContentSize()
		{
			float width = 0;
			float height = 0;
			float rowWidth = 0;
			float rowHeight = 0;
			for(int i = 0; i < GridElements.Count; ++i)
			{
				Vector2 size = GridElements[i].GetContentSize();

				if (i % GridWidth == 0)
				{
					height += rowHeight;
					rowHeight = 0;
					rowWidth = 0;
				}

				rowWidth += size.X;

				width = Math.Max(rowWidth, width);
				rowHeight = Math.Max(rowHeight, size.Y);
			}
			width = Math.Max(rowWidth, width);
			height += rowHeight;
			return new Vector2(width, height);
		}

		public override void Update(GameTime gameTime)
		{
			// set sizes of grid elements based on Width and Height of this element
			for(int i = 0; i < GridElements.Count; ++i)
			{

			}
		}

		public override void ApplyConfigSettings(TwailaConfig config)
		{
			base.ApplyConfigSettings(config);
			foreach(var element in GridElements)
			{
				element.ApplyConfigSettings(config);
			}
		}

		public override void ApplyHoverSettings(TwailaConfig config)
		{
			base.ApplyHoverSettings(config);
			foreach (var element in GridElements)
			{
				element.ApplyHoverSettings(config);
			}
		}

		protected override void DrawOverflow(SpriteBatch spriteBatch)
		{
			
		}

		protected override void DrawShrunk(SpriteBatch spriteBatch)
		{
			
		}

		protected override void DrawTrimmed(SpriteBatch spriteBatch)
		{
			
		}
	}
}
