using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ProtoContractAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute o;
			o=new ProtoBuf.ProtoContractAttribute();
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
	static public int get_Name(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Name(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_ImplicitFirstTag(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImplicitFirstTag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ImplicitFirstTag(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ImplicitFirstTag=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UseProtoMembersOnly(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.UseProtoMembersOnly);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_UseProtoMembersOnly(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.UseProtoMembersOnly=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IgnoreListHandling(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IgnoreListHandling);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IgnoreListHandling(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IgnoreListHandling=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ImplicitFields(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.ImplicitFields);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ImplicitFields(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			ProtoBuf.ImplicitFields v;
			checkEnum(l,2,out v);
			self.ImplicitFields=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_InferTagFromName(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.InferTagFromName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_InferTagFromName(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.InferTagFromName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DataMemberOffset(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DataMemberOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DataMemberOffset(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.DataMemberOffset=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SkipConstructor(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkipConstructor);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SkipConstructor(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.SkipConstructor=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AsReferenceDefault(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AsReferenceDefault);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AsReferenceDefault(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.AsReferenceDefault=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsGroup(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsGroup);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsGroup(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsGroup=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EnumPassthru(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EnumPassthru);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_EnumPassthru(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.EnumPassthru=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Surrogate(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Surrogate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Surrogate(IntPtr l) {
		try {
			ProtoBuf.ProtoContractAttribute self=(ProtoBuf.ProtoContractAttribute)checkSelf(l);
			System.Type v;
			checkType(l,2,out v);
			self.Surrogate=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoContractAttribute");
		addMember(l,ctor_s);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"ImplicitFirstTag",get_ImplicitFirstTag,set_ImplicitFirstTag,true);
		addMember(l,"UseProtoMembersOnly",get_UseProtoMembersOnly,set_UseProtoMembersOnly,true);
		addMember(l,"IgnoreListHandling",get_IgnoreListHandling,set_IgnoreListHandling,true);
		addMember(l,"ImplicitFields",get_ImplicitFields,set_ImplicitFields,true);
		addMember(l,"InferTagFromName",get_InferTagFromName,set_InferTagFromName,true);
		addMember(l,"DataMemberOffset",get_DataMemberOffset,set_DataMemberOffset,true);
		addMember(l,"SkipConstructor",get_SkipConstructor,set_SkipConstructor,true);
		addMember(l,"AsReferenceDefault",get_AsReferenceDefault,set_AsReferenceDefault,true);
		addMember(l,"IsGroup",get_IsGroup,set_IsGroup,true);
		addMember(l,"EnumPassthru",get_EnumPassthru,set_EnumPassthru,true);
		addMember(l,"Surrogate",get_Surrogate,set_Surrogate,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.ProtoContractAttribute),typeof(System.Attribute));
	}
}
