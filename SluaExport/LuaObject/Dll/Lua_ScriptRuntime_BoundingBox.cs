using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_BoundingBox : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox o;
			o=new ScriptRuntime.BoundingBox();
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
			ScriptRuntime.BoundingBox o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetCorners(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			var ret=self.GetCorners();
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
	static public int GetCorners__A_Vector3(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3[] a1;
			checkArray(l,2,out a1);
			self.GetCorners(a1);
			pushValue(l,true);
			setBack(l,(object)self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Equals__BoundingBox(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			ScriptRuntime.BoundingBox a1;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
	static public int Intersects__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
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
	static public int Intersects__Ray(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
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
	static public int Intersects__R_BoundingBox__O_Boolean(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
	static public int Intersects__R_BoundingSphere__O_Boolean(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
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
	static public int Intersects__R_Ray__O_Single(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Contains__BoundingBox(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
			ScriptRuntime.BoundingBox self;
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
	static public int Transform__R_Matrix44__O_BoundingBox(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.BoundingBox a2;
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
	static public int CreateFromSphere__BoundingSphere_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.BoundingBox.CreateFromSphere(a1);
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
	static public int CreateFromSphere__R_BoundingSphere__O_BoundingBox_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.BoundingBox a2;
			ScriptRuntime.BoundingBox.CreateFromSphere(ref a1,out a2);
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
	static public int op_Equality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public int op_Inequality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int set_Min(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Min=v;
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
	[UnityEngine.Scripting.Preserve]
	static public int set_Max(IntPtr l) {
		try {
			ScriptRuntime.BoundingBox self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Max=v;
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
		getTypeTable(l,"ScriptRuntime.BoundingBox");
		addMember(l,ctor_s);
		addMember(l,ctor__Vector3__Vector3_s);
		addMember(l,GetCorners);
		addMember(l,GetCorners__A_Vector3);
		addMember(l,Equals__BoundingBox);
		addMember(l,Equals__Object);
		addMember(l,ToString);
		addMember(l,Intersects__BoundingBox);
		addMember(l,Intersects__BoundingFrustum);
		addMember(l,Intersects__Plane);
		addMember(l,Intersects__BoundingSphere);
		addMember(l,Intersects__Ray);
		addMember(l,Intersects__R_BoundingBox__O_Boolean);
		addMember(l,Intersects__R_Plane__O_PlaneIntersectionStatus);
		addMember(l,Intersects__R_BoundingSphere__O_Boolean);
		addMember(l,Intersects__R_Ray__O_Single);
		addMember(l,Contains__BoundingBox);
		addMember(l,Contains__BoundingFrustum);
		addMember(l,Contains__Vector3);
		addMember(l,Contains__BoundingSphere);
		addMember(l,Contains__R_BoundingBox__O_ClipStatus);
		addMember(l,Contains__R_Vector3__O_ClipStatus);
		addMember(l,Contains__R_BoundingSphere__O_ClipStatus);
		addMember(l,Transform__Matrix44);
		addMember(l,Transform__R_Matrix44__O_BoundingBox);
		addMember(l,CreateFromSphere__BoundingSphere_s);
		addMember(l,CreateFromSphere__R_BoundingSphere__O_BoundingBox_s);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,"CornerCount",get_CornerCount,null,false);
		addMember(l,"Min",get_Min,set_Min,true);
		addMember(l,"Max",get_Max,set_Max,true);
		createTypeMetatable(l,null, typeof(ScriptRuntime.BoundingBox),typeof(System.ValueType));
	}
}
