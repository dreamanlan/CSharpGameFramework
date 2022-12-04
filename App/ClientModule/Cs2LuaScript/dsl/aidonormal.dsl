require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("aicommand");
require("ailogicutility");

class(AiDoNormal) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiDoNormal, "g_AiDoNormal", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiDoNormal, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiDoNormal.__cctor_called){
				return();
			}else{
				AiDoNormal.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		c_IntervalTime = 200;
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		Clone = deffunc(1)args(this){
			local(__method_ret_13_4_16_5);
			__method_ret_13_4_16_5 = newobject(AiDoNormal, "g_AiDoNormal", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_13_4_16_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryCommandPlugin, TypeKind.Interface, 0, true)];
		ResetState = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, AiDoNormal, "m_ParamReaded", false);
			setinstance(SymbolKind.Field, this, AiDoNormal, "m_EnableLearning", false);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		ExecCommand = deffunc(1)args(this, instance, handler, _params, delta){
			local(__method_ret_24_4_48_5);
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			if( execunary("!", getinstance(SymbolKind.Field, this, AiDoNormal, "m_ParamReaded"), System.Boolean, TypeKind.Structure), 27_8_33_9 ){
				setinstance(SymbolKind.Field, this, AiDoNormal, "m_ParamReaded", true);
				local(__old_val_29_12_29_29); __old_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiDoNormal, "m_ObjId");
				setinstance(SymbolKind.Field, this, AiDoNormal, "m_ObjId", invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0)));
				local(__new_val_29_12_29_29); __new_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiDoNormal, "m_ObjId");
				recycleandkeepstructvalue(System.Int32, __old_val_29_12_29_29, __new_val_29_12_29_29);
				if( execbinary(">", getexterninstance(SymbolKind.Property, args, System.Collections.Generic.List_T, "Count"), 1, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 30_12_32_13 ){
					setinstance(SymbolKind.Field, this, AiDoNormal, "m_EnableLearning", execbinary("!=", callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "GetInt"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure));
				};
			};
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiDoNormal, "m_ObjId"));
			if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(npc, GameFramework.EntityInfo, "IsUnderControl"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 35_8_46_9 ){
				local(info); info = callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo");
				local(__switch_37_12_45_13); __switch_37_12_45_13 = getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "CurState");
				if( __switch_37_12_45_13 == 1 ){
					callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 101);
					__method_ret_24_4_48_5 = true;
					return(__method_ret_24_4_48_5);
				}elseif( __switch_37_12_45_13 == 101 ){
					__method_ret_24_4_48_5 = callinstance(this, AiDoNormal, "CombatHandler", npc, info, delta);
					return(__method_ret_24_4_48_5);
				}elseif( __switch_37_12_45_13 == 102 ){
					__method_ret_24_4_48_5 = callinstance(this, AiDoNormal, "GohomeHandler", npc, info, delta);
					return(__method_ret_24_4_48_5);
				};
			};
			__method_ret_24_4_48_5 = false;
			return(__method_ret_24_4_48_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true)];
		CombatHandler = deffunc(1)args(this, npc, info, deltaTime){
			local(__method_ret_50_4_115_5);
			setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", execbinary("+", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), deltaTime, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
			if( execbinary(">", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), 100, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 53_8_57_9 ){
				setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", 0);
			}else{
				__method_ret_50_4_115_5 = true;
				return(__method_ret_50_4_115_5);
			};
			if( callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "IsSkillActivated"), 59_8_61_9 ){
				__method_ret_50_4_115_5 = true;
				return(__method_ret_50_4_115_5);
			};
			local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
			local(distSqrToHome); distSqrToHome = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), getexterninstancestructmember(SymbolKind.Property, info, GameFramework.AiStateInfo, "HomePos"));
			if( execbinary(">", distSqrToHome, execbinary("*", getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "GohomeRange"), getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "GohomeRange"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 65_8_69_9 ){
				callstatic(AiCommand, "AiStopPursue", npc);
				callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 102);
				__method_ret_50_4_115_5 = true;
				return(__method_ret_50_4_115_5);
			};
			local(attackTarget); attackTarget = null;
			local(currSkInfo); currSkInfo = callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo");
			local(skInfo); skInfo = callstatic(AiLogicUtility, "NpcFindCanUseSkill", npc);
			callstatic(AiCommand, "AiSelectSkill", npc, skInfo);
			if( execbinary("==", skInfo, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 77_8_81_9 ){
				callstatic(AiCommand, "AiStopPursue", npc);
				callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
				__method_ret_50_4_115_5 = false;
				return(__method_ret_50_4_115_5);
			};
			local(relation); relation = condexpfunc(true, retval_83_37_85_48, condexp_83_37_85_48, AiDoNormal, false, skInfo){condexp(execbinary("==", getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Field, skInfo, GameFramework.SkillInfo, "ConfigData"), TableConfig.Skill, "targetType"), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), true, 1, true, 0);}options[needfuncinfo(true), rettype(return, GameFramework.CharacterRelation, TypeKind.Enum, 0, true), paramtype(skInfo, GameFramework.SkillInfo, Class, 0, true)];
			attackTarget = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__Single__CharacterRelation", npc, getexterninstance(SymbolKind.Property, skInfo, GameFramework.SkillInfo, "Distance"), relation);
			if( execbinary("&&", execbinary("!=", attackTarget, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execbinary("!=", null, skInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 90_8_103_9 ){
				getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Target") = callexterninstance(attackTarget, GameFramework.EntityInfo, "GetId");
				local(targetPos); targetPos = callexterninstancereturnstruct(callexterninstance(attackTarget, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
				local(dir); dir = callexternstatic(GameFramework.Geometry, "GetYRadian__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
				local(curDir); curDir = callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetFaceDir");
				if( execbinary(">", callexternstatic(UnityEngine.Mathf, "Abs__Single", execbinary("-", dir, curDir, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure)), 0.15700000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 96_12_101_13 ){
					callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "SetWantedFaceDir", dir);
				}else{
					callstatic(AiCommand, "AiStopPursue", npc);
					callstatic(AiCommand, "AiSkill", npc, getexterninstance(SymbolKind.Field, skInfo, GameFramework.SkillInfo, "SkillId"));
					comment("攻击目标");
				};
				__method_ret_50_4_115_5 = true;
				return(__method_ret_50_4_115_5);
			};
			attackTarget = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__Single__CharacterRelation", npc, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ViewRange"), relation);
			if( execbinary("!=", attackTarget, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 106_8_110_9 ){
				callstatic(AiCommand, "AiPursue", npc, callexterninstancereturnstruct(callexterninstance(attackTarget, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"));
				comment(" 追赶目标");
				__method_ret_50_4_115_5 = true;
				return(__method_ret_50_4_115_5);
			};
			callexterninstance(currSkInfo, GameFramework.SkillStateInfo, "SetCurSkillInfo", 0);
			callstatic(AiCommand, "AiStopPursue", npc);
			__method_ret_50_4_115_5 = true;
			return(__method_ret_50_4_115_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(info, GameFramework.AiStateInfo, TypeKind.Class, 0, true), paramtype(deltaTime, System.Int64, TypeKind.Structure, 0, true)];
		GohomeHandler = deffunc(1)args(this, npc, info, deltaTime){
			local(__method_ret_117_4_140_5);
			setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", execbinary("+", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), deltaTime, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
			if( execbinary(">", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), 100, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 120_8_124_9 ){
				setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", 0);
			}else{
				__method_ret_117_4_140_5 = true;
				return(__method_ret_117_4_140_5);
			};
			local(targetPos); targetPos = getexterninstancestructmember(SymbolKind.Property, info, GameFramework.AiStateInfo, "HomePos");
			local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
			local(distSqr); distSqr = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), getexterninstancestructmember(SymbolKind.Property, info, GameFramework.AiStateInfo, "HomePos"));
			if( execbinary("<=", distSqr, 1, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 129_8_138_9 ){
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving", false);
				callstatic(AiCommand, "AiStopPursue", npc);
				callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
				__method_ret_117_4_140_5 = false;
				return(__method_ret_117_4_140_5);
			}else{
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving", true);
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "TargetPosition", targetPos);
				callstatic(AiCommand, "AiPursue", npc, wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
			};
			__method_ret_117_4_140_5 = true;
			return(__method_ret_117_4_140_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(info, GameFramework.AiStateInfo, TypeKind.Class, 0, true), paramtype(deltaTime, System.Int64, TypeKind.Structure, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiDoNormal, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiDoNormal, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiDoNormal, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_ObjId = 0;
		m_EnableLearning = false;
		m_ParamReaded = false;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {
		"GameFramework.Plugin.ISimpleStoryCommandPlugin";
	};

	class_info(TypeKind.Class, Accessibility.Public) {
	};
	method_info {
		Clone(MethodKind.Ordinary, Accessibility.Public){
		};
		ResetState(MethodKind.Ordinary, Accessibility.Public){
		};
		ExecCommand(MethodKind.Ordinary, Accessibility.Public){
		};
		CombatHandler(MethodKind.Ordinary, Accessibility.Private){
		};
		GohomeHandler(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



