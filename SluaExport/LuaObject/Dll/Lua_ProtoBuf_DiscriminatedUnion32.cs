using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_DiscriminatedUnion32 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 o;
			o=new ProtoBuf.DiscriminatedUnion32();
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
	static public int ctor__Int32__Int32_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion32(a1,a2);
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
	static public int ctor__Int32__UInt32_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.UInt32 a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion32(a1,a2);
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
	static public int ctor__Int32__Boolean_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion32(a1,a2);
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
	static public int ctor__Int32__Single_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion32(a1,a2);
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
	static public int Is(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.Is(a1);
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
	static public int Reset_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 a1;
			checkValueType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			ProtoBuf.DiscriminatedUnion32.Reset(ref a1,a2);
			pushValue(l,true);
			pushValue(l,a1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Int32(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Int32);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UInt32(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.UInt32);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Boolean(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Boolean);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Single(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Single);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Discriminator(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion32 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Discriminator);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.DiscriminatedUnion32");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32__Int32_s);
		addMember(l,ctor__Int32__UInt32_s);
		addMember(l,ctor__Int32__Boolean_s);
		addMember(l,ctor__Int32__Single_s);
		addMember(l,Is);
		addMember(l,Reset_s);
		addMember(l,"Int32",get_Int32,null,true);
		addMember(l,"UInt32",get_UInt32,null,true);
		addMember(l,"Boolean",get_Boolean,null,true);
		addMember(l,"Single",get_Single,null,true);
		addMember(l,"Discriminator",get_Discriminator,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.DiscriminatedUnion32),typeof(System.ValueType));
	}
}
