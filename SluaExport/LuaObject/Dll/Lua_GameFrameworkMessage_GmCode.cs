using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_GmCode : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.GmCode o;
			o=new GameFrameworkMessage.GmCode();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Content(IntPtr l) {
		try {
			GameFrameworkMessage.GmCode self=(GameFrameworkMessage.GmCode)checkSelf(l);
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
			GameFrameworkMessage.GmCode self=(GameFrameworkMessage.GmCode)checkSelf(l);
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
		getTypeTable(l,"GameFrameworkMessage.GmCode");
		addMember(l,"m_Content",get_m_Content,set_m_Content,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.GmCode));
	}
}
