using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Plugin_ISimpleStoryCommandPlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clone(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryCommandPlugin self=(GameFramework.Plugin.ISimpleStoryCommandPlugin)checkSelf(l);
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
	static public int ResetState(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryCommandPlugin self=(GameFramework.Plugin.ISimpleStoryCommandPlugin)checkSelf(l);
			self.ResetState();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ExecCommand(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryCommandPlugin self=(GameFramework.Plugin.ISimpleStoryCommandPlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandler a2;
			checkType(l,3,out a2);
			StorySystem.StoryValueParams a3;
			checkType(l,4,out a3);
			System.Int64 a4;
			checkType(l,5,out a4);
			var ret=self.ExecCommand(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.ISimpleStoryCommandPlugin");
		addMember(l,Clone);
		addMember(l,ResetState);
		addMember(l,ExecCommand);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.ISimpleStoryCommandPlugin));
	}
}
