using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Experimental_Director_AudioPlayableGraphExtensions : LuaObject {
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Experimental.Director.AudioPlayableGraphExtensions");
		createTypeMetatable(l,null, typeof(UnityEngine.Experimental.Director.AudioPlayableGraphExtensions));
	}
}
