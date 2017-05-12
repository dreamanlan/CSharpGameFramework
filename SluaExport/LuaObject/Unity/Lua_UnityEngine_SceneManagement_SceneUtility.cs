using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_SceneManagement_SceneUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetScenePathByBuildIndex_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetBuildIndexByScenePath_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.SceneManagement.SceneUtility.GetBuildIndexByScenePath(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.SceneManagement.SceneUtility");
		addMember(l,GetScenePathByBuildIndex_s);
		addMember(l,GetBuildIndexByScenePath_s);
		createTypeMetatable(l,null, typeof(UnityEngine.SceneManagement.SceneUtility));
	}
}
