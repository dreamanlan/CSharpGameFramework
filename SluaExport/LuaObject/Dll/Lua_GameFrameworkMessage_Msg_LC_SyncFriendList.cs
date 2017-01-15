using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_SyncFriendList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncFriendList o;
			o=new GameFrameworkMessage.Msg_LC_SyncFriendList();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Friends(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncFriendList self=(GameFrameworkMessage.Msg_LC_SyncFriendList)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Friends);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_SyncFriendList");
		addMember(l,"m_Friends",get_m_Friends,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_SyncFriendList));
	}
}
