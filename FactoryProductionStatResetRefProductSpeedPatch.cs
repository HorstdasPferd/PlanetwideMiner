using System.Collections.Generic;
using HarmonyLib;

namespace PlanetwideMiner;

[HarmonyPatch(typeof(FactoryProductionStat), "ResetRefProductSpeed")]
internal static class FactoryProductionStatResetRefProductSpeedPatch
{
	[HarmonyPrefix]
	private static bool Prefix(int itemId)
	{
		if (!OilRefSpeedPatchState.InTipContext)
		{
			return true;
		}
		HashSet<int> oilProductIdsThisFactory = OilRefSpeedPatchState.OilProductIdsThisFactory;
		if (oilProductIdsThisFactory == null || !oilProductIdsThisFactory.Contains(itemId))
		{
			return true;
		}
		return false;
	}
}
