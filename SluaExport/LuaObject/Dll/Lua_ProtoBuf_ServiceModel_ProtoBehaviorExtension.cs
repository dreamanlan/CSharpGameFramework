using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ServiceModel_ProtoBehaviorExtension : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.ProtoBehaviorExtension o;
			o=new ProtoBuf.ServiceModel.ProtoBehaviorExtension();
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
	static public int get_BehaviorType(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.ProtoBehaviorExtension self=(ProtoBuf.ServiceModel.ProtoBehaviorExtension)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BehaviorType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ServiceModel.ProtoBehaviorExtension");
		addMember(l,ctor_s);
		addMember(l,"BehaviorType",get_BehaviorType,null,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.ServiceModel.ProtoBehaviorExtension),typeof(System.ServiceModel.Configuration.BehaviorExtensionElement));
	}
}
