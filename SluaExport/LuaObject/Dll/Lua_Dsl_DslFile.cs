using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_DslFile : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			Dsl.DslFile o;
			o=new Dsl.DslFile();
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
	static public int AddDslInfo(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.AddDslInfo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Load(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			Dsl.DslLogDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.Load(a1,a2);
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
	static public int LoadFromString(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			Dsl.DslLogDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			var ret=self.LoadFromString(a1,a2,a3);
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
	static public int Save(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Save(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			var ret=self.ToScriptString();
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
	static public int SaveBinaryFile(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SaveBinaryFile(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadLua(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			Dsl.DslLogDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.LoadLua(a1,a2);
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
	static public int LoadLuaFromString(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			Dsl.DslLogDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			var ret=self.LoadLuaFromString(a1,a2,a3);
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
	static public int LoadCpp(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			Dsl.DslLogDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.LoadCpp(a1,a2);
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
	static public int LoadCppFromString(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			Dsl.DslLogDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			var ret=self.LoadCppFromString(a1,a2,a3);
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
	static public int LoadGpp__String__DslLogDelegation(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			Dsl.DslLogDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.LoadGpp(a1,a2);
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
	static public int LoadGpp__String__DslLogDelegation__String__String(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			Dsl.DslLogDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			var ret=self.LoadGpp(a1,a2,a3,a4);
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
	static public int LoadGpp__String__DslLogDelegation__String__String__O_String(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			Dsl.DslLogDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.String a5;
			var ret=self.LoadGpp(a1,a2,a3,a4,out a5);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a5);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadGppFromString__String__String__DslLogDelegation(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			Dsl.DslLogDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			var ret=self.LoadGppFromString(a1,a2,a3);
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
	static public int LoadGppFromString__String__String__DslLogDelegation__String__String__O_String(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			Dsl.DslLogDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.String a5;
			checkType(l,6,out a5);
			System.String a6;
			var ret=self.LoadGppFromString(a1,a2,a3,a4,a5,out a6);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a6);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetStringDelimiter(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.SetStringDelimiter(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetScriptDelimiter(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.SetScriptDelimiter(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsBinaryDsl_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=Dsl.DslFile.IsBinaryDsl(a1,a2);
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
	static public int Mac2Unix_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dsl.DslFile.Mac2Unix(a1);
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
	static public int Text2Dos_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dsl.DslFile.Text2Dos(a1);
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
	static public int Text2Unix_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dsl.DslFile.Text2Unix(a1);
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
	static public int Text2Mac_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=Dsl.DslFile.Text2Mac(a1);
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
	static public int get_c_BinaryIdentity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.DslFile.c_BinaryIdentity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StringBeginDelimiter(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StringBeginDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StringEndDelimiter(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StringEndDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ScriptBeginDelimiter(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ScriptBeginDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ScriptEndDelimiter(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ScriptEndDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onGetToken(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			Dsl.Common.GetTokenDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onGetToken=v;
			else if(op==1) self.onGetToken+=v;
			else if(op==2) self.onGetToken-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onBeforeAddFunction(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			Dsl.Common.BeforeAddFunctionDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onBeforeAddFunction=v;
			else if(op==1) self.onBeforeAddFunction+=v;
			else if(op==2) self.onBeforeAddFunction-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onAddFunction(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			Dsl.Common.AddFunctionDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onAddFunction=v;
			else if(op==1) self.onAddFunction+=v;
			else if(op==2) self.onAddFunction-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onBeforeEndStatement(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			Dsl.Common.BeforeEndStatementDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onBeforeEndStatement=v;
			else if(op==1) self.onBeforeEndStatement+=v;
			else if(op==2) self.onBeforeEndStatement-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onEndStatement(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			Dsl.Common.EndStatementDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onEndStatement=v;
			else if(op==1) self.onEndStatement+=v;
			else if(op==2) self.onEndStatement-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BinaryIdentity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.DslFile.BinaryIdentity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DontLoadComments(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.DslFile.DontLoadComments);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DontLoadComments(IntPtr l) {
		try {
			bool v;
			checkType(l,2,out v);
			Dsl.DslFile.DontLoadComments=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.DslFile");
		addMember(l,ctor_s);
		addMember(l,AddDslInfo);
		addMember(l,Load);
		addMember(l,LoadFromString);
		addMember(l,Save);
		addMember(l,ToScriptString);
		addMember(l,SaveBinaryFile);
		addMember(l,LoadLua);
		addMember(l,LoadLuaFromString);
		addMember(l,LoadCpp);
		addMember(l,LoadCppFromString);
		addMember(l,LoadGpp__String__DslLogDelegation);
		addMember(l,LoadGpp__String__DslLogDelegation__String__String);
		addMember(l,LoadGpp__String__DslLogDelegation__String__String__O_String);
		addMember(l,LoadGppFromString__String__String__DslLogDelegation);
		addMember(l,LoadGppFromString__String__String__DslLogDelegation__String__String__O_String);
		addMember(l,SetStringDelimiter);
		addMember(l,SetScriptDelimiter);
		addMember(l,IsBinaryDsl_s);
		addMember(l,Mac2Unix_s);
		addMember(l,Text2Dos_s);
		addMember(l,Text2Unix_s);
		addMember(l,Text2Mac_s);
		addMember(l,"c_BinaryIdentity",get_c_BinaryIdentity,null,false);
		addMember(l,"StringBeginDelimiter",get_StringBeginDelimiter,null,true);
		addMember(l,"StringEndDelimiter",get_StringEndDelimiter,null,true);
		addMember(l,"ScriptBeginDelimiter",get_ScriptBeginDelimiter,null,true);
		addMember(l,"ScriptEndDelimiter",get_ScriptEndDelimiter,null,true);
		addMember(l,"onGetToken",null,set_onGetToken,true);
		addMember(l,"onBeforeAddFunction",null,set_onBeforeAddFunction,true);
		addMember(l,"onAddFunction",null,set_onAddFunction,true);
		addMember(l,"onBeforeEndStatement",null,set_onBeforeEndStatement,true);
		addMember(l,"onEndStatement",null,set_onEndStatement,true);
		addMember(l,"BinaryIdentity",get_BinaryIdentity,null,false);
		addMember(l,"DontLoadComments",get_DontLoadComments,set_DontLoadComments,false);
		createTypeMetatable(l,null, typeof(Dsl.DslFile));
	}
}
