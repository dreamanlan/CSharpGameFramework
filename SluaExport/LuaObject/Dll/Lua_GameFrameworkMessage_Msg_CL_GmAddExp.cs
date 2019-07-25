using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_GmAddExp : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmAddExp o;
			o=new GameFrameworkMessage.Msg_CL_GmAddExp();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Nick(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmAddExp self=(GameFrameworkMessage.Msg_CL_GmAddExp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nick);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Nick(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmAddExp self=(GameFrameworkMessage.Msg_CL_GmAddExp)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Nick=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Exp(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmAddExp self=(GameFrameworkMessage.Msg_CL_GmAddExp)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Exp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Exp(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmAddExp self=(GameFrameworkMessage.Msg_CL_GmAddExp)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Exp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmAddExp");
		addMember(l,"m_Nick",get_m_Nick,set_m_Nick,true);
		addMember(l,"m_Exp",get_m_Exp,set_m_Exp,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_GmAddExp));
	}
}
