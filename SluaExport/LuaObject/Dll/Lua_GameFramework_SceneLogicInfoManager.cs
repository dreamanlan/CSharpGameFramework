using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SceneLogicInfoManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager o;
			System.Int32 a1;
			checkType(l,2,out a1);
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
	static public int AddSceneLogicInfo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(GameFramework.SceneLogicConfig))){
				GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
				GameFramework.SceneLogicConfig a1;
				checkType(l,2,out a1);
				var ret=self.AddSceneLogicInfo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.AddSceneLogicInfo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(GameFramework.SceneLogicConfig))){
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
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DelayAddSceneLogicInfo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(GameFramework.SceneLogicConfig))){
				GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
				GameFramework.SceneLogicConfig a1;
				checkType(l,2,out a1);
				var ret=self.DelayAddSceneLogicInfo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.DelayAddSceneLogicInfo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(GameFramework.SceneLogicConfig))){
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
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneLogicInfos(IntPtr l) {
		try {
			GameFramework.SceneLogicInfoManager self=(GameFramework.SceneLogicInfoManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneLogicInfos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SceneLogicInfoManager");
		addMember(l,SetSceneContext);
		addMember(l,GetSceneLogicInfo);
		addMember(l,GetSceneLogicInfoByConfigId);
		addMember(l,AddSceneLogicInfo);
		addMember(l,DelayAddSceneLogicInfo);
		addMember(l,ExecuteDelayAdd);
		addMember(l,RemoveSceneLogicInfo);
		addMember(l,Reset);
		addMember(l,"SceneLogicInfos",get_SceneLogicInfos,null,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.SceneLogicInfoManager));
	}
}
