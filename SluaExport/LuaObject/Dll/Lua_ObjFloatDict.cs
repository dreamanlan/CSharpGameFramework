using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ObjFloatDict : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ObjFloatDict o;
			o=new ObjFloatDict();
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
			ObjFloatDict o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new ObjFloatDict(a1);
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
	static public int Add(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.Add(a1,a2);
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
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
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
	static public int ContainsKey(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.ContainsKey(a1);
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
	static public int ContainsValue(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			var ret=self.ContainsValue(a1);
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
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
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
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
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
	static public int Remove(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
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
	static public int TryGetValue(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Single a2;
			var ret=self.TryGetValue(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int EnsureCapacity(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.EnsureCapacity(a1);
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
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
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
	static public int TrimExcess__Int32(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.TrimExcess(a1);
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
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
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
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
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
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getItem(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Object v;
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
	[UnityEngine.Scripting.Preserve]
	static public int setItem(IntPtr l) {
		try {
			ObjFloatDict self=(ObjFloatDict)checkSelf(l);
			System.Object v;
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ObjFloatDict");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32_s);
		addMember(l,Add);
		addMember(l,Clear);
		addMember(l,ContainsKey);
		addMember(l,ContainsValue);
		addMember(l,GetObjectData);
		addMember(l,OnDeserialization);
		addMember(l,Remove);
		addMember(l,TryGetValue);
		addMember(l,EnsureCapacity);
		addMember(l,TrimExcess);
		addMember(l,TrimExcess__Int32);
		addMember(l,Equals);
		addMember(l,ToString);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,null, typeof(ObjFloatDict),typeof(System.Collections.Generic.Dictionary<System.Object,System.Single>));
	}
}
