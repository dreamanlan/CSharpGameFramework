using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoIncludeAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			ProtoBuf.ProtoIncludeAttribute o;
			if(matchType(l,argc,2,typeof(int),typeof(System.Type))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Type a2;
				checkType(l,3,out a2);
				o=new ProtoBuf.ProtoIncludeAttribute(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(string))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				o=new ProtoBuf.ProtoIncludeAttribute(a1,a2);
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
	static public int get_Tag(IntPtr l) {
		try {
			ProtoBuf.ProtoIncludeAttribute self=(ProtoBuf.ProtoIncludeAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Tag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_KnownTypeName(IntPtr l) {
		try {
			ProtoBuf.ProtoIncludeAttribute self=(ProtoBuf.ProtoIncludeAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KnownTypeName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_KnownType(IntPtr l) {
		try {
			ProtoBuf.ProtoIncludeAttribute self=(ProtoBuf.ProtoIncludeAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KnownType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DataFormat(IntPtr l) {
		try {
			ProtoBuf.ProtoIncludeAttribute self=(ProtoBuf.ProtoIncludeAttribute)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.DataFormat);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DataFormat(IntPtr l) {
		try {
			ProtoBuf.ProtoIncludeAttribute self=(ProtoBuf.ProtoIncludeAttribute)checkSelf(l);
			ProtoBuf.DataFormat v;
			checkEnum(l,2,out v);
			self.DataFormat=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoIncludeAttribute");
		addMember(l,"Tag",get_Tag,null,true);
		addMember(l,"KnownTypeName",get_KnownTypeName,null,true);
		addMember(l,"KnownType",get_KnownType,null,true);
		addMember(l,"DataFormat",get_DataFormat,set_DataFormat,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoIncludeAttribute),typeof(System.Attribute));
	}
}
