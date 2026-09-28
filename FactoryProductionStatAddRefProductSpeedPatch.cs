using System.Collections.Generic;
using HarmonyLib;

namespace PlanetwideMiner;

[HarmonyPatch(typeof(FactoryProductionStat), "AddRefProductSpeed")]
internal static class FactoryProductionStatAddRefProductSpeedPatch
{
	[HarmonyPrefix]
	private static void Prefix(int itemId, ref float refSpeed)
	{
		HashSet<int> oilProductIdsThisFactory = OilRefSpeedPatchState.OilProductIdsThisFactory;
		if (oilProductIdsThisFactory != null && oilProductIdsThisFactory.Contains(itemId))
		{
			refSpeed = 0f;
		}
	}
}
