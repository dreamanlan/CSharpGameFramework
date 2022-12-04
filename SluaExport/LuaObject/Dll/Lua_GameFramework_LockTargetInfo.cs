using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_LockTargetInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.LockTargetInfo o;
			o=new GameFramework.LockTargetInfo();
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
	static public int get_TargetId(IntPtr l) {
		try {
			GameFramework.LockTargetInfo self=(GameFramework.LockTargetInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TargetId(IntPtr l) {
		try {
			GameFramework.LockTargetInfo self=(GameFramework.LockTargetInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.TargetId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Target(IntPtr l) {
		try {
			GameFramework.LockTargetInfo self=(GameFramework.LockTargetInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Target);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Target(IntPtr l) {
		try {
			GameFramework.LockTargetInfo self=(GameFramework.LockTargetInfo)checkSelf(l);
			GameFramework.EntityInfo v;
			checkType(l,2,out v);
			self.Target=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.LockTargetInfo");
		addMember(l,ctor_s);
		addMember(l,"TargetId",get_TargetId,set_TargetId,true);
		addMember(l,"Target",get_Target,set_Target,true);
		createTypeMetatable(l,null, typeof(GameFramework.LockTargetInfo));
	}
}
