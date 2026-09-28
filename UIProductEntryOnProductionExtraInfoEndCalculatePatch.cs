using System;
using BepInEx.Logging;
using HarmonyLib;

namespace PlanetwideMiner;

[HarmonyPatch(typeof(UIProductEntry), "OnProductionExtraInfoEndCalculate")]
internal static class UIProductEntryOnProductionExtraInfoEndCalculatePatch
{
	[HarmonyPostfix]
	private static void Postfix(UIProductEntry __instance)
	{
		try
		{
			__instance.UpdateExtraProductTexts();
		}
		catch (Exception arg)
		{
			ManualLogSource? log = Plugin.Log;
			if (log != null)
			{
				log.LogError((object)$"[PlanetwideMiner] UIProductEntryOnProductionExtraInfoEndCalculatePatch: {arg}");
			}
		}
	}
}
