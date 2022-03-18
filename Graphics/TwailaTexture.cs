using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace Twaila.Graphics
{
    public class TwailaTexture
    {
        public Texture2D Texture { get; private set; }
        public float Scale { get; set; }

        public TwailaTexture(Texture2D texture, float scale)
        {
            Texture = texture ?? Main.buffTexture[BuffID.Confused];
            Scale = scale;
        }

        public TwailaTexture(Texture2D texture) : this(texture, 1) { }

        public int Width()
        {
            return (int)(Texture.Width * Scale);
        }

        public int Height()
        {
            return (int)(Texture.Height * Scale);
        }
    }
}
