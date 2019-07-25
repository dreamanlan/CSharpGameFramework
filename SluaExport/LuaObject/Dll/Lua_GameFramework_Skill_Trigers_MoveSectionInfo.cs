using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_MoveSectionInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo o;
			o=new GameFramework.Skill.Trigers.MoveSectionInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_moveTime(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.moveTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_moveTime(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.moveTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_speedVect(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.speedVect);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_speedVect(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.speedVect=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_accelVect(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.accelVect);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_accelVect(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.accelVect=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_startTime(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.startTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_startTime(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.startTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_lastUpdateTime(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.lastUpdateTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_lastUpdateTime(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.lastUpdateTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_curSpeedVect(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.curSpeedVect);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_curSpeedVect(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.MoveSectionInfo self=(GameFramework.Skill.Trigers.MoveSectionInfo)checkSelf(l);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.curSpeedVect=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.Trigers.MoveSectionInfo");
		addMember(l,"moveTime",get_moveTime,set_moveTime,true);
		addMember(l,"speedVect",get_speedVect,set_speedVect,true);
		addMember(l,"accelVect",get_accelVect,set_accelVect,true);
		addMember(l,"startTime",get_startTime,set_startTime,true);
		addMember(l,"lastUpdateTime",get_lastUpdateTime,set_lastUpdateTime,true);
		addMember(l,"curSpeedVect",get_curSpeedVect,set_curSpeedVect,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.Trigers.MoveSectionInfo));
	}
}
