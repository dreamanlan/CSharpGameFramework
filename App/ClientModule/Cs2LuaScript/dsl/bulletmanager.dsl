require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(BulletManager) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(BulletManager, "g_BulletManager", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		get_Instance = deffunc(1)args(){
			local(__method_ret_83_4_89_5);
			__method_ret_83_4_89_5 = getstatic(SymbolKind.Field, BulletManager, "s_Instance");
			return(__method_ret_83_4_89_5);
		}options[needfuncinfo(false), rettype(return, BulletManager, TypeKind.Class, 0, false)];
		cctor = deffunc(0)args(){
			callstatic(BulletManager, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(BulletManager.__cctor_called){
				return();
			}else{
				BulletManager.__cctor_called = true;
			};
			setstatic(SymbolKind.Field, BulletManager, "s_Instance", newobject(BulletManager, "g_BulletManager", typeargs(), typekinds(), "ctor", 0, null));
		}options[needfuncinfo(false)];
	};
	static_fields {
		s_Instance = null;
		__cctor_called = false;
	};
	static_props {
		Instance = {
			get = static_methods.get_Instance;
		};
	};
	static_events {};

	instance_methods {
		GetCollideObject = deffunc(1)args(this, bullet){
			local(__method_ret_12_4_38_5);
			local(pos1); pos1 = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, bullet, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
			foreach(__foreach_15_8_36_9, pair, getinstance(SymbolKind.Field, this, BulletManager, "m_Bullets"), System.Collections.Generic.Dictionary_TKey_TValue, System.Collections.Generic.Dictionary_TKey_TValue, true){
				local(other); other = getexterninstance(SymbolKind.Property, pair, System.Collections.Generic.KeyValuePair_TKey_TValue, "Value");
				local(otherObj); otherObj = getinstance(SymbolKind.Field, other, BulletManager.CollideInfo, "Obj");
				if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", bullet, otherObj), 18_12_35_13 ){
					local(pos2); pos2 = getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, otherObj, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position");
					local(lastPos2); lastPos2 = callinstance(other, BulletManager.CollideInfo, "GetLastPos");
					lastPos2 = wrapexternstruct(lastPos2, UnityEngine.Vector3);
					local(distSqr); distSqr = getexterninstance(SymbolKind.Property, ( invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(pos1, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(pos2, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local)) ), UnityEngine.Vector3, "sqrMagnitude");
					if( execbinary("<", distSqr, 0.01000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 22_16_27_17 ){
						callexternstatic(UnityEngine.Debug, "Log__Object", dslstrtocsstr("distSqr<0.01f, collide."));
						__method_ret_12_4_38_5 = otherObj;
						return(__method_ret_12_4_38_5);
					}else{
						callexternstatic(UnityEngine.Debug, "Log__Object", callexternstatic(System.String, "Format__String__Object", dslstrtocsstr("Dist:{0}"), distSqr));
					};
					local(dot); dot = callexternstatic(UnityEngine.Vector3, "Dot", invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(lastPos2, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(pos2, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local)), invokeexternoperatorreturnstruct(UnityEngine.Vector3, UnityEngine.Vector3, "op_Subtraction", wrapexternstructargument(pos1, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local), wrapexternstructargument(pos2, UnityEngine.Vector3, OperationKind.LocalReference, SymbolKind.Local)));
					if( execbinary("&&", execbinary(">", getexterninstance(SymbolKind.Property, lastPos2, UnityEngine.Vector3, "sqrMagnitude"), 0.00010000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), execbinary(">", dot, 0, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 29_16_34_17 ){
						callexternstatic(UnityEngine.Debug, "Log__Object", dslstrtocsstr("cross, collide."));
						__method_ret_12_4_38_5 = otherObj;
						return(__method_ret_12_4_38_5);
					}else{
						callexternstatic(UnityEngine.Debug, "Log__Object", callexternstatic(System.String, "Format__String__Object__Object", dslstrtocsstr("lastPos2Sqr:{0} dot:{1}"), getexterninstance(SymbolKind.Property, lastPos2, UnityEngine.Vector3, "sqrMagnitude"), dot));
					};
				};
			};
			__method_ret_12_4_38_5 = null;
			return(__method_ret_12_4_38_5);
		}options[needfuncinfo(true), rettype(return, UnityEngine.GameObject, TypeKind.Class, 0, true), paramtype(bullet, UnityEngine.GameObject, TypeKind.Class, 0, true)];
		UpdatePos = deffunc(0)args(this, bullet){
			local(info);
			if( execclosure(true, __invoke_42_12_42_67, true){ multiassign(precode{
				},postcode{
				})varlist(__invoke_42_12_42_67, info) = callexterninstance(getinstance(SymbolKind.Field, this, BulletManager, "m_Bullets"), System.Collections.Generic.Dictionary_TKey_TValue, "TryGetValue", callexterninstance(bullet, UnityEngine.Object, "GetInstanceID"), __cs2dsl_out); }, 42_8_44_9 ){
				callinstance(info, BulletManager.CollideInfo, "SetLastPos", getexterninstancestructmember(SymbolKind.Property, getexterninstance(SymbolKind.Property, bullet, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "position"));
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(bullet, UnityEngine.GameObject, TypeKind.Class, 0, true)];
		AddBullet = deffunc(0)args(this, bullet){
			callexterninstance(getinstance(SymbolKind.Field, this, BulletManager, "m_Bullets"), System.Collections.Generic.Dictionary_TKey_TValue, "Add", callexterninstance(bullet, UnityEngine.Object, "GetInstanceID"), newobject(BulletManager.CollideInfo, "g_BulletManager_CollideInfo", typeargs(), typekinds(), "ctor", 0, function(newobj){ setinstance(SymbolKind.Field, newobj, BulletManager.CollideInfo, "Obj", bullet); }));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(bullet, UnityEngine.GameObject, TypeKind.Class, 0, true)];
		RemoveBullet = deffunc(0)args(this, bullet){
			callexterninstance(getinstance(SymbolKind.Field, this, BulletManager, "m_Bullets"), System.Collections.Generic.Dictionary_TKey_TValue, "Remove", callexterninstance(bullet, UnityEngine.Object, "GetInstanceID"));
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(bullet, UnityEngine.GameObject, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, BulletManager, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, BulletManager, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, BulletManager, "__ctor_called", true);
			};
			setinstance(SymbolKind.Field, this, BulletManager, "m_Bullets", newexterndictionary(System.Collections.Generic.Dictionary_TKey_TValue, "g_System_Collections_Generic_Dictionary_System_Int32_BulletManager_CollideInfo", typeargs(System.Int32, BulletManager.CollideInfo), typekinds(TypeKind.Structure, TypeKind.Class), "ctor", 0, literaldictionary("g_System_Collections_Generic_Dictionary_System_Int32_BulletManager_CollideInfo", typeargs(System.Int32, BulletManager.CollideInfo), typekinds(TypeKind.Structure, TypeKind.Class))));
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_Bullets = null;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {};

	class_info(TypeKind.Class, Accessibility.Public) {
	};
	method_info {
		GetCollideObject(MethodKind.Ordinary, Accessibility.Public){
		};
		UpdatePos(MethodKind.Ordinary, Accessibility.Public){
		};
		AddBullet(MethodKind.Ordinary, Accessibility.Public){
		};
		RemoveBullet(MethodKind.Ordinary, Accessibility.Public){
		};
		get_Instance(MethodKind.PropertyGet, Accessibility.Public){
			static(true);
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
		cctor(MethodKind.StaticConstructor, Accessibility.Private){
			static(true);
		};
	};
};




class(BulletManager.CollideInfo) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(BulletManager.CollideInfo, "g_BulletManager_CollideInfo", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(BulletManager.CollideInfo, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(BulletManager.CollideInfo.__cctor_called){
				return();
			}else{
				BulletManager.CollideInfo.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		GetLastPos = deffunc(1)args(this){
			local(__method_ret_62_8_69_9);
			if( execbinary("==", getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastSetIndex"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 64_12_68_13 ){
				__method_ret_62_8_69_9 = getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2");
				return(__method_ret_62_8_69_9);
			}else{
				__method_ret_62_8_69_9 = getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1");
				return(__method_ret_62_8_69_9);
			};
			return(null);
		}options[needfuncinfo(false), rettype(return, UnityEngine.Vector3, TypeKind.Structure, 0, true)];
		SetLastPos = deffunc(0)args(this, pos){
			setinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastSetIndex", execbinary("%", ( execbinary("+", getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastSetIndex"), 1, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ), 2, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure));
			if( execbinary("==", getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastSetIndex"), 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 73_12_77_13 ){
				local(__old_val_74_16_74_30); __old_val_74_16_74_30 = getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1");
				setinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1", pos);
				getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1") = wrapexternstruct(getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1"), UnityEngine.Vector3);
				local(__new_val_74_16_74_30); __new_val_74_16_74_30 = getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1");
				recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_74_16_74_30, __new_val_74_16_74_30);
			}else{
				local(__old_val_76_16_76_30); __old_val_76_16_76_30 = getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2");
				setinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2", pos);
				getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2") = wrapexternstruct(getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2"), UnityEngine.Vector3);
				local(__new_val_76_16_76_30); __new_val_76_16_76_30 = getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2");
				recycleandkeepstructvalue(UnityEngine.Vector3, __old_val_76_16_76_30, __new_val_76_16_76_30);
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(pos, UnityEngine.Vector3, TypeKind.Structure, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, BulletManager.CollideInfo, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "__ctor_called", true);
			};
			setinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos1"));
			setinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2", getexternstaticstructmember(SymbolKind.Property, UnityEngine.Vector3, "zero"));
			recycleandkeepstructvalue(UnityEngine.Vector3, nil, getinstance(SymbolKind.Field, this, BulletManager.CollideInfo, "LastPos2"));
		}options[needfuncinfo(true)];
	};
	instance_fields {
		LastPos1 = null;
		LastPos2 = null;
		Obj = null;
		LastSetIndex = 0;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {};

	class_info(TypeKind.Class, Accessibility.Internal) {
	};
	method_info {
		GetLastPos(MethodKind.Ordinary, Accessibility.Internal){
		};
		SetLastPos(MethodKind.Ordinary, Accessibility.Internal){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



