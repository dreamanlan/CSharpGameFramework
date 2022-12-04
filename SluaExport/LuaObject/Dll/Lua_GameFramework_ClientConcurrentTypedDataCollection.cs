using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_ClientConcurrentTypedDataCollection : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.ClientConcurrentTypedDataCollection o;
			o=new GameFramework.ClientConcurrentTypedDataCollection();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clear(IntPtr l) {
		try {
			GameFramework.ClientConcurrentTypedDataCollection self=(GameFramework.ClientConcurrentTypedDataCollection)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientConcurrentTypedDataCollection");
		addMember(l,ctor_s);
		addMember(l,Clear);
		createTypeMetatable(l,null, typeof(GameFramework.ClientConcurrentTypedDataCollection));
	}
}
