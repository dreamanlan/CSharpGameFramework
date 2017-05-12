using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_PreferBinarySerialization : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.PreferBinarySerialization o;
			o=new UnityEngine.PreferBinarySerialization();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.PreferBinarySerialization");
		createTypeMetatable(l,constructor, typeof(UnityEngine.PreferBinarySerialization),typeof(System.Attribute));
	}
}
