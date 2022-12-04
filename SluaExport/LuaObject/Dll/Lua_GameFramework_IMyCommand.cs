using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_IMyCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Execute(IntPtr l) {
		try {
			GameFramework.IMyCommand self=(GameFramework.IMyCommand)checkSelf(l);
			var ret=self.Execute();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.IMyCommand");
		addMember(l,Execute);
		createTypeMetatable(l,null, typeof(GameFramework.IMyCommand));
	}
}
