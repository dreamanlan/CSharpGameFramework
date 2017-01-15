using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_GmCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_GmCommand o;
			o=new GameFrameworkMessage.Msg_CR_GmCommand();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_type(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_GmCommand self=(GameFrameworkMessage.Msg_CR_GmCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_type(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_GmCommand self=(GameFrameworkMessage.Msg_CR_GmCommand)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_GmCommand self=(GameFrameworkMessage.Msg_CR_GmCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.content);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_GmCommand self=(GameFrameworkMessage.Msg_CR_GmCommand)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.content=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_GmCommand");
		addMember(l,"type",get_type,set_type,true);
		addMember(l,"content",get_content,set_content,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_GmCommand));
	}
}
