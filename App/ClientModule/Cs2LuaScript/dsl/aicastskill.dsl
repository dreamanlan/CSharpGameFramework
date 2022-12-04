require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("aicommand");

class(AiCastSkill) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiCastSkill, "g_AiCastSkill", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiCastSkill, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiCastSkill.__cctor_called){
				return();
			}else{
				AiCastSkill.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		Clone = deffunc(1)args(this){
			local(__method_ret_13_4_16_5);
			__method_ret_13_4_16_5 = newobject(AiCastSkill, "g_AiCastSkill", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_13_4_16_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryCommandPlugin, TypeKind.Interface, 0, true)];
		ResetState = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, AiCastSkill, "m_ParamReaded", false);
			setinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillCasted", false);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		ExecCommand = deffunc(1)args(this, instance, handler, _params, delta){
			local(__method_ret_24_4_67_5);
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			if( execunary("!", getinstance(SymbolKind.Field, this, AiCastSkill, "m_ParamReaded"), System.Boolean, TypeKind.Structure), 27_8_30_9 ){
				local(__old_val_28_12_28_29); __old_val_28_12_28_29 = getinstance(SymbolKind.Field, this, AiCastSkill, "m_ObjId");
				setinstance(SymbolKind.Field, this, AiCastSkill, "m_ObjId", invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0)));
				local(__new_val_28_12_28_29); __new_val_28_12_28_29 = getinstance(SymbolKind.Field, this, AiCastSkill, "m_ObjId");
				recycleandkeepstructvalue(System.Int32, __old_val_28_12_28_29, __new_val_28_12_28_29);
				setinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillInfo", typeas(getexterninstance(SymbolKind.Field, getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "ObjectVal"), GameFramework.SkillInfo, TypeKind.Class));
			};
			if( execunary("!", getinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillCasted"), System.Boolean, TypeKind.Structure), 31_8_65_9 ){
				local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiCastSkill, "m_ObjId"));
				if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(npc, GameFramework.EntityInfo, "IsUnderControl"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 33_12_54_13 ){
					local(targetId); targetId = getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo"), GameFramework.AiStateInfo, "Target");
					local(target); target = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", targetId);
					if( execbinary("&&", execbinary("&&", execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(target, GameFramework.EntityInfo, "IsDead"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), execbinary("<=", callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"), callexterninstancereturnstruct(callexterninstance(target, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D")), execbinary("*", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillInfo"), GameFramework.SkillInfo, "Distance"), getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillInfo"), GameFramework.SkillInfo, "Distance"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 36_16_53_17 ){
						local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
						local(targetPos); targetPos = callexterninstancereturnstruct(callexterninstance(target, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
						local(dir); dir = callexternstatic(GameFramework.Geometry, "GetYRadian__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
						local(curDir); curDir = callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetFaceDir");
						if( execbinary(">", callexternstatic(UnityEngine.Mathf, "Abs__Single", execbinary("-", dir, curDir, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure)), 0.15700000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 41_20_47_21 ){
							callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "SetWantedFaceDir", dir);
						}else{
							setinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillCasted", true);
							callstatic(AiCommand, "AiStopPursue", npc);
							callstatic(AiCommand, "AiSkill", npc, getexterninstance(SymbolKind.Field, getinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillInfo"), GameFramework.SkillInfo, "SkillId"));
						};
						__method_ret_24_4_67_5 = true;
						return(__method_ret_24_4_67_5);
					}elseif( execunary("!", getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Field, getinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillInfo"), GameFramework.SkillInfo, "ConfigData"), TableConfig.Skill, "skillData"), TableConfig.SkillData, "needTarget"), System.Boolean, TypeKind.Structure), 49_18_53_17 ){
						setinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillCasted", true);
						callstatic(AiCommand, "AiStopPursue", npc);
						callstatic(AiCommand, "AiSkill", npc, getexterninstance(SymbolKind.Field, getinstance(SymbolKind.Field, this, AiCastSkill, "m_SkillInfo"), GameFramework.SkillInfo, "SkillId"));
					};
				};
			}else{
				local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiCastSkill, "m_ObjId"));
				if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 57_12_64_13 ){
					local(info); info = callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo");
					if( callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "IsSkillActivated"), 59_16_63_17 ){
						__method_ret_24_4_67_5 = true;
						return(__method_ret_24_4_67_5);
					}else{
						__method_ret_24_4_67_5 = false;
						return(__method_ret_24_4_67_5);
					};
				};
			};
			__method_ret_24_4_67_5 = false;
			return(__method_ret_24_4_67_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiCastSkill, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiCastSkill, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiCastSkill, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_ObjId = 0;
		m_SkillInfo = null;
		m_SkillCasted = false;
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
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



