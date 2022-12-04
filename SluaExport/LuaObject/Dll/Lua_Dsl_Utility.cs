using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_Utility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			Dsl.Utility o;
			o=new Dsl.Utility();
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
	static public int CheckCppParseTable_s(IntPtr l) {
		try {
			var ret=Dsl.Utility.CheckCppParseTable();
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
	static public int writeSyntaxComponent_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			Dsl.ISyntaxComponent a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Boolean a4;
			checkType(l,4,out a4);
			System.Boolean a5;
			checkType(l,5,out a5);
			Dsl.DelimiterInfo a6;
			checkType(l,6,out a6);
			Dsl.Utility.writeSyntaxComponent(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int writeValueData_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			Dsl.ValueData a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Boolean a4;
			checkType(l,4,out a4);
			System.Boolean a5;
			checkType(l,5,out a5);
			Dsl.Utility.writeValueData(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int writeFunctionData_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			Dsl.FunctionData a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Boolean a4;
			checkType(l,4,out a4);
			System.Boolean a5;
			checkType(l,5,out a5);
			Dsl.DelimiterInfo a6;
			checkType(l,6,out a6);
			Dsl.Utility.writeFunctionData(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int writeStatementData_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			Dsl.StatementData a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Boolean a4;
			checkType(l,4,out a4);
			System.Boolean a5;
			checkType(l,5,out a5);
			Dsl.DelimiterInfo a6;
			checkType(l,6,out a6);
			Dsl.Utility.writeStatementData(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.Utility");
		addMember(l,ctor_s);
		addMember(l,CheckCppParseTable_s);
		addMember(l,writeSyntaxComponent_s);
		addMember(l,writeValueData_s);
		addMember(l,writeFunctionData_s);
		addMember(l,writeStatementData_s);
		createTypeMetatable(l,null, typeof(Dsl.Utility));
	}
}
