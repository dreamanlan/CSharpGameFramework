using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_GiveUpBattle : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_GiveUpBattle o;
			o=new GameFrameworkMessage.Msg_CR_GiveUpBattle();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_GiveUpBattle");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_GiveUpBattle));
	}
}
