using Microsoft.Xna.Framework.Graphics;

namespace Twaila.Graphics
{
    public class TwailaTexture
    {
        public Texture2D Texture { get; private set; }
        public float Scale { get; set; }

        public TwailaTexture(Texture2D texture, float scale)
        {
            Texture = texture;
            Scale = scale;
        }

        public TwailaTexture(Texture2D texture) : this(texture, 1) { }

        public int Width()
        {
            return (int)(Texture.Width * Scale + 1);
        }

        public int Height()
        {
            return (int)(Texture.Height * Scale + 1);
        }
    }
}
