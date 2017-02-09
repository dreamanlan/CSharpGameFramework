using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_IStartupPluginFactory : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateInstance(IntPtr l) {
		try {
			IStartupPluginFactory self=(IStartupPluginFactory)checkSelf(l);
			var ret=self.CreateInstance();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"IStartupPluginFactory");
		addMember(l,CreateInstance);
		createTypeMetatable(l,null, typeof(IStartupPluginFactory));
	}
}
