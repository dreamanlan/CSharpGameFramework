using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoWriter : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoWriter o;
			System.IO.Stream a1;
			checkType(l,2,out a1);
			ProtoBuf.Meta.TypeModel a2;
			checkType(l,3,out a2);
			ProtoBuf.SerializationContext a3;
			checkType(l,4,out a3);
			o=new ProtoBuf.ProtoWriter(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Close(IntPtr l) {
		try {
			ProtoBuf.ProtoWriter self=(ProtoBuf.ProtoWriter)checkSelf(l);
			self.Close();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetRootObject(IntPtr l) {
		try {
			ProtoBuf.ProtoWriter self=(ProtoBuf.ProtoWriter)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.SetRootObject(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteObject_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter a3;
			checkType(l,3,out a3);
			ProtoBuf.ProtoWriter.WriteObject(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteRecursionSafeObject_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter a3;
			checkType(l,3,out a3);
			ProtoBuf.ProtoWriter.WriteRecursionSafeObject(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteFieldHeader_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			ProtoBuf.WireType a2;
			checkEnum(l,2,out a2);
			ProtoBuf.ProtoWriter a3;
			checkType(l,3,out a3);
			ProtoBuf.ProtoWriter.WriteFieldHeader(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteBytes_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				ProtoBuf.ProtoWriter a2;
				checkType(l,2,out a2);
				ProtoBuf.ProtoWriter.WriteBytes(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				ProtoBuf.ProtoWriter a4;
				checkType(l,4,out a4);
				ProtoBuf.ProtoWriter.WriteBytes(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StartSubItem_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.ProtoWriter.StartSubItem(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EndSubItem_s(IntPtr l) {
		try {
			ProtoBuf.SubItemToken a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.EndSubItem(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteString_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteString(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteUInt64_s(IntPtr l) {
		try {
			System.UInt64 a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteUInt64(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteInt64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteInt64(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteUInt32_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteUInt32(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteInt16_s(IntPtr l) {
		try {
			System.Int16 a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteInt16(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteUInt16_s(IntPtr l) {
		try {
			System.UInt16 a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteUInt16(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteByte_s(IntPtr l) {
		try {
			System.Byte a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteByte(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteSByte_s(IntPtr l) {
		try {
			System.SByte a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteSByte(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteInt32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteInt32(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteDouble_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteDouble(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteSingle_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteSingle(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ThrowEnumException_s(IntPtr l) {
		try {
			ProtoBuf.ProtoWriter a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.ThrowEnumException(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteBoolean_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteBoolean(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AppendExtensionData_s(IntPtr l) {
		try {
			ProtoBuf.IExtensible a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.AppendExtensionData(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetPackedField_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.SetPackedField(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteType_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoWriter a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoWriter.WriteType(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Context(IntPtr l) {
		try {
			ProtoBuf.ProtoWriter self=(ProtoBuf.ProtoWriter)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Context);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Model(IntPtr l) {
		try {
			ProtoBuf.ProtoWriter self=(ProtoBuf.ProtoWriter)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Model);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoWriter");
		addMember(l,Close);
		addMember(l,SetRootObject);
		addMember(l,WriteObject_s);
		addMember(l,WriteRecursionSafeObject_s);
		addMember(l,WriteFieldHeader_s);
		addMember(l,WriteBytes_s);
		addMember(l,StartSubItem_s);
		addMember(l,EndSubItem_s);
		addMember(l,WriteString_s);
		addMember(l,WriteUInt64_s);
		addMember(l,WriteInt64_s);
		addMember(l,WriteUInt32_s);
		addMember(l,WriteInt16_s);
		addMember(l,WriteUInt16_s);
		addMember(l,WriteByte_s);
		addMember(l,WriteSByte_s);
		addMember(l,WriteInt32_s);
		addMember(l,WriteDouble_s);
		addMember(l,WriteSingle_s);
		addMember(l,ThrowEnumException_s);
		addMember(l,WriteBoolean_s);
		addMember(l,AppendExtensionData_s);
		addMember(l,SetPackedField_s);
		addMember(l,WriteType_s);
		addMember(l,"Context",get_Context,null,true);
		addMember(l,"Model",get_Model,null,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoWriter));
	}
}
