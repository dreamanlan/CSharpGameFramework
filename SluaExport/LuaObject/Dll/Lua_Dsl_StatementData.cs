using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_StatementData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			Dsl.StatementData o;
			o=new Dsl.StatementData();
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
	static public int IsValid(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetId(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetIdType(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetLine(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int HaveId(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetFunctionNum(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			var ret=self.GetFunctionNum();
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
	static public int SetFunction(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			Dsl.ValueOrFunctionData a2;
			checkType(l,3,out a2);
			self.SetFunction(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetFunction(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetFunction(a1);
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
	static public int GetFunctionId(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetFunctionId(a1);
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
	static public int AddFunction(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			Dsl.ValueOrFunctionData a1;
			checkType(l,2,out a1);
			self.AddFunction(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clear(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CopyFrom(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			Dsl.StatementData a1;
			checkType(l,2,out a1);
			self.CopyFrom(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_First(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.First);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Second(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Second);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Third(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Third);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Last(IntPtr l) {
		try {
			Dsl.StatementData self=(Dsl.StatementData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Last);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NullStatementData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.StatementData.NullStatementData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.StatementData");
		addMember(l,ctor_s);
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,ToScriptString);
		addMember(l,HaveId);
		addMember(l,GetFunctionNum);
		addMember(l,SetFunction);
		addMember(l,GetFunction);
		addMember(l,GetFunctionId);
		addMember(l,AddFunction);
		addMember(l,Clear);
		addMember(l,CopyFrom);
		addMember(l,"First",get_First,null,true);
		addMember(l,"Second",get_Second,null,true);
		addMember(l,"Third",get_Third,null,true);
		addMember(l,"Last",get_Last,null,true);
		addMember(l,"NullStatementData",get_NullStatementData,null,false);
		createTypeMetatable(l,null, typeof(Dsl.StatementData),typeof(Dsl.AbstractSyntaxComponent));
	}
}
