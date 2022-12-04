using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dict : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Parse_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dict.Parse(a1);
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
	static public int Get_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dict.Get(a1);
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
	static public int Format_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Object[] a2;
			checkParams(l,2,out a2);
			var ret=Dict.Format(a1,a2);
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
	static public int set_OnFindDictionary(IntPtr l) {
		try {
			Dict.FindDictionaryDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) Dict.OnFindDictionary=v;
			else if(op==1) Dict.OnFindDictionary+=v;
			else if(op==2) Dict.OnFindDictionary-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dict");
		addMember(l,Parse_s);
		addMember(l,Get_s);
		addMember(l,Format_s);
		addMember(l,"OnFindDictionary",null,set_OnFindDictionary,false);
		createTypeMetatable(l,null, typeof(Dict));
	}
}
