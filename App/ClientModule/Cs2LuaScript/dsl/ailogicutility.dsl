require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(AiLogicUtility) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiLogicUtility, "g_AiLogicUtility", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		GetNearstTargetHelper__EntityInfo__CharacterRelation = deffunc(1)args(srcObj, relation){
			local(__method_ret_31_4_34_5);
			__method_ret_31_4_34_5 = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__CharacterRelation__AiTargetType", srcObj, relation, 3);
			return(__method_ret_31_4_34_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(srcObj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(relation, GameFramework.CharacterRelation, TypeKind.Enum, 0, true)];
		GetNearstTargetHelper__EntityInfo__Single__CharacterRelation = deffunc(1)args(srcObj, range, relation){
			local(__method_ret_36_4_39_5);
			__method_ret_36_4_39_5 = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__Single__CharacterRelation__AiTargetType", srcObj, range, relation, 3);
			return(__method_ret_36_4_39_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(srcObj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(range, System.Single, TypeKind.Structure, 0, true), paramtype(relation, GameFramework.CharacterRelation, TypeKind.Enum, 0, true)];
		GetNearstTargetHelper__EntityInfo__CharacterRelation__AiTargetType = deffunc(1)args(srcObj, relation, type){
			local(__method_ret_41_4_44_5);
			__method_ret_41_4_44_5 = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__Single__CharacterRelation__AiTargetType", srcObj, getexterninstance(SymbolKind.Property, srcObj, GameFramework.EntityInfo, "ViewRange"), relation, type);
			return(__method_ret_41_4_44_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(srcObj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(relation, GameFramework.CharacterRelation, TypeKind.Enum, 0, true), paramtype(type, AiTargetType, TypeKind.Enum, 0, false)];
		GetNearstTargetHelper__EntityInfo__Single__CharacterRelation__AiTargetType = deffunc(1)args(srcObj, range, relation, type){
			local(__method_ret_46_4_54_5);
			local(nearstTarget); nearstTarget = null;
			local(minDistSqr); minDistSqr = 999999;
			callexterninstance(getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, srcObj, GameFramework.EntityInfo, "SceneContext"), GameFramework.SceneContextInfo, "KdTree"), GameFramework.KdObjectTree, "QueryWithAction__EntityInfo__Single__MyAction_2_Single_KdTreeObject", srcObj, range, deffunc(0)args(distSqr, kdTreeObj){
				multiassign(precode{
					},postcode{
					})varlist(minDistSqr, nearstTarget) = callstatic(AiLogicUtility, "StepCalcNearstTarget", srcObj, relation, type, distSqr, getexterninstance(SymbolKind.Field, kdTreeObj, GameFramework.KdTreeObject, "Object"), minDistSqr, nearstTarget);
			}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(distSqr, System.Single, TypeKind.Structure, 0, true), paramtype(kdTreeObj, GameFramework.KdTreeObject, TypeKind.Class, 0, true)]);
			__method_ret_46_4_54_5 = nearstTarget;
			return(__method_ret_46_4_54_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(srcObj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(range, System.Single, TypeKind.Structure, 0, true), paramtype(relation, GameFramework.CharacterRelation, TypeKind.Enum, 0, true), paramtype(type, AiTargetType, TypeKind.Enum, 0, false)];
		GetLivingCharacterInfoHelper = deffunc(1)args(srcObj, id){
			local(__method_ret_56_4_64_5);
			local(target); target = callexterninstance(getexterninstance(SymbolKind.Property, srcObj, GameFramework.EntityInfo, "EntityManager"), GameFramework.EntityManager, "GetEntityInfo", id);
			if( execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 59_8_62_9 ){
				if( callexterninstance(target, GameFramework.EntityInfo, "IsDead"), 60_12_61_30 ){
					target = null;
				};
			};
			__method_ret_56_4_64_5 = target;
			return(__method_ret_56_4_64_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(srcObj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(id, System.Int32, TypeKind.Structure, 0, true)];
		GetSeeingLivingCharacterInfoHelper = deffunc(1)args(srcObj, id){
			local(__method_ret_66_4_74_5);
			local(target); target = callexterninstance(getexterninstance(SymbolKind.Property, srcObj, GameFramework.EntityInfo, "EntityManager"), GameFramework.EntityManager, "GetEntityInfo", id);
			if( execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 69_8_72_9 ){
				if( callexterninstance(target, GameFramework.EntityInfo, "IsDead"), 70_12_71_30 ){
					target = null;
				};
			};
			__method_ret_66_4_74_5 = target;
			return(__method_ret_66_4_74_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(srcObj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(id, System.Int32, TypeKind.Structure, 0, true)];
		StepCalcNearstTarget = deffunc(2)args(srcObj, relation, type, distSqr, obj, minDistSqr, nearstTarget){
			local(target); target = callstatic(AiLogicUtility, "GetSeeingLivingCharacterInfoHelper", srcObj, callexterninstance(obj, GameFramework.EntityInfo, "GetId"));
			if( execbinary("&&", execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(target, GameFramework.EntityInfo, "IsDead"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 79_8_101_9 ){
				if( execunary("!", callexterninstance(target, GameFramework.EntityInfo, "IsTargetNpc"), System.Boolean, TypeKind.Structure), 80_12_82_13 ){
					return(, minDistSqr, nearstTarget);
				};
				if( execbinary("&&", execbinary("==", type, 1, AiTargetType, AiTargetType, TypeKind.Enum, TypeKind.Enum), execbinary("!=", getexterninstance(SymbolKind.Property, target, GameFramework.EntityInfo, "EntityType"), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 83_12_85_13 ){
					return(, minDistSqr, nearstTarget);
				};
				if( execbinary("&&", execbinary("==", type, 2, AiTargetType, AiTargetType, TypeKind.Enum, TypeKind.Enum), execbinary("!=", getexterninstance(SymbolKind.Property, target, GameFramework.EntityInfo, "EntityType"), 3, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 86_12_88_13 ){
					return(, minDistSqr, nearstTarget);
				};
				if( execbinary("&&", execbinary("==", type, 0, AiTargetType, AiTargetType, TypeKind.Enum, TypeKind.Enum), execbinary("!=", getexterninstance(SymbolKind.Property, target, GameFramework.EntityInfo, "EntityType"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 89_12_91_13 ){
					return(, minDistSqr, nearstTarget);
				};
				if( execbinary("==", relation, callexternstatic(GameFramework.EntityInfo, "GetRelation__EntityInfo__EntityInfo", srcObj, target), GameFramework.CharacterRelation, GameFramework.CharacterRelation, TypeKind.Enum, TypeKind.Enum), 93_12_100_13 ){
					if( execbinary("||", execbinary("||", execbinary("==", getexterninstance(SymbolKind.Property, srcObj, GameFramework.EntityInfo, "EntityType"), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), execunary("!", getexterninstance(SymbolKind.Property, srcObj, GameFramework.EntityInfo, "IsPassive"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), callexternstructdictionaryinstance(3, [System.Collections.Generic.MyDictionary_TKey_TValue, System.Int32, TypeKind.Structure, GameFramework.EntityInfo.AttackerInfo, TypeKind.Structure], [null, null, null, null, System.Int32, TypeKind.Structure, OperationKind.Invocation, SymbolKind.Method], getexterninstance(SymbolKind.Property, srcObj, GameFramework.EntityInfo, "AttackerInfos"), System.Collections.Generic.MyDictionary_TKey_TValue, "ContainsKey", callexterninstance(target, GameFramework.EntityInfo, "GetId")), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 94_16_99_17 ){
						if( execbinary("<", distSqr, minDistSqr, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 95_20_98_21 ){
							nearstTarget = target;
							minDistSqr = distSqr;
						};
					};
				};
			};
			return(minDistSqr, nearstTarget);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), rettype(minDistSqr, System.Single, TypeKind.Structure, 1, true), rettype(nearstTarget, GameFramework.EntityInfo, TypeKind.Class, 1, true), paramtype(srcObj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(relation, GameFramework.CharacterRelation, TypeKind.Enum, 0, true), paramtype(type, AiTargetType, TypeKind.Enum, 0, false), paramtype(distSqr, System.Single, TypeKind.Structure, 0, true), paramtype(obj, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(minDistSqr, System.Single, TypeKind.Structure, 1, true), paramtype(nearstTarget, GameFramework.EntityInfo, TypeKind.Class, 1, true)];
		NpcFindCanUseSkill = deffunc(1)args(npc){
			local(__method_ret_103_4_126_5);
			local(skStateInfo); skStateInfo = callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo");
			local(priority); priority = -1;
			local(skInfo); skInfo = null;
			local(curTime); curTime = callexternstatic(GameFramework.TimeUtility, "GetLocalMilliseconds");
			if( execbinary("<=", getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "AutoSkillIds"), System.Collections.Generic.List_T, "Count"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 109_8_110_24 ){
				__method_ret_103_4_126_5 = null;
				return(__method_ret_103_4_126_5);
			};
			local(randIndex); randIndex = callexternstatic(GameFramework.Helper.Random, "Next__Int32__Int32", 0, getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "AutoSkillIds"), System.Collections.Generic.List_T, "Count"));
			skInfo = callexterninstance(skStateInfo, GameFramework.SkillStateInfo, "GetSkillInfoById", getexterninstanceindexer(System.Int32, TypeKind.Structure, System.Collections.Generic.List_T, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "AutoSkillIds"), System.Collections.Generic.List_T, "get_Item", 1, randIndex));
			local(selectSkill); selectSkill = null;
			if( execbinary("&&", execbinary("!=", null, skInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(skInfo, GameFramework.SkillInfo, "IsInCd", curTime), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 114_8_124_9 ){
				selectSkill = skInfo;
			}else{
				local(i); i = 0;
				while( execbinary("<", i, getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "AutoSkillIds"), System.Collections.Generic.List_T, "Count"), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
					skInfo = callexterninstance(skStateInfo, GameFramework.SkillStateInfo, "GetSkillInfoById", getexterninstanceindexer(System.Int32, TypeKind.Structure, System.Collections.Generic.List_T, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "AutoSkillIds"), System.Collections.Generic.List_T, "get_Item", 1, i));
					if( execbinary("&&", execbinary("&&", execbinary("!=", null, skInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(skInfo, GameFramework.SkillInfo, "IsInCd", curTime), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), execbinary(">", getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Field, skInfo, GameFramework.SkillInfo, "ConfigData"), TableConfig.Skill, "skillData"), TableConfig.SkillData, "autoCast"), priority, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 119_16_122_17 ){
						selectSkill = skInfo;
						priority = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Field, skInfo, GameFramework.SkillInfo, "ConfigData"), TableConfig.Skill, "skillData"), TableConfig.SkillData, "autoCast");
					};
				i = execbinary("+", i, 1, null, null, null, null);
				};
			};
			__method_ret_103_4_126_5 = selectSkill;
			return(__method_ret_103_4_126_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.SkillInfo, TypeKind.Class, 0, true), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		cctor = deffunc(0)args(){
			callstatic(AiLogicUtility, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiLogicUtility.__cctor_called){
				return();
			}else{
				AiLogicUtility.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		c_MaxComboInterval = 6000;
		c_MaxViewRange = 30;
		c_MaxViewRangeSqr = 900;
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		ctor = deffunc(0)args(this){
			callinstance(this, AiLogicUtility, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiLogicUtility, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiLogicUtility, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {};

	class_info(TypeKind.Class, Accessibility.Public) {
		sealed(true);
	};
	method_info {
		GetNearstTargetHelper__EntityInfo__CharacterRelation(MethodKind.Ordinary, Accessibility.Public){
			static(true);
		};
		GetNearstTargetHelper__EntityInfo__Single__CharacterRelation(MethodKind.Ordinary, Accessibility.Public){
			static(true);
		};
		GetNearstTargetHelper__EntityInfo__CharacterRelation__AiTargetType(MethodKind.Ordinary, Accessibility.Public){
			static(true);
		};
		GetNearstTargetHelper__EntityInfo__Single__CharacterRelation__AiTargetType(MethodKind.Ordinary, Accessibility.Public){
			static(true);
		};
		GetLivingCharacterInfoHelper(MethodKind.Ordinary, Accessibility.Public){
			static(true);
		};
		GetSeeingLivingCharacterInfoHelper(MethodKind.Ordinary, Accessibility.Public){
			static(true);
		};
		StepCalcNearstTarget(MethodKind.Ordinary, Accessibility.Private){
			static(true);
		};
		NpcFindCanUseSkill(MethodKind.Ordinary, Accessibility.Internal){
			static(true);
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



