using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_SkillInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor__Int32_s(IntPtr l) {
		try {
			GameFramework.SkillInfo o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new GameFramework.SkillInfo(a1);
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
	static public int ctor__Skill_s(IntPtr l) {
		try {
			GameFramework.SkillInfo o;
			TableConfig.Skill a1;
			checkType(l,1,out a1);
			o=new GameFramework.SkillInfo(a1);
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
	static public int Reset(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
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
	static public int AddCD(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			self.AddCD(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetCD(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			var ret=self.GetCD(a1);
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
	static public int IsInCd(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			var ret=self.IsInCd(a1);
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
	static public int Refresh(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			self.Refresh();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SkillId(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SkillId(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.SkillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SkillLevel(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillLevel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SkillLevel(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.SkillLevel=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsSkillActivated(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsSkillActivated);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsSkillActivated(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.IsSkillActivated=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CdEndTime(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CdEndTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_CdEndTime(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.CdEndTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ManualSkillId(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
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
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int32 v;
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
	static public int get_m_SkillCDRefreshCount(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_SkillCDRefreshCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_SkillCDRefreshCount(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_SkillCDRefreshCount=v;
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
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ConfigData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ConfigData(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			TableConfig.Skill v;
			checkType(l,2,out v);
			self.ConfigData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TargetType(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.TargetType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Distance(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Distance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_InterruptPriority(IntPtr l) {
		try {
			GameFramework.SkillInfo self=(GameFramework.SkillInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.InterruptPriority);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SkillInfo");
		addMember(l,ctor__Int32_s);
		addMember(l,ctor__Skill_s);
		addMember(l,Reset);
		addMember(l,AddCD);
		addMember(l,GetCD);
		addMember(l,IsInCd);
		addMember(l,Refresh);
		addMember(l,"SkillId",get_SkillId,set_SkillId,true);
		addMember(l,"SkillLevel",get_SkillLevel,set_SkillLevel,true);
		addMember(l,"IsSkillActivated",get_IsSkillActivated,set_IsSkillActivated,true);
		addMember(l,"CdEndTime",get_CdEndTime,set_CdEndTime,true);
		addMember(l,"ManualSkillId",get_ManualSkillId,set_ManualSkillId,true);
		addMember(l,"m_SkillCDRefreshCount",get_m_SkillCDRefreshCount,set_m_SkillCDRefreshCount,true);
		addMember(l,"ConfigData",get_ConfigData,set_ConfigData,true);
		addMember(l,"TargetType",get_TargetType,null,true);
		addMember(l,"Distance",get_Distance,null,true);
		addMember(l,"InterruptPriority",get_InterruptPriority,null,true);
		createTypeMetatable(l,null, typeof(GameFramework.SkillInfo));
	}
}
