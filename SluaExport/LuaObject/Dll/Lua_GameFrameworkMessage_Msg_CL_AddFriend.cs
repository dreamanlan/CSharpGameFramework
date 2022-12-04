using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CL_AddFriend : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_AddFriend");
		addMember(l,ctor_s);
		addMember(l,"m_FriendNickname",get_m_FriendNickname,set_m_FriendNickname,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CL_AddFriend));
	}
}
