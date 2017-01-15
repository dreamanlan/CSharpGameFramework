using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_RequestNicknameResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.RequestNicknameResult o;
			o=new GameFrameworkMessage.RequestNicknameResult();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Nicknames(IntPtr l) {
		try {
			GameFrameworkMessage.RequestNicknameResult self=(GameFrameworkMessage.RequestNicknameResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nicknames);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.RequestNicknameResult");
		addMember(l,"m_Nicknames",get_m_Nicknames,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.RequestNicknameResult));
	}
}
