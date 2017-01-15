using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_GmAddExp : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmAddExp o;
			o=new GameFrameworkMessage.Msg_LC_GmAddExp();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmAddExp self=(GameFrameworkMessage.Msg_LC_GmAddExp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmAddExp self=(GameFrameworkMessage.Msg_LC_GmAddExp)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Result=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_GmAddExp");
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_GmAddExp));
	}
}
