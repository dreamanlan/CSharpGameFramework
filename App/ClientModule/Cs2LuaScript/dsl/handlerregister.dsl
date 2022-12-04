require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(HandlerRegister) {
	static_methods {
		Register = deffunc(0)args(){
			comment("在这里注册所有基于名字的消息处理");
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		Call = deffunc(0)args(name, ...){
			local(args); args = params(System.Object, TypeKind.Class);
			local(handler);
			if( execclosure(true, __invoke_15_12_15_53, true){ multiassign(precode{
				},postcode{
				})varlist(__invoke_15_12_15_53, handler) = callexterninstance(getstatic(SymbolKind.Field, HandlerRegister, "s_Handlers"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", name, __cs2dsl_out); }, 15_8_17_9 ){
				calldelegation(handler, "HandlerDelegation.Invoke", dslunpack(dsltoobject(SymbolKind.Method, false, "HandlerDelegation:Invoke", args)));
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(name, System.String, TypeKind.Class, 0, true), paramtype(..., System.Object, TypeKind.Array, 0, true)];
		Register__String__HandlerDelegation = deffunc(0)args(name, handler){
			if( callexterninstance(getstatic(SymbolKind.Field, HandlerRegister, "s_Handlers"), System.Collections.Generic.Dictionary_TKey_TValue, "ContainsKey", name), 21_8_25_9 ){
				callexterninstance(getstatic(SymbolKind.Field, HandlerRegister, "s_Handlers"), System.Collections.Generic.Dictionary_TKey_TValue, "Add", name, handler);
			}else{
				setexterninstanceindexer(HandlerDelegation, TypeKind.Delegate, System.Collections.Generic.Dictionary_TKey_TValue, getstatic(SymbolKind.Field, HandlerRegister, "s_Handlers"), System.Collections.Generic.Dictionary_TKey_TValue, "set_Item", 2, name, handler);
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(name, System.String, TypeKind.Class, 0, true), paramtype(handler, HandlerDelegation, TypeKind.Delegate, 0, false)];
		cctor = deffunc(0)args(){
			callstatic(HandlerRegister, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(HandlerRegister.__cctor_called){
				return();
			}else{
				HandlerRegister.__cctor_called = true;
			};
			setstatic(SymbolKind.Field, HandlerRegister, "s_Handlers", newexterndictionary(System.Collections.Generic.Dictionary_TKey_TValue, "g_System_Collections_Generic_Dictionary_System_String_HandlerDelegation", typeargs(System.String, "HandlerDelegation"), typekinds(TypeKind.Class, TypeKind.Delegate), "ctor", 0, literaldictionary("g_System_Collections_Generic_Dictionary_System_String_HandlerDelegation", typeargs(System.String, "HandlerDelegation"), typekinds(TypeKind.Class, TypeKind.Delegate))));
		}options[needfuncinfo(false)];
	};
	static_fields {
		s_Handlers = null;
		__cctor_called = false;
	};
	static_props {};
	static_events {};

};



