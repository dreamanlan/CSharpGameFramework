using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Dsl_FunctionData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Dsl.FunctionData o;
			o=new Dsl.FunctionData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsValid(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
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
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
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
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
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
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
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
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
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
	static public int SetExtentClass(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetExtentClass(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetExtentClass(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.GetExtentClass();
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
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
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
	static public int HaveParam(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveParam();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HaveStatement(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveStatement();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HaveExternScript(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveExternScript();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetExternScript(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetExternScript(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetExternScript(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.GetExternScript();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStatementNum(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.GetStatementNum();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetStatement(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			Dsl.ISyntaxComponent a2;
			checkType(l,3,out a2);
			self.SetStatement(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStatement(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetStatement(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStatementId(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetStatementId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearStatements(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			self.ClearStatements();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddStatement(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Dsl.FunctionData))){
				Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
				Dsl.FunctionData a1;
				checkType(l,2,out a1);
				self.AddStatement(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.StatementData))){
				Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
				Dsl.StatementData a1;
				checkType(l,2,out a1);
				self.AddStatement(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.ISyntaxComponent))){
				Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
				Dsl.ISyntaxComponent a1;
				checkType(l,2,out a1);
				self.AddStatement(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.CallData))){
				Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
				Dsl.CallData a1;
				checkType(l,2,out a1);
				self.AddStatement(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.AddStatement(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.ValueData))){
				Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
				Dsl.ValueData a1;
				checkType(l,2,out a1);
				self.AddStatement(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.AddStatement(a1,a2);
				pushValue(l,true);
				return 1;
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
	static public int Clear(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
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
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.FunctionData a1;
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
	static public int get_Call(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Call);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Call(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.CallData v;
			checkType(l,2,out v);
			self.Call=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Statements(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Statements);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Statements(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			List<Dsl.ISyntaxComponent> v;
			checkType(l,2,out v);
			self.Statements=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NullFunctionData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.FunctionData.NullFunctionData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.FunctionData");
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,ToScriptString);
		addMember(l,SetExtentClass);
		addMember(l,GetExtentClass);
		addMember(l,HaveId);
		addMember(l,HaveParam);
		addMember(l,HaveStatement);
		addMember(l,HaveExternScript);
		addMember(l,SetExternScript);
		addMember(l,GetExternScript);
		addMember(l,GetStatementNum);
		addMember(l,SetStatement);
		addMember(l,GetStatement);
		addMember(l,GetStatementId);
		addMember(l,ClearStatements);
		addMember(l,AddStatement);
		addMember(l,Clear);
		addMember(l,CopyFrom);
		addMember(l,"Call",get_Call,set_Call,true);
		addMember(l,"Statements",get_Statements,set_Statements,true);
		addMember(l,"NullFunctionData",get_NullFunctionData,null,false);
		createTypeMetatable(l,constructor, typeof(Dsl.FunctionData),typeof(Dsl.AbstractSyntaxComponent));
	}
}
