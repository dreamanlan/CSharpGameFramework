using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_GmCode : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmCode o;
			o=new GameFrameworkMessage.Msg_CL_GmCode();
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
			GameFrameworkMessage.Msg_CL_GmCode self=(GameFrameworkMessage.Msg_CL_GmCode)checkSelf(l);
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
			GameFrameworkMessage.Msg_CL_GmCode self=(GameFrameworkMessage.Msg_CL_GmCode)checkSelf(l);
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
	static public int get_m_Content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmCode self=(GameFrameworkMessage.Msg_CL_GmCode)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Content);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmCode self=(GameFrameworkMessage.Msg_CL_GmCode)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Content=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmCode");
		addMember(l,"m_Nick",get_m_Nick,set_m_Nick,true);
		addMember(l,"m_Content",get_m_Content,set_m_Content,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_GmCode));
	}
}
