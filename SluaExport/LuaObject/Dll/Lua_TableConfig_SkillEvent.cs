using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TableConfig_SkillEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			TableConfig.SkillEvent o;
			o=new TableConfig.SkillEvent();
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
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
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
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
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
	static public int get_actorId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.actorId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_actorId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.actorId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skillId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skillId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_eventId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.eventId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_eventId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.eventId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_triggerBuffId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.triggerBuffId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_triggerBuffId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.triggerBuffId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_triggerSkillId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.triggerSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_triggerSkillId(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.triggerSkillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_proc(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.proc);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_proc(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.proc=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_param1(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param1(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc1(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc1(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_param2(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param2(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc2(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc2(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_param3(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param3(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc3(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc3(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_param4(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param4(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc4(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc4(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_param5(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param5(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param5=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc5(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc5(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc5=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_param6(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param6);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param6(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param6=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc6(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc6);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc6(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc6=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_param7(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param7);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param7(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param7=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc7(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc7);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc7(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc7=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_param8(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.param8);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_param8(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.param8=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc8(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc8);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc8(IntPtr l) {
		try {
			TableConfig.SkillEvent self=(TableConfig.SkillEvent)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc8=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.SkillEvent");
		addMember(l,ctor_s);
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,"actorId",get_actorId,set_actorId,true);
		addMember(l,"skillId",get_skillId,set_skillId,true);
		addMember(l,"eventId",get_eventId,set_eventId,true);
		addMember(l,"triggerBuffId",get_triggerBuffId,set_triggerBuffId,true);
		addMember(l,"triggerSkillId",get_triggerSkillId,set_triggerSkillId,true);
		addMember(l,"proc",get_proc,set_proc,true);
		addMember(l,"desc",get_desc,set_desc,true);
		addMember(l,"param1",get_param1,set_param1,true);
		addMember(l,"desc1",get_desc1,set_desc1,true);
		addMember(l,"param2",get_param2,set_param2,true);
		addMember(l,"desc2",get_desc2,set_desc2,true);
		addMember(l,"param3",get_param3,set_param3,true);
		addMember(l,"desc3",get_desc3,set_desc3,true);
		addMember(l,"param4",get_param4,set_param4,true);
		addMember(l,"desc4",get_desc4,set_desc4,true);
		addMember(l,"param5",get_param5,set_param5,true);
		addMember(l,"desc5",get_desc5,set_desc5,true);
		addMember(l,"param6",get_param6,set_param6,true);
		addMember(l,"desc6",get_desc6,set_desc6,true);
		addMember(l,"param7",get_param7,set_param7,true);
		addMember(l,"desc7",get_desc7,set_desc7,true);
		addMember(l,"param8",get_param8,set_param8,true);
		addMember(l,"desc8",get_desc8,set_desc8,true);
		createTypeMetatable(l,null, typeof(TableConfig.SkillEvent));
	}
}
