using System;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Object = object;   

namespace PlanetwideMiner;

[HarmonyPatch(typeof(UIMinerWindow), "_OnUpdate")]
internal static class UIMinerWindowUpdatePatch
{
	[HarmonyPostfix]
	private static void Postfix(UIMinerWindow __instance)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Invalid comparison between Unknown and I4
		try
		{
			if ((Object)(object)__instance == (Object)null || __instance.factory == null || __instance.factorySystem == null)
			{
				return;
			}
			int minerId = __instance.minerId;
			if (minerId <= 0)
			{
				return;
			}
			MinerComponent val = __instance.factorySystem.minerPool[minerId];
			if (val.id != minerId || (int)val.type != 3)
			{
				return;
			}
			VeinData[] veinPool = __instance.factory.veinPool;
			if (veinPool == null || val.veinCount <= 0 || val.veins == null)
			{
				return;
			}
			int num = val.veins[0];
			if (num <= 0 || num >= veinPool.Length || veinPool[num].id != num)
			{
				return;
			}
			int productId = veinPool[num].productId;
			if (productId <= 0)
			{
				return;
			}
			long num2 = 0L;
			for (int i = 1; i < veinPool.Length; i++)
			{
				if (veinPool[i].id == i && veinPool[i].productId == productId)
				{
					num2 += veinPool[i].amount;
				}
			}
			if (num2 > 0)
			{
				float num3 = (float)num2 * VeinData.oilSpeedMultiplier;
				__instance.veinAmountText.text = num3.ToString("0.0000") + "/s";
				int networkId = __instance.powerSystem.consumerPool[val.pcId].networkId;
				PowerNetwork val2 = __instance.powerSystem.netPool[networkId];
				float num4 = ((val2 != null && networkId > 0) ? ((float)val2.consumerRatio) : 0f);
				float num5 = (float)((double)val.period / 600000.0);
				float num6 = ((num5 > 0f) ? (60f / num5) : 0f);
				float num7 = (float)(0.0001 * (double)val.speed * (double)val.speedDamper * (double)num4 * (double)GameMain.history.miningSpeedScale) * (float)((double)num2 * (double)VeinData.oilSpeedMultiplier);
				if ((int)val.workstate <= 0 || num4 < 0.1f)
				{
					num7 = 0f;
				}
				__instance.speedText.text = (num6 * num7).ToString("0.0") + Localization.Translate("每分钟");
			}
		}
		catch (Exception arg)
		{
			ManualLogSource? log = Plugin.Log;
			if (log != null)
			{
				log.LogError((object)$"[PlanetwideMiner] UIMinerWindowUpdatePatch 异常: {arg}");
			}
		}
	}
}
