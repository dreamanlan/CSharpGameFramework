using System.Collections.Generic;
using DotnetStoryScript.DslExpression;

namespace DotnetStoryScript
{
    public sealed class DslCalculatorCodeStructure
    {
        public Dsl.DslLogDelegation OnLog;
        public DslCalculator.LoadFailbackDelegation OnLoadFailback;
        public DslCalculatorApiRegistry ApiRegistry
        {
            get { return m_ApiRegistry; }
            set { m_ApiRegistry = value; }
        }
        public void NewApiRegistry()
        {
            m_ApiRegistry = new DslCalculatorApiRegistry();
            m_ApiRegistry.Init();
        }
        public void Register(string name, string doc, IExpressionFactory factory)
        {
            if (null != m_ApiRegistry) {
                m_ApiRegistry.Register(name, doc, factory);
            }
        }
        public SortedList<string, string> ApiDocs
        {
            get { return null != m_ApiRegistry ? m_ApiRegistry.ApiDocs : null; }
        }
        public void Clear()
        {
            m_Funcs.Clear();
            m_FuncCalls.Clear();
            m_NamedGlobalVariableIndexes.Clear();
        }
        public void LoadDsl(string dslFile)
        {
            Dsl.DslFile file = new Dsl.DslFile();
            ScriptableDslHelper.ForDslCalculator.SetCallbacks(file);
            string path = dslFile;
            if (file.Load(path, LogError)) {
                foreach (Dsl.ISyntaxComponent info in file.DslInfos) {
                    LoadDsl(info);
                }
            }
        }
        public void LoadDsl(Dsl.ISyntaxComponent info)
        {
            string fid = info.GetId();
            if (fid != "script" && fid != "func")
                return;
            var func = info as Dsl.FunctionData;
            string id;
            DslCalculator.FuncInfo funcInfo = null;
            bool isAsync = false;
            if (null != func) {
                if (func.IsHighOrder) {
                    id = func.LowerOrderFunction.GetParamId(0);
                    if (func.LowerOrderFunction.GetParamNum() > 1 && func.LowerOrderFunction.GetParamId(1) == "async")
                        isAsync = true;
                }
                else
                    return;
            }
            else {
                var statement = info as Dsl.StatementData;
                if (null != statement && statement.GetFunctionNum() == 2) {
                    id = statement.First.AsFunction.GetParamId(0);
                    if (statement.First.AsFunction.GetParamNum() > 1 && statement.First.AsFunction.GetParamId(1) == "async")
                        isAsync = true;
                    func = statement.Second.AsFunction;
                    if ((func.GetId() == "params" || func.GetId() == "args") && func.IsHighOrder) {
                        if (func.LowerOrderFunction.GetParamNum() > 0) {
                            funcInfo = new DslCalculator.FuncInfo();
                            foreach (var p in func.LowerOrderFunction.Params) {
                                string argName = p.GetId();
                                funcInfo.AddParamNameIndex(argName);
                            }
                        }
                    }
                    else {
                        return;
                    }
                }
                else {
                    return;
                }
            }
            if (null == funcInfo)
                funcInfo = new DslCalculator.FuncInfo();
            foreach (Dsl.ISyntaxComponent comp in func.Params) {
                var exp = Load(comp);
                if (null != exp) {
                    funcInfo.Codes.Add(exp);
                }
            }
            funcInfo.IsAsync = isAsync;
            m_Funcs[id] = funcInfo;
        }
        public void LoadDsl(string func, Dsl.FunctionData dslFunc)
        {
            LoadDsl(func, null, dslFunc);
        }
        public void LoadDsl(string func, IList<string> paramNames, Dsl.FunctionData dslFunc)
        {
            LoadDsl(func, paramNames, dslFunc, false);
        }
        public void LoadDsl(string func, IList<string> paramNames, Dsl.FunctionData dslFunc, bool isAsync)
        {
            DslCalculator.FuncInfo funcInfo = null;
            if (null != paramNames && paramNames.Count > 0) {
                funcInfo = new DslCalculator.FuncInfo();
                funcInfo.BuildParamNameIndexes(paramNames);
            }
            if (null == funcInfo)
                funcInfo = new DslCalculator.FuncInfo();
            LoadDsl(dslFunc.Params, funcInfo.Codes);
            funcInfo.IsAsync = isAsync;
            m_Funcs[func] = funcInfo;
        }
        public void LoadDsl(IList<Dsl.ISyntaxComponent> statements, List<IExpression> exps)
        {
            foreach (Dsl.ISyntaxComponent comp in statements) {
                if (comp.IsValid()) {
                    var exp = Load(comp);
                    if (null != exp) {
                        exps.Add(exp);
                    }
                }
            }
        }
        public bool TryGetFuncInfo(string func, out DslCalculator.FuncInfo funcInfo)
        {
            return m_Funcs.TryGetValue(func, out funcInfo);
        }
        public void CheckFuncXrefs()
        {
            foreach (var func in m_FuncCalls) {
                if (!m_Funcs.ContainsKey(func.FuncName)) {
                    //error
                    Log("DslCalculator error, unknown func '{0}', {1} line {2}", func.FuncName, func.SyntaxComponent.ToScriptString(false, Dsl.DelimiterInfo.Default), func.SyntaxComponent.GetLine());
                }
            }
        }
        public IExpression Load(Dsl.ISyntaxComponent comp)
        {
            Dsl.ValueData valueData = comp as Dsl.ValueData;
            Dsl.FunctionData funcData = null;
            if (null != valueData) {
                int idType = valueData.GetIdType();
                if (idType == Dsl.ValueData.ID_TOKEN) {
                    string id = valueData.GetId();
                    var p = Create(id);
                    if (null != p) {
                        //Convert a parameterless name into a parameterless function call.
                        Dsl.FunctionData fd = new Dsl.FunctionData();
                        fd.Name.CopyFrom(valueData);
                        fd.SetParenthesesParamClass();
                        if (!p.Load(fd, this)) {
                            //error
                            Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                        }
                        return p;
                    }
                    else if (id == "true" || id == "false") {
                        ConstGet constExp = new ConstGet();
                        constExp.Load(comp, this);
                        return constExp;
                    }
                    else if (id.Length > 0 && id[0] == '$') {
                        LocalVarGet varExp = new LocalVarGet();
                        varExp.Load(comp, this);
                        return varExp;
                    }
                    else {
                        GlobalVarGet varExp = new GlobalVarGet();
                        varExp.Load(comp, this);
                        return varExp;
                    }
                }
                else {
                    ConstGet constExp = new ConstGet();
                    constExp.Load(comp, this);
                    return constExp;
                }
            }
            else {
                funcData = comp as Dsl.FunctionData;
                if (null != funcData) {
                    if (funcData.HaveParam()) {
                        var callData = funcData;
                        if (!callData.HaveId() && !callData.IsHighOrder && (callData.GetParamClass() == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES || callData.GetParamClass() == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                            switch (callData.GetParamClass()) {
                                case (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES: {
                                        int num = callData.GetParamNum();
                                        if (num == 1) {
                                            Dsl.ISyntaxComponent param = callData.GetParam(0);
                                            return Load(param);
                                        }
                                        else {
                                            TupleExp exp = new TupleExp();
                                            exp.Load(comp, this);
                                            return exp;
                                        }
                                    }
                                case (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET: {
                                        ArrayExp exp = new ArrayExp();
                                        exp.Load(comp, this);
                                        return exp;
                                    }
                                default:
                                    return null;
                            }
                        }
                        else if (!callData.HaveParam()) {
                            //degeneration
                            valueData = callData.Name;
                            return Load(valueData);
                        }
                        else {
                            int paramClass = callData.GetParamClass();
                            string op = callData.GetId();
                            if (op == "`") {//backtick
                                int paramNum = callData.GetParamNum();
                                if (paramNum == 2) {
                                    Dsl.ISyntaxComponent param0 = callData.GetParam(0);
                                    Dsl.ISyntaxComponent param1 = callData.GetParam(1);
                                    if (param0 is Dsl.ValueData vd) {
                                        if (vd.GetId() == "return") {
                                            Dsl.FunctionData newCall = new Dsl.FunctionData();
                                            newCall.Name = new Dsl.ValueData("return", Dsl.ValueData.ID_TOKEN);
                                            newCall.SetParenthesesParamClass();
                                            newCall.Params.Add(param1);
                                            return Load(newCall);
                                        }
                                        else {
                                            //error
                                            Log("DslCalculator error, compound statements within an expression cannot be abbreviated to the brace-less form, return statements must be enclosed in parentheses, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                                            return null;
                                        }
                                    }
                                    else if (param0 is Dsl.FunctionData fd) {
                                        if (fd.IsOperatorParamClass() || fd.IsTernaryOperatorParamClass()) {
                                            //error
                                            Log("DslCalculator error, compound statements within an expression cannot be abbreviated to the brace-less form, return statements must be enclosed in parentheses, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                                            return null;
                                        }
                                        else {
                                            Dsl.FunctionData newCall = new Dsl.FunctionData();
                                            newCall.LowerOrderFunction = fd;
                                            newCall.SetStatementParamClass();
                                            newCall.Params.Add(param1);
                                            return Load(newCall);
                                        }
                                    }
                                    else {
                                        //error
                                        Log("DslCalculator error, compound statements within an expression cannot be abbreviated to the brace-less form, return statements must be enclosed in parentheses, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                                        return null;
                                    }
                                }
                            }
                            else if (op == "=") {//assignment
                                Dsl.FunctionData innerCall = callData.GetParam(0) as Dsl.FunctionData;
                                if (null != innerCall) {
                                    int innerParamClass = innerCall.GetParamClass();
                                    if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                      innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET) {
                                        //obj.property = val -> dotnetset(obj, property, val)
                                        //obj[property] = val -> collectionset(obj, property, val)
                                        Dsl.FunctionData newCall = new Dsl.FunctionData();
                                        if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD)
                                            newCall.Name = new Dsl.ValueData("dotnetset", Dsl.ValueData.ID_TOKEN);
                                        else
                                            newCall.Name = new Dsl.ValueData("collectionset", Dsl.ValueData.ID_TOKEN);
                                        newCall.SetParenthesesParamClass();
                                        if (innerCall.IsHighOrder) {
                                            newCall.Params.Add(innerCall.LowerOrderFunction);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            newCall.Params.Add(callData.GetParam(1));
                                        }
                                        else {
                                            newCall.Params.Add(innerCall.Name);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            newCall.Params.Add(callData.GetParam(1));
                                        }

                                        return Load(newCall);
                                    }
                                    else if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES && !innerCall.HaveId()) {
                                        //(a,b,c) = val;
                                        TupleSetExp tuple = new TupleSetExp();
                                        tuple.Load(comp, this);
                                        return tuple;
                                    }
                                }
                                IExpression exp = null;
                                string name = callData.GetParamId(0);
                                if (name.Length > 0 && name[0] == '$') {
                                    exp = new LocalVarSet();
                                }
                                else {
                                    exp = new GlobalVarSet();
                                }
                                if (null != exp) {
                                    exp.Load(comp, this);
                                }
                                else {
                                    //error
                                    Log("DslCalculator error, {0} line {1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
                                }
                                return exp;
                            }
                            else {
                                if (callData.IsHighOrder) {
                                    Dsl.FunctionData innerCall = callData.LowerOrderFunction;
                                    int innerParamClass = innerCall.GetParamClass();
                                    if (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES && (
                                        innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                        innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                                        //obj.member(a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> dotnetcall(obj,member,a,b,...)
                                        //obj[member](a,b,...) -> collectioncall(obj,member,a,b,...)
                                        string apiName;
                                        string member = innerCall.GetParamId(0);
                                        if (m_ApiRegistry.LinqOperatorRegistry.IsLinqMethod(member)) {
                                            apiName = "linq";
                                        }
                                        else if (innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD) {
                                            apiName = "dotnetcall";
                                        }
                                        else {
                                            apiName = "collectioncall";
                                        }
                                        Dsl.FunctionData newCall = new Dsl.FunctionData();
                                        newCall.Name = new Dsl.ValueData(apiName, Dsl.ValueData.ID_TOKEN);
                                        newCall.SetParenthesesParamClass();
                                        if (innerCall.IsHighOrder) {
                                            newCall.Params.Add(innerCall.LowerOrderFunction);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            for (int i = 0; i < callData.GetParamNum(); ++i) {
                                                Dsl.ISyntaxComponent p = callData.Params[i];
                                                newCall.Params.Add(p);
                                            }
                                        }
                                        else {
                                            newCall.Params.Add(innerCall.Name);
                                            newCall.Params.Add(ConvertMember(innerCall.GetParam(0), innerCall.GetParamClass()));
                                            for (int i = 0; i < callData.GetParamNum(); ++i) {
                                                Dsl.ISyntaxComponent p = callData.Params[i];
                                                newCall.Params.Add(p);
                                            }
                                        }

                                        return Load(newCall);
                                    }
                                }
                                if (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                  paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET) {
                                    //obj.property or obj.(property) or obj.[property] or obj.{property} -> dotnetget(obj,property)
                                    //obj[property] -> collectionget(obj,property)
                                    Dsl.FunctionData newCall = new Dsl.FunctionData();
                                    if (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD)
                                        newCall.Name = new Dsl.ValueData("dotnetget", Dsl.ValueData.ID_TOKEN);
                                    else
                                        newCall.Name = new Dsl.ValueData("collectionget", Dsl.ValueData.ID_TOKEN);
                                    newCall.SetParenthesesParamClass();
                                    if (callData.IsHighOrder) {
                                        newCall.Params.Add(callData.LowerOrderFunction);
                                        newCall.Params.Add(ConvertMember(callData.GetParam(0), callData.GetParamClass()));
                                    }
                                    else {
                                        newCall.Params.Add(callData.Name);
                                        newCall.Params.Add(ConvertMember(callData.GetParam(0), callData.GetParamClass()));
                                    }

                                    return Load(newCall);
                                }
                            }
                        }
                    }
                    else {
                        if (funcData.HaveStatement()) {
                            if (!funcData.HaveId() && !funcData.IsHighOrder) {
                                HashtableExp exp = new HashtableExp();
                                exp.Load(comp, this);
                                return exp;
                            }
                        }
                        else if (!funcData.HaveExternScript()) {
                            //degeneration
                            valueData = funcData.Name;
                            return Load(valueData);
                        }
                    }
                }
            }
            IExpression ret = Create(comp.GetId());
            if (null == ret) {
                // We enable the function to be called before it is defined, so failover is done first
                if (null == OnLoadFailback || !OnLoadFailback(comp, this, out ret)) {
                    Dsl.StatementData stData = comp as Dsl.StatementData;
                    if (null != stData) {
                        //Convert command line syntax into function call syntax.
                        if (DslSyntaxTransformer.TryTransformCommandLineLikeSyntax(stData, out var fd)) {
                            funcData = fd;
                        }
                    }
                    if (null != funcData && !funcData.IsHighOrder) {
                        var fc = new FunctionCall();
                        m_FuncCalls.Add(fc);
                        ret = fc;
                    }
                    else {
                        //error
                        Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                    }
                }
            }
            if (null != ret) {
                Dsl.StatementData stData = comp as Dsl.StatementData;
                if (null != stData) {
                    //Convert command line syntax into function call syntax.
                    if (DslSyntaxTransformer.TryTransformCommandLineLikeSyntax(stData, out var fd)) {
                        if (!ret.Load(fd, this)) {
                            //error
                            Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                        }
                        return ret;
                    }
                }
                if (!ret.Load(comp, this)) {
                    //error
                    Log("DslCalculator error, {0} line {1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                }
            }
            return ret;
        }
        public void Log(string fmt, params object[] args)
        {
            if (null != OnLog) {
                if (args.Length == 0)
                    OnLog(fmt);
                else
                    OnLog(string.Format(fmt, args));
            }
        }
        public void Log(object arg)
        {
            if (null != OnLog) {
                OnLog(string.Format("{0}", arg));
            }
        }
        private void LogError(string error)
        {
            error = DslSyntaxTransformer.TranslateDslSyntaxError(error);
            Log(error);
        }
        private Dsl.ISyntaxComponent ConvertMember(Dsl.ISyntaxComponent p, int paramClass)
        {
            var pvd = p as Dsl.ValueData;
            if (null != pvd && pvd.IsId() && (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD
                || paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_POINTER)) {
                pvd.SetType(Dsl.ValueData.STRING_TOKEN);
                return pvd;
            }
            else {
                return p;
            }
        }

        private IExpression Create(string name)
        {
            IExpression ret = null;
            IExpressionFactory factory;
            if (null != m_ApiRegistry && m_ApiRegistry.TryGetFactory(name, out factory)) {
                ret = factory.Create();
            }
            return ret;
        }

        internal Dictionary<string, DslCalculator.FuncInfo> Funcs
        {
            get { return m_Funcs; }
        }
        internal Dictionary<string, int> NamedGlobalVariableIndexes
        {
            get { return m_NamedGlobalVariableIndexes; }
        }
        internal int AllocGlobalVariableIndex(string name)
        {
            if (!m_NamedGlobalVariableIndexes.TryGetValue(name, out int ix)) {
                ix = m_NamedGlobalVariableIndexes.Count;
                m_NamedGlobalVariableIndexes.Add(name, ix);
            }
            return ix;
        }

        private Dictionary<string, DslCalculator.FuncInfo> m_Funcs = new Dictionary<string, DslCalculator.FuncInfo>();
        private List<FunctionCall> m_FuncCalls = new List<FunctionCall>();
        private Dictionary<string, int> m_NamedGlobalVariableIndexes = new Dictionary<string, int>();
        private DslCalculatorApiRegistry m_ApiRegistry;
    }
}
