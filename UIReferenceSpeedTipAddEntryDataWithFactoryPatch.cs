using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace PlanetwideMiner;

[HarmonyPatch(typeof(UIReferenceSpeedTip), "AddEntryDataWithFactory")]
internal static class UIReferenceSpeedTipAddEntryDataWithFactoryPatch
{
	private static readonly FieldInfo? TipItemIdField = AccessTools.Field(typeof(UIReferenceSpeedTip), "itemId");

	private static readonly FieldInfo? TipItemCycleField = AccessTools.Field(typeof(UIReferenceSpeedTip), "itemCycle");

	private static readonly FieldInfo? TipLoadedEntryDatasField = AccessTools.Field(typeof(UIReferenceSpeedTip), "loadedEntryDatas");

	[HarmonyPrefix]
	private static void Prefix(PlanetFactory factory)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		OilRefSpeedPatchState.OilProductIdsThisFactory = null;
		OilRefSpeedPatchState.InTipContext = false;
		if (factory?.factorySystem?.minerPool == null || factory.veinPool == null)
		{
			return;
		}
		MinerComponent[] minerPool = factory.factorySystem.minerPool;
		VeinData[] veinPool = factory.veinPool;
		int minerCursor = factory.factorySystem.minerCursor;
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 1; i < minerCursor; i++)
		{
			if (minerPool[i].id != i || (int)minerPool[i].type != 3 || minerPool[i].veins == null || minerPool[i].veinCount <= 0)
			{
				continue;
			}
			int num = minerPool[i].veins[0];
			if (num > 0 && num < veinPool.Length && veinPool[num].id == num)
			{
				int productId = veinPool[num].productId;
				if (productId > 0)
				{
					hashSet.Add(productId);
				}
			}
		}
		if (hashSet.Count > 0)
		{
			OilRefSpeedPatchState.OilProductIdsThisFactory = hashSet;
			OilRefSpeedPatchState.InTipContext = true;
		}
	}

	[HarmonyPostfix]
	private static void Postfix(UIReferenceSpeedTip __instance, PlanetFactory factory)
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Invalid comparison between Unknown and I4
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Invalid comparison between Unknown and I4
		try
		{
			object obj = TipItemCycleField?.GetValue(__instance);
			if (obj == null || (int)obj != 1)
			{
				return;
			}
			object obj2 = TipItemIdField?.GetValue(__instance);
			if (obj2 == null)
			{
				return;
			}
			int num = (int)obj2;
			if (GameMain.data?.statistics?.production?.factoryStatPool == null || factory.veinPool == null || factory.factorySystem?.minerPool == null)
			{
				return;
			}
			VeinData[] veinPool = factory.veinPool;
			MinerComponent[] minerPool = factory.factorySystem.minerPool;
			int minerCursor = factory.factorySystem.minerCursor;
			EntityData[] entityPool = factory.entityPool;
			HashSet<int> hashSet = new HashSet<int>();
			int? num2 = null;
			for (int i = 1; i < minerCursor; i++)
			{
				if (minerPool[i].id != i || (int)minerPool[i].type != 3 || minerPool[i].veins == null || minerPool[i].veinCount <= 0)
				{
					continue;
				}
				int num3 = minerPool[i].veins[0];
				if (num3 <= 0 || num3 >= veinPool.Length || veinPool[num3].id != num3)
				{
					continue;
				}
				int productId = veinPool[num3].productId;
				if (productId <= 0)
				{
					continue;
				}
				hashSet.Add(productId);
				if (entityPool != null && minerPool[i].entityId > 0 && minerPool[i].entityId < entityPool.Length)
				{
					int valueOrDefault = num2.GetValueOrDefault();
					if (!num2.HasValue)
					{
						valueOrDefault = entityPool[minerPool[i].entityId].protoId;
						num2 = valueOrDefault;
					}
				}
			}
			if (hashSet.Count == 0 || !hashSet.Contains(num))
			{
				OilRefSpeedPatchState.OilProductIdsThisFactory = null;
				OilRefSpeedPatchState.InTipContext = false;
				return;
			}
			FactoryProductionStat val = GameMain.data.statistics.production.factoryStatPool[factory.index];
			if (val?.productPool == null || val.productIndices == null)
			{
				OilRefSpeedPatchState.OilProductIdsThisFactory = null;
				OilRefSpeedPatchState.InTipContext = false;
				return;
			}
			int num4 = val.productIndices[num];
			if (num4 <= 0 || num4 >= val.productPool.Length || val.productPool[num4] == null)
			{
				OilRefSpeedPatchState.OilProductIdsThisFactory = null;
				OilRefSpeedPatchState.InTipContext = false;
				return;
			}
			float refProductSpeed = val.productPool[num4].refProductSpeed;
			double num5 = GameMain.data.history.miningSpeedScale;
			double num6 = 0.0;
			for (int j = 1; j < minerCursor; j++)
			{
				if (minerPool[j].id == j && (int)minerPool[j].type == 3 && minerPool[j].veins != null && minerPool[j].veinCount > 0)
				{
					int num7 = minerPool[j].veins[0];
					if (num7 > 0 && num7 < veinPool.Length && veinPool[num7].productId == num)
					{
						num6 += 3600.0 / (double)minerPool[j].period * num5 * (double)minerPool[j].speed * (double)veinPool[num7].amount * (double)VeinData.oilSpeedMultiplier;
					}
				}
			}
			float num8 = refProductSpeed - (float)num6;
			if (!num2.HasValue || !(TipLoadedEntryDatasField?.GetValue(__instance) is Array array) || array.Length <= num2.Value)
			{
				return;
			}
			Type elementType = array.GetType().GetElementType();
			if (!(elementType != null))
			{
				return;
			}
			FieldInfo fieldInfo = AccessTools.Field(elementType, "normalSpeed");
			if (fieldInfo != null)
			{
				object value = array.GetValue(num2.Value);
				if (value != null)
				{
					float num9 = (float)(fieldInfo.GetValue(value) ?? ((object)0f));
					fieldInfo.SetValue(value, num9 + num8);
					array.SetValue(value, num2.Value);
				}
			}
		}
		finally
		{
			OilRefSpeedPatchState.OilProductIdsThisFactory = null;
			OilRefSpeedPatchState.InTipContext = false;
		}
	}
}
