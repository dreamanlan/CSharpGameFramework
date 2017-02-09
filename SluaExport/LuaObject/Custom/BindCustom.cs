using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_System_String.reg,
				Lua_System_Array.reg,
				Lua_System_Collections_ArrayList.reg,
				Lua_System_Collections_Hashtable.reg,
				Lua_System_SByte.reg,
				Lua_System_Byte.reg,
				Lua_System_Int16.reg,
				Lua_System_UInt16.reg,
				Lua_System_Int32.reg,
				Lua_System_UInt32.reg,
				Lua_System_Int64.reg,
				Lua_System_UInt64.reg,
				Lua_System_Single.reg,
				Lua_System_Double.reg,
				Lua_System_Convert.reg,
				Lua_System_TypeCode.reg,
				Lua_System_Type.reg,
				Lua_System_IO_File.reg,
				Lua_System_IO_Path.reg,
				Lua_System_DateTime.reg,
				Lua_System_TimeSpan.reg,
				Lua_System_Math.reg,
			};
			return list;
		}
	}
}
