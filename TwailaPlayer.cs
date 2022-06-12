using Terraria.GameInput;
using Terraria.ModLoader;
using Twaila.Systems;

namespace Twaila
{
    public class TwailaPlayer : ModPlayer
    {
        public bool CyclingPaused { get; set; }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            KeybindSystem.HandleKeys(this);
        }

        public override void PreSavePlayer()
        {
            TwailaConfig.Get().Save();
        }
    }
}
