using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_SubType : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.Meta.SubType o;
			System.Int32 a1;
			checkType(l,1,out a1);
			ProtoBuf.Meta.MetaType a2;
			checkType(l,2,out a2);
			ProtoBuf.DataFormat a3;
			checkEnum(l,3,out a3);
			o=new ProtoBuf.Meta.SubType(a1,a2,a3);
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
	static public int get_FieldNumber(IntPtr l) {
		try {
			ProtoBuf.Meta.SubType self=(ProtoBuf.Meta.SubType)checkSelf(l);
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
	static public int get_DerivedType(IntPtr l) {
		try {
			ProtoBuf.Meta.SubType self=(ProtoBuf.Meta.SubType)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DerivedType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.SubType");
		addMember(l,ctor_s);
		addMember(l,"FieldNumber",get_FieldNumber,null,true);
		addMember(l,"DerivedType",get_DerivedType,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.SubType));
	}
}
