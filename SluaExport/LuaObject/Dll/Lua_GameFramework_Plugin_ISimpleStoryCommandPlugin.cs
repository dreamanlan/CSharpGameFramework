using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Plugin_ISimpleStoryCommandPlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int ExecCommand(IntPtr l) {
		try {
			GameFramework.Plugin.ISimpleStoryCommandPlugin self=(GameFramework.Plugin.ISimpleStoryCommandPlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryValueParams a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			var ret=self.ExecCommand(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.ISimpleStoryCommandPlugin");
		addMember(l,ResetState);
		addMember(l,ExecCommand);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.ISimpleStoryCommandPlugin));
	}
}
