using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace BetterFire;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
	private static Harmony _harmony;

	private void Awake()
	{
		_harmony = Harmony.CreateAndPatchAll(typeof(DamageablePatch));
		// Plugin startup logic
		Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
	}

	private void OnDestroy()
	{
		_harmony.UnpatchSelf();
	}
}

[HarmonyPatch]
public static class DamageablePatch
{
	[HarmonyTranspiler]
	[HarmonyPatch(typeof(Damageable), nameof(Damageable.disapearAfterDeathAnimation), MethodType.Enumerator)]
	static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
	{
		var label = il.DefineLabel();
		var c = new CodeMatcher(instructions, il)
			.MatchForward(false,
				new CodeMatch(OpCodes.Ldloc_1),
				new CodeMatch(OpCodes.Call),
				new CodeMatch(OpCodes.Call),
				new CodeMatch(OpCodes.Brfalse),
				new CodeMatch(OpCodes.Ldc_I4_5),
				new CodeMatch(OpCodes.Stloc_S)
			);

		c.Advance(3).SetOperandAndAdvance(label);
		c.MatchForward(false,
			new CodeMatch(OpCodes.Ldsfld),
			new CodeMatch(OpCodes.Ldsfld),
			new CodeMatch(OpCodes.Ldloc_S),
			new CodeMatch(OpCodes.Callvirt),
			new CodeMatch(OpCodes.Ldc_I4_1),
			new CodeMatch(OpCodes.Ldloc_S),
			new CodeMatch(OpCodes.Callvirt),
			new CodeMatch(OpCodes.Ldnull),
			new CodeMatch(OpCodes.Ldc_I4_1),
			new CodeMatch(OpCodes.Ldloc_S),
			new CodeMatch(OpCodes.Callvirt)
		);
		c.RemoveInstructions(11);
		c.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_1) { labels = { label } });
		c.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_S, (byte)4));
		c.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_S, (byte)5));
		c.InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_S, (byte)6));
		c.InsertAndAdvance(Transpilers.EmitDelegate<Action<Damageable, Transform, InventoryItem, int>>(
			(damageable, transform, randomDropFromTable, xPType) =>
			{
				int itemId;
				var itemChange = randomDropFromTable.itemChange;
				var isOnFire = damageable.onFire && damageable.NetworkonFire;
				var canBeCooked = itemChange != null && itemChange.changesAndTheirChanger.Length != 0 &&
				                  itemChange.changesAndTheirChanger[0].taskType == DailyTaskGenerator.genericTaskType.CookMeat;
				if (isOnFire && canBeCooked)
				{
					itemId = Inventory.inv.getInvItemId(itemChange.changesAndTheirChanger[0].changesWhenComplete);
				}
				else
				{
					itemId = Inventory.inv.getInvItemId(randomDropFromTable);
				}

				NetworkMapSharer.share.spawnAServerDrop(
					itemId, 1, transform.position, null, true, xPType
				);
			}
		));
		return c.InstructionEnumeration();
	}
}