// The MIT License (MIT)

// Copyright 2015 Siney/Pangweiwei siney@yeah.net
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
namespace SLua
{

/* https://msdn.microsoft.com/zh-cn/library/s1ax56ch.aspx
 * 
 * null								LUA_TNIL
 * Value Types:
 * 	enum
 * 	struct:
 * 		Numeric types: 
 *	 		Integral Types: 		LUA_TNUMBER
 * 				sbyte  = SByte
 * 				byte   = Byte
 * 				char   = Char
 * 				short  = Int16
 * 				ushort = UInt16
 * 				int	   = Int32
 * 				uint   = UInt32
 * 				long   = Int64
 * 				ulong  = UInt64
 * 			Floating-Point Types: 	LUA_TNUMBER
 * 				float  = Single
 * 				double = Double
 * 		bool = Boolean				LUA_TBOOLEAN
 * 		User defined structs		LUA_TTABLE(Vector...) || non_cached@LUA_TUSERDATA
 * Reference Types:
 *  string							LUA_TSTRING
 * 	delegate						LUA_TFUNCTION
 * 	class、System.Type				LUA_TTABLE || cached@LUA_TUSERDATA
 * 	object							cached@LUA_TUSERDATA
 *  char[]							LUA_TSTRING
 * 	T[]								LUA_TTABLE limit support
 *  interface, dynamic 				unsupport
 * IntPtr							LUA_TLIGHTUSERDATA
 * 
 * 
 * every type should implement: 
 * 		public static bool checkType(IntPtr l, int p, out T v)
 *		public static void pushValue(IntPtr l, T v)
 * 
*/	
	public partial class LuaObject
	{
#region enum
		static public bool checkEnum<T>(IntPtr l, int p, out T o) where T : struct
		{
			int i = (int) LuaDLL.luaL_checkinteger (l, p);
			o = (T)Enum.ToObject(typeof(T), i);

			return true;
		}

		public static void pushEnum(IntPtr l, int e)
		{
			pushValue(l, e);
		}
#endregion

#region Integral Types
	#region sbyte
		public static bool checkType(IntPtr l, int p, out sbyte v)
		{
			v = (sbyte)LuaDLL.luaL_checkinteger(l, p);
			return true;
		}
		
		public static void pushValue(IntPtr l, sbyte v)
		{
			LuaDLL.lua_pushinteger(l, v);
		}

	#endregion

		#region byte
		static public bool checkType(IntPtr l, int p, out byte v)
		{
			v = (byte)LuaDLL.luaL_checkinteger(l, p);
			return true;
		}

		public static void pushValue(IntPtr l, byte i)
		{
			LuaDLL.lua_pushinteger(l, i);
		}

		// why doesn't have a checkArray<byte[]> function accept lua string?
		// I think you should did a Buffer class to wrap byte[] pass/accept between mono and lua vm
		#endregion

		#region char
		static public bool checkType(IntPtr l, int p,out char c)
		{
			c = (char)LuaDLL.luaL_checkinteger(l, p);
			return true;
		}

		public static void pushValue(IntPtr l, char v)
		{
			LuaDLL.lua_pushinteger(l, v);
		}

		static public bool checkArray(IntPtr l, int p, out char[] pars)
		{
            if (LuaDLL.lua_type(l, p) == LuaTypes.LUA_TTABLE) {
                int n = LuaDLL.lua_rawlen(l, p);
                pars = new char[n];
                for (int k = 0; k < n; k++) {
                    LuaDLL.lua_rawgeti(l, p, k + 1);
                    checkType(l, -1, out pars[k]);
                    LuaDLL.lua_pop(l, 1);
                }
            }
            else {
                LuaDLL.luaL_checktype(l, p, LuaTypes.LUA_TSTRING);
                string s;
                checkType(l, p, out s);
                pars = s.ToCharArray();
            }
			return true;
		}
		#endregion

		#region short
		static public bool checkType(IntPtr l, int p, out short v)
		{
			v = (short)LuaDLL.luaL_checkinteger(l, p);
			return true;
		}
		
		public static void pushValue(IntPtr l, short i)
		{
			LuaDLL.lua_pushinteger(l, i);
		}
		#endregion

		#region ushort
		static public bool checkType(IntPtr l, int p, out ushort v)
		{
			v = (ushort)LuaDLL.luaL_checkinteger(l, p);
			return true;
		}
                
		public static void pushValue(IntPtr l, ushort v)
		{
		    LuaDLL.lua_pushinteger(l, v);
		}

