using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_BclHelpers : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetUninitializedObject_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.BclHelpers.GetUninitializedObject(a1);
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
	static public int WriteTimeSpan_s(IntPtr l) {
		try {
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.BclHelpers.WriteTimeSpan(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadTimeSpan_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.BclHelpers.ReadTimeSpan(a1);
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
	static public int ReadDuration_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.BclHelpers.ReadDuration(a1);
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
	static public int WriteDuration_s(IntPtr l) {
		try {
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.BclHelpers.WriteDuration(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadTimestamp_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.BclHelpers.ReadTimestamp(a1);
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
	static public int WriteTimestamp_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.BclHelpers.WriteTimestamp(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadDateTime_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.BclHelpers.ReadDateTime(a1);
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
	static public int WriteDateTime_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.BclHelpers.WriteDateTime(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int WriteDateTimeWithKind_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.BclHelpers.WriteDateTimeWithKind(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadDecimal_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.BclHelpers.ReadDecimal(a1);
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
	static public int WriteDecimal_s(IntPtr l) {
		try {
			System.Decimal a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.BclHelpers.WriteDecimal(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int WriteGuid_s(IntPtr l) {
		try {
			System.Guid a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.BclHelpers.WriteGuid(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadGuid_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.BclHelpers.ReadGuid(a1);
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
	static public int ReadNetObject_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoReader a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Type a4;
			checkType(l,4,out a4);
			ProtoBuf.BclHelpers.NetObjectOptions a5;
			checkEnum(l,5,out a5);
			var ret=ProtoBuf.BclHelpers.ReadNetObject(a1,a2,a3,a4,a5);
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
	static public int WriteNetObject_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			ProtoBuf.BclHelpers.NetObjectOptions a4;
			checkEnum(l,4,out a4);
			ProtoBuf.BclHelpers.WriteNetObject(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.BclHelpers");
		addMember(l,GetUninitializedObject_s);
		addMember(l,WriteTimeSpan_s);
		addMember(l,ReadTimeSpan_s);
		addMember(l,ReadDuration_s);
		addMember(l,WriteDuration_s);
		addMember(l,ReadTimestamp_s);
		addMember(l,WriteTimestamp_s);
		addMember(l,ReadDateTime_s);
		addMember(l,WriteDateTime_s);
		addMember(l,WriteDateTimeWithKind_s);
		addMember(l,ReadDecimal_s);
		addMember(l,WriteDecimal_s);
		addMember(l,WriteGuid_s);
		addMember(l,ReadGuid_s);
		addMember(l,ReadNetObject_s);
		addMember(l,WriteNetObject_s);
		createTypeMetatable(l,null, typeof(ProtoBuf.BclHelpers));
	}
}
