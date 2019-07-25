using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_DirectLogin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.DirectLogin o;
			o=new GameFrameworkMessage.DirectLogin();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.DirectLogin");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.DirectLogin));
	}
}
