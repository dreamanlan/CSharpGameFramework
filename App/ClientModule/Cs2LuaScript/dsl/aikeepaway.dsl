require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("aicommand");

class(AiKeepAway) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiKeepAway, "g_AiKeepAway", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiKeepAway, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiKeepAway.__cctor_called){
				return();
			}else{
				AiKeepAway.__cctor_called = true;
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
			__method_ret_13_4_16_5 = newobject(AiKeepAway, "g_AiKeepAway", typeargs(), typekinds(), "ctor", 0, null);
			return(__method_ret_13_4_16_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISimpleStoryCommandPlugin, TypeKind.Interface, 0, true)];
		ResetState = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, AiKeepAway, "m_KeepAwayStarted", false);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		ExecCommand = deffunc(1)args(this, instance, handler, _params, delta){
			local(__method_ret_23_4_57_5);
			local(args); args = getexterninstance(SymbolKind.Property, _params, StorySystem.StoryValueParams, "Values");
			if( execunary("!", getinstance(SymbolKind.Field, this, AiKeepAway, "m_KeepAwayStarted"), System.Boolean, TypeKind.Structure), 26_8_32_9 ){
				setinstance(SymbolKind.Field, this, AiKeepAway, "m_KeepAwayStarted", true);
				local(__old_val_29_12_29_29); __old_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiKeepAway, "m_ObjId");
				setinstance(SymbolKind.Field, this, AiKeepAway, "m_ObjId", invokeexternoperator(System.Int32, BoxedValue, "op_Implicit__Int32__BoxedValue", getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 0)));
				local(__new_val_29_12_29_29); __new_val_29_12_29_29 = getinstance(SymbolKind.Field, this, AiKeepAway, "m_ObjId");
				recycleandkeepstructvalue(System.Int32, __old_val_29_12_29_29, __new_val_29_12_29_29);
				setinstance(SymbolKind.Field, this, AiKeepAway, "m_SkillInfo", typeas(getexterninstance(SymbolKind.Field, getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 1), BoxedValue, "ObjectVal"), GameFramework.SkillInfo, TypeKind.Class));
				getinstance(SymbolKind.Field, this, AiKeepAway, "m_Ratio") = callexterninstance(getinstanceindexerstruct(true, BoxedValue, BoxedValueList, args, System.Collections.Generic.List_T, "get_Item", 1, 2), BoxedValue, "GetFloat");
			};
			local(npc); npc = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getinstance(SymbolKind.Field, this, AiKeepAway, "m_ObjId"));
			if( execbinary("&&", execbinary("!=", null, npc, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execunary("!", callexterninstance(npc, GameFramework.EntityInfo, "IsUnderControl"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 34_8_55_9 ){
				local(info); info = callexterninstance(npc, GameFramework.EntityInfo, "GetAiStateInfo");
				local(target); target = callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.PluginFramework, "Instance"), GameFramework.PluginFramework, "GetEntityById", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Target"));
				if( execbinary("&&", execbinary("!=", null, target, System.Object, System.Object, TypeKind.Class, TypeKind.Class), execbinary("!=", null, getinstance(SymbolKind.Field, this, AiKeepAway, "m_SkillInfo"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 37_12_54_13 ){
					setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", execbinary("+", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), delta, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure));
					if( execbinary(">", getexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time"), 100, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 39_16_43_17 ){
						setexterninstance(SymbolKind.Property, info, GameFramework.AiStateInfo, "Time", 0);
					}else{
						__method_ret_23_4_57_5 = true;
						return(__method_ret_23_4_57_5);
					};
					local(srcPos); srcPos = callexterninstancereturnstruct(callexterninstance(npc, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
					local(targetPos); targetPos = callexterninstancereturnstruct(callexterninstance(target, GameFramework.EntityInfo, "GetMovementStateInfo"), GameFramework.MovementStateInfo, "GetPosition3D");
					local(distSqr); distSqr = callexternstatic(GameFramework.Geometry, "DistanceSquare__Vector3__Vector3", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
					local(dir); dir = invokeexternoperatorreturnstruct(ScriptRuntime.Vector3, ScriptRuntime.Vector3, "op_Subtraction", wrapexternstructargument(srcPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
					callexterninstance(dir, ScriptRuntime.Vector3, "Normalize");
					targetPos = invokeexternoperatorreturnstruct(ScriptRuntime.Vector3, ScriptRuntime.Vector3, "op_Addition", wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), invokeexternoperatorreturnstruct(ScriptRuntime.Vector3, ScriptRuntime.Vector3, "op_Multiply__Vector3__Single", invokeexternoperatorreturnstruct(ScriptRuntime.Vector3, ScriptRuntime.Vector3, "op_Multiply__Vector3__Single", wrapexternstructargument(dir, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local), getinstance(SymbolKind.Field, this, AiKeepAway, "m_Ratio")), getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiKeepAway, "m_SkillInfo"), GameFramework.SkillInfo, "Distance")));
					if( execbinary("<", distSqr, execbinary("*", execbinary("*", execbinary("*", getinstance(SymbolKind.Field, this, AiKeepAway, "m_Ratio"), getinstance(SymbolKind.Field, this, AiKeepAway, "m_Ratio"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiKeepAway, "m_SkillInfo"), GameFramework.SkillInfo, "Distance"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiKeepAway, "m_SkillInfo"), GameFramework.SkillInfo, "Distance"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 50_16_53_17 ){
						callstatic(AiCommand, "AiPursue", npc, wrapexternstructargument(targetPos, ScriptRuntime.Vector3, OperationKind.LocalReference, SymbolKind.Local));
						__method_ret_23_4_57_5 = true;
						return(__method_ret_23_4_57_5);
					};
				};
			};
			__method_ret_23_4_57_5 = false;
			return(__method_ret_23_4_57_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(_params, StorySystem.StoryValueParams, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiKeepAway, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiKeepAway, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiKeepAway, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_ObjId = 0;
		m_SkillInfo = null;
		m_Ratio = 1.00000000;
		m_KeepAwayStarted = false;
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



