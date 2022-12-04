using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_MailItemForMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.MailItemForMessage o;
			o=new GameFrameworkMessage.MailItemForMessage();
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
	static public int get_m_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.MailItemForMessage self=(GameFrameworkMessage.MailItemForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ItemId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.MailItemForMessage self=(GameFrameworkMessage.MailItemForMessage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_ItemId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_ItemNum(IntPtr l) {
		try {
			GameFrameworkMessage.MailItemForMessage self=(GameFrameworkMessage.MailItemForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ItemNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_ItemNum(IntPtr l) {
		try {
			GameFrameworkMessage.MailItemForMessage self=(GameFrameworkMessage.MailItemForMessage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_ItemNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.MailItemForMessage");
		addMember(l,ctor_s);
		addMember(l,"m_ItemId",get_m_ItemId,set_m_ItemId,true);
		addMember(l,"m_ItemNum",get_m_ItemNum,set_m_ItemNum,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.MailItemForMessage));
	}
}
