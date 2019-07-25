using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Logout : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Logout o;
			o=new GameFrameworkMessage.Logout();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Logout");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Logout));
	}
}
