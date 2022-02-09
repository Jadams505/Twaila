using Terraria.GameInput;
using Terraria.ModLoader;
using Twaila.Util;
using Twaila.UI;
using Terraria;

namespace Twaila
{
    public class TwailaPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Keybinds.toggleUI.JustPressed)
            {
                TwailaUI.ToggleVisibility(null);
            }
        }
    }
}
