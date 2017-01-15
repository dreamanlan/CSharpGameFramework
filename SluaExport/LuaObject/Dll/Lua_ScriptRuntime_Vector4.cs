using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_Vector4 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			ScriptRuntime.Vector4 o;
			if(argc==5){
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				o=new ScriptRuntime.Vector4(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				o=new ScriptRuntime.Vector4(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				o=new ScriptRuntime.Vector4(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				System.Single a1;
				checkType(l,2,out a1);
				o=new ScriptRuntime.Vector4(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==0){
				o=new ScriptRuntime.Vector4();
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
	static public int Length(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			var ret=self.Length();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LengthSquared(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			var ret=self.LengthSquared();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Normalize(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			self.Normalize();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Distance_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Distance(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Vector4.Distance(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int DistanceSquared_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.DistanceSquared(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Vector4.DistanceSquared(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Dot_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Dot(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Vector4.Dot(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Normalize_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Vector4.Normalize(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				ScriptRuntime.Vector4.Normalize(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
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
	static public int Min_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Min(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Min(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Max_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Max(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Max(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Clamp_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				checkValueType(l,3,out a3);
				var ret=ScriptRuntime.Vector4.Clamp(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				checkValueType(l,3,out a3);
				ScriptRuntime.Vector4 a4;
				ScriptRuntime.Vector4.Clamp(ref a1,ref a2,ref a3,out a4);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				pushValue(l,a4);
				return 5;
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
	static public int Lerp_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				var ret=ScriptRuntime.Vector4.Lerp(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				ScriptRuntime.Vector4 a4;
				ScriptRuntime.Vector4.Lerp(ref a1,ref a2,a3,out a4);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a4);
				return 4;
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
	static public int SmoothStep_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				var ret=ScriptRuntime.Vector4.SmoothStep(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				ScriptRuntime.Vector4 a4;
				ScriptRuntime.Vector4.SmoothStep(ref a1,ref a2,a3,out a4);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a4);
				return 4;
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
	static public int Hermite_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				checkValueType(l,3,out a3);
				ScriptRuntime.Vector4 a4;
				checkValueType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				var ret=ScriptRuntime.Vector4.Hermite(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				checkValueType(l,3,out a3);
				ScriptRuntime.Vector4 a4;
				checkValueType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				ScriptRuntime.Vector4 a6;
				ScriptRuntime.Vector4.Hermite(ref a1,ref a2,ref a3,ref a4,a5,out a6);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				pushValue(l,a4);
				pushValue(l,a6);
				return 6;
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
	static public int Project_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Project(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Project(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Negate_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Vector4.Negate(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				ScriptRuntime.Vector4.Negate(ref a1,out a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
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
	static public int Add_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Add(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Add(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Sub_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Sub(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Sub(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Multiply_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(float))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Multiply(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(ScriptRuntime.Vector4))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Multiply(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(float),typeof(LuaOut))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Multiply(ref a1,a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a3);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(ScriptRuntime.Vector4),typeof(LuaOut))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Multiply(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int Divide_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(float))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Divide(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(ScriptRuntime.Vector4))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector4.Divide(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(float),typeof(LuaOut))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Divide(ref a1,a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a3);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(ScriptRuntime.Vector4),typeof(LuaOut))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Vector4.Divide(ref a1,ref a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
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
	static public int op_UnaryNegation(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			var ret=-a1;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
			if(matchType(l,argc,1,typeof(float),typeof(ScriptRuntime.Vector4))){
				System.Single a1;
				checkType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=a1*a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(float))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=a1*a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(ScriptRuntime.Vector4))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
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
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(float))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=a1/a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector4),typeof(ScriptRuntime.Vector4))){
				ScriptRuntime.Vector4 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
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
	static public int get_X(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.X);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_X(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.X=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Y(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Y);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Y(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Y=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Z(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Z);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Z(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Z=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_W(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.W);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_W(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.W=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Zero(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.Zero);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_One(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.One);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UnitX(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.UnitX);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UnitY(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.UnitY);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UnitZ(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.UnitZ);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UnitW(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.UnitW);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getItem(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			int v;
			checkType(l,2,out v);
			var ret = self[v];
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int setItem(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			int v;
			checkType(l,2,out v);
			float c;
			checkType(l,3,out c);
			self[v]=c;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Vector4");
		addMember(l,Length);
		addMember(l,LengthSquared);
		addMember(l,Normalize);
		addMember(l,Distance_s);
		addMember(l,DistanceSquared_s);
		addMember(l,Dot_s);
		addMember(l,Normalize_s);
		addMember(l,Min_s);
		addMember(l,Max_s);
		addMember(l,Clamp_s);
		addMember(l,Lerp_s);
		addMember(l,SmoothStep_s);
		addMember(l,Hermite_s);
		addMember(l,Project_s);
		addMember(l,Negate_s);
		addMember(l,Add_s);
		addMember(l,Sub_s);
		addMember(l,Multiply_s);
		addMember(l,Divide_s);
		addMember(l,op_UnaryNegation);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,op_Addition);
		addMember(l,op_Subtraction);
		addMember(l,op_Multiply);
		addMember(l,op_Division);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"X",get_X,set_X,true);
		addMember(l,"Y",get_Y,set_Y,true);
		addMember(l,"Z",get_Z,set_Z,true);
		addMember(l,"W",get_W,set_W,true);
		addMember(l,"Zero",get_Zero,null,false);
		addMember(l,"One",get_One,null,false);
		addMember(l,"UnitX",get_UnitX,null,false);
		addMember(l,"UnitY",get_UnitY,null,false);
		addMember(l,"UnitZ",get_UnitZ,null,false);
		addMember(l,"UnitW",get_UnitW,null,false);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.Vector4),typeof(System.ValueType));
	}
}
