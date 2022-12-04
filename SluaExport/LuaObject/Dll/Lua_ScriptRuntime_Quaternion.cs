using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_Quaternion : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion o;
			o=new ScriptRuntime.Quaternion();
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
	static public int ctor__Single__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion o;
			System.Single a1;
			checkType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			o=new ScriptRuntime.Quaternion(a1,a2);
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
			ScriptRuntime.Quaternion o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			o=new ScriptRuntime.Quaternion(a1,a2,a3);
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
	static public int ctor__Single__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			o=new ScriptRuntime.Quaternion(a1,a2,a3);
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
			ScriptRuntime.Quaternion o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			o=new ScriptRuntime.Quaternion(a1,a2,a3,a4);
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
	static new public int ToString(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
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
	static public int Equals__Quaternion(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			ScriptRuntime.Quaternion a1;
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
			ScriptRuntime.Quaternion self;
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
	static public int LengthSquared(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			var ret=self.LengthSquared();
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
	static public int Length(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			var ret=self.Length();
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
	static public int Normalize(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			self.Normalize();
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
	static public int Conjugate(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			self.Conjugate();
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
	static public int Normalize__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Quaternion.Normalize(a1);
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
	static public int Normalize__R_Quaternion__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			ScriptRuntime.Quaternion.Normalize(ref a1,out a2);
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
	static public int Inverse__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Quaternion.Inverse(a1);
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
	static public int Inverse__R_Quaternion__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			ScriptRuntime.Quaternion.Inverse(ref a1,out a2);
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
	static public int CreateFromAxisAngle__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.Quaternion.CreateFromAxisAngle(a1,a2);
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
	static public int CreateFromAxisAngle__R_Vector3__Single__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			ScriptRuntime.Quaternion a3;
			ScriptRuntime.Quaternion.CreateFromAxisAngle(ref a1,a2,out a3);
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
	static public int CreateFromYawPitchRoll__Single__Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Quaternion.CreateFromYawPitchRoll(a1,a2,a3);
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
	static public int CreateFromYawPitchRoll__Single__Single__Single__O_Quaternion_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Quaternion a4;
			ScriptRuntime.Quaternion.CreateFromYawPitchRoll(a1,a2,a3,out a4);
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
	static public int CreateFromRotationMatrix__Matrix44_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Quaternion.CreateFromRotationMatrix(a1);
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
	static public int CreateFromRotationMatrix__R_Matrix44__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			ScriptRuntime.Quaternion.CreateFromRotationMatrix(ref a1,out a2);
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
	static public int Dot__Quaternion__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Quaternion.Dot(a1,a2);
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
	static public int Dot__R_Quaternion__R_Quaternion__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Quaternion.Dot(ref a1,ref a2,out a3);
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
	static public int Slerp__Quaternion__Quaternion__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Quaternion.Slerp(a1,a2,a3);
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
	static public int Slerp__R_Quaternion__R_Quaternion__Single__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Quaternion a4;
			ScriptRuntime.Quaternion.Slerp(ref a1,ref a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a4);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Lerp__Quaternion__Quaternion__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Quaternion.Lerp(a1,a2,a3);
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
	static public int Lerp__R_Quaternion__R_Quaternion__Single__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Quaternion a4;
			ScriptRuntime.Quaternion.Lerp(ref a1,ref a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a4);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Conjugate__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Quaternion.Conjugate(a1);
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
	static public int Conjugate__R_Quaternion__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			ScriptRuntime.Quaternion.Conjugate(ref a1,out a2);
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
	static public int Negate__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Quaternion.Negate(a1);
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
	static public int Negate__R_Quaternion__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			ScriptRuntime.Quaternion.Negate(ref a1,out a2);
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
	static public int Sub__Quaternion__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Quaternion.Sub(a1,a2);
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
	static public int Sub__R_Quaternion__R_Quaternion__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Quaternion a3;
			ScriptRuntime.Quaternion.Sub(ref a1,ref a2,out a3);
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
	static public int Rotate__Quaternion__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Quaternion.Rotate(a1,a2);
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
	static public int Rotate__R_Quaternion__R_Vector3__O_Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			ScriptRuntime.Quaternion.Rotate(ref a1,ref a2,out a3);
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
	static public int Multiply__Quaternion__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Quaternion.Multiply(a1,a2);
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
	static public int Multiply__R_Quaternion__R_Quaternion__O_Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Quaternion a3;
			ScriptRuntime.Quaternion.Multiply(ref a1,ref a2,out a3);
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
			ScriptRuntime.Quaternion a1;
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
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
	static public int op_Subtraction_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
	static public int get_W(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.W);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_W(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.W=v;
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
	static public int get_X(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.X);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_X(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.X=v;
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
	static public int get_Y(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Y);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Y(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Y=v;
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
	static public int get_Z(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Z);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Z(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Z=v;
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
			pushValue(l,ScriptRuntime.Quaternion.Identity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Quaternion");
		addMember(l,ctor_s);
		addMember(l,ctor__Single__Vector3_s);
		addMember(l,ctor__Vector3__Vector3__Vector3_s);
		addMember(l,ctor__Single__Single__Single_s);
		addMember(l,ctor__Single__Single__Single__Single_s);
		addMember(l,ToString);
		addMember(l,Equals__Quaternion);
		addMember(l,Equals__Object);
		addMember(l,LengthSquared);
		addMember(l,Length);
		addMember(l,Normalize);
		addMember(l,Conjugate);
		addMember(l,Normalize__Quaternion_s);
		addMember(l,Normalize__R_Quaternion__O_Quaternion_s);
		addMember(l,Inverse__Quaternion_s);
		addMember(l,Inverse__R_Quaternion__O_Quaternion_s);
		addMember(l,CreateFromAxisAngle__Vector3__Single_s);
		addMember(l,CreateFromAxisAngle__R_Vector3__Single__O_Quaternion_s);
		addMember(l,CreateFromYawPitchRoll__Single__Single__Single_s);
		addMember(l,CreateFromYawPitchRoll__Single__Single__Single__O_Quaternion_s);
		addMember(l,CreateFromRotationMatrix__Matrix44_s);
		addMember(l,CreateFromRotationMatrix__R_Matrix44__O_Quaternion_s);
		addMember(l,Dot__Quaternion__Quaternion_s);
		addMember(l,Dot__R_Quaternion__R_Quaternion__O_Single_s);
		addMember(l,Slerp__Quaternion__Quaternion__Single_s);
		addMember(l,Slerp__R_Quaternion__R_Quaternion__Single__O_Quaternion_s);
		addMember(l,Lerp__Quaternion__Quaternion__Single_s);
		addMember(l,Lerp__R_Quaternion__R_Quaternion__Single__O_Quaternion_s);
		addMember(l,Conjugate__Quaternion_s);
		addMember(l,Conjugate__R_Quaternion__O_Quaternion_s);
		addMember(l,Negate__Quaternion_s);
		addMember(l,Negate__R_Quaternion__O_Quaternion_s);
		addMember(l,Sub__Quaternion__Quaternion_s);
		addMember(l,Sub__R_Quaternion__R_Quaternion__O_Quaternion_s);
		addMember(l,Rotate__Quaternion__Vector3_s);
		addMember(l,Rotate__R_Quaternion__R_Vector3__O_Vector3_s);
		addMember(l,Multiply__Quaternion__Quaternion_s);
		addMember(l,Multiply__R_Quaternion__R_Quaternion__O_Quaternion_s);
		addMember(l,op_UnaryNegation_s);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,op_Subtraction_s);
		addMember(l,op_Multiply_s);
		addMember(l,"W",get_W,set_W,true);
		addMember(l,"X",get_X,set_X,true);
		addMember(l,"Y",get_Y,set_Y,true);
		addMember(l,"Z",get_Z,set_Z,true);
		addMember(l,"Identity",get_Identity,null,false);
		createTypeMetatable(l,null, typeof(ScriptRuntime.Quaternion),typeof(System.ValueType));
	}
}
