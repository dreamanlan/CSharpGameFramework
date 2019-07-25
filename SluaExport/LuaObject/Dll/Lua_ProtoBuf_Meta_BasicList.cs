using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_Meta_BasicList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.Meta.BasicList o;
			o=new ProtoBuf.Meta.BasicList();
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
			ProtoBuf.Meta.BasicList self=(ProtoBuf.Meta.BasicList)checkSelf(l);
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
	static public int Trim(IntPtr l) {
		try {
			ProtoBuf.Meta.BasicList self=(ProtoBuf.Meta.BasicList)checkSelf(l);
			self.Trim();
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
			ProtoBuf.Meta.BasicList self=(ProtoBuf.Meta.BasicList)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getItem(IntPtr l) {
		try {
			ProtoBuf.Meta.BasicList self=(ProtoBuf.Meta.BasicList)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.BasicList");
		addMember(l,Add);
		addMember(l,Trim);
		addMember(l,getItem);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.Meta.BasicList));
	}
}
