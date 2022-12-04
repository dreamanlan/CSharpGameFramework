using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CR_DlgClosed : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_DlgClosed o;
			o=new GameFrameworkMessage.Msg_CR_DlgClosed();
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
	static public int get_dialog_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_DlgClosed self=(GameFrameworkMessage.Msg_CR_DlgClosed)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dialog_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dialog_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_DlgClosed self=(GameFrameworkMessage.Msg_CR_DlgClosed)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.dialog_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_DlgClosed");
		addMember(l,ctor_s);
		addMember(l,"dialog_id",get_dialog_id,set_dialog_id,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CR_DlgClosed));
	}
}
