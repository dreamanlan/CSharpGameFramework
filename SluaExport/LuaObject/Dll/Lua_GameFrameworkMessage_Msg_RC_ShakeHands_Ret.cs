using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_RC_ShakeHands_Ret : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ShakeHands_Ret o;
			o=new GameFrameworkMessage.Msg_RC_ShakeHands_Ret();
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
	static public int get_auth_result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ShakeHands_Ret self=(GameFrameworkMessage.Msg_RC_ShakeHands_Ret)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.auth_result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_auth_result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ShakeHands_Ret self=(GameFrameworkMessage.Msg_RC_ShakeHands_Ret)checkSelf(l);
			GameFrameworkMessage.Msg_RC_ShakeHands_Ret.RetType v;
			checkEnum(l,2,out v);
			self.auth_result=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_ShakeHands_Ret");
		addMember(l,ctor_s);
		addMember(l,"auth_result",get_auth_result,set_auth_result,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_RC_ShakeHands_Ret));
	}
}
