using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ResourceSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetVisible(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			UnityEngine.CharacterController a3;
			checkType(l,4,out a3);
			self.SetVisible(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PreloadObject(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.PreloadObject(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.PreloadObject(a1,a2);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PreloadSharedResource(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.PreloadSharedResource(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NewObject(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.Object))){
				GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
				UnityEngine.Object a1;
				checkType(l,2,out a1);
				var ret=self.NewObject(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string))){
				GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.NewObject(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Object),typeof(float))){
				GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
				UnityEngine.Object a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				var ret=self.NewObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(float))){
				GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				var ret=self.NewObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RecycleObject(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			var ret=self.RecycleObject(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Tick(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSharedResource(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetSharedResource(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CleanupResourcePool(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			self.CleanupResourcePool();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FroceSetGameObjectLayer_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			GameFramework.ResourceSystem.FroceSetGameObjectLayer(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SharedResourcePath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.ResourceSystem.SharedResourcePath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SharedResourcePath(IntPtr l) {
		try {
			System.Collections.Generic.HashSet<System.String> v;
			checkType(l,2,out v);
			GameFramework.ResourceSystem.SharedResourcePath=v;
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
			pushValue(l,GameFramework.ResourceSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ResourceSystem");
		addMember(l,SetVisible);
		addMember(l,PreloadObject);
		addMember(l,PreloadSharedResource);
		addMember(l,NewObject);
		addMember(l,RecycleObject);
		addMember(l,Tick);
		addMember(l,GetSharedResource);
		addMember(l,CleanupResourcePool);
		addMember(l,FroceSetGameObjectLayer_s);
		addMember(l,"SharedResourcePath",get_SharedResourcePath,set_SharedResourcePath,false);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.ResourceSystem));
	}
}
