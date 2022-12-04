using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_Common_DslAction : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			Dsl.Common.DslAction o;
			o=new Dsl.Common.DslAction();
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
	static public int buildOperator(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.buildOperator();
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
	static public int buildFirstTernaryOperator(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.buildFirstTernaryOperator();
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
	static public int buildSecondTernaryOperator(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.buildSecondTernaryOperator();
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
	static public int beginStatement(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.beginStatement();
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
	static public int endStatement(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.endStatement();
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
	static public int addFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.addFunction();
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
	static public int setFunctionId(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.setFunctionId();
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
	static public int setMemberId(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.setMemberId();
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
	static public int buildHighOrderFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.buildHighOrderFunction();
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
	static public int markParenthesisParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markParenthesisParam();
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
	static public int markBracketParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markBracketParam();
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
	static public int markPeriodParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markPeriodParam();
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
	static public int markPeriodParenthesisParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markPeriodParenthesisParam();
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
	static public int markPeriodBracketParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markPeriodBracketParam();
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
	static public int markPeriodBraceParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markPeriodBraceParam();
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
	static public int markQuestionPeriodParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markQuestionPeriodParam();
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
	static public int markQuestionParenthesisParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markQuestionParenthesisParam();
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
	static public int markQuestionBracketParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markQuestionBracketParam();
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
	static public int markQuestionBraceParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markQuestionBraceParam();
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
	static public int markStatement(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markStatement();
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
	static public int markExternScript(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markExternScript();
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
	static public int markBracketColonParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markBracketColonParam();
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
	static public int markParenthesisColonParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markParenthesisColonParam();
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
	static public int markAngleBracketColonParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markAngleBracketColonParam();
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
	static public int markBracePercentParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markBracePercentParam();
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
	static public int markBracketPercentParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markBracketPercentParam();
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
	static public int markParenthesisPercentParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markParenthesisPercentParam();
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
	static public int markAngleBracketPercentParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markAngleBracketPercentParam();
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
	static public int markColonColonParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markColonColonParam();
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
	static public int markColonColonParenthesisParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markColonColonParenthesisParam();
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
	static public int markColonColonBracketParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markColonColonBracketParam();
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
	static public int markColonColonBraceParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markColonColonBraceParam();
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
	static public int setExternScript(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.setExternScript();
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
	static public int markPointerParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markPointerParam();
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
	static public int markPeriodStarParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markPeriodStarParam();
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
	static public int markQuestionPeriodStarParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markQuestionPeriodStarParam();
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
	static public int markPointerStarParam(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			self.markPointerStarParam();
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
	static public int push(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.push(a1,a2);
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
	static public int getCurStatement(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.getCurStatement();
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
	static public int getCurParentStatement(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.getCurParentStatement();
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
	static public int getFirstFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.getFirstFunction();
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
	static public int getSecondFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.getSecondFunction();
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
	static public int getThirdFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.getThirdFunction();
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
	static public int getLastFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.getLastFunction();
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
	static public int newStatementWithOneFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.newStatementWithOneFunction();
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
	static public int newStatementWithoutFunction(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			var ret=self.newStatementWithoutFunction();
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
	static public int newFunctionOfStatement(IntPtr l) {
		try {
			Dsl.Common.DslAction self;
			checkValueType(l,1,out self);
			Dsl.StatementData a1;
			checkType(l,2,out a1);
			var ret=self.newFunctionOfStatement(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.Common.DslAction");
		addMember(l,ctor_s);
		addMember(l,buildOperator);
		addMember(l,buildFirstTernaryOperator);
		addMember(l,buildSecondTernaryOperator);
		addMember(l,beginStatement);
		addMember(l,endStatement);
		addMember(l,addFunction);
		addMember(l,setFunctionId);
		addMember(l,setMemberId);
		addMember(l,buildHighOrderFunction);
		addMember(l,markParenthesisParam);
		addMember(l,markBracketParam);
		addMember(l,markPeriodParam);
		addMember(l,markPeriodParenthesisParam);
		addMember(l,markPeriodBracketParam);
		addMember(l,markPeriodBraceParam);
		addMember(l,markQuestionPeriodParam);
		addMember(l,markQuestionParenthesisParam);
		addMember(l,markQuestionBracketParam);
		addMember(l,markQuestionBraceParam);
		addMember(l,markStatement);
		addMember(l,markExternScript);
		addMember(l,markBracketColonParam);
		addMember(l,markParenthesisColonParam);
		addMember(l,markAngleBracketColonParam);
		addMember(l,markBracePercentParam);
		addMember(l,markBracketPercentParam);
		addMember(l,markParenthesisPercentParam);
		addMember(l,markAngleBracketPercentParam);
		addMember(l,markColonColonParam);
		addMember(l,markColonColonParenthesisParam);
		addMember(l,markColonColonBracketParam);
		addMember(l,markColonColonBraceParam);
		addMember(l,setExternScript);
		addMember(l,markPointerParam);
		addMember(l,markPeriodStarParam);
		addMember(l,markQuestionPeriodStarParam);
		addMember(l,markPointerStarParam);
		addMember(l,push);
		addMember(l,getCurStatement);
		addMember(l,getCurParentStatement);
		addMember(l,getFirstFunction);
		addMember(l,getSecondFunction);
		addMember(l,getThirdFunction);
		addMember(l,getLastFunction);
		addMember(l,newStatementWithOneFunction);
		addMember(l,newStatementWithoutFunction);
		addMember(l,newFunctionOfStatement);
		createTypeMetatable(l,null, typeof(Dsl.Common.DslAction),typeof(System.ValueType));
	}
}
