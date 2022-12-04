require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(SkillHandler) {
	static_methods {
		cctor = deffunc(0)args(){
			callstatic(SkillHandler, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(SkillHandler.__cctor_called){
				return();
			}else{
				SkillHandler.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

};



