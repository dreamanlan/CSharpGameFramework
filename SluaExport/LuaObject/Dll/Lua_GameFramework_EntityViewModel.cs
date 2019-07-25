using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_EntityViewModel : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.EntityViewModel o;
			o=new GameFramework.EntityViewModel();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToggleMaskEffect(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.ToggleMaskEffect(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Hide(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.Hide(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetRedEdge(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			self.SetRedEdge(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UpdateEdgeColor(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.UpdateEdgeColor();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Create(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			self.Create(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Destroy(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.Destroy();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Update(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.Update();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SyncSpatial(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.SyncSpatial();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SyncFaceDir(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.SyncFaceDir();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SyncPosition(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.SyncPosition();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Death(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.Death();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetMoveAgentEnable(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.SetMoveAgentEnable(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MoveTo(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			self.MoveTo(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StopMove(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			self.StopMove();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PlayAnimation(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.PlayAnimation(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_StandAnim(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.EntityViewModel.c_StandAnim);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_MoveAnim(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.EntityViewModel.c_MoveAnim);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_CrossFadeTime(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.EntityViewModel.c_CrossFadeTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Actor(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Actor);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ObjId(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ObjId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Visible(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Visible);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Visible(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.Visible=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Entity(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Entity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Agent(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Agent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Animator(IntPtr l) {
		try {
			GameFramework.EntityViewModel self=(GameFramework.EntityViewModel)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Animator);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.EntityViewModel");
		addMember(l,ToggleMaskEffect);
		addMember(l,Hide);
		addMember(l,SetRedEdge);
		addMember(l,UpdateEdgeColor);
		addMember(l,Create);
		addMember(l,Destroy);
		addMember(l,Update);
		addMember(l,SyncSpatial);
		addMember(l,SyncFaceDir);
		addMember(l,SyncPosition);
		addMember(l,Death);
		addMember(l,SetMoveAgentEnable);
		addMember(l,MoveTo);
		addMember(l,StopMove);
		addMember(l,PlayAnimation);
		addMember(l,"c_StandAnim",get_c_StandAnim,null,false);
		addMember(l,"c_MoveAnim",get_c_MoveAnim,null,false);
		addMember(l,"c_CrossFadeTime",get_c_CrossFadeTime,null,false);
		addMember(l,"Actor",get_Actor,null,true);
		addMember(l,"ObjId",get_ObjId,null,true);
		addMember(l,"Visible",get_Visible,set_Visible,true);
		addMember(l,"Entity",get_Entity,null,true);
		addMember(l,"Agent",get_Agent,null,true);
		addMember(l,"Animator",get_Animator,null,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.EntityViewModel));
	}
}
