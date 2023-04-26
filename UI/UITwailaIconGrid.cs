using Microsoft.Xna.Framework;
using System;
using Twaila.Graphics;

namespace Twaila.UI
{
    public class UITwailaIconGrid : UITwailaGrid
    {
        public float MaxSize { get; set; } = 20f;

        public UITwailaIconGrid(int width) : base(width)
        {
            RowPadding = 4;
            MaxCellHeight = MaxSize;
            MaxCellWidth = MaxSize;
        }

        public void AddIcon(TwailaRender iconImage)
        {
            Add(new UITwailaImage(iconImage));
        }

        public float IconScale(TwailaRender iconImage)
        {
            return Math.Clamp(MaxSize / Math.Max(iconImage.Width, iconImage.Height), 0, 1);
        }

        public Vector2 IconSize(TwailaRender image)
        {
            float scale = IconScale(image);
            return new Vector2(image.Width * scale, image.Height * scale);
        }
    }
}
