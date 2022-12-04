using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_AccountLoginResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLoginResult o;
			o=new GameFrameworkMessage.AccountLoginResult();
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
	static public int get_m_AccountId(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLoginResult self=(GameFrameworkMessage.AccountLoginResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_AccountId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_AccountId(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLoginResult self=(GameFrameworkMessage.AccountLoginResult)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLoginResult self=(GameFrameworkMessage.AccountLoginResult)checkSelf(l);
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
			GameFrameworkMessage.AccountLoginResult self=(GameFrameworkMessage.AccountLoginResult)checkSelf(l);
			GameFrameworkMessage.AccountLoginResult.AccountLoginResultEnum v;
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
	static public int get_m_UserGuid(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLoginResult self=(GameFrameworkMessage.AccountLoginResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_UserGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_UserGuid(IntPtr l) {
		try {
			GameFrameworkMessage.AccountLoginResult self=(GameFrameworkMessage.AccountLoginResult)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.m_UserGuid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.AccountLoginResult");
		addMember(l,ctor_s);
		addMember(l,"m_AccountId",get_m_AccountId,set_m_AccountId,true);
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_UserGuid",get_m_UserGuid,set_m_UserGuid,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.AccountLoginResult));
	}
}
