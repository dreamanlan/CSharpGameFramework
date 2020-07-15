using System;

using SLua;
using System.Collections.Generic;
public class Lua_Dsl_Utility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int needQuote_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dsl.Utility.needQuote(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int quoteString_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=Dsl.Utility.quoteString(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int unquoteString_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dsl.Utility.unquoteString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getCallString_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			var ret=Dsl.Utility.getCallString(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int writeContent_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			Dsl.Utility.writeContent(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int writeLine_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			Dsl.Utility.writeLine(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int writeSyntaxComponent_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			Dsl.ISyntaxComponent a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			Dsl.Utility.writeSyntaxComponent(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
			Dsl.Utility.writeFunctionData(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int writeStatementData_s(IntPtr l) {
		try {
			System.Text.StringBuilder a1;
			checkType(l,1,out a1);
			Dsl.StatementData a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			Dsl.Utility.writeStatementData(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.Utility");
		addMember(l,needQuote_s);
		addMember(l,quoteString_s);
		addMember(l,unquoteString_s);
		addMember(l,getCallString_s);
		addMember(l,writeContent_s);
		addMember(l,writeLine_s);
		addMember(l,writeSyntaxComponent_s);
		addMember(l,writeFunctionData_s);
		addMember(l,writeStatementData_s);
		createTypeMetatable(l,constructor, typeof(Dsl.Utility));
	}
}
