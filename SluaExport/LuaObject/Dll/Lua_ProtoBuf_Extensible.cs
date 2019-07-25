using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_Extensible : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetExtensionObject_s(IntPtr l) {
		try {
			ProtoBuf.IExtension a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.Extensible.GetExtensionObject(ref a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryGetValue_s(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel a1;
			checkType(l,1,out a1);
			System.Type a2;
			checkType(l,2,out a2);
			ProtoBuf.IExtensible a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			ProtoBuf.DataFormat a5;
			checkEnum(l,5,out a5);
			System.Boolean a6;
			checkType(l,6,out a6);
			System.Object a7;
			var ret=ProtoBuf.Extensible.TryGetValue(a1,a2,a3,a4,a5,a6,out a7);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a7);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetValues_s(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel a1;
			checkType(l,1,out a1);
			System.Type a2;
			checkType(l,2,out a2);
			ProtoBuf.IExtensible a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			ProtoBuf.DataFormat a5;
			checkEnum(l,5,out a5);
			var ret=ProtoBuf.Extensible.GetValues(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AppendValue_s(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel a1;
			checkType(l,1,out a1);
			ProtoBuf.IExtensible a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			ProtoBuf.DataFormat a4;
			checkEnum(l,4,out a4);
			System.Object a5;
			checkType(l,5,out a5);
			ProtoBuf.Extensible.AppendValue(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Extensible");
		addMember(l,GetExtensionObject_s);
		addMember(l,TryGetValue_s);
		addMember(l,GetValues_s);
		addMember(l,AppendValue_s);
		createTypeMetatable(l,null, typeof(ProtoBuf.Extensible));
	}
}
