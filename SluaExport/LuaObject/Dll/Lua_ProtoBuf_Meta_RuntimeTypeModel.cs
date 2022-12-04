using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_RuntimeTypeModel : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetTypes(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			var ret=self.GetTypes();
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
	static public int GetSchema__Type(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.GetSchema(a1);
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
	static public int GetSchema__Type__ProtoSyntax(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			ProtoBuf.Meta.ProtoSyntax a2;
			checkEnum(l,3,out a2);
			var ret=self.GetSchema(a1,a2);
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
	static public int Add(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			var ret=self.Add(a1,a2);
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
	static public int Freeze(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			self.Freeze();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompileInPlace(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			self.CompileInPlace();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Compile(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			var ret=self.Compile();
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
	static public int Compile__CompilerOptions(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions a1;
			checkType(l,2,out a1);
			var ret=self.Compile(a1);
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
	static public int Compile__String__String(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			var ret=self.Compile(a1,a2);
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
	static public int SetDefaultFactory(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			System.Reflection.MethodInfo a1;
			checkType(l,2,out a1);
			self.SetDefaultFactory(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Create_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.Meta.RuntimeTypeModel.Create(a1);
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
	static public int get_InferTagFromNameDefault(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.InferTagFromNameDefault);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_InferTagFromNameDefault(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.InferTagFromNameDefault=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AutoAddProtoContractTypesOnly(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AutoAddProtoContractTypesOnly);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AutoAddProtoContractTypesOnly(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.AutoAddProtoContractTypesOnly=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UseImplicitZeroDefaults(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.UseImplicitZeroDefaults);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_UseImplicitZeroDefaults(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.UseImplicitZeroDefaults=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AllowParseableTypes(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AllowParseableTypes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AllowParseableTypes(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.AllowParseableTypes=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IncludeDateTimeKind(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IncludeDateTimeKind);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IncludeDateTimeKind(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IncludeDateTimeKind=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_InternStrings(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.InternStrings);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_InternStrings(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.InternStrings=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Default(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ProtoBuf.Meta.RuntimeTypeModel.Default);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AutoCompile(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AutoCompile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AutoCompile(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.AutoCompile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AutoAddMissingTypes(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AutoAddMissingTypes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AutoAddMissingTypes(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.AutoAddMissingTypes=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MetadataTimeoutMilliseconds(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MetadataTimeoutMilliseconds);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MetadataTimeoutMilliseconds(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.MetadataTimeoutMilliseconds=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LockCount(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LockCount);
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
			ProtoBuf.Meta.RuntimeTypeModel self=(ProtoBuf.Meta.RuntimeTypeModel)checkSelf(l);
			System.Type v;
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.RuntimeTypeModel");
		addMember(l,GetTypes);
		addMember(l,GetSchema__Type);
		addMember(l,GetSchema__Type__ProtoSyntax);
		addMember(l,Add);
		addMember(l,Freeze);
		addMember(l,CompileInPlace);
		addMember(l,Compile);
		addMember(l,Compile__CompilerOptions);
		addMember(l,Compile__String__String);
		addMember(l,SetDefaultFactory);
		addMember(l,Create_s);
		addMember(l,getItem);
		addMember(l,"InferTagFromNameDefault",get_InferTagFromNameDefault,set_InferTagFromNameDefault,true);
		addMember(l,"AutoAddProtoContractTypesOnly",get_AutoAddProtoContractTypesOnly,set_AutoAddProtoContractTypesOnly,true);
		addMember(l,"UseImplicitZeroDefaults",get_UseImplicitZeroDefaults,set_UseImplicitZeroDefaults,true);
		addMember(l,"AllowParseableTypes",get_AllowParseableTypes,set_AllowParseableTypes,true);
		addMember(l,"IncludeDateTimeKind",get_IncludeDateTimeKind,set_IncludeDateTimeKind,true);
		addMember(l,"InternStrings",get_InternStrings,set_InternStrings,true);
		addMember(l,"Default",get_Default,null,false);
		addMember(l,"AutoCompile",get_AutoCompile,set_AutoCompile,true);
		addMember(l,"AutoAddMissingTypes",get_AutoAddMissingTypes,set_AutoAddMissingTypes,true);
		addMember(l,"MetadataTimeoutMilliseconds",get_MetadataTimeoutMilliseconds,set_MetadataTimeoutMilliseconds,true);
		addMember(l,"LockCount",get_LockCount,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.RuntimeTypeModel),typeof(ProtoBuf.Meta.TypeModel));
	}
}
