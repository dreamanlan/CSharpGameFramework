using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryCommandManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterCommandFactory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				StorySystem.IStoryCommandFactory a2;
				checkType(l,3,out a2);
				self.RegisterCommandFactory(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(GameFramework.StoryCommandGroupDefine),typeof(string),typeof(StorySystem.IStoryCommandFactory))){
				StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
				GameFramework.StoryCommandGroupDefine a1;
				checkEnum(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				StorySystem.IStoryCommandFactory a3;
				checkType(l,4,out a3);
				self.RegisterCommandFactory(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(StorySystem.IStoryCommandFactory),typeof(bool))){
				StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				StorySystem.IStoryCommandFactory a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				self.RegisterCommandFactory(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
				GameFramework.StoryCommandGroupDefine a1;
				checkEnum(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				StorySystem.IStoryCommandFactory a3;
				checkType(l,4,out a3);
				System.Boolean a4;
				checkType(l,5,out a4);
				self.RegisterCommandFactory(a1,a2,a3,a4);
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
				StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.FindFactory(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
				GameFramework.StoryCommandGroupDefine a1;
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
	static public int CreateCommand(IntPtr l) {
		try {
			StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			var ret=self.CreateCommand(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_MaxCommandGroupNum(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryCommandManager.c_MaxCommandGroupNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ThreadCommandGroupsMask(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryCommandManager.ThreadCommandGroupsMask);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ThreadCommandGroupsMask(IntPtr l) {
		try {
			System.UInt64 v;
			checkType(l,2,out v);
			StorySystem.StoryCommandManager.ThreadCommandGroupsMask=v;
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
			pushValue(l,StorySystem.StoryCommandManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryCommandManager");
		addMember(l,RegisterCommandFactory);
		addMember(l,FindFactory);
		addMember(l,CreateCommand);
		addMember(l,"c_MaxCommandGroupNum",get_c_MaxCommandGroupNum,null,false);
		addMember(l,"ThreadCommandGroupsMask",get_ThreadCommandGroupsMask,set_ThreadCommandGroupsMask,false);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryCommandManager));
	}
}
