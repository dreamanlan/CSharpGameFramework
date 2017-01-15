using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SandClockLogicInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.SandClockLogicInfo o;
			o=new GameFramework.SandClockLogicInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_LastHour(IntPtr l) {
		try {
			GameFramework.SandClockLogicInfo self=(GameFramework.SandClockLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_LastHour);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_LastHour(IntPtr l) {
		try {
			GameFramework.SandClockLogicInfo self=(GameFramework.SandClockLogicInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_LastHour=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_LastMinute(IntPtr l) {
		try {
			GameFramework.SandClockLogicInfo self=(GameFramework.SandClockLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_LastMinute);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_LastMinute(IntPtr l) {
		try {
			GameFramework.SandClockLogicInfo self=(GameFramework.SandClockLogicInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_LastMinute=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SandClockLogicInfo");
		addMember(l,"m_LastHour",get_m_LastHour,set_m_LastHour,true);
		addMember(l,"m_LastMinute",get_m_LastMinute,set_m_LastMinute,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.SandClockLogicInfo));
	}
}
