﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_PublishSubscribeSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.PublishSubscribeSystem o;
			o=new GameFramework.PublishSubscribeSystem();
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
	static public int Subscribe(IntPtr l) {
		try {
			GameFramework.PublishSubscribeSystem self=(GameFramework.PublishSubscribeSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			GameFramework.MyAction a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			var ret=self.Subscribe(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Unsubscribe(IntPtr l) {
		try {
			GameFramework.PublishSubscribeSystem self=(GameFramework.PublishSubscribeSystem)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.Unsubscribe(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Publish(IntPtr l) {
		try {
			GameFramework.PublishSubscribeSystem self=(GameFramework.PublishSubscribeSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Object[] a3;
			checkParams(l,4,out a3);
			self.Publish(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.PublishSubscribeSystem");
		addMember(l,ctor_s);
		addMember(l,Subscribe);
		addMember(l,Unsubscribe);
		addMember(l,Publish);
		createTypeMetatable(l,null, typeof(GameFramework.PublishSubscribeSystem));
	}
}
