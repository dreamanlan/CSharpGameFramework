using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_SyncMemberList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncMemberList o;
			o=new GameFrameworkMessage.Msg_LC_SyncMemberList();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Members(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncMemberList self=(GameFrameworkMessage.Msg_LC_SyncMemberList)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Members);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_SyncMemberList");
		addMember(l,"m_Members",get_m_Members,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_SyncMemberList));
	}
}
