using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ProtoPartialMemberAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.ProtoPartialMemberAttribute o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoPartialMemberAttribute");
		addMember(l,ctor_s);
		addMember(l,"MemberName",get_MemberName,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.ProtoPartialMemberAttribute),typeof(ProtoBuf.ProtoMemberAttribute));
	}
}