        #endregion


        #region interface
        static public void pushInterface(IntPtr l,object i,Type t) {
			ObjectCache oc = ObjectCache.get(l);
            oc.pushInterface(l, i, t);
        }
        #endregion

		#region int
		static public bool checkType(IntPtr l, int p, out int v)
		{
			v = (int)LuaDLL.luaL_checkinteger(l, p);
			return true;
		}
                
		public static void pushValue(IntPtr l, int i)
		{
			LuaDLL.lua_pushinteger(l, i);
		}
		
		#endregion
		
		#region uint
		static public bool checkType(IntPtr l, int p, out uint v)
		{
			v = (uint)LuaDLL.luaL_checkinteger(l, p);
			return true;
		}
		
		public static void pushValue(IntPtr l, uint o)
		{
			LuaDLL.lua_pushnumber(l, o);
		}
		#endregion

		#region long
		static public bool checkType(IntPtr l, int p, out long v)
		{
#if LUA_5_3
            v = (long)LuaDLL.luaL_checkinteger(l, p);
#else
			v = (long)LuaDLL.luaL_checknumber(l, p);
#endif
			return true;
		}
		
		public static void pushValue(IntPtr l, long i)
		{
#if LUA_5_3
            LuaDLL.lua_pushinteger(l,i);
#else
			LuaDLL.lua_pushnumber(l, i);
#endif
		}
		
		#endregion

		#region ulong
		static public bool checkType(IntPtr l, int p, out ulong v)
		{
#if LUA_5_3
			v = (ulong)LuaDLL.luaL_checkinteger(l, p);
#else
			v = (ulong)LuaDLL.luaL_checknumber(l, p);
#endif
			return true;
		}
		
		public static void pushValue(IntPtr l, ulong o)
		{
			#if LUA_5_3
			LuaDLL.lua_pushinteger(l, (long)o);
			#else
			LuaDLL.lua_pushnumber(l, o);
			#endif
		}
		#endregion
		

#endregion

#region Floating-Point Types
		#region float
		public static bool checkType(IntPtr l, int p, out float v)
		{
			v = (float)LuaDLL.luaL_checknumber(l, p);
			return true;
		}

		public static void pushValue(IntPtr l, float o)
		{
			LuaDLL.lua_pushnumber(l, o);
		}
		
		#endregion

		#region double
		static public bool checkType(IntPtr l, int p, out double v)
		{
			v = LuaDLL.luaL_checknumber(l, p);
			return true;
		}
		
		public static void pushValue(IntPtr l, double d)
		{
			LuaDLL.lua_pushnumber(l, d);
		}
		
		#endregion
#endregion

		#region bool
		static public bool checkType(IntPtr l, int p, out bool v)
		{
			LuaDLL.luaL_checktype(l, p, LuaTypes.LUA_TBOOLEAN);
			v = LuaDLL.lua_toboolean(l, p);
			return true;
		}
		
		public static void pushValue(IntPtr l, bool b)
		{
			LuaDLL.lua_pushboolean(l, b);
		}
	
		#endregion

		#region string
		static public bool checkType(IntPtr l, int p, out string v)
		{
			if(LuaDLL.lua_isuserdata(l,p)>0)
			{
				object o = checkObj(l, p);
				if (o is string)
				{
					v = o as string;
					return true;
				}
			}
			else if (LuaDLL.lua_isstring(l, p))
			{
				v = LuaDLL.lua_tostring(l, p);
				return true;
			}
			
			v = null;
			return false;
		}

		static public bool checkBinaryString(IntPtr l,int p,out byte[] bytes){
			if(LuaDLL.lua_isstring(l,p)){
				bytes = LuaDLL.lua_tobytes(l, p);
				return true;
			}
			bytes = null;
			return false;
		}

		public static void pushValue(IntPtr l, string s)
		{
			LuaDLL.lua_pushstring(l, s);
		}
		

		#endregion

		#region IntPtr
		static public bool checkType(IntPtr l, int p, out IntPtr v)
		{
			v = LuaDLL.lua_touserdata(l, p);
			return true;
		}
		#endregion

