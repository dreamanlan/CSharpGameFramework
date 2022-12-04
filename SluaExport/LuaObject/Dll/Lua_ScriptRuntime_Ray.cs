using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_Ray : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.Ray o;
			o=new ScriptRuntime.Ray();
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
			ScriptRuntime.Ray o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
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
	[UnityEngine.Scripting.Preserve]
	static public int Equals__Ray(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
			ScriptRuntime.Ray a1;
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
			ScriptRuntime.Ray self;
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
			ScriptRuntime.Ray self;
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
			ScriptRuntime.Ray self;
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
			ScriptRuntime.Ray self;
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
	static public int Intersects__BoundingSphere(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
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
	static public int Intersects__Plane(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
			ScriptRuntime.Plane a1;
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
	static public int Intersects__R_BoundingBox__O_Single(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_BoundingSphere__O_Single(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersects__R_Plane__O_Single(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Equality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public int op_Inequality_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int set_Position(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Position=v;
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
	[UnityEngine.Scripting.Preserve]
	static public int set_Direction(IntPtr l) {
		try {
			ScriptRuntime.Ray self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Direction=v;
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
		getTypeTable(l,"ScriptRuntime.Ray");
		addMember(l,ctor_s);
		addMember(l,ctor__Vector3__Vector3_s);
		addMember(l,Equals__Ray);
		addMember(l,Equals__Object);
		addMember(l,ToString);
		addMember(l,Intersects__BoundingBox);
		addMember(l,Intersects__BoundingFrustum);
		addMember(l,Intersects__BoundingSphere);
		addMember(l,Intersects__Plane);
		addMember(l,Intersects__R_BoundingBox__O_Single);
		addMember(l,Intersects__R_BoundingSphere__O_Single);
		addMember(l,Intersects__R_Plane__O_Single);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,"Position",get_Position,set_Position,true);
		addMember(l,"Direction",get_Direction,set_Direction,true);
		createTypeMetatable(l,null, typeof(ScriptRuntime.Ray),typeof(System.ValueType));
	}
}
