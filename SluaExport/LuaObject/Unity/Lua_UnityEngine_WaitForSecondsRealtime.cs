using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_WaitForSecondsRealtime : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.WaitForSecondsRealtime o;
			System.Single a1;
			checkType(l,2,out a1);
			o=new UnityEngine.WaitForSecondsRealtime(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_keepWaiting(IntPtr l) {
		try {
			UnityEngine.WaitForSecondsRealtime self=(UnityEngine.WaitForSecondsRealtime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.keepWaiting);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.WaitForSecondsRealtime");
		addMember(l,"keepWaiting",get_keepWaiting,null,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.WaitForSecondsRealtime),typeof(UnityEngine.CustomYieldInstruction));
	}
}
