using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Dsl_CallData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Dsl.CallData o;
			o=new Dsl.CallData();
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int CalcComment(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int SetParamClass(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int GetParamClass(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int HaveParam(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int GetParamNum(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int SetParam(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int GetParam(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int GetParamId(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int ClearParams(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
			self.ClearParams();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddParams(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Dsl.FunctionData))){
				Dsl.CallData self=(Dsl.CallData)checkSelf(l);
				Dsl.FunctionData a1;
				checkType(l,2,out a1);
				self.AddParams(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.StatementData))){
				Dsl.CallData self=(Dsl.CallData)checkSelf(l);
				Dsl.StatementData a1;
				checkType(l,2,out a1);
				self.AddParams(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.ISyntaxComponent))){
				Dsl.CallData self=(Dsl.CallData)checkSelf(l);
				Dsl.ISyntaxComponent a1;
				checkType(l,2,out a1);
				self.AddParams(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.CallData))){
				Dsl.CallData self=(Dsl.CallData)checkSelf(l);
				Dsl.CallData a1;
				checkType(l,2,out a1);
				self.AddParams(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				Dsl.CallData self=(Dsl.CallData)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.AddParams(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.ValueData))){
				Dsl.CallData self=(Dsl.CallData)checkSelf(l);
				Dsl.ValueData a1;
				checkType(l,2,out a1);
				self.AddParams(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				Dsl.CallData self=(Dsl.CallData)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.AddParams(a1,a2);
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
			Dsl.CallData a1;
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
	static public int get_Params(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Params);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Params(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
			List<Dsl.ISyntaxComponent> v;
			checkType(l,2,out v);
			self.Params=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsHighOrder(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsHighOrder);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Name(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Name(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int get_Call(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
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
	static public int get_Comments(IntPtr l) {
		try {
			Dsl.CallData self=(Dsl.CallData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Comments);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.CallData");
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,CalcComment);
		addMember(l,ToScriptString);
		addMember(l,HaveId);
		addMember(l,SetParamClass);
		addMember(l,GetParamClass);
		addMember(l,HaveParam);
		addMember(l,GetParamNum);
		addMember(l,SetParam);
		addMember(l,GetParam);
		addMember(l,GetParamId);
		addMember(l,ClearParams);
		addMember(l,AddParams);
		addMember(l,Clear);
		addMember(l,CopyFrom);
		addMember(l,"Params",get_Params,set_Params,true);
		addMember(l,"IsHighOrder",get_IsHighOrder,null,true);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"Call",get_Call,set_Call,true);
		addMember(l,"Comments",get_Comments,null,true);
		createTypeMetatable(l,constructor, typeof(Dsl.CallData),typeof(Dsl.AbstractSyntaxComponent));
	}
}
