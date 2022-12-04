using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_RC_LockFrame : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_LockFrame");
		addMember(l,ctor_s);
		addMember(l,"scale",get_scale,set_scale,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_RC_LockFrame));
	}
}
