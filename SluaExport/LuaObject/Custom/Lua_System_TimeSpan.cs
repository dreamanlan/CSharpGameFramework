using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_System_TimeSpan : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			System.TimeSpan o;
			if(argc==2){
				System.Int64 a1;
				checkType(l,2,out a1);
				o=new System.TimeSpan(a1);
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
				o=new System.TimeSpan(a1,a2,a3);
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
				System.Int32 a4;
				checkType(l,5,out a4);
				o=new System.TimeSpan(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==6){
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
				o=new System.TimeSpan(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc<=1){
				o=new System.TimeSpan();
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
			System.TimeSpan self;
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
	static public int CompareTo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(System.TimeSpan))){
				System.TimeSpan self;
				checkValueType(l,1,out self);
				System.TimeSpan a1;
				checkValueType(l,2,out a1);
				var ret=self.CompareTo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Object))){
				System.TimeSpan self;
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
	static public int Duration(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			var ret=self.Duration();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Negate(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			var ret=self.Negate();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Subtract(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			System.TimeSpan a1;
			checkValueType(l,2,out a1);
			var ret=self.Subtract(a1);
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
			checkValueType(l,2,out a2);
			var ret=System.TimeSpan.Compare(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromDays_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=System.TimeSpan.FromDays(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromHours_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=System.TimeSpan.FromHours(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromMinutes_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=System.TimeSpan.FromMinutes(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromSeconds_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=System.TimeSpan.FromSeconds(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromMilliseconds_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=System.TimeSpan.FromMilliseconds(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FromTicks_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=System.TimeSpan.FromTicks(a1);
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
			System.String a1;
			checkType(l,1,out a1);
			var ret=System.TimeSpan.Parse(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryParse_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.TimeSpan a2;
			var ret=System.TimeSpan.TryParse(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Addition(IntPtr l) {
		try {
			System.TimeSpan a1;
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
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
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			System.TimeSpan a2;
			checkValueType(l,2,out a2);
			var ret=a1-a2;
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_UnaryNegation(IntPtr l) {
		try {
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			var ret=-a1;
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_UnaryPlus(IntPtr l) {
		try {
			System.TimeSpan a1;
			checkValueType(l,1,out a1);
			var ret=+a1;
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TicksPerDay(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.TimeSpan.TicksPerDay);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TicksPerHour(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.TimeSpan.TicksPerHour);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TicksPerMillisecond(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.TimeSpan.TicksPerMillisecond);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TicksPerMinute(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.TimeSpan.TicksPerMinute);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TicksPerSecond(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.TimeSpan.TicksPerSecond);
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
			pushValue(l,System.TimeSpan.MaxValue);
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
			pushValue(l,System.TimeSpan.MinValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Zero(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,System.TimeSpan.Zero);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Days(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Days);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Hours(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Hours);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Milliseconds(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Milliseconds);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Minutes(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Minutes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Seconds(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Seconds);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Ticks(IntPtr l) {
		try {
			System.TimeSpan self;
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
	static public int get_TotalDays(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TotalDays);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TotalHours(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TotalHours);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TotalMilliseconds(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TotalMilliseconds);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TotalMinutes(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TotalMinutes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TotalSeconds(IntPtr l) {
		try {
			System.TimeSpan self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.TotalSeconds);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.TimeSpan");
		addMember(l,Add);
		addMember(l,CompareTo);
		addMember(l,Duration);
		addMember(l,Negate);
		addMember(l,Subtract);
		addMember(l,Compare_s);
		addMember(l,FromDays_s);
		addMember(l,FromHours_s);
		addMember(l,FromMinutes_s);
		addMember(l,FromSeconds_s);
		addMember(l,FromMilliseconds_s);
		addMember(l,FromTicks_s);
		addMember(l,Parse_s);
		addMember(l,TryParse_s);
		addMember(l,op_Addition);
		addMember(l,op_Equality);
		addMember(l,op_GreaterThan);
		addMember(l,op_GreaterThanOrEqual);
		addMember(l,op_Inequality);
		addMember(l,op_LessThan);
		addMember(l,op_LessThanOrEqual);
		addMember(l,op_Subtraction);
		addMember(l,op_UnaryNegation);
		addMember(l,op_UnaryPlus);
		addMember(l,"TicksPerDay",get_TicksPerDay,null,false);
		addMember(l,"TicksPerHour",get_TicksPerHour,null,false);
		addMember(l,"TicksPerMillisecond",get_TicksPerMillisecond,null,false);
		addMember(l,"TicksPerMinute",get_TicksPerMinute,null,false);
		addMember(l,"TicksPerSecond",get_TicksPerSecond,null,false);
		addMember(l,"MaxValue",get_MaxValue,null,false);
		addMember(l,"MinValue",get_MinValue,null,false);
		addMember(l,"Zero",get_Zero,null,false);
		addMember(l,"Days",get_Days,null,true);
		addMember(l,"Hours",get_Hours,null,true);
		addMember(l,"Milliseconds",get_Milliseconds,null,true);
		addMember(l,"Minutes",get_Minutes,null,true);
		addMember(l,"Seconds",get_Seconds,null,true);
		addMember(l,"Ticks",get_Ticks,null,true);
		addMember(l,"TotalDays",get_TotalDays,null,true);
		addMember(l,"TotalHours",get_TotalHours,null,true);
		addMember(l,"TotalMilliseconds",get_TotalMilliseconds,null,true);
		addMember(l,"TotalMinutes",get_TotalMinutes,null,true);
		addMember(l,"TotalSeconds",get_TotalSeconds,null,true);
		createTypeMetatable(l,constructor, typeof(System.TimeSpan),typeof(System.ValueType));
	}
}
