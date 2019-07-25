using System;

using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryValueHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_MaxWaitCommandTime(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValueHelper.c_MaxWaitCommandTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValueHelper");
		addMember(l,"c_MaxWaitCommandTime",get_c_MaxWaitCommandTime,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryValueHelper));
	}
}
