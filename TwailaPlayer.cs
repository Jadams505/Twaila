using Microsoft.Xna.Framework;
using Terraria.GameInput;
using Terraria.ModLoader;
using Twaila.Config;
using Twaila.Systems;

namespace Twaila
{
    public class TwailaPlayer : ModPlayer
    {
        public Point MapTilePos { get; set; }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            KeybindSystem.HandleKeys(this);
        }

        public override void PreSavePlayer()
        {
            TwailaConfig.Instance.Save();
        }
    }
}
