require("cs2dsl__syslib");
require("cs2dsl__attributes");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");
require("bulletmanager");

class(TrackBulletTrigger) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(TrackBulletTrigger, "g_TrackBulletTrigger", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(TrackBulletTrigger, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(TrackBulletTrigger.__cctor_called){
				return();
			}else{
				TrackBulletTrigger.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__attributes = TrackBulletTrigger__Attrs;
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		SetProxy = deffunc(0)args(this, triggerProxy){
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy", triggerProxy);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(triggerProxy, SkillSystem.SkillTriggerProxy, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_16_4_27_5);
			local(triger); triger = newobject(TrackBulletTrigger, "g_TrackBulletTrigger", typeargs(), typekinds(), "ctor", 0, null);
			callexterninstance(getinstance(SymbolKind.Field, triger, TrackBulletTrigger, "m_TrackBone"), SkillSystem.SkillStringParam, "CopyFrom", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TrackBone"));
			setinstance(SymbolKind.Field, triger, TrackBulletTrigger, "m_NoImpact", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_NoImpact"));
			callexterninstance(getinstance(SymbolKind.Field, triger, TrackBulletTrigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "CopyFrom", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Duration"));
			setinstance(SymbolKind.Field, triger, TrackBulletTrigger, "m_NotMove", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_NotMove"));
			callexterninstance(getinstance(SymbolKind.Field, triger, TrackBulletTrigger, "m_EffectPath"), SkillSystem.SkillResourceParam, "CopyFrom", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_EffectPath"));
			callexterninstance(getinstance(SymbolKind.Field, triger, TrackBulletTrigger, "m_DeleteTime"), SkillSystem.SkillNonStringParam_T, "CopyFrom", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_DeleteTime"));
			__method_ret_16_4_27_5 = triger;
			return(__method_ret_16_4_27_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.ISkillTriggerPlugin, TypeKind.Interface, 0, true)];
		Reset = deffunc(0)args(this){
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsStarted", false);
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect", null);
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_BoneTransform", null);
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime", 0);
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsHit", false);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		Execute = deffunc(1)args(this, sender, instance, delta, curSectionTime){
			local(__method_ret_36_4_185_5);
			local(senderObj); senderObj = typeas(sender, GameFramework.Skill.GfxSkillSenderInfo, TypeKind.Class);
			if( execbinary("==", null, senderObj, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 39_8_41_9 ){
				__method_ret_36_4_185_5 = false;
				return(__method_ret_36_4_185_5);
			};
			if( execbinary("==", getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"), TableConfig.Skill, "type"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 42_8_44_9 ){
				__method_ret_36_4_185_5 = false;
				return(__method_ret_36_4_185_5);
				comment("track只能在impact或buff里使用");
			};
			local(obj); obj = getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "GfxObj");
			if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, obj), 46_8_184_9 ){
				if( execbinary(">=", curSectionTime, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 47_12_180_13 ){
					if( execunary("!", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsStarted"), System.Boolean, TypeKind.Structure), 48_16_176_17 ){
						setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsStarted", true);
						comment("LogSystem.Warn(\"trackbullet start.\");");
						local(dest);
						local(trackBone); trackBone = callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TrackBone"), SkillSystem.SkillStringParam, "Get", instance);
						getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_BoneTransform") = callexternstatic(GameFramework.Utility, "FindChildRecursive", getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), trackBone);
						if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_BoneTransform")), 56_20_62_21 ){
							dest = getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_BoneTransform"), UnityEngine.Transform, "position");
						}else{
							dest = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
							setexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y", execbinary("+", getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y"), 1.50000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
							callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] trackbullet bone {2} can\'t find."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"), trackBone);
						};
						local(__old_val_63_20_63_133); __old_val_63_20_63_133 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos");
						getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos") = callexterninstancereturnstruct(getexternstatic(SymbolKind.Property, GameFramework.EntityController, "Instance"), GameFramework.EntityController, "GetImpactSenderPosition", getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ObjId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "Seq"));
						local(__new_val_63_20_63_133); __new_val_63_20_63_133 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos");
						recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_63_20_63_133, __new_val_63_20_63_133);
						dest = callexternstaticreturnstruct(GameFramework.Utility, "FrontOfTarget", wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), 0.10000000);
						local(speedObj);
						if( execclosure(true, __invoke_66_24_66_81, true){ multiassign(precode{
							},postcode{
							})varlist(__invoke_66_24_66_81, speedObj) = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", dslstrtocsstr("emitSpeed"), __cs2dsl_out); }, 66_20_70_21 ){
							setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Speed", typecast(speedObj, System.Single, TypeKind.Structure));
						}else{
							__method_ret_36_4_185_5 = false;
							return(__method_ret_36_4_185_5);
						};
						local(duration); duration = callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "Get", instance);
						if( execbinary(">", duration, 0.00010000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 72_20_81_21 ){
							local(d); d = execbinary("/", duration, 1000.00000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure);
							setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime", d);
							setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Speed", execbinary("/", getexterninstance(SymbolKind.Property, ( invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field)) ), UnityEngine.Vector3, "magnitude"), getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
						}else{
							setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime", 1.00000000);
							if( execbinary(">", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Speed"), 0.00010000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 78_24_80_25 ){
								setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime", execbinary("/", getexterninstance(SymbolKind.Property, ( invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field)) ), UnityEngine.Vector3, "magnitude"), getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Speed"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
							};
						};
						local(newSectionDuration); newSectionDuration = execbinary("+", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), typecast(( execbinary("*", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime"), 1000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure) ), System.Int64, TypeKind.Structure), System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure);
						if( execbinary("<", getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "CurSectionDuration"), newSectionDuration, System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure), 83_20_85_21 ){
							callexterninstance(instance, SkillSystem.SkillInstance, "SetCurSectionDuration", newSectionDuration);
						};
						local(dir);
						local(dirObj);
						if( execclosure(true, __invoke_88_24_88_77, true){ multiassign(precode{
							},postcode{
							})varlist(__invoke_88_24_88_77, dirObj) = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", dslstrtocsstr("emitDir"), __cs2dsl_out); }, 88_20_92_21 ){
							dir = typecast(dirObj, UnityEngine.Quaternion, TypeKind.Structure);
						}else{
							dir = getexternstaticstructmember(SymbolKind.Property, UnityEngine.Quaternion, "identity");
						};
						local(scale);
						local(scaleObj);
						if( execclosure(true, __invoke_95_24_95_81, true){ multiassign(precode{
							},postcode{
							})varlist(__invoke_95_24_95_81, scaleObj) = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", dslstrtocsstr("emitScale"), __cs2dsl_out); }, 95_20_99_21 ){
							scale = typecast(scaleObj, UnityEngine.Vector3, TypeKind.Structure);
						}else{
							scale = getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "one");
						};
						local(lookDir); lookDir = invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field));
						local(q); q = callexternstaticreturnstruct(UnityEngine.Quaternion, "LookRotation__Vector3", wrapexternstructargument(lookDir, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local));
						local(__old_val_102_20_102_122); __old_val_102_20_102_122 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_ControlPos");
						setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_ControlPos", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Addition", wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), callexternstaticreturnstruct(UnityEngine.Vector3, "Scale", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Quaternion, "op_Multiply__Quaternion__Vector3", invokeexternoperatorreturnstruct(UnityEngine.Quaternion, UnityEngine.Quaternion, "op_Multiply__Quaternion__Quaternion", wrapexternstructargument(q, UnityEngine.Quaternion, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(dir, UnityEngine.Quaternion, OperationKind.LocalReference, SymbolKind.Local)), getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "forward")), invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Multiply__Vector3__Single", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Multiply__Vector3__Single", wrapexternstructargument(scale, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), getexterninstance(SymbolKind.Property, lookDir, UnityEngine.Vector3, "magnitude")), 0.50000000))));
						local(__new_val_102_20_102_122); __new_val_102_20_102_122 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_ControlPos");
						recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_102_20_102_122, __new_val_102_20_102_122);
						local(effectPath); effectPath = callexternstatic(SkillSystem.SkillParamUtility, "RefixResourceVariable", dslstrtocsstr("emitEffect"), instance, getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"), TableConfig.Skill, "resources"));
						setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect", typeas(callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.ResourceSystem, "Instance"), GameFramework.ResourceSystem, "NewObject__String__Single", effectPath, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime")), UnityEngine.GameObject, TypeKind.Class));
						if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect")), 105_20_123_21 ){
							comment("LogSystem.Warn(\"trackbullet effect {0} {1}\", effectPath, m_Lifetime);");
							callinstance(getstatic(SymbolKind.Property, BulletManager, "Instance"), BulletManager, "AddBullet", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"));
							setexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "TrackEffectObj", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"));
							callexternstatic(GameFramework.Skill.Trigers.TriggerUtil, "SetObjVisible", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), true);
							callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "SetActive", false);
							setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"));
							setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "localRotation", q);
							callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "SetActive", true);
							comment("LogSystem.Warn(\"trackbullet effect actived {0} {1} pos {2} {3} {4}\", effectPath, m_Lifetime, m_StartPos.x, m_StartPos.y, m_StartPos.z);");
						}else{
							if( callexternstatic(System.String, "IsNullOrEmpty", effectPath), 118_24_122_25 ){
								callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] trackbullet effect is empty."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"));
							}else{
								callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] trackbullet effect {2} can\'t find."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"), effectPath);
							};
						};
					}elseif( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect")), 124_18_176_17 ){
						if( execbinary("&&", execunary("!", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_NotMove"), System.Boolean, TypeKind.Structure), execunary("!", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsHit"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 125_20_159_21 ){
							local(dest);
							if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_BoneTransform")), 127_24_132_25 ){
								dest = getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_BoneTransform"), UnityEngine.Transform, "position");
							}else{
								dest = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
								setexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y", execbinary("+", getexterninstance(SymbolKind.Field, dest, UnityEngine.Vector3, "y"), 1.50000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
							};
							dest = callexternstaticreturnstruct(GameFramework.Utility, "FrontOfTarget", wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), 0.10000000);
							comment("m_Effect.transform.position = Vector3.MoveTowards(m_Effect.transform.position, dest, m_RealSpeed * Time.deltaTime);");
							getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position") = callexternstaticreturnstruct(GameFramework.Utility, "GetBezierPoint", wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_ControlPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), execbinary("/", execbinary("/", ( execbinary("-", curSectionTime, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), System.Int64, System.Int64, TypeKind.Structure, TypeKind.Structure) ), 1000.00000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
							local(pos); pos = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
							if( callinstance(this, TrackBulletTrigger, "CheckCollide", senderObj, instance, obj), 137_24_139_25 ){
								__method_ret_36_4_185_5 = true;
								return(__method_ret_36_4_185_5);
							};
							comment("LogSystem.Warn(\"trackbullet effect move to {0} {1} {2}\", pos.x, pos.y, pos.z);");
							if( execbinary("<=", getexterninstance(SymbolKind.Property, ( invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position")) ), UnityEngine.Vector3, "sqrMagnitude"), 0.01000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 141_24_158_25 ){
								local(__old_val_142_28_142_92); __old_val_142_28_142_92 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation");
								getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation") = callexternstaticreturnstruct(UnityEngine.Quaternion, "LookRotation__Vector3", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), wrapexternstructargument(dest, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local)));
								local(__new_val_142_28_142_92); __new_val_142_28_142_92 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation");
								recycleandkeepstructvalue(UnityEngine.Quaternion, __old_val_142_28_142_92, __new_val_142_28_142_92);
								if( getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_NoImpact"), 143_28_156_29 ){
									callexterninstance(instance, SkillSystem.SkillInstance, "SetVariable", dslstrtocsstr("hitEffectRotation"), getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation"));
								}else{
									local(impactId); impactId = callexternstatic(GameFramework.Skill.Trigers.TriggerUtil, "GetSkillImpactId", getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "Variables"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"));
									local(args);
									multiassign(precode{
										},postcode{
										})varlist(args) = callexternstatic(GameFramework.Skill.Trigers.TriggerUtil, "CalcImpactConfig", 0, impactId, instance, getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"), __cs2dsl_out);
									if( callexterninstance(args, System.Collections.Generic.Dictionary_TKey_TValue, "ContainsKey", dslstrtocsstr("hitEffectRotation")), 149_32_152_87 ){
										setexterninstanceindexer(System.Object, TypeKind.Class, System.Collections.Generic.Dictionary_TKey_TValue, args, System.Collections.Generic.Dictionary_TKey_TValue, "set_Item", 2, dslstrtocsstr("hitEffectRotation"), getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation"));
									}else{
										callexterninstance(args, System.Collections.Generic.Dictionary_TKey_TValue, "Add", dslstrtocsstr("hitEffectRotation"), getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation"));
									};
									callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.EntityController, "Instance"), GameFramework.EntityController, "TrackSendImpact", getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ObjId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "Seq"), impactId, args);
									comment("LogSystem.Warn(\"trackbullet effect hit target {0} {1} {2}\", pos.x, pos.y, pos.z);");
								};
								setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsHit", true);
							};
						};
						callinstance(getstatic(SymbolKind.Property, BulletManager, "Instance"), BulletManager, "UpdatePos", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"));
						if( callinstance(this, TrackBulletTrigger, "CheckCollide", senderObj, instance, obj), 161_20_163_21 ){
							__method_ret_36_4_185_5 = true;
							return(__method_ret_36_4_185_5);
						};
						if( execbinary("||", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsHit"), execbinary(">", curSectionTime, execbinary("+", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime"), execbinary("*", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Lifetime"), 1000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 164_20_173_21 ){
							callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "SetActive", false);
							callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.ResourceSystem, "Instance"), GameFramework.ResourceSystem, "RecycleObject", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"));
							callinstance(getstatic(SymbolKind.Property, BulletManager, "Instance"), BulletManager, "RemoveBullet", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"));
							setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect", null);
							callexterninstance(instance, SkillSystem.SkillInstance, "StopCurSection");
							comment("LogSystem.Warn(\"trackbullet effect finish.\");");
							__method_ret_36_4_185_5 = false;
							return(__method_ret_36_4_185_5);
						};
					}else{
						__method_ret_36_4_185_5 = false;
						return(__method_ret_36_4_185_5);
					};
					__method_ret_36_4_185_5 = true;
					return(__method_ret_36_4_185_5);
				}else{
					__method_ret_36_4_185_5 = true;
					return(__method_ret_36_4_185_5);
				};
			}else{
				callexterninstance(instance, SkillSystem.SkillInstance, "StopCurSection");
				__method_ret_36_4_185_5 = false;
				return(__method_ret_36_4_185_5);
			};
			return(null);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(sender, System.Object, TypeKind.Class, 0, true), paramtype(instance, SkillSystem.SkillInstance, TypeKind.Class, 0, true), paramtype(delta, System.Int64, TypeKind.Structure, 0, true), paramtype(curSectionTime, System.Int64, TypeKind.Structure, 0, true)];
		OnInitProperties = deffunc(0)args(this){
			callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "AddProperty", dslstrtocsstr("TrackBone"), deffunc(1)args(){
				local(__method_ret_188_48_188_91);
				__method_ret_188_48_188_91 = getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TrackBone"), SkillSystem.SkillStringParam, "EditableValue");
				return(__method_ret_188_48_188_91);
			}options[needfuncinfo(false), rettype(return, System.Object, TypeKind.Class, 0, true)], deffunc(0)args(val){
				setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TrackBone"), SkillSystem.SkillStringParam, "EditableValue", val);
			}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(val, System.Object, TypeKind.Class, 0, true)]);
			callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "AddProperty", dslstrtocsstr("Duration"), deffunc(1)args(){
				local(__method_ret_189_47_189_89);
				__method_ret_189_47_189_89 = getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "EditableValue");
				return(__method_ret_189_47_189_89);
			}options[needfuncinfo(false), rettype(return, System.Object, TypeKind.Class, 0, true)], deffunc(0)args(val){
				setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "EditableValue", val);
			}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(val, System.Object, TypeKind.Class, 0, true)]);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		LoadCallData = deffunc(0)args(this, callData, instance){
			local(num); num = callexterninstance(callData, Dsl.FunctionData, "GetParamNum");
			if( execbinary(">", num, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 194_8_196_9 ){
				callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TrackBone"), SkillSystem.SkillStringParam, "Set__ISyntaxComponent", callexterninstance(callData, Dsl.FunctionData, "GetParam", 0));
			};
			if( execbinary(">", num, 1, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 197_8_199_9 ){
				setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_NoImpact", execbinary("==", callexterninstance(callData, Dsl.FunctionData, "GetParamId", 1), "true", System.String, System.String, TypeKind.Class, TypeKind.Class));
			};
			if( execbinary(">", num, 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 200_8_202_9 ){
				getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TriggerProxy"), SkillSystem.SkillTriggerProxy, "StartTime") = callexternstatic(System.Int64, "Parse__String", callexterninstance(callData, Dsl.FunctionData, "GetParamId", 2));
			};
			if( execbinary(">", num, 3, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 203_8_205_9 ){
				callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Duration"), SkillSystem.SkillNonStringParam_T, "Set__ISyntaxComponent", callexterninstance(callData, Dsl.FunctionData, "GetParam", 3));
			};
			if( execbinary(">", num, 4, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 206_8_208_9 ){
				setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_NotMove", execbinary("==", callexterninstance(callData, Dsl.FunctionData, "GetParamId", 4), "true", System.String, System.String, TypeKind.Class, TypeKind.Class));
			};
			if( execbinary(">", num, 5, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 209_8_211_9 ){
				callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_EffectPath"), SkillSystem.SkillResourceParam, "Set__ISyntaxComponent", callexterninstance(callData, Dsl.FunctionData, "GetParam", 5));
			};
			if( execbinary(">", num, 6, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 212_8_214_9 ){
				callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_DeleteTime"), SkillSystem.SkillNonStringParam_T, "Set__ISyntaxComponent", callexterninstance(callData, Dsl.FunctionData, "GetParam", 6));
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(callData, Dsl.FunctionData, TypeKind.Class, 0, true), paramtype(instance, SkillSystem.SkillInstance, TypeKind.Class, 0, true)];
		ShowExplodeEffect = deffunc(0)args(this, obj, senderObj, instance){
			local(deleteTime); deleteTime = callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_DeleteTime"), SkillSystem.SkillNonStringParam_T, "Get", instance);
			if( execbinary("<=", deleteTime, 0, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 230_8_233_9 ){
				callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] explode effect deleteTime <= 0."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"));
				return();
			};
			local(effectPath); effectPath = callexterninstance(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_EffectPath"), SkillSystem.SkillResourceParam, "Get__SkillInstance__Dictionary_2_String_String", instance, getexterninstance(SymbolKind.Field, getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "ConfigData"), TableConfig.Skill, "resources"));
			local(effectObj); effectObj = null;
			if( callexternstatic(System.String, "IsNullOrEmpty", effectPath), 236_8_243_9 ){
				callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] explode effect is empty."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"));
			}else{
				effectObj = typeas(callexterninstance(getexternstatic(SymbolKind.Property, GameFramework.ResourceSystem, "Instance"), GameFramework.ResourceSystem, "NewObject__String__Single", effectPath, execbinary("/", deleteTime, 1000.00000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure)), UnityEngine.GameObject, TypeKind.Class);
				if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Equality", null, effectObj), 240_12_242_13 ){
					callexternstatic(GameFramework.LogSystem, "Warn", dslstrtocsstr("[skill:{0} dsl skill id:{1}] selfeffect effect {2} can\'t find."), getexterninstance(SymbolKind.Property, senderObj, GameFramework.Skill.GfxSkillSenderInfo, "SkillId"), getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "DslSkillId"), effectPath);
				};
			};
			if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, effectObj), 244_8_263_9 ){
				callexternstatic(GameFramework.Skill.Trigers.TriggerUtil, "SetObjVisible", effectObj, true);
				callexterninstance(effectObj, UnityEngine.GameObject, "SetActive", false);
				if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform")), 247_12_261_13 ){
					callexterninstance(getexterninstance(SymbolKind.Property, effectObj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "SetParent__Transform", getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"));
					setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, effectObj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "localPosition", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
					setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, effectObj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "localScale", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "one"));
					setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, effectObj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "rotation", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation"));
					callexterninstance(getexterninstance(SymbolKind.Property, effectObj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "SetParent__Transform", null);
					local(em); em = callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "CustomDatas"), GameFramework.CustomDataCollection, "GetData__Type", GameFramework.Skill.Trigers.EffectManager);
					if( execbinary("==", em, null, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 255_16_258_17 ){
						em = newexternobject(GameFramework.Skill.Trigers.EffectManager, "g_GameFramework_Skill_Trigers_EffectManager", typeargs(), typekinds(), "ctor", 0, null);
						callexterninstance(getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "CustomDatas"), GameFramework.CustomDataCollection, "AddData__Type__Object", GameFramework.Skill.Trigers.EffectManager, em);
					};
					callexterninstance(em, GameFramework.Skill.Trigers.EffectManager, "AddEffect", effectObj);
					callexterninstance(em, GameFramework.Skill.Trigers.EffectManager, "SetParticleSpeed", getexterninstance(SymbolKind.Property, instance, SkillSystem.SkillInstance, "EffectScale"));
				};
				callexterninstance(effectObj, UnityEngine.GameObject, "SetActive", true);
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(obj, UnityEngine.GameObject, TypeKind.Class, 0, true), paramtype(senderObj, GameFramework.Skill.GfxSkillSenderInfo, TypeKind.Class, 0, true), paramtype(instance, SkillSystem.SkillInstance, TypeKind.Class, 0, true)];
		CheckCollide = deffunc(1)args(this, senderObj, instance, obj){
			local(__method_ret_266_4_280_5);
			local(other); other = callinstance(getstatic(SymbolKind.Property, BulletManager, "Instance"), BulletManager, "GetCollideObject", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"));
			if( execbinary("&&", invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, other), execunary("!", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsHit"), System.Boolean, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 269_8_278_9 ){
				setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_IsHit", true);
				local(__old_val_271_12_271_94); __old_val_271_12_271_94 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation");
				getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation") = callexternstaticreturnstruct(UnityEngine.Quaternion, "LookRotation__Vector3", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"), UnityEngine.Vector3, OperationKind.FieldReference, SymbolKind.Field), getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, obj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position")));
				local(__new_val_271_12_271_94); __new_val_271_12_271_94 = getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation");
				recycleandkeepstructvalue(UnityEngine.Quaternion, __old_val_271_12_271_94, __new_val_271_12_271_94);
				callinstance(this, TrackBulletTrigger, "ShowExplodeEffect", getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), senderObj, instance);
				local(pos1); pos1 = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Effect"), UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
				local(pos2); pos2 = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, other, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
				comment("LogSystem.Warn(\"trackbullet effect explode {0}({1} {2} {3}) with {4}({5} {6} {7})\", m_Effect.name, pos1.x, pos1.y, pos1.z, other.name, pos2.x, pos2.y, pos2.z);");
				__method_ret_266_4_280_5 = true;
				return(__method_ret_266_4_280_5);
			};
			__method_ret_266_4_280_5 = false;
			return(__method_ret_266_4_280_5);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), paramtype(senderObj, GameFramework.Skill.GfxSkillSenderInfo, TypeKind.Class, 0, true), paramtype(instance, SkillSystem.SkillInstance, TypeKind.Class, 0, true), paramtype(obj, UnityEngine.GameObject, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, TrackBulletTrigger, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, TrackBulletTrigger, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, TrackBulletTrigger, "__ctor_called", true);
			};
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_TrackBone", newexternobject(SkillSystem.SkillStringParam, "g_SkillSystem_SkillStringParam", typeargs(), typekinds(), "ctor", 0, null));
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_Duration", newexternobject(SkillSystem.SkillLongParam, "g_SkillSystem_SkillLongParam", typeargs(), typekinds(), "ctor", 0, null));
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_EffectPath", newexternobject(SkillSystem.SkillResourceParam, "g_SkillSystem_SkillResourceParam", typeargs(), typekinds(), "ctor", 0, null));
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_DeleteTime", newexternobject(SkillSystem.SkillLongParam, "g_SkillSystem_SkillLongParam", typeargs(), typekinds(), "ctor", 0, null));
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_StartPos"));
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_ControlPos", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_ControlPos"));
			setinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation", newexternstruct(UnityEngine.Quaternion, "g_UnityEngine_Quaternion", typeargs(), typekinds(), "ctor", 0, null));
			recycleandkeepstructvalue(UnityEngine.Quaternion, nil, getinstance(SymbolKind.Field, this, TrackBulletTrigger, "m_HitEffectRotation"));
		}options[needfuncinfo(true)];
	};
	instance_fields {
		m_TrackBone = null;
		m_NoImpact = false;
		m_Duration = null;
		m_NotMove = false;
		m_EffectPath = null;
		m_DeleteTime = null;
		m_StartPos = null;
		m_ControlPos = null;
		m_Speed = 10.00000000;
		m_Lifetime = 1.00000000;
		m_IsStarted = false;
		m_HitEffectRotation = null;
		m_Effect = null;
		m_BoneTransform = null;
		m_IsHit = false;
		m_TriggerProxy = null;
		__attributes = TrackBulletTrigger__Attrs;
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
		ShowExplodeEffect(MethodKind.Ordinary, Accessibility.Private){
		};
		CheckCollide(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



