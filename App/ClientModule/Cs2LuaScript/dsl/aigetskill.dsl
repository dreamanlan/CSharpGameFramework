require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(AiGetSkill) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiGetSkill, "g_AiGetSkill", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiGetSkill, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiGetSkill.__cctor_called){
				return();
			}else{
				AiGetSkill.__cctor_called = true;
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
			setinstance(SymbolKind.Field, this, AiGetSkill, "m_Proxy", result);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(result, StorySystem.StoryValueResult, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_17_4_20_5);
			__method_ret_17_4_20_5 = newobject(AiGetSkill, "g_AiGetSkill", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_17_4_20_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryValuePlugin, TypeKind.Interface, 0, true)];
		Evaluate = deffunc(0)args(this, instance, handler, _params){
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			local(objId); objId = invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0));
			local(index); index = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "GetInt");
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", objId);
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 27_8_71_9 ){
				local(skillId); skillId = 0;
				local(__switch_29_12_60_13); __switch_29_12_60_13 = index;
				if( __switch_29_12_60_13 == 0 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill0");
				}elseif( __switch_29_12_60_13 == 1 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill1");
				}elseif( __switch_29_12_60_13 == 2 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill2");
				}elseif( __switch_29_12_60_13 == 3 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill3");
				}elseif( __switch_29_12_60_13 == 4 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill4");
				}elseif( __switch_29_12_60_13 == 5 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill5");
				}elseif( __switch_29_12_60_13 == 6 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill6");
				}elseif( __switch_29_12_60_13 == 7 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill7");
				}elseif( __switch_29_12_60_13 == 8 ){
					skillId = getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, npc, GameFramework.EntityInfo, "ConfigData"), TableConfig.Actor, "skill8");
				}else{
					skillId = 0;
				};
				if( execbinary(">", skillId, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 61_12_70_13 ){
					local(skillInfo); skillInfo = callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "GetSkillInfoById", skillId);
					if( execbinary("==", null, skillInfo, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 63_16_66_17 ){
						skillInfo = newexternobject(GameFramework.SkillInfo, "g_GameFramework_SkillInfo", typeargs(), typekinds(), "ctor__Int32", 0, null, skillId);
						callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetSkillStateInfo"), GameFramework.SkillStateInfo, "AddSkill__SkillInfo", skillInfo);
					};
					getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiGetSkill, "m_Proxy"), StorySystem.StoryValueResult, "Value") = callexternstaticreturnstruct(BoxedValue, "FromObject", skillInfo);
				}else{
					setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiGetSkill, "m_Proxy"), StorySystem.StoryValueResult, "Value", getexternstaticstructmember(SymbolKind.Property, BoxedValue, "NullObject"));
				};
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiGetSkill, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiGetSkill, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiGetSkill, "__ctor_called", true);
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



