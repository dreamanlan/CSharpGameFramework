using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ServiceModel_XmlProtoSerializer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer o;
			ProtoBuf.Meta.TypeModel a1;
			checkType(l,1,out a1);
			System.Type a2;
			checkType(l,2,out a2);
			o=new ProtoBuf.ServiceModel.XmlProtoSerializer(a1,a2);
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
	static public int WriteEndObject__XmlDictionaryWriter(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlDictionaryWriter a1;
			checkType(l,2,out a1);
			self.WriteEndObject(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int WriteEndObject__XmlWriter(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlWriter a1;
			checkType(l,2,out a1);
			self.WriteEndObject(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int WriteStartObject__XmlDictionaryWriter__Object(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlDictionaryWriter a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.WriteStartObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int WriteStartObject__XmlWriter__Object(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlWriter a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.WriteStartObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int WriteObjectContent__XmlDictionaryWriter__Object(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlDictionaryWriter a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.WriteObjectContent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int WriteObjectContent__XmlWriter__Object(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlWriter a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.WriteObjectContent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsStartObject__XmlDictionaryReader(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlDictionaryReader a1;
			checkType(l,2,out a1);
			var ret=self.IsStartObject(a1);
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
	static public int IsStartObject__XmlReader(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlReader a1;
			checkType(l,2,out a1);
			var ret=self.IsStartObject(a1);
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
	static public int ReadObject__Stream(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.IO.Stream a1;
			checkType(l,2,out a1);
			var ret=self.ReadObject(a1);
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
	static public int ReadObject__XmlReader(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlReader a1;
			checkType(l,2,out a1);
			var ret=self.ReadObject(a1);
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
	static public int ReadObject__XmlDictionaryReader(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlDictionaryReader a1;
			checkType(l,2,out a1);
			var ret=self.ReadObject(a1);
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
	static public int ReadObject__XmlDictionaryReader__Boolean(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlDictionaryReader a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			var ret=self.ReadObject(a1,a2);
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
	static public int ReadObject__XmlReader__Boolean(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.XmlProtoSerializer self=(ProtoBuf.ServiceModel.XmlProtoSerializer)checkSelf(l);
			System.Xml.XmlReader a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			var ret=self.ReadObject(a1,a2);
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
	static public int TryCreate_s(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel a1;
			checkType(l,1,out a1);
			System.Type a2;
			checkType(l,2,out a2);
			var ret=ProtoBuf.ServiceModel.XmlProtoSerializer.TryCreate(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ServiceModel.XmlProtoSerializer");
		addMember(l,ctor_s);
		addMember(l,WriteEndObject__XmlDictionaryWriter);
		addMember(l,WriteEndObject__XmlWriter);
		addMember(l,WriteStartObject__XmlDictionaryWriter__Object);
		addMember(l,WriteStartObject__XmlWriter__Object);
		addMember(l,WriteObjectContent__XmlDictionaryWriter__Object);
		addMember(l,WriteObjectContent__XmlWriter__Object);
		addMember(l,IsStartObject__XmlDictionaryReader);
		addMember(l,IsStartObject__XmlReader);
		addMember(l,ReadObject__Stream);
		addMember(l,ReadObject__XmlReader);
		addMember(l,ReadObject__XmlDictionaryReader);
		addMember(l,ReadObject__XmlDictionaryReader__Boolean);
		addMember(l,ReadObject__XmlReader__Boolean);
		addMember(l,TryCreate_s);
		createTypeMetatable(l,null, typeof(ProtoBuf.ServiceModel.XmlProtoSerializer),typeof(System.Runtime.Serialization.XmlObjectSerializer));
	}
}
