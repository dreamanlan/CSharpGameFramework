using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_IStoryValueList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			StorySystem.IStoryValueList o;
			o=new StorySystem.IStoryValueList();
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
			StorySystem.IStoryValueList o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new StorySystem.IStoryValueList(a1);
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			self.Add(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int BinarySearch(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			var ret=self.BinarySearch(a1);
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
	static public int Clear(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue[] a1;
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue[] a1;
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
	static public int CopyTo__Int32__A_T__Int32__Int32(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			StorySystem.IStoryValue[] a2;
			checkArray(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			self.CopyTo(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IndexOf__T(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			var ret=self.IndexOf(a1);
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
	static public int IndexOf__T__Int32(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.IndexOf(a1,a2);
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
	static public int IndexOf__T__Int32__Int32(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.IndexOf(a1,a2,a3);
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
	static public int Insert(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			StorySystem.IStoryValue a2;
			checkType(l,3,out a2);
			self.Insert(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LastIndexOf__T(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			var ret=self.LastIndexOf(a1);
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
	static public int LastIndexOf__T__Int32(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.LastIndexOf(a1,a2);
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
	static public int LastIndexOf__T__Int32__Int32(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.LastIndexOf(a1,a2,a3);
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
	static public int Remove(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			StorySystem.IStoryValue a1;
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
	static public int RemoveAt(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveAt(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveRange(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.RemoveRange(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reverse(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			self.Reverse();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reverse__Int32__Int32(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.Reverse(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sort(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			self.Sort();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ToArray(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			var ret=self.ToArray();
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
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
	static public int get_Capacity(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Capacity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Capacity(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Capacity=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Count(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
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
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int setItem(IntPtr l) {
		try {
			StorySystem.IStoryValueList self=(StorySystem.IStoryValueList)checkSelf(l);
			int v;
			checkType(l,2,out v);
			StorySystem.IStoryValue c;
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
		getTypeTable(l,"StorySystem.IStoryValueList");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32_s);
		addMember(l,Add);
		addMember(l,BinarySearch);
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,CopyTo__A_T);
		addMember(l,CopyTo__A_T__Int32);
		addMember(l,CopyTo__Int32__A_T__Int32__Int32);
		addMember(l,IndexOf__T);
		addMember(l,IndexOf__T__Int32);
		addMember(l,IndexOf__T__Int32__Int32);
		addMember(l,Insert);
		addMember(l,LastIndexOf__T);
		addMember(l,LastIndexOf__T__Int32);
		addMember(l,LastIndexOf__T__Int32__Int32);
		addMember(l,Remove);
		addMember(l,RemoveAt);
		addMember(l,RemoveRange);
		addMember(l,Reverse);
		addMember(l,Reverse__Int32__Int32);
		addMember(l,Sort);
		addMember(l,ToArray);
		addMember(l,TrimExcess);
		addMember(l,Equals);
		addMember(l,ToString);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"Capacity",get_Capacity,set_Capacity,true);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,null, typeof(StorySystem.IStoryValueList),typeof(System.Collections.Generic.List<StorySystem.IStoryValue>));
	}
}
