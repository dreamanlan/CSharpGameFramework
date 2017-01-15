using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_Color32 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ScriptRuntime.Color32 o;
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			o=new ScriptRuntime.Color32(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToColorF(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
			checkValueType(l,1,out self);
			var ret=self.ToColorF();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Lerp_s(IntPtr l) {
		try {
			ScriptRuntime.Color32 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Color32 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Color32.Lerp(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Multiply_s(IntPtr l) {
		try {
			ScriptRuntime.Color32 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.Color32.Multiply(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Multiply(IntPtr l) {
		try {
			ScriptRuntime.Color32 a1;
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
	static public int op_Equality(IntPtr l) {
		try {
			ScriptRuntime.Color32 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Color32 a2;
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
			ScriptRuntime.Color32 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Color32 a2;
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
	static public int get_R(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
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
	static public int set_R(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
			checkValueType(l,1,out self);
			System.Byte v;
			checkType(l,2,out v);
			self.R=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_G(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
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
	static public int set_G(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
			checkValueType(l,1,out self);
			System.Byte v;
			checkType(l,2,out v);
			self.G=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_B(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
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
	static public int set_B(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
			checkValueType(l,1,out self);
			System.Byte v;
			checkType(l,2,out v);
			self.B=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_A(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
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
	static public int set_A(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
			checkValueType(l,1,out self);
			System.Byte v;
			checkType(l,2,out v);
			self.A=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PackedValue(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.PackedValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PackedValue(IntPtr l) {
		try {
			ScriptRuntime.Color32 self;
			checkValueType(l,1,out self);
			System.UInt32 v;
			checkType(l,2,out v);
			self.PackedValue=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Black(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Color32.Black);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Blue(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Color32.Blue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Green(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Color32.Green);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Red(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Color32.Red);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_White(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Color32.White);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Color32");
		addMember(l,ToColorF);
		addMember(l,Lerp_s);
		addMember(l,Multiply_s);
		addMember(l,op_Multiply);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,"R",get_R,set_R,true);
		addMember(l,"G",get_G,set_G,true);
		addMember(l,"B",get_B,set_B,true);
		addMember(l,"A",get_A,set_A,true);
		addMember(l,"PackedValue",get_PackedValue,set_PackedValue,true);
		addMember(l,"Black",get_Black,null,false);
		addMember(l,"Blue",get_Blue,null,false);
		addMember(l,"Green",get_Green,null,false);
		addMember(l,"Red",get_Red,null,false);
		addMember(l,"White",get_White,null,false);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.Color32),typeof(System.ValueType));
	}
}
