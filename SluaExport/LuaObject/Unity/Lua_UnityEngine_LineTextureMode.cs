using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_LineTextureMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.LineTextureMode");
		addMember(l,0,"Stretch");
		addMember(l,1,"Tile");
		LuaDLL.lua_pop(l, 1);
	}
}
