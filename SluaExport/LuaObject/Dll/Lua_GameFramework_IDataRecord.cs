using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_IDataRecord : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReadFromBinary(IntPtr l) {
		try {
			GameFramework.IDataRecord self=(GameFramework.IDataRecord)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.ReadFromBinary(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteToBinary(IntPtr l) {
		try {
			GameFramework.IDataRecord self=(GameFramework.IDataRecord)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			self.WriteToBinary(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.IDataRecord");
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		createTypeMetatable(l,null, typeof(GameFramework.IDataRecord));
	}
}
