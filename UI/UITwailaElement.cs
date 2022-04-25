using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Twaila.UI
{
    public enum DrawMode
    {
        Shrink, Trim, Overflow
    }
    public class UITwailaElement : UIElement
    {
        public DrawMode DrawMode { get; set; }
        public float Opacity { get; set; }

        public UITwailaElement()
        {
            DrawMode = DrawMode.Trim;
            Opacity = 1.0f;
        }

        protected virtual void DrawShrunk(SpriteBatch spriteBatch)
        {

        }

        protected virtual void DrawTrimmed(SpriteBatch spriteBatch)
        {

        }

        protected virtual void DrawOverflow(SpriteBatch spriteBatch)
        {

        }
    }
}
