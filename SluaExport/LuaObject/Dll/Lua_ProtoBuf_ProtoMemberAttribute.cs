using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoMemberAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute o;
			System.Int32 a1;
			checkType(l,2,out a1);
			o=new ProtoBuf.ProtoMemberAttribute(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CompareTo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ProtoBuf.ProtoMemberAttribute))){
				ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
				ProtoBuf.ProtoMemberAttribute a1;
				checkType(l,2,out a1);
				var ret=self.CompareTo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Object))){
				ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
				System.Object a1;
				checkType(l,2,out a1);
				var ret=self.CompareTo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int get_Name(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
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
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DataFormat(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
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
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Tag(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Tag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsRequired(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsRequired);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsRequired(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsRequired=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsPacked(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsPacked);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsPacked(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsPacked=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_OverwriteList(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OverwriteList);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OverwriteList(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.OverwriteList=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AsReference(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AsReference);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_AsReference(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.AsReference=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DynamicType(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DynamicType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DynamicType(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.DynamicType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Options(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.Options);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Options(IntPtr l) {
		try {
			ProtoBuf.ProtoMemberAttribute self=(ProtoBuf.ProtoMemberAttribute)checkSelf(l);
			ProtoBuf.MemberSerializationOptions v;
			checkEnum(l,2,out v);
			self.Options=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoMemberAttribute");
		addMember(l,CompareTo);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"DataFormat",get_DataFormat,set_DataFormat,true);
		addMember(l,"Tag",get_Tag,null,true);
		addMember(l,"IsRequired",get_IsRequired,set_IsRequired,true);
		addMember(l,"IsPacked",get_IsPacked,set_IsPacked,true);
		addMember(l,"OverwriteList",get_OverwriteList,set_OverwriteList,true);
		addMember(l,"AsReference",get_AsReference,set_AsReference,true);
		addMember(l,"DynamicType",get_DynamicType,set_DynamicType,true);
		addMember(l,"Options",get_Options,set_Options,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoMemberAttribute),typeof(System.Attribute));
	}
}
