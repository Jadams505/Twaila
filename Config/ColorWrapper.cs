using Microsoft.Xna.Framework;

namespace Twaila.Config
{
    public class ColorWrapper
    {
        public Color Color;

        public ColorWrapper(byte r, byte g, byte b, byte a)
        {
            Color = new Color(r, g, b, a);
        }

        public ColorWrapper()
        {
            Color = new Color(0, 0, 0, 0);
        }

        public override bool Equals(object obj)
        {
            if (obj is Color other)
            {
                return Color.Equals(other);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode();
        }
    }
}
