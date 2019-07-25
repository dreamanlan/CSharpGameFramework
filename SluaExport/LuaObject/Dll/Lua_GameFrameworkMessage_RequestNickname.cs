using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_RequestNickname : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.RequestNickname o;
			o=new GameFrameworkMessage.RequestNickname();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.RequestNickname");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.RequestNickname));
	}
}
