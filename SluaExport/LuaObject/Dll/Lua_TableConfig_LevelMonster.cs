using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TableConfig_LevelMonster : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			TableConfig.LevelMonster o;
			o=new TableConfig.LevelMonster();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ReadFromBinary(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.ReadFromBinary(a1,a2);
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
	static public int WriteToBinary(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			self.WriteToBinary(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_group(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.group);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_group(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.group=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_scene(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.scene);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_scene(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.scene=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_camp(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.camp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_camp(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.camp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_actorID(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.actorID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_actorID(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.actorID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.x=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_y(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.y);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_y(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.y=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_level(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_level(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_passive(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.passive);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_passive(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.passive=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_aiLogic(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.aiLogic);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_aiLogic(IntPtr l) {
		try {
			TableConfig.LevelMonster self=(TableConfig.LevelMonster)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.aiLogic=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.LevelMonster");
		addMember(l,ctor_s);
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,"group",get_group,set_group,true);
		addMember(l,"scene",get_scene,set_scene,true);
		addMember(l,"camp",get_camp,set_camp,true);
		addMember(l,"actorID",get_actorID,set_actorID,true);
		addMember(l,"x",get_x,set_x,true);
		addMember(l,"y",get_y,set_y,true);
		addMember(l,"dir",get_dir,set_dir,true);
		addMember(l,"level",get_level,set_level,true);
		addMember(l,"passive",get_passive,set_passive,true);
		addMember(l,"aiLogic",get_aiLogic,set_aiLogic,true);
		createTypeMetatable(l,null, typeof(TableConfig.LevelMonster));
	}
}
