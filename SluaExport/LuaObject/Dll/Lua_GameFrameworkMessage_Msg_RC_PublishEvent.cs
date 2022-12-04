using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_RC_PublishEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent o;
			o=new GameFrameworkMessage.Msg_RC_PublishEvent();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_is_logic_event(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent self=(GameFrameworkMessage.Msg_RC_PublishEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.is_logic_event);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_is_logic_event(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent self=(GameFrameworkMessage.Msg_RC_PublishEvent)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.is_logic_event=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ev_name(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent self=(GameFrameworkMessage.Msg_RC_PublishEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ev_name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ev_name(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent self=(GameFrameworkMessage.Msg_RC_PublishEvent)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ev_name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_group(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent self=(GameFrameworkMessage.Msg_RC_PublishEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.group);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_group(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_PublishEvent self=(GameFrameworkMessage.Msg_RC_PublishEvent)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.group=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_PublishEvent");
		addMember(l,ctor_s);
		addMember(l,"is_logic_event",get_is_logic_event,set_is_logic_event,true);
		addMember(l,"ev_name",get_ev_name,set_ev_name,true);
		addMember(l,"group",get_group,set_group,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_RC_PublishEvent));
	}
}
