using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public abstract Vector2 GetContentSize();

        protected abstract void DrawShrunk(SpriteBatch spriteBatch);

        protected abstract void DrawTrimmed(SpriteBatch spriteBatch);

        protected abstract void DrawOverflow(SpriteBatch spriteBatch);
    }
}
