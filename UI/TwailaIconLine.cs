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

        public List<Texture2D> IconImages { get; private set; }

        public TwailaIconLine() : base()
        {
            IconImages = new List<Texture2D>();
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
            return new Vector2(width + PADDING * (IconImages.Count - 1), height);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = new Vector2((int)GetDimensions().X, (int)GetDimensions().Y);
            IconImages.ForEach(image =>
            {
                float scale = IconScale(image);
                spriteBatch.Draw(image, drawPos, new Rectangle(0, 0, image.Width, image.Height), Color.White * Opacity, 0, Vector2.Zero, scale, 0, 0);
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
                spriteBatch.Draw(image, centerPos, new Rectangle(0, 0, image.Width, image.Height), Color.White * Opacity, 0, Vector2.Zero, MathHelper.Clamp(scale * IconScale(image), 0, 1), 0, 0);
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
                    spriteBatch.Draw(image, centerPos, new Rectangle(0, 0, image.Width, image.Height), Color.White * Opacity, 0, Vector2.Zero, scale, 0, 0);
                    drawPos.X += image.Width * scale + PADDING;
                }
            });
        }

        public static float IconScale(Texture2D iconImage)
        {
            return MathHelper.Clamp(MAX_SIZE / Math.Max(iconImage.Width, iconImage.Height), 0, 1);
        }

        public static Vector2 IconScaleVector(Texture2D iconImage)
        {
            return new Vector2(MathHelper.Clamp(MAX_SIZE / iconImage.Width, 0, 1),
                MathHelper.Clamp(MAX_SIZE / iconImage.Height, 0, 1));
        }

        public static Vector2 IconSizeVector(Texture2D image)
        {
            Vector2 scale = IconScaleVector(image);
            return new Vector2(image.Width * scale.X, image.Height * scale.Y);
        }

        public static Vector2 IconSize(Texture2D image)
        {
            float scale = IconScale(image);
            return new Vector2(image.Width * scale, image.Height * scale);
        }
    }
}
