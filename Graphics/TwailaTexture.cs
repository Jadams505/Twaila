using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace Twaila.Graphics
{
    public delegate void TextureDrawer(SpriteBatch spriteBatch, Point pos);

    public class TwailaTexture
    {
        public Texture2D Texture { get; private set; }
        public float Scale { get; set; }

        public TextureDrawer Drawer { get; set; }

        public float Width => Texture == null ? 0 : Texture.Width * Scale;

        public float Height => Texture == null ? 0 : Texture.Height * Scale;

        public TwailaTexture(Texture2D texture, float scale)
        {
            Texture = texture;
            Scale = scale;
        }

        public TwailaTexture(Texture2D texture) : this(texture, 1) { }

    }
}
