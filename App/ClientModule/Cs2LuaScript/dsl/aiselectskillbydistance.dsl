require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(AiSelectSkillByDistance) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiSelectSkillByDistance, "g_AiSelectSkillByDistance", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiSelectSkillByDistance, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiSelectSkillByDistance.__cctor_called){
				return();
			}else{
				AiSelectSkillByDistance.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		SetProxy = deffunc(0)args(this, result){
			setinstance(SymbolKind.Field, this, AiSelectSkillByDistance, "m_Proxy", result);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(result, StorySystem.StoryValueResult, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_17_4_20_5);
			__method_ret_17_4_20_5 = newobject(AiSelectSkillByDistance, "g_AiSelectSkillByDistance", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_17_4_20_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryValuePlugin, TypeKind.Interface, 0, true)];
		Evaluate = deffunc(0)args(this, instance, handler, _params){
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			local(objId); objId = invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0));
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", objId);
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 26_8_54_9 ){
				local(targetId); targetId = getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo"), GameFramework.AiStateInfo, "Target");
				if( execbinary(">", targetId, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 28_12_53_13 ){
					local(target); target = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", targetId);
					if( execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 30_16_52_17 ){
						local(distToTarget); distToTarget = callexternstatic(GameFramework.Geometry, "Distance__Vector3__Vector3", callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"), callexterninstancereturnstruct(callexterninstance(target, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"));
						local(maxSkillInfo); maxSkillInfo = null;
						local(diffDist); diffDist = 340282300000000000000000000000000000000.00000000;
						local(targetSkillInfo); targetSkillInfo = null;
						local(i); i = 0;
						while( execbinary("<", i, getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "AutoSkillIds"), System.Collections.Generic.List_T, "Count"), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
							local(skillId); skillId = getexterninstanceindexer(System.Int32, TypeKind.Structure, System.Collections.Generic.List_T, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "AutoSkillIds"), System.Collections.Generic.List_T, "get_Item", 1, i);
							local(temp); temp = callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "GetSkillInfoById", skillId);
							if( execbinary("!=", null, temp, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 38_24_45_25 ){
								local(dist); dist = getexterninstance(SymbolKind.Property, temp, GameFramework.SkillInfo, "Distance");
								local(absDist); absDist = callexternstatic(UnityEngine.Mathf, "Abs__Single", execbinary("-", distToTarget, dist, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
								if( execbinary(">", diffDist, absDist, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 41_28_44_29 ){
									diffDist = absDist;
									targetSkillInfo = temp;
								};
							};
						i = execbinary("+", i, 1, null, null, null, null);
						};
						if( execbinary("!=", null, targetSkillInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 47_20_50_76 ){
							getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiSelectSkillByDistance, "m_Proxy"), StorySystem.StoryValueResult, "Value") = callexternstaticreturnstruct(BoxedValue, "FromObject", targetSkillInfo);
						}else{
							getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiSelectSkillByDistance, "m_Proxy"), StorySystem.StoryValueResult, "Value") = callexternstaticreturnstruct(BoxedValue, "FromObject", maxSkillInfo);
						};
						return();
					};
				};
			};
			setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiSelectSkillByDistance, "m_Proxy"), StorySystem.StoryValueResult, "Value", getexternstaticstructmember(SymbolKind.Property, BoxedValue, "NullObject"));
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiSelectSkillByDistance, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiSelectSkillByDistance, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiSelectSkillByDistance, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_Proxy = null;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {
		"GameFramework.Plugin.ISimpleStoryValuePlugin";
	};

	class_info(TypeKind.Class, Accessibility.Public) {
	};
	method_info {
		SetProxy(MethodKind.Ordinary, Accessibility.Public){
		};
		Clone(MethodKind.Ordinary, Accessibility.Public){
		};
		Evaluate(MethodKind.Ordinary, Accessibility.Public){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



