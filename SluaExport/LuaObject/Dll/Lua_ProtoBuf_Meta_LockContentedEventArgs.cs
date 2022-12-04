using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_LockContentedEventArgs : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_OwnerStackTrace(IntPtr l) {
		try {
			ProtoBuf.Meta.LockContentedEventArgs self=(ProtoBuf.Meta.LockContentedEventArgs)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OwnerStackTrace);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Meta.LockContentedEventArgs");
		addMember(l,"OwnerStackTrace",get_OwnerStackTrace,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.Meta.LockContentedEventArgs),typeof(System.EventArgs));
	}
}
