using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryCommandManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AllocLocalInfoIndex(IntPtr l) {
		try {
			StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
			var ret=self.AllocLocalInfoIndex();
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
	static public int RegisterCommandFactory__String__IStoryCommandFactory(IntPtr l) {
		try {
			StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			StorySystem.IStoryCommandFactory a2;
			checkType(l,3,out a2);
			self.RegisterCommandFactory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RegisterCommandFactory__String__IStoryCommandFactory__Boolean(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RegisterCommandFactory__StoryCommandGroupDefine__String__IStoryCommandFactory(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RegisterCommandFactory__StoryCommandGroupDefine__String__IStoryCommandFactory__Boolean(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FindFactory__String(IntPtr l) {
		try {
			StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.FindFactory(a1);
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
	static public int FindFactory__StoryCommandGroupDefine__String(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int Substitute(IntPtr l) {
		try {
			StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.Substitute(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryGetSubstitute(IntPtr l) {
		try {
			StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			var ret=self.TryGetSubstitute(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearSubstitutes(IntPtr l) {
		try {
			StorySystem.StoryCommandManager self=(StorySystem.StoryCommandManager)checkSelf(l);
			self.ClearSubstitutes();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryCommandManager");
		addMember(l,AllocLocalInfoIndex);
		addMember(l,RegisterCommandFactory__String__IStoryCommandFactory);
		addMember(l,RegisterCommandFactory__String__IStoryCommandFactory__Boolean);
		addMember(l,RegisterCommandFactory__StoryCommandGroupDefine__String__IStoryCommandFactory);
		addMember(l,RegisterCommandFactory__StoryCommandGroupDefine__String__IStoryCommandFactory__Boolean);
		addMember(l,FindFactory__String);
		addMember(l,FindFactory__StoryCommandGroupDefine__String);
		addMember(l,CreateCommand);
		addMember(l,Substitute);
		addMember(l,TryGetSubstitute);
		addMember(l,ClearSubstitutes);
		addMember(l,"c_MaxCommandGroupNum",get_c_MaxCommandGroupNum,null,false);
		addMember(l,"ThreadCommandGroupsMask",get_ThreadCommandGroupsMask,set_ThreadCommandGroupsMask,false);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryCommandManager));
	}
}
