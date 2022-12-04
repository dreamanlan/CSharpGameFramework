using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TableConfig_Formation : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			TableConfig.Formation o;
			o=new TableConfig.Formation();
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
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
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
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
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
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
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
	static public int GetPosDir(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetPosDir(a1);
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
	static public int BuildFormationInfo(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			self.BuildFormationInfo();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_teamid(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.teamid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_teamid(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.teamid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir0(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir0);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir0(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir0=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir1(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir1);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir1(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir1=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir2(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir2(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir2=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir3(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir3);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir3(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir3=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir4(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir4);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir4(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir4=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir5(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir5(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir5=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir6(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir6);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir6(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir6=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir7(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir7);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir7(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir7=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir8(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir8);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir8(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir8=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir9(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir9);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir9(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir9=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir10(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir10);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir10(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir10=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir11(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir11);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir11(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir11=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir12(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir12);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir12(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir12=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir13(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir13);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir13(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir13=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir14(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir14);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir14(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir14=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dir15(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dir15);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dir15(IntPtr l) {
		try {
			TableConfig.Formation self=(TableConfig.Formation)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.dir15=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.Formation");
		addMember(l,ctor_s);
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,GetId);
		addMember(l,GetPosDir);
		addMember(l,BuildFormationInfo);
		addMember(l,"teamid",get_teamid,set_teamid,true);
		addMember(l,"dir0",get_dir0,set_dir0,true);
		addMember(l,"dir1",get_dir1,set_dir1,true);
		addMember(l,"dir2",get_dir2,set_dir2,true);
		addMember(l,"dir3",get_dir3,set_dir3,true);
		addMember(l,"dir4",get_dir4,set_dir4,true);
		addMember(l,"dir5",get_dir5,set_dir5,true);
		addMember(l,"dir6",get_dir6,set_dir6,true);
		addMember(l,"dir7",get_dir7,set_dir7,true);
		addMember(l,"dir8",get_dir8,set_dir8,true);
		addMember(l,"dir9",get_dir9,set_dir9,true);
		addMember(l,"dir10",get_dir10,set_dir10,true);
		addMember(l,"dir11",get_dir11,set_dir11,true);
		addMember(l,"dir12",get_dir12,set_dir12,true);
		addMember(l,"dir13",get_dir13,set_dir13,true);
		addMember(l,"dir14",get_dir14,set_dir14,true);
		addMember(l,"dir15",get_dir15,set_dir15,true);
		createTypeMetatable(l,null, typeof(TableConfig.Formation));
	}
}
