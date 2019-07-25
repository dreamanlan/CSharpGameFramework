using System;

using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_ImpactData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.ImpactData o;
			o=new TableConfig.ImpactData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReadFromBinary(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
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
	static public int WriteToBinary(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
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
	static public int GetId(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
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
	static public int get_id(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_id(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
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
	static public int get_desc(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_desc(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_type(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_type(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
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
	static public int get_icon(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.icon);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_icon(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
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
	static public int get_duration(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.duration);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_duration(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.duration=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_cooldown(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.cooldown);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_cooldown(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.cooldown=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_multiple(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.multiple);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_multiple(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Collections.Generic.List<System.Int32> v;
			checkType(l,2,out v);
			self.multiple=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_damage(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.damage);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_damage(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Collections.Generic.List<System.Int32> v;
			checkType(l,2,out v);
			self.damage=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_vampire(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.vampire);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_vampire(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Collections.Generic.List<System.Int32> v;
			checkType(l,2,out v);
			self.vampire=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr1(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr1(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value1(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value1(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr2(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr2(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value2(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value2(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr3(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr3(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value3(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value3(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr4(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr4(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value4(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value4(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr5(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr5(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr5=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value5(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value5(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value5=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr6(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr6);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr6(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr6=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value6(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value6);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value6(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value6=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr7(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr7);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr7(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr7=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value7(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value7);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value7(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value7=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_attr8(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attr8);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attr8(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.attr8=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_value8(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.value8);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_value8(IntPtr l) {
		try {
			TableConfig.ImpactData self=(TableConfig.ImpactData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.value8=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.ImpactData");
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,GetId);
		addMember(l,"id",get_id,set_id,true);
		addMember(l,"desc",get_desc,set_desc,true);
		addMember(l,"type",get_type,set_type,true);
		addMember(l,"icon",get_icon,set_icon,true);
		addMember(l,"duration",get_duration,set_duration,true);
		addMember(l,"cooldown",get_cooldown,set_cooldown,true);
		addMember(l,"multiple",get_multiple,set_multiple,true);
		addMember(l,"damage",get_damage,set_damage,true);
		addMember(l,"vampire",get_vampire,set_vampire,true);
		addMember(l,"attr1",get_attr1,set_attr1,true);
		addMember(l,"value1",get_value1,set_value1,true);
		addMember(l,"attr2",get_attr2,set_attr2,true);
		addMember(l,"value2",get_value2,set_value2,true);
		addMember(l,"attr3",get_attr3,set_attr3,true);
		addMember(l,"value3",get_value3,set_value3,true);
		addMember(l,"attr4",get_attr4,set_attr4,true);
		addMember(l,"value4",get_value4,set_value4,true);
		addMember(l,"attr5",get_attr5,set_attr5,true);
		addMember(l,"value5",get_value5,set_value5,true);
		addMember(l,"attr6",get_attr6,set_attr6,true);
		addMember(l,"value6",get_value6,set_value6,true);
		addMember(l,"attr7",get_attr7,set_attr7,true);
		addMember(l,"value7",get_value7,set_value7,true);
		addMember(l,"attr8",get_attr8,set_attr8,true);
		addMember(l,"value8",get_value8,set_value8,true);
		createTypeMetatable(l,constructor, typeof(TableConfig.ImpactData));
	}
}
