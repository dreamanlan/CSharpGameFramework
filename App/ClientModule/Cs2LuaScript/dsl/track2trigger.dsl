require("cs2dsl__syslib");
require("cs2dsl__attributes");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(Track2Trigger) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(Track2Trigger, "g_Track2Trigger", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(Track2Trigger, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(Track2Trigger.__cctor_called){
				return();
			}else{
				Track2Trigger.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__attributes = Track2Trigger__Attrs;
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		SetProxy = deffunc(0)args(this, triggerProxy){
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy", triggerProxy);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(triggerProxy, SkillSystem.SkillTriggerProxy, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_16_4_25_5);
			local(triger); triger = newobject(Track2Trigger, "g_Track2Trigger", typeargs(), typekinds(), "ctor", 0, null);
			callexterninstance(getinstance(SymbolKind.Field, triger, Track2Trigger, "m_TrackBone"), SkillSystem.SkillStringParam, "CopyFrom", getinstance(SymbolKind.Field, this, Track2Trigger, "m_TrackBone"));
			callexterninstance(getinstance(SymbolKind.Field, triger, Track2Trigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "CopyFrom", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Duration"));
			callexterninstance(getinstance(SymbolKind.Field, triger, Track2Trigger, "m_DamageInterval"), SkillSystem.SkillNonStringParam_T, "CopyFrom", getinstance(SymbolKind.Field, this, Track2Trigger, "m_DamageInterval"));
			setinstance(SymbolKind.Field, triger, Track2Trigger, "m_BulletRadius", getinstance(SymbolKind.Field, this, Track2Trigger, "m_BulletRadius"));
			setinstance(SymbolKind.Field, triger, Track2Trigger, "m_BulletRadiusSquare", getinstance(SymbolKind.Field, this, Track2Trigger, "m_BulletRadiusSquare"));
			__method_ret_16_4_25_5 = triger;
			return(__method_ret_16_4_25_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISkillTriggerPlugin, TypeKind.Interface, 0, true)];
		Reset = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_IsStarted", false);
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect", null);
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_BoneTransform", null);
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime", 0);
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_IsHit", false);
			local(__old_val_34_8_34_32); __old_val_34_8_34_32 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos");
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			local(__new_val_34_8_34_32); __new_val_34_8_34_32 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos");
			recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_34_8_34_32, __new_val_34_8_34_32);
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_LastTime", 0.00000000);
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		Execute = deffunc(1)args(this, sender, instance, delta, curSectionTime){
			local(__method_ret_37_4_182_5);
			local(senderObj); senderObj = typeas(sender, GameFramework.Skill.GfxSkillSenderInfo, TypeKind.Class);
			if( execbinary("==", null, senderObj, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 40_8_42_9 ){
				__method_ret_37_4_182_5 = false;
				return(__method_ret_37_4_182_5);
			};
			if( execbinary("==", getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"), TableConfig.Skill, "type"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 43_8_45_9 ){
				__method_ret_37_4_182_5 = false;
				return(__method_ret_37_4_182_5);
				comment("track只能在impact或buff里使用");
			};
			local(obj); obj = getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "GfxObj");
			if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, obj), 47_8_181_9 ){
				if( execbinary(">=", curSectionTime, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 48_12_177_13 ){
					if( execunary("!", getinstance(SymbolKind.Field, this, Track2Trigger, "m_IsStarted"), System.Boolean, TypeKind.Structure), 49_16_173_17 ){
						setinstance(SymbolKind.Field, this, Track2Trigger, "m_IsStarted", true);
						local(dest);
						local(trackBone); trackBone = callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_TrackBone"), SkillSystem.SkillStringParam, "Get", instance);
						getinstance(SymbolKind.Field, this, Track2Trigger, "m_BoneTransform") = callexternstatic(GameFramework.Utility, "FindChildRecursive", getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), trackBone);
						if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, Track2Trigger, "m_BoneTransform")), 55_20_61_21 ){
							dest = getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_BoneTransform"), UnityEngine.Transform, "position");
						}else{
							dest = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
							setexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y", execbinary("+", getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y"), 1.50000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
							callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] trackbullet bone {2} can\'t find."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"), trackBone);
						};
						local(__old_val_62_20_62_133); __old_val_62_20_62_133 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos");
						getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos") = callexterninstancereturnstruct(getexternstatic(SymbolKind.Property, GameFramework.EntityController, "Instance"), GameFramework.EntityController, "GetImpactSenderPosition", getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ObjId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "Seq"));
						local(__new_val_62_20_62_133); __new_val_62_20_62_133 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos");
						recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_62_20_62_133, __new_val_62_20_62_133);
						local(speedObj);
						if( execclosure(true, __invoke_64_24_64_81, true){ multiassign(precode{
							},postcode{
							})varlist(__invoke_64_24_64_81, speedObj) = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", dslstrtocsstr("emitSpeed"), __cs2dsl_out); }, 64_20_68_21 ){
							setinstance(SymbolKind.Field, this, Track2Trigger, "m_Speed", typecast(speedObj, System.Single, TypeKind.Structure));
						}else{
							__method_ret_37_4_182_5 = false;
							return(__method_ret_37_4_182_5);
						};
						local(duration); duration = callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "Get", instance);
						setinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime", execbinary("/", duration, 1000.00000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
						if( execbinary(">", callexternstatic(GameFramework.Geometry, "DistanceSquare__Single__Single__Single__Single", getexterninstance(SymbolKind.Field, getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, "x"), getexterninstance(SymbolKind.Field, getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, "z"), getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "x"), getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "z")), 0.01000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 71_20_75_21 ){
							local(__old_val_72_24_72_99); __old_val_72_24_72_99 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos");
							getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos") = callexternstaticreturnstruct(GameFramework.Utility, "FrontOfTarget", wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), execbinary("*", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Speed"), getinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
							local(__new_val_72_24_72_99); __new_val_72_24_72_99 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos");
							recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_72_24_72_99, __new_val_72_24_72_99);
						}else{
							local(__old_val_74_24_74_94); __old_val_74_24_74_94 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos");
							getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos") = callexterninstancereturnstruct(getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "TransformPoint__Single__Single__Single", 0, 0, execbinary("*", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Speed"), getinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
							local(__new_val_74_24_74_94); __new_val_74_24_74_94 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos");
							recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_74_24_74_94, __new_val_74_24_74_94);
						};
						local(newSectionDuration); newSectionDuration = execbinary("+", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), typecast(( execbinary("*", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime"), 1000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure) ), System.Int64, TypeKind.Structure), System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure);
						if( execbinary("<", getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "CurSectionDuration"), newSectionDuration, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 78_20_80_21 ){
							callexterninstance(instance, SkillSystem.SkillInstance, "SetCurSectionDuration", newSectionDuration);
						};
						local(dir);
						local(dirObj);
						if( execclosure(true, __invoke_83_24_83_77, true){ multiassign(precode{
							},postcode{
							})varlist(__invoke_83_24_83_77, dirObj) = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", dslstrtocsstr("emitDir"), __cs2dsl_out); }, 83_20_87_21 ){
							dir = typecast(dirObj, UnityEngine.Quaternion, TypeKind.Structure);
						}else{
							dir = getexternstaticstructmember(SymbolKind.Property, UnityEngine.Quaternion, "identity");
						};
						local(scale);
						local(scaleObj);
						if( execclosure(true, __invoke_90_24_90_81, true){ multiassign(precode{
							},postcode{
							})varlist(__invoke_90_24_90_81, scaleObj) = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", dslstrtocsstr("emitScale"), __cs2dsl_out); }, 90_20_94_21 ){
							scale = typecast(scaleObj, UnityEngine.Vector3, TypeKind.Structure);
						}else{
							scale = getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "one");
						};
						local(lookDir); lookDir = invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field));
						local(q); q = callexternstaticreturnstruct(UnityEngine.Quaternion, "LookRotation__Vector3", wrapexternstructargument(lookDir, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local));
						local(__old_val_97_20_97_122); __old_val_97_20_97_122 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_ControlPos");
						setinstance(SymbolKind.Field, this, Track2Trigger, "m_ControlPos", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Addition", wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), callexternstaticreturnstruct(UnityEngine.Vector3, "Scale", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Quaternion, "op_Multiply__Quaternion__Vector3", invokeexternoperatorreturnstruct(UnityEngine.Quaternion, UnityEngine.Quaternion, "op_Multiply__Quaternion__Quaternion", wrapexternstructargument(q, UnityEngine.Quaternion, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(dir, UnityEngine.Quaternion, OperationKind.LocalReference, SymbolKind.Local)), getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "forward")), invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Multiply__Vector3__Single", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Multiply__Vector3__Single", wrapexternstructargument(scale, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), getexterninstance(SymbolKind.Property, lookDir, UnityEngine.Vector3, "magnitude")), 0.50000000))));
						local(__new_val_97_20_97_122); __new_val_97_20_97_122 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_ControlPos");
						recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_97_20_97_122, __new_val_97_20_97_122);
						local(effectPath); effectPath = callexternstatic(SkillSystem.SkillParamUtility, "RefixResourceVariable", dslstrtocsstr("emitEffect"), instance, getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"), TableConfig.Skill, "resources"));
						setinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect", typeas(callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.ResourceSystem, "Instance"), GameFramework.ResourceSystem, "NewObject__String__Single", effectPath, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime")), UnityEngine.GameObject, TypeKind.Class));
						if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect")), 100_20_121_21 ){
							setexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "TrackEffectObj", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"));
							callexternstatic(GameFramework.Skill.Trigers.TriggerUtil, "SetObjVisible", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), true);
							callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), UnityEngine.GameObject, "SetActive", false);
							setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position", getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"));
							setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "localRotation", q);
							callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), UnityEngine.GameObject, "SetActive", true);
							local(em); em = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "CustomDatas"), GameFramework.CustomDataCollection, "GetData__Type", GameFramework.Skill.Trigers.EffectManager);
							if( execbinary("==", em, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 109_24_112_25 ){
								em = newexternobject(GameFramework.Skill.Trigers.EffectManager, "g_GameFramework_Skill_Trigers_EffectManager", typeargs(), typekinds(), "ctor", 0, null);
								callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "CustomDatas"), GameFramework.CustomDataCollection, "AddData__Type__Object", GameFramework.Skill.Trigers.EffectManager, em);
							};
							callexterninstance(em, GameFramework.Skill.Trigers.EffectManager, "AddEffect", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"));
							callexterninstance(em, GameFramework.Skill.Trigers.EffectManager, "SetParticleSpeed", getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "EffectScale"));
						}else{
							if( callexternstatic(System.String, "IsNullOrEmpty", effectPath), 116_24_120_25 ){
								callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] trackbullet effect is empty."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"));
							}else{
								callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] trackbullet effect {2} can\'t find."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"), effectPath);
							};
						};
					}elseif( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect")), 122_18_173_17 ){
						local(dest);
						if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, Track2Trigger, "m_BoneTransform")), 124_20_129_21 ){
							dest = getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_BoneTransform"), UnityEngine.Transform, "position");
						}else{
							dest = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
							setexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y", execbinary("+", getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y"), 1.50000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
						};
						dest = callexternstaticreturnstruct(GameFramework.Utility, "FrontOfTarget", wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), 0.10000000);
						comment("m_Effect.transform.position = Vector3.MoveTowards(m_Effect.transform.position, m_TargetPos, m_RealSpeed * Time.deltaTime);");
						getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position") = callexternstaticreturnstruct(GameFramework.Utility, "GetBezierPoint", wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_ControlPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), execbinary("/", execbinary("/", ( execbinary("-", curSectionTime, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure) ), 1000.00000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), getinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
						local(pos); pos = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
						if( execunary("!", getinstance(SymbolKind.Field, this, Track2Trigger, "m_IsHit"), System.Boolean, TypeKind.Structure), 134_20_163_21 ){
							local(distSqr); distSqr = 340282300000000000000000000000000000000.00000000;
							if( execbinary(">", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos"), UnityEngine.Vector3, "sqrMagnitude"), 0.00010000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 136_24_143_25 ){
								local(np);
								local(targetPos); targetPos = newexternstruct(ScriptRuntime.Vector2, "g_ScriptRuntime_Vector2", typeargs(), typekinds(), "ctor__Single__Single", 0, null, getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "x"), getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "z"));
								local(lastPos); lastPos = newexternstruct(ScriptRuntime.Vector2, "g_ScriptRuntime_Vector2", typeargs(), typekinds(), "ctor__Single__Single", 0, null, getexterninstance(SymbolKind.Field, getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos"), UnityEngine.Vector3, "x"), getexterninstance(SymbolKind.Field, getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos"), UnityEngine.Vector3, "z"));
								multiassign(precode{
									},postcode{
										np = wrapexternstruct(np, ScriptRuntime.Vector2);
									})varlist(distSqr, np) = callexternstatic(GameFramework.Geometry, "PointToLineSegmentDistanceSquare__Vector2__Vector2__Vector2__O_Vector2", wrapexternstructargument(targetPos, ScriptRuntime.Vector2, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(lastPos, ScriptRuntime.Vector2, OperationKind.LocalReference, SymbolKind.Local), newexternstruct(ScriptRuntime.Vector2, "g_ScriptRuntime_Vector2", typeargs(), typekinds(), "ctor__Single__Single", 0, null, getexterninstance(SymbolKind.Field, pos, UnityEngine.Vector3, "x"), getexterninstance(SymbolKind.Field, pos, UnityEngine.Vector3, "z")), __cs2dsl_out);
							}else{
								distSqr = getexterninstance(SymbolKind.Property, ( invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(pos, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local)) ), UnityEngine.Vector3, "sqrMagnitude");
							};
							local(__old_val_144_24_144_39); __old_val_144_24_144_39 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos");
							setinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos", pos);
							getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos") = wrapexternstruct(getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos"), UnityEngine.Vector3);
							local(__new_val_144_24_144_39); __new_val_144_24_144_39 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos");
							recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_144_24_144_39, __new_val_144_24_144_39);
							if( execbinary("<=", distSqr, getinstance(SymbolKind.Field, this, Track2Trigger, "m_BulletRadiusSquare"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 145_24_162_25 ){
								local(curTime); curTime = getexternstatic(SymbolKind.Property, UnityEngine.Time, "time");
								local(interval); interval = execbinary("/", callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_DamageInterval"), SkillSystem.SkillNonStringParam_T, "Get", instance), 1000.00000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure);
								if( execbinary("<=", execbinary("+", getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastTime"), interval, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), curTime, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 148_28_161_29 ){
									setinstance(SymbolKind.Field, this, Track2Trigger, "m_LastTime", curTime);
									local(__old_val_151_32_151_96); __old_val_151_32_151_96 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_HitEffectRotation");
									getinstance(SymbolKind.Field, this, Track2Trigger, "m_HitEffectRotation") = callexternstaticreturnstruct(UnityEngine.Quaternion, "LookRotation__Vector3", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local)));
									local(__new_val_151_32_151_96); __new_val_151_32_151_96 = getinstance(SymbolKind.Field, this, Track2Trigger, "m_HitEffectRotation");
									recycleandkeepstructvalue(UnityEngine.Quaternion, __old_val_151_32_151_96, __new_val_151_32_151_96);
									local(impactId); impactId = callexternstatic(GameFramework.Skill.Trigers.TriggerUtil, "GetSkillImpactId", getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"));
									local(args);
									multiassign(precode{
										},postcode{
										})varlist(args) = callexternstatic(GameFramework.Skill.Trigers.TriggerUtil, "CalcImpactConfig", 0, impactId, instance, getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"), __cs2dsl_out);
									if( callexterninstance(args, System.Collections.Generic.Dictionary_TKey_TValue, "ContainsKey", dslstrtocsstr("hitEffectRotation")), 155_32_158_87 ){
										setexterninstanceindexer(System.Object, TypeKind.Class, System.Collections.Generic.Dictionary_TKey_TValue, args, System.Collections.Generic.Dictionary_TKey_TValue, "set_Item", 2, dslstrtocsstr("hitEffectRotation"), getinstance(SymbolKind.Field, this, Track2Trigger, "m_HitEffectRotation"));
									}else{
										callexterninstance(args, System.Collections.Generic.Dictionary_TKey_TValue, "Add", dslstrtocsstr("hitEffectRotation"), getinstance(SymbolKind.Field, this, Track2Trigger, "m_HitEffectRotation"));
									};
									callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.EntityController, "Instance"), GameFramework.EntityController, "TrackSendImpact", getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ObjId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "Seq"), impactId, args);
									comment("m_IsHit = true;");
								};
							};
						};
						if( execbinary(">", curSectionTime, execbinary("+", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), execbinary("*", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Lifetime"), 1000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 164_20_170_21 ){
							callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"), UnityEngine.GameObject, "SetActive", false);
							callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.ResourceSystem, "Instance"), GameFramework.ResourceSystem, "RecycleObject", getinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect"));
							setinstance(SymbolKind.Field, this, Track2Trigger, "m_Effect", null);
							callexterninstance(instance, SkillSystem.SkillInstance, "StopCurSection");
							__method_ret_37_4_182_5 = false;
							return(__method_ret_37_4_182_5);
						};
					}else{
						__method_ret_37_4_182_5 = false;
						return(__method_ret_37_4_182_5);
					};
					__method_ret_37_4_182_5 = true;
					return(__method_ret_37_4_182_5);
				}else{
					__method_ret_37_4_182_5 = true;
					return(__method_ret_37_4_182_5);
				};
			}else{
				callexterninstance(instance, SkillSystem.SkillInstance, "StopCurSection");
				__method_ret_37_4_182_5 = false;
				return(__method_ret_37_4_182_5);
			};
			return(null);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(sender, System.Object, TypeKind.Class, 0, true), paramtype(instance, SkillSystem.SkillInstance, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true), paramtype(curSectionTime, System.Int64, TypeKind.Structure, 0, true)];
		OnInitProperties = deffunc(0)args(this){
			callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "AddProperty", dslstrtocsstr("TrackBone"), deffunc(1)args(){
				local(__method_ret_185_48_185_91);
				__method_ret_185_48_185_91 = getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TrackBone"), SkillSystem.SkillStringParam, "EditableValue");
				return(__method_ret_185_48_185_91);
			}options[needfuncinfo(false), rettype(return, System.Object, TypeKind.Class, 0, true)], deffunc(0)args(val){
				setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TrackBone"), SkillSystem.SkillStringParam, "EditableValue", val);
			}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(val, System.Object, TypeKind.Class, 0, true)]);
			callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "AddProperty", dslstrtocsstr("Duration"), deffunc(1)args(){
				local(__method_ret_186_47_186_89);
				__method_ret_186_47_186_89 = getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "EditableValue");
				return(__method_ret_186_47_186_89);
			}options[needfuncinfo(false), rettype(return, System.Object, TypeKind.Class, 0, true)], deffunc(0)args(val){
				setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "EditableValue", val);
			}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(val, System.Object, TypeKind.Class, 0, true)]);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		LoadCallData = deffunc(0)args(this, callData, instance){
			local(num); num = callexterninstance(callData, Dsl.FunctionData, "GetParamNum");
			if( execbinary(">", num, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 191_8_193_9 ){
				callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_TrackBone"), SkillSystem.SkillStringParam, "Set__ISyntaxComponent", callexterninstance(callData, Dsl.FunctionData, "GetParam", 0));
			};
			if( execbinary(">", num, 1, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 194_8_196_9 ){
				callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "Set__ISyntaxComponent", callexterninstance(callData, Dsl.FunctionData, "GetParam", 1));
			};
			if( execbinary(">", num, 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 197_8_199_9 ){
				callexterninstance(getinstance(SymbolKind.Field, this, Track2Trigger, "m_DamageInterval"), SkillSystem.SkillNonStringParam_T, "Set__ISyntaxComponent", callexterninstance(callData, Dsl.FunctionData, "GetParam", 2));
			};
			if( execbinary(">", num, 3, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 200_8_203_9 ){
				getinstance(SymbolKind.Field, this, Track2Trigger, "m_BulletRadius") = callexternstatic(System.Single, "Parse__String", callexterninstance(callData, Dsl.FunctionData, "GetParamId", 3));
				setinstance(SymbolKind.Field, this, Track2Trigger, "m_BulletRadiusSquare", execbinary("*", getinstance(SymbolKind.Field, this, Track2Trigger, "m_BulletRadius"), getinstance(SymbolKind.Field, this, Track2Trigger, "m_BulletRadius"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
			};
			if( execbinary(">", num, 4, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 204_8_206_9 ){
				getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime") = callexternstatic(System.Int64, "Parse__String", callexterninstance(callData, Dsl.FunctionData, "GetParamId", 4));
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(callData, Dsl.FunctionData, TypeKind.Class, 0, true), paramtype(instance, SkillSystem.SkillInstance, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, Track2Trigger, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, Track2Trigger, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, Track2Trigger, "__ctor_called", true);
			};
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_TrackBone", newexternobject(SkillSystem.SkillStringParam, "g_SkillSystem_SkillStringParam", typeargs(), typekinds(), "ctor", 0, null));
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_Duration", newexternobject(SkillSystem.SkillLongParam, "g_SkillSystem_SkillLongParam", typeargs(), typekinds(), "ctor", 0, null));
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_DamageInterval", newexternobject(SkillSystem.SkillLongParam, "g_SkillSystem_SkillLongParam", typeargs(), typekinds(), "ctor", 0, null));
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, Track2Trigger, "m_StartPos"));
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_ControlPos", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, Track2Trigger, "m_ControlPos"));
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, Track2Trigger, "m_TargetPos"));
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, Track2Trigger, "m_LastPos"));
			setinstance(SymbolKind.Field, this, Track2Trigger, "m_HitEffectRotation", newexternstruct(UnityEngine.Quaternion, "g_UnityEngine_Quaternion", typeargs(), typekinds(), "ctor", 0, null));
			recycleandkeepstructvalue(UnityEngine.Quaternion, nil, getinstance(SymbolKind.Field, this, Track2Trigger, "m_HitEffectRotation"));
		}options[needfuncinfo(true)];
	};
	instance_fields {
		m_TrackBone = null;
		m_Duration = null;
		m_DamageInterval = null;
		m_BulletRadius = 0.10000000;
		m_BulletRadiusSquare = 0.01000000;
		m_StartPos = null;
		m_ControlPos = null;
		m_TargetPos = null;
		m_LastPos = null;
		m_LastTime = 0.00000000;
		m_Speed = 10.00000000;
		m_Lifetime = 1.00000000;
		m_IsStarted = false;
		m_HitEffectRotation = null;
		m_Effect = null;
		m_BoneTransform = null;
		m_IsHit = false;
		m_TriggerProxy = null;
		__attributes = Track2Trigger__Attrs;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {
		"GameFramework.Plugin.ISkillTriggerPlugin";
	};

	class_info(TypeKind.Class, Accessibility.Public) {
	};
	method_info {
		SetProxy(MethodKind.Ordinary, Accessibility.Public){
		};
		Clone(MethodKind.Ordinary, Accessibility.Public){
		};
		Reset(MethodKind.Ordinary, Accessibility.Public){
		};
		Execute(MethodKind.Ordinary, Accessibility.Public){
		};
		OnInitProperties(MethodKind.Ordinary, Accessibility.Public){
		};
		LoadCallData(MethodKind.Ordinary, Accessibility.Public){
		};
		LoadFuncData(MethodKind.Ordinary, Accessibility.Public){
		};
		LoadStatementData(MethodKind.Ordinary, Accessibility.Public){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



