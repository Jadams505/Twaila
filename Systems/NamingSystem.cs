using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Util;

namespace Twaila.Systems;

public enum NamingMethod
{
    DroppedItem,
    PlacedItem,
    Map,
    Internal,
}

public class NamingSystem : ModSystem
{
    public static NamingSystem Instance => ModContent.GetInstance<NamingSystem>();

    public List<NameScheme> NameSchemes { get; private set; } = new();

    public override void Load()
    {
        NameSchemes.Add(new DroppedItemScheme());
        NameSchemes.Add(new PlacedItemScheme());
        NameSchemes.Add(new MapScheme());
        NameSchemes.Add(new InternalScheme());
    }

    public override void Unload()
    {
        NameSchemes = null;
        base.Unload();
    }

    public void SortContexts()
    {
        NameSchemes.Sort((first, second) => first.GetPriority().CompareTo(second.GetPriority()));
    }

    public string GetName(string droppedItemName, string placedItemName, string mapName, string internalName)
    {
        string SchemeToName(NameScheme scheme) => scheme switch
        {
            DroppedItemScheme => droppedItemName,
            PlacedItemScheme => placedItemName,
            MapScheme => mapName,
            InternalScheme => internalName,
            _ => null
        };

        foreach(var scheme in NameSchemes)
        {
            if (!scheme.IsEnabled()) continue;

            string name = SchemeToName(scheme);
            if (name is not null)
                return name;
        }

        return null;
    }
}

public abstract class NameScheme()
{
    protected NamingPreferences Config => TwailaConfig.Instance.DisplayContent.NamingPreferences;
    public virtual bool IsEnabled() => true;
    public virtual int GetPriority() => -1;
}

public class DroppedItemScheme() : NameScheme
{
    public override bool IsEnabled() => Config.EnableNames.EnableDroppedItemName;
    public override int GetPriority() => Config.NamePriorities.DroppedItemPriority;
}

public class PlacedItemScheme : NameScheme
{
    public override bool IsEnabled() => Config.EnableNames.EnablePlacedItemName;
    public override int GetPriority() => Config.NamePriorities.PlacedItemPriority;
}

public class MapScheme : NameScheme
{
    public override bool IsEnabled() => Config.EnableNames.EnableMapName;
    public override int GetPriority() => Config.NamePriorities.MapPriority;
}

public class InternalScheme : NameScheme
{
    public override bool IsEnabled() => Config.EnableNames.EnableInternalName;
    public override int GetPriority() => Config.NamePriorities.InternalPriority;
}
