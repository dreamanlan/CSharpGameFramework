require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(Cs2LuaScript.Plugin) {
	static_methods {
		get_Proxy = deffunc(1)args(){
			local(__method_ret_18_8_24_9);
			__method_ret_18_8_24_9 = getexternstatic(SymbolKind.Property, GameFramework.Plugin.PluginProxy, "NativeProxy");
			return(__method_ret_18_8_24_9);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.IPluginProxy, TypeKind.Interface, 0, true)];
		cctor = deffunc(0)args(){
			callstatic(Cs2LuaScript.Plugin, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(Cs2LuaScript.Plugin.__cctor_called){
				return();
			}else{
				Cs2LuaScript.Plugin.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {
		Proxy = {
			get = static_methods.get_Proxy;
		};
	};
	static_events {};

};



