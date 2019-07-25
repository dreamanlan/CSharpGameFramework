using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_CombatStatisticInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo o;
			o=new GameFramework.CombatStatisticInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddDeadCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddDeadCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddKillHeroCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddKillHeroCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddKillNpcCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddKillNpcCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearContinueKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			self.ClearContinueKillCount();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddContinueKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddContinueKillCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearContinueDeadCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			self.ClearContinueDeadCount();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddContinueDeadCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddContinueDeadCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearMultiKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			self.ClearMultiKillCount();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddMultiKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddMultiKillCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddKillTowerCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddKillTowerCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddTotalDamageFromMyself(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddTotalDamageFromMyself(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddTotalDamageToMyself(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.AddTotalDamageToMyself(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DeadCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DeadCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DeadCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.DeadCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_KillHeroCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KillHeroCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_KillHeroCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.KillHeroCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_KillNpcCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KillNpcCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_KillNpcCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.KillNpcCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ContinueKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ContinueKillCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ContinueKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ContinueKillCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ContinueDeadCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ContinueDeadCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ContinueDeadCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ContinueDeadCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MultiKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MultiKillCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MultiKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.MultiKillCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MaxContinueKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MaxContinueKillCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MaxContinueKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.MaxContinueKillCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MaxMultiKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MaxMultiKillCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MaxMultiKillCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.MaxMultiKillCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_KillTowerCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KillTowerCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_KillTowerCount(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.KillTowerCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TotalDamageFromMyself(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TotalDamageFromMyself);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_TotalDamageFromMyself(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.TotalDamageFromMyself=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TotalDamageToMyself(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TotalDamageToMyself);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_TotalDamageToMyself(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.TotalDamageToMyself=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DataChanged(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DataChanged);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DataChanged(IntPtr l) {
		try {
			GameFramework.CombatStatisticInfo self=(GameFramework.CombatStatisticInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.DataChanged=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.CombatStatisticInfo");
		addMember(l,AddDeadCount);
		addMember(l,AddKillHeroCount);
		addMember(l,AddKillNpcCount);
		addMember(l,ClearContinueKillCount);
		addMember(l,AddContinueKillCount);
		addMember(l,ClearContinueDeadCount);
		addMember(l,AddContinueDeadCount);
		addMember(l,ClearMultiKillCount);
		addMember(l,AddMultiKillCount);
		addMember(l,AddKillTowerCount);
		addMember(l,AddTotalDamageFromMyself);
		addMember(l,AddTotalDamageToMyself);
		addMember(l,Reset);
		addMember(l,"DeadCount",get_DeadCount,set_DeadCount,true);
		addMember(l,"KillHeroCount",get_KillHeroCount,set_KillHeroCount,true);
		addMember(l,"KillNpcCount",get_KillNpcCount,set_KillNpcCount,true);
		addMember(l,"ContinueKillCount",get_ContinueKillCount,set_ContinueKillCount,true);
		addMember(l,"ContinueDeadCount",get_ContinueDeadCount,set_ContinueDeadCount,true);
		addMember(l,"MultiKillCount",get_MultiKillCount,set_MultiKillCount,true);
		addMember(l,"MaxContinueKillCount",get_MaxContinueKillCount,set_MaxContinueKillCount,true);
		addMember(l,"MaxMultiKillCount",get_MaxMultiKillCount,set_MaxMultiKillCount,true);
		addMember(l,"KillTowerCount",get_KillTowerCount,set_KillTowerCount,true);
		addMember(l,"TotalDamageFromMyself",get_TotalDamageFromMyself,set_TotalDamageFromMyself,true);
		addMember(l,"TotalDamageToMyself",get_TotalDamageToMyself,set_TotalDamageToMyself,true);
		addMember(l,"DataChanged",get_DataChanged,set_DataChanged,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.CombatStatisticInfo));
	}
}
