using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twaila.Graphics;
using Twaila.Systems;
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

        public abstract bool Applies();

        public abstract void UpdateOnChange(BaseContext prevContext, Layout layout);
    }
}
