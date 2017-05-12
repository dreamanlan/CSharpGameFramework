using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Video_VideoAudioOutputMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.Video.VideoAudioOutputMode");
		addMember(l,0,"None");
		addMember(l,1,"AudioSource");
		addMember(l,2,"Direct");
		LuaDLL.lua_pop(l, 1);
	}
}
