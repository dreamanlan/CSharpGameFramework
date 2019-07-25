using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_SyncProperty : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty o;
			o=new GameFrameworkMessage.Msg_RC_SyncProperty();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_role_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.role_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_role_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.role_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_hp(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.hp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_hp(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.hp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_np(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.np);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_np(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.np=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_shield(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.shield);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_shield(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.shield=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_state(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.state);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_state(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_SyncProperty self=(GameFrameworkMessage.Msg_RC_SyncProperty)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.state=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_SyncProperty");
		addMember(l,"role_id",get_role_id,set_role_id,true);
		addMember(l,"hp",get_hp,set_hp,true);
		addMember(l,"np",get_np,set_np,true);
		addMember(l,"shield",get_shield,set_shield,true);
		addMember(l,"state",get_state,set_state,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_SyncProperty));
	}
}
