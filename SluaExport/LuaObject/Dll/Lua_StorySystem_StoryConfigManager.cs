using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryConfigManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int LoadStoryText(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Byte[] a2;
			checkArray(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			self.LoadStoryText(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FreeStory(IntPtr l) {
		try {
			StorySystem.StoryConfigManager self=(StorySystem.StoryConfigManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.FreeStory(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryConfigManager");
		addMember(l,LoadStories);
		addMember(l,ExistStory);
		addMember(l,LoadStory);
		addMember(l,LoadStoryText);
		addMember(l,FreeStory);
		addMember(l,NewStoryInstance);
		addMember(l,Clear);
		addMember(l,NewInstance_s);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryConfigManager));
	}
}
