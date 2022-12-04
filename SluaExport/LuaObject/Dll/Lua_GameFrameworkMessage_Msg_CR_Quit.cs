using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CR_Quit : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Quit o;
			o=new GameFrameworkMessage.Msg_CR_Quit();
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
	static public int get_is_force(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Quit self=(GameFrameworkMessage.Msg_CR_Quit)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.is_force);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_is_force(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Quit self=(GameFrameworkMessage.Msg_CR_Quit)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.is_force=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_Quit");
		addMember(l,ctor_s);
		addMember(l,"is_force",get_is_force,set_is_force,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CR_Quit));
	}
}
