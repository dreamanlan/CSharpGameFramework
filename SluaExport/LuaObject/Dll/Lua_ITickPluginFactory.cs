using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ITickPluginFactory : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateInstance(IntPtr l) {
		try {
			ITickPluginFactory self=(ITickPluginFactory)checkSelf(l);
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
		getTypeTable(l,"ITickPluginFactory");
		addMember(l,CreateInstance);
		createTypeMetatable(l,null, typeof(ITickPluginFactory));
	}
}
