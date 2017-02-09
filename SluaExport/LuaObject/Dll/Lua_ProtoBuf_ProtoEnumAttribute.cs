using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoEnumAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoEnumAttribute o;
			o=new ProtoBuf.ProtoEnumAttribute();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HasValue(IntPtr l) {
		try {
			ProtoBuf.ProtoEnumAttribute self=(ProtoBuf.ProtoEnumAttribute)checkSelf(l);
			var ret=self.HasValue();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Value(IntPtr l) {
		try {
			ProtoBuf.ProtoEnumAttribute self=(ProtoBuf.ProtoEnumAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Value(IntPtr l) {
		try {
			ProtoBuf.ProtoEnumAttribute self=(ProtoBuf.ProtoEnumAttribute)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Value=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Name(IntPtr l) {
		try {
			ProtoBuf.ProtoEnumAttribute self=(ProtoBuf.ProtoEnumAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Name(IntPtr l) {
		try {
			ProtoBuf.ProtoEnumAttribute self=(ProtoBuf.ProtoEnumAttribute)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.Name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoEnumAttribute");
		addMember(l,HasValue);
		addMember(l,"Value",get_Value,set_Value,true);
		addMember(l,"Name",get_Name,set_Name,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoEnumAttribute),typeof(System.Attribute));
	}
}
