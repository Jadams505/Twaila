using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using Twaila.ObjectData;

namespace Twaila
{
    public class Twaila : Mod
    {
        public static Twaila Instance => ModContent.GetInstance<Twaila>();

        public override void Load()
        {
            ExtraObjectData.Initialize();
            IL_Main.DrawMap += IL_Main_DrawMap;
        }

        private void IL_Main_DrawMap(MonoMod.Cil.ILContext il)
        {
            var cursor = new ILCursor(il);

            bool success = cursor.TryGotoNext(MoveType.After,
                x => x.MatchLdloc(out int index) && index == 133,   // ldloc.s flag2
                x => x.Match(OpCodes.Brtrue));                      // brtrue
                                                                    // ldsfld Main::Map
            if (!success)
            {
                Logger.Warn("Failed to IL edit Main.DrawMap");
                return;
            }

            cursor.Emit(OpCodes.Ldloc, 131);    // ldloc.s num89
            cursor.Emit(OpCodes.Ldloc, 132);    // ldloc.s num90
            cursor.EmitDelegate(StealMapPointFromVanilla);
        }

        public static void StealMapPointFromVanilla(int x, int y)
        {
            if (Main.LocalPlayer.TryGetModPlayer<TwailaPlayer>(out var player))
            {
                player.MapTilePos = new Point(x, y);
            }
        }

        public override void Unload()
        {
            ExtraObjectData.Unload();
        }
    }
}