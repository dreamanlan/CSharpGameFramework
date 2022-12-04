using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TableConfig_Actor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			TableConfig.Actor o;
			o=new TableConfig.Actor();
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
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
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
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
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
	static public int GetId(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			var ret=self.GetId();
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
	static public int get_id(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_id(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_name(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_name(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_icon(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.icon);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_icon(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.icon=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_bigIcon(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bigIcon);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_bigIcon(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.bigIcon=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_type(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_type(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_avatar(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.avatar);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_avatar(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.avatar=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill0(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill0);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill0(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill0=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill1(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill1(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill2(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill2(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill3(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill3(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill4(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill4(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill5(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill5(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill5=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill6(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill6);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill6(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill6=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill7(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill7);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill7(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill7=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skill8(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill8);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skill8(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skill8=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_bornskill(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bornskill);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_bornskill(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.bornskill=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_deadskill(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.deadskill);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_deadskill(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.deadskill=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_size(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.size);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_size(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.size=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_logicsize(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.logicsize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_logicsize(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.logicsize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_speed(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.speed);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_speed(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.speed=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_viewrange(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.viewrange);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_viewrange(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.viewrange=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_gohomerange(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.gohomerange);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_gohomerange(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.gohomerange=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x4001(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x4001);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x4001(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x4001=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x4002(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x4002);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x4002(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x4002=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x4003(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x4003);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x4003(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x4003=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x4004(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x4004);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x4004(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x4004=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1001(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1001);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1001(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1001=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1002(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1002);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1002(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1002=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1006(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1006);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1006(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1006=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1007(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1007);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1007(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1007=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1011(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1011);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1011(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1011=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1012(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1012);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1012(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1012=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1016(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1016);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1016(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1016=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1017(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1017);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1017(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1017=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1021(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1021);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1021(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1021=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1022(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1022);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1022(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1022=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1024(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1024);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1024(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1024=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1026(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1026);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1026(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1026=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1028(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1028);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1028(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1028=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1030(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1030);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1030(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1030=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1032(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1032);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1032(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1032=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1033(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1033);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1033(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1033=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x1034(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x1034);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x1034(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x1034=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x2001(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x2001);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x2001(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x2001=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x2002(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x2002);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x2002(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x2002=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x2007(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x2007);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x2007(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x2007=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_x2008(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x2008);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_x2008(IntPtr l) {
		try {
			TableConfig.Actor self=(TableConfig.Actor)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.x2008=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.Actor");
		addMember(l,ctor_s);
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,GetId);
		addMember(l,"id",get_id,set_id,true);
		addMember(l,"name",get_name,set_name,true);
		addMember(l,"icon",get_icon,set_icon,true);
		addMember(l,"bigIcon",get_bigIcon,set_bigIcon,true);
		addMember(l,"type",get_type,set_type,true);
		addMember(l,"avatar",get_avatar,set_avatar,true);
		addMember(l,"skill0",get_skill0,set_skill0,true);
		addMember(l,"skill1",get_skill1,set_skill1,true);
		addMember(l,"skill2",get_skill2,set_skill2,true);
		addMember(l,"skill3",get_skill3,set_skill3,true);
		addMember(l,"skill4",get_skill4,set_skill4,true);
		addMember(l,"skill5",get_skill5,set_skill5,true);
		addMember(l,"skill6",get_skill6,set_skill6,true);
		addMember(l,"skill7",get_skill7,set_skill7,true);
		addMember(l,"skill8",get_skill8,set_skill8,true);
		addMember(l,"bornskill",get_bornskill,set_bornskill,true);
		addMember(l,"deadskill",get_deadskill,set_deadskill,true);
		addMember(l,"size",get_size,set_size,true);
		addMember(l,"logicsize",get_logicsize,set_logicsize,true);
		addMember(l,"speed",get_speed,set_speed,true);
		addMember(l,"viewrange",get_viewrange,set_viewrange,true);
		addMember(l,"gohomerange",get_gohomerange,set_gohomerange,true);
		addMember(l,"x4001",get_x4001,set_x4001,true);
		addMember(l,"x4002",get_x4002,set_x4002,true);
		addMember(l,"x4003",get_x4003,set_x4003,true);
		addMember(l,"x4004",get_x4004,set_x4004,true);
		addMember(l,"x1001",get_x1001,set_x1001,true);
		addMember(l,"x1002",get_x1002,set_x1002,true);
		addMember(l,"x1006",get_x1006,set_x1006,true);
		addMember(l,"x1007",get_x1007,set_x1007,true);
		addMember(l,"x1011",get_x1011,set_x1011,true);
		addMember(l,"x1012",get_x1012,set_x1012,true);
		addMember(l,"x1016",get_x1016,set_x1016,true);
		addMember(l,"x1017",get_x1017,set_x1017,true);
		addMember(l,"x1021",get_x1021,set_x1021,true);
		addMember(l,"x1022",get_x1022,set_x1022,true);
		addMember(l,"x1024",get_x1024,set_x1024,true);
		addMember(l,"x1026",get_x1026,set_x1026,true);
		addMember(l,"x1028",get_x1028,set_x1028,true);
		addMember(l,"x1030",get_x1030,set_x1030,true);
		addMember(l,"x1032",get_x1032,set_x1032,true);
		addMember(l,"x1033",get_x1033,set_x1033,true);
		addMember(l,"x1034",get_x1034,set_x1034,true);
		addMember(l,"x2001",get_x2001,set_x2001,true);
		addMember(l,"x2002",get_x2002,set_x2002,true);
		addMember(l,"x2007",get_x2007,set_x2007,true);
		addMember(l,"x2008",get_x2008,set_x2008,true);
		createTypeMetatable(l,null, typeof(TableConfig.Actor));
	}
}
