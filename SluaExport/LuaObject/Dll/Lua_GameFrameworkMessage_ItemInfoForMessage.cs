using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_ItemInfoForMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.ItemInfoForMessage o;
			o=new GameFrameworkMessage.ItemInfoForMessage();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ItemGuid(IntPtr l) {
		try {
			GameFrameworkMessage.ItemInfoForMessage self=(GameFrameworkMessage.ItemInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ItemGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ItemGuid(IntPtr l) {
		try {
			GameFrameworkMessage.ItemInfoForMessage self=(GameFrameworkMessage.ItemInfoForMessage)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.ItemInfoForMessage self=(GameFrameworkMessage.ItemInfoForMessage)checkSelf(l);
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
			GameFrameworkMessage.ItemInfoForMessage self=(GameFrameworkMessage.ItemInfoForMessage)checkSelf(l);
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
			GameFrameworkMessage.ItemInfoForMessage self=(GameFrameworkMessage.ItemInfoForMessage)checkSelf(l);
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
			GameFrameworkMessage.ItemInfoForMessage self=(GameFrameworkMessage.ItemInfoForMessage)checkSelf(l);
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
		getTypeTable(l,"GameFrameworkMessage.ItemInfoForMessage");
		addMember(l,"ItemGuid",get_ItemGuid,set_ItemGuid,true);
		addMember(l,"ItemId",get_ItemId,set_ItemId,true);
		addMember(l,"ItemNum",get_ItemNum,set_ItemNum,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.ItemInfoForMessage));
	}
}
