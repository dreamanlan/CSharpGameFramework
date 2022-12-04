require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("aicommand");

class(AiGohome) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiGohome, "g_AiGohome", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiGohome, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiGohome.__cctor_called){
				return();
			}else{
				AiGohome.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		Clone = deffunc(1)args(this){
			local(__method_ret_12_4_15_5);
			__method_ret_12_4_15_5 = newobject(AiGohome, "g_AiGohome", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_12_4_15_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryCommandPlugin, TypeKind.Interface, 0, true)];
		ResetState = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, AiGohome, "m_ParamReaded", false);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		ExecCommand = deffunc(1)args(this, instance, handler, _params, delta){
			local(__method_ret_22_4_35_5);
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			if( execunary("!", getinstance(SymbolKind.Field, this, AiGohome, "m_ParamReaded"), System.Boolean, TypeKind.Structure), 25_8_28_9 ){
				setinstance(SymbolKind.Field, this, AiGohome, "m_ParamReaded", true);
				local(__old_val_27_12_27_29); __old_val_27_12_27_29 = getinstance(SymbolKind.Field, this, AiGohome, "m_ObjId");
				setinstance(SymbolKind.Field, this, AiGohome, "m_ObjId", invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0)));
				local(__new_val_27_12_27_29); __new_val_27_12_27_29 = getinstance(SymbolKind.Field, this, AiGohome, "m_ObjId");
				recycleandkeepstructvalue(System.Int32, __old_val_27_12_27_29, __new_val_27_12_27_29);
			};
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiGohome, "m_ObjId"));
			if( execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 30_8_33_9 ){
				local(info); info = callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo");
				__method_ret_22_4_35_5 = callinstance(this, AiGohome, "GohomeHandler", npc, info, delta);
				return(__method_ret_22_4_35_5);
			};
			__method_ret_22_4_35_5 = false;
			return(__method_ret_22_4_35_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true)];
		GohomeHandler = deffunc(1)args(this, npc, info, deltaTime){
			local(__method_ret_37_4_60_5);
			setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", execbinary("+", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), deltaTime, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
			if( execbinary(">", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), 100, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 40_8_44_9 ){
				setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", 0);
			}else{
				__method_ret_37_4_60_5 = true;
				return(__method_ret_37_4_60_5);
			};
			local(targetPos); targetPos = getexterninstancestructmember(SymbolKind.Property, info, GameFramework.AiStateInfo, "HomePos");
			local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
			local(distSqr); distSqr = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), getexterninstancestructmember(SymbolKind.Property, info, GameFramework.AiStateInfo, "HomePos"));
			if( execbinary("<=", distSqr, 1, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 49_8_58_9 ){
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving", false);
				callstatic(AiCommand, "AiStopPursue", npc);
				callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
				__method_ret_37_4_60_5 = false;
				return(__method_ret_37_4_60_5);
			}else{
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving", true);
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "TargetPosition", targetPos);
				callstatic(AiCommand, "AiPursue", npc, wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
			};
			__method_ret_37_4_60_5 = true;
			return(__method_ret_37_4_60_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(info, GameFramework.AiStateInfo, TypeKind.Class, 0, true), paramtype(deltaTime, System.Int64, TypeKind.Structure, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiGohome, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiGohome, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiGohome, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_ObjId = 0;
		m_ParamReaded = false;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {
		"GameFramework.Plugin.ISimpleStoryCommandPlugin";
	};

	class_info(TypeKind.Class, Accessibility.Public) {
	};
	method_info {
		Clone(MethodKind.Ordinary, Accessibility.Public){
		};
		ResetState(MethodKind.Ordinary, Accessibility.Public){
		};
		ExecCommand(MethodKind.Ordinary, Accessibility.Public){
		};
		GohomeHandler(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



