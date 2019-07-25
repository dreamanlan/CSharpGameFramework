using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoPartialMemberAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoPartialMemberAttribute o;
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			o=new ProtoBuf.ProtoPartialMemberAttribute(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MemberName(IntPtr l) {
		try {
			ProtoBuf.ProtoPartialMemberAttribute self=(ProtoBuf.ProtoPartialMemberAttribute)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MemberName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoPartialMemberAttribute");
		addMember(l,"MemberName",get_MemberName,null,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoPartialMemberAttribute),typeof(ProtoBuf.ProtoMemberAttribute));
	}
}
