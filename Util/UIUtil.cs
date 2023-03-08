using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Twaila.UI;

namespace Twaila.Util
{
    public static class UIUtil
    {
        public static Vector2 GetSizeIfAppended(this UIElement element)
        {
            if (element.Parent != null && element.Parent.HasChild(element))
            {
                return new Vector2(element.Width.Pixels, element.Height.Pixels);
            }
            return Vector2.Zero;
        }

        public static float GetSizeIfAppended(this UIElement element, float size)
        {
            if (element.Parent != null && element.Parent.HasChild(element))
            {
                return size;
            }
            return 0f;
        }

        public static void AppendOrRemove(this UIElement parent, UIElement child, bool shouldAppend)
        {
            if (shouldAppend)
            {
                if (!parent.HasChild(child))
                {
                    parent.Append(child);
                }
            }
            else if (parent.HasChild(child))
            {
                parent.RemoveChild(child);
            }
        }

        public static void ScaleElement(this UITwailaElement element, Vector2 maxSize)
        {
            float scale = element.GetScale(maxSize);
            float width = element.GetContentSize().X * scale;
            float height = element.GetContentSize().Y * scale;

            element.Width.Set(width, 0);
            element.Height.Set(height, 0);
            element.Recalculate();
        }
    }
}
