﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_PreferBinarySerialization : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.PreferBinarySerialization");
		addMember(l,ctor_s);
		createTypeMetatable(l,null, typeof(UnityEngine.PreferBinarySerialization),typeof(System.Attribute));
	}
}
