using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_Plane : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.Plane o;
			o=new ScriptRuntime.Plane();
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
	static public int ctor__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Plane o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			o=new ScriptRuntime.Plane(a1,a2);
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
	static public int ctor__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Plane o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			o=new ScriptRuntime.Plane(a1,a2);
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
	static public int ctor__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Plane o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			o=new ScriptRuntime.Plane(a1,a2,a3);
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
	static public int ctor__Single__Single__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Plane o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			o=new ScriptRuntime.Plane(a1,a2,a3,a4);
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
	static public int Equals__Plane(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.Plane a1;
			checkValueType(l,2,out a1);
			var ret=self.Equals(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Equals__Object(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static new public int ToString(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			var ret=self.ToString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__BoundingBox(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,2,out a1);
			var ret=self.Intersects(a1);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__BoundingFrustum(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingFrustum a1;
			checkType(l,2,out a1);
			var ret=self.Intersects(a1);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,2,out a1);
			var ret=self.Intersects(a1);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__Ray(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.Ray a1;
			checkValueType(l,2,out a1);
			var ret=self.Intersects(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_BoundingBox__O_PlaneIntersectionStatus(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.PlaneIntersectionStatus a2;
			self.Intersects(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushEnum(l,(int)a2);
			setBack(l,(object)self);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_Ray__O_Single(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_BoundingSphere__O_PlaneIntersectionStatus(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.PlaneIntersectionStatus a2;
			self.Intersects(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushEnum(l,(int)a2);
			setBack(l,(object)self);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Transform__Matrix44__Plane_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Plane a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Plane.Transform(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Transform__R_Matrix44__R_Plane__O_Plane_s(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Equality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public int op_Inequality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int set_Normal(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Normal=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int set_D(IntPtr l) {
		try {
			ScriptRuntime.Plane self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.D=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Plane");
		addMember(l,ctor_s);
		addMember(l,ctor__Vector3__Vector3_s);
		addMember(l,ctor__Vector3__Single_s);
		addMember(l,ctor__Vector3__Vector3__Vector3_s);
		addMember(l,ctor__Single__Single__Single__Single_s);
		addMember(l,Equals__Plane);
		addMember(l,Equals__Object);
		addMember(l,ToString);
		addMember(l,Intersects__BoundingBox);
		addMember(l,Intersects__BoundingFrustum);
		addMember(l,Intersects__BoundingSphere);
		addMember(l,Intersects__Ray);
		addMember(l,Intersects__R_BoundingBox__O_PlaneIntersectionStatus);
		addMember(l,Intersects__R_Ray__O_Single);
		addMember(l,Intersects__R_BoundingSphere__O_PlaneIntersectionStatus);
		addMember(l,Transform__Matrix44__Plane_s);
		addMember(l,Transform__R_Matrix44__R_Plane__O_Plane_s);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,"Normal",get_Normal,set_Normal,true);
		addMember(l,"D",get_D,set_D,true);
		createTypeMetatable(l,null, typeof(ScriptRuntime.Plane),typeof(System.ValueType));
	}
}
