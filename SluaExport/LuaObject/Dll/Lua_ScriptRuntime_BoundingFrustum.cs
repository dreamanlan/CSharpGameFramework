using System;

using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_BoundingFrustum : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum o;
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,2,out a1);
			o=new ScriptRuntime.BoundingFrustum(a1);
			pushValue(l,true);
			pushValue(l,o);
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
			if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Contains(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3),typeof(LuaOut))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.ClipStatus a2;
				self.Contains(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.ClipStatus a2;
				self.Contains(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox),typeof(LuaOut))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.ClipStatus a2;
				self.Contains(ref a1,out a2);
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
	static public int GetCorners(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				var ret=self.GetCorners();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.Vector3[] a1;
				checkArray(l,2,out a1);
				self.GetCorners(a1);
				pushValue(l,true);
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
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.Plane a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.Ray a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingBox))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingFrustum))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingFrustum a1;
				checkType(l,2,out a1);
				var ret=self.Intersects(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Plane),typeof(LuaOut))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.Plane a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.PlaneIntersectionStatus a2;
				self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Ray),typeof(LuaOut))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingBox a1;
				checkValueType(l,2,out a1);
				System.Boolean a2;
				self.Intersects(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.BoundingSphere),typeof(LuaOut))){
				ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
				ScriptRuntime.BoundingSphere a1;
				checkValueType(l,2,out a1);
				System.Boolean a2;
				self.Intersects(ref a1,out a2);
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
	static public int op_Equality(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum a1;
			checkType(l,1,out a1);
			ScriptRuntime.BoundingFrustum a2;
			checkType(l,2,out a2);
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
			ScriptRuntime.BoundingFrustum a1;
			checkType(l,1,out a1);
			ScriptRuntime.BoundingFrustum a2;
			checkType(l,2,out a2);
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
			pushValue(l,ScriptRuntime.BoundingFrustum.CornerCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Bottom(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Bottom);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Far(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Far);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Left(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Left);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Matrix(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Matrix);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Matrix(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			ScriptRuntime.Matrix44 v;
			checkValueType(l,2,out v);
			self.Matrix=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Near(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Near);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Right(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Right);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Top(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Top);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.BoundingFrustum");
		addMember(l,Contains);
		addMember(l,GetCorners);
		addMember(l,Intersects);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,"CornerCount",get_CornerCount,null,false);
		addMember(l,"Bottom",get_Bottom,null,true);
		addMember(l,"Far",get_Far,null,true);
		addMember(l,"Left",get_Left,null,true);
		addMember(l,"Matrix",get_Matrix,set_Matrix,true);
		addMember(l,"Near",get_Near,null,true);
		addMember(l,"Right",get_Right,null,true);
		addMember(l,"Top",get_Top,null,true);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.BoundingFrustum));
	}
}
