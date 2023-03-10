using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Twaila.Graphics;

namespace Twaila.UI
{
    public class UITwailaIconLine : UITwailaElement
    {
        public const float MAX_SIZE = 20.8f;
        public const int PADDING = 2;

        public List<TwailaRender> IconImages { get; private set; }

        public UITwailaIconLine() : base()
        {
            IconImages = new List<TwailaRender>();
        }

        public override Vector2 GetContentSize()
        {
            float width = 0;
            float height = 0;
            foreach(TwailaRender icon in IconImages)
            {
                Vector2 iconSize = IconSize(icon);
                width += iconSize.X;
                if (iconSize.Y > height)
                {
                    height = iconSize.Y;
                }
            }
            return new Vector2(width + PADDING * (IconImages.Count - 1), height);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = GetDimensions().Position();
            foreach(TwailaRender icon in IconImages)
            {
                float scale = IconScale(icon);
                Rectangle bounds = new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)icon.Width, (int)icon.Height);
                icon.Draw(spriteBatch, bounds, Color.White * Opacity, scale);
                drawPos.X += icon.Width * scale + PADDING;
            }
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            float scale = GetScale(new Vector2(Width.Pixels, Height.Pixels));
            Vector2 drawPos = GetDimensions().Position();

            foreach(TwailaRender icon in IconImages)
            {
                Vector2 iconSize = IconSize(icon);
                Vector2 centerPos = new Vector2((int)drawPos.X, (int)(drawPos.Y + (Height.Pixels - (iconSize.Y * scale)) / 2));
                Rectangle bounds = new Rectangle((int)centerPos.X, (int)centerPos.Y, (int)icon.Width, (int)icon.Height);
                float drawScale = Math.Clamp(scale * IconScale(icon), 0, 1);

                icon.Draw(spriteBatch, bounds, Color.White * Opacity, drawScale);
                drawPos.X += iconSize.X * scale + PADDING;
            }
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = GetDimensions().Position();
            float width = 0;

            if(GetContentSize().Y > Height.Pixels)
            {
               return;
            }

            foreach(TwailaRender icon in IconImages)
            {
                float scale = IconScale(icon);
                Vector2 size = IconSize(icon);
                width += size.X;
                if (width <= Width.Pixels)
                {
                    Vector2 centerPos = new Vector2(drawPos.X, drawPos.Y + (Height.Pixels - size.Y) / 2);
                    Rectangle bounds = new Rectangle((int)centerPos.X, (int)centerPos.Y, (int)icon.Width, (int)icon.Height);

                    icon.Draw(spriteBatch, bounds, Color.White * Opacity, scale);
                    drawPos.X += size.X + PADDING;
                    width += PADDING;
                }
            }
        }

        public static float IconScale(TwailaRender iconImage)
        {
            return Math.Clamp(MAX_SIZE / Math.Max(iconImage.Width, iconImage.Height), 0, 1);
        }

        public static Vector2 IconSize(TwailaRender image)
        {
            float scale = IconScale(image);
            return new Vector2(image.Width * scale, image.Height * scale);
        }
    }
}
