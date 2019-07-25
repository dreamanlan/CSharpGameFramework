using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_GmUnlockUser : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmUnlockUser o;
			o=new GameFrameworkMessage.Msg_LC_GmUnlockUser();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmUnlockUser self=(GameFrameworkMessage.Msg_LC_GmUnlockUser)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.m_Result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmUnlockUser self=(GameFrameworkMessage.Msg_LC_GmUnlockUser)checkSelf(l);
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
	static public int get_m_AccountId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmUnlockUser self=(GameFrameworkMessage.Msg_LC_GmUnlockUser)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_AccountId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_AccountId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmUnlockUser self=(GameFrameworkMessage.Msg_LC_GmUnlockUser)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_AccountId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_GmUnlockUser");
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_AccountId",get_m_AccountId,set_m_AccountId,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_GmUnlockUser));
	}
}
