using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_Ray : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ScriptRuntime.Ray o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,3,out a2);
			o=new ScriptRuntime.Ray(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Intersects(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere))){
				ScriptRuntime.Ray self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Plane))){
				ScriptRuntime.Ray self;
				checkValueType(l,1,out self);
				ScriptRuntime.Plane a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.Ray self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.Ray self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Plane),typeof(LuaOut))){
				ScriptRuntime.Ray self;
				checkValueType(l,1,out self);
				ScriptRuntime.Plane a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				var ret=self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				pushValue(l,a2);
				return 4;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox),typeof(LuaOut))){
				ScriptRuntime.Ray self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				var ret=self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				pushValue(l,a2);
				return 4;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.Ray self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				var ret=self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				pushValue(l,a2);
				return 4;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Equality(IntPtr l) {
		try {
			ScriptRuntime.Ray a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Ray a2;
			checkValueType(l,2,out a2);
			var ret=(a1==a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Inequality(IntPtr l) {
		try {
			ScriptRuntime.Ray a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Ray a2;
			checkValueType(l,2,out a2);
			var ret=(a1!=a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Position(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
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
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Position=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Direction(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Direction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Direction(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Direction=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Ray");
		addMember(l,Intersects);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,"Position",get_Position,set_Position,true);
		addMember(l,"Direction",get_Direction,set_Direction,true);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.Ray),typeof(System.ValueType));
	}
}
