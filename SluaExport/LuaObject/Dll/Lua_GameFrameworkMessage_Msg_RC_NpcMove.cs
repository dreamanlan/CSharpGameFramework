using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_NpcMove : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcMove o;
			o=new GameFrameworkMessage.Msg_RC_NpcMove();
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
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
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
	static public int get_velocity(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.velocity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_velocity(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.velocity=v;
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
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
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
	static public int get_target_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.target_pos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_target_pos(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcMove self=(GameFrameworkMessage.Msg_RC_NpcMove)checkSelf(l);
			GameFrameworkMessage.Position v;
			checkType(l,2,out v);
			self.target_pos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_NpcMove");
		addMember(l,"npc_id",get_npc_id,set_npc_id,true);
		addMember(l,"velocity",get_velocity,set_velocity,true);
		addMember(l,"cur_pos",get_cur_pos,set_cur_pos,true);
		addMember(l,"target_pos",get_target_pos,set_target_pos,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_NpcMove));
	}
}
