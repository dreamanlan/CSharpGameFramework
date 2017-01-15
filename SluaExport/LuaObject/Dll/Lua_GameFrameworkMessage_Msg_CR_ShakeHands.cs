using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_ShakeHands : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_ShakeHands o;
			o=new GameFrameworkMessage.Msg_CR_ShakeHands();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_auth_key(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_ShakeHands self=(GameFrameworkMessage.Msg_CR_ShakeHands)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.auth_key);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_auth_key(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_ShakeHands self=(GameFrameworkMessage.Msg_CR_ShakeHands)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.auth_key=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_ShakeHands");
		addMember(l,"auth_key",get_auth_key,set_auth_key,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_ShakeHands));
	}
}
