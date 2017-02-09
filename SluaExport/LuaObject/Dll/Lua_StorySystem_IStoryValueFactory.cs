using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_IStoryValueFactory : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Build(IntPtr l) {
		try {
			StorySystem.IStoryValueFactory self=(StorySystem.IStoryValueFactory)checkSelf(l);
			var ret=self.Build();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.IStoryValueFactory");
		addMember(l,Build);
		createTypeMetatable(l,null, typeof(StorySystem.IStoryValueFactory));
	}
}
