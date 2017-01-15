using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_GmGeneralOperation : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmGeneralOperation o;
			o=new GameFrameworkMessage.Msg_LC_GmGeneralOperation();
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
			GameFrameworkMessage.Msg_LC_GmGeneralOperation self=(GameFrameworkMessage.Msg_LC_GmGeneralOperation)checkSelf(l);
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
			GameFrameworkMessage.Msg_LC_GmGeneralOperation self=(GameFrameworkMessage.Msg_LC_GmGeneralOperation)checkSelf(l);
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
	static public int get_m_OperationType(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmGeneralOperation self=(GameFrameworkMessage.Msg_LC_GmGeneralOperation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_OperationType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_OperationType(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmGeneralOperation self=(GameFrameworkMessage.Msg_LC_GmGeneralOperation)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_OperationType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Params(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmGeneralOperation self=(GameFrameworkMessage.Msg_LC_GmGeneralOperation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Params);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_GmGeneralOperation");
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_OperationType",get_m_OperationType,set_m_OperationType,true);
		addMember(l,"m_Params",get_m_Params,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_GmGeneralOperation));
	}
}
