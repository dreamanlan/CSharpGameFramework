using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_RequestSceneRoomList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.RequestSceneRoomList o;
			o=new GameFrameworkMessage.RequestSceneRoomList();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.RequestSceneRoomList");
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.RequestSceneRoomList));
	}
}
