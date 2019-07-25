using System;

using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_Vector3 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			ScriptRuntime.Vector3 o;
			if(argc==4){
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				o=new ScriptRuntime.Vector3(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				System.Single a1;
				checkType(l,2,out a1);
				o=new ScriptRuntime.Vector3(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				o=new ScriptRuntime.Vector3(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc<=1){
				o=new ScriptRuntime.Vector3();
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
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Distance(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Vector3.Distance(ref a1,ref a2,out a3);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.DistanceSquared(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Vector3.DistanceSquared(ref a1,ref a2,out a3);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Dot(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Vector3.Dot(ref a1,ref a2,out a3);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Vector3.Normalize(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				ScriptRuntime.Vector3.Normalize(ref a1,out a2);
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
	static public int Cross_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Cross(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Cross(ref a1,ref a2,out a3);
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
	static public int Reflect_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Reflect(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Reflect(ref a1,ref a2,out a3);
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
	static public int Min_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Min(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Min(ref a1,ref a2,out a3);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Max(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Max(ref a1,ref a2,out a3);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,3,out a3);
				var ret=ScriptRuntime.Vector3.Clamp(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,3,out a3);
				ScriptRuntime.Vector3 a4;
				ScriptRuntime.Vector3.Clamp(ref a1,ref a2,ref a3,out a4);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				var ret=ScriptRuntime.Vector3.Lerp(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				ScriptRuntime.Vector3 a4;
				ScriptRuntime.Vector3.Lerp(ref a1,ref a2,a3,out a4);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				var ret=ScriptRuntime.Vector3.SmoothStep(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				ScriptRuntime.Vector3 a4;
				ScriptRuntime.Vector3.SmoothStep(ref a1,ref a2,a3,out a4);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,3,out a3);
				ScriptRuntime.Vector3 a4;
				checkValueType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				var ret=ScriptRuntime.Vector3.Hermite(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,3,out a3);
				ScriptRuntime.Vector3 a4;
				checkValueType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				ScriptRuntime.Vector3 a6;
				ScriptRuntime.Vector3.Hermite(ref a1,ref a2,ref a3,ref a4,a5,out a6);
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
	static public int Negate_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Vector3.Negate(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				ScriptRuntime.Vector3.Negate(ref a1,out a2);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Add(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Add(ref a1,ref a2,out a3);
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
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Sub(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Sub(ref a1,ref a2,out a3);
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
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(float))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Multiply(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Multiply(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(float),typeof(LuaOut))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Multiply(ref a1,a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a3);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(LuaOut))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Multiply(ref a1,ref a2,out a3);
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
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(float))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Divide(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Divide(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(float),typeof(LuaOut))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Divide(ref a1,a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a3);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(LuaOut))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Divide(ref a1,ref a2,out a3);
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
	static public int OrthoNormalize_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3.OrthoNormalize(ref a1,ref a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,3,out a3);
				ScriptRuntime.Vector3.OrthoNormalize(ref a1,ref a2,ref a3);
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
	static public int Project_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Project(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Vector3.Project(ref a1,ref a2,out a3);
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
	static public int Angle_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Vector3.Angle(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Vector3.Angle(ref a1,ref a2,out a3);
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
			ScriptRuntime.Vector3 a1;
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
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
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
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
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
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
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
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
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
			if(matchType(l,argc,1,typeof(float),typeof(ScriptRuntime.Vector3))){
				System.Single a1;
				checkType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=a1*a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(float))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=a1*a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
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
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(float))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=a1/a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
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
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
	static public int get_Zero(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector3.Zero);
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
			pushValue(l,ScriptRuntime.Vector3.One);
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
			pushValue(l,ScriptRuntime.Vector3.UnitX);
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
			pushValue(l,ScriptRuntime.Vector3.UnitY);
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
			pushValue(l,ScriptRuntime.Vector3.UnitZ);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Up(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector3.Up);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Down(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector3.Down);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Right(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector3.Right);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Left(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector3.Left);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Forward(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector3.Forward);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Backward(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector3.Backward);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getItem(IntPtr l) {
		try {
			ScriptRuntime.Vector3 self;
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
			ScriptRuntime.Vector3 self;
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
		getTypeTable(l,"ScriptRuntime.Vector3");
		addMember(l,Length);
		addMember(l,LengthSquared);
		addMember(l,Normalize);
		addMember(l,Distance_s);
		addMember(l,DistanceSquared_s);
		addMember(l,Dot_s);
		addMember(l,Normalize_s);
		addMember(l,Cross_s);
		addMember(l,Reflect_s);
		addMember(l,Min_s);
		addMember(l,Max_s);
		addMember(l,Clamp_s);
		addMember(l,Lerp_s);
		addMember(l,SmoothStep_s);
		addMember(l,Hermite_s);
		addMember(l,Negate_s);
		addMember(l,Add_s);
		addMember(l,Sub_s);
		addMember(l,Multiply_s);
		addMember(l,Divide_s);
		addMember(l,OrthoNormalize_s);
		addMember(l,Project_s);
		addMember(l,Angle_s);
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
		addMember(l,"Zero",get_Zero,null,false);
		addMember(l,"One",get_One,null,false);
		addMember(l,"UnitX",get_UnitX,null,false);
		addMember(l,"UnitY",get_UnitY,null,false);
		addMember(l,"UnitZ",get_UnitZ,null,false);
		addMember(l,"Up",get_Up,null,false);
		addMember(l,"Down",get_Down,null,false);
		addMember(l,"Right",get_Right,null,false);
		addMember(l,"Left",get_Left,null,false);
		addMember(l,"Forward",get_Forward,null,false);
		addMember(l,"Backward",get_Backward,null,false);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.Vector3),typeof(System.ValueType));
	}
}
