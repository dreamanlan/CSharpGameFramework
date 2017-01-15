using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_OperateMode : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_OperateMode o;
			o=new GameFrameworkMessage.Msg_CR_OperateMode();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isauto(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_OperateMode self=(GameFrameworkMessage.Msg_CR_OperateMode)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isauto);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_isauto(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_OperateMode self=(GameFrameworkMessage.Msg_CR_OperateMode)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.isauto=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_OperateMode");
		addMember(l,"isauto",get_isauto,set_isauto,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_OperateMode));
	}
}
