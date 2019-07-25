using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_UserHeartbeat : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.UserHeartbeat o;
			o=new GameFrameworkMessage.UserHeartbeat();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.UserHeartbeat");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.UserHeartbeat));
	}
}
