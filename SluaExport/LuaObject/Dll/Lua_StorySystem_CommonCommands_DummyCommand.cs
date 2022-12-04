using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_CommonCommands_DummyCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.CommonCommands.DummyCommand");
		addMember(l,ctor_s);
		createTypeMetatable(l,null, typeof(StorySystem.CommonCommands.DummyCommand),typeof(StorySystem.AbstractStoryCommand));
	}
}
