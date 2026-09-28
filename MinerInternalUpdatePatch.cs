using HarmonyLib;

namespace PlanetwideMiner;

[HarmonyPatch(typeof(MinerComponent), "InternalUpdate")]
internal static class MinerInternalUpdatePatch
{
	[HarmonyPrefix]
	private static bool Prefix(ref MinerComponent __instance, PlanetFactory factory, VeinData[] veinPool, float power, float miningRate, float miningSpeed, int[] productRegister, ref uint __result)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected I4, but got Unknown
		if (factory == null || veinPool == null)
		{
			return true;
		}
		if ((int)__instance.type != 3)
		{
			return true;
		}
		if (power < 0.1f || __instance.veinCount <= 0)
		{
			__result = 0u;
			return false;
		}
		int num = ((__instance.veins != null && __instance.veins.Length != 0) ? __instance.veins[0] : 0);
		if (num <= 0 || num >= veinPool.Length || veinPool[num].id != num)
		{
			__result = 0u;
			return false;
		}
		uint num2 = 0u;
		double num3 = 0.0;
		lock (veinPool)
		{
			int productId = veinPool[num].productId;
			long num4 = 0L;
			for (int i = 1; i < veinPool.Length; i++)
			{
				if (veinPool[i].id == i && veinPool[i].productId == productId)
				{
					num4 += veinPool[i].amount;
				}
			}
			if (num4 <= 0)
			{
				__result = 0u;
				return false;
			}
			float num5 = (float)num4 * VeinData.oilSpeedMultiplier;
			num3 = 36000000.0 / (double)__instance.period * (double)miningSpeed * (double)(float)num4 * (double)VeinData.oilSpeedMultiplier;
			if (__instance.time < __instance.period)
			{
				__instance.time += (int)(power * __instance.speedDamper * (float)__instance.speed * miningSpeed * num5 + 0.5f);
				num2 = 1u;
			}
			if (__instance.time >= __instance.period && __instance.productCount < 50)
			{
				__instance.productId = productId;
				int num6 = __instance.time / __instance.period;
				if (miningRate > 0f && veinPool[num].amount > 2500)
				{
					__instance.costFrac += (double)miningRate * (double)num6;
					int num7 = (int)__instance.costFrac;
					__instance.costFrac -= num7;
					int num8 = veinPool[num].amount - 2500;
					if (num7 > 0)
					{
						if (num7 > num8)
						{
							num7 = num8;
						}
						if (num7 > 0)
						{
							int num9 = num;
							veinPool[num9].amount -= num7;
							VeinGroup[] veinGroups = factory.veinGroups;
							short groupIndex = veinPool[num].groupIndex;
							veinGroups[groupIndex].amount -= num7;
							factory.veinAnimPool[num].time = ((veinPool[num].amount >= 25000) ? 0f : (1f - (float)veinPool[num].amount * VeinData.oilSpeedMultiplier));
							if (veinPool[num].amount <= 2500)
							{
								factory.NotifyVeinExhausted((int)veinPool[num].type, (int)veinPool[num].groupIndex, veinPool[num].pos, false);
							}
						}
					}
				}
				__instance.productCount += num6;
				lock (productRegister)
				{
					productRegister[__instance.productId] += num6;
				}
				__instance.time -= __instance.period * num6;
			}
		}
		if (__instance.productCount > 0 && __instance.insertTarget > 0 && __instance.productId > 0)
		{
			int num10 = (int)(((num3 > 0.0) ? num3 : (36000000.0 / (double)__instance.period * (double)miningSpeed)) - 0.01) / 1800 + 1;
			if (num10 < 1)
			{
				num10 = 1;
			}
			if (num10 > 25)
			{
				num10 = 25;
			}
			int num11 = ((__instance.productCount < num10) ? __instance.productCount : num10);
			byte b = default(byte);
			int num12 = factory.InsertInto(__instance.insertTarget, 0, __instance.productId, (byte)num11, (byte)0, out b);


			__instance.productCount -= num12;
		}
		__result = num2;
		return false;
	}
}
