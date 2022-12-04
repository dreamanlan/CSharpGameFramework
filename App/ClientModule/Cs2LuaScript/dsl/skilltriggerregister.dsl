require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("cs2luascript__plugin");

class(SkillTriggerRegister) {
	static_methods {
		Register = deffunc(0)args(){
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSkillTrigger", dslstrtocsstr("trackbullet"), dslstrtocsstr("TrackBulletTrigger"));
			callinterface(getstatic(SymbolKind.Property, Cs2LuaScript.Plugin, "Proxy"), GameFramework.Plugin.IPluginProxy, "RegisterSkillTrigger", dslstrtocsstr("track2"), dslstrtocsstr("Track2Trigger"));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		cctor = deffunc(0)args(){
			callstatic(SkillTriggerRegister, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(SkillTriggerRegister.__cctor_called){
				return();
			}else{
				SkillTriggerRegister.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

};



