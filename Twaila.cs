using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using Twaila.ObjectData;

namespace Twaila;

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
        int mapXLocalIndex = -1;
        int mapYLocalIndex = -1;
        int flag2LocalIndex = -1;

        bool preSuccess = cursor.TryGotoNext(MoveType.After,
            x => x.MatchStloc(out flag2LocalIndex),             // stloc.s flag2
            x => x.MatchLdloc(flag2LocalIndex),                 // ldloc.s flag2
            x => x.MatchBrtrue(out _));                         // brtrue

        int index = cursor.Index;                                                             

        bool postSuccess = cursor.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld(typeof(Main), nameof(Main.Map)),         // ldsfld Main::Map
            x => x.MatchLdloc(out mapXLocalIndex),                      // ldloc.s num89
            x => x.MatchLdloc(out mapYLocalIndex));                     // ldloc.s num90

        bool prevSuccess = cursor.TryGotoPrev(MoveType.After,
            x => x.MatchCeq(),       
            x => x.MatchStloc(out _),                     
            x => x.MatchLdloc(out _),
            x => x.MatchBrfalse(out _));                     

        cursor.Index -= 2; // insert after the Stloc above.

        bool validIndexes = mapXLocalIndex != -1 && mapYLocalIndex != -1 && flag2LocalIndex != -1;

        if (!preSuccess || !postSuccess || !prevSuccess || !validIndexes)
        {
            Logger.Warn("Failed to IL edit Main.DrawMap");
            return;
        }

        cursor.Emit(OpCodes.Ldloc, mapXLocalIndex);    // ldloc.s num89
        cursor.Emit(OpCodes.Ldloc, mapYLocalIndex);    // ldloc.s num90
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