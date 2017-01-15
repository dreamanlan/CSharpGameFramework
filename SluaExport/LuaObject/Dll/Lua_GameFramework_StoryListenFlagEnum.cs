using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_StoryListenFlagEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.StoryListenFlagEnum");
		addMember(l,1,"Damage");
		addMember(l,2,"Story_Bit_2");
		addMember(l,4,"Story_Bit_3");
		addMember(l,8,"Story_Bit_4");
		addMember(l,16,"Story_Bit_5");
		addMember(l,32,"Story_Bit_6");
		addMember(l,64,"Story_Bit_7");
		addMember(l,128,"Story_Bit_8");
		addMember(l,256,"Story_Bit_9");
		addMember(l,512,"Story_Bit_10");
		addMember(l,1024,"Story_Bit_11");
		addMember(l,2048,"Story_Bit_12");
		addMember(l,4096,"Story_Bit_13");
		addMember(l,8192,"Story_Bit_14");
		addMember(l,16384,"Story_Bit_15");
		addMember(l,32768,"Story_Bit_16");
		addMember(l,65536,"Story_Bit_17");
		addMember(l,131072,"Story_Bit_18");
		addMember(l,262144,"Story_Bit_19");
		addMember(l,524288,"Story_Bit_20");
		addMember(l,1048576,"Story_Bit_21");
		addMember(l,2097152,"Story_Bit_22");
		addMember(l,4194304,"Story_Bit_23");
		addMember(l,8388608,"Story_Bit_24");
		addMember(l,16777216,"Story_Bit_25");
		addMember(l,33554432,"Story_Bit_26");
		addMember(l,67108864,"Story_Bit_27");
		addMember(l,134217728,"Story_Bit_28");
		addMember(l,268435456,"Story_Bit_29");
		addMember(l,536870912,"Story_Bit_30");
		addMember(l,1073741824,"Story_Bit_31");
		addMember(l,-2147483648,"Story_Bit_32");
		LuaDLL.lua_pop(l, 1);
	}
}
