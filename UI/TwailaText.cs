using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
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

        public TwailaText(string text) : this(text, FontAssets.ItemStack.Value, Color.White, 1f) { }

        public TwailaText() : this("Default Text") { }

        public void SetText(string text)
        {
            if(text == null || text.Length == 0)
            {
                text = "Default Text";
            }
            Text = text;
            Width.Set(GetContentSize().X, 0);
            Height.Set(GetContentSize().Y, 0);
        }

        public override void ApplyConfigSettings(TwailaConfig config)
        {
            base.ApplyConfigSettings(config);
            OverrideTextColor = config.OverrideColor;
            Color = config.TextColor.Color;
            TextShadow = config.TextShadow;
        }

        public override Vector2 GetContentSize()
        {
            return ChatManager.GetStringSize(Font, Text, new Vector2(Scale, Scale));
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
            if (GetContentSize().X > GetDimensions().Width)
            {
                scaleX = GetDimensions().Width / GetContentSize().X;
            }
            float scaleY = 1;
            if (GetContentSize().Y > GetDimensions().Height)
            {
                scaleY = GetDimensions().Height / GetContentSize().Y;
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
                ChatManager.DrawColorCodedStringShadow(spriteBatch, Font, snippets, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color.Black * Opacity, 0, Vector2.Zero, scale);
            }
            ChatManager.DrawColorCodedString(spriteBatch, Font, snippets, new Vector2((int)GetDimensions().X, (int)GetDimensions().Y), Color * Opacity, 0, Vector2.Zero, scale, out int unimplemented, -1, OverrideTextColor);
        }
    }
}
