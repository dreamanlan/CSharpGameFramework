using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_StoryDlg : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.StoryDlg o;
			o=new TableConfig.StoryDlg();
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
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
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
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
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
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
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
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
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
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
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
	static public int get_dialogId(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dialogId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_dialogId(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.dialogId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_index(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.index);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_index(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.index=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_speaker(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.speaker);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_speaker(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.speaker=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_leftOrRight(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.leftOrRight);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_leftOrRight(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.leftOrRight=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_dialog(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dialog);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_dialog(IntPtr l) {
		try {
			TableConfig.StoryDlg self=(TableConfig.StoryDlg)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.dialog=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.StoryDlg");
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,GetId);
		addMember(l,"id",get_id,set_id,true);
		addMember(l,"dialogId",get_dialogId,set_dialogId,true);
		addMember(l,"index",get_index,set_index,true);
		addMember(l,"speaker",get_speaker,set_speaker,true);
		addMember(l,"leftOrRight",get_leftOrRight,set_leftOrRight,true);
		addMember(l,"dialog",get_dialog,set_dialog,true);
		createTypeMetatable(l,constructor, typeof(TableConfig.StoryDlg));
	}
}
