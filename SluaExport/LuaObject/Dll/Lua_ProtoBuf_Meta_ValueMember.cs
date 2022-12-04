using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_ValueMember : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember o;
			ProtoBuf.Meta.RuntimeTypeModel a1;
			checkType(l,1,out a1);
			System.Type a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Reflection.MemberInfo a4;
			checkType(l,4,out a4);
			System.Type a5;
			checkType(l,5,out a5);
			System.Type a6;
			checkType(l,6,out a6);
			System.Type a7;
			checkType(l,7,out a7);
			ProtoBuf.DataFormat a8;
			checkEnum(l,8,out a8);
			System.Object a9;
			checkType(l,9,out a9);
			o=new ProtoBuf.Meta.ValueMember(a1,a2,a3,a4,a5,a6,a7,a8,a9);
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
	static public int SetSpecified(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			System.Reflection.MethodInfo a1;
			checkType(l,2,out a1);
			System.Reflection.MethodInfo a2;
			checkType(l,3,out a2);
			self.SetSpecified(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_FieldNumber(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FieldNumber);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Member(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Member);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BackingMember(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BackingMember);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_BackingMember(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			System.Reflection.MemberInfo v;
			checkType(l,2,out v);
			self.BackingMember=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ItemType(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ItemType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MemberType(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MemberType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DefaultType(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DefaultType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ParentType(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ParentType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DefaultValue(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DefaultValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DefaultValue(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.DefaultValue=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DataFormat(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.DataFormat);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DataFormat(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_IsStrict(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsStrict);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsStrict(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsStrict=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsPacked(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsPacked);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsPacked(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_OverwriteList(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OverwriteList);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OverwriteList(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_IsRequired(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsRequired);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsRequired(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_AsReference(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AsReference);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AsReference(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_DynamicType(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DynamicType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DynamicType(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_IsMap(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsMap);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsMap(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsMap=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MapKeyFormat(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.MapKeyFormat);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MapKeyFormat(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			ProtoBuf.DataFormat v;
			checkEnum(l,2,out v);
			self.MapKeyFormat=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MapValueFormat(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.MapValueFormat);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MapValueFormat(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			ProtoBuf.DataFormat v;
			checkEnum(l,2,out v);
			self.MapValueFormat=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Name(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
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
	static public int get_SupportNull(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SupportNull);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SupportNull(IntPtr l) {
		try {
			ProtoBuf.Meta.ValueMember self=(ProtoBuf.Meta.ValueMember)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.SupportNull=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.ValueMember");
		addMember(l,ctor_s);
		addMember(l,SetSpecified);
		addMember(l,"FieldNumber",get_FieldNumber,null,true);
		addMember(l,"Member",get_Member,null,true);
		addMember(l,"BackingMember",get_BackingMember,set_BackingMember,true);
		addMember(l,"ItemType",get_ItemType,null,true);
		addMember(l,"MemberType",get_MemberType,null,true);
		addMember(l,"DefaultType",get_DefaultType,null,true);
		addMember(l,"ParentType",get_ParentType,null,true);
		addMember(l,"DefaultValue",get_DefaultValue,set_DefaultValue,true);
		addMember(l,"DataFormat",get_DataFormat,set_DataFormat,true);
		addMember(l,"IsStrict",get_IsStrict,set_IsStrict,true);
		addMember(l,"IsPacked",get_IsPacked,set_IsPacked,true);
		addMember(l,"OverwriteList",get_OverwriteList,set_OverwriteList,true);
		addMember(l,"IsRequired",get_IsRequired,set_IsRequired,true);
		addMember(l,"AsReference",get_AsReference,set_AsReference,true);
		addMember(l,"DynamicType",get_DynamicType,set_DynamicType,true);
		addMember(l,"IsMap",get_IsMap,set_IsMap,true);
		addMember(l,"MapKeyFormat",get_MapKeyFormat,set_MapKeyFormat,true);
		addMember(l,"MapValueFormat",get_MapValueFormat,set_MapValueFormat,true);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"SupportNull",get_SupportNull,set_SupportNull,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.ValueMember));
	}
}
