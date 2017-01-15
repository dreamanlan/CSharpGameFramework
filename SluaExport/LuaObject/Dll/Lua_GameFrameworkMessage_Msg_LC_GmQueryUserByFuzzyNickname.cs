using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_GmQueryUserByFuzzyNickname : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname o;
			o=new GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname();
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
			GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname self=(GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname)checkSelf(l);
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
			GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname self=(GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname)checkSelf(l);
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
	static public int get_m_QueryNickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname self=(GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_QueryNickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_QueryNickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname self=(GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_QueryNickname=v;
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
			GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname self=(GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Infos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname");
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_QueryNickname",get_m_QueryNickname,set_m_QueryNickname,true);
		addMember(l,"m_Infos",get_m_Infos,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_GmQueryUserByFuzzyNickname));
	}
}
