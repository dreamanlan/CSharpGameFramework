using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_VersionVerifyResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.VersionVerifyResult o;
			o=new GameFrameworkMessage.VersionVerifyResult();
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
			GameFrameworkMessage.VersionVerifyResult self=(GameFrameworkMessage.VersionVerifyResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.VersionVerifyResult self=(GameFrameworkMessage.VersionVerifyResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Result=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_EnableLog(IntPtr l) {
		try {
			GameFrameworkMessage.VersionVerifyResult self=(GameFrameworkMessage.VersionVerifyResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_EnableLog);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_EnableLog(IntPtr l) {
		try {
			GameFrameworkMessage.VersionVerifyResult self=(GameFrameworkMessage.VersionVerifyResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_EnableLog=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_ShopMask(IntPtr l) {
		try {
			GameFrameworkMessage.VersionVerifyResult self=(GameFrameworkMessage.VersionVerifyResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ShopMask);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_ShopMask(IntPtr l) {
		try {
			GameFrameworkMessage.VersionVerifyResult self=(GameFrameworkMessage.VersionVerifyResult)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.m_ShopMask=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.VersionVerifyResult");
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_EnableLog",get_m_EnableLog,set_m_EnableLog,true);
		addMember(l,"m_ShopMask",get_m_ShopMask,set_m_ShopMask,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.VersionVerifyResult));
	}
}
