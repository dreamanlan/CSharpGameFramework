using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Serializer_NonGeneric : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DeepClone_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.Serializer.NonGeneric.DeepClone(a1);
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
	static public int Serialize_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			ProtoBuf.Serializer.NonGeneric.Serialize(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Deserialize_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.IO.Stream a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.Serializer.NonGeneric.Deserialize(a1,a2);
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
	static public int Merge_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.Serializer.NonGeneric.Merge(a1,a2);
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
	static public int SerializeWithLengthPrefix_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			ProtoBuf.PrefixStyle a3;
			checkEnum(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			ProtoBuf.Serializer.NonGeneric.SerializeWithLengthPrefix(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryDeserializeWithLengthPrefix_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			ProtoBuf.PrefixStyle a2;
			checkEnum(l,2,out a2);
			ProtoBuf.Serializer.TypeResolver a3;
			LuaDelegation.checkDelegate(l,3,out a3);
			System.Object a4;
			var ret=ProtoBuf.Serializer.NonGeneric.TryDeserializeWithLengthPrefix(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CanSerialize_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.Serializer.NonGeneric.CanSerialize(a1);
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
	static public int PrepareSerializer_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			ProtoBuf.Serializer.NonGeneric.PrepareSerializer(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Serializer.NonGeneric");
		addMember(l,DeepClone_s);
		addMember(l,Serialize_s);
		addMember(l,Deserialize_s);
		addMember(l,Merge_s);
		addMember(l,SerializeWithLengthPrefix_s);
		addMember(l,TryDeserializeWithLengthPrefix_s);
		addMember(l,CanSerialize_s);
		addMember(l,PrepareSerializer_s);
		createTypeMetatable(l,null, typeof(ProtoBuf.Serializer.NonGeneric));
	}
}
