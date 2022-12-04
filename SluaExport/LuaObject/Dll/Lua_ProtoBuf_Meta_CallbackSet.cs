using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_CallbackSet : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BeforeSerialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BeforeSerialize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_BeforeSerialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			System.Reflection.MethodInfo v;
			checkType(l,2,out v);
			self.BeforeSerialize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BeforeDeserialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BeforeDeserialize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_BeforeDeserialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			System.Reflection.MethodInfo v;
			checkType(l,2,out v);
			self.BeforeDeserialize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AfterSerialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AfterSerialize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AfterSerialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			System.Reflection.MethodInfo v;
			checkType(l,2,out v);
			self.AfterSerialize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AfterDeserialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AfterDeserialize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AfterDeserialize(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			System.Reflection.MethodInfo v;
			checkType(l,2,out v);
			self.AfterDeserialize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NonTrivial(IntPtr l) {
		try {
			ProtoBuf.Meta.CallbackSet self=(ProtoBuf.Meta.CallbackSet)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.NonTrivial);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.CallbackSet");
		addMember(l,"BeforeSerialize",get_BeforeSerialize,set_BeforeSerialize,true);
		addMember(l,"BeforeDeserialize",get_BeforeDeserialize,set_BeforeDeserialize,true);
		addMember(l,"AfterSerialize",get_AfterSerialize,set_AfterSerialize,true);
		addMember(l,"AfterDeserialize",get_AfterDeserialize,set_AfterDeserialize,true);
		addMember(l,"NonTrivial",get_NonTrivial,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.CallbackSet));
	}
}
