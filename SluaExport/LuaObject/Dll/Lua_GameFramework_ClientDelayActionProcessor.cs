using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ClientDelayActionProcessor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.ClientDelayActionProcessor o;
			o=new GameFramework.ClientDelayActionProcessor();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientDelayActionProcessor");
		createTypeMetatable(l,constructor, typeof(GameFramework.ClientDelayActionProcessor),typeof(GameFramework.ClientConcurrentActionProcessor));
	}
}
