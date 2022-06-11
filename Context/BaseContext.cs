using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Twaila.Graphics;
using Twaila.UI;

namespace Twaila.Context
{
    public abstract class BaseContext
    {
        public Point Pos { get; set; }

        public BaseContext(Point pos)
        {
            Pos = pos;
        }

        public abstract void Update();

        public abstract bool ContextChanged(BaseContext other);

        protected abstract string GetName();

        protected abstract TwailaTexture GetImage(SpriteBatch spriteBatch);

        protected abstract List<UITwailaElement> InfoElements();

        protected abstract string GetMod();

        public abstract void UpdateOnChange(BaseContext prevContext, Layout layout);
    }
}
