using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_StopSkill : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_StopSkill o;
			o=new GameFrameworkMessage.Msg_CR_StopSkill();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_StopSkill");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_StopSkill));
	}
}
