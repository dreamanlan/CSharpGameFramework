using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Camera_StereoscopicEye : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.Camera.StereoscopicEye");
		addMember(l,0,"Left");
		addMember(l,1,"Right");
		LuaDLL.lua_pop(l, 1);
	}
}
