using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_LackOfSpace : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace o;
			o=new GameFrameworkMessage.Msg_LC_LackOfSpace();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Succeed(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Succeed);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Succeed(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.m_Succeed=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_ReceiveNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ReceiveNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_ReceiveNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_ReceiveNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_FreeNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_FreeNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_FreeNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_FreeNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_MailGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_MailGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_MailGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_LackOfSpace self=(GameFrameworkMessage.Msg_LC_LackOfSpace)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.m_MailGuid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_LackOfSpace");
		addMember(l,"m_Succeed",get_m_Succeed,set_m_Succeed,true);
		addMember(l,"m_ReceiveNum",get_m_ReceiveNum,set_m_ReceiveNum,true);
		addMember(l,"m_FreeNum",get_m_FreeNum,set_m_FreeNum,true);
		addMember(l,"m_MailGuid",get_m_MailGuid,set_m_MailGuid,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_LackOfSpace));
	}
}
