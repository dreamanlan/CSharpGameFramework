using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_LobbyMessageDefine2Type : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Query__Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=GameFrameworkMessage.LobbyMessageDefine2Type.Query(a1);
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
	static public int Query__Type_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			var ret=GameFrameworkMessage.LobbyMessageDefine2Type.Query(a1);
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
		getTypeTable(l,"GameFrameworkMessage.LobbyMessageDefine2Type");
		addMember(l,Query__Int32_s);
		addMember(l,Query__Type_s);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.LobbyMessageDefine2Type));
	}
}
