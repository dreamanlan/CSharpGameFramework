using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_LightProbeProxyVolume_ResolutionMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.LightProbeProxyVolume.ResolutionMode");
		addMember(l,0,"Automatic");
		addMember(l,1,"Custom");
		LuaDLL.lua_pop(l, 1);
	}
}
