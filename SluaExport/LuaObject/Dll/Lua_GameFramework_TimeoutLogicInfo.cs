using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_TimeoutLogicInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.TimeoutLogicInfo o;
			o=new GameFramework.TimeoutLogicInfo();
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
	static public int get_m_Timeout(IntPtr l) {
		try {
			GameFramework.TimeoutLogicInfo self=(GameFramework.TimeoutLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Timeout);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Timeout(IntPtr l) {
		try {
			GameFramework.TimeoutLogicInfo self=(GameFramework.TimeoutLogicInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.m_Timeout=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_CurTime(IntPtr l) {
		try {
			GameFramework.TimeoutLogicInfo self=(GameFramework.TimeoutLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_CurTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_CurTime(IntPtr l) {
		try {
			GameFramework.TimeoutLogicInfo self=(GameFramework.TimeoutLogicInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.m_CurTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_IsTriggered(IntPtr l) {
		try {
			GameFramework.TimeoutLogicInfo self=(GameFramework.TimeoutLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_IsTriggered);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_IsTriggered(IntPtr l) {
		try {
			GameFramework.TimeoutLogicInfo self=(GameFramework.TimeoutLogicInfo)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.m_IsTriggered=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.TimeoutLogicInfo");
		addMember(l,ctor_s);
		addMember(l,"m_Timeout",get_m_Timeout,set_m_Timeout,true);
		addMember(l,"m_CurTime",get_m_CurTime,set_m_CurTime,true);
		addMember(l,"m_IsTriggered",get_m_IsTriggered,set_m_IsTriggered,true);
		createTypeMetatable(l,null, typeof(GameFramework.TimeoutLogicInfo));
	}
}
