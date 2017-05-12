using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_ParticleSystemSubEmitterProperties : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.ParticleSystemSubEmitterProperties");
		addMember(l,0,"InheritNothing");
		addMember(l,1,"InheritColor");
		addMember(l,2,"InheritSize");
		addMember(l,4,"InheritRotation");
		addMember(l,7,"InheritEverything");
		LuaDLL.lua_pop(l, 1);
	}
}
