using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_NodeRegister : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.NodeRegister o;
			o=new GameFrameworkMessage.NodeRegister();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Name(IntPtr l) {
		try {
			GameFrameworkMessage.NodeRegister self=(GameFrameworkMessage.NodeRegister)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Name(IntPtr l) {
		try {
			GameFrameworkMessage.NodeRegister self=(GameFrameworkMessage.NodeRegister)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.NodeRegister");
		addMember(l,"m_Name",get_m_Name,set_m_Name,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.NodeRegister));
	}
}
