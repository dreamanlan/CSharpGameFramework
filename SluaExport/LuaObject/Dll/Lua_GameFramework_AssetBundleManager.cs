using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_AssetBundleManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int Init(IntPtr l) {
		try {
			GameFramework.AssetBundleManager self=(GameFramework.AssetBundleManager)checkSelf(l);
			self.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int Unload(IntPtr l) {
		try {
			GameFramework.AssetBundleManager self=(GameFramework.AssetBundleManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Unload(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AssetBundleManager");
		addMember(l,Init);
		addMember(l,Contains);
		addMember(l,Load);
		addMember(l,Unload);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(GameFramework.AssetBundleManager));
	}
}
