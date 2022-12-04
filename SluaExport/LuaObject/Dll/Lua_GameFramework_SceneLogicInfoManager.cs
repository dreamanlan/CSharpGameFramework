using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_SceneLogicInfoManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new GameFramework.SceneLogicInfoManager(a1);
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
	static public int SetSceneContext(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			GameFramework.SceneContextInfo a1;
			checkType(l,2,out a1);
			self.SetSceneContext(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSceneLogicInfo(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSceneLogicInfo(a1);
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
	static public int GetSceneLogicInfoByConfigId(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSceneLogicInfoByConfigId(a1);
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
	static public int AddSceneLogicInfo__Int32(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.AddSceneLogicInfo(a1);
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
	static public int AddSceneLogicInfo__SceneLogicConfig(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			GameFramework.SceneLogicConfig a1;
			checkType(l,2,out a1);
			var ret=self.AddSceneLogicInfo(a1);
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
	static public int AddSceneLogicInfo__Int32__Int32(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.AddSceneLogicInfo(a1,a2);
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
	static public int AddSceneLogicInfo__Int32__SceneLogicConfig(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			GameFramework.SceneLogicConfig a2;
			checkType(l,3,out a2);
			var ret=self.AddSceneLogicInfo(a1,a2);
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
	static public int DelayAddSceneLogicInfo__Int32(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.DelayAddSceneLogicInfo(a1);
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
	static public int DelayAddSceneLogicInfo__SceneLogicConfig(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			GameFramework.SceneLogicConfig a1;
			checkType(l,2,out a1);
			var ret=self.DelayAddSceneLogicInfo(a1);
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
	static public int DelayAddSceneLogicInfo__Int32__Int32(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.DelayAddSceneLogicInfo(a1,a2);
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
	static public int DelayAddSceneLogicInfo__Int32__SceneLogicConfig(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			GameFramework.SceneLogicConfig a2;
			checkType(l,3,out a2);
			var ret=self.DelayAddSceneLogicInfo(a1,a2);
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
	static public int ExecuteDelayAdd(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			self.ExecuteDelayAdd();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveSceneLogicInfo(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveSceneLogicInfo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SceneLogicInfoManager");
		addMember(l,ctor_s);
		addMember(l,SetSceneContext);
		addMember(l,GetSceneLogicInfo);
		addMember(l,GetSceneLogicInfoByConfigId);
		addMember(l,AddSceneLogicInfo__Int32);
		addMember(l,AddSceneLogicInfo__SceneLogicConfig);
		addMember(l,AddSceneLogicInfo__Int32__Int32);
		addMember(l,AddSceneLogicInfo__Int32__SceneLogicConfig);
		addMember(l,DelayAddSceneLogicInfo__Int32);
		addMember(l,DelayAddSceneLogicInfo__SceneLogicConfig);
		addMember(l,DelayAddSceneLogicInfo__Int32__Int32);
		addMember(l,DelayAddSceneLogicInfo__Int32__SceneLogicConfig);
		addMember(l,ExecuteDelayAdd);
		addMember(l,RemoveSceneLogicInfo);
		addMember(l,Reset);
		createTypeMetatable(l,null, typeof(GameFramework.SceneLogicInfoManager));
	}
}
