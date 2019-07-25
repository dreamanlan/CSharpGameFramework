using System;

using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryValueManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterValueFactory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				StorySystem.IStoryValueFactory a2;
				checkType(l,3,out a2);
				self.RegisterValueFactory(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(GameFramework.StoryValueGroupDefine),typeof(string),typeof(StorySystem.IStoryValueFactory))){
				StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
				GameFramework.StoryValueGroupDefine a1;
				checkEnum(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				StorySystem.IStoryValueFactory a3;
				checkType(l,4,out a3);
				self.RegisterValueFactory(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(StorySystem.IStoryValueFactory),typeof(bool))){
				StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				StorySystem.IStoryValueFactory a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				self.RegisterValueFactory(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
				GameFramework.StoryValueGroupDefine a1;
				checkEnum(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				StorySystem.IStoryValueFactory a3;
				checkType(l,4,out a3);
				System.Boolean a4;
				checkType(l,5,out a4);
				self.RegisterValueFactory(a1,a2,a3,a4);
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
	static public int FindFactory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.FindFactory(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
				GameFramework.StoryValueGroupDefine a1;
				checkEnum(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				var ret=self.FindFactory(a1,a2);
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
	static public int CalcValue(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			var ret=self.CalcValue(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_MaxValueGroupNum(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValueManager.c_MaxValueGroupNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ThreadValueGroupsMask(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValueManager.ThreadValueGroupsMask);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ThreadValueGroupsMask(IntPtr l) {
		try {
			System.UInt64 v;
			checkType(l,2,out v);
			StorySystem.StoryValueManager.ThreadValueGroupsMask=v;
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
			pushValue(l,StorySystem.StoryValueManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValueManager");
		addMember(l,RegisterValueFactory);
		addMember(l,FindFactory);
		addMember(l,CalcValue);
		addMember(l,"c_MaxValueGroupNum",get_c_MaxValueGroupNum,null,false);
		addMember(l,"ThreadValueGroupsMask",get_ThreadValueGroupsMask,set_ThreadValueGroupsMask,false);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryValueManager));
	}
}
