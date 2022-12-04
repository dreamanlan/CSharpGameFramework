using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_System_Collections_Generic_LinkedListNode_1_GameFramework_EntityInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			System.Collections.Generic.LinkedListNode<GameFramework.EntityInfo> o;
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			o=new System.Collections.Generic.LinkedListNode<GameFramework.EntityInfo>(a1);
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
	static public int get_Value(IntPtr l) {
		try {
			System.Collections.Generic.LinkedListNode<GameFramework.EntityInfo> self=(System.Collections.Generic.LinkedListNode<GameFramework.EntityInfo>)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Value(IntPtr l) {
		try {
			System.Collections.Generic.LinkedListNode<GameFramework.EntityInfo> self=(System.Collections.Generic.LinkedListNode<GameFramework.EntityInfo>)checkSelf(l);
			GameFramework.EntityInfo v;
			checkType(l,2,out v);
			self.Value=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"LinkedListNodeEntityInfo");
		addMember(l,ctor_s);
		addMember(l,"Value",get_Value,set_Value,true);
		createTypeMetatable(l,null, typeof(System.Collections.Generic.LinkedListNode<GameFramework.EntityInfo>));
	}
}
