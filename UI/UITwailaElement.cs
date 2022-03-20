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
        internal DrawMode drawMode;

        public UITwailaElement()
        {
            drawMode = DrawMode.Trim;
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
