using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_GmUpdateMaxUserCount : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount o;
			o=new GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_MaxUserCount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount self=(GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_MaxUserCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_MaxUserCount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount self=(GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_MaxUserCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_MaxQueueingCount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount self=(GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_MaxQueueingCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_MaxQueueingCount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount self=(GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_MaxQueueingCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount");
		addMember(l,"m_MaxUserCount",get_m_MaxUserCount,set_m_MaxUserCount,true);
		addMember(l,"m_MaxQueueingCount",get_m_MaxQueueingCount,set_m_MaxQueueingCount,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_GmUpdateMaxUserCount));
	}
}