		#region LuaType
		static public bool checkType(IntPtr l, int p, out LuaDelegate f)
		{
			LuaState state = LuaState.get(l);

			p = LuaDLL.lua_absindex(l, p);
			LuaDLL.luaL_checktype(l, p, LuaTypes.LUA_TFUNCTION);

			LuaDLL.lua_getglobal(l, DelgateTable);
			LuaDLL.lua_pushvalue(l, p);
			LuaDLL.lua_gettable(l, -2); // find function in __LuaDelegate table
			if (LuaDLL.lua_isnil(l, -1))
			{ // not found
				LuaDLL.lua_pop(l, 1); // pop nil
				f = newDelegate(l, p);
			}
			else
			{
				int fref = LuaDLL.lua_tointeger(l, -1);
				LuaDLL.lua_pop(l, 1); // pop ref value;
				f = state.delegateMap[fref];
				if (f == null)
				{
					f = newDelegate(l, p);
				}
			}
			LuaDLL.lua_pop(l, 1); // pop DelgateTable
			return true;
		}
		
		static public bool checkType(IntPtr l, int p, out LuaThread lt)
		{
			if (LuaDLL.lua_isnil(l, p))
			{
				lt = null;
				return true;
			}
			LuaDLL.luaL_checktype(l, p, LuaTypes.LUA_TTHREAD);
			LuaDLL.lua_pushvalue(l, p);
			int fref = LuaDLL.luaL_ref(l, LuaIndexes.LUA_REGISTRYINDEX);
			lt = new LuaThread(l, fref);
			return true;
		}

        static public bool checkType(IntPtr l, int p, out LuaFunction f)
		{
			if (LuaDLL.lua_isnil(l, p))
			{
				f = null;
				return true;
			}
			LuaDLL.luaL_checktype(l, p, LuaTypes.LUA_TFUNCTION);
			LuaDLL.lua_pushvalue(l, p);
			int fref = LuaDLL.luaL_ref(l, LuaIndexes.LUA_REGISTRYINDEX);
			f = new LuaFunction(l, fref);
			return true;
		}

		static public bool checkType(IntPtr l, int p, out LuaTable t)
		{
			if (LuaDLL.lua_isnil(l, p))
			{
				t = null;
				return true;
			}
			LuaDLL.luaL_checktype(l, p, LuaTypes.LUA_TTABLE);
			LuaDLL.lua_pushvalue(l, p);
			int fref = LuaDLL.luaL_ref(l, LuaIndexes.LUA_REGISTRYINDEX);
			t = new LuaTable(l, fref);
			return true;
		}

		public static void pushValue(IntPtr l, LuaCSFunction f)
		{
			LuaState.pushcsfunction (l, f);
		}
		
		public static void pushValue(IntPtr l, LuaTable t)
		{
			if (t == null)
				LuaDLL.lua_pushnil(l);
			else
				t.push(l);
		}
		#endregion

		#region Type
		private static Type MonoType = typeof(Type).GetType();

		public static Type FindType(string qualifiedTypeName) 
		{
			Type t = Type.GetType(qualifiedTypeName);

			if (t != null)
			{
				return t;
			}
			else
			{
				Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int n = 0; n < Assemblies.Length;n++ )
				{
					Assembly asm = Assemblies[n];
					t = asm.GetType(qualifiedTypeName);
					if (t != null)
						return t;
				}
				return null;
			}
		}


