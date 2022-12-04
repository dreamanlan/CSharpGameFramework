using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_RuntimeTypeModel_CompilerOptions : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions o;
			o=new ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions();
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
	static public int SetFrameworkOptions(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			ProtoBuf.Meta.MetaType a1;
			checkType(l,2,out a1);
			self.SetFrameworkOptions(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TargetFrameworkName(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetFrameworkName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TargetFrameworkName(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.TargetFrameworkName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TargetFrameworkDisplayName(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetFrameworkDisplayName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TargetFrameworkDisplayName(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.TargetFrameworkDisplayName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TypeName(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TypeName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TypeName(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.TypeName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_OutputPath(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OutputPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OutputPath(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.OutputPath=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ImageRuntimeVersion(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImageRuntimeVersion);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ImageRuntimeVersion(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.ImageRuntimeVersion=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MetaDataVersion(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MetaDataVersion);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MetaDataVersion(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.MetaDataVersion=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Accessibility(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.Accessibility);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Accessibility(IntPtr l) {
		try {
			ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions self=(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions)checkSelf(l);
			ProtoBuf.Meta.RuntimeTypeModel.Accessibility v;
			checkEnum(l,2,out v);
			self.Accessibility=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions");
		addMember(l,ctor_s);
		addMember(l,SetFrameworkOptions);
		addMember(l,"TargetFrameworkName",get_TargetFrameworkName,set_TargetFrameworkName,true);
		addMember(l,"TargetFrameworkDisplayName",get_TargetFrameworkDisplayName,set_TargetFrameworkDisplayName,true);
		addMember(l,"TypeName",get_TypeName,set_TypeName,true);
		addMember(l,"OutputPath",get_OutputPath,set_OutputPath,true);
		addMember(l,"ImageRuntimeVersion",get_ImageRuntimeVersion,set_ImageRuntimeVersion,true);
		addMember(l,"MetaDataVersion",get_MetaDataVersion,set_MetaDataVersion,true);
		addMember(l,"Accessibility",get_Accessibility,set_Accessibility,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.RuntimeTypeModel.CompilerOptions));
	}
}
