using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_ResourceSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int InitGroup(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.InitGroup(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetGroupMaxCount(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.SetGroupMaxCount(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetGroupResourceCount(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGroupResourceCount(a1);
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
	static public int SetVisible(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			UnityEngine.CharacterController a3;
			checkType(l,4,out a3);
			self.SetVisible(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Init(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			self.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PreloadObject__String(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.PreloadObject(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PreloadObject__String__Int32(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.PreloadObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PreloadSharedResource(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.PreloadSharedResource(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CanNewObject(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.CanNewObject(a1);
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
	static public int NewObject__String(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.NewObject(a1);
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
	static public int NewObject__Object(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			var ret=self.NewObject(a1);
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
	static public int NewObject__String__Int32(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.NewObject(a1,a2);
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
	static public int NewObject__Object__Int32(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.NewObject(a1,a2);
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
	static public int NewObject__String__Single(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			var ret=self.NewObject(a1,a2);
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
	static public int NewObject__Object__Single(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			var ret=self.NewObject(a1,a2);
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
	static public int NewObject__String__Single__Int32(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.NewObject(a1,a2,a3);
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
	static public int NewObject__Object__Single__Int32(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.NewObject(a1,a2,a3);
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
	static public int RecycleObject(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			var ret=self.RecycleObject(a1);
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
	static public int Tick(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSharedResource(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetSharedResource(a1);
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
	static public int CleanupResourcePool(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			self.CleanupResourcePool();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ForceSetGameObjectLayer_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			GameFramework.ResourceSystem.ForceSetGameObjectLayer(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MaxSameUnusedObjectNum(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MaxSameUnusedObjectNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MaxSameUnusedObjectNum(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.MaxSameUnusedObjectNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MaxUnusedObjectNum(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MaxUnusedObjectNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MaxUnusedObjectNum(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.MaxUnusedObjectNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MaxUnuseTimeForCleanup(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MaxUnuseTimeForCleanup);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MaxUnuseTimeForCleanup(IntPtr l) {
		try {
			GameFramework.ResourceSystem self=(GameFramework.ResourceSystem)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.MaxUnuseTimeForCleanup=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.ResourceSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ResourceSystem");
		addMember(l,InitGroup);
		addMember(l,SetGroupMaxCount);
		addMember(l,GetGroupResourceCount);
		addMember(l,SetVisible);
		addMember(l,Init);
		addMember(l,PreloadObject__String);
		addMember(l,PreloadObject__String__Int32);
		addMember(l,PreloadSharedResource);
		addMember(l,CanNewObject);
		addMember(l,NewObject__String);
		addMember(l,NewObject__Object);
		addMember(l,NewObject__String__Int32);
		addMember(l,NewObject__Object__Int32);
		addMember(l,NewObject__String__Single);
		addMember(l,NewObject__Object__Single);
		addMember(l,NewObject__String__Single__Int32);
		addMember(l,NewObject__Object__Single__Int32);
		addMember(l,RecycleObject);
		addMember(l,Tick);
		addMember(l,GetSharedResource);
		addMember(l,CleanupResourcePool);
		addMember(l,ForceSetGameObjectLayer_s);
		addMember(l,"MaxSameUnusedObjectNum",get_MaxSameUnusedObjectNum,set_MaxSameUnusedObjectNum,true);
		addMember(l,"MaxUnusedObjectNum",get_MaxUnusedObjectNum,set_MaxUnusedObjectNum,true);
		addMember(l,"MaxUnuseTimeForCleanup",get_MaxUnuseTimeForCleanup,set_MaxUnuseTimeForCleanup,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.ResourceSystem));
	}
}
