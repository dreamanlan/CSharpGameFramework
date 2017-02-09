using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_Meta_TypeModel : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Serialize(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ProtoBuf.ProtoWriter),typeof(System.Object))){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				ProtoBuf.ProtoWriter a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				self.Serialize(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(System.IO.Stream),typeof(System.Object))){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				self.Serialize(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				ProtoBuf.SerializationContext a3;
				checkType(l,4,out a3);
				self.Serialize(a1,a2,a3);
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
	static public int DeserializeWithLengthPrefix(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==6){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				ProtoBuf.PrefixStyle a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				var ret=self.DeserializeWithLengthPrefix(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				ProtoBuf.PrefixStyle a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				ProtoBuf.Serializer.TypeResolver a6;
				LuaDelegation.checkDelegate(l,7,out a6);
				var ret=self.DeserializeWithLengthPrefix(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==8){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				ProtoBuf.PrefixStyle a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				ProtoBuf.Serializer.TypeResolver a6;
				LuaDelegation.checkDelegate(l,7,out a6);
				System.Int32 a7;
				var ret=self.DeserializeWithLengthPrefix(a1,a2,a3,a4,a5,a6,out a7);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a7);
				return 3;
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
	static public int DeserializeItems(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==6){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Type a2;
				checkType(l,3,out a2);
				ProtoBuf.PrefixStyle a3;
				checkEnum(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				ProtoBuf.Serializer.TypeResolver a5;
				LuaDelegation.checkDelegate(l,6,out a5);
				var ret=self.DeserializeItems(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Type a2;
				checkType(l,3,out a2);
				ProtoBuf.PrefixStyle a3;
				checkEnum(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				ProtoBuf.Serializer.TypeResolver a5;
				LuaDelegation.checkDelegate(l,6,out a5);
				ProtoBuf.SerializationContext a6;
				checkType(l,7,out a6);
				var ret=self.DeserializeItems(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int SerializeWithLengthPrefix(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==6){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				ProtoBuf.PrefixStyle a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				self.SerializeWithLengthPrefix(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(argc==7){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				ProtoBuf.PrefixStyle a4;
				checkEnum(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				ProtoBuf.SerializationContext a6;
				checkType(l,7,out a6);
				self.SerializeWithLengthPrefix(a1,a2,a3,a4,a5,a6);
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
	static public int Deserialize(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ProtoBuf.ProtoReader),typeof(System.Object),typeof(System.Type))){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				ProtoBuf.ProtoReader a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				var ret=self.Deserialize(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.IO.Stream),typeof(System.Object),typeof(System.Type))){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				var ret=self.Deserialize(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.IO.Stream),typeof(System.Object),typeof(System.Type),typeof(int))){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				var ret=self.Deserialize(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.IO.Stream),typeof(System.Object),typeof(System.Type),typeof(ProtoBuf.SerializationContext))){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				ProtoBuf.SerializationContext a4;
				checkType(l,5,out a4);
				var ret=self.Deserialize(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
				System.IO.Stream a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				System.Type a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				ProtoBuf.SerializationContext a5;
				checkType(l,6,out a5);
				var ret=self.Deserialize(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int IsDefined(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.IsDefined(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DeepClone(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.DeepClone(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CanSerializeContractType(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.CanSerializeContractType(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CanSerialize(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.CanSerialize(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CanSerializeBasicType(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.CanSerializeBasicType(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSchema(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.GetSchema(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ThrowCannotCreateInstance_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			ProtoBuf.Meta.TypeModel.ThrowCannotCreateInstance(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ForwardsOnly(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ForwardsOnly);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ForwardsOnly(IntPtr l) {
		try {
			ProtoBuf.Meta.TypeModel self=(ProtoBuf.Meta.TypeModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.ForwardsOnly=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.TypeModel");
		addMember(l,Serialize);
		addMember(l,DeserializeWithLengthPrefix);
		addMember(l,DeserializeItems);
		addMember(l,SerializeWithLengthPrefix);
		addMember(l,Deserialize);
		addMember(l,IsDefined);
		addMember(l,DeepClone);
		addMember(l,CanSerializeContractType);
		addMember(l,CanSerialize);
		addMember(l,CanSerializeBasicType);
		addMember(l,GetSchema);
		addMember(l,ThrowCannotCreateInstance_s);
		addMember(l,"ForwardsOnly",get_ForwardsOnly,set_ForwardsOnly,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.TypeModel));
	}
}
