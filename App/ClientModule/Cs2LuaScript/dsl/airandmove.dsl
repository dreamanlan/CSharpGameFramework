require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("aicommand");

class(AiRandMove) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiRandMove, "g_AiRandMove", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiRandMove, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiRandMove.__cctor_called){
				return();
			}else{
				AiRandMove.__cctor_called = true;
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
			local(__method_ret_13_4_16_5);
			__method_ret_13_4_16_5 = newobject(AiRandMove, "g_AiRandMove", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_13_4_16_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryCommandPlugin, TypeKind.Interface, 0, true)];
		ResetState = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, AiRandMove, "m_ParamReaded", false);
			setinstance(SymbolKind.Field, this, AiRandMove, "m_PursueInterval", 0);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		ExecCommand = deffunc(1)args(this, instance, handler, _params, delta){
			local(__method_ret_24_4_47_5);
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			if( execunary("!", getinstance(SymbolKind.Field, this, AiRandMove, "m_ParamReaded"), System.Boolean, TypeKind.Structure), 27_8_45_9 ){
				setinstance(SymbolKind.Field, this, AiRandMove, "m_ParamReaded", true);
				getinstance(SymbolKind.Field, this, AiRandMove, "m_ObjId") = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0), BoxedValue, "GetInt");
				getinstance(SymbolKind.Field, this, AiRandMove, "m_Time") = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "GetInt");
				getinstance(SymbolKind.Field, this, AiRandMove, "m_Radius") = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 2), BoxedValue, "GetInt");
				local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiRandMove, "m_ObjId"));
				if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(npc, GameFramework.EntityInfo, "IsUnderControl"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 35_12_38_13 ){
					callinstance(this, AiRandMove, "SelectTargetPos", npc);
					__method_ret_24_4_47_5 = true;
					return(__method_ret_24_4_47_5);
				};
			}else{
				local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiRandMove, "m_ObjId"));
				if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(npc, GameFramework.EntityInfo, "IsUnderControl"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 41_12_44_13 ){
					local(info); info = callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo");
					__method_ret_24_4_47_5 = callinstance(this, AiRandMove, "RandMoveHandler", npc, info, delta);
					return(__method_ret_24_4_47_5);
				};
			};
			__method_ret_24_4_47_5 = false;
			return(__method_ret_24_4_47_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true)];
		RandMoveHandler = deffunc(1)args(this, npc, info, deltaTime){
			local(__method_ret_49_4_85_5);
			setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", execbinary("+", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), deltaTime, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
			setinstance(SymbolKind.Field, this, AiRandMove, "m_PursueInterval", execbinary("+", getinstance(SymbolKind.Field, this, AiRandMove, "m_PursueInterval"), deltaTime, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
			if( execbinary(">", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), getinstance(SymbolKind.Field, this, AiRandMove, "m_Time"), System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 53_8_64_9 ){
				setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", 0);
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving", false);
				callstatic(AiCommand, "AiStopPursue", npc);
				callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
				local(target); target = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Target"));
				if( execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 59_12_62_13 ){
					local(dir); dir = callexternstatic(GameFramework.Geometry, "GetYRadian__Vector3__Vector3", callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"), callexterninstancereturnstruct(callexterninstance(target, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D"));
					callexterninstance(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "SetFaceDir__Single", dir);
				};
				__method_ret_49_4_85_5 = false;
				return(__method_ret_49_4_85_5);
			};
			if( execbinary("<", getinstance(SymbolKind.Field, this, AiRandMove, "m_PursueInterval"), 100, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 65_8_69_9 ){
				__method_ret_49_4_85_5 = true;
				return(__method_ret_49_4_85_5);
			}else{
				setinstance(SymbolKind.Field, this, AiRandMove, "m_PursueInterval", 0);
			};
			local(targetPos); targetPos = getexterninstancestructmember(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "TargetPosition");
			local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
			local(distSqr); distSqr = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
			if( execbinary("<=", distSqr, 1, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 74_8_83_9 ){
				if( getexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving"), 75_12_79_13 ){
					setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving", false);
					callstatic(AiCommand, "AiStopPursue", npc);
					callexterninstance(info, GameFramework.AiStateInfo, "ChangeToState", 1);
				};
			}else{
				setexterninstance(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "IsMoving", true);
				callstatic(AiCommand, "AiPursue", npc, wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
			};
			__method_ret_49_4_85_5 = true;
			return(__method_ret_49_4_85_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true), paramtype(info, GameFramework.AiStateInfo, TypeKind.Class, 0, true), paramtype(deltaTime, System.Int64, TypeKind.Structure, 0, true)];
		SelectTargetPos = deffunc(0)args(this, npc){
			local(pos); pos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
			local(dx); dx = execbinary("-", callexternstatic(GameFramework.Helper.Random, "Next__Int32", getinstance(SymbolKind.Field, this, AiRandMove, "m_Radius")), execbinary("/", getinstance(SymbolKind.Field, this, AiRandMove, "m_Radius"), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure);
			local(dz); dz = execbinary("-", callexternstatic(GameFramework.Helper.Random, "Next__Int32", getinstance(SymbolKind.Field, this, AiRandMove, "m_Radius")), execbinary("/", getinstance(SymbolKind.Field, this, AiRandMove, "m_Radius"), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure);
			setexterninstance(SymbolKind.Field, pos, ScriptRuntime.Vector3, "X", execbinary("+", getexterninstance(SymbolKind.Field, pos, ScriptRuntime.Vector3, "X"), dx, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
			setexterninstance(SymbolKind.Field, pos, ScriptRuntime.Vector3, "Z", execbinary("+", getexterninstance(SymbolKind.Field, pos, ScriptRuntime.Vector3, "Z"), dz, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
			getexterninstancestructmember(SymbolKind.Property, callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "TargetPosition") = callstatic(AiCommand, "AiGetValidPosition", npc, wrapexternstructargument(pos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), getinstance(SymbolKind.Field, this, AiRandMove, "m_Radius"));
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(npc, GameFramework.EntityInfo, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiRandMove, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiRandMove, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiRandMove, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_ObjId = 0;
		m_Time = 0;
		m_Radius = 0;
		m_ParamReaded = false;
		m_PursueInterval = 0;
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
		RandMoveHandler(MethodKind.Ordinary, Accessibility.Private){
		};
		SelectTargetPos(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



