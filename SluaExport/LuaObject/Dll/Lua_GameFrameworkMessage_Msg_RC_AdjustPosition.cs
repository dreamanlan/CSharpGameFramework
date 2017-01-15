using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_AdjustPosition : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AdjustPosition o;
			o=new GameFrameworkMessage.Msg_RC_AdjustPosition();
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
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
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
	static public int get_x(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_x(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.x=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_z(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.z);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_z(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.z=v;
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
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_AdjustPosition self=(GameFrameworkMessage.Msg_RC_AdjustPosition)checkSelf(l);
			int v;
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
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_AdjustPosition");
		addMember(l,"role_id",get_role_id,set_role_id,true);
		addMember(l,"x",get_x,set_x,true);
		addMember(l,"z",get_z,set_z,true);
		addMember(l,"face_dir",get_face_dir,set_face_dir,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_AdjustPosition));
	}
}
