using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryMessageHandlerEnumerator : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MoveNext(IntPtr l) {
		try {
			StorySystem.StoryMessageHandlerEnumerator self=(StorySystem.StoryMessageHandlerEnumerator)checkSelf(l);
			var ret=self.MoveNext();
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
	static public int get_Current(IntPtr l) {
		try {
			StorySystem.StoryMessageHandlerEnumerator self=(StorySystem.StoryMessageHandlerEnumerator)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Current);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryMessageHandlerEnumerator");
		addMember(l,MoveNext);
		addMember(l,"Current",get_Current,null,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryMessageHandlerEnumerator));
	}
}
