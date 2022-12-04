require("cs2dsl__syslib");
require("cs2dsl__attributes");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(Cs2LuaScript.Program) {
	static_methods {
		Init = deffunc(0)args(){
			comment("使用c#代码时需要的初始化（平台相关代码，不会转换为lua代码，cs2lua在进行翻译时会添加宏__LUA__）");
			callstatic(Cs2LuaScript.Program, "InitDll");
			comment("公共初始化（也就是逻辑相关的代码）");
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		Main = deffunc(0)args(args){
			callstatic(Cs2LuaScript.Program, "Init");
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(args, System.String, TypeKind.Array, 0, true)];
		cctor = deffunc(0)args(){
			callstatic(Cs2LuaScript.Program, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(Cs2LuaScript.Program.__cctor_called){
				return();
			}else{
				Cs2LuaScript.Program.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__attributes = Cs2LuaScript__Program__Attrs;
		__cctor_called = false;
	};
	static_props {};
	static_events {};

};



defineentry(Cs2LuaScript.Program);
