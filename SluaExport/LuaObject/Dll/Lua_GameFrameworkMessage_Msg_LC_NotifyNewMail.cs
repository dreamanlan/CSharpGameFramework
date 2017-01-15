using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_NotifyNewMail : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_NotifyNewMail o;
			o=new GameFrameworkMessage.Msg_LC_NotifyNewMail();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_NotifyNewMail");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_NotifyNewMail));
	}
}
