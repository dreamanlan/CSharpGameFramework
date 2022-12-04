using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_ClientInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.ClientInfo o;
			o=new GameFramework.ClientInfo();
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
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.ClientInfo.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Guid(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Guid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Guid(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.Guid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_RoleData(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RoleData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_RoleData(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			GameFrameworkMessage.RoleEnterResult v;
			checkType(l,2,out v);
			self.RoleData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_PropertyKey(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PropertyKey);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_PropertyKey(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.PropertyKey=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientInfo");
		addMember(l,ctor_s);
		addMember(l,"Instance",get_Instance,null,false);
		addMember(l,"Guid",get_Guid,set_Guid,true);
		addMember(l,"RoleData",get_RoleData,set_RoleData,true);
		addMember(l,"PropertyKey",get_PropertyKey,set_PropertyKey,true);
		createTypeMetatable(l,null, typeof(GameFramework.ClientInfo));
	}
}
