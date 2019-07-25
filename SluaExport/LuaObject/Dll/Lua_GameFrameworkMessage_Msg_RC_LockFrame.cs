using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_LockFrame : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_LockFrame o;
			o=new GameFrameworkMessage.Msg_RC_LockFrame();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_scale(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_LockFrame self=(GameFrameworkMessage.Msg_RC_LockFrame)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.scale);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_scale(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_LockFrame self=(GameFrameworkMessage.Msg_RC_LockFrame)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.scale=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_LockFrame");
		addMember(l,"scale",get_scale,set_scale,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_LockFrame));
	}
}
