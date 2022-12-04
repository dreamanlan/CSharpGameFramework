using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_System_Threading_ClientInterlocked : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Add__R_Int32__Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Add(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Add__R_Int64__Int64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			System.Int64 a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Add(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompareExchange__R_Int32__Int32__Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=System.Threading.ClientInterlocked.CompareExchange(ref a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompareExchange__R_Int64__Int64__Int64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			System.Int64 a2;
			checkType(l,2,out a2);
			System.Int64 a3;
			checkType(l,3,out a3);
			var ret=System.Threading.ClientInterlocked.CompareExchange(ref a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompareExchange__R_IntPtr__IntPtr__IntPtr_s(IntPtr l) {
		try {
			System.IntPtr a1;
			checkType(l,1,out a1);
			System.IntPtr a2;
			checkType(l,2,out a2);
			System.IntPtr a3;
			checkType(l,3,out a3);
			var ret=System.Threading.ClientInterlocked.CompareExchange(ref a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompareExchange__R_Object__Object__Object_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.Object a3;
			checkType(l,3,out a3);
			var ret=System.Threading.ClientInterlocked.CompareExchange(ref a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompareExchange__R_Double__Double__Double_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			System.Double a2;
			checkType(l,2,out a2);
			System.Double a3;
			checkType(l,3,out a3);
			var ret=System.Threading.ClientInterlocked.CompareExchange(ref a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompareExchange__R_Single__Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=System.Threading.ClientInterlocked.CompareExchange(ref a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Decrement__R_Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=System.Threading.ClientInterlocked.Decrement(ref a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Decrement__R_Int64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=System.Threading.ClientInterlocked.Decrement(ref a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Exchange__R_Int32__Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Exchange(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Exchange__R_Int64__Int64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			System.Int64 a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Exchange(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Exchange__R_IntPtr__IntPtr_s(IntPtr l) {
		try {
			System.IntPtr a1;
			checkType(l,1,out a1);
			System.IntPtr a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Exchange(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Exchange__R_Object__Object_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Exchange(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Exchange__R_Double__Double_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			System.Double a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Exchange(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Exchange__R_Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=System.Threading.ClientInterlocked.Exchange(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Increment__R_Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=System.Threading.ClientInterlocked.Increment(ref a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Increment__R_Int64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=System.Threading.ClientInterlocked.Increment(ref a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Read_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=System.Threading.ClientInterlocked.Read(ref a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientInterlocked");
		addMember(l,Add__R_Int32__Int32_s);
		addMember(l,Add__R_Int64__Int64_s);
		addMember(l,CompareExchange__R_Int32__Int32__Int32_s);
		addMember(l,CompareExchange__R_Int64__Int64__Int64_s);
		addMember(l,CompareExchange__R_IntPtr__IntPtr__IntPtr_s);
		addMember(l,CompareExchange__R_Object__Object__Object_s);
		addMember(l,CompareExchange__R_Double__Double__Double_s);
		addMember(l,CompareExchange__R_Single__Single__Single_s);
		addMember(l,Decrement__R_Int32_s);
		addMember(l,Decrement__R_Int64_s);
		addMember(l,Exchange__R_Int32__Int32_s);
		addMember(l,Exchange__R_Int64__Int64_s);
		addMember(l,Exchange__R_IntPtr__IntPtr_s);
		addMember(l,Exchange__R_Object__Object_s);
		addMember(l,Exchange__R_Double__Double_s);
		addMember(l,Exchange__R_Single__Single_s);
		addMember(l,Increment__R_Int32_s);
		addMember(l,Increment__R_Int64_s);
		addMember(l,Read_s);
		createTypeMetatable(l,null, typeof(System.Threading.ClientInterlocked));
	}
}
