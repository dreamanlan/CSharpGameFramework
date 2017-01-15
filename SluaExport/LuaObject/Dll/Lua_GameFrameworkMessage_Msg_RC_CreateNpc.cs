using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_CreateNpc : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc o;
			o=new GameFrameworkMessage.Msg_RC_CreateNpc();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_npc_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.npc_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_npc_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.npc_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_unit_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.unit_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_unit_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.unit_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cur_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cur_pos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cur_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			GameFrameworkMessage.Position v;
			checkType(l,2,out v);
			self.cur_pos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_face_direction(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.face_direction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_face_direction(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.face_direction=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_link_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.link_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_link_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.link_id=v;
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
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_owner_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.owner_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_owner_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.owner_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_leader_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.leader_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_leader_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.leader_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_key(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.key);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_key(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.key=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_level(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_level(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_CreateNpc self=(GameFrameworkMessage.Msg_RC_CreateNpc)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_CreateNpc");
		addMember(l,"npc_id",get_npc_id,set_npc_id,true);
		addMember(l,"unit_id",get_unit_id,set_unit_id,true);
		addMember(l,"cur_pos",get_cur_pos,set_cur_pos,true);
		addMember(l,"face_direction",get_face_direction,set_face_direction,true);
		addMember(l,"link_id",get_link_id,set_link_id,true);
		addMember(l,"camp_id",get_camp_id,set_camp_id,true);
		addMember(l,"owner_id",get_owner_id,set_owner_id,true);
		addMember(l,"leader_id",get_leader_id,set_leader_id,true);
		addMember(l,"key",get_key,set_key,true);
		addMember(l,"level",get_level,set_level,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_CreateNpc));
	}
}
