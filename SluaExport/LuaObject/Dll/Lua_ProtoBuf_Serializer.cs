using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Serializer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Deserialize_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.IO.Stream a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.Serializer.Deserialize(a1,a2);
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
	static public int TryReadLengthPrefix__Stream__PrefixStyle__O_Int32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			ProtoBuf.PrefixStyle a2;
			checkEnum(l,2,out a2);
			System.Int32 a3;
			var ret=ProtoBuf.Serializer.TryReadLengthPrefix(a1,a2,out a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryReadLengthPrefix__A_Byte__Int32__Int32__PrefixStyle__O_Int32_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			ProtoBuf.PrefixStyle a4;
			checkEnum(l,4,out a4);
			System.Int32 a5;
			var ret=ProtoBuf.Serializer.TryReadLengthPrefix(a1,a2,a3,a4,out a5);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a5);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FlushPool_s(IntPtr l) {
		try {
			ProtoBuf.Serializer.FlushPool();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ListItemTag(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ProtoBuf.Serializer.ListItemTag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Serializer");
		addMember(l,Deserialize_s);
		addMember(l,TryReadLengthPrefix__Stream__PrefixStyle__O_Int32_s);
		addMember(l,TryReadLengthPrefix__A_Byte__Int32__Int32__PrefixStyle__O_Int32_s);
		addMember(l,FlushPool_s);
		addMember(l,"ListItemTag",get_ListItemTag,null,false);
		createTypeMetatable(l,null, typeof(ProtoBuf.Serializer));
	}
}
