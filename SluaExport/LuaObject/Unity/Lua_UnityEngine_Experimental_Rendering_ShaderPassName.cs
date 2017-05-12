using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Experimental_Rendering_ShaderPassName : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.ShaderPassName o;
			System.String a1;
			checkType(l,2,out a1);
			o=new UnityEngine.Experimental.Rendering.ShaderPassName(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Experimental.Rendering.ShaderPassName");
		createTypeMetatable(l,constructor, typeof(UnityEngine.Experimental.Rendering.ShaderPassName),typeof(System.ValueType));
	}
}
