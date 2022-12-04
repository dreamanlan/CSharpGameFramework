require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(AiQuery) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(AiQuery, "g_AiQuery", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(AiQuery, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(AiQuery.__cctor_called){
				return();
			}else{
				AiQuery.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		SetProxy = deffunc(0)args(this, result){
			setinstance(SymbolKind.Field, this, AiQuery, "m_Proxy", result);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(result, StorySystem.StoryValueResult, TypeKind.Class, 0, true)];
		Clone = deffunc(1)args(this){
			local(__method_ret_17_4_34_5);
			local(newObj); newObj = newobject(AiQuery, "g_AiQuery", typeargs(), typekinds(), "ctor", 0, null);
			if( execbinary("!=", null, getinstance(SymbolKind.Field, this, AiQuery, "m_Select"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), 20_8_22_9 ){
				setinstance(SymbolKind.Field, newObj, AiQuery, "m_Select", typeas(callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_Select"), StorySystem.IStoryValue, "Clone"), StorySystem.IStoryValue, TypeKind.Interface));
			};
			if( execbinary("!=", null, getinstance(SymbolKind.Field, this, AiQuery, "m_From"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), 23_8_25_9 ){
				setinstance(SymbolKind.Field, newObj, AiQuery, "m_From", typeas(callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_From"), StorySystem.IStoryValue, "Clone"), StorySystem.IStoryValue, TypeKind.Interface));
			};
			if( execbinary("!=", null, getinstance(SymbolKind.Field, this, AiQuery, "m_Where"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), 26_8_28_9 ){
				setinstance(SymbolKind.Field, newObj, AiQuery, "m_Where", typeas(callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_Where"), StorySystem.IStoryValue, "Clone"), StorySystem.IStoryValue, TypeKind.Interface));
			};
			local(i); i = 0;
			while( execbinary("<", i, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiQuery, "m_OrderBy"), System.Collections.Generic.List_T, "Count"), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
				callexterninstance(getinstance(SymbolKind.Field, newObj, AiQuery, "m_OrderBy"), System.Collections.Generic.List_T, "Add", typeas(callinterface(getexterninstanceindexer(StorySystem.IStoryValue, TypeKind.Interface, System.Collections.Generic.List_T, getinstance(SymbolKind.Field, this, AiQuery, "m_OrderBy"), System.Collections.Generic.List_T, "get_Item", 1, i), StorySystem.IStoryValue, "Clone"), StorySystem.IStoryValue, TypeKind.Interface));
			i = execbinary("+", i, 1, null, null, null, null);
			};
			setinstance(SymbolKind.Field, newObj, AiQuery, "m_Desc", getinstance(SymbolKind.Field, this, AiQuery, "m_Desc"));
			__method_ret_17_4_34_5 = newObj;
			return(__method_ret_17_4_34_5);
		}options[needfuncinfo(false), rettype(return, GameFramework.Plugin.IStoryValuePlugin, TypeKind.Interface, 0, true)];
		Evaluate = deffunc(0)args(this, instance, handler, iterator, args){
			if( execbinary("&&", execbinary("!=", null, getinstance(SymbolKind.Field, this, AiQuery, "m_Select"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), execbinary("!=", null, getinstance(SymbolKind.Field, this, AiQuery, "m_From"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), System.Boolean, System.Boolean, TypeKind.Structure, TypeKind.Structure), 38_8_75_9 ){
				callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_From"), StorySystem.IStoryValue, "Evaluate", instance, handler, wrapexternstructargument(iterator, BoxedValue, OperationKind.ParameterReference, SymbolKind.Parameter), args);
				local(coll); coll = newexternlist(System.Collections.ArrayList, "g_System_Collections_ArrayList", typeargs(), typekinds(), "ctor", 0, literallist("g_System_Collections_ArrayList", typeargs(), typekinds()));
				comment("筛选");
				local(enumer); enumer = typeas(getexterninstance(SymbolKind.Field, getexterninterfacestructmember(getinstance(SymbolKind.Field, this, AiQuery, "m_From"), StorySystem.IStoryValue, "Value", "get_Value"), BoxedValue, "ObjectVal"), System.Collections.IEnumerable, TypeKind.Interface);
				if( execbinary("!=", null, enumer, System.Object, System.Object, TypeKind.Class, TypeKind.Class), 44_12_60_13 ){
					local(enumerator); enumerator = callinterface(enumer, System.Collections.IEnumerable, "GetEnumerator");
					while( callinterface(enumerator, System.Collections.IEnumerator, "MoveNext") ){
						local(v); v = getinterface(enumerator, System.Collections.IEnumerator, "Current", "get_Current");
						local(bv); bv = callexternstaticreturnstruct(BoxedValue, "FromObject", v);
						if( execbinary("!=", null, getinstance(SymbolKind.Field, this, AiQuery, "m_Where"), System.Object, System.Object, TypeKind.Class, TypeKind.Class), 49_20_58_21 ){
							callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_Where"), StorySystem.IStoryValue, "Evaluate", instance, handler, wrapexternstructargument(bv, BoxedValue, OperationKind.LocalReference, SymbolKind.Local), args);
							local(wvObj); wvObj = getexterninterfacestructmember(getinstance(SymbolKind.Field, this, AiQuery, "m_Where"), StorySystem.IStoryValue, "Value", "get_Value");
							local(wv); wv = typecast(callexternstatic(System.Convert, "ChangeType__Object__Type", wvObj, typeof(System.Int32)), System.Int32, TypeKind.Structure);
							if( execbinary("!=", wv, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 53_24_55_25 ){
								callinstance(this, AiQuery, "AddRow", coll, wrapexternstructargument(bv, BoxedValue, OperationKind.LocalReference, SymbolKind.Local), instance, handler, args);
							};
						}else{
							callinstance(this, AiQuery, "AddRow", coll, wrapexternstructargument(bv, BoxedValue, OperationKind.LocalReference, SymbolKind.Local), instance, handler, args);
						};
					};
				};
				comment("排序");
				local(ct); ct = getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiQuery, "m_OrderBy"), System.Collections.Generic.List_T, "Count");
				if( execbinary(">", ct, 0, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure), 64_12_66_13 ){
					callexterninstance(coll, System.Collections.ArrayList, "Sort__IComparer", newexternobject(AiQueryComparer, "g_AiQueryComparer", typeargs(), typekinds(), "ctor", 0, null, getinstance(SymbolKind.Field, this, AiQuery, "m_Desc"), ct));
				};
				comment("收集结果");
				local(result); result = newexternlist(System.Collections.ArrayList, "g_System_Collections_ArrayList", typeargs(), typekinds(), "ctor", 0, literallist("g_System_Collections_ArrayList", typeargs(), typekinds()));
				local(i); i = 0;
				while( execbinary("<", i, getexterninstance(SymbolKind.Property, coll, System.Collections.ArrayList, "Count"), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
					local(ao); ao = typeas(getexterninstanceindexer(System.Object, TypeKind.Class, System.Collections.ArrayList, coll, System.Collections.ArrayList, "get_Item", 1, i), System.Collections.ArrayList, TypeKind.Class);
					callexterninstance(result, System.Collections.ArrayList, "Add", getexterninstanceindexer(System.Object, TypeKind.Class, System.Collections.ArrayList, ao, System.Collections.ArrayList, "get_Item", 1, 0));
				i = execbinary("+", i, 1, null, null, null, null);
				};
				setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiQuery, "m_Proxy"), StorySystem.StoryValueResult, "Value", invokeexternoperatorreturnstruct(BoxedValue, BoxedValue, "op_Implicit__BoxedValue__ArrayList", result));
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(iterator, BoxedValue, TypeKind.Structure, 0, true), paramtype(args, BoxedValueList, TypeKind.Class, 0, true)];
		LoadCallData = deffunc(0)args(this, callData){
			local(id); id = callexterninstance(callData, Dsl.FunctionData, "GetId");
			if( execbinary("==", id, "select", System.String, System.String, TypeKind.Class, TypeKind.Class), 81_8_100_9 ){
				setinstance(SymbolKind.Field, this, AiQuery, "m_Select", newexternobject(StorySystem.StoryValue, "g_StorySystem_StoryValue", typeargs(), typekinds(), "ctor", 0, null));
				callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_Select"), StorySystem.IStoryValue, "InitFromDsl", callexterninstance(callData, Dsl.FunctionData, "GetParam", 0));
			}elseif( execbinary("==", id, "from", System.String, System.String, TypeKind.Class, TypeKind.Class), 84_10_100_9 ){
				setinstance(SymbolKind.Field, this, AiQuery, "m_From", newexternobject(StorySystem.StoryValue, "g_StorySystem_StoryValue", typeargs(), typekinds(), "ctor", 0, null));
				callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_From"), StorySystem.IStoryValue, "InitFromDsl", callexterninstance(callData, Dsl.FunctionData, "GetParam", 0));
			}elseif( execbinary("==", id, "where", System.String, System.String, TypeKind.Class, TypeKind.Class), 87_10_100_9 ){
				setinstance(SymbolKind.Field, this, AiQuery, "m_Where", newexternobject(StorySystem.StoryValue, "g_StorySystem_StoryValue", typeargs(), typekinds(), "ctor", 0, null));
				callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_Where"), StorySystem.IStoryValue, "InitFromDsl", callexterninstance(callData, Dsl.FunctionData, "GetParam", 0));
			}elseif( execbinary("==", id, "orderby", System.String, System.String, TypeKind.Class, TypeKind.Class), 90_10_100_9 ){
				local(i); i = 0;
				while( execbinary("<", i, callexterninstance(callData, Dsl.FunctionData, "GetParamNum"), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
					local(v); v = newexternobject(StorySystem.StoryValue, "g_StorySystem_StoryValue", typeargs(), typekinds(), "ctor", 0, null);
					callexterninstance(v, StorySystem.StoryValue, "InitFromDsl", callexterninstance(callData, Dsl.FunctionData, "GetParam", i));
					callexterninstance(getinstance(SymbolKind.Field, this, AiQuery, "m_OrderBy"), System.Collections.Generic.List_T, "Add", v);
				i = execbinary("+", i, 1, null, null, null, null);
				};
			}elseif( execbinary("==", id, "asc", System.String, System.String, TypeKind.Class, TypeKind.Class), 96_10_100_9 ){
				setinstance(SymbolKind.Field, this, AiQuery, "m_Desc", false);
			}elseif( execbinary("==", id, "desc", System.String, System.String, TypeKind.Class, TypeKind.Class), 98_10_100_9 ){
				setinstance(SymbolKind.Field, this, AiQuery, "m_Desc", true);
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(callData, Dsl.FunctionData, TypeKind.Class, 0, true)];
		LoadFuncData = deffunc(0)args(this, funcData){
			callinstance(this, AiQuery, "LoadCallData", funcData);
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(funcData, Dsl.FunctionData, TypeKind.Class, 0, true)];
		LoadStatementData = deffunc(0)args(this, statementData){
			local(i); i = 0;
			while( execbinary("<", i, getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, statementData, Dsl.StatementData, "Functions"), System.Collections.Generic.List_T, "Count"), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
				local(funcData); funcData = getexterninstance(SymbolKind.Property, getexterninstanceindexer(Dsl.ValueOrFunctionData, TypeKind.Class, System.Collections.Generic.List_T, getexterninstance(SymbolKind.Property, statementData, Dsl.StatementData, "Functions"), System.Collections.Generic.List_T, "get_Item", 1, i), Dsl.ValueOrFunctionData, "AsFunction");
				callinstance(this, AiQuery, "LoadFuncData", funcData);
			i = execbinary("+", i, 1, null, null, null, null);
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(statementData, Dsl.StatementData, TypeKind.Class, 0, true)];
		AddRow = deffunc(0)args(this, coll, v, instance, handler, args){
			local(row); row = newexternlist(System.Collections.ArrayList, "g_System_Collections_ArrayList", typeargs(), typekinds(), "ctor", 0, literallist("g_System_Collections_ArrayList", typeargs(), typekinds()));
			callexterninstance(coll, System.Collections.ArrayList, "Add", row);
			callinterface(getinstance(SymbolKind.Field, this, AiQuery, "m_Select"), StorySystem.IStoryValue, "Evaluate", instance, handler, wrapexternstructargument(v, BoxedValue, OperationKind.ParameterReference, SymbolKind.Parameter), args);
			callexterninstance(row, System.Collections.ArrayList, "Add", callexterninstance(getexterninterfacestructmember(getinstance(SymbolKind.Field, this, AiQuery, "m_Select"), StorySystem.IStoryValue, "Value", "get_Value"), BoxedValue, "GetObject"));
			local(i); i = 0;
			while( execbinary("<", i, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, AiQuery, "m_OrderBy"), System.Collections.Generic.List_T, "Count"), System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
				local(val); val = getexterninstanceindexer(StorySystem.IStoryValue, TypeKind.Interface, System.Collections.Generic.List_T, getinstance(SymbolKind.Field, this, AiQuery, "m_OrderBy"), System.Collections.Generic.List_T, "get_Item", 1, i);
				callinterface(val, StorySystem.IStoryValue, "Evaluate", instance, handler, wrapexternstructargument(v, BoxedValue, OperationKind.ParameterReference, SymbolKind.Parameter), args);
				callexterninstance(row, System.Collections.ArrayList, "Add", callexterninstance(getexterninterfacestructmember(val, StorySystem.IStoryValue, "Value", "get_Value"), BoxedValue, "GetObject"));
			i = execbinary("+", i, 1, null, null, null, null);
			};
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(coll, System.Collections.ArrayList, TypeKind.Class, 0, true), paramtype(v, BoxedValue, TypeKind.Structure, 0, true), paramtype(instance, StorySystem.StoryInstance, TypeKind.Class, 0, true), paramtype(handler, StorySystem.StoryMessageHandler, TypeKind.Class, 0, true), paramtype(args, BoxedValueList, TypeKind.Class, 0, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, AiQuery, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, AiQuery, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, AiQuery, "__ctor_called", true);
			};
			setinstance(SymbolKind.Field, this, AiQuery, "m_OrderBy", newexternlist(System.Collections.Generic.List_T, "g_System_Collections_Generic_List_StorySystem_IStoryValue", typeargs(StorySystem.IStoryValue), typekinds(TypeKind.Interface), "ctor", 0, literallist("g_System_Collections_Generic_List_StorySystem_IStoryValue", typeargs(StorySystem.IStoryValue), typekinds(TypeKind.Interface))));
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_Proxy = null;
		m_Select = null;
		m_From = null;
		m_Where = null;
		m_OrderBy = null;
		m_Desc = false;
		__ctor_called = false;
	};
	instance_props {};
	instance_events {};

	interfaces {
		"GameFramework.Plugin.IStoryValuePlugin";
	};

	class_info(TypeKind.Class, Accessibility.Public) {
	};
	method_info {
		SetProxy(MethodKind.Ordinary, Accessibility.Public){
		};
		Clone(MethodKind.Ordinary, Accessibility.Public){
		};
		Evaluate(MethodKind.Ordinary, Accessibility.Public){
		};
		LoadCallData(MethodKind.Ordinary, Accessibility.Public){
		};
		LoadFuncData(MethodKind.Ordinary, Accessibility.Public){
		};
		LoadStatementData(MethodKind.Ordinary, Accessibility.Public){
		};
		AddRow(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



