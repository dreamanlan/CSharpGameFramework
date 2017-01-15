using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UobjHashSet : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UobjHashSet o;
			o=new UobjHashSet();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Add(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			UnityEngine.Object a1;
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
	static public int Clear(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
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
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			UnityEngine.Object a1;
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
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			UnityEngine.Object a1;
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
	static public int RemoveWhere(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Predicate<UnityEngine.Object> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.RemoveWhere(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TrimExcess(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			self.TrimExcess();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IntersectWith(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			self.IntersectWith(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExceptWith(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			self.ExceptWith(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Overlaps(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			var ret=self.Overlaps(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetEquals(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			var ret=self.SetEquals(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SymmetricExceptWith(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			self.SymmetricExceptWith(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnionWith(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			self.UnionWith(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsSubsetOf(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			var ret=self.IsSubsetOf(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsProperSubsetOf(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			var ret=self.IsProperSubsetOf(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsSupersetOf(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			var ret=self.IsSupersetOf(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsProperSupersetOf(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			System.Collections.Generic.IEnumerable<UnityEngine.Object> a1;
			checkType(l,2,out a1);
			var ret=self.IsProperSupersetOf(a1);
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
			UobjHashSet self=(UobjHashSet)checkSelf(l);
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
			UobjHashSet self=(UobjHashSet)checkSelf(l);
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
	static public int get_Count(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Comparer(IntPtr l) {
		try {
			UobjHashSet self=(UobjHashSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Comparer);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UobjHashSet");
		addMember(l,Add);
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,Remove);
		addMember(l,RemoveWhere);
		addMember(l,TrimExcess);
		addMember(l,IntersectWith);
		addMember(l,ExceptWith);
		addMember(l,Overlaps);
		addMember(l,SetEquals);
		addMember(l,SymmetricExceptWith);
		addMember(l,UnionWith);
		addMember(l,IsSubsetOf);
		addMember(l,IsProperSubsetOf);
		addMember(l,IsSupersetOf);
		addMember(l,IsProperSupersetOf);
		addMember(l,GetObjectData);
		addMember(l,OnDeserialization);
		addMember(l,"Count",get_Count,null,true);
		addMember(l,"Comparer",get_Comparer,null,true);
		createTypeMetatable(l,constructor, typeof(UobjHashSet),typeof(System.Collections.Generic.HashSet<UnityEngine.Object>));
	}
}
