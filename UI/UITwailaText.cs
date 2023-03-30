using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Twaila.UI
{
    public class UITwailaText : UITwailaElement
    {
        public string Text { get; private set; }
        public Color Color { get; set; }
        public bool OverrideTextColor { get; set; }
        public float Scale { get; set; }
        public DynamicSpriteFont Font { get; set; }
        public bool TextShadow { get; set; }

        public UITwailaText(string text, DynamicSpriteFont font, Color color, float scale)
        {
            Font = font;
            Color = color;
            Scale = scale;
            SetText(text);
        }

        public UITwailaText(string text) : this(text, FontAssets.ItemStack.Value, Color.White, 1f) { }

        public UITwailaText() : this(Language.GetTextValue("Mods.Twaila.Defaults.Text")) { }

        public void SetText(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                text = Language.GetTextValue("Mods.Twaila.Defaults.Text");
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

        public override void ApplyHoverSettings(TwailaConfig config)
        {
            base.ApplyHoverSettings(config);
            OverrideTextColor = true;
        }

        public override Vector2 GetContentSize()
        {
            return ChatManager.GetStringSize(Font, Text, new Vector2(Scale, Scale));
        }

        protected override void DrawTrimmed(SpriteBatch spriteBatch)
        {
            Vector2 textSize = GetContentSize();
            if(textSize.Y <= Height.Pixels)
            {
                List<TextSnippet> snippets = ChatManager.ParseMessage(Text, Color);
                if(snippets.Count == 0)
                {
                    return;
                }
                TextSnippet trimSnippet = GetSnippetToTrim(snippets, out int snippetIndex, out float trimWidth);
                if(trimWidth > 0)
                {
                    if(trimSnippet.GetType() == typeof(TextSnippet))
                    {
                        string trimmed = trimSnippet.Text;
                        int charIndex = trimmed.Length - 1;
                        float len = ChatManager.GetStringSize(Font, trimSnippet.Text, new Vector2(Scale, Scale)).X;
                        while (len > trimWidth && charIndex >= 0)
                        {
                            len -= Font.GetCharacterMetrics(trimSnippet.Text[charIndex]).KernedWidth;
                            charIndex--;
                        }
                        snippets[snippetIndex].Text = snippets[snippetIndex].Text.Substring(0, charIndex + 1);
                    }
                    else
                    {
                        snippetIndex--; // Non text snippets cannot be trimmed so the whole snippet must be removed
                    }    
                }
                TextSnippet[] remainingSnippets = new TextSnippet[snippetIndex + 1];
                snippets.CopyTo(0, remainingSnippets, 0, snippetIndex + 1);

                DrawText(spriteBatch, remainingSnippets, new Vector2(Scale, Scale));
            }
        }

        private TextSnippet GetSnippetToTrim(List<TextSnippet> snippets, out int index, out float trimWidth)
        {
            float len = 0;
            for(int i = 0; i < snippets.Count; ++i)
            {
                float snippetLength = snippets[i].GetStringLength(Font);
                if (len + snippetLength > Width.Pixels)
                {
                    index = i;
                    trimWidth = Width.Pixels - len;
                    return snippets[i];
                }
                len += snippetLength;
            }
            index = snippets.Count - 1;
            trimWidth = snippets[index].GetStringLength(Font) - Width.Pixels;
            return snippets[index];
        }

        protected override void DrawShrunk(SpriteBatch spriteBatch)
        {
            float scale = GetDrawScale() * Scale;
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

            Vector2 drawPos = new Vector2(GetDimensions().X, GetDimensions().Y).Floor();
            Vector2 drawSize = ChatManager.GetStringSize(Font, Text, new Vector2(Scale, Scale)) * scale;
            drawPos.Y += Math.Max(0, (int)(Height.Pixels - drawSize.Y) / 2); // center the text vertically

            if (TextShadow)
            {
                ChatManager.DrawColorCodedStringShadow(spriteBatch, Font, snippets, drawPos, Color.Black * Opacity, 0, Vector2.Zero, scale);
            }
            ChatManager.DrawColorCodedString(spriteBatch, Font, snippets, drawPos, Color * Opacity, 0, Vector2.Zero, scale, out int unimplemented, -1, OverrideTextColor);
        }
    }
}
