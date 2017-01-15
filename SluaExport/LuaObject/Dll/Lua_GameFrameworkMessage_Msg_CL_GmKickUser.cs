using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_GmKickUser : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmKickUser o;
			o=new GameFrameworkMessage.Msg_CL_GmKickUser();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmKickUser self=(GameFrameworkMessage.Msg_CL_GmKickUser)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmKickUser self=(GameFrameworkMessage.Msg_CL_GmKickUser)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Nickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_LockMinutes(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmKickUser self=(GameFrameworkMessage.Msg_CL_GmKickUser)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_LockMinutes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_LockMinutes(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmKickUser self=(GameFrameworkMessage.Msg_CL_GmKickUser)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_LockMinutes=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmKickUser");
		addMember(l,"m_Nickname",get_m_Nickname,set_m_Nickname,true);
		addMember(l,"m_LockMinutes",get_m_LockMinutes,set_m_LockMinutes,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_GmKickUser));
	}
}
