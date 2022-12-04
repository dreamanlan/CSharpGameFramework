using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StringLower : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StrToLower_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=StringLower.StrToLower(a1);
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
	static public int StrToUpper_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=StringLower.StrToUpper(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StringLower");
		addMember(l,StrToLower_s);
		addMember(l,StrToUpper_s);
		createTypeMetatable(l,null, typeof(StringLower));
	}
}
