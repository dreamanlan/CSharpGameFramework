using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_UiResourceSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.UiResourceSystem o;
			o=new GameFramework.UiResourceSystem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetUiResource(IntPtr l) {
		try {
			GameFramework.UiResourceSystem self=(GameFramework.UiResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetUiResource(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.UiResourceSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.UiResourceSystem");
		addMember(l,GetUiResource);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(GameFramework.UiResourceSystem));
	}
}
