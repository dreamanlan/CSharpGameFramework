using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_AssetBundleManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.AssetBundleManager o;
			o=new GameFramework.AssetBundleManager();
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
	static public int Contains(IntPtr l) {
		try {
			GameFramework.AssetBundleManager self=(GameFramework.AssetBundleManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.Contains(a1);
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
	static public int Load(IntPtr l) {
		try {
			GameFramework.AssetBundleManager self=(GameFramework.AssetBundleManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.Load(a1);
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
	static public int LoadAsync(IntPtr l) {
		try {
			GameFramework.AssetBundleManager self=(GameFramework.AssetBundleManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			GameFramework.ResourceSystem.ResourceLoadDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.LoadAsync(a1,a2);
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
			pushValue(l,GameFramework.AssetBundleManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AssetBundleManager");
		addMember(l,ctor_s);
		addMember(l,Contains);
		addMember(l,Load);
		addMember(l,LoadAsync);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.AssetBundleManager));
	}
}
