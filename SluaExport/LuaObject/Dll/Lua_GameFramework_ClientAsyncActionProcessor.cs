using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_ClientAsyncActionProcessor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientAsyncActionProcessor");
		addMember(l,ctor_s);
		createTypeMetatable(l,null, typeof(GameFramework.ClientAsyncActionProcessor),typeof(GameFramework.ClientConcurrentActionProcessor));
	}
}
