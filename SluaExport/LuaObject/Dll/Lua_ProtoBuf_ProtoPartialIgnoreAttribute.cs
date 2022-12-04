using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ProtoPartialIgnoreAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.ProtoPartialIgnoreAttribute o;
			System.String a1;
			checkType(l,1,out a1);
			o=new ProtoBuf.ProtoPartialIgnoreAttribute(a1);
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
			ProtoBuf.ProtoPartialIgnoreAttribute self=(ProtoBuf.ProtoPartialIgnoreAttribute)checkSelf(l);
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
		getTypeTable(l,"ProtoBuf.ProtoPartialIgnoreAttribute");
		addMember(l,ctor_s);
		addMember(l,"MemberName",get_MemberName,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.ProtoPartialIgnoreAttribute),typeof(ProtoBuf.ProtoIgnoreAttribute));
	}
}
