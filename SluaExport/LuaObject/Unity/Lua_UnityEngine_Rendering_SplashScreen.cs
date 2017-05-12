using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Rendering_SplashScreen : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.Rendering.SplashScreen o;
			o=new UnityEngine.Rendering.SplashScreen();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Begin_s(IntPtr l) {
		try {
			UnityEngine.Rendering.SplashScreen.Begin();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Draw_s(IntPtr l) {
		try {
			UnityEngine.Rendering.SplashScreen.Draw();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isFinished(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UnityEngine.Rendering.SplashScreen.isFinished);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Rendering.SplashScreen");
		addMember(l,Begin_s);
		addMember(l,Draw_s);
		addMember(l,"isFinished",get_isFinished,null,false);
		createTypeMetatable(l,constructor, typeof(UnityEngine.Rendering.SplashScreen));
	}
}
