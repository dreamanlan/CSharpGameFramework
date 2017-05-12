using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_SpriteDrawMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.SpriteDrawMode");
		addMember(l,0,"Simple");
		addMember(l,1,"Sliced");
		addMember(l,2,"Tiled");
		LuaDLL.lua_pop(l, 1);
	}
}
