using System;

using SLua;
using System.Collections.Generic;
public class Lua_Dsl_ValueData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			Dsl.ValueData o;
			if(argc==1){
				o=new Dsl.ValueData();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				System.String a1;
				checkType(l,2,out a1);
				o=new Dsl.ValueData(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				o=new Dsl.ValueData(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsValid(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.IsValid();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetId(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.GetId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetIdType(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.GetIdType();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLine(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.GetLine();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			var ret=self.ToScriptString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HaveId(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.HaveId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetId(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetId(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetType(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetType(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetLine(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetLine(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsId(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.IsId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsNumber(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.IsNumber();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsString(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.IsString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsBoolean(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			var ret=self.IsBoolean();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyFrom(IntPtr l) {
		try {
			Dsl.ValueData self=(Dsl.ValueData)checkSelf(l);
			Dsl.ValueData a1;
			checkType(l,2,out a1);
			self.CopyFrom(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.ValueData");
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,ToScriptString);
		addMember(l,HaveId);
		addMember(l,SetId);
		addMember(l,SetType);
		addMember(l,SetLine);
		addMember(l,IsId);
		addMember(l,IsNumber);
		addMember(l,IsString);
		addMember(l,IsBoolean);
		addMember(l,Clear);
		addMember(l,CopyFrom);
		createTypeMetatable(l,constructor, typeof(Dsl.ValueData),typeof(Dsl.AbstractSyntaxComponent));
	}
}
