﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_AnimationCurve : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UnityEngine.AnimationCurve o;
			o=new UnityEngine.AnimationCurve();
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
	static public int ctor__A_Keyframe_s(IntPtr l) {
		try {
			UnityEngine.AnimationCurve o;
			UnityEngine.Keyframe[] a1;
			checkValueParams(l,1,out a1);
			o=new UnityEngine.AnimationCurve(a1);
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
	static public int Evaluate(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			var ret=self.Evaluate(a1);
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
	static public int AddKey__Keyframe(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.Keyframe a1;
			checkValueType(l,2,out a1);
			var ret=self.AddKey(a1);
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
	static public int AddKey__Single__Single(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			var ret=self.AddKey(a1,a2);
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
	static public int MoveKey(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			UnityEngine.Keyframe a2;
			checkValueType(l,3,out a2);
			var ret=self.MoveKey(a1,a2);
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
	static public int RemoveKey(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveKey(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SmoothTangents(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.SmoothTangents(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Equals__Object(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static public int Equals__AnimationCurve(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.AnimationCurve a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static public int Constant_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=UnityEngine.AnimationCurve.Constant(a1,a2,a3);
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
	static public int Linear_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=UnityEngine.AnimationCurve.Linear(a1,a2,a3,a4);
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
	static public int EaseInOut_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=UnityEngine.AnimationCurve.EaseInOut(a1,a2,a3,a4);
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
	static public int get_keys(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.keys);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_keys(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.Keyframe[] v;
			checkArray(l,2,out v);
			self.keys=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_length(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.length);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_preWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.preWrapMode);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_preWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.WrapMode v;
			checkEnum(l,2,out v);
			self.preWrapMode=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_postWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.postWrapMode);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_postWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.WrapMode v;
			checkEnum(l,2,out v);
			self.postWrapMode=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getItem(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.AnimationCurve");
		addMember(l,ctor_s);
		addMember(l,ctor__A_Keyframe_s);
		addMember(l,Evaluate);
		addMember(l,AddKey__Keyframe);
		addMember(l,AddKey__Single__Single);
		addMember(l,MoveKey);
		addMember(l,RemoveKey);
		addMember(l,SmoothTangents);
		addMember(l,Equals__Object);
		addMember(l,Equals__AnimationCurve);
		addMember(l,Constant_s);
		addMember(l,Linear_s);
		addMember(l,EaseInOut_s);
		addMember(l,getItem);
		addMember(l,"keys",get_keys,set_keys,true);
		addMember(l,"length",get_length,null,true);
		addMember(l,"preWrapMode",get_preWrapMode,set_preWrapMode,true);
		addMember(l,"postWrapMode",get_postWrapMode,set_postWrapMode,true);
		createTypeMetatable(l,null, typeof(UnityEngine.AnimationCurve));
	}
}
