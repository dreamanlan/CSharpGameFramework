using System;

using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_BoundingSphere : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			o=new ScriptRuntime.BoundingSphere(a1,a2);
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
			if(matchType(l,argc,2,typeof(ScriptRuntime.Plane))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.Plane a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.Ray a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				System.Boolean a2;
				self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				setBack(l,self);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray),typeof(LuaOut))){
				ScriptRuntime.BoundingSphere self;
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
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				System.Boolean a2;
				self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				setBack(l,self);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Plane),typeof(LuaOut))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.Plane a1;
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
	static public int Contains(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.ClipStatus a2;
				self.Contains(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				setBack(l,self);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox),typeof(LuaOut))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.ClipStatus a2;
				self.Contains(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				setBack(l,self);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3),typeof(LuaOut))){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.ClipStatus a2;
				self.Contains(ref a1,out a2);
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
	static public int Transform(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,2,out a1);
				var ret=self.Transform(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.BoundingSphere self;
				checkValueType(l,1,out self);
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.BoundingSphere a2;
				self.Transform(ref a1,out a2);
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
	static public int CreateFromBoundingBox_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.BoundingSphere.CreateFromBoundingBox(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.BoundingSphere a2;
				ScriptRuntime.BoundingSphere.CreateFromBoundingBox(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
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
	static public int CreateFromPoints_s(IntPtr l) {
		try {
			System.Collections.Generic.IEnumerable<ScriptRuntime.Vector3> a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.BoundingSphere.CreateFromPoints(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateFromFrustum_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.BoundingSphere.CreateFromFrustum(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Equality(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.BoundingSphere a2;
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
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.BoundingSphere a2;
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
	static public int get_Center(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Center);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Center(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Center=v;
			setBack(l,self);
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
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
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
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Radius=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.BoundingSphere");
		addMember(l,Intersects);
		addMember(l,Contains);
		addMember(l,Transform);
		addMember(l,CreateFromBoundingBox_s);
		addMember(l,CreateFromPoints_s);
		addMember(l,CreateFromFrustum_s);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,"Center",get_Center,set_Center,true);
		addMember(l,"Radius",get_Radius,set_Radius,true);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.BoundingSphere),typeof(System.ValueType));
	}
}
