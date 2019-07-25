using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Network_Serialize : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Encode_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Network.Serialize.Encode(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Decode_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			System.Int32 a2;
			var ret=GameFramework.Network.Serialize.Decode(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Network.Serialize");
		addMember(l,Encode_s);
		addMember(l,Decode_s);
		createTypeMetatable(l,null, typeof(GameFramework.Network.Serialize));
	}
}
