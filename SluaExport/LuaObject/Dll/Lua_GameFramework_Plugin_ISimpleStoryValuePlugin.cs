using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Plugin_ISimpleStoryValuePlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int Evaluate(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryValuePlugin self=(GameFramework.Plugin.ISimpleStoryValuePlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryValueParams a2;
			checkType(l,3,out a2);
			self.Evaluate(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.ISimpleStoryValuePlugin");
		addMember(l,SetProxy);
		addMember(l,Evaluate);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.ISimpleStoryValuePlugin));
	}
}
