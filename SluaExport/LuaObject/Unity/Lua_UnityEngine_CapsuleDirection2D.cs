using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_CapsuleDirection2D : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.CapsuleDirection2D");
		addMember(l,0,"Vertical");
		addMember(l,1,"Horizontal");
		LuaDLL.lua_pop(l, 1);
	}
}
