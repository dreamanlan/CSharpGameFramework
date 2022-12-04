require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("ailogicutility");
require("aicommand");

class(AiDoMember) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiDoMember, "g_AiDoMember", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiDoMember, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiDoMember.__cctor_called){
				return();
			}else{
				AiDoMember.__cctor_called = true;
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
			__method_ret_13_4_16_5 = newobject(AiDoMember, "g_AiDoMember", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_13_4_16_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryCommandPlugin, TypeKind.Interface, 0, true)];
		ResetState = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, AiDoMember, "m_ParamReaded", false);
			setinstance(SymbolKind.Field, this, AiDoMember, "m_EnableLearning", false);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		ExecCommand = deffunc(1)args(this, instance, handler, _params, delta){
			local(__method_ret_24_4_48_5);
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			if( execunary("!", getinstance(SymbolKind.Field, this, AiDoMember, "m_ParamReaded"), System.Boolean, TypeKind.Structure), 27_8_33_9 ){
				setinstance(SymbolKind.Field, this, AiDoMember, "m_ParamReaded", true);
				local(__old_val_29_12_29_29); __old_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiDoMember, "m_ObjId");
				setinstance(SymbolKind.Field, this, AiDoMember, "m_ObjId", invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0)));
				local(__new_val_29_12_29_29); __new_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiDoMember, "m_ObjId");
				recycleandkeepstructvalue(System.Int32, __old_val_29_12_29_29, __new_val_29_12_29_29);
				if( execbinary(">", getexterninstance(SymbolKind.Property, args, System.Collections.Generic.List_T, "Count"), 1, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 30_12_32_13 ){
					setinstance(SymbolKind.Field, this, AiDoMember, "m_EnableLearning", execbinary("!=", callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "GetInt"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure));
				};
			};
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiDoMember, "m_ObjId"));
			if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(npc, GameFramework.EntityInfo, "IsUnderControl"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 35_8_46_9 ){
				local(info); info = callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo");
				local(__switch_37_12_45_13); __switch_37_12_45_13 = getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "CurState");
				if( __switch_37_12_45_13 == 1 ){
					callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 101);
					__method_ret_24_4_48_5 = true;
					return(__method_ret_24_4_48_5);
				}elseif( __switch_37_12_45_13 == 101 ){
					__method_ret_24_4_48_5 = callinstance(this, AiDoMember, "CombatHandler", npc, info, delta);
					return(__method_ret_24_4_48_5);
				}elseif( __switch_37_12_45_13 == 102 ){
					__method_ret_24_4_48_5 = callinstance(this, AiDoMember, "GohomeHandler", npc, info, delta);
					return(__method_ret_24_4_48_5);
				};
			};
			__method_ret_24_4_48_5 = false;
			return(__method_ret_24_4_48_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true)];
		CombatHandler = deffunc(1)args(this, npc, info, deltaTime){
			local(__method_ret_50_4_107_5);
			if( callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "IsSkillActivated"), 52_8_54_9 ){
				__method_ret_50_4_107_5 = true;
				return(__method_ret_50_4_107_5);
			};
			local(leader); leader = callstatic(AiLogicUtility, "GetLivingCharacterInfoHelper", npc, getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "LeaderId"));
			local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
			local(homePos); homePos = getexternstaticstructmember(SymbolKind.Property, ScriptRuntime.Vector3, "Zero");
			if( execbinary("!=", null, leader, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 59_8_61_9 ){
				homePos = callinstance(this, AiDoMember, "GetHomePos", getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "FormationIndex"), leader);
				homePos = wrapexternstruct(homePos, ScriptRuntime.Vector3);
			};
			local(distSqrToHome); distSqrToHome = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(homePos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
			if( execbinary(">", distSqrToHome, execbinary("*", getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "GohomeRange"), getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "GohomeRange"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 63_8_67_9 ){
				callstatic(AiCommand, "AiStopPursue", npc);
				callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 102);
				__method_ret_50_4_107_5 = true;
				return(__method_ret_50_4_107_5);
			};
			local(attackTarget); attackTarget = null;
			local(currSkInfo); currSkInfo = callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo");
			local(skInfo); skInfo = callstatic(AiLogicUtility, "NpcFindCanUseSkill", npc);
			callstatic(AiCommand, "AiSelectSkill", npc, skInfo);
			if( execbinary("==", skInfo, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 75_8_79_9 ){
				comment("没有可以使用的技能就切换到Idle状态");
				callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
				__method_ret_50_4_107_5 = false;
				return(__method_ret_50_4_107_5);
			};
			local(relation); relation = condexpfunc(true, retval_82_16_85_48, condexp_82_16_85_48, AiDoMember, false, skInfo){condexp(( execbinary("||", execbinary("==", getexterninstance(SymbolKind.Property, skInfo, GameFramework.SkillInfo, "TargetType"), 2, GameFramework.SkillTargetType, GameFramework.SkillTargetType, TypeKind.Enum, TypeKind.Enum), execbinary("==", getexterninstance(SymbolKind.Property, skInfo, GameFramework.SkillInfo, "TargetType"), 4, GameFramework.SkillTargetType, GameFramework.SkillTargetType, TypeKind.Enum, TypeKind.Enum), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure) ), true, 1, true, 0);}options[needfuncinfo(true), rettype(return, GameFramework.CharacterRelation, TypeKind.Enum, 0, true), paramtype(skInfo, GameFramework.SkillInfo, Class, 0, true)];
			attackTarget = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__Single__CharacterRelation", npc, getexterninstance(SymbolKind.Property, skInfo, GameFramework.SkillInfo, "Distance"), relation);
			if( execbinary("&&", execbinary("!=", attackTarget, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execbinary("!=", null, skInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 90_8_95_9 ){
				getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Target") = callexterninstance(attackTarget, GameFramework.EntityInfo, "GetId");
				callstatic(AiCommand, "AiStopPursue", npc);
				callstatic(AiCommand, "AiSkill", npc, getexterninstance(SymbolKind.Field, skInfo, GameFramework.SkillInfo, "SkillId"));
				comment("攻击目标");
				__method_ret_50_4_107_5 = true;
				return(__method_ret_50_4_107_5);
				comment("攻击范围内找到可攻击目标            ");
			};
			attackTarget = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__Single__CharacterRelation", npc, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ViewRange"), relation);
			if( execbinary("!=", attackTarget, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 98_8_101_9 ){
				callstatic(AiCommand, "AiPursue", npc, callexterninstancereturnstruct(callexterninstance(attackTarget, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"));
				comment(" 追赶目标");
				__method_ret_50_4_107_5 = true;
				return(__method_ret_50_4_107_5);
				comment("视野范围内找到可攻击目标");
			};
			callexterninstance(currSkInfo, GameFramework.SkillStateInfo, "SetCurSkillInfo", 0);
			callstatic(AiCommand, "AiStopPursue", npc);
			callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 102);
			__method_ret_50_4_107_5 = true;
			return(__method_ret_50_4_107_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(info, GameFramework.AiStateInfo, TypeKind.Class, 0, true), paramtype(deltaTime, System.Int64, TypeKind.Structure, 0, true)];
		GohomeHandler = deffunc(1)args(this, entity, info, deltaTime){
			local(__method_ret_108_4_134_5);
			setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", execbinary("+", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), deltaTime, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
			if( execbinary(">", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), 200, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 111_8_132_9 ){
				setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", 0);
				local(leader); leader = callstatic(AiLogicUtility, "GetLivingCharacterInfoHelper", entity, getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "LeaderId"));
				if( execbinary("!=", null, leader, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 115_12_131_13 ){
					local(minDist); minDist = execbinary("+", callexterninstance(entity, GameFramework.EntityInfo, "GetRadius"), callexterninstance(leader, GameFramework.EntityInfo, "GetRadius"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure);
					local(targetPos); targetPos = callinstance(this, AiDoMember, "GetHomePos", getexterninstance(SymbolKind.Property, callexterninstance(entity, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "FormationIndex"), leader);
					targetPos = wrapexternstruct(targetPos, ScriptRuntime.Vector3);
					local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(entity, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
					local(powDistToHome); powDistToHome = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
					if( execbinary("<=", powDistToHome, execbinary("*", ( execbinary("+", minDist, 1, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure) ), ( execbinary("+", minDist, 1, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure) ), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 120_16_126_17 ){
						callstatic(AiCommand, "AiStopPursue", entity);
						callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
						__method_ret_108_4_134_5 = false;
						return(__method_ret_108_4_134_5);
					}else{
						callstatic(AiCommand, "AiPursue", entity, wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
					};
				}else{
					callstatic(AiCommand, "AiStopPursue", entity);
					callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
					__method_ret_108_4_134_5 = false;
					return(__method_ret_108_4_134_5);
				};
			};
			__method_ret_108_4_134_5 = true;
			return(__method_ret_108_4_134_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(entity, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(info, GameFramework.AiStateInfo, TypeKind.Class, 0, true), paramtype(deltaTime, System.Int64, TypeKind.Structure, 0, true)];
		GetFormationId = deffunc(1)args(this, leader){
			local(__method_ret_135_4_142_5);
			local(ret); ret = 0;
			if( execbinary("!=", null, leader, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 138_8_140_9 ){
				ret = getexterninstance(SymbolKind.Property, callexterninstance(leader, GameFramework.EntityInfo, "GetAiStateInfo"), GameFramework.AiStateInfo, "FormationId");
			};
			__method_ret_135_4_142_5 = ret;
			return(__method_ret_135_4_142_5);
		}options[needfuncinfo(false), rettype(return, System.Int32, TypeKind.Structure, 0, true), paramtype(leader, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		GetHomePos = deffunc(1)args(this, formationIndex, leader){
			local(__method_ret_143_4_156_5);
			local(pos);
			local(id); id = callinstance(this, AiDoMember, "GetFormationId", leader);
			local(formation); formation = callexterninstance(getexternstatic(SymbolKind.Property, TableConfig.FormationProvider, "Instance"), TableConfig.FormationProvider, "GetFormation", id);
			if( execbinary("!=", null, formation, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 148_8_154_9 ){
				local(posDir); posDir = callexterninstancereturnstruct(formation, TableConfig.Formation, "GetPosDir", formationIndex);
				local(dir);
				multiassign(precode{
					},postcode{
					})varlist(pos, dir) = callexterninstancereturnstruct(posDir, TableConfig.Formation.PosDir, "CalcPosDir", getexterninstancestructmember(SymbolKind.Property, callexterninstance(leader, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "TargetPosition"), callexterninstance(callexterninstance(leader, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetFaceDir"), __cs2dsl_out);
			}else{
				pos = getexternstaticstructmember(SymbolKind.Property, ScriptRuntime.Vector3, "Zero");
			};
			__method_ret_143_4_156_5 = pos;
			return(__method_ret_143_4_156_5);
		}options[needfuncinfo(true), rettype(return, ScriptRuntime.Vector3, TypeKind.Structure, 0, true), paramtype(formationIndex, System.Int32, TypeKind.Structure, 0, true), paramtype(leader, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		IsLeaderDead = deffunc(1)args(this, entity){
			local(__method_ret_157_4_166_5);
			local(ret); ret = true;
			local(info); info = callexterninstance(entity, GameFramework.EntityInfo, "GetAiStateInfo");
			local(leader); leader = callstatic(AiLogicUtility, "GetLivingCharacterInfoHelper", entity, getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "LeaderId"));
			if( execbinary("!=", null, leader, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 162_8_164_9 ){
				ret = callexterninstance(leader, GameFramework.EntityInfo, "IsDead");
			};
			__method_ret_157_4_166_5 = ret;
			return(__method_ret_157_4_166_5);
		}options[needfuncinfo(false), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(entity, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiDoMember, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiDoMember, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiDoMember, "__ctor_called", true);
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
		GetFormationId(MethodKind.Ordinary, Accessibility.Private){
		};
		GetHomePos(MethodKind.Ordinary, Accessibility.Private){
		};
		IsLeaderDead(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



