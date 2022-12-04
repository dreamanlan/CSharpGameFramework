using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_ItemDataMsg : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg o;
			o=new GameFrameworkMessage.ItemDataMsg();
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
	static public int get_Guid(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Guid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Guid(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.Guid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ItemId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ItemId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Level(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Level(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Experience(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Experience);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Experience(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Experience=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Num(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Num);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Num(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Num=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AppendProperty(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AppendProperty);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AppendProperty(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.AppendProperty=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EnhanceStarLevel(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EnhanceStarLevel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_EnhanceStarLevel(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.EnhanceStarLevel=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StrengthLevel(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StrengthLevel);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StrengthLevel(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.StrengthLevel=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StrengthFailCount(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StrengthFailCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StrengthFailCount(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.StrengthFailCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsCanTrade(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsCanTrade);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsCanTrade(IntPtr l) {
		try {
			GameFrameworkMessage.ItemDataMsg self=(GameFrameworkMessage.ItemDataMsg)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsCanTrade=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.ItemDataMsg");
		addMember(l,ctor_s);
		addMember(l,"Guid",get_Guid,set_Guid,true);
		addMember(l,"ItemId",get_ItemId,set_ItemId,true);
		addMember(l,"Level",get_Level,set_Level,true);
		addMember(l,"Experience",get_Experience,set_Experience,true);
		addMember(l,"Num",get_Num,set_Num,true);
		addMember(l,"AppendProperty",get_AppendProperty,set_AppendProperty,true);
		addMember(l,"EnhanceStarLevel",get_EnhanceStarLevel,set_EnhanceStarLevel,true);
		addMember(l,"StrengthLevel",get_StrengthLevel,set_StrengthLevel,true);
		addMember(l,"StrengthFailCount",get_StrengthFailCount,set_StrengthFailCount,true);
		addMember(l,"IsCanTrade",get_IsCanTrade,set_IsCanTrade,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.ItemDataMsg));
	}
}
