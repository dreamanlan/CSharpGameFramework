using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_MemberInfoForMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.MemberInfoForMessage o;
			o=new GameFrameworkMessage.MemberInfoForMessage();
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
	static public int get_Hero(IntPtr l) {
		try {
			GameFrameworkMessage.MemberInfoForMessage self=(GameFrameworkMessage.MemberInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Hero);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Hero(IntPtr l) {
		try {
			GameFrameworkMessage.MemberInfoForMessage self=(GameFrameworkMessage.MemberInfoForMessage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Hero=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Level(IntPtr l) {
		try {
			GameFrameworkMessage.MemberInfoForMessage self=(GameFrameworkMessage.MemberInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Level(IntPtr l) {
		try {
			GameFrameworkMessage.MemberInfoForMessage self=(GameFrameworkMessage.MemberInfoForMessage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.MemberInfoForMessage");
		addMember(l,ctor_s);
		addMember(l,"Hero",get_Hero,set_Hero,true);
		addMember(l,"Level",get_Level,set_Level,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.MemberInfoForMessage));
	}
}
