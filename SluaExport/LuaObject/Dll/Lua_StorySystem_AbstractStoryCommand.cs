using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_AbstractStoryCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetId(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			var ret=self.GetId();
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
	static public int GetComments(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			var ret=self.GetComments();
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
	static public int GetConfig(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			var ret=self.GetConfig();
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
	static public int ShareConfig(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			StorySystem.IStoryCommand a1;
			checkType(l,2,out a1);
			self.ShareConfig(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Init(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			var ret=self.Init(a1);
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
	static public int Reset(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Execute(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandler a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			BoxedValue a4;
			checkValueType(l,5,out a4);
			BoxedValueList a5;
			checkType(l,6,out a5);
			var ret=self.Execute(a1,a2,a3,a4,a5);
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
	static public int ExecDebugger(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandler a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			BoxedValue a4;
			checkValueType(l,5,out a4);
			BoxedValueList a5;
			checkType(l,6,out a5);
			var ret=self.ExecDebugger(a1,a2,a3,a4,a5);
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
	static public int Clone(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			var ret=self.Clone();
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
	static public int get_IsCompositeCommand(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsCompositeCommand);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_PrologueCommand(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PrologueCommand);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EpilogueCommand(IntPtr l) {
		try {
			StorySystem.AbstractStoryCommand self=(StorySystem.AbstractStoryCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EpilogueCommand);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.AbstractStoryCommand");
		addMember(l,GetId);
		addMember(l,GetComments);
		addMember(l,GetConfig);
		addMember(l,ShareConfig);
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,Execute);
		addMember(l,ExecDebugger);
		addMember(l,Clone);
		addMember(l,"IsCompositeCommand",get_IsCompositeCommand,null,true);
		addMember(l,"PrologueCommand",get_PrologueCommand,null,true);
		addMember(l,"EpilogueCommand",get_EpilogueCommand,null,true);
		createTypeMetatable(l,null, typeof(StorySystem.AbstractStoryCommand));
	}
}
