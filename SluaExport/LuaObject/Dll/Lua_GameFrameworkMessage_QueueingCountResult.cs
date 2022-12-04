using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_QueueingCountResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.QueueingCountResult o;
			o=new GameFrameworkMessage.QueueingCountResult();
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
	static public int get_m_QueueingCount(IntPtr l) {
		try {
			GameFrameworkMessage.QueueingCountResult self=(GameFrameworkMessage.QueueingCountResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_QueueingCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_QueueingCount(IntPtr l) {
		try {
			GameFrameworkMessage.QueueingCountResult self=(GameFrameworkMessage.QueueingCountResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_QueueingCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.QueueingCountResult");
		addMember(l,ctor_s);
		addMember(l,"m_QueueingCount",get_m_QueueingCount,set_m_QueueingCount,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.QueueingCountResult));
	}
}
