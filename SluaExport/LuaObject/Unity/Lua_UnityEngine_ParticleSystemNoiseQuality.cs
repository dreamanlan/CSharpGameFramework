using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_ParticleSystemNoiseQuality : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.ParticleSystemNoiseQuality");
		addMember(l,0,"Low");
		addMember(l,1,"Medium");
		addMember(l,2,"High");
		LuaDLL.lua_pop(l, 1);
	}
}
