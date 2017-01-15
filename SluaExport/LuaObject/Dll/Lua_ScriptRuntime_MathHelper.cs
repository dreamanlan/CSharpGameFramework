using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_MathHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clamp_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.MathHelper.Clamp(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Hermite_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			var ret=ScriptRuntime.MathHelper.Hermite(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Lerp_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.MathHelper.Lerp(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Max_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(int),typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.MathHelper.Max(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float),typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.MathHelper.Max(a1,a2);
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
	static public int Min_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(int),typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.MathHelper.Min(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float),typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=ScriptRuntime.MathHelper.Min(a1,a2);
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
	static public int SmoothStep_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.MathHelper.SmoothStep(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RadiansToDegrees_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.RadiansToDegrees(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DegreesToRadians_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.DegreesToRadians(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Sin_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Sin(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Cos_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Cos(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Tan_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Tan(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ASin_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.ASin(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ACos_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.ACos(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ATan_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.ATan(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ATan2_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.MathHelper.ATan2(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Sqrt_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Sqrt(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Abs_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(int))){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=ScriptRuntime.MathHelper.Abs(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=ScriptRuntime.MathHelper.Abs(a1);
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
	static public int Pow_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.MathHelper.Pow(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Exp_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Exp(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Log_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Log(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Log10_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Log10(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Ceil_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Ceil(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Floor_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Floor(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Round_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.Round(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ICeil_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.ICeil(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IFloor_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.IFloor(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IRound_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.IRound(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsPowerOfTwo_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.IsPowerOfTwo(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NextPowerOfTwo_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=ScriptRuntime.MathHelper.NextPowerOfTwo(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_E(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.E);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Log10E(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.Log10E);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Log2E(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.Log2E);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Pi(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.Pi);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PiOver2(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.PiOver2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PiOver4(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.PiOver4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TwoPi(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.TwoPi);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Deg2Rad(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.Deg2Rad);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Rad2Deg(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.Rad2Deg);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Epsilon(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.Epsilon);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Infinity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.Infinity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_NegativeInfinity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.MathHelper.NegativeInfinity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.MathHelper");
		addMember(l,Clamp_s);
		addMember(l,Hermite_s);
		addMember(l,Lerp_s);
		addMember(l,Max_s);
		addMember(l,Min_s);
		addMember(l,SmoothStep_s);
		addMember(l,RadiansToDegrees_s);
		addMember(l,DegreesToRadians_s);
		addMember(l,Sin_s);
		addMember(l,Cos_s);
		addMember(l,Tan_s);
		addMember(l,ASin_s);
		addMember(l,ACos_s);
		addMember(l,ATan_s);
		addMember(l,ATan2_s);
		addMember(l,Sqrt_s);
		addMember(l,Abs_s);
		addMember(l,Pow_s);
		addMember(l,Exp_s);
		addMember(l,Log_s);
		addMember(l,Log10_s);
		addMember(l,Ceil_s);
		addMember(l,Floor_s);
		addMember(l,Round_s);
		addMember(l,ICeil_s);
		addMember(l,IFloor_s);
		addMember(l,IRound_s);
		addMember(l,IsPowerOfTwo_s);
		addMember(l,NextPowerOfTwo_s);
		addMember(l,"E",get_E,null,false);
		addMember(l,"Log10E",get_Log10E,null,false);
		addMember(l,"Log2E",get_Log2E,null,false);
		addMember(l,"Pi",get_Pi,null,false);
		addMember(l,"PiOver2",get_PiOver2,null,false);
		addMember(l,"PiOver4",get_PiOver4,null,false);
		addMember(l,"TwoPi",get_TwoPi,null,false);
		addMember(l,"Deg2Rad",get_Deg2Rad,null,false);
		addMember(l,"Rad2Deg",get_Rad2Deg,null,false);
		addMember(l,"Epsilon",get_Epsilon,null,false);
		addMember(l,"Infinity",get_Infinity,null,false);
		addMember(l,"NegativeInfinity",get_NegativeInfinity,null,false);
		createTypeMetatable(l,null, typeof(ScriptRuntime.MathHelper));
	}
}
