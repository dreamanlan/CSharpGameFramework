using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_System_Threading_ClientInterlocked : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Add_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int64),typeof(System.Int64))){
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
			else if(matchType(l,argc,1,typeof(System.Int32),typeof(int))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CompareExchange_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.IntPtr),typeof(System.IntPtr),typeof(System.IntPtr))){
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
			else if(matchType(l,argc,1,typeof(object),typeof(System.Object),typeof(System.Object))){
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
			else if(matchType(l,argc,1,typeof(System.Single),typeof(float),typeof(float))){
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
			else if(matchType(l,argc,1,typeof(System.Double),typeof(double),typeof(double))){
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
			else if(matchType(l,argc,1,typeof(System.Int32),typeof(int),typeof(int))){
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
			else if(matchType(l,argc,1,typeof(System.Int64),typeof(System.Int64),typeof(System.Int64))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Decrement_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Threading.ClientInterlocked.Decrement(ref a1);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(System.Int32))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Threading.ClientInterlocked.Decrement(ref a1);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
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
	static public int Exchange_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.IntPtr),typeof(System.IntPtr))){
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
			else if(matchType(l,argc,1,typeof(object),typeof(System.Object))){
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
			else if(matchType(l,argc,1,typeof(System.Single),typeof(float))){
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
			else if(matchType(l,argc,1,typeof(System.Double),typeof(double))){
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
			else if(matchType(l,argc,1,typeof(System.Int32),typeof(int))){
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
			else if(matchType(l,argc,1,typeof(System.Int64),typeof(System.Int64))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Increment_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Threading.ClientInterlocked.Increment(ref a1);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(System.Int32))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Threading.ClientInterlocked.Increment(ref a1);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientInterlocked");
		addMember(l,Add_s);
		addMember(l,CompareExchange_s);
		addMember(l,Decrement_s);
		addMember(l,Exchange_s);
		addMember(l,Increment_s);
		addMember(l,Read_s);
		createTypeMetatable(l,null, typeof(System.Threading.ClientInterlocked));
	}
}
