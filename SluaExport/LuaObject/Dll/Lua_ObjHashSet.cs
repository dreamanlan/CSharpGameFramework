using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ObjHashSet : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ObjHashSet o;
			o=new ObjHashSet();
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
	static public int ctor__Int32_s(IntPtr l) {
		try {
			ObjHashSet o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new ObjHashSet(a1);
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
	static public int Clear(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
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
	static public int Contains(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Contains(a1);
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
	static public int CopyTo__A_T(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Object[] a1;
			checkArray(l,2,out a1);
			self.CopyTo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CopyTo__A_T__Int32(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Object[] a1;
			checkArray(l,2,out a1);
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
	static public int CopyTo__A_T__Int32__Int32(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Object[] a1;
			checkArray(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.CopyTo(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Remove(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Remove(a1);
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
	static public int GetObjectData(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Runtime.Serialization.SerializationInfo a1;
			checkType(l,2,out a1);
			System.Runtime.Serialization.StreamingContext a2;
			checkValueType(l,3,out a2);
			self.GetObjectData(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnDeserialization(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.OnDeserialization(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Add(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Add(a1);
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
	static public int TrimExcess(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			self.TrimExcess();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static new public int Equals(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
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
	static new public int ToString(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			var ret=self.ToString();
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
	static public int get_Count(IntPtr l) {
		try {
			ObjHashSet self=(ObjHashSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ObjHashSet");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32_s);
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,CopyTo__A_T);
		addMember(l,CopyTo__A_T__Int32);
		addMember(l,CopyTo__A_T__Int32__Int32);
		addMember(l,Remove);
		addMember(l,GetObjectData);
		addMember(l,OnDeserialization);
		addMember(l,Add);
		addMember(l,TrimExcess);
		addMember(l,Equals);
		addMember(l,ToString);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,null, typeof(ObjHashSet),typeof(System.Collections.Generic.HashSet<System.Object>));
	}
}
