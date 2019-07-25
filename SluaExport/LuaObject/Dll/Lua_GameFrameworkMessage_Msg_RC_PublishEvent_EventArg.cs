using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_PublishEvent_EventArg : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent.EventArg o;
			o=new GameFrameworkMessage.Msg_RC_PublishEvent.EventArg();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_val_type(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_RC_PublishEvent.EventArg)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.val_type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_val_type(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_RC_PublishEvent.EventArg)checkSelf(l);
			GameFrameworkMessage.ArgType v;
			checkEnum(l,2,out v);
			self.val_type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_str_val(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_RC_PublishEvent.EventArg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.str_val);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_str_val(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_RC_PublishEvent.EventArg)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.str_val=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_PublishEvent.EventArg");
		addMember(l,"val_type",get_val_type,set_val_type,true);
		addMember(l,"str_val",get_str_val,set_str_val,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_PublishEvent.EventArg));
	}
}
