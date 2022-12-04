require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(AiCommand) {
	static_methods {
		NotifyAiDeath = deffunc(0)args(npc){
			local(view); view = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.EntityViewModelManager, "Instance"), GameFramework.EntityViewModelManager, "GetEntityViewById", callexterninstance(npc, GameFramework.EntityInfo, "GetId"));
			callexterninstance(view, GameFramework.EntityViewModel, "Death");
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		AiTarget = deffunc(0)args(npc, target){
			if( execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 18_8_26_9 ){
				if( execbinary("!=", null, getexterninstance(SymbolKind.Property, getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "SelectedTarget"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), 19_12_24_13 ){
					local(curTarget); curTarget = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "SelectedTarget"), GameFramework.LockTargetInfo, "TargetId"));
					if( execbinary("==", curTarget, getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "SelectedTarget"), GameFramework.LockTargetInfo, "Target"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), 21_16_23_17 ){
						return();
					};
				};
				callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "SetLockTarget", callexterninstance(target, GameFramework.EntityInfo, "GetId"));
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(target, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		AiFace = deffunc(0)args(npc){
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 30_8_34_9 ){
				local(dir); dir = callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetFaceDir");
				local(actor); actor = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.EntityViewModelManager, "Instance"), GameFramework.EntityViewModelManager, "GetGameObject", callexterninstance(npc, GameFramework.EntityInfo, "GetId"));
				getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, actor, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "localRotation") = callexternstaticreturnstruct(UnityEngine.Quaternion, "Euler__Single__Single__Single", 0, callexternstatic(GameFramework.Geometry, "RadianToDegree", dir), 0);
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		AiGetValidPosition = deffunc(1)args(npc, target, maxDistance){
			local(__method_ret_36_4_44_5);
			local(navMeshHit);
			local(__invoke_39_8_39_132); multiassign(precode{
				},postcode{
					navMeshHit = wrapexternstruct(navMeshHit, UnityEngine.AI.NavMeshHit);
				})varlist(__invoke_39_8_39_132, navMeshHit) = callexternstatic(UnityEngine.AI.NavMesh, "SamplePosition__Vector3__O_NavMeshHit__Single__Int32", newexternstruct(UnityEngine.Vector3, "g_UnityEngine_Vector3", typeargs(), typekinds(), "ctor__Single__Single__Single", 0, null, getexterninstance(SymbolKind.Field, target, ScriptRuntime.Vector3, "X"), getexterninstance(SymbolKind.Field, target, ScriptRuntime.Vector3, "Y"), getexterninstance(SymbolKind.Field, target, ScriptRuntime.Vector3, "Z")), __cs2dsl_out, maxDistance, -1);
			if( execbinary("&&", execbinary("&&", execunary("!", callexternstatic(System.Single, "IsInfinity", getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, navMeshHit, UnityEngine.AI.NavMeshHit, "position"), UnityEngine.Vector3, "x")), System.Boolean, TypeKind.Structure), execunary("!", callexternstatic(System.Single, "IsInfinity", getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, navMeshHit, UnityEngine.AI.NavMeshHit, "position"), UnityEngine.Vector3, "y")), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), execunary("!", callexternstatic(System.Single, "IsInfinity", getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, navMeshHit, UnityEngine.AI.NavMeshHit, "position"), UnityEngine.Vector3, "z")), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 40_8_42_9 ){
				__method_ret_36_4_44_5 = newexternstruct(ScriptRuntime.Vector3, "g_ScriptRuntime_Vector3", typeargs(), typekinds(), "ctor__Single__Single__Single", 0, null, getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, navMeshHit, UnityEngine.AI.NavMeshHit, "position"), UnityEngine.Vector3, "x"), getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, navMeshHit, UnityEngine.AI.NavMeshHit, "position"), UnityEngine.Vector3, "y"), getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, navMeshHit, UnityEngine.AI.NavMeshHit, "position"), UnityEngine.Vector3, "z"));
				return(__method_ret_36_4_44_5);
			};
			__method_ret_36_4_44_5 = target;
			return(__method_ret_36_4_44_5);
		}options[needfuncinfo(true), rettype(return, ScriptRuntime.Vector3, TypeKind.Structure, 0, true), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(target, ScriptRuntime.Vector3, TypeKind.Structure, 0, true), paramtype(maxDistance, System.Single, TypeKind.Structure, 0, true)];
		AiPursue = deffunc(0)args(npc, target){
			local(npcView); npcView = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.EntityViewModelManager, "Instance"), GameFramework.EntityViewModelManager, "GetEntityViewById", callexterninstance(npc, GameFramework.EntityInfo, "GetId"));
			callexterninstance(npcView, GameFramework.EntityViewModel, "MoveTo", getexterninstance(SymbolKind.Field, target, ScriptRuntime.Vector3, "X"), getexterninstance(SymbolKind.Field, target, ScriptRuntime.Vector3, "Y"), getexterninstance(SymbolKind.Field, target, ScriptRuntime.Vector3, "Z"));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(target, ScriptRuntime.Vector3, TypeKind.Structure, 0, true)];
		AiStopPursue = deffunc(0)args(npc){
			local(npcView); npcView = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.EntityViewModelManager, "Instance"), GameFramework.EntityViewModelManager, "GetEntityViewById", callexterninstance(npc, GameFramework.EntityInfo, "GetId"));
			callexterninstance(npcView, GameFramework.EntityViewModel, "StopMove");
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		AiSelectSkill = deffunc(0)args(npc, skill){
			if( execbinary("==", skill, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 57_8_60_67 ){
				callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "SetCurSkillInfo", 0);
			}else{
				callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "SetCurSkillInfo", getexterninstance(SymbolKind.Field, skill, GameFramework.SkillInfo, "SkillId"));
			};
			callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.Utility, "EventSystem"), GameFramework.PublishSubscribeSystem, "Publish", dslstrtocsstr("update_debug_state"), dslstrtocsstr("ui"), execbinary("+", "try use skill:", getexterninstance(SymbolKind.Field, skill, GameFramework.SkillInfo, "SkillId"), System.String, System.Object, TypeKind.Class, TypeKind.Class));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(skill, GameFramework.SkillInfo, TypeKind.Class, 0, true)];
		AiSkill = deffunc(0)args(npc, skillId){
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 66_8_76_9 ){
				comments {					comment("            if (npc.GetAiStateInfo().Target > 0) {");
					comment("                PluginFramework.Instance.SetLockTarget(npc.GetAiStateInfo().Target);");
					comment("            }");
					comment("            ");
				};
				local(skillInfo); skillInfo = callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "GetSkillInfoById", skillId);
				if( execbinary("!=", null, skillInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 73_12_75_13 ){
					callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.Skill.GfxSkillSystem, "Instance"), GameFramework.Skill.GfxSkillSystem, "StartSkill", callexterninstance(npc, GameFramework.EntityInfo, "GetId"), getexterninstance(SymbolKind.Field, skillInfo, GameFramework.SkillInfo, "ConfigData"), 0);
				};
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(skillId, System.Int32, TypeKind.Structure, 0, true)];
		AiStopSkill = deffunc(0)args(npc){
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 80_8_82_9 ){
				callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.Skill.GfxSkillSystem, "Instance"), GameFramework.Skill.GfxSkillSystem, "StopAllSkill__Int32__Boolean", callexterninstance(npc, GameFramework.EntityInfo, "GetId"), false);
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		AiAddImpact = deffunc(0)args(npc, impactId){
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(impactId, System.Int32, TypeKind.Structure, 0, true)];
		AiRemoveImpact = deffunc(0)args(npc, impactId){
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(impactId, System.Int32, TypeKind.Structure, 0, true)];
		AiSendStoryMessage = deffunc(0)args(npc, msgId, ...){
			local(args); args = params(System.Object, TypeKind.Class);
			callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.Story.GfxStorySystem, "Instance"), GameFramework.Story.GfxStorySystem, "SendMessage__String__BoxedValue", msgId, invokeexternoperatorreturnstruct(BoxedValue, BoxedValue, "op_Implicit__BoxedValue__A_Object", dsltoobject(SymbolKind.Method, true, "BoxedValue:op_Implicit", args)));
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(msgId, System.String, TypeKind.Class, 0, true), paramtype(..., System.Object, TypeKind.Array, 0, true)];
		cctor = deffunc(0)args(){
			callstatic(AiCommand, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiCommand.__cctor_called){
				return();
			}else{
				AiCommand.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

};



