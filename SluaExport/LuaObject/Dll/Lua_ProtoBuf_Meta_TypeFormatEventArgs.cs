using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_TypeFormatEventArgs : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Type(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeFormatEventArgs self=(ProtoBuf.Meta.TypeFormatEventArgs)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Type(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeFormatEventArgs self=(ProtoBuf.Meta.TypeFormatEventArgs)checkSelf(l);
			System.Type v;
			checkType(l,2,out v);
			self.Type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_FormattedName(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeFormatEventArgs self=(ProtoBuf.Meta.TypeFormatEventArgs)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FormattedName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_FormattedName(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeFormatEventArgs self=(ProtoBuf.Meta.TypeFormatEventArgs)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.FormattedName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.TypeFormatEventArgs");
		addMember(l,"Type",get_Type,set_Type,true);
		addMember(l,"FormattedName",get_FormattedName,set_FormattedName,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.TypeFormatEventArgs),typeof(System.EventArgs));
	}
}
