using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Camera_MonoOrStereoscopicEye : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.Camera.MonoOrStereoscopicEye");
		addMember(l,0,"Left");
		addMember(l,1,"Right");
		addMember(l,2,"Mono");
		LuaDLL.lua_pop(l, 1);
	}
}
