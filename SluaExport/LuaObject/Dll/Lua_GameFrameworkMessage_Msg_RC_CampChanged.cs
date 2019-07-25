using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_CampChanged : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CampChanged o;
			o=new GameFrameworkMessage.Msg_RC_CampChanged();
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
			GameFrameworkMessage.Msg_RC_CampChanged self=(GameFrameworkMessage.Msg_RC_CampChanged)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_CampChanged self=(GameFrameworkMessage.Msg_RC_CampChanged)checkSelf(l);
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
	static public int get_camp_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CampChanged self=(GameFrameworkMessage.Msg_RC_CampChanged)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.camp_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_camp_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CampChanged self=(GameFrameworkMessage.Msg_RC_CampChanged)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.camp_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_CampChanged");
		addMember(l,"obj_id",get_obj_id,set_obj_id,true);
		addMember(l,"camp_id",get_camp_id,set_camp_id,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_CampChanged));
	}
}
