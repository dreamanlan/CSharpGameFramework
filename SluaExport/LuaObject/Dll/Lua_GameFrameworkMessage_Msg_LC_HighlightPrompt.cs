using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_LC_HighlightPrompt : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_HighlightPrompt o;
			o=new GameFrameworkMessage.Msg_LC_HighlightPrompt();
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
	static public int get_dict_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_HighlightPrompt self=(GameFrameworkMessage.Msg_LC_HighlightPrompt)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dict_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dict_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_HighlightPrompt self=(GameFrameworkMessage.Msg_LC_HighlightPrompt)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_HighlightPrompt");
		addMember(l,ctor_s);
		addMember(l,"dict_id",get_dict_id,set_dict_id,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_LC_HighlightPrompt));
	}
}
