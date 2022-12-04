using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_ServiceModel_ProtoOperationBehavior : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.ProtoOperationBehavior o;
			System.ServiceModel.Description.OperationDescription a1;
			checkType(l,1,out a1);
			o=new ProtoBuf.ServiceModel.ProtoOperationBehavior(a1);
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
	static public int get_Model(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.ProtoOperationBehavior self=(ProtoBuf.ServiceModel.ProtoOperationBehavior)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Model);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Model(IntPtr l) {
		try {
			ProtoBuf.ServiceModel.ProtoOperationBehavior self=(ProtoBuf.ServiceModel.ProtoOperationBehavior)checkSelf(l);
			ProtoBuf.Meta.TypeModel v;
			checkType(l,2,out v);
			self.Model=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ServiceModel.ProtoOperationBehavior");
		addMember(l,ctor_s);
		addMember(l,"Model",get_Model,set_Model,true);
		createTypeMetatable(l,null, typeof(ProtoBuf.ServiceModel.ProtoOperationBehavior),typeof(System.ServiceModel.Description.DataContractSerializerOperationBehavior));
	}
}
