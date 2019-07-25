using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_DebugSpaceInfo_DebugSpaceInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo o;
			o=new GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo();
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
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
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
	static public int get_is_player(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.is_player);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_is_player(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.is_player=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_pos_x(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.pos_x);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_pos_x(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.pos_x=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_pos_z(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.pos_z);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_pos_z(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.pos_z=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_face_dir(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.face_dir);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_face_dir(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo self=(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.face_dir=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo");
		addMember(l,"obj_id",get_obj_id,set_obj_id,true);
		addMember(l,"is_player",get_is_player,set_is_player,true);
		addMember(l,"pos_x",get_pos_x,set_pos_x,true);
		addMember(l,"pos_z",get_pos_z,set_pos_z,true);
		addMember(l,"face_dir",get_face_dir,set_face_dir,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_DebugSpaceInfo.DebugSpaceInfo));
	}
}
