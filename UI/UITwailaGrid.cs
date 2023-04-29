using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Twaila.Util;

namespace Twaila.UI
{
    public class UITwailaGrid : UITwailaElement
    {
        public int RowPadding { get; set; } = 10;

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

        public override Vector2 GetContentSize()
        {
            float width = 0;
            float height = 0;

            for(int row = 0; row < GridHeight; ++row)
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
            }

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
            }
            return new Vector2(width + Math.Max(GridWidth - 1, 0) * RowPadding, height + Math.Max(GridHeight - 1, 0) * ColumnPadding);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 scale = DrawMode == DrawMode.Shrink ? GetDrawScaleVector() : Vector2.One;

            float height = 0;
            float width = 0;
            for (int row = 0; row < GridHeight; ++row)
            {
                int rowStartIndex = row * GridWidth;
                int elementsInRow = Math.Min(rowStartIndex + GridWidth, GridElements.Count) - rowStartIndex;
                float biggestHeightInRow = 0;
                float padding = row * ColumnPadding;
                for (int col = 0, i = 0; col < elementsInRow && i < GridElements.Count; ++col)
                {
                    i = row * GridWidth + col;
                    Vector2 size = GridElements[i].GetContentSize();
                    biggestHeightInRow = Math.Clamp(Math.Max(biggestHeightInRow, size.Y), 0, MaxCellHeight);
                }

                biggestHeightInRow *= scale.Y;
                padding *= scale.Y;

                for (int col = 0, i = 0; col < elementsInRow && i < GridElements.Count; ++col)
                {
                    i = row * GridWidth + col;
                    UITwailaElement element = GridElements[i];
                    element.Top.Set(height + padding, 0);
                    element.Height.Set(biggestHeightInRow, 0);
                    if (DrawMode == DrawMode.Trim && height + padding + biggestHeightInRow > Height.Pixels)
                    {
                        element.Height.Set(Math.Max(Height.Pixels - height - padding, 0), 0);
                    }
                }
                height += biggestHeightInRow;
            }

            for (int col = 0; col < GridWidth; ++col)
            {
                int elementsInCol = GridWidth - (GridWidth * GridHeight - GridElements.Count) > col ? GridHeight : GridHeight - 1;
                float biggestWidthInCol = 0;
                float padding = col * RowPadding;
                for (int row = 0, i = 0; row < elementsInCol && i < GridElements.Count; ++row)
                {
                    i = row * GridWidth + col;
                    Vector2 size = GridElements[i].GetContentSize();
                    biggestWidthInCol = Math.Clamp(Math.Max(biggestWidthInCol, size.X), 0, MaxCellHeight);
                }

                biggestWidthInCol *= scale.X;
                padding *= scale.X;

                for (int row = 0, i = 0; row < elementsInCol && i < GridElements.Count; ++row)
                {
                    i = row * GridWidth + col;
                    UITwailaElement element = GridElements[i];
                    element.Left.Set(width + padding, 0);
                    element.Width.Set(biggestWidthInCol, 0);
                    if (DrawMode == DrawMode.Trim && width + padding + biggestWidthInCol > Width.Pixels)
                    {
                        element.Width.Set(Math.Max(0, Width.Pixels - width - padding), 0);
                    }
                }
                width += biggestWidthInCol;
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
