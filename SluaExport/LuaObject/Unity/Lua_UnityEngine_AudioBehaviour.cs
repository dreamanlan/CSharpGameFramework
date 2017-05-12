using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_AudioBehaviour : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.AudioBehaviour o;
			o=new UnityEngine.AudioBehaviour();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.AudioBehaviour");
		createTypeMetatable(l,constructor, typeof(UnityEngine.AudioBehaviour),typeof(UnityEngine.Behaviour));
	}
}
