using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_ColorF : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			ScriptRuntime.ColorF o;
			if(argc==5){
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				o=new ScriptRuntime.ColorF(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				o=new ScriptRuntime.ColorF(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==0){
				o=new ScriptRuntime.ColorF();
				pushValue(l,true);
				pushObject(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int op_Equality(IntPtr l) {
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
	static public int op_Inequality(IntPtr l) {
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
	static public int op_Addition(IntPtr l) {
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
	static public int op_Subtraction(IntPtr l) {
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
	static public int op_Multiply(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(float),typeof(ScriptRuntime.ColorF))){
				System.Single a1;
				checkType(l,1,out a1);
				ScriptRuntime.ColorF a2;
				checkValueType(l,2,out a2);
				var ret=a1*a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.ColorF),typeof(float))){
				ScriptRuntime.ColorF a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=a1*a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.ColorF),typeof(ScriptRuntime.ColorF))){
				ScriptRuntime.ColorF a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.ColorF a2;
				checkValueType(l,2,out a2);
				var ret=a1*a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int op_Division(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.ColorF),typeof(float))){
				ScriptRuntime.ColorF a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=a1/a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.ColorF),typeof(ScriptRuntime.ColorF))){
				ScriptRuntime.ColorF a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.ColorF a2;
				checkValueType(l,2,out a2);
				var ret=a1/a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int set_R(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
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
	static public int set_G(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
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
	static public int set_B(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
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
	static public int set_A(IntPtr l) {
		try {
			ScriptRuntime.ColorF self;
			checkValueType(l,1,out self);
			System.Single v;
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.ColorF");
		addMember(l,ToColor32);
		addMember(l,ToVector4);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,op_Addition);
		addMember(l,op_Subtraction);
		addMember(l,op_Multiply);
		addMember(l,op_Division);
		addMember(l,"R",get_R,set_R,true);
		addMember(l,"G",get_G,set_G,true);
		addMember(l,"B",get_B,set_B,true);
		addMember(l,"A",get_A,set_A,true);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.ColorF),typeof(System.ValueType));
	}
}
