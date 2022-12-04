require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("handlerregister");
require("skilltriggerregister");
require("storyregister");

class(Startup) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(Startup, "g_Startup", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(Startup, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(Startup.__cctor_called){
				return();
			}else{
				Startup.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		Init = deffunc(0)args(this, obj, behaviour){
			callstatic(HandlerRegister, "Register");
			callstatic(SkillTriggerRegister, "Register");
			callstatic(StoryRegister, "Register");
			callexterninstance(behaviour, MonoBehaviourProxy, "StartCoroutine", callinstance(this, Startup, "Tick"));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(obj, UnityEngine.GameObject, TypeKind.Class, 0, true), paramtype(behaviour, MonoBehaviourProxy, TypeKind.Class, 0, true)];
		Call = deffunc(0)args(this, name, ...){
			local(args); args = params(System.Object, TypeKind.Class);
			callstatic(HandlerRegister, "Call", name, dslunpack(dsltoobject(SymbolKind.Method, true, "HandlerRegister:Call", args)));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(name, System.String, TypeKind.Class, 0, true), paramtype(..., System.Object, TypeKind.Array, 0, true)];
		Tick = wrapenumerable(deffunc(1)args(this){
			local(__method_ret_18_4_23_5);
			wrapyield(newexternobject(UnityEngine.WaitForSeconds, "g_UnityEngine_WaitForSeconds", typeargs(), typekinds(), "ctor", 0, null, 10.00000000), false, true);
			callexternstatic(UnityEngine.Debug, "Log__Object", dslstrtocsstr("tick after start 10 seconds."));
			wrapyield(null, false, false);
			return(null);
		}options[needfuncinfo(false), rettype(return, System.Collections.IEnumerator, TypeKind.Interface, 0, true)]);
		ctor = deffunc(0)args(this){
			callinstance(this, Startup, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, Startup, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, Startup, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {
		"IStartupPlugin";
	};

	class_info(TypeKind.Class, Accessibility.Internal) {
	};
	method_info {
		Init(MethodKind.Ordinary, Accessibility.Public){
		};
		Call(MethodKind.Ordinary, Accessibility.Public){
		};
		Tick(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



