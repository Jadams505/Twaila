using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;

namespace Twaila.UI
{
    public enum DrawMode
    {
        Shrink, Trim, Overflow
    }

    public abstract class UITwailaElement : UIElement
    {
        public DrawMode DrawMode { get; set; }
        public float Opacity { get; set; }

        public UITwailaElement()
        {
            DrawMode = DrawMode.Trim;
            Opacity = 1.0f;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            switch (DrawMode)
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

        public virtual void ApplyConfigSettings(TwailaConfig config)
        {
            DrawMode = config.ContentSetting;
        }

        public float GetScale(Vector2 maxSize)
        {
            float scaleX = 1;
            if (GetContentSize().X > maxSize.X)
            {
                scaleX = maxSize.X / GetContentSize().X;
            }
            float scaleY = 1;
            if (GetContentSize().Y > maxSize.Y)
            {
                scaleY = maxSize.Y / GetContentSize().Y;
            }
            return Math.Min(scaleX, scaleY);
        }

        public abstract Vector2 GetContentSize();

        protected abstract void DrawShrunk(SpriteBatch spriteBatch);

        protected abstract void DrawTrimmed(SpriteBatch spriteBatch);

        protected abstract void DrawOverflow(SpriteBatch spriteBatch);
    }
}