		static public bool checkType(IntPtr l, int p, out Type t)
		{
			string tname = null;
			LuaTypes lt = LuaDLL.lua_type(l, p);
            switch (lt)
            {
                case LuaTypes.LUA_TUSERDATA:
                    object o = checkObj(l, p);
                    if (o.GetType() != MonoType)
                        throw new Exception(string.Format("{0} expect Type, got {1}", p, o.GetType().Name));
                    t = (Type)o;
					return true;
                case LuaTypes.LUA_TTABLE:
                    LuaDLL.lua_pushstring(l, "__type");
                    LuaDLL.lua_rawget(l, p);
                    if (!LuaDLL.lua_isnil(l, -1))
                    {
                        t = (Type)checkObj(l, -1);
                        LuaDLL.lua_pop(l, 1);
                        return true;
                    }
                    else
                    {
                        LuaDLL.lua_pushstring(l, "__fullname");
                        LuaDLL.lua_rawget(l, p);
						if (!LuaDLL.lua_isnil (l, -1)) {
							tname = LuaDLL.lua_tostring (l, -1);
							LuaDLL.lua_pop (l, 2);
						} else {
                            /**
                             * 这里处理的是slua手写lua实现的值类型作为参数传递时的处理（比如typeof操作的结果）
                             * slua手写lua实现的值类型，一般会提供lua端的type table，然后用__raw指向c#端定义
                             * 的type table(注意对象实例的元表也使用了lua端的，实例在c#端会重建一个，从而可以
                             * 正常使用c#端定义的元表，见checkType对应于值类型的重载，主要由slua.c里提供的系列
                             * 方法luaS_checkXXX/luaS_pushXXX来进行c#/lua间值类型的转换):
                             *      local Raw = UnityEngine.Vector4
                             *      local Vector4 = { __typename = 'Vector4', __raw = Raw }
                             *      local I = { __typename = 'Vector4' }
                             *      _G['UnityEngine.Vector4.Instance'] = I
                             *      UnityEngine.Vector4 = Vector4
                            */
                            LuaDLL.lua_pushstring(l, "__raw");
							LuaDLL.lua_rawget(l, p);
							if (!LuaDLL.lua_isnil (l, -1)) {
								LuaDLL.lua_pushstring (l, "__type");
								LuaDLL.lua_rawget (l, -2);
								if (!LuaDLL.lua_isnil (l, -1)) {
									t = (Type)checkObj (l, -1);
									LuaDLL.lua_pop (l, 4);
									return true;
								} else {
									LuaDLL.lua_pushstring (l, "__fullname");
									LuaDLL.lua_rawget (l, -3);
									tname = LuaDLL.lua_tostring (l, -1);
									LuaDLL.lua_pop (l, 5);
								}
							} else {
								LuaDLL.lua_pop (l, 3);
							}
						}
                    }
                    if (tname == null) {
                        t = typeof(LuaTable);
                        return true;
                    }
                    break;

                case LuaTypes.LUA_TSTRING:
                    checkType(l, p, out tname);
                    break;
            }

			if (tname == null)
				throw new Exception("expect string or type table");

			t = LuaObject.FindType(tname);
            if (t != null && lt==LuaTypes.LUA_TTABLE)
            {
                //缓存Type到__type,注意__type只是一个cache字段，不是一定存在的
                LuaDLL.lua_pushstring(l, "__type");
				pushLightObject(l, t);
                LuaDLL.lua_rawset(l, p);
            }
			return t != null;
		}
		#endregion

		#region struct
		static public bool checkValueType<T>(IntPtr l, int p, out T v) where T:struct
		{
            var ret = checkObj(l, p);
            if (null == ret)
                v = default(T);
            else
			    v = (T)ret;
			return true;
		}
		#endregion

		static public bool checkNullable<T>(IntPtr l, int p, out Nullable<T> v) where T : struct
		{
			if (LuaDLL.lua_isnil(l, p))
				v = null;
			else
			{
				object o=checkVar(l, p, typeof(T));
				if (o == null) v = null;
				else v = new Nullable<T>((T)o);
			}
			return true;
		}

        #region object
		static public bool checkType<T>(IntPtr l, int p, out T o) where T:class
		{
            if (typeof(T) == typeof(Type)) {
                Type t;
                bool ret = checkType(l, p, out t);
                o = t as T;
                return ret;
            } else if (typeof(T) == typeof(LuaDelegate)) {
                LuaDelegate t;
                bool ret = checkType(l, p, out t);
                o = t as T;
                return ret;
            }
			object obj = checkVar(l, p);
			if (obj == null)
			{
				o = null;
				return true;
			}

			o = obj as T;
			if (o == null)
				throw new Exception(string.Format("arg {0} is not type of {1}", p, typeof(T).Name));
            if (typeof(T) == typeof(object) && o is LuaTable) {
                object[] arr;
                bool ret = checkArray(l, p, out arr);
                var to = arr as T;
                if (null != to && arr.Length > 0) {
                    o = to;
                }
                return ret;
            }
			return true;
		}
        static public bool checkArray(IntPtr l, int p, out object[] ta)
        {
            if (LuaDLL.lua_type(l, p) == LuaTypes.LUA_TTABLE) {
                int n = LuaDLL.lua_rawlen(l, p);
                ta = new object[n];
                for (int k = 0; k < n; k++) {
                    LuaDLL.lua_rawgeti(l, p, k + 1);
                    ta[k] = checkVar(l, -1);
                    LuaDLL.lua_pop(l, 1);
                }
                return true;
            } else {
                var obj = checkObj(l, p);
                Array array = obj as Array;
                if (null != array) {
                    ta = new object[array.Length];
                    array.CopyTo(ta, 0);
                } else if (null == obj) {
                    ta = null;
                } else {
                    throw new ArgumentException("expect array");
                }
                return null != array;
            }
        }
		#endregion
	}
}
