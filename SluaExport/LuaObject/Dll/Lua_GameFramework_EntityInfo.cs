using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_EntityInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.EntityInfo o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new GameFramework.EntityInfo(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int InitBase(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.InitBase(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetUnitId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetUnitId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetUnitId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetUnitId(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetTableId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetTableId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetTableId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetTableId(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetName(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetName(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetName(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetName();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int HaveState(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			var ret=self.HaveState(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddState(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			self.AddState(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveState(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			self.RemoveState(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DisableState(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			self.DisableState(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearState(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			self.ClearState(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int HaveAnyState(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.CharacterPropertyEnum[] a1;
			checkValueParams(l,2,out a1);
			var ret=self.HaveAnyState(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetModel(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetModel();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetModel(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetModel(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetAIEnable(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetAIEnable();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetAIEnable(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.SetAIEnable(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetCampId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetCampId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetCampId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetCampId(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetRadius(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetRadius();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsHaveStoryFlag(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.StoryListenFlagEnum a1;
			checkEnum(l,2,out a1);
			var ret=self.IsHaveStoryFlag(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddStoryFlag(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.StoryListenFlagEnum a1;
			checkEnum(l,2,out a1);
			self.AddStoryFlag(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveStoryFlag(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.StoryListenFlagEnum a1;
			checkEnum(l,2,out a1);
			self.RemoveStoryFlag(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsDead(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.IsDead();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsDeadSkillCasting(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.IsDeadSkillCasting();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ResetAttackerInfo(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			self.ResetAttackerInfo();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RetireAttackerInfos(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			self.RetireAttackerInfos(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetAttackerInfo(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			System.Int32 a6;
			checkType(l,7,out a6);
			self.SetAttackerInfo(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetAttackTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			self.SetAttackTime();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CopyBaseAttr(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			self.CopyBaseAttr();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetMovementStateInfo(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetMovementStateInfo();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSkillStateInfo(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetSkillStateInfo();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetCombatStatisticInfo(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetCombatStatisticInfo();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsUnderControl(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.IsUnderControl();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetCanUseSkill(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.SetCanUseSkill(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CanUseSkill(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.CanUseSkill();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int InitId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.InitId(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadData__Actor(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			TableConfig.Actor a1;
			checkType(l,2,out a1);
			self.LoadData(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadData__Int32__Int32__Actor__String__A_String(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			TableConfig.Actor a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.String[] a5;
			checkParams(l,6,out a5);
			self.LoadData(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReloadData(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			TableConfig.Actor a1;
			checkType(l,2,out a1);
			self.ReloadData(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetAiStateInfo(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.GetAiStateInfo();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsCombatNpc(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.IsCombatNpc();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsTargetNpc(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			var ret=self.IsTargetNpc();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSkillEvent(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.GetSkillEvent(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetRelation__EntityInfo__EntityInfo_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			GameFramework.EntityInfo a2;
			checkType(l,2,out a2);
			var ret=GameFramework.EntityInfo.GetRelation(a1,a2);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetRelation__Int32__Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=GameFramework.EntityInfo.GetRelation(a1,a2);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CanSee__EntityInfo__EntityInfo_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			GameFramework.EntityInfo a2;
			checkType(l,2,out a2);
			var ret=GameFramework.EntityInfo.CanSee(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CanSee__EntityInfo__EntityInfo__Single__Vector3__Vector3_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			GameFramework.EntityInfo a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Vector3 a4;
			checkValueType(l,4,out a4);
			ScriptRuntime.Vector3 a5;
			checkValueType(l,5,out a5);
			var ret=GameFramework.EntityInfo.CanSee(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_StartUserUnitId(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.EntityInfo.c_StartUserUnitId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UniqueId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.UniqueId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_UniqueId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.UniqueId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsControlByStory(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsControlByStory);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsControlByStory(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsControlByStory=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsControlByManual(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsControlByManual);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsControlByManual(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsControlByManual=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DeadTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DeadTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DeadTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.DeadTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DeadTimeout(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DeadTimeout);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Level(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Level(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Exp(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Exp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Exp(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Exp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Hp(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Hp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Hp(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Hp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_HpMax(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HpMax);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Energy(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Energy);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Energy(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Energy=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EnergyMax(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EnergyMax);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Shield(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Shield);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Shield(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Shield=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Speed(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Speed);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Speed(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.Speed=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ViewRange(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ViewRange);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ViewRange(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.ViewRange=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GohomeRange(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GohomeRange);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_GohomeRange(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.GohomeRange=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_KillerId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KillerId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_KillerId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.KillerId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LastAttackedTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LastAttackedTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LastAttackedTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.LastAttackedTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LastAttackTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LastAttackTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LastAttackTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.LastAttackTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StoryListenFlag(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StoryListenFlag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StoryListenFlag(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.StoryListenFlag=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BaseProperty(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BaseProperty);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ActualProperty(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ActualProperty);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LevelChanged(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LevelChanged);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LevelChanged(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.LevelChanged=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_PropertyChanged(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PropertyChanged);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_PropertyChanged(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.PropertyChanged=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SceneContext(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneContext);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SceneContext(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			GameFramework.SceneContextInfo v;
			checkType(l,2,out v);
			self.SceneContext=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SceneLogicInfoManager(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneLogicInfoManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EntityManager(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EntityManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BlackBoard(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BlackBoard);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_OwnerId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OwnerId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OwnerId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.OwnerId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SummonerId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SummonerId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SummonerId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.SummonerId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SummonSkillId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SummonSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SummonSkillId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.SummonSkillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CustomData(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CustomData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_CustomData(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.CustomData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EntityType(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EntityType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_EntityType(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.EntityType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Scale(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Scale);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CanMove(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CanMove);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CanHitMove(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CanHitMove);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CanRotate(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CanRotate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CanDead(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CanDead);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_CanDead(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.CanDead=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsPassive(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsPassive);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsPassive(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsPassive=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsServerEntity(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsServerEntity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsServerEntity(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsServerEntity=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CreatorId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CreatorId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_CreatorId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.CreatorId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsBorning(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsBorning);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsBorning(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsBorning=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BornTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BornTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_BornTime(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.BornTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BornTimeout(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BornTimeout);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ReliveTimeout(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ReliveTimeout);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NeedDelete(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NeedDelete);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_NeedDelete(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.NeedDelete=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BornSkillId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BornSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DeadSkillId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DeadSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NormalSkillId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NormalSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ManualSkillId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ManualSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ManualSkillId(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ManualSkillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DropMoney(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DropMoney);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DropMoney(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.DropMoney=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LevelMonsterData(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LevelMonsterData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LevelMonsterData(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			TableConfig.LevelMonster v;
			checkType(l,2,out v);
			self.LevelMonsterData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ConfigData(IntPtr l) {
		try {
			GameFramework.EntityInfo self=(GameFramework.EntityInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ConfigData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.EntityInfo");
		addMember(l,ctor_s);
		addMember(l,InitBase);
		addMember(l,GetId);
		addMember(l,GetUnitId);
		addMember(l,SetUnitId);
		addMember(l,GetTableId);
		addMember(l,SetTableId);
		addMember(l,SetName);
		addMember(l,GetName);
		addMember(l,HaveState);
		addMember(l,AddState);
		addMember(l,RemoveState);
		addMember(l,DisableState);
		addMember(l,ClearState);
		addMember(l,HaveAnyState);
		addMember(l,GetModel);
		addMember(l,SetModel);
		addMember(l,GetAIEnable);
		addMember(l,SetAIEnable);
		addMember(l,GetCampId);
		addMember(l,SetCampId);
		addMember(l,GetRadius);
		addMember(l,IsHaveStoryFlag);
		addMember(l,AddStoryFlag);
		addMember(l,RemoveStoryFlag);
		addMember(l,IsDead);
		addMember(l,IsDeadSkillCasting);
		addMember(l,ResetAttackerInfo);
		addMember(l,RetireAttackerInfos);
		addMember(l,SetAttackerInfo);
		addMember(l,SetAttackTime);
		addMember(l,CopyBaseAttr);
		addMember(l,GetMovementStateInfo);
		addMember(l,GetSkillStateInfo);
		addMember(l,GetCombatStatisticInfo);
		addMember(l,IsUnderControl);
		addMember(l,SetCanUseSkill);
		addMember(l,CanUseSkill);
		addMember(l,InitId);
		addMember(l,Reset);
		addMember(l,LoadData__Actor);
		addMember(l,LoadData__Int32__Int32__Actor__String__A_String);
		addMember(l,ReloadData);
		addMember(l,GetAiStateInfo);
		addMember(l,IsCombatNpc);
		addMember(l,IsTargetNpc);
		addMember(l,GetSkillEvent);
		addMember(l,GetRelation__EntityInfo__EntityInfo_s);
		addMember(l,GetRelation__Int32__Int32_s);
		addMember(l,CanSee__EntityInfo__EntityInfo_s);
		addMember(l,CanSee__EntityInfo__EntityInfo__Single__Vector3__Vector3_s);
		addMember(l,"c_StartUserUnitId",get_c_StartUserUnitId,null,false);
		addMember(l,"UniqueId",get_UniqueId,set_UniqueId,true);
		addMember(l,"IsControlByStory",get_IsControlByStory,set_IsControlByStory,true);
		addMember(l,"IsControlByManual",get_IsControlByManual,set_IsControlByManual,true);
		addMember(l,"DeadTime",get_DeadTime,set_DeadTime,true);
		addMember(l,"DeadTimeout",get_DeadTimeout,null,true);
		addMember(l,"Level",get_Level,set_Level,true);
		addMember(l,"Exp",get_Exp,set_Exp,true);
		addMember(l,"Hp",get_Hp,set_Hp,true);
		addMember(l,"HpMax",get_HpMax,null,true);
		addMember(l,"Energy",get_Energy,set_Energy,true);
		addMember(l,"EnergyMax",get_EnergyMax,null,true);
		addMember(l,"Shield",get_Shield,set_Shield,true);
		addMember(l,"Speed",get_Speed,set_Speed,true);
		addMember(l,"ViewRange",get_ViewRange,set_ViewRange,true);
		addMember(l,"GohomeRange",get_GohomeRange,set_GohomeRange,true);
		addMember(l,"KillerId",get_KillerId,set_KillerId,true);
		addMember(l,"LastAttackedTime",get_LastAttackedTime,set_LastAttackedTime,true);
		addMember(l,"LastAttackTime",get_LastAttackTime,set_LastAttackTime,true);
		addMember(l,"StoryListenFlag",get_StoryListenFlag,set_StoryListenFlag,true);
		addMember(l,"BaseProperty",get_BaseProperty,null,true);
		addMember(l,"ActualProperty",get_ActualProperty,null,true);
		addMember(l,"LevelChanged",get_LevelChanged,set_LevelChanged,true);
		addMember(l,"PropertyChanged",get_PropertyChanged,set_PropertyChanged,true);
		addMember(l,"SceneContext",get_SceneContext,set_SceneContext,true);
		addMember(l,"SceneLogicInfoManager",get_SceneLogicInfoManager,null,true);
		addMember(l,"EntityManager",get_EntityManager,null,true);
		addMember(l,"BlackBoard",get_BlackBoard,null,true);
		addMember(l,"OwnerId",get_OwnerId,set_OwnerId,true);
		addMember(l,"SummonerId",get_SummonerId,set_SummonerId,true);
		addMember(l,"SummonSkillId",get_SummonSkillId,set_SummonSkillId,true);
		addMember(l,"CustomData",get_CustomData,set_CustomData,true);
		addMember(l,"EntityType",get_EntityType,set_EntityType,true);
		addMember(l,"Scale",get_Scale,null,true);
		addMember(l,"CanMove",get_CanMove,null,true);
		addMember(l,"CanHitMove",get_CanHitMove,null,true);
		addMember(l,"CanRotate",get_CanRotate,null,true);
		addMember(l,"CanDead",get_CanDead,set_CanDead,true);
		addMember(l,"IsPassive",get_IsPassive,set_IsPassive,true);
		addMember(l,"IsServerEntity",get_IsServerEntity,set_IsServerEntity,true);
		addMember(l,"CreatorId",get_CreatorId,set_CreatorId,true);
		addMember(l,"IsBorning",get_IsBorning,set_IsBorning,true);
		addMember(l,"BornTime",get_BornTime,set_BornTime,true);
		addMember(l,"BornTimeout",get_BornTimeout,null,true);
		addMember(l,"ReliveTimeout",get_ReliveTimeout,null,true);
		addMember(l,"NeedDelete",get_NeedDelete,set_NeedDelete,true);
		addMember(l,"BornSkillId",get_BornSkillId,null,true);
		addMember(l,"DeadSkillId",get_DeadSkillId,null,true);
		addMember(l,"NormalSkillId",get_NormalSkillId,null,true);
		addMember(l,"ManualSkillId",get_ManualSkillId,set_ManualSkillId,true);
		addMember(l,"DropMoney",get_DropMoney,set_DropMoney,true);
		addMember(l,"LevelMonsterData",get_LevelMonsterData,set_LevelMonsterData,true);
		addMember(l,"ConfigData",get_ConfigData,null,true);
		createTypeMetatable(l,null, typeof(GameFramework.EntityInfo));
	}
}
