using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SceneLogicInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo o;
			System.Int32 a1;
			checkType(l,2,out a1);
			o=new GameFramework.SceneLogicInfo(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetId(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			var ret=self.GetId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitId(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.InitId(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DataId(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DataId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DataId(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.DataId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ConfigId(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ConfigId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LogicId(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LogicId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneLogicConfig(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneLogicConfig);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneLogicConfig(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			GameFramework.SceneLogicConfig v;
			checkType(l,2,out v);
			self.SceneLogicConfig=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsLogicFinished(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsLogicFinished);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsLogicFinished(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsLogicFinished=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsLogicPaused(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsLogicPaused);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsLogicPaused(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsLogicPaused=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneContext(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneContext);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneContext(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			GameFramework.SceneContextInfo v;
			checkType(l,2,out v);
			self.SceneContext=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneLogicInfoManager(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneLogicInfoManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EntityManager(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EntityManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_BlackBoard(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BlackBoard);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Time(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Time);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Time(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.Time=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LogicDatas(IntPtr l) {
		try {
			GameFramework.SceneLogicInfo self=(GameFramework.SceneLogicInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LogicDatas);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SceneLogicInfo");
		addMember(l,GetId);
		addMember(l,InitId);
		addMember(l,Reset);
		addMember(l,"DataId",get_DataId,set_DataId,true);
		addMember(l,"ConfigId",get_ConfigId,null,true);
		addMember(l,"LogicId",get_LogicId,null,true);
		addMember(l,"SceneLogicConfig",get_SceneLogicConfig,set_SceneLogicConfig,true);
		addMember(l,"IsLogicFinished",get_IsLogicFinished,set_IsLogicFinished,true);
		addMember(l,"IsLogicPaused",get_IsLogicPaused,set_IsLogicPaused,true);
		addMember(l,"SceneContext",get_SceneContext,set_SceneContext,true);
		addMember(l,"SceneLogicInfoManager",get_SceneLogicInfoManager,null,true);
		addMember(l,"EntityManager",get_EntityManager,null,true);
		addMember(l,"BlackBoard",get_BlackBoard,null,true);
		addMember(l,"Time",get_Time,set_Time,true);
		addMember(l,"LogicDatas",get_LogicDatas,null,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.SceneLogicInfo));
	}
}
