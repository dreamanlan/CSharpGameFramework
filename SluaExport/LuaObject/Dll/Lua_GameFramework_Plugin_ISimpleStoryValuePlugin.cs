using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Plugin_ISimpleStoryValuePlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetProxy(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryValuePlugin self=(GameFramework.Plugin.ISimpleStoryValuePlugin)checkSelf(l);
			StorySystem.StoryValueResult a1;
			checkType(l,2,out a1);
			self.SetProxy(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clone(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryValuePlugin self=(GameFramework.Plugin.ISimpleStoryValuePlugin)checkSelf(l);
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
	static public int Evaluate(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryValuePlugin self=(GameFramework.Plugin.ISimpleStoryValuePlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandler a2;
			checkType(l,3,out a2);
			StorySystem.StoryValueParams a3;
			checkType(l,4,out a3);
			self.Evaluate(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.ISimpleStoryValuePlugin");
		addMember(l,SetProxy);
		addMember(l,Clone);
		addMember(l,Evaluate);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.ISimpleStoryValuePlugin));
	}
}
