using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_UseItem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_UseItem o;
			o=new GameFrameworkMessage.Msg_CL_UseItem();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_UseItem self=(GameFrameworkMessage.Msg_CL_UseItem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ItemId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_UseItem self=(GameFrameworkMessage.Msg_CL_UseItem)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ItemId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ItemNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_UseItem self=(GameFrameworkMessage.Msg_CL_UseItem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ItemNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ItemNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_UseItem self=(GameFrameworkMessage.Msg_CL_UseItem)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ItemNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_UseItem");
		addMember(l,"ItemId",get_ItemId,set_ItemId,true);
		addMember(l,"ItemNum",get_ItemNum,set_ItemNum,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_UseItem));
	}
}
