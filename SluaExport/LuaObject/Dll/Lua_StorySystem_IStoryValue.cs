using System;

using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_IStoryValue : LuaObject {
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.IStoryValue");
		createTypeMetatable(l,null, typeof(StorySystem.IStoryValue));
	}
}
