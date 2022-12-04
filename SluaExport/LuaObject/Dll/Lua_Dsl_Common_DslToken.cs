using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_Common_DslToken : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			Dsl.Common.DslToken o;
			o=new Dsl.Common.DslToken();
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
	static public int setCurToken(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.String a1;
			checkType(l,2,out a1);
			self.setCurToken(a1);
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
	static public int setLastToken(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.String a1;
			checkType(l,2,out a1);
			self.setLastToken(a1);
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
	static public int enqueueToken(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.String a1;
			checkType(l,2,out a1);
			System.Int16 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.enqueueToken(a1,a2,a3);
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
	static public int getCurToken(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			var ret=self.getCurToken();
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
	static public int getLastToken(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			var ret=self.getLastToken();
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
	static public int getLineNumber(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			var ret=self.getLineNumber();
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
	static public int getLastLineNumber(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			var ret=self.getLastLineNumber();
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
	static public int getOperatorToken(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			self.getOperatorToken();
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
	static public int getOperatorTokenValue(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			var ret=self.getOperatorTokenValue();
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
	static public int isNotIdentifierAndEndParenthesis(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isNotIdentifierAndEndParenthesis(a1);
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
	static public int isWhiteSpace(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isWhiteSpace(a1);
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
	static public int isDelimiter(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isDelimiter(a1);
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
	static public int isBeginParentheses(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isBeginParentheses(a1);
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
	static public int isEndParentheses(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isEndParentheses(a1);
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
	static public int isOperator(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isOperator(a1);
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
	static public int isQuote(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isQuote(a1);
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
	static public int isSpecialChar(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			var ret=self.isSpecialChar(a1);
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
	static public int PeekChar(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.PeekChar(a1);
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
	static public int PeekNextValidChar__Int32(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.PeekNextValidChar(a1);
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
	static public int PeekNextValidChar__Int32__O_Int32(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			var ret=self.PeekNextValidChar(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int myisdigit__Char__Boolean_s(IntPtr l) {
		try {
			System.Char a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			var ret=Dsl.Common.DslToken.myisdigit(a1,a2);
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
	static public int myisdigit__Char__Boolean__Boolean__Boolean_s(IntPtr l) {
		try {
			System.Char a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			System.Boolean a3;
			checkType(l,3,out a3);
			System.Boolean a4;
			checkType(l,4,out a4);
			var ret=Dsl.Common.DslToken.myisdigit(a1,a2,a3,a4);
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
	static public int mychar2int_s(IntPtr l) {
		try {
			System.Char a1;
			checkType(l,1,out a1);
			var ret=Dsl.Common.DslToken.mychar2int(a1);
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
	static public int get_CurChar(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.CurChar);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NextChar(IntPtr l) {
		try {
			Dsl.Common.DslToken self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.NextChar);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.Common.DslToken");
		addMember(l,ctor_s);
		addMember(l,setCurToken);
		addMember(l,setLastToken);
		addMember(l,enqueueToken);
		addMember(l,getCurToken);
		addMember(l,getLastToken);
		addMember(l,getLineNumber);
		addMember(l,getLastLineNumber);
		addMember(l,getOperatorToken);
		addMember(l,getOperatorTokenValue);
		addMember(l,isNotIdentifierAndEndParenthesis);
		addMember(l,isWhiteSpace);
		addMember(l,isDelimiter);
		addMember(l,isBeginParentheses);
		addMember(l,isEndParentheses);
		addMember(l,isOperator);
		addMember(l,isQuote);
		addMember(l,isSpecialChar);
		addMember(l,PeekChar);
		addMember(l,PeekNextValidChar__Int32);
		addMember(l,PeekNextValidChar__Int32__O_Int32);
		addMember(l,myisdigit__Char__Boolean_s);
		addMember(l,myisdigit__Char__Boolean__Boolean__Boolean_s);
		addMember(l,mychar2int_s);
		addMember(l,"CurChar",get_CurChar,null,true);
		addMember(l,"NextChar",get_NextChar,null,true);
		createTypeMetatable(l,null, typeof(Dsl.Common.DslToken),typeof(System.ValueType));
	}
}
