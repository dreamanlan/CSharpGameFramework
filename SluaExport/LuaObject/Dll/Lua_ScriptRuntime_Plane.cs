using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_Plane : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			ScriptRuntime.Plane o;
			if(argc==5){
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				o=new ScriptRuntime.Plane(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3),typeof(float))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				o=new ScriptRuntime.Plane(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,3,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,4,out a3);
				o=new ScriptRuntime.Plane(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,3,out a2);
				o=new ScriptRuntime.Plane(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==0){
				o=new ScriptRuntime.Plane();
				pushValue(l,true);
				pushObject(l,o);
				return 2;
			}
			return error(l,"New object failed.");
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
				ScriptRuntime.Plane self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray))){
				ScriptRuntime.Plane self;
				checkValueType(l,1,out self);
				ScriptRuntime.Ray a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.Plane self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.Plane self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.Plane self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.PlaneIntersectionStatus a2;
				self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				setBack(l,self);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray),typeof(LuaOut))){
				ScriptRuntime.Plane self;
				checkValueType(l,1,out self);
				ScriptRuntime.Ray a1;
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
				ScriptRuntime.Plane self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.PlaneIntersectionStatus a2;
				self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				setBack(l,self);
				return 3;
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
	static public int Transform_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Plane a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Plane.Transform(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Plane a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Plane a3;
				ScriptRuntime.Plane.Transform(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
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
			ScriptRuntime.Plane a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Plane a2;
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
			ScriptRuntime.Plane a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Plane a2;
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
	static public int get_Normal(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Normal);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Normal(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Normal=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_D(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.D);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_D(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.D=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Plane");
		addMember(l,Intersects);
		addMember(l,Transform_s);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,"Normal",get_Normal,set_Normal,true);
		addMember(l,"D",get_D,set_D,true);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.Plane),typeof(System.ValueType));
	}
}
