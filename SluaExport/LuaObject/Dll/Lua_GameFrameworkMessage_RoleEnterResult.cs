using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_RoleEnterResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult o;
			o=new GameFrameworkMessage.RoleEnterResult();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Result(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.Result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Result(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			GameFrameworkMessage.RoleEnterResult.RoleEnterResultEnum v;
			checkEnum(l,2,out v);
			self.Result=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Nickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.Nickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HeroId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HeroId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HeroId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.HeroId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Money(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Money);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Money(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Money=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Gold);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Gold=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Level(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Level(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.SceneId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_WorldId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.WorldId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_WorldId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.WorldId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Members(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Members);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Items(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Items);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Friends(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Friends);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SummonerSkillId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SummonerSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SummonerSkillId(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnterResult self=(GameFrameworkMessage.RoleEnterResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.SummonerSkillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.RoleEnterResult");
		addMember(l,"Result",get_Result,set_Result,true);
		addMember(l,"Nickname",get_Nickname,set_Nickname,true);
		addMember(l,"HeroId",get_HeroId,set_HeroId,true);
		addMember(l,"Money",get_Money,set_Money,true);
		addMember(l,"Gold",get_Gold,set_Gold,true);
		addMember(l,"Level",get_Level,set_Level,true);
		addMember(l,"SceneId",get_SceneId,set_SceneId,true);
		addMember(l,"WorldId",get_WorldId,set_WorldId,true);
		addMember(l,"Members",get_Members,null,true);
		addMember(l,"Items",get_Items,null,true);
		addMember(l,"Friends",get_Friends,null,true);
		addMember(l,"SummonerSkillId",get_SummonerSkillId,set_SummonerSkillId,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.RoleEnterResult));
	}
}
