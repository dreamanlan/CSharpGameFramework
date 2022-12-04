using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_BoundingFrustum : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum o;
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
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
	[UnityEngine.Scripting.Preserve]
	static public int Contains__BoundingBox(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
	static public int Contains__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
	static public int Contains__Vector3(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
	static public int Contains__R_BoundingBox__O_ClipStatus(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			ScriptRuntime.BoundingBox a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.ClipStatus a2;
			self.Contains(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushEnum(l,(int)a2);
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
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			ScriptRuntime.BoundingSphere a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.ClipStatus a2;
			self.Contains(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushEnum(l,(int)a2);
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
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.ClipStatus a2;
			self.Contains(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushEnum(l,(int)a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Equals__BoundingFrustum(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			ScriptRuntime.BoundingFrustum a1;
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
	static public int Equals__Object(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
	static public int GetCorners(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			ScriptRuntime.Vector3[] a1;
			checkArray(l,2,out a1);
			self.GetCorners(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__BoundingBox(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
	static public int Intersects__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
	static public int Intersects__Plane(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_BoundingSphere__O_Boolean(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_Plane__O_PlaneIntersectionStatus(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
			ScriptRuntime.Plane a1;
			checkValueType(l,2,out a1);
			ScriptRuntime.PlaneIntersectionStatus a2;
			self.Intersects(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushEnum(l,(int)a2);
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static new public int ToString(IntPtr l) {
		try {
			ScriptRuntime.BoundingFrustum self=(ScriptRuntime.BoundingFrustum)checkSelf(l);
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
	static public int op_Equality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public int op_Inequality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.BoundingFrustum");
		addMember(l,ctor_s);
		addMember(l,Contains__BoundingBox);
		addMember(l,Contains__BoundingFrustum);
		addMember(l,Contains__BoundingSphere);
		addMember(l,Contains__Vector3);
		addMember(l,Contains__R_BoundingBox__O_ClipStatus);
		addMember(l,Contains__R_BoundingSphere__O_ClipStatus);
		addMember(l,Contains__R_Vector3__O_ClipStatus);
		addMember(l,Equals__BoundingFrustum);
		addMember(l,Equals__Object);
		addMember(l,GetCorners);
		addMember(l,GetCorners__A_Vector3);
		addMember(l,Intersects__BoundingBox);
		addMember(l,Intersects__BoundingFrustum);
		addMember(l,Intersects__BoundingSphere);
		addMember(l,Intersects__Plane);
		addMember(l,Intersects__Ray);
		addMember(l,Intersects__R_BoundingBox__O_Boolean);
		addMember(l,Intersects__R_BoundingSphere__O_Boolean);
		addMember(l,Intersects__R_Plane__O_PlaneIntersectionStatus);
		addMember(l,Intersects__R_Ray__O_Single);
		addMember(l,ToString);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,"CornerCount",get_CornerCount,null,false);
		addMember(l,"Bottom",get_Bottom,null,true);
		addMember(l,"Far",get_Far,null,true);
		addMember(l,"Left",get_Left,null,true);
		addMember(l,"Matrix",get_Matrix,set_Matrix,true);
		addMember(l,"Near",get_Near,null,true);
		addMember(l,"Right",get_Right,null,true);
		addMember(l,"Top",get_Top,null,true);
		createTypeMetatable(l,null, typeof(ScriptRuntime.BoundingFrustum));
	}
}
