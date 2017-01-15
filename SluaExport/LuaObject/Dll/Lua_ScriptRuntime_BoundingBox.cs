using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_BoundingBox : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,3,out a2);
			o=new ScriptRuntime.BoundingBox(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetCorners(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				var ret=self.GetCorners();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.Vector3[] a1;
				checkArray(l,2,out a1);
				self.GetCorners(a1);
				pushValue(l,true);
				setBack(l,self);
				return 1;
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
	static public int Intersects(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ScriptRuntime.Plane))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.Plane a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.Ray a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray),typeof(LuaOut))){
				ScriptRuntime.BoundingBox self;
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
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.BoundingBox self;
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
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox),typeof(LuaOut))){
				ScriptRuntime.BoundingBox self;
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
				ScriptRuntime.BoundingBox self;
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
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.BoundingBox self;
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
				ScriptRuntime.BoundingBox self;
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
				ScriptRuntime.BoundingBox self;
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
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,2,out a1);
				var ret=self.Transform(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.BoundingBox self;
				checkValueType(l,1,out self);
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.BoundingBox a2;
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
	static public int CreateFromSphere_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.BoundingBox.CreateFromSphere(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.BoundingBox a2;
				ScriptRuntime.BoundingBox.CreateFromSphere(ref a1,out a2);
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
			var ret=ScriptRuntime.BoundingBox.CreateFromPoints(a1);
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
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.BoundingBox a2;
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
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.BoundingBox a2;
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
	static public int get_CornerCount(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.BoundingBox.CornerCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Min(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Min);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Min(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Min=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Max(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Max);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Max(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Max=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.BoundingBox");
		addMember(l,GetCorners);
		addMember(l,Intersects);
		addMember(l,Contains);
		addMember(l,Transform);
		addMember(l,CreateFromSphere_s);
		addMember(l,CreateFromPoints_s);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,"CornerCount",get_CornerCount,null,false);
		addMember(l,"Min",get_Min,set_Min,true);
		addMember(l,"Max",get_Max,set_Max,true);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.BoundingBox),typeof(System.ValueType));
	}
}
