using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_LobbyMessageDefine2Type : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Query_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Type))){
				System.Type a1;
				checkType(l,1,out a1);
				var ret=GameFrameworkMessage.LobbyMessageDefine2Type.Query(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=GameFrameworkMessage.LobbyMessageDefine2Type.Query(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.LobbyMessageDefine2Type");
		addMember(l,Query_s);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.LobbyMessageDefine2Type));
	}
}
