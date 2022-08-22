using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twaila.Graphics;

namespace Twaila.UI
{
    public class TwailaIconLine : UITwailaElement
    {
        public const float MAX_SIZE = 20.8f;
        public const int PADDING = 2;

        public List<TwailaRender> IconImages { get; private set; }

        public TwailaIconLine() : base()
        {
            IconImages = new List<TwailaRender>();
        }

        public override Vector2 GetContentSize()
        {
            float width = 0;
            float height = 0;
            IconImages.ForEach(image =>
            {
                Vector2 iconSize = IconSize(image);
                width += iconSize.X;
                if (iconSize.Y > height)
                {
                    height = iconSize.Y;
                }
            });
            return new Vector2(MAX_SIZE * IconImages.Count + PADDING * (IconImages.Count - 1), MAX_SIZE + PADDING);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = new Vector2((int)GetDimensions().X, (int)GetDimensions().Y);
            IconImages.ForEach(image =>
            {
                float scale = IconScale(image);
                Rectangle bounds = new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)image.Width, (int)image.Height);
                image.Draw(spriteBatch, bounds, Color.White * Opacity, scale);
                drawPos.X += image.Width * scale + PADDING;
            });
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            float scale = GetScale(new Vector2(Width.Pixels, Height.Pixels));
            Vector2 drawPos = GetDimensions().Position();

            IconImages.ForEach(image =>
            {
                Vector2 iconSize = IconSize(image);
                Vector2 centerPos = new Vector2((int)drawPos.X, (int)(drawPos.Y + (Height.Pixels - (iconSize.Y * scale)) / 2));
                Rectangle bounds = new Rectangle((int)centerPos.X, (int)centerPos.Y, (int)image.Width, (int)image.Height);
                float drawScale = MathHelper.Clamp(scale * IconScale(image), 0, 1);

                image.Draw(spriteBatch, bounds, Color.White * Opacity, drawScale);
                drawPos.X += iconSize.X * scale + PADDING;
            });
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = GetDimensions().Position();
            float width = 0;

            if(GetContentSize().Y > Height.Pixels)
            {
                return;
            }

            IconImages.ForEach(image =>
            {
                float scale = IconScale(image);
                width += image.Width * scale;
                if(width <= Width.Pixels)
                {
                    Vector2 centerPos = new Vector2((int)drawPos.X, (int)(drawPos.Y + (Height.Pixels - IconSize(image).Y) / 2));
                    Rectangle bounds = new Rectangle((int)centerPos.X, (int)centerPos.Y, (int)image.Width, (int)image.Height);

                    image.Draw(spriteBatch, bounds, Color.White * Opacity, scale);
                    drawPos.X += image.Width * scale + PADDING;
                }
            });
        }

        public static float IconScale(TwailaRender iconImage)
        {
            return MathHelper.Clamp(MAX_SIZE / Math.Max(iconImage.Width, iconImage.Height), 0, 1);
        }

        public static Vector2 IconSize(TwailaRender image)
        {
            float scale = IconScale(image);
            return new Vector2(image.Width * scale, image.Height * scale);
        }
    }
}
