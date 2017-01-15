using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_UserMoveToPos : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_UserMoveToPos o;
			o=new GameFrameworkMessage.Msg_CR_UserMoveToPos();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_target_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_UserMoveToPos self=(GameFrameworkMessage.Msg_CR_UserMoveToPos)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.target_pos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_target_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_UserMoveToPos self=(GameFrameworkMessage.Msg_CR_UserMoveToPos)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.target_pos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_is_stop(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_UserMoveToPos self=(GameFrameworkMessage.Msg_CR_UserMoveToPos)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.is_stop);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_is_stop(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_UserMoveToPos self=(GameFrameworkMessage.Msg_CR_UserMoveToPos)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.is_stop=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_UserMoveToPos");
		addMember(l,"target_pos",get_target_pos,set_target_pos,true);
		addMember(l,"is_stop",get_is_stop,set_is_stop,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_UserMoveToPos));
	}
}
