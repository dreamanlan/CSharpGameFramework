using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_AccountLogin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLogin o;
			o=new GameFrameworkMessage.AccountLogin();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_AccountId(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLogin self=(GameFrameworkMessage.AccountLogin)checkSelf(l);
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
			GameFrameworkMessage.AccountLogin self=(GameFrameworkMessage.AccountLogin)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Password(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLogin self=(GameFrameworkMessage.AccountLogin)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Password);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Password(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLogin self=(GameFrameworkMessage.AccountLogin)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Password=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_ClientInfo(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLogin self=(GameFrameworkMessage.AccountLogin)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ClientInfo);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_ClientInfo(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLogin self=(GameFrameworkMessage.AccountLogin)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_ClientInfo=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.AccountLogin");
		addMember(l,"m_AccountId",get_m_AccountId,set_m_AccountId,true);
		addMember(l,"m_Password",get_m_Password,set_m_Password,true);
		addMember(l,"m_ClientInfo",get_m_ClientInfo,set_m_ClientInfo,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.AccountLogin));
	}
}
