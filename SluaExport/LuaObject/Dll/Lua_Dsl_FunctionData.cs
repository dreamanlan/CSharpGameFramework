using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_FunctionData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int CalcComment(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.CalcComment();
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
	static public int HaveLowerOrderParam(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveLowerOrderParam();
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
	static public int HaveLowerOrderStatement(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveLowerOrderStatement();
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
	static public int HaveLowerOrderExternScript(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveLowerOrderExternScript();
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
	static public int SetParamClass(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetParamClass(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetParamClass(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.GetParamClass();
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
	static public int GetParamClassUnmasked(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.GetParamClassUnmasked();
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
	static public int HaveParamClassInfixFlag(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveParamClassInfixFlag();
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
	static public int IsOperatorParamClass(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.IsOperatorParamClass();
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
	static public int IsTernaryOperatorParamClass(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.IsTernaryOperatorParamClass();
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
	static public int IsMemberParamClass(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.IsMemberParamClass();
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
	static public int HaveParamOrStatement(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.HaveParamOrStatement();
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int GetParamNum(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			var ret=self.GetParamNum();
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
	static public int SetParam(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			Dsl.ISyntaxComponent a2;
			checkType(l,3,out a2);
			self.SetParam(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetParam(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetParam(a1);
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
	static public int GetParamId(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetParamId(a1);
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
	static public int ClearParams(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			self.ClearParams();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddParam__String(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.AddParam(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddParam__ValueData(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.ValueData a1;
			checkType(l,2,out a1);
			self.AddParam(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddParam__FunctionData(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			self.AddParam(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddParam__StatementData(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.StatementData a1;
			checkType(l,2,out a1);
			self.AddParam(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddParam__ISyntaxComponent(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.AddParam(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddParam__String__Int32(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.AddParam(a1,a2);
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int get_IsHighOrder(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsHighOrder);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Name(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Name(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.ValueData v;
			checkType(l,2,out v);
			self.Name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LowerOrderFunction(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LowerOrderFunction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LowerOrderFunction(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			Dsl.FunctionData v;
			checkType(l,2,out v);
			self.LowerOrderFunction=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ThisOrLowerOrderCall(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ThisOrLowerOrderCall);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ThisOrLowerOrderBody(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ThisOrLowerOrderBody);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ThisOrLowerOrderScript(IntPtr l) {
		try {
			Dsl.FunctionData self=(Dsl.FunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ThisOrLowerOrderScript);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NullFunction(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.FunctionData.NullFunction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.FunctionData");
		addMember(l,ctor_s);
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,ToScriptString);
		addMember(l,HaveId);
		addMember(l,CalcComment);
		addMember(l,HaveLowerOrderParam);
		addMember(l,HaveLowerOrderStatement);
		addMember(l,HaveLowerOrderExternScript);
		addMember(l,SetParamClass);
		addMember(l,GetParamClass);
		addMember(l,GetParamClassUnmasked);
		addMember(l,HaveParamClassInfixFlag);
		addMember(l,IsOperatorParamClass);
		addMember(l,IsTernaryOperatorParamClass);
		addMember(l,IsMemberParamClass);
		addMember(l,HaveParamOrStatement);
		addMember(l,HaveParam);
		addMember(l,HaveStatement);
		addMember(l,HaveExternScript);
		addMember(l,GetParamNum);
		addMember(l,SetParam);
		addMember(l,GetParam);
		addMember(l,GetParamId);
		addMember(l,ClearParams);
		addMember(l,AddParam__String);
		addMember(l,AddParam__ValueData);
		addMember(l,AddParam__FunctionData);
		addMember(l,AddParam__StatementData);
		addMember(l,AddParam__ISyntaxComponent);
		addMember(l,AddParam__String__Int32);
		addMember(l,Clear);
		addMember(l,CopyFrom);
		addMember(l,"IsHighOrder",get_IsHighOrder,null,true);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"LowerOrderFunction",get_LowerOrderFunction,set_LowerOrderFunction,true);
		addMember(l,"ThisOrLowerOrderCall",get_ThisOrLowerOrderCall,null,true);
		addMember(l,"ThisOrLowerOrderBody",get_ThisOrLowerOrderBody,null,true);
		addMember(l,"ThisOrLowerOrderScript",get_ThisOrLowerOrderScript,null,true);
		addMember(l,"NullFunction",get_NullFunction,null,false);
		createTypeMetatable(l,null, typeof(Dsl.FunctionData),typeof(Dsl.ValueOrFunctionData));
	}
}
