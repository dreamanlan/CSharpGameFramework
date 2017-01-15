using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_GmQueryAccount : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryAccount o;
			o=new GameFrameworkMessage.Msg_LC_GmQueryAccount();
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
			GameFrameworkMessage.Msg_LC_GmQueryAccount self=(GameFrameworkMessage.Msg_LC_GmQueryAccount)checkSelf(l);
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
			GameFrameworkMessage.Msg_LC_GmQueryAccount self=(GameFrameworkMessage.Msg_LC_GmQueryAccount)checkSelf(l);
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
	static public int get_m_QueryAccount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryAccount self=(GameFrameworkMessage.Msg_LC_GmQueryAccount)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_QueryAccount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_QueryAccount(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryAccount self=(GameFrameworkMessage.Msg_LC_GmQueryAccount)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_QueryAccount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_AccountState(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryAccount self=(GameFrameworkMessage.Msg_LC_GmQueryAccount)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.m_AccountState);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_AccountState(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryAccount self=(GameFrameworkMessage.Msg_LC_GmQueryAccount)checkSelf(l);
			GameFrameworkMessage.GmStateEnum v;
			checkEnum(l,2,out v);
			self.m_AccountState=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Infos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryAccount self=(GameFrameworkMessage.Msg_LC_GmQueryAccount)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Infos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_GmQueryAccount");
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_QueryAccount",get_m_QueryAccount,set_m_QueryAccount,true);
		addMember(l,"m_AccountState",get_m_AccountState,set_m_AccountState,true);
		addMember(l,"m_Infos",get_m_Infos,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_GmQueryAccount));
	}
}
