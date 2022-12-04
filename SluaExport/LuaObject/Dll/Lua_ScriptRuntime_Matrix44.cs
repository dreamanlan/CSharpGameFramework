using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_Matrix44 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 o;
			o=new ScriptRuntime.Matrix44();
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
	static public int ctor__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			System.Single a6;
			checkType(l,6,out a6);
			System.Single a7;
			checkType(l,7,out a7);
			System.Single a8;
			checkType(l,8,out a8);
			System.Single a9;
			checkType(l,9,out a9);
			System.Single a10;
			checkType(l,10,out a10);
			System.Single a11;
			checkType(l,11,out a11);
			System.Single a12;
			checkType(l,12,out a12);
			System.Single a13;
			checkType(l,13,out a13);
			System.Single a14;
			checkType(l,14,out a14);
			System.Single a15;
			checkType(l,15,out a15);
			System.Single a16;
			checkType(l,16,out a16);
			o=new ScriptRuntime.Matrix44(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14,a15,a16);
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
	static public int GetRow(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetRow(a1);
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
	static public int SetRow(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,3,out a2);
			self.SetRow(a1,a2);
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
	static public int GetColumn(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetColumn(a1);
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
	static public int SetColumn(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,3,out a2);
			self.SetColumn(a1,a2);
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
	static public int Decompose(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 a1;
			ScriptRuntime.Quaternion a2;
			ScriptRuntime.Vector3 a3;
			self.Decompose(out a1,out a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			setBack(l,(object)self);
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
			ScriptRuntime.Matrix44 self;
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
	static public int Equals__Matrix44(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Matrix44 a1;
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
			ScriptRuntime.Matrix44 self;
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
	static public int Determinant(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			var ret=self.Determinant();
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
	static public int CreateTranslation__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.CreateTranslation(a1);
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
	static public int CreateTranslation__R_Vector3__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.CreateTranslation(ref a1,out a2);
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
	static public int CreateScale__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.CreateScale(a1);
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
	static public int CreateScale__R_Vector3__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.CreateScale(ref a1,out a2);
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
	static public int CreatePerspectiveFieldOfView__Single__Single__Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=ScriptRuntime.Matrix44.CreatePerspectiveFieldOfView(a1,a2,a3,a4);
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
	static public int CreatePerspectiveFieldOfView__Single__Single__Single__Single__O_Matrix44_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			ScriptRuntime.Matrix44 a5;
			ScriptRuntime.Matrix44.CreatePerspectiveFieldOfView(a1,a2,a3,a4,out a5);
			pushValue(l,true);
			pushValue(l,a5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreatePerspective__Single__Single__Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=ScriptRuntime.Matrix44.CreatePerspective(a1,a2,a3,a4);
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
	static public int CreatePerspective__Single__Single__Single__Single__O_Matrix44_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			ScriptRuntime.Matrix44 a5;
			ScriptRuntime.Matrix44.CreatePerspective(a1,a2,a3,a4,out a5);
			pushValue(l,true);
			pushValue(l,a5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateOrthographic__Single__Single__Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=ScriptRuntime.Matrix44.CreateOrthographic(a1,a2,a3,a4);
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
	static public int CreateOrthographic__Single__Single__Single__Single__O_Matrix44_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			ScriptRuntime.Matrix44 a5;
			ScriptRuntime.Matrix44.CreateOrthographic(a1,a2,a3,a4,out a5);
			pushValue(l,true);
			pushValue(l,a5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateLookAt__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=ScriptRuntime.Matrix44.CreateLookAt(a1,a2,a3);
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
	static public int CreateLookAt__R_Vector3__R_Vector3__R_Vector3__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Matrix44 a4;
			ScriptRuntime.Matrix44.CreateLookAt(ref a1,ref a2,ref a3,out a4);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			return 5;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateFromQuaternion__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.CreateFromQuaternion(a1);
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
	static public int CreateFromQuaternion__R_Quaternion__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.CreateFromQuaternion(ref a1,out a2);
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
	static public int CreateFromYawPitchRoll__Single__Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Matrix44.CreateFromYawPitchRoll(a1,a2,a3);
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
	static public int CreateFromYawPitchRoll__Single__Single__Single__O_Matrix44_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Matrix44 a4;
			ScriptRuntime.Matrix44.CreateFromYawPitchRoll(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,a4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateRotationX__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.CreateRotationX(a1);
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
	static public int CreateRotationX__Single__O_Matrix44_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.CreateRotationX(a1,out a2);
			pushValue(l,true);
			pushValue(l,a2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateRotationY__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.CreateRotationY(a1);
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
	static public int CreateRotationY__Single__O_Matrix44_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.CreateRotationY(a1,out a2);
			pushValue(l,true);
			pushValue(l,a2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateRotationZ__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.CreateRotationZ(a1);
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
	static public int CreateRotationZ__Single__O_Matrix44_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.CreateRotationZ(a1,out a2);
			pushValue(l,true);
			pushValue(l,a2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateFromAxisAngle__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.Matrix44.CreateFromAxisAngle(a1,a2);
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
	static public int CreateFromAxisAngle__R_Vector3__Single__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			ScriptRuntime.Matrix44 a3;
			ScriptRuntime.Matrix44.CreateFromAxisAngle(ref a1,a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Transpose__Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.Transpose(a1);
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
	static public int Transpose__R_Matrix44__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.Transpose(ref a1,out a2);
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
	static public int Invert__Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Matrix44.Invert(a1);
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
	static public int Invert__R_Matrix44__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			ScriptRuntime.Matrix44.Invert(ref a1,out a2);
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
	static public int Add__Matrix44__Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Matrix44.Add(a1,a2);
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
	static public int Add__R_Matrix44__R_Matrix44__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Matrix44 a3;
			ScriptRuntime.Matrix44.Add(ref a1,ref a2,out a3);
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
	static public int Sub__Matrix44__Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Matrix44.Sub(a1,a2);
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
	static public int Sub__R_Matrix44__R_Matrix44__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Matrix44 a3;
			ScriptRuntime.Matrix44.Sub(ref a1,ref a2,out a3);
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
	static public int Multiply__Matrix44__Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Matrix44.Multiply(a1,a2);
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
	static public int Multiply__R_Matrix44__R_Matrix44__O_Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Matrix44 a3;
			ScriptRuntime.Matrix44.Multiply(ref a1,ref a2,out a3);
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
	static public int TransformVector4__Matrix44__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Matrix44.TransformVector4(a1,a2);
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
	static public int TransformVector4__R_Matrix44__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Matrix44.TransformVector4(ref a1,ref a2,out a3);
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
	static public int TransformPosition__Matrix44__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Matrix44.TransformPosition(a1,a2);
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
	static public int TransformPosition__R_Matrix44__R_Vector3__O_Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			ScriptRuntime.Matrix44.TransformPosition(ref a1,ref a2,out a3);
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
	static public int TransformDirection__Matrix44__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Matrix44.TransformDirection(a1,a2);
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
	static public int TransformDirection__R_Matrix44__R_Vector3__O_Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			ScriptRuntime.Matrix44.TransformDirection(ref a1,ref a2,out a3);
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
	static public int op_UnaryNegation_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			var ret=-a1;
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
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
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
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
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
	static public int op_Addition_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			var ret=a1+a2;
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
	static public int op_Subtraction_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			var ret=a1-a2;
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
	static public int op_Multiply_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			var ret=a1*a2;
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
	static public int get_M00(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M00);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M00(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M00=v;
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
	static public int get_M01(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M01);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M01(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M01=v;
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
	static public int get_M02(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M02);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M02(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M02=v;
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
	static public int get_M03(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M03);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M03(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M03=v;
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
	static public int get_M10(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M10);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M10(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M10=v;
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
	static public int get_M11(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M11);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M11(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M11=v;
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
	static public int get_M12(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M12);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M12(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M12=v;
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
	static public int get_M13(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M13);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M13(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M13=v;
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
	static public int get_M20(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M20);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M20(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M20=v;
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
	static public int get_M21(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M21);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M21(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M21=v;
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
	static public int get_M22(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M22);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M22(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M22=v;
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
	static public int get_M23(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M23);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M23(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M23=v;
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
	static public int get_M30(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M30);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M30(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M30=v;
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
	static public int get_M31(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M31);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M31(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M31=v;
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
	static public int get_M32(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M32);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M32(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M32=v;
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
	static public int get_M33(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M33);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_M33(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M33=v;
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
	static public int get_Identity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Matrix44.Identity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Up(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Up);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Up(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Up=v;
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
	static public int get_Down(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Down);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Down(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Down=v;
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
	static public int get_Right(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
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
	static public int set_Right(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Right=v;
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
	static public int get_Left(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
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
	static public int set_Left(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Left=v;
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
	static public int get_Forward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Forward);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Forward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Forward=v;
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
	static public int get_Backward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Backward);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Backward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Backward=v;
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
	static public int getItem(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			int v;
			checkType(l,2,out v);
			var ret = self[v];
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
	static public int setItem(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			int v;
			checkType(l,2,out v);
			float c;
			checkType(l,3,out c);
			self[v]=c;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Matrix44");
		addMember(l,ctor_s);
		addMember(l,ctor__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single__Single_s);
		addMember(l,GetRow);
		addMember(l,SetRow);
		addMember(l,GetColumn);
		addMember(l,SetColumn);
		addMember(l,Decompose);
		addMember(l,ToString);
		addMember(l,Equals__Matrix44);
		addMember(l,Equals__Object);
		addMember(l,Determinant);
		addMember(l,CreateTranslation__Vector3_s);
		addMember(l,CreateTranslation__R_Vector3__O_Matrix44_s);
		addMember(l,CreateScale__Vector3_s);
		addMember(l,CreateScale__R_Vector3__O_Matrix44_s);
		addMember(l,CreatePerspectiveFieldOfView__Single__Single__Single__Single_s);
		addMember(l,CreatePerspectiveFieldOfView__Single__Single__Single__Single__O_Matrix44_s);
		addMember(l,CreatePerspective__Single__Single__Single__Single_s);
		addMember(l,CreatePerspective__Single__Single__Single__Single__O_Matrix44_s);
		addMember(l,CreateOrthographic__Single__Single__Single__Single_s);
		addMember(l,CreateOrthographic__Single__Single__Single__Single__O_Matrix44_s);
		addMember(l,CreateLookAt__Vector3__Vector3__Vector3_s);
		addMember(l,CreateLookAt__R_Vector3__R_Vector3__R_Vector3__O_Matrix44_s);
		addMember(l,CreateFromQuaternion__Quaternion_s);
		addMember(l,CreateFromQuaternion__R_Quaternion__O_Matrix44_s);
		addMember(l,CreateFromYawPitchRoll__Single__Single__Single_s);
		addMember(l,CreateFromYawPitchRoll__Single__Single__Single__O_Matrix44_s);
		addMember(l,CreateRotationX__Single_s);
		addMember(l,CreateRotationX__Single__O_Matrix44_s);
		addMember(l,CreateRotationY__Single_s);
		addMember(l,CreateRotationY__Single__O_Matrix44_s);
		addMember(l,CreateRotationZ__Single_s);
		addMember(l,CreateRotationZ__Single__O_Matrix44_s);
		addMember(l,CreateFromAxisAngle__Vector3__Single_s);
		addMember(l,CreateFromAxisAngle__R_Vector3__Single__O_Matrix44_s);
		addMember(l,Transpose__Matrix44_s);
		addMember(l,Transpose__R_Matrix44__O_Matrix44_s);
		addMember(l,Invert__Matrix44_s);
		addMember(l,Invert__R_Matrix44__O_Matrix44_s);
		addMember(l,Add__Matrix44__Matrix44_s);
		addMember(l,Add__R_Matrix44__R_Matrix44__O_Matrix44_s);
		addMember(l,Sub__Matrix44__Matrix44_s);
		addMember(l,Sub__R_Matrix44__R_Matrix44__O_Matrix44_s);
		addMember(l,Multiply__Matrix44__Matrix44_s);
		addMember(l,Multiply__R_Matrix44__R_Matrix44__O_Matrix44_s);
		addMember(l,TransformVector4__Matrix44__Vector4_s);
		addMember(l,TransformVector4__R_Matrix44__R_Vector4__O_Vector4_s);
		addMember(l,TransformPosition__Matrix44__Vector3_s);
		addMember(l,TransformPosition__R_Matrix44__R_Vector3__O_Vector3_s);
		addMember(l,TransformDirection__Matrix44__Vector3_s);
		addMember(l,TransformDirection__R_Matrix44__R_Vector3__O_Vector3_s);
		addMember(l,op_UnaryNegation_s);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,op_Addition_s);
		addMember(l,op_Subtraction_s);
		addMember(l,op_Multiply_s);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"M00",get_M00,set_M00,true);
		addMember(l,"M01",get_M01,set_M01,true);
		addMember(l,"M02",get_M02,set_M02,true);
		addMember(l,"M03",get_M03,set_M03,true);
		addMember(l,"M10",get_M10,set_M10,true);
		addMember(l,"M11",get_M11,set_M11,true);
		addMember(l,"M12",get_M12,set_M12,true);
		addMember(l,"M13",get_M13,set_M13,true);
		addMember(l,"M20",get_M20,set_M20,true);
		addMember(l,"M21",get_M21,set_M21,true);
		addMember(l,"M22",get_M22,set_M22,true);
		addMember(l,"M23",get_M23,set_M23,true);
		addMember(l,"M30",get_M30,set_M30,true);
		addMember(l,"M31",get_M31,set_M31,true);
		addMember(l,"M32",get_M32,set_M32,true);
		addMember(l,"M33",get_M33,set_M33,true);
		addMember(l,"Identity",get_Identity,null,false);
		addMember(l,"Up",get_Up,set_Up,true);
		addMember(l,"Down",get_Down,set_Down,true);
		addMember(l,"Right",get_Right,set_Right,true);
		addMember(l,"Left",get_Left,set_Left,true);
		addMember(l,"Forward",get_Forward,set_Forward,true);
		addMember(l,"Backward",get_Backward,set_Backward,true);
		createTypeMetatable(l,null, typeof(ScriptRuntime.Matrix44),typeof(System.ValueType));
	}
}
