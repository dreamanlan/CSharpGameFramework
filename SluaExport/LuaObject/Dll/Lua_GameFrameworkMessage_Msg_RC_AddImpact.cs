using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_AddImpact : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact o;
			o=new GameFrameworkMessage.Msg_RC_AddImpact();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_sender_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.sender_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_sender_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.sender_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_target_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.target_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_target_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.target_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_impact_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.impact_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_impact_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.impact_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_skill_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_skill_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.skill_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_duration(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.duration);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_duration(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AddImpact self=(GameFrameworkMessage.Msg_RC_AddImpact)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.duration=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_AddImpact");
		addMember(l,"sender_id",get_sender_id,set_sender_id,true);
		addMember(l,"target_id",get_target_id,set_target_id,true);
		addMember(l,"impact_id",get_impact_id,set_impact_id,true);
		addMember(l,"skill_id",get_skill_id,set_skill_id,true);
		addMember(l,"duration",get_duration,set_duration,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_AddImpact));
	}
}
