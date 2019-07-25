using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_VersionVerify : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.VersionVerify o;
			o=new GameFrameworkMessage.VersionVerify();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.VersionVerify");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.VersionVerify));
	}
}
