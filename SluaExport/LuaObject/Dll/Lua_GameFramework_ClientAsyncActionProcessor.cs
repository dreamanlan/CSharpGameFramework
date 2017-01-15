using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ClientAsyncActionProcessor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.ClientAsyncActionProcessor o;
			o=new GameFramework.ClientAsyncActionProcessor();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientAsyncActionProcessor");
		createTypeMetatable(l,constructor, typeof(GameFramework.ClientAsyncActionProcessor),typeof(GameFramework.ClientConcurrentActionProcessor));
	}
}
