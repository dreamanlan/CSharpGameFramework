using System;

using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_Level : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.Level o;
			o=new TableConfig.Level();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReadFromBinary(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.ReadFromBinary(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteToBinary(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			self.WriteToBinary(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetId(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
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
	static public int get_id(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_id(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_prefab(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.prefab);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_prefab(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.prefab=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_type(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_type(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneDslFile(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneDslFile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneDslFile(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Collections.Generic.List<System.String> v;
			checkType(l,2,out v);
			self.SceneDslFile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ClientDslFile(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ClientDslFile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ClientDslFile(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Collections.Generic.List<System.String> v;
			checkType(l,2,out v);
			self.ClientDslFile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RoomDslFile(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RoomDslFile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RoomDslFile(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Collections.Generic.List<System.String> v;
			checkType(l,2,out v);
			self.RoomDslFile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneUi(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneUi);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneUi(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Collections.Generic.List<System.Int32> v;
			checkType(l,2,out v);
			self.SceneUi=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EnterX(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EnterX);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_EnterX(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.EnterX=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EnterY(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EnterY);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_EnterY(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.EnterY=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EnterRadius(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EnterRadius);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_EnterRadius(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.EnterRadius=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RoomServer(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RoomServer);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RoomServer(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Collections.Generic.List<System.String> v;
			checkType(l,2,out v);
			self.RoomServer=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ThreadCountPerScene(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ThreadCountPerScene);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ThreadCountPerScene(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.ThreadCountPerScene=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RoomCountPerThread(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RoomCountPerThread);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RoomCountPerThread(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.RoomCountPerThread=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MaxUserCount(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MaxUserCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_MaxUserCount(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.MaxUserCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CanPK(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CanPK);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CanPK(IntPtr l) {
		try {
			TableConfig.Level self=(TableConfig.Level)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.CanPK=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.Level");
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,GetId);
		addMember(l,"id",get_id,set_id,true);
		addMember(l,"prefab",get_prefab,set_prefab,true);
		addMember(l,"type",get_type,set_type,true);
		addMember(l,"SceneDslFile",get_SceneDslFile,set_SceneDslFile,true);
		addMember(l,"ClientDslFile",get_ClientDslFile,set_ClientDslFile,true);
		addMember(l,"RoomDslFile",get_RoomDslFile,set_RoomDslFile,true);
		addMember(l,"SceneUi",get_SceneUi,set_SceneUi,true);
		addMember(l,"EnterX",get_EnterX,set_EnterX,true);
		addMember(l,"EnterY",get_EnterY,set_EnterY,true);
		addMember(l,"EnterRadius",get_EnterRadius,set_EnterRadius,true);
		addMember(l,"RoomServer",get_RoomServer,set_RoomServer,true);
		addMember(l,"ThreadCountPerScene",get_ThreadCountPerScene,set_ThreadCountPerScene,true);
		addMember(l,"RoomCountPerThread",get_RoomCountPerThread,set_RoomCountPerThread,true);
		addMember(l,"MaxUserCount",get_MaxUserCount,set_MaxUserCount,true);
		addMember(l,"CanPK",get_CanPK,set_CanPK,true);
		createTypeMetatable(l,constructor, typeof(TableConfig.Level));
	}
}
