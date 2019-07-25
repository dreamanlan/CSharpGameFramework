using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SceneContextInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.SceneContextInfo o;
			o=new GameFramework.SceneContextInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityById(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityById(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityByUnitId(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityByUnitId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HighlightPromptAll(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.HighlightPromptAll(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int HighlightPrompt(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Object[] a3;
			checkParams(l,4,out a3);
			self.HighlightPrompt(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ResetUniqueId(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			self.ResetUniqueId();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GenUniqueId(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			var ret=self.GenUniqueId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ObjectSet(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Object a3;
			checkType(l,4,out a3);
			self.ObjectSet(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ObjectTryGet(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Object a3;
			var ret=self.ObjectTryGet(a1,a2,out a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OnHighlightPrompt(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			GameFramework.HighlightPromptDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnHighlightPrompt=v;
			else if(op==1) self.OnHighlightPrompt+=v;
			else if(op==2) self.OnHighlightPrompt-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_KdTree(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KdTree);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_KdTree(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			GameFramework.KdObjectTree v;
			checkType(l,2,out v);
			self.KdTree=v;
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
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneLogicInfoManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneLogicInfoManager(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			GameFramework.SceneLogicInfoManager v;
			checkType(l,2,out v);
			self.SceneLogicInfoManager=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EntityManager(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EntityManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_EntityManager(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			GameFramework.EntityManager v;
			checkType(l,2,out v);
			self.EntityManager=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_BlackBoard(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BlackBoard);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_BlackBoard(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			GameFramework.BlackBoard v;
			checkType(l,2,out v);
			self.BlackBoard=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CommandInfos(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CommandInfos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneResId(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneResId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneResId(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.SceneResId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsRunWithRoomServer(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsRunWithRoomServer);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsRunWithRoomServer(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsRunWithRoomServer=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StartTime(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StartTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_StartTime(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.StartTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CustomData(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CustomData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CustomData(IntPtr l) {
		try {
			GameFramework.SceneContextInfo self=(GameFramework.SceneContextInfo)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.CustomData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SceneContextInfo");
		addMember(l,GetEntityById);
		addMember(l,GetEntityByUnitId);
		addMember(l,HighlightPromptAll);
		addMember(l,HighlightPrompt);
		addMember(l,ResetUniqueId);
		addMember(l,GenUniqueId);
		addMember(l,ObjectSet);
		addMember(l,ObjectTryGet);
		addMember(l,"OnHighlightPrompt",null,set_OnHighlightPrompt,true);
		addMember(l,"KdTree",get_KdTree,set_KdTree,true);
		addMember(l,"SceneLogicInfoManager",get_SceneLogicInfoManager,set_SceneLogicInfoManager,true);
		addMember(l,"EntityManager",get_EntityManager,set_EntityManager,true);
		addMember(l,"BlackBoard",get_BlackBoard,set_BlackBoard,true);
		addMember(l,"CommandInfos",get_CommandInfos,null,true);
		addMember(l,"SceneResId",get_SceneResId,set_SceneResId,true);
		addMember(l,"IsRunWithRoomServer",get_IsRunWithRoomServer,set_IsRunWithRoomServer,true);
		addMember(l,"StartTime",get_StartTime,set_StartTime,true);
		addMember(l,"CustomData",get_CustomData,set_CustomData,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.SceneContextInfo));
	}
}
