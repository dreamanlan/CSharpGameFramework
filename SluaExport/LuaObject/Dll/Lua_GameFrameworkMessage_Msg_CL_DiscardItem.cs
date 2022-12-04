using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CL_DiscardItem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_DiscardItem o;
			o=new GameFrameworkMessage.Msg_CL_DiscardItem();
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
	static public int get_ItemGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_DiscardItem self=(GameFrameworkMessage.Msg_CL_DiscardItem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ItemGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ItemGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_DiscardItem self=(GameFrameworkMessage.Msg_CL_DiscardItem)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.ItemGuid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_DiscardItem");
		addMember(l,ctor_s);
		addMember(l,"ItemGuid",get_ItemGuid,set_ItemGuid,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CL_DiscardItem));
	}
}
