require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("ailogicutility");

class(AiSelectTarget) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiSelectTarget, "g_AiSelectTarget", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiSelectTarget, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiSelectTarget.__cctor_called){
				return();
			}else{
				AiSelectTarget.__cctor_called = true;
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
			setinstance(SymbolKind.Field, this, AiSelectTarget, "m_Proxy", result);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(result, StorySystem.StoryValueResult, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_17_4_20_5);
			__method_ret_17_4_20_5 = newobject(AiSelectTarget, "g_AiSelectTarget", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_17_4_20_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryValuePlugin, TypeKind.Interface, 0, true)];
		Evaluate = deffunc(0)args(this, instance, handler, _params){
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			local(objId); objId = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0), BoxedValue, "GetInt");
			local(dist); dist = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "GetFloat");
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", objId);
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 27_8_41_9 ){
				local(entity);
				if( execbinary("<", dist, 0.00010000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 29_12_39_13 ){
					entity = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__CharacterRelation", npc, 0);
					if( execbinary("!=", null, entity, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 31_16_33_17 ){
						getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo"), GameFramework.AiStateInfo, "Target") = callexterninstance(entity, GameFramework.EntityInfo, "GetId");
					};
				}else{
					entity = callstatic(AiLogicUtility, "GetNearstTargetHelper__EntityInfo__Single__CharacterRelation", npc, dist, 0);
					if( execbinary("!=", null, entity, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 36_16_38_17 ){
						getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo"), GameFramework.AiStateInfo, "Target") = callexterninstance(entity, GameFramework.EntityInfo, "GetId");
					};
				};
				getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiSelectTarget, "m_Proxy"), StorySystem.StoryValueResult, "Value") = callexternstaticreturnstruct(BoxedValue, "FromObject", entity);
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiSelectTarget, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiSelectTarget, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiSelectTarget, "__ctor_called", true);
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



