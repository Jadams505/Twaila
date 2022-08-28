using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Twaila.Graphics
{
    public class DrawInfo
    {
        public Texture2D Texture { get; set; }
        public Point Position { get; set; }
        public Rectangle Source { get; set; }
        public Color Color { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Scale { get; set; }

        public DrawInfo(Texture2D texture, Point position, Rectangle source, Color color, SpriteEffects effects = SpriteEffects.None, float scale = 1)
        {
            Texture = texture;
            Position = position;
            Source = source;
            Color = color;
            Scale = scale;
            Effects = effects;
        }

        public DrawInfo(Texture2D texture, Point position, Rectangle source, float scale = 1) :
            this(texture, position, source, Color.White, scale: scale)
        {

        }

        public Vector2 Size()
        {
            return new Vector2(Source.Width * Scale, Source.Height * Scale);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position.ToVector2(), Source, Color, 0, Vector2.Zero, Scale, 0, 0);
        }
    }
}
