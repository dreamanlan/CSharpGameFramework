using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_TimeUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LobbyGetMillisecondsFromLastResponse_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeUtility.LobbyGetMillisecondsFromLastResponse();
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
	static public int GetMillisecondsFromLastResponse_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeUtility.GetMillisecondsFromLastResponse();
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
	static public int GetServerDateTime_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeUtility.GetServerDateTime();
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
	static public int GetTimeScale_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeUtility.GetTimeScale();
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
	static public int GetLocalMilliseconds_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeUtility.GetLocalMilliseconds();
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
	static public int GetLocalRealMilliseconds_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeUtility.GetLocalRealMilliseconds();
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
	static public int GetElapsedTimeUs_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeUtility.GetElapsedTimeUs();
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
	static public int UpdateGfxTime_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			GameFramework.TimeUtility.UpdateGfxTime(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TimeCounterInterval(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.TimeCounterInterval);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CurTimestamp(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.CurTimestamp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LobbyLastResponseTime(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.LobbyLastResponseTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LobbyLastResponseTime(IntPtr l) {
		try {
			System.Int64 v;
			checkType(l,2,out v);
			GameFramework.TimeUtility.LobbyLastResponseTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LobbyAverageRoundtripTime(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.LobbyAverageRoundtripTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LobbyAverageRoundtripTime(IntPtr l) {
		try {
			System.Int64 v;
			checkType(l,2,out v);
			GameFramework.TimeUtility.LobbyAverageRoundtripTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LastResponseTime(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.LastResponseTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LastResponseTime(IntPtr l) {
		try {
			System.Int64 v;
			checkType(l,2,out v);
			GameFramework.TimeUtility.LastResponseTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AverageRoundtripTime(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.AverageRoundtripTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AverageRoundtripTime(IntPtr l) {
		try {
			System.Int64 v;
			checkType(l,2,out v);
			GameFramework.TimeUtility.AverageRoundtripTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_RemoteTimeOffset(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.RemoteTimeOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_RemoteTimeOffset(IntPtr l) {
		try {
			System.Int64 v;
			checkType(l,2,out v);
			GameFramework.TimeUtility.RemoteTimeOffset=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GfxFps(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.GfxFps);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_GfxFps(IntPtr l) {
		try {
			float v;
			checkType(l,2,out v);
			GameFramework.TimeUtility.GfxFps=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GfxAvgFps(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.TimeUtility.GfxAvgFps);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_GfxAvgFps(IntPtr l) {
		try {
			float v;
			checkType(l,2,out v);
			GameFramework.TimeUtility.GfxAvgFps=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.TimeUtility");
		addMember(l,LobbyGetMillisecondsFromLastResponse_s);
		addMember(l,GetMillisecondsFromLastResponse_s);
		addMember(l,GetServerDateTime_s);
		addMember(l,GetTimeScale_s);
		addMember(l,GetLocalMilliseconds_s);
		addMember(l,GetLocalRealMilliseconds_s);
		addMember(l,GetElapsedTimeUs_s);
		addMember(l,UpdateGfxTime_s);
		addMember(l,"TimeCounterInterval",get_TimeCounterInterval,null,false);
		addMember(l,"CurTimestamp",get_CurTimestamp,null,false);
		addMember(l,"LobbyLastResponseTime",get_LobbyLastResponseTime,set_LobbyLastResponseTime,false);
		addMember(l,"LobbyAverageRoundtripTime",get_LobbyAverageRoundtripTime,set_LobbyAverageRoundtripTime,false);
		addMember(l,"LastResponseTime",get_LastResponseTime,set_LastResponseTime,false);
		addMember(l,"AverageRoundtripTime",get_AverageRoundtripTime,set_AverageRoundtripTime,false);
		addMember(l,"RemoteTimeOffset",get_RemoteTimeOffset,set_RemoteTimeOffset,false);
		addMember(l,"GfxFps",get_GfxFps,set_GfxFps,false);
		addMember(l,"GfxAvgFps",get_GfxAvgFps,set_GfxAvgFps,false);
		createTypeMetatable(l,null, typeof(GameFramework.TimeUtility));
	}
}
