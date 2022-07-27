using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Twaila.UI
{
    public class UIStatElement : UITwailaElement
    {
        public Texture2D Icon { get; set; }
        public TwailaText StatText { get; set; }

        public const float BACKGROUND_BASE_SCALE = 0.75f;
        public const int ICON_WIDTH = 32;
        public const float PADDING_BETWEEN = 20f;
        public const float PADDING_RIGHT = 4f;

        public UIStatElement(Texture2D icon, string text) : base()
        {
            Icon = icon;
            StatText = new TwailaText(text);

            StatText.Left.Set(ICON_WIDTH, 0);
            StatText.Top.Set(2, 0);
            Width.Set(GetContentSize().X, 0);
            Height.Set(GetContentSize().Y, 0);
        }

        public override Vector2 GetContentSize()
        {
            return new Vector2(Icon.Width * BACKGROUND_BASE_SCALE, Icon.Height * BACKGROUND_BASE_SCALE);
        }

        public override void ApplyConfigSettings(TwailaConfig config)
        {
            base.ApplyConfigSettings(config);
            StatText.ApplyConfigSettings(config);
        }

        public override void ApplyHoverSettings(TwailaConfig config)
        {
            base.ApplyHoverSettings(config);
            StatText.ApplyHoverSettings(config);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = new Vector2((int)GetDimensions().X, (int)GetDimensions().Y);

            float width = ICON_WIDTH + PADDING_BETWEEN + StatText.GetContentSize().X + PADDING_RIGHT;

            spriteBatch.Draw(Icon, drawPos, new Rectangle(0, 0, (int)width, Icon.Height), Color.White * Opacity, 0, Vector2.Zero, BACKGROUND_BASE_SCALE, 0, 0);

            if (!HasChild(StatText))
            {
                Append(StatText);
            }
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = new Vector2((int)GetDimensions().X, (int)GetDimensions().Y);

            int width = (int)Math.Min(ICON_WIDTH + PADDING_BETWEEN + StatText.GetContentSize().X + PADDING_RIGHT, Icon.Width);

            float scale = GetScale(new Vector2(Width.Pixels, Height.Pixels));

            StatText.Left.Set(ICON_WIDTH * scale, 0);

            spriteBatch.Draw(Icon, drawPos, new Rectangle(0, 0, width, Icon.Height), Color.White * Opacity, 0, Vector2.Zero, scale * BACKGROUND_BASE_SCALE, 0, 0);

            if (!HasChild(StatText))
            {
                Append(StatText);
            }
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = new Vector2((int)GetDimensions().X, (int)GetDimensions().Y);

            int width = (int)Math.Min(ICON_WIDTH + PADDING_BETWEEN + StatText.GetContentSize().X + PADDING_RIGHT, Width.Pixels * (1 / BACKGROUND_BASE_SCALE));

            spriteBatch.Draw(Icon, drawPos, new Rectangle(0, 0, width, Icon.Height), Color.White * Opacity, 0, Vector2.Zero, BACKGROUND_BASE_SCALE, 0, 0);

            if(ICON_WIDTH + StatText.GetContentSize().X > width)
            {
                RemoveChild(StatText);
            }
            else if (!HasChild(StatText))
            {
                Append(StatText);
            }
        }

    }
}
