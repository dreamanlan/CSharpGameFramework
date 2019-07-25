using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_NpcSkill : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcSkill o;
			o=new GameFrameworkMessage.Msg_RC_NpcSkill();
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
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
	static public int get_skill_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
	static public int get_stand_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.stand_pos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_stand_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
			GameFrameworkMessage.Position v;
			checkType(l,2,out v);
			self.stand_pos=v;
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
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
	static public int get_target_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcSkill self=(GameFrameworkMessage.Msg_RC_NpcSkill)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_NpcSkill");
		addMember(l,"npc_id",get_npc_id,set_npc_id,true);
		addMember(l,"skill_id",get_skill_id,set_skill_id,true);
		addMember(l,"stand_pos",get_stand_pos,set_stand_pos,true);
		addMember(l,"face_direction",get_face_direction,set_face_direction,true);
		addMember(l,"target_id",get_target_id,set_target_id,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_NpcSkill));
	}
}
