using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_KdTreeObject : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.KdTreeObject o;
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			o=new GameFramework.KdTreeObject(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyFrom(IntPtr l) {
		try {
			GameFramework.KdTreeObject self=(GameFramework.KdTreeObject)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			self.CopyFrom(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Object(IntPtr l) {
		try {
			GameFramework.KdTreeObject self=(GameFramework.KdTreeObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Object);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Object(IntPtr l) {
		try {
			GameFramework.KdTreeObject self=(GameFramework.KdTreeObject)checkSelf(l);
			GameFramework.EntityInfo v;
			checkType(l,2,out v);
			self.Object=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Position(IntPtr l) {
		try {
			GameFramework.KdTreeObject self=(GameFramework.KdTreeObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Position);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Position(IntPtr l) {
		try {
			GameFramework.KdTreeObject self=(GameFramework.KdTreeObject)checkSelf(l);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Position=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Radius(IntPtr l) {
		try {
			GameFramework.KdTreeObject self=(GameFramework.KdTreeObject)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Radius);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Radius(IntPtr l) {
		try {
			GameFramework.KdTreeObject self=(GameFramework.KdTreeObject)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.Radius=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.KdTreeObject");
		addMember(l,CopyFrom);
		addMember(l,"Object",get_Object,set_Object,true);
		addMember(l,"Position",get_Position,set_Position,true);
		addMember(l,"Radius",get_Radius,set_Radius,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.KdTreeObject));
	}
}
