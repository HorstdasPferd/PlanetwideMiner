using System.Collections.Generic;

namespace PlanetwideMiner;

internal static class OilRefSpeedPatchState
{
	internal static HashSet<int>? OilProductIdsThisFactory;

	internal static bool InTipContext;
}
