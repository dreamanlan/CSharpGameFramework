using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_RequestSceneRoomInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.RequestSceneRoomInfo o;
			o=new GameFrameworkMessage.RequestSceneRoomInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.RequestSceneRoomInfo");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.RequestSceneRoomInfo));
	}
}
