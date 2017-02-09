using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_Meta_BasicList_NodeEnumerator : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.Meta.BasicList.NodeEnumerator o;
			o=new ProtoBuf.Meta.BasicList.NodeEnumerator();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MoveNext(IntPtr l) {
		try {
			ProtoBuf.Meta.BasicList.NodeEnumerator self;
			checkValueType(l,1,out self);
			var ret=self.MoveNext();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Current(IntPtr l) {
		try {
			ProtoBuf.Meta.BasicList.NodeEnumerator self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Current);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.BasicList.NodeEnumerator");
		addMember(l,MoveNext);
		addMember(l,"Current",get_Current,null,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.Meta.BasicList.NodeEnumerator),typeof(System.ValueType));
	}
}
