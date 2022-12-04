using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_FriendInfoForMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage o;
			o=new GameFrameworkMessage.FriendInfoForMessage();
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
	static public int get_FriendGuid(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FriendGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_FriendGuid(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.FriendGuid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_FriendNickname(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FriendNickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_FriendNickname(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.FriendNickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_QQ(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.QQ);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_QQ(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.QQ=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_weixin(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.weixin);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_weixin(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.weixin=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsBlack(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsBlack);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsBlack(IntPtr l) {
		try {
			GameFrameworkMessage.FriendInfoForMessage self=(GameFrameworkMessage.FriendInfoForMessage)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsBlack=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.FriendInfoForMessage");
		addMember(l,ctor_s);
		addMember(l,"FriendGuid",get_FriendGuid,set_FriendGuid,true);
		addMember(l,"FriendNickname",get_FriendNickname,set_FriendNickname,true);
		addMember(l,"QQ",get_QQ,set_QQ,true);
		addMember(l,"weixin",get_weixin,set_weixin,true);
		addMember(l,"IsBlack",get_IsBlack,set_IsBlack,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.FriendInfoForMessage));
	}
}
