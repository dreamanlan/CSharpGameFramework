using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_ChangeNameResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.ChangeNameResult o;
			o=new GameFrameworkMessage.ChangeNameResult();
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
			GameFrameworkMessage.ChangeNameResult self=(GameFrameworkMessage.ChangeNameResult)checkSelf(l);
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
			GameFrameworkMessage.ChangeNameResult self=(GameFrameworkMessage.ChangeNameResult)checkSelf(l);
			GameFrameworkMessage.ChangeNameResult.ChangeNameResultEnum v;
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
	static public int get_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.ChangeNameResult self=(GameFrameworkMessage.ChangeNameResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.ChangeNameResult self=(GameFrameworkMessage.ChangeNameResult)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.ChangeNameResult");
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		addMember(l,"m_Nickname",get_m_Nickname,set_m_Nickname,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.ChangeNameResult));
	}
}
