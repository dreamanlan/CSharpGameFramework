using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_RC_NpcStopSkill : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcStopSkill o;
			o=new GameFrameworkMessage.Msg_RC_NpcStopSkill();
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
	static public int get_npc_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcStopSkill self=(GameFrameworkMessage.Msg_RC_NpcStopSkill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.npc_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_npc_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_NpcStopSkill self=(GameFrameworkMessage.Msg_RC_NpcStopSkill)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_NpcStopSkill");
		addMember(l,ctor_s);
		addMember(l,"npc_id",get_npc_id,set_npc_id,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_RC_NpcStopSkill));
	}
}
