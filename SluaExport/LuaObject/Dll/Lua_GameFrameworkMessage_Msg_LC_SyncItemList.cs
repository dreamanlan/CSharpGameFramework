using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_LC_SyncItemList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncItemList o;
			o=new GameFrameworkMessage.Msg_LC_SyncItemList();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_SyncItemList");
		addMember(l,ctor_s);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_LC_SyncItemList));
	}
}
