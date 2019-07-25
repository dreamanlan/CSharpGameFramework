using System;

using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_IStoryCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			StorySystem.IStoryCommand self=(StorySystem.IStoryCommand)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.Init(a1);
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
			StorySystem.IStoryCommand self=(StorySystem.IStoryCommand)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Execute(IntPtr l) {
		try {
			StorySystem.IStoryCommand self=(StorySystem.IStoryCommand)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			System.Object a3;
			checkType(l,4,out a3);
			System.Object[] a4;
			checkArray(l,5,out a4);
			var ret=self.Execute(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LeadCommand(IntPtr l) {
		try {
			StorySystem.IStoryCommand self=(StorySystem.IStoryCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LeadCommand);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.IStoryCommand");
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,Execute);
		addMember(l,"LeadCommand",get_LeadCommand,null,true);
		createTypeMetatable(l,null, typeof(StorySystem.IStoryCommand));
	}
}
