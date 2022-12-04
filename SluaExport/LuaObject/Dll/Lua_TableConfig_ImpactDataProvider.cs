using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TableConfig_ImpactDataProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider o;
			o=new TableConfig.ImpactDataProvider();
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
	static public int LoadForClient(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider self=(TableConfig.ImpactDataProvider)checkSelf(l);
			self.LoadForClient();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadForServer(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider self=(TableConfig.ImpactDataProvider)checkSelf(l);
			self.LoadForServer();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Load(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider self=(TableConfig.ImpactDataProvider)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Load(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Save(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider self=(TableConfig.ImpactDataProvider)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Save(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clear(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider self=(TableConfig.ImpactDataProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetImpactDataCount(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider self=(TableConfig.ImpactDataProvider)checkSelf(l);
			var ret=self.GetImpactDataCount();
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
	static public int GetImpactData(IntPtr l) {
		try {
			TableConfig.ImpactDataProvider self=(TableConfig.ImpactDataProvider)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetImpactData(a1);
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
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,TableConfig.ImpactDataProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.ImpactDataProvider");
		addMember(l,ctor_s);
		addMember(l,LoadForClient);
		addMember(l,LoadForServer);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetImpactDataCount);
		addMember(l,GetImpactData);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(TableConfig.ImpactDataProvider));
	}
}
