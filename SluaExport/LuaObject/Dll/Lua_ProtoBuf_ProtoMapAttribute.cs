using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ProtoMapAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.ProtoMapAttribute o;
			o=new ProtoBuf.ProtoMapAttribute();
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
	static public int get_KeyFormat(IntPtr l) {
		try {
			ProtoBuf.ProtoMapAttribute self=(ProtoBuf.ProtoMapAttribute)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.KeyFormat);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_KeyFormat(IntPtr l) {
		try {
			ProtoBuf.ProtoMapAttribute self=(ProtoBuf.ProtoMapAttribute)checkSelf(l);
			ProtoBuf.DataFormat v;
			checkEnum(l,2,out v);
			self.KeyFormat=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ValueFormat(IntPtr l) {
		try {
			ProtoBuf.ProtoMapAttribute self=(ProtoBuf.ProtoMapAttribute)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.ValueFormat);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ValueFormat(IntPtr l) {
		try {
			ProtoBuf.ProtoMapAttribute self=(ProtoBuf.ProtoMapAttribute)checkSelf(l);
			ProtoBuf.DataFormat v;
			checkEnum(l,2,out v);
			self.ValueFormat=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DisableMap(IntPtr l) {
		try {
			ProtoBuf.ProtoMapAttribute self=(ProtoBuf.ProtoMapAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DisableMap);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DisableMap(IntPtr l) {
		try {
			ProtoBuf.ProtoMapAttribute self=(ProtoBuf.ProtoMapAttribute)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.DisableMap=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoMapAttribute");
		addMember(l,ctor_s);
		addMember(l,"KeyFormat",get_KeyFormat,set_KeyFormat,true);
		addMember(l,"ValueFormat",get_ValueFormat,set_ValueFormat,true);
		addMember(l,"DisableMap",get_DisableMap,set_DisableMap,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.ProtoMapAttribute),typeof(System.Attribute));
	}
}
