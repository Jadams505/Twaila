using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;

namespace Twaila.UI
{
    public enum InfoType
    {
        Name,
        PickaxePower,
        InfoIcons,
        Mod,
        Count
    }

    public class TwailaInfoBox : UITwailaElement
    {
        public TwailaText[] InfoLines { get; private set; }
        public bool[] Appended { get; private set; }

        public TwailaInfoBox()
        {
            InfoLines = new TwailaText[(int)InfoType.Count];
            Appended = new bool[InfoLines.Length];
            InfoLines[0] = new TwailaText("Default Name", FontAssets.CombatText[0].Value, Color.White, 1f);
            for (int i = 1; i < InfoLines.Length; ++i)
            {
                InfoLines[i] = new TwailaText("Terraria", FontAssets.ItemStack.Value, Color.White, 1f);
            }
        }

        public void ApplyToAll(Action<TwailaText> action)
        {
            for (int i = 0; i < InfoLines.Length; ++i)
            {
                TwailaText text = InfoLines[i];
                if (text != null && Appended[i])
                {
                    action.Invoke(text);
                }
            }
        }

        public void SetText(InfoType type, string text)
        {
            InfoLines[(int)type].SetText(text);
        }

        public void SetAndAppend(InfoType type, string text)
        {
            InfoLines[(int)type].SetText(text);
            AppendElements(type);
        }

        public void SetEnabled(InfoType type, bool enabled)
        {
            if (enabled)
            {
                AppendElements(type);
            }
            else
            {
                RemoveElements(type);
            }
        }

        public void AppendElements(params InfoType[] indexes)
        {
            foreach(InfoType type in indexes)
            {
                int i = (int)type;
                if (!Appended[i])
                {
                    Append(InfoLines[i]);
                    Appended[i] = true;
                } 
            }
        }

        public void RemoveElements(params InfoType[] indexes)
        {
            foreach (InfoType type in indexes)
            {
                int i = (int)type;
                if (Appended[i])
                {
                    RemoveChild(InfoLines[i]);
                    Appended[i] = false;
                }
            }
        }

        public void RemoveAll()
        {
            RemoveAllChildren();
            Array.Fill(Appended, false);
        }

        public bool IsEmpty()
        {
            return Elements.Count == 0;
        }

        public int NumberOfAppendedElements()
        {
            return Elements.Count;
        }

        public void UpdateDimensions()
        {
            float width = 0, height = 0;
            for(int i = 0; i < InfoLines.Length; ++i)
            {
                if (Appended[i])
                {
                    TwailaText element = InfoLines[i];
                    Vector2 size = element.GetTextSize();
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

        private TwailaText Get(InfoType type)
        {
            return InfoLines[(int)type];
        }

        public void UpdateVertically()
        {
            float height = 0;
            for(int i = 0; i < (int)InfoType.Count; ++i)
            {
                TwailaText curr = Get((InfoType)i);
                if (Appended[i])
                {
                    curr.Top.Set(height, 0);
                    height += curr.Height.Pixels;
                }
            }
        }
    }
}
