using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twaila.UI
{
    public interface IDragable
    {
        bool IsDragging();

        void Drag();
    }
}
