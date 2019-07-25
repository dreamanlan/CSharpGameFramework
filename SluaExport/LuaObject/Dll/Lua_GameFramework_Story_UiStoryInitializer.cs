using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Story_UiStoryInitializer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			GameFramework.Story.UiStoryInitializer self=(GameFramework.Story.UiStoryInitializer)checkSelf(l);
			self.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_WindowName(IntPtr l) {
		try {
			GameFramework.Story.UiStoryInitializer self=(GameFramework.Story.UiStoryInitializer)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.WindowName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_WindowName(IntPtr l) {
		try {
			GameFramework.Story.UiStoryInitializer self=(GameFramework.Story.UiStoryInitializer)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.WindowName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Story.UiStoryInitializer");
		addMember(l,Init);
		addMember(l,"WindowName",get_WindowName,set_WindowName,true);
		createTypeMetatable(l,null, typeof(GameFramework.Story.UiStoryInitializer),typeof(UnityEngine.MonoBehaviour));
	}
}
