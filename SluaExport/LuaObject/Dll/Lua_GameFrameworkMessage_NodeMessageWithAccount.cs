using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_NodeMessageWithAccount : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.NodeMessageWithAccount o;
			o=new GameFrameworkMessage.NodeMessageWithAccount();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Account(IntPtr l) {
		try {
			GameFrameworkMessage.NodeMessageWithAccount self=(GameFrameworkMessage.NodeMessageWithAccount)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Account);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Account(IntPtr l) {
		try {
			GameFrameworkMessage.NodeMessageWithAccount self=(GameFrameworkMessage.NodeMessageWithAccount)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Account=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.NodeMessageWithAccount");
		addMember(l,"m_Account",get_m_Account,set_m_Account,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.NodeMessageWithAccount));
	}
}
