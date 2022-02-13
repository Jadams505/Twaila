using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.UI;

namespace Twaila.UI
{
    public class TwailaText : UIElement
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

        public TwailaText(string text) : this(text, Main.fontCombatText[0], Color.White, 1f)
        {

        }

        public TwailaText(string text, Color color) : this(text)
        {
            Color = color;
        }
        public Vector2 GetTextSize()
        {
            return Font.MeasureString(Text) * Scale;
        }

        public void SetText(string text)
        {
            Text = text;
            Width.Set(GetTextSize().X, 0);
            Height.Set(GetTextSize().Y, 0);
            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color, 0, Vector2.Zero, Scale, 0, 0);
        }


    }
}
