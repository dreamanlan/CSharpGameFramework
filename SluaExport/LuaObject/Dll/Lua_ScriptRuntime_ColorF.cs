using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_ColorF : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.ColorF o;
			o=new ScriptRuntime.ColorF();
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
			ScriptRuntime.ColorF o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			o=new ScriptRuntime.ColorF(a1,a2,a3);
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
			ScriptRuntime.ColorF o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			o=new ScriptRuntime.ColorF(a1,a2,a3,a4);
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
			ScriptRuntime.ColorF self;
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
	static public int Equals__Object(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
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
	static public int Equals__ColorF(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			ScriptRuntime.ColorF a1;
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
	static public int ToColor32(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			var ret=self.ToColor32();
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
	static public int ToVector4(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			var ret=self.ToVector4();
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
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.ColorF a2;
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
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.ColorF a2;
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
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.ColorF a2;
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
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.ColorF a2;
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
	static public int op_Multiply__ColorF__ColorF_s(IntPtr l) {
		try {
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.ColorF a2;
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
	static public int op_Multiply__ColorF__Single_s(IntPtr l) {
		try {
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
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
	static public int op_Multiply__Single__ColorF_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			ScriptRuntime.ColorF a2;
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
	static public int op_Division__ColorF__ColorF_s(IntPtr l) {
		try {
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.ColorF a2;
			checkValueType(l,2,out a2);
			var ret=a1/a2;
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
	static public int op_Division__ColorF__Single_s(IntPtr l) {
		try {
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=a1/a2;
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
	static public int get_R(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.R);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_R(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.R=v;
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
	static public int get_G(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.G);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_G(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.G=v;
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
	static public int get_B(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.B);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_B(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.B=v;
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
	static public int get_A(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.A);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_A(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.A=v;
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
		getTypeTable(l,"ScriptRuntime.ColorF");
		addMember(l,ctor_s);
		addMember(l,ctor__Single__Single__Single_s);
		addMember(l,ctor__Single__Single__Single__Single_s);
		addMember(l,ToString);
		addMember(l,Equals__Object);
		addMember(l,Equals__ColorF);
		addMember(l,ToColor32);
		addMember(l,ToVector4);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,op_Addition_s);
		addMember(l,op_Subtraction_s);
		addMember(l,op_Multiply__ColorF__ColorF_s);
		addMember(l,op_Multiply__ColorF__Single_s);
		addMember(l,op_Multiply__Single__ColorF_s);
		addMember(l,op_Division__ColorF__ColorF_s);
		addMember(l,op_Division__ColorF__Single_s);
		addMember(l,"R",get_R,set_R,true);
		addMember(l,"G",get_G,set_G,true);
		addMember(l,"B",get_B,set_B,true);
		addMember(l,"A",get_A,set_A,true);
		createTypeMetatable(l,null, typeof(ScriptRuntime.ColorF),typeof(System.ValueType));
	}
}
