using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_EntityViewModelManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			self.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Release(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			self.Release();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Tick(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateEntityView(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.CreateEntityView(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DestroyEntityView(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.DestroyEntityView(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityViewById(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityViewById(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityViewByUnitId(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityViewByUnitId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObject(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObject(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObjectByUnitId(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectByUnitId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityView(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityView(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObjectUnitId(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectUnitId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObjectId(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExistGameObject(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.ExistGameObject(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GameObject))){
				GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				var ret=self.ExistGameObject(a1);
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
	static public int MarkSpaceInfoViews(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			self.MarkSpaceInfoViews();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateSpaceInfoView(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			self.UpdateSpaceInfoView(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DestroyUnusedSpaceInfoViews(IntPtr l) {
		try {
			GameFramework.EntityViewModelManager self=(GameFramework.EntityViewModelManager)checkSelf(l);
			self.DestroyUnusedSpaceInfoViews();
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
			pushValue(l,GameFramework.EntityViewModelManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.EntityViewModelManager");
		addMember(l,Init);
		addMember(l,Release);
		addMember(l,Tick);
		addMember(l,CreateEntityView);
		addMember(l,DestroyEntityView);
		addMember(l,GetEntityViewById);
		addMember(l,GetEntityViewByUnitId);
		addMember(l,GetGameObject);
		addMember(l,GetGameObjectByUnitId);
		addMember(l,GetEntityView);
		addMember(l,GetGameObjectUnitId);
		addMember(l,GetGameObjectId);
		addMember(l,ExistGameObject);
		addMember(l,MarkSpaceInfoViews);
		addMember(l,UpdateSpaceInfoView);
		addMember(l,DestroyUnusedSpaceInfoViews);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.EntityViewModelManager));
	}
}
