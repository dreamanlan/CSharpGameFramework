using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_Quaternion : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			ScriptRuntime.Quaternion o;
			if(argc==5){
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				o=new ScriptRuntime.Quaternion(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				System.Single a1;
				checkType(l,2,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,3,out a2);
				o=new ScriptRuntime.Quaternion(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,3,out a2);
				ScriptRuntime.Vector3 a3;
				checkValueType(l,4,out a3);
				o=new ScriptRuntime.Quaternion(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(float),typeof(float),typeof(float))){
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				o=new ScriptRuntime.Quaternion(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc<=1){
				o=new ScriptRuntime.Quaternion();
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
	static public int LengthSquared(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
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
	static public int Length(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
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
	static public int Normalize(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
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
	static public int Conjugate(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
			checkValueType(l,1,out self);
			self.Conjugate();
			pushValue(l,true);
			setBack(l,self);
			return 1;
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
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Quaternion.Normalize(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				ScriptRuntime.Quaternion.Normalize(ref a1,out a2);
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
	static public int Inverse_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Quaternion.Inverse(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				ScriptRuntime.Quaternion.Inverse(ref a1,out a2);
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
	static public int CreateFromAxisAngle_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.Quaternion.CreateFromAxisAngle(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				ScriptRuntime.Quaternion a3;
				ScriptRuntime.Quaternion.CreateFromAxisAngle(ref a1,a2,out a3);
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
				var ret=ScriptRuntime.Quaternion.CreateFromYawPitchRoll(a1,a2,a3);
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
				ScriptRuntime.Quaternion a4;
				ScriptRuntime.Quaternion.CreateFromYawPitchRoll(a1,a2,a3,out a4);
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
	static public int CreateFromRotationMatrix_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Quaternion.CreateFromRotationMatrix(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Matrix44 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				ScriptRuntime.Quaternion.CreateFromRotationMatrix(ref a1,out a2);
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
	static public int Dot_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Quaternion.Dot(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				ScriptRuntime.Quaternion.Dot(ref a1,ref a2,out a3);
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
	static public int Slerp_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				var ret=ScriptRuntime.Quaternion.Slerp(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				ScriptRuntime.Quaternion a4;
				ScriptRuntime.Quaternion.Slerp(ref a1,ref a2,a3,out a4);
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
	static public int Lerp_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				var ret=ScriptRuntime.Quaternion.Lerp(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				ScriptRuntime.Quaternion a4;
				ScriptRuntime.Quaternion.Lerp(ref a1,ref a2,a3,out a4);
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
	static public int Conjugate_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Quaternion.Conjugate(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				ScriptRuntime.Quaternion.Conjugate(ref a1,out a2);
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
	static public int Negate_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				var ret=ScriptRuntime.Quaternion.Negate(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				ScriptRuntime.Quaternion.Negate(ref a1,out a2);
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
	static public int Sub_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Quaternion.Sub(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Quaternion a3;
				ScriptRuntime.Quaternion.Sub(ref a1,ref a2,out a3);
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
	static public int Rotate_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Quaternion.Rotate(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Vector3 a3;
				ScriptRuntime.Quaternion.Rotate(ref a1,ref a2,out a3);
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
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				var ret=ScriptRuntime.Quaternion.Multiply(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				ScriptRuntime.Quaternion a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Quaternion a2;
				checkValueType(l,2,out a2);
				ScriptRuntime.Quaternion a3;
				ScriptRuntime.Quaternion.Multiply(ref a1,ref a2,out a3);
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
			ScriptRuntime.Quaternion a1;
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
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
	static public int op_Subtraction(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion a2;
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
	static public int get_W(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
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
			ScriptRuntime.Quaternion self;
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
	static public int get_X(IntPtr l) {
		try {
			ScriptRuntime.Quaternion self;
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
			ScriptRuntime.Quaternion self;
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
			ScriptRuntime.Quaternion self;
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
			ScriptRuntime.Quaternion self;
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
			ScriptRuntime.Quaternion self;
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
			ScriptRuntime.Quaternion self;
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
	static public int get_Identity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Quaternion.Identity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Quaternion");
		addMember(l,LengthSquared);
		addMember(l,Length);
		addMember(l,Normalize);
		addMember(l,Conjugate);
		addMember(l,Normalize_s);
		addMember(l,Inverse_s);
		addMember(l,CreateFromAxisAngle_s);
		addMember(l,CreateFromYawPitchRoll_s);
		addMember(l,CreateFromRotationMatrix_s);
		addMember(l,Dot_s);
		addMember(l,Slerp_s);
		addMember(l,Lerp_s);
		addMember(l,Conjugate_s);
		addMember(l,Negate_s);
		addMember(l,Sub_s);
		addMember(l,Rotate_s);
		addMember(l,Multiply_s);
		addMember(l,op_UnaryNegation);
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,op_Subtraction);
		addMember(l,op_Multiply);
		addMember(l,"W",get_W,set_W,true);
		addMember(l,"X",get_X,set_X,true);
		addMember(l,"Y",get_Y,set_Y,true);
		addMember(l,"Z",get_Z,set_Z,true);
		addMember(l,"Identity",get_Identity,null,false);
		createTypeMetatable(l,constructor, typeof(ScriptRuntime.Quaternion),typeof(System.ValueType));
	}
}
