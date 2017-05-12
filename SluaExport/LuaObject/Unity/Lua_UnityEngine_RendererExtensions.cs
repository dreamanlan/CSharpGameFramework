using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_RendererExtensions : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateGIMaterials_s(IntPtr l) {
		try {
			UnityEngine.Renderer a1;
			checkType(l,1,out a1);
			UnityEngine.RendererExtensions.UpdateGIMaterials(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.RendererExtensions");
		addMember(l,UpdateGIMaterials_s);
		createTypeMetatable(l,null, typeof(UnityEngine.RendererExtensions));
	}
}
