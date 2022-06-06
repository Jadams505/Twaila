using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.UI;

namespace Twaila.UI
{
    public class UIInfoBox : UIElement
    {
        public List<UITwailaElement> InfoLines { get; private set; }
        public List<bool> Enabled { get; private set; }

        public UIInfoBox()
        {
            InfoLines = new List<UITwailaElement>();
            Enabled = new List<bool>();
        }

        public void AddAndEnable(UITwailaElement element)
        {
            InfoLines.Add(element);
            Enabled.Add(true);
            Append(element);
        }

        public void ApplyToAll(Action<UITwailaElement> action)
        {
            for (int i = 0; i < InfoLines.Count; ++i)
            {
                UITwailaElement line = InfoLines[i];
                if (line != null && Enabled[i])
                {
                    action.Invoke(line);
                }
            }
        }

        public void RemoveElement(int i)
        {
            RemoveChild(InfoLines[i]);
            InfoLines.RemoveAt(i);
            Enabled.RemoveAt(i);
        }

        public void RemoveAll()
        {
            InfoLines.Clear();
            Enabled.Clear();
            RemoveAllChildren();
        }

        public bool IsEmpty()
        {
            return InfoLines.Count == 0;
        }

        public int NumberOfAppendedElements()
        {
            return Elements.Count;
        }

        public void UpdateDimensions()
        {
            float width = 0, height = 0;
            for (int i = 0; i < InfoLines.Count; ++i)
            {
                if (Enabled[i])
                {
                    UITwailaElement element = InfoLines[i];
                    Vector2 size = element.GetContentSize();
                    if (size.X > width)
                    {
                        width = size.X;
                    }
                    height += size.Y;
                }
            }
            Width.Set(width, 0);
            Height.Set(height, 0);
        }

        public void UpdateDimensionsUI()
        {
            float width = 0, height = 0;
            for (int i = 0; i < InfoLines.Count; ++i)
            {
                if (Enabled[i])
                {
                    UITwailaElement element = InfoLines[i];
                    if (element.Width.Pixels > width)
                    {
                        width = element.Width.Pixels;
                    }
                    height += element.Height.Pixels;
                }
            }
            Width.Set(width, 0);
            Height.Set(height, 0);
        }

        private UITwailaElement Get(int index)
        {
            return InfoLines[index];
        }

        public void UpdateVertically()
        {
            float height = 0;
            for (int i = 0; i < InfoLines.Count; ++i)
            {
                UITwailaElement curr = Get(i);
                if (Enabled[i])
                {
                    curr.Top.Set(height, 0);
                    height += curr.Height.Pixels;
                }
            }
            Height.Set(height, 0);
        }
    }
}
