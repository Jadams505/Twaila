using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Twaila.UI
{
	public class UITwailaGrid : UITwailaElement
	{
		public int RowPadding { get; set; } = 10;

		public List<UITwailaElement> GridElements { get; set; }

		public int GridWidth { get; set; }

		public int GridHeight => (int)Math.Ceiling((float)GridElements.Count / GridWidth);

		public UITwailaGrid(List<UITwailaElement> elements, int width) : base()
		{
			GridElements = elements;
			GridWidth = width;

			for (int i = 0; i < GridElements.Count; ++i)
			{
				Append(GridElements[i]);
			}

			Width.Set(GetContentSize().X, 0);
			Height.Set(GetContentSize().Y, 0);
		}

		public override Vector2 GetContentSize()
		{
			float width = 0;
			float height = 0;

			for (int row = 0; row < GridHeight; ++row)
			{
                int rowStartIndex = row * GridWidth;
                int elementsInRow = Math.Min(rowStartIndex + GridWidth, GridElements.Count) - rowStartIndex;
				float biggestWidthInRow = 0;
				float biggestHeightInRow = 0;
                for (int col = 0, i = 0; col < elementsInRow && i < GridElements.Count; ++col)
				{
					i = row * GridWidth + col;
					Vector2 size = GridElements[i].GetContentSize();
					biggestWidthInRow = Math.Max(biggestWidthInRow, size.X);
					biggestHeightInRow = Math.Max(biggestHeightInRow, size.Y);
				}
				width = Math.Max(width, biggestWidthInRow * elementsInRow + RowPadding * Math.Max(elementsInRow - 1, 0));
				height = Math.Max(height, biggestHeightInRow * GridHeight);
			}
            return new Vector2(width, height);
		}

		public override void Update(GameTime gameTime)
		{
			Vector2 size = GetContentSize();
			for(int i = 0; i < GridElements.Count; ++i)
			{
				int row = i / GridWidth;
				int col = i % GridWidth;

				int rowStartIndex = row * GridWidth;
                int elementsInRow = Math.Min(rowStartIndex + GridWidth, GridElements.Count) - rowStartIndex;
				int numberOfRows = GridHeight;

				float div = GridWidth;

				if(GridElements.Count < div)
					div = GridElements.Count;

                float width = (Width.Pixels - RowPadding * Math.Max(elementsInRow - 1, 0)) / div;
                float height = Height.Pixels / numberOfRows;

                if (DrawMode == DrawMode.Overflow)
				{
					width = size.X / elementsInRow;
					height = size.Y / numberOfRows;
				}

                UITwailaElement element = GridElements[i];
                element.Left.Set(col * (width + RowPadding), 0);
				element.Top.Set(row * height, 0);
				element.Width.Set(width, 0);
				element.Height.Set(height, 0);
			}

            base.Update(gameTime);
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
	}
}
