using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_SerializationContext : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.SerializationContext o;
			o=new ProtoBuf.SerializationContext();
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
	static public int op_Implicit__StreamingContext__SerializationContext_s(IntPtr l) {
		try {
			ProtoBuf.SerializationContext a1;
			checkType(l,1,out a1);
			System.Runtime.Serialization.StreamingContext ret=a1;
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
	static public int op_Implicit__SerializationContext__StreamingContext_s(IntPtr l) {
		try {
			System.Runtime.Serialization.StreamingContext a1;
			checkValueType(l,1,out a1);
			ProtoBuf.SerializationContext ret=a1;
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
	static public int get_Context(IntPtr l) {
		try {
			ProtoBuf.SerializationContext self=(ProtoBuf.SerializationContext)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Context);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Context(IntPtr l) {
		try {
			ProtoBuf.SerializationContext self=(ProtoBuf.SerializationContext)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.Context=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_State(IntPtr l) {
		try {
			ProtoBuf.SerializationContext self=(ProtoBuf.SerializationContext)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.State);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_State(IntPtr l) {
		try {
			ProtoBuf.SerializationContext self=(ProtoBuf.SerializationContext)checkSelf(l);
			System.Runtime.Serialization.StreamingContextStates v;
			checkEnum(l,2,out v);
			self.State=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.SerializationContext");
		addMember(l,ctor_s);
		addMember(l,op_Implicit__StreamingContext__SerializationContext_s);
		addMember(l,op_Implicit__SerializationContext__StreamingContext_s);
		addMember(l,"Context",get_Context,set_Context,true);
		addMember(l,"State",get_State,set_State,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.SerializationContext));
	}
}
