require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(AiNeedChase) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiNeedChase, "g_AiNeedChase", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiNeedChase, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiNeedChase.__cctor_called){
				return();
			}else{
				AiNeedChase.__cctor_called = true;
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
			setinstance(SymbolKind.Field, this, AiNeedChase, "m_Proxy", result);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(result, StorySystem.StoryValueResult, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_17_4_20_5);
			__method_ret_17_4_20_5 = newobject(AiNeedChase, "g_AiNeedChase", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_17_4_20_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryValuePlugin, TypeKind.Interface, 0, true)];
		Evaluate = deffunc(0)args(this, instance, handler, _params){
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			local(objId); objId = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0), BoxedValue, "GetInt");
			local(skillInfo); skillInfo = typeas(getexterninstance(SymbolKind.Field, getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "ObjectVal"), GameFramework.SkillInfo, TypeKind.Class);
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", objId);
			if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execbinary("!=", null, skillInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 27_8_39_9 ){
				local(targetId); targetId = getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo"), GameFramework.AiStateInfo, "Target");
				if( execbinary(">", targetId, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 29_12_38_13 ){
					local(target); target = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", targetId);
					if( execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 31_16_37_17 ){
						local(distSqr); distSqr = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"), callexterninstancereturnstruct(callexterninstance(target, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"));
						if( execbinary(">", distSqr, execbinary("*", getexterninstance(SymbolKind.Property, skillInfo, GameFramework.SkillInfo, "Distance"), getexterninstance(SymbolKind.Property, skillInfo, GameFramework.SkillInfo, "Distance"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 33_20_36_21 ){
							setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiNeedChase, "m_Proxy"), StorySystem.StoryValueResult, "Value", invokeexternoperatorreturnstruct(BoxedValue, BoxedValue, "op_Implicit__BoxedValue__Int32", 1));
							return();
						};
					};
				};
			};
			setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiNeedChase, "m_Proxy"), StorySystem.StoryValueResult, "Value", invokeexternoperatorreturnstruct(BoxedValue, BoxedValue, "op_Implicit__BoxedValue__Int32", 0));
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiNeedChase, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiNeedChase, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiNeedChase, "__ctor_called", true);
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



