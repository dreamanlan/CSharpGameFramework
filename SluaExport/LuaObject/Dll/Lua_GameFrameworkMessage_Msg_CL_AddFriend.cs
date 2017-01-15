using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_AddFriend : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_AddFriend o;
			o=new GameFrameworkMessage.Msg_CL_AddFriend();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_FriendNickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_AddFriend self=(GameFrameworkMessage.Msg_CL_AddFriend)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_FriendNickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_FriendNickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_AddFriend self=(GameFrameworkMessage.Msg_CL_AddFriend)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_FriendNickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_AddFriend");
		addMember(l,"m_FriendNickname",get_m_FriendNickname,set_m_FriendNickname,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_AddFriend));
	}
}
