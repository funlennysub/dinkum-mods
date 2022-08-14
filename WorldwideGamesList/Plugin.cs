using System.Collections.Generic;
using System.Reflection.Emit;
using BepInEx;
using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace WorldwideGamesList;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static Harmony _harmony;

    private void Awake()
    {
        _harmony = Harmony.CreateAndPatchAll(typeof(SteamLobbySearchForLobbies));
        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }
}

[HarmonyPatch]
public static class SteamLobbySearchForLobbies
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(SteamLobby), nameof(SteamLobby.searchForLobbies))]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
               .MatchForward(true,
                   new CodeMatch(OpCodes.Ldc_I4_0),
                   new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Behaviour), "set_enabled")),
                   new CodeMatch(OpCodes.Ldloc_2),
                   new CodeMatch(OpCodes.Ldc_I4_1),
                   new CodeMatch(OpCodes.Add),
                   new CodeMatch(OpCodes.Stloc_2),
                   new CodeMatch(OpCodes.Ldloc_2),
                   new CodeMatch(OpCodes.Ldloc_1),
                   new CodeMatch(i => i.opcode == OpCodes.Blt),
                   new CodeMatch(i => i.opcode == OpCodes.Br))
               .Advance(1)
               .Insert(
                   new CodeInstruction(OpCodes.Ldc_I4_3),
                   new CodeInstruction(OpCodes.Call,
                       AccessTools.Method(typeof(SteamMatchmaking), nameof(SteamMatchmaking.AddRequestLobbyListDistanceFilter))
                   )
               ).InstructionEnumeration();
    }
}