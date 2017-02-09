using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_IExtension : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BeginAppend(IntPtr l) {
		try {
			ProtoBuf.IExtension self=(ProtoBuf.IExtension)checkSelf(l);
			var ret=self.BeginAppend();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EndAppend(IntPtr l) {
		try {
			ProtoBuf.IExtension self=(ProtoBuf.IExtension)checkSelf(l);
			System.IO.Stream a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.EndAppend(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BeginQuery(IntPtr l) {
		try {
			ProtoBuf.IExtension self=(ProtoBuf.IExtension)checkSelf(l);
			var ret=self.BeginQuery();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EndQuery(IntPtr l) {
		try {
			ProtoBuf.IExtension self=(ProtoBuf.IExtension)checkSelf(l);
			System.IO.Stream a1;
			checkType(l,2,out a1);
			self.EndQuery(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLength(IntPtr l) {
		try {
			ProtoBuf.IExtension self=(ProtoBuf.IExtension)checkSelf(l);
			var ret=self.GetLength();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.IExtension");
		addMember(l,BeginAppend);
		addMember(l,EndAppend);
		addMember(l,BeginQuery);
		addMember(l,EndQuery);
		addMember(l,GetLength);
		createTypeMetatable(l,null, typeof(ProtoBuf.IExtension));
	}
}
