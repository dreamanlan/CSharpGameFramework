using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_GmQueryUserByNickname : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmQueryUserByNickname o;
			o=new GameFrameworkMessage.Msg_CL_GmQueryUserByNickname();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_QueryNickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmQueryUserByNickname self=(GameFrameworkMessage.Msg_CL_GmQueryUserByNickname)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_QueryNickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_QueryNickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmQueryUserByNickname self=(GameFrameworkMessage.Msg_CL_GmQueryUserByNickname)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_QueryNickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmQueryUserByNickname");
		addMember(l,"m_QueryNickname",get_m_QueryNickname,set_m_QueryNickname,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_GmQueryUserByNickname));
	}
}
