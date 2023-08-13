using System;
using System.ComponentModel;

namespace Twaila.Config
{
    public class ContentToggles
    {
        [DefaultValue(true)]
        public bool EnableWireContent;

        [DefaultValue(true)]
        public bool EnableNpcContent;

        [DefaultValue(true)]
        public bool EnableTileContent;

        [DefaultValue(true)]
        public bool EnableLiquidContent;

        [DefaultValue(true)]
        public bool EnableWallContent;

        public ContentToggles()
        {
            EnableWireContent = true;
            EnableNpcContent = true;
            EnableTileContent = true;
            EnableLiquidContent = true;
            EnableWallContent = true;
        }

        public override bool Equals(object obj)
        {
            return obj is ContentToggles toggles &&
                   EnableWireContent == toggles.EnableWireContent &&
                   EnableNpcContent == toggles.EnableNpcContent &&
                   EnableTileContent == toggles.EnableTileContent &&
                   EnableLiquidContent == toggles.EnableLiquidContent &&
                   EnableWallContent == toggles.EnableWallContent;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EnableWireContent, EnableNpcContent, EnableTileContent, EnableLiquidContent, EnableWallContent);
        }
    }
}
