using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_SwitchDebug : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_SwitchDebug o;
			o=new GameFrameworkMessage.Msg_CR_SwitchDebug();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_is_debug(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_SwitchDebug self=(GameFrameworkMessage.Msg_CR_SwitchDebug)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.is_debug);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_is_debug(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_SwitchDebug self=(GameFrameworkMessage.Msg_CR_SwitchDebug)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.is_debug=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_SwitchDebug");
		addMember(l,"is_debug",get_is_debug,set_is_debug,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_SwitchDebug));
	}
}
