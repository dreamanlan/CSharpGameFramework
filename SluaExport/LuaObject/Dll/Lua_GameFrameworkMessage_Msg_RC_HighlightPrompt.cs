using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_HighlightPrompt : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_HighlightPrompt o;
			o=new GameFrameworkMessage.Msg_RC_HighlightPrompt();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_dict_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_HighlightPrompt self=(GameFrameworkMessage.Msg_RC_HighlightPrompt)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dict_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_dict_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_HighlightPrompt self=(GameFrameworkMessage.Msg_RC_HighlightPrompt)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.dict_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_argument(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_HighlightPrompt self=(GameFrameworkMessage.Msg_RC_HighlightPrompt)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.argument);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_HighlightPrompt");
		addMember(l,"dict_id",get_dict_id,set_dict_id,true);
		addMember(l,"argument",get_argument,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_HighlightPrompt));
	}
}
