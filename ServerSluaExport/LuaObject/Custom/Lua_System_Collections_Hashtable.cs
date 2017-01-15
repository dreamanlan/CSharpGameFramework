using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_System_Collections_Hashtable : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			System.Collections.Hashtable o;
			if(argc==1){
				o=new System.Collections.Hashtable();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(float))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				o=new System.Collections.Hashtable(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				System.Int32 a1;
				checkType(l,2,out a1);
				o=new System.Collections.Hashtable(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Collections.IDictionary),typeof(float))){
				System.Collections.IDictionary a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				o=new System.Collections.Hashtable(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Collections.IDictionary))){
				System.Collections.IDictionary a1;
				checkType(l,2,out a1);
				o=new System.Collections.Hashtable(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Collections.IDictionary),typeof(System.Collections.IEqualityComparer))){
				System.Collections.IDictionary a1;
				checkType(l,2,out a1);
				System.Collections.IEqualityComparer a2;
				checkType(l,3,out a2);
				o=new System.Collections.Hashtable(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Collections.IDictionary),typeof(float),typeof(System.Collections.IEqualityComparer))){
				System.Collections.IDictionary a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Collections.IEqualityComparer a3;
				checkType(l,4,out a3);
				o=new System.Collections.Hashtable(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Collections.IEqualityComparer))){
				System.Collections.IEqualityComparer a1;
				checkType(l,2,out a1);
				o=new System.Collections.Hashtable(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(System.Collections.IEqualityComparer))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Collections.IEqualityComparer a2;
				checkType(l,3,out a2);
				o=new System.Collections.Hashtable(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(float),typeof(System.Collections.IEqualityComparer))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Collections.IEqualityComparer a3;
				checkType(l,4,out a3);
				o=new System.Collections.Hashtable(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Add(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Object a2;
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
	static public int Clear(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Contains(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
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
	static public int Remove(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.Remove(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ContainsKey(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
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
	static public int ContainsValue(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			System.Object a1;
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
	static public int GetObjectData(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
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
	static public int OnDeserialization(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
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
	static public int Synchronized_s(IntPtr l) {
		try {
			System.Collections.Hashtable a1;
			checkType(l,1,out a1);
			var ret=System.Collections.Hashtable.Synchronized(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Count(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsSynchronized(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsSynchronized);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SyncRoot(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SyncRoot);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsFixedSize(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFixedSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsReadOnly(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsReadOnly);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Keys(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Keys);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Values(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Values);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getItem(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
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
	static public int setItem(IntPtr l) {
		try {
			System.Collections.Hashtable self=(System.Collections.Hashtable)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			System.Object c;
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
		getTypeTable(l,"System.Collections.Hashtable");
		addMember(l,Add);
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,Remove);
		addMember(l,ContainsKey);
		addMember(l,ContainsValue);
		addMember(l,GetObjectData);
		addMember(l,OnDeserialization);
		addMember(l,Synchronized_s);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"Count",get_Count,null,true);
		addMember(l,"IsSynchronized",get_IsSynchronized,null,true);
		addMember(l,"SyncRoot",get_SyncRoot,null,true);
		addMember(l,"IsFixedSize",get_IsFixedSize,null,true);
		addMember(l,"IsReadOnly",get_IsReadOnly,null,true);
		addMember(l,"Keys",get_Keys,null,true);
		addMember(l,"Values",get_Values,null,true);
		createTypeMetatable(l,constructor, typeof(System.Collections.Hashtable));
	}
}
