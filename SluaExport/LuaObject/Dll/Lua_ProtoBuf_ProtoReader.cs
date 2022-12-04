using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ProtoReader : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Dispose(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			self.Dispose();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadUInt32(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadUInt32();
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
	static public int ReadInt16(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadInt16();
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
	static public int ReadUInt16(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadUInt16();
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
	static public int ReadByte(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadByte();
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
	static public int ReadSByte(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadSByte();
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
	static public int ReadInt32(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadInt32();
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
	static public int ReadInt64(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadInt64();
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
	static public int ReadString(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadString();
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
	static public int ThrowEnumException(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.ThrowEnumException(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadDouble(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadDouble();
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
	static public int ReadFieldHeader(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadFieldHeader();
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
	static public int TryReadFieldHeader(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.TryReadFieldHeader(a1);
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
	static public int Hint(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			ProtoBuf.WireType a1;
			checkEnum(l,2,out a1);
			self.Hint(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Assert(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			ProtoBuf.WireType a1;
			checkEnum(l,2,out a1);
			self.Assert(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SkipField(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			self.SkipField();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadUInt64(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadUInt64();
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
	static public int ReadSingle(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadSingle();
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
	static public int ReadBoolean(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadBoolean();
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
	static public int AppendExtensionData(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			ProtoBuf.IExtensible a1;
			checkType(l,2,out a1);
			self.AppendExtensionData(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadType(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			var ret=self.ReadType();
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
	static public int ReadObject_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoReader a3;
			checkType(l,3,out a3);
			var ret=ProtoBuf.ProtoReader.ReadObject(a1,a2,a3);
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
	static public int EndSubItem_s(IntPtr l) {
		try {
			ProtoBuf.SubItemToken a1;
			checkValueType(l,1,out a1);
			ProtoBuf.ProtoReader a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoReader.EndSubItem(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StartSubItem_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.ProtoReader.StartSubItem(a1);
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
	static public int AppendBytes_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			ProtoBuf.ProtoReader a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.ProtoReader.AppendBytes(a1,a2);
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
	static public int ReadLengthPrefix__Stream__Boolean__PrefixStyle__O_Int32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			ProtoBuf.PrefixStyle a3;
			checkEnum(l,3,out a3);
			System.Int32 a4;
			var ret=ProtoBuf.ProtoReader.ReadLengthPrefix(a1,a2,a3,out a4);
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
	static public int ReadLengthPrefix__Stream__Boolean__PrefixStyle__O_Int32__O_Int32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			ProtoBuf.PrefixStyle a3;
			checkEnum(l,3,out a3);
			System.Int32 a4;
			System.Int32 a5;
			var ret=ProtoBuf.ProtoReader.ReadLengthPrefix(a1,a2,a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DirectReadLittleEndianInt32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.ProtoReader.DirectReadLittleEndianInt32(a1);
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
	static public int DirectReadBigEndianInt32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.ProtoReader.DirectReadBigEndianInt32(a1);
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
	static public int DirectReadVarintInt32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			var ret=ProtoBuf.ProtoReader.DirectReadVarintInt32(a1);
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
	static public int DirectReadBytes__Stream__Int32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.ProtoReader.DirectReadBytes(a1,a2);
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
	static public int DirectReadBytes__Stream__A_Byte__Int32__Int32_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Byte[] a2;
			checkArray(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			ProtoBuf.ProtoReader.DirectReadBytes(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DirectReadString_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.ProtoReader.DirectReadString(a1,a2);
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
	static public int ReadLongLengthPrefix_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			ProtoBuf.PrefixStyle a3;
			checkEnum(l,3,out a3);
			System.Int32 a4;
			System.Int32 a5;
			var ret=ProtoBuf.ProtoReader.ReadLongLengthPrefix(a1,a2,a3,out a4,out a5);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			pushValue(l,a5);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int HasSubValue_s(IntPtr l) {
		try {
			ProtoBuf.WireType a1;
			checkEnum(l,1,out a1);
			ProtoBuf.ProtoReader a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.ProtoReader.HasSubValue(a1,a2);
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
	static public int NoteObject_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			ProtoBuf.ProtoReader a2;
			checkType(l,2,out a2);
			ProtoBuf.ProtoReader.NoteObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Merge_s(IntPtr l) {
		try {
			ProtoBuf.ProtoReader a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.Object a3;
			checkType(l,3,out a3);
			var ret=ProtoBuf.ProtoReader.Merge(a1,a2,a3);
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
	static public int Create_s(IntPtr l) {
		try {
			System.IO.Stream a1;
			checkType(l,1,out a1);
			ProtoBuf.Meta.TypeModel a2;
			checkType(l,2,out a2);
			ProtoBuf.SerializationContext a3;
			checkType(l,3,out a3);
			System.Int64 a4;
			checkType(l,4,out a4);
			var ret=ProtoBuf.ProtoReader.Create(a1,a2,a3,a4);
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
	static public int get_FieldNumber(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FieldNumber);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_WireType(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.WireType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_InternStrings(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.InternStrings);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_InternStrings(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.InternStrings=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Context(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
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
	static public int get_Position(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Position);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LongPosition(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LongPosition);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Model(IntPtr l) {
		try {
			ProtoBuf.ProtoReader self=(ProtoBuf.ProtoReader)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Model);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoReader");
		addMember(l,Dispose);
		addMember(l,ReadUInt32);
		addMember(l,ReadInt16);
		addMember(l,ReadUInt16);
		addMember(l,ReadByte);
		addMember(l,ReadSByte);
		addMember(l,ReadInt32);
		addMember(l,ReadInt64);
		addMember(l,ReadString);
		addMember(l,ThrowEnumException);
		addMember(l,ReadDouble);
		addMember(l,ReadFieldHeader);
		addMember(l,TryReadFieldHeader);
		addMember(l,Hint);
		addMember(l,Assert);
		addMember(l,SkipField);
		addMember(l,ReadUInt64);
		addMember(l,ReadSingle);
		addMember(l,ReadBoolean);
		addMember(l,AppendExtensionData);
		addMember(l,ReadType);
		addMember(l,ReadObject_s);
		addMember(l,EndSubItem_s);
		addMember(l,StartSubItem_s);
		addMember(l,AppendBytes_s);
		addMember(l,ReadLengthPrefix__Stream__Boolean__PrefixStyle__O_Int32_s);
		addMember(l,ReadLengthPrefix__Stream__Boolean__PrefixStyle__O_Int32__O_Int32_s);
		addMember(l,DirectReadLittleEndianInt32_s);
		addMember(l,DirectReadBigEndianInt32_s);
		addMember(l,DirectReadVarintInt32_s);
		addMember(l,DirectReadBytes__Stream__Int32_s);
		addMember(l,DirectReadBytes__Stream__A_Byte__Int32__Int32_s);
		addMember(l,DirectReadString_s);
		addMember(l,ReadLongLengthPrefix_s);
		addMember(l,HasSubValue_s);
		addMember(l,NoteObject_s);
		addMember(l,Merge_s);
		addMember(l,Create_s);
		addMember(l,"FieldNumber",get_FieldNumber,null,true);
		addMember(l,"WireType",get_WireType,null,true);
		addMember(l,"InternStrings",get_InternStrings,set_InternStrings,true);
		addMember(l,"Context",get_Context,null,true);
		addMember(l,"Position",get_Position,null,true);
		addMember(l,"LongPosition",get_LongPosition,null,true);
		addMember(l,"Model",get_Model,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.ProtoReader));
	}
}
