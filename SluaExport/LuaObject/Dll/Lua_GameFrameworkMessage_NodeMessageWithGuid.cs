using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_NodeMessageWithGuid : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.NodeMessageWithGuid o;
			o=new GameFrameworkMessage.NodeMessageWithGuid();
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
	static public int get_m_Guid(IntPtr l) {
		try {
			GameFrameworkMessage.NodeMessageWithGuid self=(GameFrameworkMessage.NodeMessageWithGuid)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Guid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Guid(IntPtr l) {
		try {
			GameFrameworkMessage.NodeMessageWithGuid self=(GameFrameworkMessage.NodeMessageWithGuid)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.m_Guid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.NodeMessageWithGuid");
		addMember(l,ctor_s);
		addMember(l,"m_Guid",get_m_Guid,set_m_Guid,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.NodeMessageWithGuid));
	}
}
