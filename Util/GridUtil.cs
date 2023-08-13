using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twaila.Util
{
    public static class GridUtil
    {
        // 0 1 2 3 4 5 6 7 8 9   width = 3

        // 0 4 7 = 11   Total: 3 + 6 + 9 = 18
        // 1 5 8 = 14
        // 2 6 9 = 17
        // 3 _ _ = 3
        public static List<T> MapVertically<T>(List<T> elements, int width)
        {
            T[] sorted = new T[elements.Count];
            int gridHeight = (int)Math.Ceiling((float)elements.Count / width);
            for (int col = 0, i = 0; col < width; ++col)
            {
                for (int row = 0; row < gridHeight; ++row)
                {
                    int index = row * width + col;
                    if (index >= 0 && index < elements.Count && i < elements.Count)
                        sorted[index] = elements[i++];
                }
            }
            return new List<T>(sorted);
        }

        // 0 1 2 3 4 5 6 7 8 9   width = 3

        // 9 5 2 = 16   Total: 9 + 5 + 2 = 14
        // 8 4 1 = 13
        // 7 3 0 = 10
        // 6 _ _ = 6
        public static List<T> MapVerticallyReverse<T>(List<T> elements, int width)
        {
            T[] sorted = new T[elements.Count];
            int gridHeight = (int)Math.Ceiling((float)elements.Count / width);
            for (int col = 0, i = elements.Count - 1; col < width; ++col)
            {
                for (int row = 0; row < gridHeight; ++row)
                {
                    int index = row * width + col;
                    if (index >= 0 && index < elements.Count && i < elements.Count && i >= 0)
                        sorted[index] = elements[i--];
                }
            }
            return new List<T>(sorted);
        }

        // 0 1 2 3 4 5 6 7 8 9   width = 3

        // 0 4 9 = 13   Total: 9 + 5 + 8 = 22
        // 1 5 8 = 14
        // 2 6 7 = 15
        // 3 _ _ = 3
        // Uses MapVertically for the left half and MapVerticallyReverse for the right half
        public static List<T> MapVerticalHybrid<T>(List<T> elements, int width)
        {
            if (elements.Count == 0 || width <= 0)
                return elements;

            width = Math.Clamp(width, 1, elements.Count);
            int height = (int)Math.Ceiling((float)elements.Count / width);

            if (height == 1)
                return elements;

            int leftWidth = (int)Math.Ceiling(width / 2f);
            int lastRowWidth = elements.Count % width == 0 ? width : elements.Count % width;
            int leftSize = leftWidth * (height - 1) + Math.Clamp(lastRowWidth, 0, leftWidth);
            var left = MapVertically(elements.GetRange(0, leftSize), leftWidth);

            if (leftSize < elements.Count)
            {
                var right = MapVerticallyReverse(elements.GetRange(leftSize, elements.Count - leftSize), width - leftWidth);

                return AddColumns(left, right, leftWidth);
            }

            return left;
        }

        
        // 0 1     1     0 1 1     
        // 3 4  +  2  =  3 4 2
        // 6 7     3     6 7 3
        public static List<T> AddColumns<T>(List<T> oldList, List<T> columns, int oldWidth)
        {
            int gridHeight = (int)Math.Ceiling((float)oldList.Count / oldWidth);
            int newWidth = (int)Math.Ceiling((float)columns.Count / gridHeight);

            List<T> toReturn = new List<T>(oldList.Count + columns.Count);

            for (int row = 0; row < gridHeight; ++row)
            {
                for (int col = 0; col < oldWidth; ++col)
                {
                    int oldIndex = col + row * oldWidth;
                    if (oldIndex < oldList.Count)
                        toReturn.Add(oldList[oldIndex]);
                }

                for (int col = 0; col < newWidth; ++col)
                {
                    int newIndex = col + row * newWidth;
                    if (newIndex < columns.Count)
                        toReturn.Add(columns[newIndex]);
                }
            }

            return toReturn;
        }
    }
}
