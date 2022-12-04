using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_KdObjectTree : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.KdObjectTree o;
			o=new GameFramework.KdObjectTree();
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
	static public int Clear(IntPtr l) {
		try {
			GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int BeginBuild(IntPtr l) {
		try {
			GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.BeginBuild(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddObjForBuild(IntPtr l) {
		try {
			GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			self.AddObjForBuild(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int EndBuild(IntPtr l) {
		try {
			GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
			self.EndBuild();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_MaxLeafSize(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.KdObjectTree.c_MaxLeafSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.KdObjectTree");
		addMember(l,ctor_s);
		addMember(l,Clear);
		addMember(l,BeginBuild);
		addMember(l,AddObjForBuild);
		addMember(l,EndBuild);
		addMember(l,"c_MaxLeafSize",get_c_MaxLeafSize,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.KdObjectTree));
	}
}
