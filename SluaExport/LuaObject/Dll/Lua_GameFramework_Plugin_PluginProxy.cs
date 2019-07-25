using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Plugin_PluginProxy : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NativeProxy(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Plugin.PluginProxy.NativeProxy);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_NativeProxy(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy v;
			checkType(l,2,out v);
			GameFramework.Plugin.PluginProxy.NativeProxy=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LuaProxy(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Plugin.PluginProxy.LuaProxy);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LuaProxy(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy v;
			checkType(l,2,out v);
			GameFramework.Plugin.PluginProxy.LuaProxy=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.PluginProxy");
		addMember(l,"NativeProxy",get_NativeProxy,set_NativeProxy,false);
		addMember(l,"LuaProxy",get_LuaProxy,set_LuaProxy,false);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.PluginProxy));
	}
}
