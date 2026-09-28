using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using HarmonyLib;

namespace PlanetwideMiner;

[HarmonyPatch(typeof(ProductionExtraInfoCalculator), "CalculateFactory")]
internal static class ProductionExtraInfoCalculatorFactoryPatch
{
	[CompilerGenerated]
	private sealed class _003CTranspiler_003Ed__1 : IEnumerable<CodeInstruction>, IEnumerable, IEnumerator<CodeInstruction>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private CodeInstruction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<CodeInstruction> instructions;

		public IEnumerable<CodeInstruction> _003C_003E3__instructions;

		private FieldInfo _003Cfield_003E5__2;

		private bool _003CafterOilMultiplier_003E5__3;

		private IEnumerator<CodeInstruction> _003C_003E7__wrap3;

		private CodeInstruction _003Cins_003E5__5;

		CodeInstruction IEnumerator<CodeInstruction>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CTranspiler_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
			case 3:
			case 4:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003Cfield_003E5__2 = null;
			_003C_003E7__wrap3 = null;
			_003Cins_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Cfield_003E5__2 = AccessTools.Field(typeof(VeinData), "oilSpeedMultiplier");
					if (_003Cfield_003E5__2 == null)
					{
						_003C_003E7__wrap3 = instructions.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_009f;
					}
					_003CafterOilMultiplier_003E5__3 = false;
					_003C_003E7__wrap3 = instructions.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				case 1:
					_003C_003E1__state = -3;
					goto IL_009f;
				case 2:
					_003C_003E1__state = -4;
					_003C_003E2__current = new CodeInstruction(OpCodes.Ldc_R4, (object)0f);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -4;
					_003C_003E2__current = ((_003Cins_003E5__5.opcode == OpCodes.Stloc || _003Cins_003E5__5.opcode == OpCodes.Stloc_S) ? new CodeInstruction(_003Cins_003E5__5.opcode, _003Cins_003E5__5.operand) : new CodeInstruction(_003Cins_003E5__5.opcode, (object)null));
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -4;
					_003CafterOilMultiplier_003E5__3 = false;
					break;
				case 5:
					{
						_003C_003E1__state = -4;
						_003Cins_003E5__5 = null;
						break;
					}
					IL_009f:
					if (_003C_003E7__wrap3.MoveNext())
					{
						CodeInstruction current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					return false;
				}
				if (_003C_003E7__wrap3.MoveNext())
				{
					_003Cins_003E5__5 = _003C_003E7__wrap3.Current;
					if (_003CafterOilMultiplier_003E5__3 && (_003Cins_003E5__5.opcode == OpCodes.Stloc || _003Cins_003E5__5.opcode == OpCodes.Stloc_S || _003Cins_003E5__5.opcode == OpCodes.Stloc_0 || _003Cins_003E5__5.opcode == OpCodes.Stloc_1 || _003Cins_003E5__5.opcode == OpCodes.Stloc_2 || _003Cins_003E5__5.opcode == OpCodes.Stloc_3))
					{
						_003C_003E2__current = new CodeInstruction(OpCodes.Pop, (object)null);
						_003C_003E1__state = 2;
						return true;
					}
					if (_003Cins_003E5__5.opcode == OpCodes.Ldsfld && _003Cins_003E5__5.operand is FieldInfo fieldInfo && fieldInfo == _003Cfield_003E5__2)
					{
						_003CafterOilMultiplier_003E5__3 = true;
					}
					_003C_003E2__current = _003Cins_003E5__5;
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap3 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<CodeInstruction> IEnumerable<CodeInstruction>.GetEnumerator()
		{
			_003CTranspiler_003Ed__1 _003CTranspiler_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CTranspiler_003Ed__ = this;
			}
			else
			{
				_003CTranspiler_003Ed__ = new _003CTranspiler_003Ed__1(0);
			}
			_003CTranspiler_003Ed__.instructions = _003C_003E3__instructions;
			return _003CTranspiler_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CodeInstruction>)this).GetEnumerator();
		}
	}

	[HarmonyPrefix]
	private static void Prefix(PlanetFactory factory)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and I4
		OilRefSpeedPatchState.OilProductIdsThisFactory = null;
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
		}
	}

	[IteratorStateMachine(typeof(_003CTranspiler_003Ed__1))]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CTranspiler_003Ed__1(-2)
		{
			_003C_003E3__instructions = instructions
		};
	}

	[HarmonyPostfix]
	private static void Postfix(ProductionExtraInfoCalculator __instance, GameData gameData, PlanetFactory factory)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Invalid comparison between Unknown and I4
		try
		{
			if (gameData?.statistics?.production == null || factory == null || factory.factorySystem == null)
			{
				return;
			}
			FactorySystem factorySystem = factory.factorySystem;
			MinerComponent[] minerPool = factorySystem.minerPool;
			int minerCursor = factorySystem.minerCursor;
			VeinData[] veinPool = factory.veinPool;
			if (veinPool == null || veinPool.Length == 0)
			{
				return;
			}
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
			if (hashSet.Count == 0)
			{
				OilRefSpeedPatchState.OilProductIdsThisFactory = null;
				return;
			}
			double num2 = gameData.history.miningSpeedScale;
			FactoryProductionStat val = gameData.statistics.production.factoryStatPool[factory.index];
			if (val?.productPool == null || val.productIndices == null)
			{
				return;
			}
			foreach (int item in hashSet)
			{
				long num3 = 0L;
				for (int j = 1; j < veinPool.Length; j++)
				{
					if (veinPool[j].id == j && veinPool[j].productId == item)
					{
						num3 += veinPool[j].amount;
					}
				}
				if (num3 <= 0)
				{
					continue;
				}
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				for (int k = 1; k < minerCursor; k++)
				{
					if (minerPool[k].id != k || (int)minerPool[k].type != 3 || minerPool[k].veins == null || minerPool[k].veinCount <= 0)
					{
						continue;
					}
					int num7 = minerPool[k].veins[0];
					if (num7 > 0 && num7 < veinPool.Length && veinPool[num7].productId == item)
					{
						num6++;
						if (num4 <= 0)
						{
							num4 = minerPool[k].period;
							num5 = minerPool[k].speed;
						}
					}
				}
				if (num4 <= 0 || num6 <= 0)
				{
					continue;
				}
				double num8 = 3600.0 / (double)num4 * num2 * (double)num5 * (double)num3 * (double)VeinData.oilSpeedMultiplier * (double)num6;
				int num9 = val.productIndices[item];
				if (num9 > 0 && num9 < val.productPool.Length)
				{
					ProductStat val2 = val.productPool[num9];
					if (val2 != null)
					{
						val2.refProductSpeed = (float)num8;
					}
				}
			}
			OilRefSpeedPatchState.OilProductIdsThisFactory = null;
		}
		catch (Exception arg)
		{
			OilRefSpeedPatchState.OilProductIdsThisFactory = null;
			ManualLogSource? log = Plugin.Log;
			if (log != null)
			{
				log.LogError((object)$"[PlanetwideMiner] ProductionExtraInfoCalculatorFactoryPatch 异常: {arg}");
			}
		}
	}
}
