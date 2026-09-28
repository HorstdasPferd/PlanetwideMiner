using System;
using BepInEx.Logging;
using HarmonyLib;

namespace PlanetwideMiner;

[HarmonyPatch(typeof(PlanetFactory), "OnBuildEntity")]
internal static class PlanetFactoryBuildPatch
{
	[HarmonyPostfix]
	private static void Postfix(PlanetFactory __instance, int newEntityId, int oldPrebuildId)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		try
		{
			if (__instance == null || __instance.factorySystem == null || __instance.veinPool == null || newEntityId <= 0 || newEntityId >= __instance.entityPool.Length)
			{
				return;
			}
			ref EntityData reference = ref __instance.entityPool[newEntityId];
			if (reference.id == newEntityId && reference.minerId > 0)
			{
				ref MinerComponent reference2 = ref __instance.factorySystem.minerPool[reference.minerId];
				if ((int)reference2.type == 2)
				{
					Plugin.ExpandVeinMinerToPlanet(ref reference2, __instance, __instance.veinPool);
				}
				else if ((int)reference2.type == 3)
				{
					Plugin.ExpandOilMinerVeins(ref reference2, __instance, __instance.veinPool);
				}
			}
		}
		catch (Exception arg)
		{
			ManualLogSource? log = Plugin.Log;
			if (log != null)
			{
				log.LogError((object)$"[PlanetwideMiner] PlanetFactoryBuildPatch 异常: {arg}");
			}
		}
	}
}
