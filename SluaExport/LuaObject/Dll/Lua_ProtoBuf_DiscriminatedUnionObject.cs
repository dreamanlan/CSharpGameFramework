using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_DiscriminatedUnionObject : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnionObject o;
			o=new ProtoBuf.DiscriminatedUnionObject();
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
			ProtoBuf.DiscriminatedUnionObject o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.DiscriminatedUnionObject(a1,a2);
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
			ProtoBuf.DiscriminatedUnionObject self;
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
			ProtoBuf.DiscriminatedUnionObject a1;
			checkValueType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			ProtoBuf.DiscriminatedUnionObject.Reset(ref a1,a2);
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
	static public int get_Object(IntPtr l) {
		try {
			ProtoBuf.DiscriminatedUnionObject self;
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
			ProtoBuf.DiscriminatedUnionObject self;
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
		getTypeTable(l,"ProtoBuf.DiscriminatedUnionObject");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32__Object_s);
		addMember(l,Is);
		addMember(l,Reset_s);
		addMember(l,"Object",get_Object,null,true);
		addMember(l,"Discriminator",get_Discriminator,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.DiscriminatedUnionObject),typeof(System.ValueType));
	}
}
