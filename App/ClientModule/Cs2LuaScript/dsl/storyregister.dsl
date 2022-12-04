require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("cs2luascript__plugin");

class(StoryRegister) {
	static_methods {
		Register = deffunc(0)args(){
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryCommand", dslstrtocsstr("ai_do_normal"), dslstrtocsstr("AiDoNormal"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryCommand", dslstrtocsstr("ai_do_member"), dslstrtocsstr("AiDoMember"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryCommand", dslstrtocsstr("ai_cast_skill"), dslstrtocsstr("AiCastSkill"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryCommand", dslstrtocsstr("ai_chase"), dslstrtocsstr("AiChase"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryCommand", dslstrtocsstr("ai_keep_away"), dslstrtocsstr("AiKeepAway"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryCommand", dslstrtocsstr("ai_go_home"), dslstrtocsstr("AiGohome"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryCommand", dslstrtocsstr("ai_rand_move"), dslstrtocsstr("AiRandMove"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_select_skill_by_distance"), dslstrtocsstr("AiSelectSkillByDistance"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_need_chase"), dslstrtocsstr("AiNeedChase"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_need_keep_away"), dslstrtocsstr("AiNeedKeepAway"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_select_skill"), dslstrtocsstr("AiSelectSkill"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_select_target"), dslstrtocsstr("AiSelectTarget"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_get_skill"), dslstrtocsstr("AiGetSkill"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_get_target"), dslstrtocsstr("AiGetTarget"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_get_skills"), dslstrtocsstr("AiGetSkills"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSimpleStoryValue", dslstrtocsstr("ai_get_entities"), dslstrtocsstr("AiGetEntities"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterStoryValue", dslstrtocsstr("select"), dslstrtocsstr("AiQuery"));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		cctor = deffunc(0)args(){
			callstatic(StoryRegister, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(StoryRegister.__cctor_called){
				return();
			}else{
				StoryRegister.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

};



