using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_RemoveImpact : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_RemoveImpact o;
			o=new GameFrameworkMessage.Msg_RC_RemoveImpact();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_obj_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_RemoveImpact self=(GameFrameworkMessage.Msg_RC_RemoveImpact)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.obj_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_obj_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_RemoveImpact self=(GameFrameworkMessage.Msg_RC_RemoveImpact)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.obj_id=v;
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
			GameFrameworkMessage.Msg_RC_RemoveImpact self=(GameFrameworkMessage.Msg_RC_RemoveImpact)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_RemoveImpact self=(GameFrameworkMessage.Msg_RC_RemoveImpact)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_RemoveImpact");
		addMember(l,"obj_id",get_obj_id,set_obj_id,true);
		addMember(l,"impact_id",get_impact_id,set_impact_id,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_RemoveImpact));
	}
}
