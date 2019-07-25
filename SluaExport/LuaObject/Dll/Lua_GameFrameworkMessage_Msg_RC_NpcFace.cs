using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_NpcFace : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcFace o;
			o=new GameFrameworkMessage.Msg_RC_NpcFace();
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
			GameFrameworkMessage.Msg_RC_NpcFace self=(GameFrameworkMessage.Msg_RC_NpcFace)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcFace self=(GameFrameworkMessage.Msg_RC_NpcFace)checkSelf(l);
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
	static public int get_face_direction(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcFace self=(GameFrameworkMessage.Msg_RC_NpcFace)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_NpcFace self=(GameFrameworkMessage.Msg_RC_NpcFace)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_NpcFace");
		addMember(l,"npc_id",get_npc_id,set_npc_id,true);
		addMember(l,"face_direction",get_face_direction,set_face_direction,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_NpcFace));
	}
}
