using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_SendGfxMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage o;
			o=new GameFrameworkMessage.Msg_LC_SendGfxMessage();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_is_with_tag(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage self=(GameFrameworkMessage.Msg_LC_SendGfxMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.is_with_tag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_is_with_tag(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage self=(GameFrameworkMessage.Msg_LC_SendGfxMessage)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.is_with_tag=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_name(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage self=(GameFrameworkMessage.Msg_LC_SendGfxMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_name(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage self=(GameFrameworkMessage.Msg_LC_SendGfxMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_msg(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage self=(GameFrameworkMessage.Msg_LC_SendGfxMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.msg);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_msg(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage self=(GameFrameworkMessage.Msg_LC_SendGfxMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.msg=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_args(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SendGfxMessage self=(GameFrameworkMessage.Msg_LC_SendGfxMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.args);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_SendGfxMessage");
		addMember(l,"is_with_tag",get_is_with_tag,set_is_with_tag,true);
		addMember(l,"name",get_name,set_name,true);
		addMember(l,"msg",get_msg,set_msg,true);
		addMember(l,"args",get_args,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_SendGfxMessage));
	}
}
