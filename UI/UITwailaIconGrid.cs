using Microsoft.Xna.Framework;
using System;
using Twaila.Graphics;

namespace Twaila.UI
{
    public class UITwailaIconGrid : UITwailaGrid
    {
        public UITwailaIconGrid(int width) : this(width, 20f)
        {

        }

        public UITwailaIconGrid(int width, float maxSize) : base(width)
        {
            RowPadding = 4;
            MaxCellHeight = maxSize;
            MaxCellWidth = maxSize;
        }

        public override Vector2 SizePriority()
        {
            return new Vector2(Math.Max(GridWidth / 4f, 1), GridHeight);
        }

        public void AddIcon(TwailaRender iconImage)
        {
            foreach(var info in iconImage.Info)
            {
                info.Scale = IconScale(iconImage);
            }
            Add(new UITwailaImage(new TwailaRender(iconImage.Info)));
        }

        public float IconScale(TwailaRender iconImage)
        {
            return Math.Clamp(Math.Min(MaxCellWidth, MaxCellHeight) / Math.Max(iconImage.Width, iconImage.Height), 0, 1);
        }

        public Vector2 IconSize(TwailaRender image)
        {
            float scale = IconScale(image);
            return new Vector2(image.Width * scale, image.Height * scale);
        }
    }
}
