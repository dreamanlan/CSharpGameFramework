using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_DiscriminatedUnion64Object : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object o;
			o=new ProtoBuf.DiscriminatedUnion64Object();
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
	static public int ctor__Int32__Int64_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int64 a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
	static public int ctor__Int32__UInt64_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.UInt64 a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.UInt32 a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
	static public int ctor__Int32__Object_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
	static public int ctor__Int32__Double_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Double a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnion64Object(a1,a2);
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
			ProtoBuf.DiscriminatedUnion64Object self;
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
			ProtoBuf.DiscriminatedUnion64Object a1;
			checkValueType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			ProtoBuf.DiscriminatedUnion64Object.Reset(ref a1,a2);
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
	static public int get_Int64(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Int64);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UInt64(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.UInt64);
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
			ProtoBuf.DiscriminatedUnion64Object self;
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
			ProtoBuf.DiscriminatedUnion64Object self;
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
			ProtoBuf.DiscriminatedUnion64Object self;
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
			ProtoBuf.DiscriminatedUnion64Object self;
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
	static public int get_Double(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Double);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DateTime(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.DateTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TimeSpan(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TimeSpan);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Object(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnion64Object self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Object);
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
			ProtoBuf.DiscriminatedUnion64Object self;
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
		getTypeTable(l,"ProtoBuf.DiscriminatedUnion64Object");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32__Int64_s);
		addMember(l,ctor__Int32__Int32_s);
		addMember(l,ctor__Int32__UInt64_s);
		addMember(l,ctor__Int32__UInt32_s);
		addMember(l,ctor__Int32__Boolean_s);
		addMember(l,ctor__Int32__Object_s);
		addMember(l,ctor__Int32__Single_s);
		addMember(l,ctor__Int32__Double_s);
		addMember(l,Is);
		addMember(l,Reset_s);
		addMember(l,"Int64",get_Int64,null,true);
		addMember(l,"UInt64",get_UInt64,null,true);
		addMember(l,"Int32",get_Int32,null,true);
		addMember(l,"UInt32",get_UInt32,null,true);
		addMember(l,"Boolean",get_Boolean,null,true);
		addMember(l,"Single",get_Single,null,true);
		addMember(l,"Double",get_Double,null,true);
		addMember(l,"DateTime",get_DateTime,null,true);
		addMember(l,"TimeSpan",get_TimeSpan,null,true);
		addMember(l,"Object",get_Object,null,true);
		addMember(l,"Discriminator",get_Discriminator,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.DiscriminatedUnion64Object),typeof(System.ValueType));
	}
}
