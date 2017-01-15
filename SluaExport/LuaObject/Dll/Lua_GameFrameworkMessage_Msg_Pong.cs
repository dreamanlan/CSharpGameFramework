using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_Pong : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_Pong o;
			o=new GameFrameworkMessage.Msg_Pong();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_send_ping_time(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_Pong self=(GameFrameworkMessage.Msg_Pong)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.send_ping_time);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_send_ping_time(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_Pong self=(GameFrameworkMessage.Msg_Pong)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.send_ping_time=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_send_pong_time(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_Pong self=(GameFrameworkMessage.Msg_Pong)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.send_pong_time);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_send_pong_time(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_Pong self=(GameFrameworkMessage.Msg_Pong)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.send_pong_time=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_Pong");
		addMember(l,"send_ping_time",get_send_ping_time,set_send_ping_time,true);
		addMember(l,"send_pong_time",get_send_pong_time,set_send_pong_time,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_Pong));
	}
}
