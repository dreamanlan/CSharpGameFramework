using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_GmQueryAccount : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmQueryAccount o;
			o=new GameFrameworkMessage.Msg_CL_GmQueryAccount();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_QueryAccount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmQueryAccount self=(GameFrameworkMessage.Msg_CL_GmQueryAccount)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_QueryAccount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_QueryAccount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmQueryAccount self=(GameFrameworkMessage.Msg_CL_GmQueryAccount)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_QueryAccount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmQueryAccount");
		addMember(l,"m_QueryAccount",get_m_QueryAccount,set_m_QueryAccount,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_GmQueryAccount));
	}
}
