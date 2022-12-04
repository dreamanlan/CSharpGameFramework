using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_LC_GmKickUser : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser o;
			o=new GameFrameworkMessage.Msg_LC_GmKickUser();
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
	static public int get_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.m_Result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			GameFrameworkMessage.GmResultEnum v;
			checkEnum(l,2,out v);
			self.m_Result=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Nickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_KickedGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_KickedGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_KickedGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.m_KickedGuid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_KickedAccountId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_KickedAccountId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_KickedAccountId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmKickUser self=(GameFrameworkMessage.Msg_LC_GmKickUser)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_KickedAccountId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_GmKickUser");
		addMember(l,ctor_s);
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_Nickname",get_m_Nickname,set_m_Nickname,true);
		addMember(l,"m_KickedGuid",get_m_KickedGuid,set_m_KickedGuid,true);
		addMember(l,"m_KickedAccountId",get_m_KickedAccountId,set_m_KickedAccountId,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_LC_GmKickUser));
	}
}
