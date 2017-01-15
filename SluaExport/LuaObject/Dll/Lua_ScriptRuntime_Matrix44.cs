using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_Matrix44 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 o;
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Single a7;
			checkType(l,8,out a7);
			System.Single a8;
			checkType(l,9,out a8);
			System.Single a9;
			checkType(l,10,out a9);
			System.Single a10;
			checkType(l,11,out a10);
			System.Single a11;
			checkType(l,12,out a11);
			System.Single a12;
			checkType(l,13,out a12);
			System.Single a13;
			checkType(l,14,out a13);
			System.Single a14;
			checkType(l,15,out a14);
			System.Single a15;
			checkType(l,16,out a15);
			System.Single a16;
			checkType(l,17,out a16);
			o=new ScriptRuntime.Matrix44(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11,a12,a13,a14,a15,a16);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRow(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetRow(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetRow(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,3,out a2);
			self.SetRow(a1,a2);
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetColumn(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetColumn(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetColumn(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,3,out a2);
			self.SetColumn(a1,a2);
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Decompose(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 a1;
			ScriptRuntime.Quaternion a2;
			ScriptRuntime.Vector3 a3;
			self.Decompose(out a1,out a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			setBack(l,self);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Determinant(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			var ret=self.Determinant();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateTranslation_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.CreateTranslation(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.CreateTranslation(ref a1,out a2);
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
	static public int CreateScale_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.CreateScale(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.CreateScale(ref a1,out a2);
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
	static public int CreatePerspectiveFieldOfView_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				var ret=ScriptRuntime.Matrix44.CreatePerspectiveFieldOfView(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				ScriptRuntime.Matrix44 a5;
				ScriptRuntime.Matrix44.CreatePerspectiveFieldOfView(a1,a2,a3,a4,out a5);
				pushValue(l,true);
				pushValue(l,a5);
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
	static public int CreatePerspective_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				var ret=ScriptRuntime.Matrix44.CreatePerspective(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				ScriptRuntime.Matrix44 a5;
				ScriptRuntime.Matrix44.CreatePerspective(a1,a2,a3,a4,out a5);
				pushValue(l,true);
				pushValue(l,a5);
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
	static public int CreateOrthographic_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				var ret=ScriptRuntime.Matrix44.CreateOrthographic(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				ScriptRuntime.Matrix44 a5;
				ScriptRuntime.Matrix44.CreateOrthographic(a1,a2,a3,a4,out a5);
				pushValue(l,true);
				pushValue(l,a5);
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
	static public int CreateLookAt_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,3,out a3);
				var ret=ScriptRuntime.Matrix44.CreateLookAt(a1,a2,a3);
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
				ScriptRuntime.Matrix44 a4;
				ScriptRuntime.Matrix44.CreateLookAt(ref a1,ref a2,ref a3,out a4);
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
	static public int CreateFromQuaternion_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.CreateFromQuaternion(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.CreateFromQuaternion(ref a1,out a2);
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
	static public int CreateFromYawPitchRoll_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				var ret=ScriptRuntime.Matrix44.CreateFromYawPitchRoll(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				ScriptRuntime.Matrix44 a4;
				ScriptRuntime.Matrix44.CreateFromYawPitchRoll(a1,a2,a3,out a4);
				pushValue(l,true);
				pushValue(l,a4);
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
	static public int CreateRotationX_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.CreateRotationX(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.Single a1;
				checkType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.CreateRotationX(a1,out a2);
				pushValue(l,true);
				pushValue(l,a2);
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
	static public int CreateRotationY_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.CreateRotationY(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.Single a1;
				checkType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.CreateRotationY(a1,out a2);
				pushValue(l,true);
				pushValue(l,a2);
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
	static public int CreateRotationZ_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.CreateRotationZ(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.Single a1;
				checkType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.CreateRotationZ(a1,out a2);
				pushValue(l,true);
				pushValue(l,a2);
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
	static public int CreateFromAxisAngle_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.Matrix44.CreateFromAxisAngle(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				ScriptRuntime.Matrix44 a3;
				ScriptRuntime.Matrix44.CreateFromAxisAngle(ref a1,a2,out a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a3);
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
	static public int Transpose_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.Transpose(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.Transpose(ref a1,out a2);
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
	static public int Invert_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Matrix44.Invert(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				ScriptRuntime.Matrix44.Invert(ref a1,out a2);
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
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Matrix44.Add(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Matrix44 a3;
				ScriptRuntime.Matrix44.Add(ref a1,ref a2,out a3);
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
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Matrix44.Sub(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Matrix44 a3;
				ScriptRuntime.Matrix44.Sub(ref a1,ref a2,out a3);
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
			if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Matrix44.Multiply(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Matrix44 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Matrix44 a3;
				ScriptRuntime.Matrix44.Multiply(ref a1,ref a2,out a3);
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
	static public int TransformVector4_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Matrix44.TransformVector4(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector4 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector4 a3;
				ScriptRuntime.Matrix44.TransformVector4(ref a1,ref a2,out a3);
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
	static public int TransformPosition_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Matrix44.TransformPosition(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Matrix44.TransformPosition(ref a1,ref a2,out a3);
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
	static public int TransformDirection_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Matrix44.TransformDirection(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Matrix44.TransformDirection(ref a1,ref a2,out a3);
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
			ScriptRuntime.Matrix44 a1;
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
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
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
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
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
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
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
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
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
			ScriptRuntime.Matrix44 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Matrix44 a2;
			checkValueType(l,2,out a2);
			var ret=a1*a2;
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M00(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M00);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M00(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M00=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M01(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M01);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M01(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M01=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M02(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M02);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M02(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M02=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M03(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M03);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M03(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M03=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M10(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M10);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M10(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M10=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M11(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M11);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M11(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M11=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M12(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M12);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M12(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M12=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M13(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M13);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M13(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M13=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M20(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M20);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M20(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M20=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M21(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M21);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M21(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M21=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M22(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M22);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M22(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M22=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M23(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M23);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M23(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M23=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M30(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M30);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M30(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M30=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M31(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M31);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M31(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M31=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M32(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M32);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M32(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M32=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_M33(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.M33);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_M33(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.M33=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Identity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Matrix44.Identity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Up(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Up);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Up(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Up=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Down(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Down);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Down(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Down=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Right(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Right);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Right(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Right=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Left(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Left);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Left(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Left=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Forward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Forward);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Forward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Forward=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Backward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Backward);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Backward(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.Backward=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getItem(IntPtr l) {
		try {
			ScriptRuntime.Matrix44 self;
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
			ScriptRuntime.Matrix44 self;
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
		getTypeTable(l,"ScriptRuntime.Matrix44");
		addMember(l,GetRow);
		addMember(l,SetRow);
		addMember(l,GetColumn);
		addMember(l,SetColumn);
		addMember(l,Decompose);
		addMember(l,Determinant);
		addMember(l,CreateTranslation_s);
		addMember(l,CreateScale_s);
		addMember(l,CreatePerspectiveFieldOfView_s);
		addMember(l,CreatePerspective_s);
		addMember(l,CreateOrthographic_s);
		addMember(l,CreateLookAt_s);
		addMember(l,CreateFromQuaternion_s);
		addMember(l,CreateFromYawPitchRoll_s);
		addMember(l,CreateRotationX_s);
		addMember(l,CreateRotationY_s);
		addMember(l,CreateRotationZ_s);
		addMember(l,CreateFromAxisAngle_s);
		addMember(l,Transpose_s);
		addMember(l,Invert_s);
		addMember(l,Add_s);
		addMember(l,Sub_s);
		addMember(l,Multiply_s);
		addMember(l,TransformVector4_s);
		addMember(l,TransformPosition_s);
		addMember(l,TransformDirection_s);
		addMember(l,op_UnaryNegation);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,op_Addition);
		addMember(l,op_Subtraction);
		addMember(l,op_Multiply);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"M00",get_M00,set_M00,true);
		addMember(l,"M01",get_M01,set_M01,true);
		addMember(l,"M02",get_M02,set_M02,true);
		addMember(l,"M03",get_M03,set_M03,true);
		addMember(l,"M10",get_M10,set_M10,true);
		addMember(l,"M11",get_M11,set_M11,true);
		addMember(l,"M12",get_M12,set_M12,true);
		addMember(l,"M13",get_M13,set_M13,true);
		addMember(l,"M20",get_M20,set_M20,true);
		addMember(l,"M21",get_M21,set_M21,true);
		addMember(l,"M22",get_M22,set_M22,true);
		addMember(l,"M23",get_M23,set_M23,true);
		addMember(l,"M30",get_M30,set_M30,true);
		addMember(l,"M31",get_M31,set_M31,true);
		addMember(l,"M32",get_M32,set_M32,true);
		addMember(l,"M33",get_M33,set_M33,true);
		addMember(l,"Identity",get_Identity,null,false);
		addMember(l,"Up",get_Up,set_Up,true);
		addMember(l,"Down",get_Down,set_Down,true);
		addMember(l,"Right",get_Right,set_Right,true);
		addMember(l,"Left",get_Left,set_Left,true);
		addMember(l,"Forward",get_Forward,set_Forward,true);
		addMember(l,"Backward",get_Backward,set_Backward,true);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.Matrix44),typeof(System.ValueType));
	}
}
