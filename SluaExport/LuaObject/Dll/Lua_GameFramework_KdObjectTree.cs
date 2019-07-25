using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_KdObjectTree : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int FullBuild(IntPtr l) {
		try {
			GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
			System.Collections.Generic.IList<GameFramework.EntityInfo> a1;
			checkType(l,2,out a1);
			self.FullBuild(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int QueryWithAction(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3),typeof(float),typeof(GameFramework.MyAction<System.Single,GameFramework.KdTreeObject>))){
				GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				GameFramework.MyAction<System.Single,GameFramework.KdTreeObject> a3;
				LuaDelegation.checkDelegate(l,4,out a3);
				self.QueryWithAction(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(GameFramework.EntityInfo),typeof(float),typeof(GameFramework.MyAction<System.Single,GameFramework.KdTreeObject>))){
				GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
				GameFramework.EntityInfo a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				GameFramework.MyAction<System.Single,GameFramework.KdTreeObject> a3;
				LuaDelegation.checkDelegate(l,4,out a3);
				self.QueryWithAction(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				GameFramework.MyAction<System.Single,GameFramework.KdTreeObject> a5;
				LuaDelegation.checkDelegate(l,6,out a5);
				self.QueryWithAction(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int QueryWithFunc(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(ScriptRuntime.Vector3),typeof(float),typeof(GameFramework.MyFunc<System.Single,GameFramework.KdTreeObject,System.Boolean>))){
				GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
				ScriptRuntime.Vector3 a1;
				checkValueType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				GameFramework.MyFunc<System.Single,GameFramework.KdTreeObject,System.Boolean> a3;
				LuaDelegation.checkDelegate(l,4,out a3);
				self.QueryWithFunc(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(GameFramework.EntityInfo),typeof(float),typeof(GameFramework.MyFunc<System.Single,GameFramework.KdTreeObject,System.Boolean>))){
				GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
				GameFramework.EntityInfo a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				GameFramework.MyFunc<System.Single,GameFramework.KdTreeObject,System.Boolean> a3;
				LuaDelegation.checkDelegate(l,4,out a3);
				self.QueryWithFunc(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==6){
				GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Single a3;
				checkType(l,4,out a3);
				System.Single a4;
				checkType(l,5,out a4);
				GameFramework.MyFunc<System.Single,GameFramework.KdTreeObject,System.Boolean> a5;
				LuaDelegation.checkDelegate(l,6,out a5);
				self.QueryWithFunc(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int VisitTreeWithAction(IntPtr l) {
		try {
			GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
			GameFramework.MyAction<System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int32,GameFramework.KdTreeObject[]> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.VisitTreeWithAction(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int VisitTreeWithFunc(IntPtr l) {
		try {
			GameFramework.KdObjectTree self=(GameFramework.KdObjectTree)checkSelf(l);
			GameFramework.MyFunc<System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int32,GameFramework.KdTreeObject[],System.Boolean> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.VisitTreeWithFunc(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.KdObjectTree");
		addMember(l,FullBuild);
		addMember(l,Clear);
		addMember(l,BeginBuild);
		addMember(l,AddObjForBuild);
		addMember(l,EndBuild);
		addMember(l,QueryWithAction);
		addMember(l,QueryWithFunc);
		addMember(l,VisitTreeWithAction);
		addMember(l,VisitTreeWithFunc);
		addMember(l,"c_MaxLeafSize",get_c_MaxLeafSize,null,false);
		createTypeMetatable(l,constructor, typeof(GameFramework.KdObjectTree));
	}
}
