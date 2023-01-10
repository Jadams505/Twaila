using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Twaila.Graphics;
using Twaila.Systems;
using Twaila.UI;

namespace Twaila.Context
{
    public abstract class BaseContext
    {
        public TwailaPoint Pos { get; set; }

        public BaseContext(TwailaPoint pos)
        {
            Pos = pos;
        }

        public abstract void Update();

        public abstract bool ContextChanged(BaseContext other);

        protected abstract string GetName();

        protected abstract TwailaRender GetImage(SpriteBatch spriteBatch);

        protected abstract List<UITwailaElement> InfoElements();

        protected abstract string GetMod();

        public abstract void UpdateOnChange(BaseContext prevContext, Layout layout);
    }
}
