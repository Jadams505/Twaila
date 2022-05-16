using Terraria.GameInput;
using Terraria.ModLoader;
using Twaila.Util;

namespace Twaila
{
    public class TwailaPlayer : ModPlayer
    {
        public bool CyclingPaused { get; set; }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Keybinds.HandleKeys(this);
        }

        public override void PreSavePlayer()
        {
            TwailaConfig.Get().Save();
        }
    }
}
