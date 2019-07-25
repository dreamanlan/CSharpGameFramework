using System;

using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_CommonCommands_DummyCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			StorySystem.CommonCommands.DummyCommand o;
			o=new StorySystem.CommonCommands.DummyCommand();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.CommonCommands.DummyCommand");
		createTypeMetatable(l,constructor, typeof(StorySystem.CommonCommands.DummyCommand),typeof(StorySystem.AbstractStoryCommand));
	}
}
