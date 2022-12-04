require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(AiGetTarget) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiGetTarget, "g_AiGetTarget", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiGetTarget, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiGetTarget.__cctor_called){
				return();
			}else{
				AiGetTarget.__cctor_called = true;
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
			setinstance(SymbolKind.Field, this, AiGetTarget, "m_Proxy", result);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(result, StorySystem.StoryValueResult, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_17_4_20_5);
			__method_ret_17_4_20_5 = newobject(AiGetTarget, "g_AiGetTarget", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_17_4_20_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryValuePlugin, TypeKind.Interface, 0, true)];
		Evaluate = deffunc(0)args(this, instance, handler, _params){
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			local(objId); objId = invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0));
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", objId);
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 26_8_38_9 ){
				local(targetId); targetId = getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo"), GameFramework.AiStateInfo, "Target");
				if( execbinary(">", targetId, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 28_12_37_13 ){
					local(entity); entity = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", targetId);
					if( execbinary("&&", execbinary("!=", null, entity, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(entity, GameFramework.EntityInfo, "IsDead"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 30_16_34_17 ){
						getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiGetTarget, "m_Proxy"), StorySystem.StoryValueResult, "Value") = callexternstaticreturnstruct(BoxedValue, "FromObject", entity);
					}else{
						setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiGetTarget, "m_Proxy"), StorySystem.StoryValueResult, "Value", getexternstaticstructmember(SymbolKind.Property, BoxedValue, "NullObject"));
					};
				}else{
					setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiGetTarget, "m_Proxy"), StorySystem.StoryValueResult, "Value", getexternstaticstructmember(SymbolKind.Property, BoxedValue, "NullObject"));
				};
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiGetTarget, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiGetTarget, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiGetTarget, "__ctor_called", true);
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



