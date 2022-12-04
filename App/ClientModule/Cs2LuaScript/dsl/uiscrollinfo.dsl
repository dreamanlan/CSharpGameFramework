require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

class(UiScrollInfo) {
	static_methods {
		__new_object = deffunc(1)args(...){
			local(__cs2dsl_newobj);__cs2dsl_newobj = newobject(UiScrollInfo, "g_UiScrollInfo", typeargs(), typekinds(), "ctor", 0, null, ...);
			return(__cs2dsl_newobj);
		}options[needfuncinfo(false)];
		cctor = deffunc(0)args(){
			callstatic(UiScrollInfo, "__cctor");
		};
		__cctor = deffunc(0)args(){
			if(UiScrollInfo.__cctor_called){
				return();
			}else{
				UiScrollInfo.__cctor_called = true;
			};
		}options[needfuncinfo(false)];
	};
	static_fields {
		c_MinTextHeight = 24.00000000;
		__cctor_called = false;
	};
	static_props {};
	static_events {};

	instance_methods {
		Init = deffunc(0)args(this, obj, behaviour){
			setinstance(SymbolKind.Field, this, UiScrollInfo, "m_GameObject", obj);
			getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Item") = callexternstatic(UnityEngine.Resources, "Load__String", dslstrtocsstr("UI/Image"));
			setinstance(SymbolKind.Field, this, UiScrollInfo, "m_RectTransform", typeas(getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_GameObject"), UnityEngine.GameObject, "transform"), UnityEngine.RectTransform, TypeKind.Class));
			local(viewObj); viewObj = getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_RectTransform"), UnityEngine.Transform, "parent"), UnityEngine.Transform, "parent"), UnityEngine.Component, "gameObject");
			getinstance(SymbolKind.Field, this, UiScrollInfo, "m_ScrollRect") = callexterninstance(viewObj, UnityEngine.GameObject, "GetComponent__Type", UnityEngine.UI.ScrollRect);
			local(rectTrans); rectTrans = typeas(getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_RectTransform"), UnityEngine.Transform, "parent"), UnityEngine.Transform, "parent"), UnityEngine.RectTransform, TypeKind.Class);
			setinstance(SymbolKind.Field, this, UiScrollInfo, "m_Top", 0);
			setinstance(SymbolKind.Field, this, UiScrollInfo, "m_Width", getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, rectTrans, UnityEngine.RectTransform, "rect"), UnityEngine.Rect, "width"));
			setinstance(SymbolKind.Field, this, UiScrollInfo, "m_Height", getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_RectTransform"), UnityEngine.RectTransform, "rect"), UnityEngine.Rect, "height"));
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(obj, UnityEngine.GameObject, TypeKind.Class, 0, true), paramtype(behaviour, MonoBehaviourProxy, TypeKind.Class, 0, true)];
		Update = deffunc(0)args(this){
			local(time); time = getexternstatic(SymbolKind.Property, UnityEngine.Time, "time");
			if( execbinary(">=", execbinary("+", getinstance(SymbolKind.Field, this, UiScrollInfo, "m_ScrollToEndTime"), 1, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), time, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 23_8_25_9 ){
				setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_ScrollRect"), UnityEngine.UI.ScrollRect, "verticalScrollbar"), UnityEngine.UI.Scrollbar, "value", 0);
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false)];
		Call = deffunc(0)args(this, name, ...){
			local(args); args = params(System.Object, TypeKind.Class);
			if( execbinary("==", name, "PushInfo", System.String, System.String, TypeKind.Class, TypeKind.Class), 29_8_34_9 ){
				local(info); info = typeas(args[1], System.String, TypeKind.Class);
				if( execbinary("!=", null, info, System.String, System.String, TypeKind.Class, TypeKind.Class), 31_12_33_13 ){
					callinstance(this, UiScrollInfo, "PushInfo", info);
				};
			};
		}options[needfuncinfo(false), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(name, System.String, TypeKind.Class, 0, true), paramtype(..., System.Object, TypeKind.Array, 0, true)];
		PushInfo = deffunc(0)args(this, text){
			if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Item")), 38_8_64_9 ){
				local(item); item = typeas(callexternstatic(UnityEngine.Object, "Instantiate__Object", getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Item")), UnityEngine.GameObject, TypeKind.Class);
				local(t); t = callexterninstance(getexterninstance(SymbolKind.Property, item, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "Find", dslstrtocsstr("Text"));
				if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, t), 41_12_63_13 ){
					callexterninstance(getexterninstance(SymbolKind.Property, item, UnityEngine.GameObject, "transform"), UnityEngine.Transform, "SetParent__Transform__Boolean", getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_GameObject"), UnityEngine.GameObject, "transform"), false);
					local(bkg); bkg = callexterninstance(item, UnityEngine.GameObject, "GetComponent__Type", UnityEngine.UI.Image);
					local(rectTrans); rectTrans = typeas(getexterninstance(SymbolKind.Property, bkg, UnityEngine.Component, "transform"), UnityEngine.RectTransform, TypeKind.Class);
					local(content); content = callexterninstance(getexterninstance(SymbolKind.Property, t, UnityEngine.Component, "gameObject"), UnityEngine.GameObject, "GetComponent__Type", UnityEngine.UI.Text);
					local(rectTrans2); rectTrans2 = typeas(getexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, t, UnityEngine.Component, "gameObject"), UnityEngine.GameObject, "transform"), UnityEngine.RectTransform, TypeKind.Class);
					local(dw); dw = 16;
					local(dh); dh = 6;
					if( invokeexternoperator(System.Boolean, UnityEngine.Object, "op_Inequality", null, content), 50_16_62_17 ){
						local(w);
						local(h);
						local(__invoke_52_20_52_131); multiassign(precode{
							},postcode{
							})varlist(__invoke_52_20_52_131, w, h) = callinstance(this, UiScrollInfo, "CalcBounds", text, content, newexternstruct(UnityEngine.Vector2, "g_UnityEngine_Vector2", typeargs(), typekinds(), "ctor__Single__Single", 0, null, getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, rectTrans, UnityEngine.RectTransform, "rect"), UnityEngine.Rect, "width"), getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, rectTrans, UnityEngine.RectTransform, "rect"), UnityEngine.Rect, "height")), execbinary("-", getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Width"), dw, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), __cs2dsl_out, __cs2dsl_out);
						if( execbinary("<", h, 24.00000000, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 53_20_54_44 ){
							h = 24.00000000;
						};
						setexterninstance(SymbolKind.Property, content, UnityEngine.UI.Text, "text", text);
						setexterninstance(SymbolKind.Property, rectTrans, UnityEngine.Transform, "localPosition", newexternstruct(UnityEngine.Vector3, "g_UnityEngine_Vector3", typeargs(), typekinds(), "ctor__Single__Single__Single", 0, null, 0, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Top"), 0));
						setexterninstance(SymbolKind.Property, rectTrans, UnityEngine.RectTransform, "sizeDelta", newexternstruct(UnityEngine.Vector2, "g_UnityEngine_Vector2", typeargs(), typekinds(), "ctor__Single__Single", 0, null, execbinary("-", getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Width"), dw, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), execbinary("+", h, dh, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure)));
						setinstance(SymbolKind.Field, this, UiScrollInfo, "m_Top", execbinary("-", getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Top"), execbinary("+", h, dh, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure));
						setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_RectTransform"), UnityEngine.Transform, "localPosition", newexternstruct(UnityEngine.Vector3, "g_UnityEngine_Vector3", typeargs(), typekinds(), "ctor__Single__Single__Single", 0, null, 0, 0, 0));
						setexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_RectTransform"), UnityEngine.RectTransform, "sizeDelta", newexternstruct(UnityEngine.Vector2, "g_UnityEngine_Vector2", typeargs(), typekinds(), "ctor__Single__Single", 0, null, getexterninstance(SymbolKind.Field, getexterninstancestructmember(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_RectTransform"), UnityEngine.RectTransform, "sizeDelta"), UnityEngine.Vector2, "x"), execbinary("-", dh, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_Top"), System.Single, System.Single, TypeKind.Structure, TypeKind.Structure)));
					};
				};
			};
			setexterninstance(SymbolKind.Property, getexterninstance(SymbolKind.Property, getinstance(SymbolKind.Field, this, UiScrollInfo, "m_ScrollRect"), UnityEngine.UI.ScrollRect, "verticalScrollbar"), UnityEngine.UI.Scrollbar, "value", 0);
			setinstance(SymbolKind.Field, this, UiScrollInfo, "m_ScrollToEndTime", getexternstatic(SymbolKind.Property, UnityEngine.Time, "time"));
		}options[needfuncinfo(true), rettype(return, System.Void, TypeKind.Unknown, 0, false), paramtype(text, System.String, TypeKind.Class, 0, true)];
		CalcBounds = deffunc(3)args(this, text, content, size, maxWidth, width, height){
			local(__method_ret_69_4_101_5);
			local(ret); ret = false;
			setexterninstance(SymbolKind.Field, size, UnityEngine.Vector2, "x", maxWidth);
			width = 0;
			height = 0;
			local(lines); lines = callbasicvalue(text, false, System.String, "Split__A_Char", wrapchar('\n', 0x0A));
			local(lineCt); lineCt = getbasicvalue(lines, false, System.Array, "Length");
			local(i); i = 0;
			while( execbinary("<", i, lineCt, System.Int32, System.Int32, TypeKind.Structure, TypeKind.Structure) ){
				local(txt); txt = callbasicvalue(lines[i + 1], false, System.String, "Trim__A_Char", wrapchar('\r', 0x0D));
				local(textSetting); textSetting = callexterninstancereturnstruct(content, UnityEngine.UI.Text, "GetGenerationSettings", wrapexternstructargument(size, UnityEngine.Vector2, OperationKind.ParameterReference, SymbolKind.Parameter));
				setexterninstance(SymbolKind.Field, textSetting, UnityEngine.TextGenerationSettings, "verticalOverflow", 1);
				setexterninstance(SymbolKind.Field, textSetting, UnityEngine.TextGenerationSettings, "horizontalOverflow", 0);
				setexterninstance(SymbolKind.Field, textSetting, UnityEngine.TextGenerationSettings, "updateBounds", true);
				local(generator); generator = newexternobject(UnityEngine.TextGenerator, "g_UnityEngine_TextGenerator", typeargs(), typekinds(), "ctor", 0, null);
				local(pw); pw = callexterninstance(generator, UnityEngine.TextGenerator, "GetPreferredWidth", txt, wrapexternstructargument(textSetting, UnityEngine.TextGenerationSettings, OperationKind.LocalReference, SymbolKind.Local));
				if( callexterninstance(generator, UnityEngine.TextGenerator, "Populate", txt, wrapexternstructargument(textSetting, UnityEngine.TextGenerationSettings, OperationKind.LocalReference, SymbolKind.Local)), 85_12_98_13 ){
					local(w); w = getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, generator, UnityEngine.TextGenerator, "rectExtents"), UnityEngine.Rect, "width");
					local(h); h = getexterninstance(SymbolKind.Property, getexterninstancestructmember(SymbolKind.Property, generator, UnityEngine.TextGenerator, "rectExtents"), UnityEngine.Rect, "height");
					if( execbinary(">", w, pw, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 89_16_91_17 ){
						w = pw;
					};
					if( execbinary("<", width, w, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure), 93_16_94_30 ){
						width = w;
					};
					height = execbinary("+", height, h, System.Single, System.Single, TypeKind.Structure, TypeKind.Structure);
					ret = true;
				};
			i = execbinary("+", i, 1, null, null, null, null);
			};
			__method_ret_69_4_101_5 = ret;
			return(__method_ret_69_4_101_5, width, height);
		}options[needfuncinfo(true), rettype(return, System.Boolean, TypeKind.Structure, 0, true), rettype(width, System.Single, TypeKind.Structure, 2, true), rettype(height, System.Single, TypeKind.Structure, 2, true), paramtype(text, System.String, TypeKind.Class, 0, true), paramtype(content, UnityEngine.UI.Text, TypeKind.Class, 0, true), paramtype(size, UnityEngine.Vector2, TypeKind.Structure, 0, true), paramtype(maxWidth, System.Single, TypeKind.Structure, 0, true), paramtype(width, System.Single, TypeKind.Structure, 2, true), paramtype(height, System.Single, TypeKind.Structure, 2, true)];
		ctor = deffunc(0)args(this){
			callinstance(this, UiScrollInfo, "__ctor");
		};
		__ctor = deffunc(0)args(this){
			if(getinstance(SymbolKind.Field, this, UiScrollInfo, "__ctor_called")){
				return();
			}else{
				setinstance(SymbolKind.Field, this, UiScrollInfo, "__ctor_called", true);
			};
		}options[needfuncinfo(false)];
	};
	instance_fields {
		m_Item = null;
		m_GameObject = null;
		m_RectTransform = null;
		m_ScrollRect = null;
		m_Top = 0;
		m_Width = 0;
		m_Height = 2048;
		m_ScrollToEndTime = 0;
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
		PushInfo(MethodKind.Ordinary, Accessibility.Private){
		};
		CalcBounds(MethodKind.Ordinary, Accessibility.Private){
		};
		ctor(MethodKind.Constructor, Accessibility.Public){
		};
	};
};



