using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_UiResourceSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int UnloadUiResource(IntPtr l) {
		try {
			GameFramework.UiResourceSystem self=(GameFramework.UiResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.UnloadUiResource(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CleanupAllUiResources(IntPtr l) {
		try {
			GameFramework.UiResourceSystem self=(GameFramework.UiResourceSystem)checkSelf(l);
			self.CleanupAllUiResources();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.UiResourceSystem");
		addMember(l,ctor_s);
		addMember(l,GetUiResource);
		addMember(l,UnloadUiResource);
		addMember(l,CleanupAllUiResources);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.UiResourceSystem));
	}
}
