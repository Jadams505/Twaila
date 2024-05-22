using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace Twaila.Config
{
    public class NamingPreferences
    {
        public EnabledNames EnableNames = new();

        [Header("Priorities")]
        public NamePriorities NamePriorities = new();

        public bool ReadMe => true;

        public override bool Equals(object obj)
        {
            return obj is NamingPreferences preferences &&
                   EqualityComparer<EnabledNames>.Default.Equals(EnableNames, preferences.EnableNames) &&
                   EqualityComparer<NamePriorities>.Default.Equals(NamePriorities, preferences.NamePriorities);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EnableNames, NamePriorities);
        }
    }

    public class EnabledNames
    {
        public bool EnablePlacedItemName = true;
        public bool EnableMapName = true;
        public bool EnableInternalName = true;
        public bool EnableDroppedItemName = true;

        public override bool Equals(object obj)
        {
            return obj is EnabledNames names &&
                   EnableDroppedItemName == names.EnableDroppedItemName &&
                   EnablePlacedItemName == names.EnablePlacedItemName &&
                   EnableMapName == names.EnableMapName &&
                   EnableInternalName == names.EnableInternalName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EnableDroppedItemName, EnablePlacedItemName, EnableMapName, EnableInternalName);
        }
    }

    public class NamePriorities
    {
        public int PlacedItemPriority = 0;
        public int MapPriority = 1;
        public int InternalPriority = 2;
        public int DroppedItemPriority = 3;

        public override bool Equals(object obj)
        {
            return obj is NamePriorities priorities &&
                   DroppedItemPriority == priorities.DroppedItemPriority &&
                   PlacedItemPriority == priorities.PlacedItemPriority &&
                   MapPriority == priorities.MapPriority &&
                   InternalPriority == priorities.InternalPriority;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DroppedItemPriority, PlacedItemPriority, MapPriority, InternalPriority);
        }
    }
}
