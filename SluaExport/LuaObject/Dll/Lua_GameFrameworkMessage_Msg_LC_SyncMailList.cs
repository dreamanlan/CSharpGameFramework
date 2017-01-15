using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_SyncMailList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncMailList o;
			o=new GameFrameworkMessage.Msg_LC_SyncMailList();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Mails(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncMailList self=(GameFrameworkMessage.Msg_LC_SyncMailList)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Mails);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_SyncMailList");
		addMember(l,"m_Mails",get_m_Mails,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_SyncMailList));
	}
}
