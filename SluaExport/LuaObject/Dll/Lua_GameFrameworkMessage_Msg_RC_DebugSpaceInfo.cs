using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_DebugSpaceInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo o;
			o=new GameFrameworkMessage.Msg_RC_DebugSpaceInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_space_infos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.space_infos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_DebugSpaceInfo");
		addMember(l,"space_infos",get_space_infos,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_DebugSpaceInfo));
	}
}
