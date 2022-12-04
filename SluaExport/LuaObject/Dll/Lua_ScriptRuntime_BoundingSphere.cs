using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_BoundingSphere : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere o;
			o=new ScriptRuntime.BoundingSphere();
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
			ScriptRuntime.BoundingSphere o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
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
	[UnityEngine.Scripting.Preserve]
	static public int Equals__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingSphere a1;
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
			ScriptRuntime.BoundingSphere self;
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
			ScriptRuntime.BoundingSphere self;
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
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingBox a1;
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
	static public int Intersects__BoundingFrustum(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingFrustum a1;
			checkType(l,2,out a1);
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
	static public int Intersects__Plane(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Plane a1;
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
			ScriptRuntime.BoundingSphere self;
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
	static public int Intersects__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingSphere a1;
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
	static public int Intersects__R_BoundingBox__O_Boolean(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,2,out a1);
			System.Boolean a2;
			self.Intersects(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			setBack(l,(object)self);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_Plane__O_PlaneIntersectionStatus(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Plane a1;
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_BoundingSphere__O_Boolean(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,2,out a1);
			System.Boolean a2;
			self.Intersects(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			setBack(l,(object)self);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Contains__BoundingBox(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,2,out a1);
			var ret=self.Contains(a1);
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
	static public int Contains__BoundingFrustum(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingFrustum a1;
			checkType(l,2,out a1);
			var ret=self.Contains(a1);
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
	static public int Contains__Vector3(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			var ret=self.Contains(a1);
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
	static public int Contains__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,2,out a1);
			var ret=self.Contains(a1);
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
	static public int Contains__R_BoundingBox__O_ClipStatus(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.ClipStatus a2;
			self.Contains(ref a1,out a2);
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
	static public int Contains__R_Vector3__O_ClipStatus(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.ClipStatus a2;
			self.Contains(ref a1,out a2);
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
	static public int Contains__R_BoundingSphere__O_ClipStatus(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.ClipStatus a2;
			self.Contains(ref a1,out a2);
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
	static public int Transform__Matrix44(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,2,out a1);
			var ret=self.Transform(a1);
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
	static public int Transform__R_Matrix44__O_BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.BoundingSphere a2;
			self.Transform(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			setBack(l,(object)self);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateFromBoundingBox__BoundingBox_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.BoundingSphere.CreateFromBoundingBox(a1);
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
	static public int CreateFromBoundingBox__R_BoundingBox__O_BoundingSphere_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.BoundingSphere a2;
			ScriptRuntime.BoundingSphere.CreateFromBoundingBox(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int op_Equality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public int op_Inequality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int set_Center(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Center=v;
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
	[UnityEngine.Scripting.Preserve]
	static public int set_Radius(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Radius=v;
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
		getTypeTable(l,"ScriptRuntime.BoundingSphere");
		addMember(l,ctor_s);
		addMember(l,ctor__Vector3__Single_s);
		addMember(l,Equals__BoundingSphere);
		addMember(l,Equals__Object);
		addMember(l,ToString);
		addMember(l,Intersects__BoundingBox);
		addMember(l,Intersects__BoundingFrustum);
		addMember(l,Intersects__Plane);
		addMember(l,Intersects__Ray);
		addMember(l,Intersects__BoundingSphere);
		addMember(l,Intersects__R_BoundingBox__O_Boolean);
		addMember(l,Intersects__R_Plane__O_PlaneIntersectionStatus);
		addMember(l,Intersects__R_Ray__O_Single);
		addMember(l,Intersects__R_BoundingSphere__O_Boolean);
		addMember(l,Contains__BoundingBox);
		addMember(l,Contains__BoundingFrustum);
		addMember(l,Contains__Vector3);
		addMember(l,Contains__BoundingSphere);
		addMember(l,Contains__R_BoundingBox__O_ClipStatus);
		addMember(l,Contains__R_Vector3__O_ClipStatus);
		addMember(l,Contains__R_BoundingSphere__O_ClipStatus);
		addMember(l,Transform__Matrix44);
		addMember(l,Transform__R_Matrix44__O_BoundingSphere);
		addMember(l,CreateFromBoundingBox__BoundingBox_s);
		addMember(l,CreateFromBoundingBox__R_BoundingBox__O_BoundingSphere_s);
		addMember(l,CreateFromFrustum_s);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,"Center",get_Center,set_Center,true);
		addMember(l,"Radius",get_Radius,set_Radius,true);
		createTypeMetatable(l,null, typeof(ScriptRuntime.BoundingSphere),typeof(System.ValueType));
	}
}
