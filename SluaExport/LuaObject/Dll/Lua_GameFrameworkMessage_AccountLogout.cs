using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_AccountLogout : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLogout o;
			o=new GameFrameworkMessage.AccountLogout();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.AccountLogout");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.AccountLogout));
	}
}
