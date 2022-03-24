using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Twaila.UI
{
    public class TwailaText : UITwailaElement
    {
        public string Text { get; private set; }
        public Color Color { get; set; }
        public float Scale { get; set; }
        public DynamicSpriteFont Font { get; set; }

        public TwailaText(string text, DynamicSpriteFont font, Color color, float scale)
        {
            Font = font;
            Color = color;
            Scale = scale;
            SetText(text);
        }

        public Vector2 GetTextSize()
        {
            return ChatManager.GetStringSize(Font, Text, new Vector2(Scale, Scale));
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
            while (ChatManager.GetStringSize(Font, trimmed, new Vector2(Scale, Scale)).X > Width.Pixels && trimmed.Length > 0)
            {
                trimmed = trimmed.Substring(0, trimmed.Length - 1);
            }
            if(ChatManager.GetStringSize(Font, trimmed, new Vector2(Scale, Scale)).Y <= Height.Pixels)
            {
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, trimmed, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color, 0, Vector2.Zero, new Vector2(Scale, Scale));
            }
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
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, Text, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color, 0, Vector2.Zero, new Vector2(scale, scale));
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, Text, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color, 0, Vector2.Zero, new Vector2(Scale, Scale));
        }

    }
}
