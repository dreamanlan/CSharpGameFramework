using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Analytics_Gender : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.Analytics.Gender");
		addMember(l,0,"Male");
		addMember(l,1,"Female");
		addMember(l,2,"Unknown");
		LuaDLL.lua_pop(l, 1);
	}
}
