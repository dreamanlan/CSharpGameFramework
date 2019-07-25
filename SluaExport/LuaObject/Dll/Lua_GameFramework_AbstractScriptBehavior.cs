using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_AbstractScriptBehavior : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ResourceEnabled(IntPtr l) {
		try {
			GameFramework.AbstractScriptBehavior self=(GameFramework.AbstractScriptBehavior)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ResourceEnabled);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ResourceEnabled(IntPtr l) {
		try {
			GameFramework.AbstractScriptBehavior self=(GameFramework.AbstractScriptBehavior)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.ResourceEnabled=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AbstractScriptBehavior");
		addMember(l,"ResourceEnabled",get_ResourceEnabled,set_ResourceEnabled,true);
		createTypeMetatable(l,null, typeof(GameFramework.AbstractScriptBehavior),typeof(UnityEngine.MonoBehaviour));
	}
}
