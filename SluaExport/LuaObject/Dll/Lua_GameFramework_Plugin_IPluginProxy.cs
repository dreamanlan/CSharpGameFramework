using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Plugin_IPluginProxy : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterAttrExpression(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RegisterAttrExpression(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterSkillTrigger(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RegisterSkillTrigger(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterStoryCommand(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RegisterStoryCommand(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterStoryValue(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RegisterStoryValue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterSimpleStoryCommand(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RegisterSimpleStoryCommand(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterSimpleStoryValue(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RegisterSimpleStoryValue(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InstallStartupPlugin(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.InstallStartupPlugin(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveStartupPlugin(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RemoveStartupPlugin(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InstallTickPlugin(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.InstallTickPlugin(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveTickPlugin(IntPtr l) {
		try {
			GameFramework.Plugin.IPluginProxy self=(GameFramework.Plugin.IPluginProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RemoveTickPlugin(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.IPluginProxy");
		addMember(l,RegisterAttrExpression);
		addMember(l,RegisterSkillTrigger);
		addMember(l,RegisterStoryCommand);
		addMember(l,RegisterStoryValue);
		addMember(l,RegisterSimpleStoryCommand);
		addMember(l,RegisterSimpleStoryValue);
		addMember(l,InstallStartupPlugin);
		addMember(l,RemoveStartupPlugin);
		addMember(l,InstallTickPlugin);
		addMember(l,RemoveTickPlugin);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.IPluginProxy));
	}
}
