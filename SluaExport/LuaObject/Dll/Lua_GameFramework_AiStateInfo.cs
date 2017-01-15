using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_AiStateInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.AiStateInfo o;
			o=new GameFramework.AiStateInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PushState(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.PushState(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PopState(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			var ret=self.PopState();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ChangeToState(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.ChangeToState(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CloneAiStates(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
				var ret=self.CloneAiStates();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
				System.Collections.Generic.IEnumerable<System.Int32> a1;
				checkType(l,2,out a1);
				self.CloneAiStates(a1);
				pushValue(l,true);
				return 1;
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
	static public int Reset(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetExternalTarget(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetExternalTarget(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_MaxAiParamNum(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.AiStateInfo.c_MaxAiParamNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CurState(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurState);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AiLogic(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AiLogic);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_AiLogic(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.AiLogic=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AiParam(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AiParam);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AiDatas(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AiDatas);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AiStoryInstanceInfo(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AiStoryInstanceInfo);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_AiStoryInstanceInfo(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			GameFramework.AiStoryInstanceInfo v;
			checkType(l,2,out v);
			self.AiStoryInstanceInfo=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsInited(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsInited);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsInited(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsInited=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Time(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Time);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Time(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.Time=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_FormationId(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FormationId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_FormationId(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.FormationId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LeaderID(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LeaderID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LeaderID(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.LeaderID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HomePos(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HomePos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HomePos(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.HomePos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Target(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Target);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Target(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Target=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HateTarget(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HateTarget);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HateTarget(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.HateTarget=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsExternalTarget(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsExternalTarget);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LastChangeTargetTime(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LastChangeTargetTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LastChangeTargetTime(IntPtr l) {
		try {
			GameFramework.AiStateInfo self=(GameFramework.AiStateInfo)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.LastChangeTargetTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AiStateInfo");
		addMember(l,PushState);
		addMember(l,PopState);
		addMember(l,ChangeToState);
		addMember(l,CloneAiStates);
		addMember(l,Reset);
		addMember(l,SetExternalTarget);
		addMember(l,"c_MaxAiParamNum",get_c_MaxAiParamNum,null,false);
		addMember(l,"CurState",get_CurState,null,true);
		addMember(l,"AiLogic",get_AiLogic,set_AiLogic,true);
		addMember(l,"AiParam",get_AiParam,null,true);
		addMember(l,"AiDatas",get_AiDatas,null,true);
		addMember(l,"AiStoryInstanceInfo",get_AiStoryInstanceInfo,set_AiStoryInstanceInfo,true);
		addMember(l,"IsInited",get_IsInited,set_IsInited,true);
		addMember(l,"Time",get_Time,set_Time,true);
		addMember(l,"FormationId",get_FormationId,set_FormationId,true);
		addMember(l,"LeaderID",get_LeaderID,set_LeaderID,true);
		addMember(l,"HomePos",get_HomePos,set_HomePos,true);
		addMember(l,"Target",get_Target,set_Target,true);
		addMember(l,"HateTarget",get_HateTarget,set_HateTarget,true);
		addMember(l,"IsExternalTarget",get_IsExternalTarget,null,true);
		addMember(l,"LastChangeTargetTime",get_LastChangeTargetTime,set_LastChangeTargetTime,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.AiStateInfo));
	}
}
