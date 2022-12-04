using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_NodeMessageWithAccount : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.NodeMessageWithAccount");
		addMember(l,ctor_s);
		addMember(l,"m_Account",get_m_Account,set_m_Account,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.NodeMessageWithAccount));
	}
}
