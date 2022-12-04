require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(MiniMap) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(MiniMap, "g_MiniMap", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(MiniMap, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(MiniMap.__cctor_called){
				return();
			}else{
				MiniMap.__cctor_called = true;
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
			local(rectTrans); rectTrans = typeas(getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.RectTransform, TypeKind.Class);
			getinstance(SymbolKind.Field, this, MiniMap, "m_RawImage") = callexterninstance(obj, UnityEngine.GameObject, "GetComponent__Type", UnityEngine.UI.RawImage);
			getinstance(SymbolKind.Field, this, MiniMap, "m_MapPlayer") = callexterninstance(getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "FindChild", dslstrtocsstr("Player"));
			setinstance(SymbolKind.Field, this, MiniMap, "m_MapWidth", typecast(getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, rectTrans, UnityEngine.RectTransform, "rect"), UnityEngine.Rect, "width"), System.Int32, TypeKind.Structure));
			setinstance(SymbolKind.Field, this, MiniMap, "m_MapHeight", typecast(getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, rectTrans, UnityEngine.RectTransform, "rect"), UnityEngine.Rect, "height"), System.Int32, TypeKind.Structure));
			setinstance(SymbolKind.Field, this, MiniMap, "m_TerrainWidth", typecast(getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getexternstatic(SymbolKind.Property, UnityEngine.Terrain, "activeTerrain"), UnityEngine.Terrain, "terrainData"), UnityEngine.TerrainData, "size"), UnityEngine.Vector3, "x"), System.Int32, TypeKind.Structure));
			setinstance(SymbolKind.Field, this, MiniMap, "m_TerrainHeight", typecast(getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getexternstatic(SymbolKind.Property, UnityEngine.Terrain, "activeTerrain"), UnityEngine.Terrain, "terrainData"), UnityEngine.TerrainData, "size"), UnityEngine.Vector3, "z"), System.Int32, TypeKind.Structure));
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(obj, UnityEngine.GameObject, TypeKind.Class, 0, true), paramtype(behaviour, MonoBehaviourProxy, TypeKind.Class, 0, true)];
		Update = deffunc(0)args(this){
			if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Equality", null, getinstance(SymbolKind.Field, this, MiniMap, "m_GamePlayer")), 21_8_30_9 ){
				getinstance(SymbolKind.Field, this, MiniMap, "m_GamePlayer") = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetGameObject", getexterninstance(SymbolKind.Property, getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "LeaderId"));
			}else{
				local(pos); pos = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, MiniMap, "m_GamePlayer"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
				local(x); x = execbinary("-", execbinary("/", execbinary("*", getexterninstance(SymbolKind.Field, pos, UnityEngine.Vector3, "x"), getinstance(SymbolKind.Field, this, MiniMap, "m_MapWidth"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), getinstance(SymbolKind.Field, this, MiniMap, "m_TerrainWidth"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), execbinary("/", getinstance(SymbolKind.Field, this, MiniMap, "m_MapWidth"), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure);
				local(y); y = execbinary("-", execbinary("/", execbinary("*", getexterninstance(SymbolKind.Field, pos, UnityEngine.Vector3, "z"), getinstance(SymbolKind.Field, this, MiniMap, "m_MapHeight"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), getinstance(SymbolKind.Field, this, MiniMap, "m_TerrainHeight"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), execbinary("/", getinstance(SymbolKind.Field, this, MiniMap, "m_MapHeight"), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure);
				local(rect); rect = typeas(getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, MiniMap, "m_MapPlayer"), UnityEngine.Component, "transform"), UnityEngine.RectTransform, TypeKind.Class);
				setexterninstance(SymbolKind.Property, rect, UnityEngine.Transform, "localPosition", newexternstruct(UnityEngine.Vector3, "g_UnityEngine_Vector3", typeargs(), typekinds(), "ctor__Single__Single__Single", 0, null, x, y, 0));
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		Call = deffunc(0)args(this, name, ...){
			local(args); args = params(System.Object, TypeKind.Class);
			if( execbinary("==", name, "SetImage", System.String, System.String, TypeKind.Class, TypeKind.Class), 34_8_40_9 ){
				local(res); res = typeas(args[1], System.String, TypeKind.Class);
				local(obj); obj = typeas(callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.UiResourceSystem, "Instance"), GameFramework.UiResourceSystem, "GetUiResource", res), UnityEngine.Texture2D, TypeKind.Class);
				if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, obj), 37_12_39_13 ){
					setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, MiniMap, "m_RawImage"), UnityEngine.UI.RawImage, "texture", obj);
				};
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(name, System.String, TypeKind.Class, 0, true), paramtype(..., System.Object, TypeKind.Array, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, MiniMap, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, MiniMap, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, MiniMap, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_RawImage = null;
		m_MapWidth = 100;
		m_MapHeight = 100;
		m_TerrainWidth = 512;
		m_TerrainHeight = 512;
		m_MapPlayer = null;
		m_GamePlayer = null;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {
		"ITickPlugin";
	};

	class_info(TypeKind.Class, Accessibility.Internal) {
	};
	method_info {
		Init(MethodKind.Ordinary, Accessibility.Public){
		};
		Update(MethodKind.Ordinary, Accessibility.Public){
		};
		Call(MethodKind.Ordinary, Accessibility.Public){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



