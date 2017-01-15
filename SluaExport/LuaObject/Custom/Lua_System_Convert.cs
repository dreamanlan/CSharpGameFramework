using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_System_Convert : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromBase64CharArray_s(IntPtr l) {
		try {
			System.Char[] a1;
			checkArray(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=System.Convert.FromBase64CharArray(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromBase64String_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=System.Convert.FromBase64String(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTypeCode_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			var ret=System.Convert.GetTypeCode(a1);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsDBNull_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			var ret=System.Convert.IsDBNull(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToBase64CharArray_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Char[] a4;
				checkArray(l,4,out a4);
				System.Int32 a5;
				checkType(l,5,out a5);
				var ret=System.Convert.ToBase64CharArray(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Char[] a4;
				checkArray(l,4,out a4);
				System.Int32 a5;
				checkType(l,5,out a5);
				System.Base64FormattingOptions a6;
				checkEnum(l,6,out a6);
				var ret=System.Convert.ToBase64CharArray(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToBase64String_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				var ret=System.Convert.ToBase64String(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.Base64FormattingOptions a2;
				checkEnum(l,2,out a2);
				var ret=System.Convert.ToBase64String(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				var ret=System.Convert.ToBase64String(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Base64FormattingOptions a4;
				checkEnum(l,4,out a4);
				var ret=System.Convert.ToBase64String(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToBoolean_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToBoolean(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToBoolean(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToBoolean(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToByte_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToByte(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToByte(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToByte(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToChar_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToChar(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToChar(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToChar(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToDateTime_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToDateTime(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToDateTime(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToDateTime(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToDecimal_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDecimal(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToDecimal(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToDecimal(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToDouble_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToDouble(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToDouble(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToDouble(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToInt16_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt16(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt16(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt16(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToInt32_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt32(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt32(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt32(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToInt64_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt64(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt64(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToInt64(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToSByte_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSByte(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToSByte(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToSByte(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToSByte(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToSingle_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToSingle(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToSingle(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToSingle(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToUInt16_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt16(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt16(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt16(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt16(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToUInt32_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt32(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt32(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt32(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt32(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToUInt64_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Int16))){
				System.Int16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string))){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Int64))){
				System.Int64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.SByte))){
				System.SByte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt16))){
				System.UInt16 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object))){
				System.Object a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt32))){
				System.UInt32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.UInt64))){
				System.UInt64 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool))){
				System.Boolean a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Byte))){
				System.Byte a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Char))){
				System.Char a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Decimal))){
				System.Decimal a1;
				checkValueType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=System.Convert.ToUInt64(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt64(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.IFormatProvider))){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt64(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ToUInt64(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ChangeType_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Object),typeof(System.TypeCode))){
				System.Object a1;
				checkType(l,1,out a1);
				System.TypeCode a2;
				checkEnum(l,2,out a2);
				var ret=System.Convert.ChangeType(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.Type))){
				System.Object a1;
				checkType(l,1,out a1);
				System.Type a2;
				checkType(l,2,out a2);
				var ret=System.Convert.ChangeType(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.TypeCode),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.TypeCode a2;
				checkEnum(l,2,out a2);
				System.IFormatProvider a3;
				checkType(l,3,out a3);
				var ret=System.Convert.ChangeType(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Object),typeof(System.Type),typeof(System.IFormatProvider))){
				System.Object a1;
				checkType(l,1,out a1);
				System.Type a2;
				checkType(l,2,out a2);
				System.IFormatProvider a3;
				checkType(l,3,out a3);
				var ret=System.Convert.ChangeType(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int get_DBNull(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.Convert.DBNull);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Convert");
		addMember(l,FromBase64CharArray_s);
		addMember(l,FromBase64String_s);
		addMember(l,GetTypeCode_s);
		addMember(l,IsDBNull_s);
		addMember(l,ToBase64CharArray_s);
		addMember(l,ToBase64String_s);
		addMember(l,ToBoolean_s);
		addMember(l,ToByte_s);
		addMember(l,ToChar_s);
		addMember(l,ToDateTime_s);
		addMember(l,ToDecimal_s);
		addMember(l,ToDouble_s);
		addMember(l,ToInt16_s);
		addMember(l,ToInt32_s);
		addMember(l,ToInt64_s);
		addMember(l,ToSByte_s);
		addMember(l,ToSingle_s);
		addMember(l,ToUInt16_s);
		addMember(l,ToUInt32_s);
		addMember(l,ToUInt64_s);
		addMember(l,ChangeType_s);
		addMember(l,"DBNull",get_DBNull,null,false);
		createTypeMetatable(l,null, typeof(System.Convert));
	}
}
