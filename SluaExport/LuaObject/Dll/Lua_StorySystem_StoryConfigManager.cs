using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryConfigManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadStories(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.String[] a3;
			checkParams(l,4,out a3);
			self.LoadStories(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExistStory(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.ExistStory(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadStory(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			self.LoadStory(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadStoryText(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			self.LoadStoryText(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadStoryCode(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			self.LoadStoryCode(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStories(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string))){
				StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.GetStories(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetStories(a1);
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
	static public int NewStoryInstance(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.NewStoryInstance(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NewInstance_s(IntPtr l) {
		try {
			var ret=StorySystem.StoryConfigManager.NewInstance();
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
			pushValue(l,StorySystem.StoryConfigManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryConfigManager");
		addMember(l,LoadStories);
		addMember(l,ExistStory);
		addMember(l,LoadStory);
		addMember(l,LoadStoryText);
		addMember(l,LoadStoryCode);
		addMember(l,GetStories);
		addMember(l,NewStoryInstance);
		addMember(l,Clear);
		addMember(l,NewInstance_s);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryConfigManager));
	}
}
