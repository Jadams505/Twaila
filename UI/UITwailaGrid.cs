using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Twaila.Config;
using Twaila.Util;

namespace Twaila.UI
{
    public class UITwailaGrid : UITwailaElement
    {
        /// <summary>
        /// Padding between the elements in each row
        /// </summary>
        public int RowPadding { get; set; } = 10;

        /// <summary>
        /// Padding between the elements in each column
        /// </summary>
        public int ColumnPadding { get; set; } = 2;

        public float MaxCellWidth { get; set; } = 1920;

        public float MaxCellHeight { get; set; } = 1080;

        public bool SmartRows { get; set; } = true;

        public List<UITwailaElement> GridElements { get; set; } = new List<UITwailaElement>();

        public int GridWidth { get; set; }

        public int GridHeight => (int)Math.Ceiling((float)GridElements.Count / GridWidth);

        public UITwailaGrid(List<UITwailaElement> elements, int width) : this(width)
        {
            SetGridElements(elements);

            for (int i = 0; i < GridElements.Count; ++i)
            {
                Append(GridElements[i]);
            }

            Width.Set(GetContentSize().X, 0);
            Height.Set(GetContentSize().Y, 0);
        }

        public UITwailaGrid(int width) : base()
        {
            GridWidth = width;
        }

        public void Add(UITwailaElement element)
        {
            GridElements.Add(element);
            Append(element);

            if (SmartRows)
                SetGridElements(GridElements);
        }

        public new void RemoveAllChildren()
        {
            GridElements.Clear();
            base.RemoveAllChildren();
        }

        private static readonly Comparison<UITwailaElement> _elementSorter = (a, b) => a.GetContentSize().X.CompareTo(b.GetContentSize().X);

        public void SetGridElements(List<UITwailaElement> elements)
        {
            if (!SmartRows)
            {
                GridElements = elements;
                return;
            }

            elements.Sort(_elementSorter);
            List<UITwailaElement> sorted = GridUtil.MapVertically(elements, GridWidth);

            GridElements = sorted;
        }

        public override Vector2 SizePriority()
        {
            float height = 0;
            float width = 0;

            for (int row = 0; row < GridHeight; ++row)
            {
                int rowStartIndex = row * GridWidth;
                int elementsInRow = Math.Min(rowStartIndex + GridWidth, GridElements.Count) - rowStartIndex;
                float biggestPriorityHeightInRow = 0;
                float widthPriority = 0;
                for (int col = 0, i = 0; col < elementsInRow && i < GridElements.Count; ++col)
                {
                    i = row * GridWidth + col;
                    Vector2 priority = GridElements[i].SizePriority();
                    biggestPriorityHeightInRow = Math.Max(biggestPriorityHeightInRow, priority.Y);
                    widthPriority += priority.X;
                }
                height += biggestPriorityHeightInRow;
                width = Math.Max(widthPriority, width);
            }
            return new Vector2(width, height);
        }

        public override Vector2 GetContentSize()
        {
            float width = 0;
            float height = 0;
            int paddingYCount = 0;
            for (int row = 0; row < GridHeight; ++row)
            {
                int rowStartIndex = row * GridWidth;
                int elementsInRow = Math.Min(rowStartIndex + GridWidth, GridElements.Count) - rowStartIndex;
                float biggestHeightInRow = 0;
                for (int col = 0, i = 0; col < elementsInRow && i < GridElements.Count; ++col)
                {
                    i = row * GridWidth + col;
                    Vector2 size = GridElements[i].GetContentSize();
                    biggestHeightInRow = Math.Clamp(Math.Max(biggestHeightInRow, size.Y), 0, MaxCellHeight);
                }
                height += biggestHeightInRow;

                if (biggestHeightInRow > 0)
                    paddingYCount++;
            }

            int paddingXCount = 0;
            for(int col = 0; col < GridWidth; ++col)
            {
                int elementsInCol = GridWidth - (GridWidth * GridHeight - GridElements.Count) > col ? GridHeight : GridHeight - 1;
                float biggestWidthInCol = 0;
                for(int row = 0, i = 0; row < elementsInCol && i < GridElements.Count; ++row)
                {
                    i = row * GridWidth + col;
                    Vector2 size = GridElements[i].GetContentSize();
                    biggestWidthInCol = Math.Clamp(Math.Max(biggestWidthInCol, size.X), 0, MaxCellHeight);
                }
                width += biggestWidthInCol;

                if (biggestWidthInCol > 0)
                    paddingXCount++;
            }
            return new Vector2(width + Math.Max(paddingXCount - 1, 0) * RowPadding, height + Math.Max(paddingXCount - 1, 0) * ColumnPadding);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 scale = DrawMode == DrawMode.Shrink ? GetDrawScaleVector() : Vector2.One;

            float height = 0;
            float width = 0;
            float paddingY = 0;
            float paddingX = 0;
            for (int row = 0; row < GridHeight; ++row)
            {
                int rowStartIndex = row * GridWidth;
                int elementsInRow = Math.Min(rowStartIndex + GridWidth, GridElements.Count) - rowStartIndex;
                float biggestHeightInRow = 0;
                for (int col = 0, i = 0; col < elementsInRow && i < GridElements.Count; ++col)
                {
                    i = row * GridWidth + col;
                    Vector2 size = GridElements[i].GetContentSize();
                    biggestHeightInRow = Math.Clamp(Math.Max(biggestHeightInRow, size.Y), 0, MaxCellHeight);
                }

                biggestHeightInRow *= scale.Y;

                for (int col = 0, i = 0; col < elementsInRow && i < GridElements.Count; ++col)
                {
                    i = row * GridWidth + col;
                    UITwailaElement element = GridElements[i];
                    element.Top.Set(height + paddingY, 0);
                    element.Height.Set(biggestHeightInRow, 0);
                    if (DrawMode == DrawMode.Trim && height + paddingY + biggestHeightInRow > Height.Pixels)
                    {
                        element.Height.Set(Math.Max(Height.Pixels - height - paddingY, 0), 0);
                    }
                }
                height += biggestHeightInRow;

                if (biggestHeightInRow != 0)
                    paddingY += ColumnPadding * scale.X;
            }

            for (int col = 0; col < GridWidth; ++col)
            {
                int elementsInCol = GridWidth - (GridWidth * GridHeight - GridElements.Count) > col ? GridHeight : GridHeight - 1;
                float biggestWidthInCol = 0;
                for (int row = 0, i = 0; row < elementsInCol && i < GridElements.Count; ++row)
                {
                    i = row * GridWidth + col;
                    Vector2 size = GridElements[i].GetContentSize();
                    biggestWidthInCol = Math.Clamp(Math.Max(biggestWidthInCol, size.X), 0, MaxCellHeight);
                }

                biggestWidthInCol *= scale.X;

                for (int row = 0, i = 0; row < elementsInCol && i < GridElements.Count; ++row)
                {
                    i = row * GridWidth + col;
                    UITwailaElement element = GridElements[i];
                    element.Left.Set(width + paddingX, 0);
                    element.Width.Set(biggestWidthInCol, 0);
                    if (DrawMode == DrawMode.Trim && width + paddingX + biggestWidthInCol > Width.Pixels)
                    {
                        element.Width.Set(Math.Max(0, Width.Pixels - width - paddingX), 0);
                    }
                }
                width += biggestWidthInCol;

                if (biggestWidthInCol != 0)
                    paddingX += RowPadding * scale.X;
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
