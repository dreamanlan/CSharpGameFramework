require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("aicommand");

class(AiChase) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiChase, "g_AiChase", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiChase, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiChase.__cctor_called){
				return();
			}else{
				AiChase.__cctor_called = true;
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
			__method_ret_13_4_16_5 = newobject(AiChase, "g_AiChase", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_13_4_16_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryCommandPlugin, TypeKind.Interface, 0, true)];
		ResetState = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, AiChase, "m_ChaseStarted", false);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		ExecCommand = deffunc(1)args(this, instance, handler, _params, delta){
			local(__method_ret_23_4_53_5);
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			if( execunary("!", getinstance(SymbolKind.Field, this, AiChase, "m_ChaseStarted"), System.Boolean, TypeKind.Structure), 26_8_31_9 ){
				setinstance(SymbolKind.Field, this, AiChase, "m_ChaseStarted", true);
				local(__old_val_29_12_29_29); __old_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiChase, "m_ObjId");
				setinstance(SymbolKind.Field, this, AiChase, "m_ObjId", invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0)));
				local(__new_val_29_12_29_29); __new_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiChase, "m_ObjId");
				recycleandkeepstructvalue(System.Int32, __old_val_29_12_29_29, __new_val_29_12_29_29);
				setinstance(SymbolKind.Field, this, AiChase, "m_SkillInfo", typeas(getexterninstance(SymbolKind.Field, getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "ObjectVal"), GameFramework.SkillInfo, TypeKind.Class));
			};
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiChase, "m_ObjId"));
			if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(npc, GameFramework.EntityInfo, "IsUnderControl"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 33_8_51_9 ){
				local(info); info = callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo");
				local(target); target = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Target"));
				if( execbinary("&&", execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execbinary("!=", null, getinstance(SymbolKind.Field, this, AiChase, "m_SkillInfo"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 36_12_50_13 ){
					setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", execbinary("+", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), delta, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
					if( execbinary(">", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), 100, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 38_16_42_17 ){
						setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", 0);
					}else{
						__method_ret_23_4_53_5 = true;
						return(__method_ret_23_4_53_5);
					};
					local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
					local(targetPos); targetPos = callexterninstancereturnstruct(callexterninstance(target, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
					local(distSqr); distSqr = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
					if( execbinary(">", distSqr, execbinary("*", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiChase, "m_SkillInfo"), GameFramework.SkillInfo, "Distance"), getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiChase, "m_SkillInfo"), GameFramework.SkillInfo, "Distance"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 46_16_49_17 ){
						callstatic(AiCommand, "AiPursue", npc, wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
						__method_ret_23_4_53_5 = true;
						return(__method_ret_23_4_53_5);
					};
				};
			};
			__method_ret_23_4_53_5 = false;
			return(__method_ret_23_4_53_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiChase, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiChase, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiChase, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_ObjId = 0;
		m_SkillInfo = null;
		m_ChaseStarted = false;
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
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



