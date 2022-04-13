using Terraria.GameInput;
using Terraria.ModLoader;
using Twaila.Util;

namespace Twaila
{
    public class TwailaPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Keybinds.HandleKeys();
        }

        public override void PreSavePlayer()
        {
            TwailaConfig.Get().Save();
        }
    }
}
