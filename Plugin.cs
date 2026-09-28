using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace PlanetwideMiner;

[BepInPlugin("un1eagle.planetwideminer", "全球矿机", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
	internal static ManualLogSource? Log;

	internal static Plugin? Instance;

	private void Awake()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Log = Logger;
		Instance = this;
		Harmony val = new Harmony("un1eagle.planetwideminer");
		val.PatchAll(typeof(Plugin).Assembly);   
		Log.LogInfo((object)"[全球矿机] 加载完成 (GUID: un1eagle.planetwideminer, Version: 1.0.0)");
	}

	internal static void ExpandVeinMinerToPlanet(ref MinerComponent miner, PlanetFactory factory, VeinData[] veinPool)
	{
		if (miner.veinCount <= 0 || miner.veins == null || veinPool.Length == 0)
		{
			return;
		}
		int num = miner.veins[0];
		if (num <= 0 || num >= veinPool.Length || veinPool[num].id != num)
		{
			return;
		}
		int productId = veinPool[num].productId;
		if (productId <= 0)
		{
			return;
		}
		List<int> list = new List<int>(128);
		for (int i = 1; i < veinPool.Length; i++)
		{
			if (veinPool[i].id == i && veinPool[i].productId == productId)
			{
				list.Add(i);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		miner.InitVeinArray(list.Count);
		for (int j = 0; j < list.Count; j++)
		{
			miner.veins[j] = list[j];
		}
		miner.ArrangeVeinArray();
		miner.currentVeinIndex = 0;
		miner.GetMinimumVeinAmount(factory, veinPool);
		if (!GameMain.data.gameDesc.isInfiniteResource)
		{
			miner.GetTotalVeinAmount(veinPool);
		}
		foreach (int item in list)
		{
			factory.RefreshVeinMiningDisplay(item, miner.entityId, 0);
		}
		ManualLogSource? log = Log;
		if (log != null)
		{
			log.LogInfo((object)$"[PlanetwideMiner] Vein 矿机 {miner.id} 在建造时扩展为全球矿机，同种矿脉数={list.Count}。");
		}
	}

	internal static void ExpandOilMinerVeins(ref MinerComponent miner, PlanetFactory factory, VeinData[] veinPool)
	{
		if (miner.veinCount <= 0 || miner.veins == null || veinPool.Length == 0)
		{
			return;
		}
		int num = miner.veins[0];
		if (num <= 0 || num >= veinPool.Length || veinPool[num].id != num)
		{
			return;
		}
		int productId = veinPool[num].productId;
		if (productId <= 0)
		{
			return;
		}
		List<int> list = new List<int>(32);
		for (int i = 1; i < veinPool.Length; i++)
		{
			if (veinPool[i].id == i && veinPool[i].productId == productId)
			{
				list.Add(i);
			}
		}
		if (list.Count != 0)
		{
			miner.InitVeinArray(list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				miner.veins[j] = list[j];
			}
			miner.ArrangeVeinArray();
			miner.currentVeinIndex = 0;
		}
	}
}
