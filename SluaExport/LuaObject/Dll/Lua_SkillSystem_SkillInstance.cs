using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_SkillInstance : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.SkillInstance o;
			o=new SkillSystem.SkillInstance();
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
	static public int SetVariable(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SetVariable(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clone(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			var ret=self.Clone();
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
	static public int Init__ISyntaxComponent(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			var ret=self.Init(a1);
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
	static public int Init__FunctionData(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			var ret=self.Init(a1);
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
	static public int ToScriptString(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
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
	static public int Save(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
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
	static public int Reset(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Start(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.Start(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessage(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SendMessage(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Tick(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			self.Tick(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnInterrupt(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.OnInterrupt(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnSkillStop(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.OnSkillStop(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsMessageDone(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			var ret=self.IsMessageDone();
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
	static public int SetCurSectionDuration(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			self.SetCurSectionDuration(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AdjustCurSectionDuration(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			self.AdjustCurSectionDuration(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopCurSection(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			self.StopCurSection();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddUseImpactForInit(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			SkillSystem.ISkillTriger a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			self.AddUseImpactForInit(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddImpactForInit__ISkillTriger(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			SkillSystem.ISkillTriger a1;
			checkType(l,2,out a1);
			self.AddImpactForInit(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddImpactForInit__ISkillTriger__Int32__Boolean(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			SkillSystem.ISkillTriger a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			self.AddImpactForInit(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddDamageForInit(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			SkillSystem.ISkillTriger a1;
			checkType(l,2,out a1);
			self.AddDamageForInit(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GenInnerEmitSkillId_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=SkillSystem.SkillInstance.GenInnerEmitSkillId(a1);
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
	static public int GenInnerHitSkillId_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=SkillSystem.SkillInstance.GenInnerHitSkillId(a1);
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
	static public int get_c_FirstInnerEmitSkillId(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,SkillSystem.SkillInstance.c_FirstInnerEmitSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_FirstInnerHitSkillId(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,SkillSystem.SkillInstance.c_FirstInnerHitSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_InnerDslSkillId(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.InnerDslSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_OuterDslSkillId(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OuterDslSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DslSkillId(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DslSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_DslSkillId(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.DslSkillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Context(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Context);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Context(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.Context=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsInterrupted(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsInterrupted);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsInterrupted(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsInterrupted=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsFinished(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFinished);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsFinished(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsFinished=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CurTime(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CurSection(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurSection);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CurSectionDuration(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurSectionDuration);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_OriginalDelta(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OriginalDelta);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TimeScale(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TimeScale);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TimeScale(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.TimeScale=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EffectScale(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EffectScale);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_EffectScale(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.EffectScale=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MoveScale(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MoveScale);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MoveScale(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.MoveScale=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GoToSection(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GoToSection);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_GoToSection(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.GoToSection=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Variables(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Variables);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CustomDatas(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CustomDatas);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EmitSkillInstances(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EmitSkillInstances);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_HitSkillInstances(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HitSkillInstances);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ImpactCount(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImpactCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_DamageCount(IntPtr l) {
		try {
			SkillSystem.SkillInstance self=(SkillSystem.SkillInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DamageCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillInstance");
		addMember(l,ctor_s);
		addMember(l,SetVariable);
		addMember(l,Clone);
		addMember(l,Init__ISyntaxComponent);
		addMember(l,Init__FunctionData);
		addMember(l,ToScriptString);
		addMember(l,Save);
		addMember(l,Reset);
		addMember(l,Start);
		addMember(l,SendMessage);
		addMember(l,Tick);
		addMember(l,OnInterrupt);
		addMember(l,OnSkillStop);
		addMember(l,IsMessageDone);
		addMember(l,SetCurSectionDuration);
		addMember(l,AdjustCurSectionDuration);
		addMember(l,StopCurSection);
		addMember(l,AddUseImpactForInit);
		addMember(l,AddImpactForInit__ISkillTriger);
		addMember(l,AddImpactForInit__ISkillTriger__Int32__Boolean);
		addMember(l,AddDamageForInit);
		addMember(l,GenInnerEmitSkillId_s);
		addMember(l,GenInnerHitSkillId_s);
		addMember(l,"c_FirstInnerEmitSkillId",get_c_FirstInnerEmitSkillId,null,false);
		addMember(l,"c_FirstInnerHitSkillId",get_c_FirstInnerHitSkillId,null,false);
		addMember(l,"InnerDslSkillId",get_InnerDslSkillId,null,true);
		addMember(l,"OuterDslSkillId",get_OuterDslSkillId,null,true);
		addMember(l,"DslSkillId",get_DslSkillId,set_DslSkillId,true);
		addMember(l,"Context",get_Context,set_Context,true);
		addMember(l,"IsInterrupted",get_IsInterrupted,set_IsInterrupted,true);
		addMember(l,"IsFinished",get_IsFinished,set_IsFinished,true);
		addMember(l,"CurTime",get_CurTime,null,true);
		addMember(l,"CurSection",get_CurSection,null,true);
		addMember(l,"CurSectionDuration",get_CurSectionDuration,null,true);
		addMember(l,"OriginalDelta",get_OriginalDelta,null,true);
		addMember(l,"TimeScale",get_TimeScale,set_TimeScale,true);
		addMember(l,"EffectScale",get_EffectScale,set_EffectScale,true);
		addMember(l,"MoveScale",get_MoveScale,set_MoveScale,true);
		addMember(l,"GoToSection",get_GoToSection,set_GoToSection,true);
		addMember(l,"Variables",get_Variables,null,true);
		addMember(l,"CustomDatas",get_CustomDatas,null,true);
		addMember(l,"EmitSkillInstances",get_EmitSkillInstances,null,true);
		addMember(l,"HitSkillInstances",get_HitSkillInstances,null,true);
		addMember(l,"ImpactCount",get_ImpactCount,null,true);
		addMember(l,"DamageCount",get_DamageCount,null,true);
		createTypeMetatable(l,null, typeof(SkillSystem.SkillInstance));
	}
}
