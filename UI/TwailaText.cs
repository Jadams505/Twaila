using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.UI;

namespace Twaila.UI
{
    public class TwailaText : UITwailaElement
    {
        public string Text { get; private set; }
        public Color Color;
        public float Scale;
        public DynamicSpriteFont Font;

        public TwailaText(string text, DynamicSpriteFont font, Color color, float scale)
        {
            Font = font;
            Color = color;
            Scale = scale;
            SetText(text);
        }

        public Vector2 GetTextSize()
        {
            return Font.MeasureString(Text) * Scale;
        }

        public void SetText(string text)
        {
            if(text == null || text.Length == 0)
            {
                text = "Default Text";
            }
            Text = text;
            Width.Set(GetTextSize().X, 0);
            Height.Set(GetTextSize().Y, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            switch (drawMode)
            {
                case DrawMode.Trim:
                    DrawTrimmed(spriteBatch);
                    break;
                case DrawMode.Shrink:
                    DrawShrunk(spriteBatch);
                    break;
                case DrawMode.Overflow:
                    DrawOverflow(spriteBatch);
                    break;
            }        
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            string trimmed = Text;
            while (Font.MeasureString(trimmed).X * Scale > Width.Pixels && Text.Length > 0)
            {
                trimmed = trimmed.Substring(0, trimmed.Length - 1);
            }
            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, trimmed, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color, 0, Vector2.Zero, Scale, 0, 0);
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            float scaleX = 1;
            if (GetTextSize().X > GetDimensions().Width)
            {
                scaleX = GetDimensions().Width / GetTextSize().X;
            }
            float scaleY = 1;
            if (GetTextSize().Y > GetDimensions().Height)
            {
                scaleY = GetDimensions().Height / GetTextSize().Y;
            }
            float scale = System.Math.Min(scaleX, scaleY) * Scale;
            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color, 0, Vector2.Zero, scale, 0, 0);
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color, 0, Vector2.Zero, Scale, 0, 0);
        }

    }
}
