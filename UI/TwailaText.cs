using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria.UI.Chat;

namespace Twaila.UI
{
    public class TwailaText : UITwailaElement
    {
        public string Text { get; private set; }
        public Color Color { get; set; }
        public bool OverrideTextColor { get; set; }
        public float Scale { get; set; }
        public DynamicSpriteFont Font { get; set; }
        public bool TextShadow { get; set; }

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
            if(ChatManager.GetStringSize(Font, Text, new Vector2(Scale, Scale)).Y <= Height.Pixels)
            {
                List<TextSnippet> snippets = ChatManager.ParseMessage(Text, Color);
                if(snippets.Count == 0)
                {
                    return;
                }
                TextSnippet trimSnippet = GetSnippetToTrim(snippets, out int index, out float trimWidth);

                if(trimWidth > 0)
                {
                    if(trimSnippet.GetType() == typeof(TextSnippet))
                    {
                        string trimmed = trimSnippet.Text;
                        int i = trimmed.Length - 1;
                        float len = ChatManager.GetStringSize(Font, trimSnippet.Text, new Vector2(Scale, Scale)).X;
                        while (len > trimWidth && i >= 0)
                        {
                            len -= Font.GetCharacterMetrics(trimSnippet.Text[i]).KernedWidth;
                            i--;
                        }
                        snippets[index].Text = snippets[index].Text.Substring(0, i + 1);
                    }
                    else
                    {
                        index--; // Non text snippets cannot be trimmed so the whole snippet must be removed
                    }    
                }
                TextSnippet[] remainingSnippets = new TextSnippet[index + 1];
                snippets.CopyTo(0, remainingSnippets, 0, index + 1);

                DrawText(spriteBatch, remainingSnippets, new Vector2(Scale, Scale));
            }
        }

        private TextSnippet GetSnippetToTrim(List<TextSnippet> snippets, out int index, out float trimWidth)
        {
            float len = 0;
            for(int i = 0; i < snippets.Count; ++i)
            {
                if (len + snippets[i].GetStringLength(Font) > Width.Pixels)
                {
                    index = i;
                    trimWidth = Width.Pixels - len;
                    return snippets[i];
                }
                len += snippets[i].GetStringLength(Font);
            }
            index = snippets.Count - 1;
            trimWidth = snippets[index].GetStringLength(Font) - Width.Pixels;
            return snippets[index];
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
            float scale = Math.Min(scaleX, scaleY) * Scale;
            TextSnippet[] snippets = ChatManager.ParseMessage(Text, Color).ToArray();
            
            foreach(TextSnippet snippet in snippets)
            {
                if(snippet.GetType() != typeof(TextSnippet))
                {
                    snippet.Scale = scale;
                }
            }

            DrawText(spriteBatch, snippets, new Vector2(scale, scale));
        }

        protected override void DrawOverflow(SpriteBatch spriteBatch)
        {
            TextSnippet[] snippets = ChatManager.ParseMessage(Text, Color).ToArray();
            Vector2 scale = new Vector2(Scale, Scale);
            DrawText(spriteBatch, snippets, scale);
        }
        private void DrawText(SpriteBatch spriteBatch, TextSnippet[] snippets, Vector2 scale)
        {
            ChatManager.ConvertNormalSnippets(snippets);
            if (TextShadow)
            {
                ChatManager.DrawColorCodedStringShadow(spriteBatch, Font, snippets, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color.Black * opacity, 0, Vector2.Zero, scale);
            }
            ChatManager.DrawColorCodedString(spriteBatch, Font, snippets, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color * opacity, 0, Vector2.Zero, scale, out int unimplemented, -1, OverrideTextColor);
        }
    }
}
