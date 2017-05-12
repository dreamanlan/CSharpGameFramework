using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_LineAlignment : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.LineAlignment");
		addMember(l,0,"View");
		addMember(l,1,"Local");
		LuaDLL.lua_pop(l, 1);
	}
}
