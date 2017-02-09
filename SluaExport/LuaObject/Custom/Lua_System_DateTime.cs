using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_System_DateTime : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			System.DateTime o;
			if(argc==2){
				System.Int64 a1;
				checkType(l,2,out a1);
				o=new System.DateTime(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				o=new System.DateTime(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==7){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				o=new System.DateTime(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				System.Int32 a7;
				checkType(l,8,out a7);
				o=new System.DateTime(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==5){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Globalization.Calendar a4;
				checkType(l,5,out a4);
				o=new System.DateTime(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(System.Globalization.Calendar))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				System.Globalization.Calendar a7;
				checkType(l,8,out a7);
				o=new System.DateTime(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(System.Globalization.Calendar))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				System.Int32 a7;
				checkType(l,8,out a7);
				System.Globalization.Calendar a8;
				checkType(l,9,out a8);
				o=new System.DateTime(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				System.Int64 a1;
				checkType(l,2,out a1);
				System.DateTimeKind a2;
				checkEnum(l,3,out a2);
				o=new System.DateTime(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(System.DateTimeKind))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				System.DateTimeKind a7;
				checkEnum(l,8,out a7);
				o=new System.DateTime(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(int),typeof(System.DateTimeKind))){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				System.Int32 a7;
				checkType(l,8,out a7);
				System.DateTimeKind a8;
				checkEnum(l,9,out a8);
				o=new System.DateTime(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==10){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				System.Int32 a7;
				checkType(l,8,out a7);
				System.Globalization.Calendar a8;
				checkType(l,9,out a8);
				System.DateTimeKind a9;
				checkEnum(l,10,out a9);
				o=new System.DateTime(a1,a2,a3,a4,a5,a6,a7,a8,a9);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc<=1){
				o=new System.DateTime();
				pushValue(l,true);
				pushObject(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Add(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.TimeSpan a1;
			checkValueType(l,2,out a1);
			var ret=self.Add(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddDays(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Double a1;
			checkType(l,2,out a1);
			var ret=self.AddDays(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddTicks(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Int64 a1;
			checkType(l,2,out a1);
			var ret=self.AddTicks(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddHours(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Double a1;
			checkType(l,2,out a1);
			var ret=self.AddHours(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddMilliseconds(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Double a1;
			checkType(l,2,out a1);
			var ret=self.AddMilliseconds(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddMinutes(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Double a1;
			checkType(l,2,out a1);
			var ret=self.AddMinutes(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddMonths(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.AddMonths(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddSeconds(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Double a1;
			checkType(l,2,out a1);
			var ret=self.AddSeconds(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddYears(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.AddYears(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CompareTo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(System.DateTime))){
				System.DateTime self;
				checkValueType(l,1,out self);
				System.DateTime a1;
				checkValueType(l,2,out a1);
				var ret=self.CompareTo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Object))){
				System.DateTime self;
				checkValueType(l,1,out self);
				System.Object a1;
				checkType(l,2,out a1);
				var ret=self.CompareTo(a1);
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
	static public int IsDaylightSavingTime(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.IsDaylightSavingTime();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToBinary(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToBinary();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetDateTimeFormats(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.DateTime self;
				checkValueType(l,1,out self);
				var ret=self.GetDateTimeFormats();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.IFormatProvider))){
				System.DateTime self;
				checkValueType(l,1,out self);
				System.IFormatProvider a1;
				checkType(l,2,out a1);
				var ret=self.GetDateTimeFormats(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Char))){
				System.DateTime self;
				checkValueType(l,1,out self);
				System.Char a1;
				checkType(l,2,out a1);
				var ret=self.GetDateTimeFormats(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				System.DateTime self;
				checkValueType(l,1,out self);
				System.Char a1;
				checkType(l,2,out a1);
				System.IFormatProvider a2;
				checkType(l,3,out a2);
				var ret=self.GetDateTimeFormats(a1,a2);
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
	static public int GetTypeCode(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.GetTypeCode();
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Subtract(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(System.TimeSpan))){
				System.DateTime self;
				checkValueType(l,1,out self);
				System.TimeSpan a1;
				checkValueType(l,2,out a1);
				var ret=self.Subtract(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.DateTime))){
				System.DateTime self;
				checkValueType(l,1,out self);
				System.DateTime a1;
				checkValueType(l,2,out a1);
				var ret=self.Subtract(a1);
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
	static public int ToFileTime(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToFileTime();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToFileTimeUtc(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToFileTimeUtc();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToLongDateString(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToLongDateString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToLongTimeString(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToLongTimeString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToOADate(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToOADate();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToShortDateString(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToShortDateString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToShortTimeString(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToShortTimeString();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToLocalTime(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToLocalTime();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToUniversalTime(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			var ret=self.ToUniversalTime();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Compare_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTime a2;
			checkValueType(l,2,out a2);
			var ret=System.DateTime.Compare(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromBinary_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=System.DateTime.FromBinary(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SpecifyKind_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTimeKind a2;
			checkEnum(l,2,out a2);
			var ret=System.DateTime.SpecifyKind(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DaysInMonth_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=System.DateTime.DaysInMonth(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromFileTime_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=System.DateTime.FromFileTime(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromFileTimeUtc_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=System.DateTime.FromFileTimeUtc(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromOADate_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=System.DateTime.FromOADate(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsLeapYear_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=System.DateTime.IsLeapYear(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Parse_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.String a1;
				checkType(l,1,out a1);
				var ret=System.DateTime.Parse(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				var ret=System.DateTime.Parse(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				System.Globalization.DateTimeStyles a3;
				checkEnum(l,3,out a3);
				var ret=System.DateTime.Parse(a1,a2,a3);
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
	static public int ParseExact_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.IFormatProvider a3;
				checkType(l,3,out a3);
				var ret=System.DateTime.ParseExact(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.String[]),typeof(System.IFormatProvider),typeof(System.Globalization.DateTimeStyles))){
				System.String a1;
				checkType(l,1,out a1);
				System.String[] a2;
				checkArray(l,2,out a2);
				System.IFormatProvider a3;
				checkType(l,3,out a3);
				System.Globalization.DateTimeStyles a4;
				checkEnum(l,4,out a4);
				var ret=System.DateTime.ParseExact(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(string),typeof(System.IFormatProvider),typeof(System.Globalization.DateTimeStyles))){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.IFormatProvider a3;
				checkType(l,3,out a3);
				System.Globalization.DateTimeStyles a4;
				checkEnum(l,4,out a4);
				var ret=System.DateTime.ParseExact(a1,a2,a3,a4);
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
	static public int TryParse_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				System.String a1;
				checkType(l,1,out a1);
				System.DateTime a2;
				var ret=System.DateTime.TryParse(a1,out a2);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a2);
				return 3;
			}
			else if(argc==4){
				System.String a1;
				checkType(l,1,out a1);
				System.IFormatProvider a2;
				checkType(l,2,out a2);
				System.Globalization.DateTimeStyles a3;
				checkEnum(l,3,out a3);
				System.DateTime a4;
				var ret=System.DateTime.TryParse(a1,a2,a3,out a4);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a4);
				return 3;
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
	static public int TryParseExact_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(string),typeof(System.String[]),typeof(System.IFormatProvider),typeof(System.Globalization.DateTimeStyles),typeof(LuaOut))){
				System.String a1;
				checkType(l,1,out a1);
				System.String[] a2;
				checkArray(l,2,out a2);
				System.IFormatProvider a3;
				checkType(l,3,out a3);
				System.Globalization.DateTimeStyles a4;
				checkEnum(l,4,out a4);
				System.DateTime a5;
				var ret=System.DateTime.TryParseExact(a1,a2,a3,a4,out a5);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a5);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(string),typeof(System.IFormatProvider),typeof(System.Globalization.DateTimeStyles),typeof(LuaOut))){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.IFormatProvider a3;
				checkType(l,3,out a3);
				System.Globalization.DateTimeStyles a4;
				checkEnum(l,4,out a4);
				System.DateTime a5;
				var ret=System.DateTime.TryParseExact(a1,a2,a3,a4,out a5);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a5);
				return 3;
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
	static public int op_Addition(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
			checkValueType(l,2,out a2);
			var ret=a1+a2;
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Equality(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTime a2;
			checkValueType(l,2,out a2);
			var ret=(a1==a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_GreaterThan(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTime a2;
			checkValueType(l,2,out a2);
			var ret=(a2<a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_GreaterThanOrEqual(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTime a2;
			checkValueType(l,2,out a2);
			var ret=(a2<=a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Inequality(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTime a2;
			checkValueType(l,2,out a2);
			var ret=(a1!=a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_LessThan(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTime a2;
			checkValueType(l,2,out a2);
			var ret=(a1<a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_LessThanOrEqual(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			System.DateTime a2;
			checkValueType(l,2,out a2);
			var ret=(a1<=a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Subtraction(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.DateTime),typeof(System.TimeSpan))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				System.TimeSpan a2;
				checkValueType(l,2,out a2);
				var ret=a1-a2;
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.DateTime),typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				System.DateTime a2;
				checkValueType(l,2,out a2);
				var ret=a1-a2;
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
	static public int get_MaxValue(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.DateTime.MaxValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MinValue(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.DateTime.MinValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Date(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Date);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Month(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Month);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Day(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Day);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DayOfWeek(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushEnum(l,(int)self.DayOfWeek);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DayOfYear(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.DayOfYear);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TimeOfDay(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TimeOfDay);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Hour(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Hour);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Minute(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Minute);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Second(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Second);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Millisecond(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Millisecond);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Now(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.DateTime.Now);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Ticks(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Ticks);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Today(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.DateTime.Today);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_UtcNow(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.DateTime.UtcNow);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Year(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Year);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Kind(IntPtr l) {
		try {
			System.DateTime self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushEnum(l,(int)self.Kind);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.DateTime");
		addMember(l,Add);
		addMember(l,AddDays);
		addMember(l,AddTicks);
		addMember(l,AddHours);
		addMember(l,AddMilliseconds);
		addMember(l,AddMinutes);
		addMember(l,AddMonths);
		addMember(l,AddSeconds);
		addMember(l,AddYears);
		addMember(l,CompareTo);
		addMember(l,IsDaylightSavingTime);
		addMember(l,ToBinary);
		addMember(l,GetDateTimeFormats);
		addMember(l,GetTypeCode);
		addMember(l,Subtract);
		addMember(l,ToFileTime);
		addMember(l,ToFileTimeUtc);
		addMember(l,ToLongDateString);
		addMember(l,ToLongTimeString);
		addMember(l,ToOADate);
		addMember(l,ToShortDateString);
		addMember(l,ToShortTimeString);
		addMember(l,ToLocalTime);
		addMember(l,ToUniversalTime);
		addMember(l,Compare_s);
		addMember(l,FromBinary_s);
		addMember(l,SpecifyKind_s);
		addMember(l,DaysInMonth_s);
		addMember(l,FromFileTime_s);
		addMember(l,FromFileTimeUtc_s);
		addMember(l,FromOADate_s);
		addMember(l,IsLeapYear_s);
		addMember(l,Parse_s);
		addMember(l,ParseExact_s);
		addMember(l,TryParse_s);
		addMember(l,TryParseExact_s);
		addMember(l,op_Addition);
		addMember(l,op_Equality);
		addMember(l,op_GreaterThan);
		addMember(l,op_GreaterThanOrEqual);
		addMember(l,op_Inequality);
		addMember(l,op_LessThan);
		addMember(l,op_LessThanOrEqual);
		addMember(l,op_Subtraction);
		addMember(l,"MaxValue",get_MaxValue,null,false);
		addMember(l,"MinValue",get_MinValue,null,false);
		addMember(l,"Date",get_Date,null,true);
		addMember(l,"Month",get_Month,null,true);
		addMember(l,"Day",get_Day,null,true);
		addMember(l,"DayOfWeek",get_DayOfWeek,null,true);
		addMember(l,"DayOfYear",get_DayOfYear,null,true);
		addMember(l,"TimeOfDay",get_TimeOfDay,null,true);
		addMember(l,"Hour",get_Hour,null,true);
		addMember(l,"Minute",get_Minute,null,true);
		addMember(l,"Second",get_Second,null,true);
		addMember(l,"Millisecond",get_Millisecond,null,true);
		addMember(l,"Now",get_Now,null,false);
		addMember(l,"Ticks",get_Ticks,null,true);
		addMember(l,"Today",get_Today,null,false);
		addMember(l,"UtcNow",get_UtcNow,null,false);
		addMember(l,"Year",get_Year,null,true);
		addMember(l,"Kind",get_Kind,null,true);
		createTypeMetatable(l,constructor, typeof(System.DateTime),typeof(System.ValueType));
	}
}
