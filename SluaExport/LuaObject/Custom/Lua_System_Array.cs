﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_System_Array : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CopyTo__Array__Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Array a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.CopyTo(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CopyTo__Array__Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Array a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			self.CopyTo(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clone(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			var ret=self.Clone();
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
	static public int GetLongLength(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLongLength(a1);
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
	static public int GetValue__Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			var ret=self.GetValue(a1);
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
	static public int GetValue__A_Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int64[] a1;
			checkParams(l,2,out a1);
			var ret=self.GetValue(a1);
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
	static public int GetValue__A_Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32[] a1;
			checkParams(l,2,out a1);
			var ret=self.GetValue(a1);
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
	static public int GetValue__Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetValue(a1);
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
	static public int GetValue__Int64__Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			var ret=self.GetValue(a1,a2);
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
	static public int GetValue__Int32__Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.GetValue(a1,a2);
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
	static public int GetValue__Int64__Int64__Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			var ret=self.GetValue(a1,a2,a3);
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
	static public int GetValue__Int32__Int32__Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.GetValue(a1,a2,a3);
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
	static public int SetValue__Object__Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			self.SetValue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetValue__Object__A_Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int64[] a2;
			checkParams(l,3,out a2);
			self.SetValue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetValue__Object__A_Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int32[] a2;
			checkParams(l,3,out a2);
			self.SetValue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetValue__Object__Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.SetValue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetValue__Object__Int64__Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			self.SetValue(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetValue__Object__Int32__Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.SetValue(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetValue__Object__Int64__Int64__Int64(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			System.Int64 a4;
			checkType(l,5,out a4);
			self.SetValue(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetValue__Object__Int32__Int32__Int32(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			self.SetValue(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetEnumerator(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			var ret=self.GetEnumerator();
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
	static public int GetLength(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLength(a1);
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
	static public int GetLowerBound(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLowerBound(a1);
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
	static public int GetUpperBound(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetUpperBound(a1);
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
	static public int Initialize(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			self.Initialize();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateInstance__Type__A_Int64_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Int64[] a2;
			checkParams(l,2,out a2);
			var ret=System.Array.CreateInstance(a1,a2);
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
	static public int CreateInstance__Type__Int32_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=System.Array.CreateInstance(a1,a2);
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
	static public int CreateInstance__Type__A_Int32_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Int32[] a2;
			checkParams(l,2,out a2);
			var ret=System.Array.CreateInstance(a1,a2);
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
	static public int CreateInstance__Type__Int32__Int32_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=System.Array.CreateInstance(a1,a2,a3);
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
	static public int CreateInstance__Type__A_Int32__A_Int32_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Int32[] a2;
			checkArray(l,2,out a2);
			System.Int32[] a3;
			checkArray(l,3,out a3);
			var ret=System.Array.CreateInstance(a1,a2,a3);
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
	static public int CreateInstance__Type__Int32__Int32__Int32_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			var ret=System.Array.CreateInstance(a1,a2,a3,a4);
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
	static public int BinarySearch__Array__Object_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=System.Array.BinarySearch(a1,a2);
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
	static public int BinarySearch__Array__Object__IComparer_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.Collections.IComparer a3;
			checkType(l,3,out a3);
			var ret=System.Array.BinarySearch(a1,a2,a3);
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
	static public int BinarySearch__Array__Int32__Int32__Object_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Object a4;
			checkType(l,4,out a4);
			var ret=System.Array.BinarySearch(a1,a2,a3,a4);
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
	static public int BinarySearch__Array__Int32__Int32__Object__IComparer_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Object a4;
			checkType(l,4,out a4);
			System.Collections.IComparer a5;
			checkType(l,5,out a5);
			var ret=System.Array.BinarySearch(a1,a2,a3,a4,a5);
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
	static public int Copy__Array__Array__Int64_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array a2;
			checkType(l,2,out a2);
			System.Int64 a3;
			checkType(l,3,out a3);
			System.Array.Copy(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Copy__Array__Array__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Array.Copy(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Copy__Array__Int64__Array__Int64__Int64_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int64 a2;
			checkType(l,2,out a2);
			System.Array a3;
			checkType(l,3,out a3);
			System.Int64 a4;
			checkType(l,4,out a4);
			System.Int64 a5;
			checkType(l,5,out a5);
			System.Array.Copy(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Copy__Array__Int32__Array__Int32__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Array a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.Int32 a5;
			checkType(l,5,out a5);
			System.Array.Copy(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IndexOf__Array__Object_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=System.Array.IndexOf(a1,a2);
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
	static public int IndexOf__Array__Object__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=System.Array.IndexOf(a1,a2,a3);
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
	static public int IndexOf__Array__Object__Int32__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			var ret=System.Array.IndexOf(a1,a2,a3,a4);
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
	static public int LastIndexOf__Array__Object_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=System.Array.LastIndexOf(a1,a2);
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
	static public int LastIndexOf__Array__Object__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=System.Array.LastIndexOf(a1,a2,a3);
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
	static public int LastIndexOf__Array__Object__Int32__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			var ret=System.Array.LastIndexOf(a1,a2,a3,a4);
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
	static public int Reverse__Array_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array.Reverse(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reverse__Array__Int32__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Array.Reverse(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array.Sort(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array__IComparer_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Collections.IComparer a2;
			checkType(l,2,out a2);
			System.Array.Sort(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array__Array_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array a2;
			checkType(l,2,out a2);
			System.Array.Sort(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array__Int32__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Array.Sort(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array__Array__IComparer_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array a2;
			checkType(l,2,out a2);
			System.Collections.IComparer a3;
			checkType(l,3,out a3);
			System.Array.Sort(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array__Int32__Int32__IComparer_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Collections.IComparer a4;
			checkType(l,4,out a4);
			System.Array.Sort(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array__Array__Int32__Int32_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.Array.Sort(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort__Array__Array__Int32__Int32__IComparer_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Array a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.Collections.IComparer a5;
			checkType(l,5,out a5);
			System.Array.Sort(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clear_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Array.Clear(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ConstrainedCopy_s(IntPtr l) {
		try {
			System.Array a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Array a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.Int32 a5;
			checkType(l,5,out a5);
			System.Array.ConstrainedCopy(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LongLength(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LongLength);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsFixedSize(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFixedSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsReadOnly(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsReadOnly);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsSynchronized(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsSynchronized);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SyncRoot(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SyncRoot);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Length(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Length);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Rank(IntPtr l) {
		try {
			System.Array self=(System.Array)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Rank);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Array");
		addMember(l,CopyTo__Array__Int32);
		addMember(l,CopyTo__Array__Int64);
		addMember(l,Clone);
		addMember(l,GetLongLength);
		addMember(l,GetValue__Int64);
		addMember(l,GetValue__A_Int64);
		addMember(l,GetValue__A_Int32);
		addMember(l,GetValue__Int32);
		addMember(l,GetValue__Int64__Int64);
		addMember(l,GetValue__Int32__Int32);
		addMember(l,GetValue__Int64__Int64__Int64);
		addMember(l,GetValue__Int32__Int32__Int32);
		addMember(l,SetValue__Object__Int64);
		addMember(l,SetValue__Object__A_Int64);
		addMember(l,SetValue__Object__A_Int32);
		addMember(l,SetValue__Object__Int32);
		addMember(l,SetValue__Object__Int64__Int64);
		addMember(l,SetValue__Object__Int32__Int32);
		addMember(l,SetValue__Object__Int64__Int64__Int64);
		addMember(l,SetValue__Object__Int32__Int32__Int32);
		addMember(l,GetEnumerator);
		addMember(l,GetLength);
		addMember(l,GetLowerBound);
		addMember(l,GetUpperBound);
		addMember(l,Initialize);
		addMember(l,CreateInstance__Type__A_Int64_s);
		addMember(l,CreateInstance__Type__Int32_s);
		addMember(l,CreateInstance__Type__A_Int32_s);
		addMember(l,CreateInstance__Type__Int32__Int32_s);
		addMember(l,CreateInstance__Type__A_Int32__A_Int32_s);
		addMember(l,CreateInstance__Type__Int32__Int32__Int32_s);
		addMember(l,BinarySearch__Array__Object_s);
		addMember(l,BinarySearch__Array__Object__IComparer_s);
		addMember(l,BinarySearch__Array__Int32__Int32__Object_s);
		addMember(l,BinarySearch__Array__Int32__Int32__Object__IComparer_s);
		addMember(l,Copy__Array__Array__Int64_s);
		addMember(l,Copy__Array__Array__Int32_s);
		addMember(l,Copy__Array__Int64__Array__Int64__Int64_s);
		addMember(l,Copy__Array__Int32__Array__Int32__Int32_s);
		addMember(l,IndexOf__Array__Object_s);
		addMember(l,IndexOf__Array__Object__Int32_s);
		addMember(l,IndexOf__Array__Object__Int32__Int32_s);
		addMember(l,LastIndexOf__Array__Object_s);
		addMember(l,LastIndexOf__Array__Object__Int32_s);
		addMember(l,LastIndexOf__Array__Object__Int32__Int32_s);
		addMember(l,Reverse__Array_s);
		addMember(l,Reverse__Array__Int32__Int32_s);
		addMember(l,Sort__Array_s);
		addMember(l,Sort__Array__IComparer_s);
		addMember(l,Sort__Array__Array_s);
		addMember(l,Sort__Array__Int32__Int32_s);
		addMember(l,Sort__Array__Array__IComparer_s);
		addMember(l,Sort__Array__Int32__Int32__IComparer_s);
		addMember(l,Sort__Array__Array__Int32__Int32_s);
		addMember(l,Sort__Array__Array__Int32__Int32__IComparer_s);
		addMember(l,Clear_s);
		addMember(l,ConstrainedCopy_s);
		addMember(l,"LongLength",get_LongLength,null,true);
		addMember(l,"IsFixedSize",get_IsFixedSize,null,true);
		addMember(l,"IsReadOnly",get_IsReadOnly,null,true);
		addMember(l,"IsSynchronized",get_IsSynchronized,null,true);
		addMember(l,"SyncRoot",get_SyncRoot,null,true);
		addMember(l,"Length",get_Length,null,true);
		addMember(l,"Rank",get_Rank,null,true);
		createTypeMetatable(l,null, typeof(System.Array));
	}
}
