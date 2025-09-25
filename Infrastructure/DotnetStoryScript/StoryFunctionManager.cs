using System;
using System.Collections.Generic;
using ScriptableFramework;
using LitJson;

namespace DotnetStoryScript
{
    public interface IStoryFunctionFactory
    {
        IStoryFunction Build();
    }
    public sealed class StoryFunctionFactoryHelper<C> : IStoryFunctionFactory where C : IStoryFunction, new()
    {
        public IStoryFunction Build()
        {
            C c = new C();
            return c;
        }
    }
    /// <summary>
    /// This class does not lock, constraint:
    /// all value/function registration must be completed when the program starts.
    /// </summary>
    public class StoryFunctionManager
    {
        public delegate bool CreateFailbackDelegation(Dsl.ISyntaxComponent comp, out IStoryFunction expression);
        public CreateFailbackDelegation OnCreateFailback;
        public void RegisterFunctionFactory(string name, string doc, IStoryFunctionFactory factory)
        {
            RegisterFunctionFactory(name, doc, factory, false);
        }
        public void RegisterFunctionFactory(string name, string doc, IStoryFunctionFactory factory, bool replace)
        {
            lock (m_Lock) {
                if (!m_FunctionFactories.ContainsKey(name)) {
                    m_FunctionFactories.Add(name, factory);
                }
                else if (replace) {
                    m_FunctionFactories[name] = factory;
                }
                else {
                    //ignore or warning
                }
                if (!m_FunctionDocs.ContainsKey(name)) {
                    m_FunctionDocs.Add(name, doc);
                }
                else if (replace) {
                    m_FunctionDocs[name] = doc;
                }
                else {
                    //ignore or warning
                }
            }
        }
        public void RegisterFunctionFactory(StoryFunctionGroupDefine group, string name, string doc, IStoryFunctionFactory factory)
        {
            RegisterFunctionFactory(group, name, doc, factory, false);
        }
        public void RegisterFunctionFactory(StoryFunctionGroupDefine group, string name, string doc, IStoryFunctionFactory factory, bool replace)
        {
            lock (m_Lock) {
                int ix = (int)group;
                if (ix >= 0 && ix < c_MaxFunctionGroupNum) {
                    Dictionary<string, IStoryFunctionFactory> handlers = m_GroupedFunctionFactories[ix];
                    if (!handlers.ContainsKey(name)) {
                        handlers.Add(name, factory);
                    }
                    else if (replace) {
                        handlers[name] = factory;
                    }
                    else {
                        //ignore or warning
                    }
                    SortedList<string, string> docs = m_GroupedFunctionDocs[ix];
                    if (!docs.ContainsKey(name)) {
                        docs.Add(name, doc);
                    }
                    else if (replace) {
                        docs[name] = doc;
                    }
                    else {
                        //ignore or warning
                    }
                }
            }
        }
        public SortedList<string, string> GenFunctionDocs()
        {
            SortedList<string, string> docs;
            lock (m_Lock) {
                docs = new SortedList<string, string>(m_FunctionDocs);
                const ulong c_one = 1;
                for (int ix = 0; ix < c_MaxFunctionGroupNum; ++ix) {
                    if ((s_ThreadFunctionGroupsMask & (c_one << ix)) != 0) {
                        foreach(var pair in m_GroupedFunctionDocs[ix]) {
                            if (!docs.ContainsKey(pair.Key)) {
                                docs.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                }
            }
            return docs;
        }
        public IStoryFunctionFactory FindFactory(string type)
        {
            IStoryFunctionFactory factory;
            lock (m_Lock) {
                m_FunctionFactories.TryGetValue(type, out factory);
            }
            return factory;
        }
        public IStoryFunctionFactory FindFactory(StoryFunctionGroupDefine group, string type)
        {
            IStoryFunctionFactory factory = null;
            lock (m_Lock) {
                int ix = (int)group;
                if (ix >= 0 && ix < c_MaxFunctionGroupNum) {
                    Dictionary<string, IStoryFunctionFactory> factories = m_GroupedFunctionFactories[ix];
                    factories.TryGetValue(type, out factory);
                }
            }
            return factory;
        }
        public IStoryFunction CreateFunction(Dsl.ISyntaxComponent param)
        {
            lock (m_Lock) {
                Dsl.StatementData statementData = param as Dsl.StatementData;
                if (null != statementData) {
                    Dsl.FunctionData func;
                    if (DslSyntaxTransformer.TryTransformCommandLineLikeSyntax(statementData, out func)) {
                        param = func;
                    }
                }
                Dsl.FunctionData callData = param as Dsl.FunctionData;
                if (null != callData && callData.IsValid() && callData.GetId().Length == 0 && !callData.IsHighOrder && (callData.GetParamClass() == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES || callData.GetParamClass() == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                    //Handling parentheses and square brackets
                    switch (callData.GetParamClass()) {
                        case (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES: {
                                int ct = callData.GetParamNum();
                                if (ct == 1) {
                                    return CreateFunction(callData.GetParam(0));
                                }
                                else {
                                    IStoryFunction ret = null;
                                    IStoryFunctionFactory factory = GetFactory("tuple");
                                    if (null != factory) {
                                        try {
                                            ret = factory.Build();
                                            ret.InitFromDsl(param);
                                        }
                                        catch (Exception ex) {
                                            var msg = string.Format("[LoadStory] value:{0} line:{1} failed.", param.ToScriptString(false, Dsl.DelimiterInfo.Default), param.GetLine());
                                            throw new Exception(msg, ex);
                                        }
                                    }
                                    return ret;
                                }
                            }
                        case (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET: {
                                IStoryFunction ret = null;
                                IStoryFunctionFactory factory = GetFactory("array");
                                if (null != factory) {
                                    try {
                                        ret = factory.Build();
                                        ret.InitFromDsl(param);
                                    }
                                    catch (Exception ex) {
                                        var msg = string.Format("[LoadStory] value:{0} line:{1} failed.", param.ToScriptString(false, Dsl.DelimiterInfo.Default), param.GetLine());
                                        throw new Exception(msg, ex);
                                    }
                                }
                                return ret;
                            }
                        default:
                            return null;
                    }
                }
                else {
                    Dsl.FunctionData funcData = param as Dsl.FunctionData;
                    if (null != funcData && funcData.HaveStatement()) {
                        //Dealing with braces
                        callData = funcData;
                        if (null == callData || !callData.HaveParam()) {
                            IStoryFunction ret = null;
                            IStoryFunctionFactory factory = GetFactory("hashtable");
                            if (null != factory) {
                                try {
                                    ret = factory.Build();
                                    ret.InitFromDsl(param);
                                }
                                catch (Exception ex) {
                                    var msg = string.Format("[LoadStory] value:{0} line:{1} failed.", param.ToScriptString(false, Dsl.DelimiterInfo.Default), param.GetLine());
                                    throw new Exception(msg, ex);
                                }
                            }
                            return ret;
                        }
                        else {
                            //unsupported syntax
                            return null;
                        }
                    }
                    else {
                        if (null != callData) {
                            int paramClass = callData.GetParamClass();
                            Dsl.FunctionData innerCall = callData.LowerOrderFunction;
                            int innerParamClass = innerCall.GetParamClass();
                            if (callData.IsHighOrder && paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES && (
                                innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET
                                )) {
                                //obj.member(a,b,...) -> dotnetcall(obj,member,a,b,...)
                                //obj[member](a,b,...) -> collectioncall(obj,member,a,b,...)
                                string method = innerCall.GetParamId(0);
                                string apiName;
                                if (method == "orderby" || method == "orderbydesc" || method == "where" || method == "top") {
                                    apiName = "linq";
                                }
                                else if(innerParamClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD) {
                                    apiName = "dotnetcall";
                                }
                                else {
                                    apiName = "collectioncall";
                                }
                                Dsl.FunctionData newCall = new Dsl.FunctionData();
                                newCall.Name = new Dsl.ValueData(apiName, Dsl.ValueData.ID_TOKEN);
                                newCall.SetParamClass((int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES);
                                if (innerCall.IsHighOrder) {
                                    newCall.Params.Add(innerCall.LowerOrderFunction);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(innerCall.GetParam(0), innerParamClass));
                                    for (int i = 0; i < callData.GetParamNum(); ++i) {
                                        Dsl.ISyntaxComponent p = callData.Params[i];
                                        newCall.Params.Add(p);
                                    }
                                }
                                else {
                                    newCall.Params.Add(innerCall.Name);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(innerCall.GetParam(0), innerParamClass));
                                    for (int i = 0; i < callData.GetParamNum(); ++i) {
                                        Dsl.ISyntaxComponent p = callData.Params[i];
                                        newCall.Params.Add(p);
                                    }
                                }
                                return CreateFunction(newCall);
                            }
                            else if (paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD ||
                              paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_BRACKET) {
                                //obj.property -> dotnetget(obj,property)
                                //obj[property] -> collectionget(obj,property)
                                Dsl.FunctionData newCall = new Dsl.FunctionData();
                                if(paramClass == (int)Dsl.ParamClassEnum.PARAM_CLASS_PERIOD)
                                    newCall.Name = new Dsl.ValueData("dotnetget", Dsl.ValueData.ID_TOKEN);
                                else
                                    newCall.Name = new Dsl.ValueData("collectionget", Dsl.ValueData.ID_TOKEN);
                                newCall.SetParamClass((int)Dsl.ParamClassEnum.PARAM_CLASS_PARENTHESES);
                                if (callData.IsHighOrder) {
                                    newCall.Params.Add(callData.LowerOrderFunction);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(callData.GetParam(0), paramClass));
                                }
                                else {
                                    newCall.Params.Add(callData.Name);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(callData.GetParam(0), paramClass));
                                }
                                return CreateFunction(newCall);
                            }
                            else if (callData.GetId() == "`") {//backtick
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
                                            return CreateFunction(newCall);
                                        }
                                    }
                                    else if (param0 is Dsl.FunctionData fd) {
                                        Dsl.FunctionData newCall = new Dsl.FunctionData();
                                        newCall.LowerOrderFunction = fd;
                                        newCall.SetStatementParamClass();
                                        newCall.Params.Add(param1);
                                        return CreateFunction(newCall);
                                    }
                                }
                            }
                        }
                        IStoryFunction ret = null;
                        string id = param.GetId();
                        if (param.GetIdType() == Dsl.ValueData.ID_TOKEN && id != "true" && id != "false") {
                            IStoryFunctionFactory factory = GetFactory(id);
                            if (null != factory) {
                                try {
                                    ret = factory.Build();
                                    ret.InitFromDsl(param);
                                }
                                catch (Exception ex) {
                                    var msg = string.Format("[LoadStory] value:{0}[{1}] line:{2} failed.", id, param.ToScriptString(false, Dsl.DelimiterInfo.Default), param.GetLine());
                                    throw new Exception(msg, ex);
                                }
                            }
                            else if (null == OnCreateFailback || !OnCreateFailback(param, out ret)) {
#if DEBUG
                                string err = string.Format("[LoadStory] value:{0}[{1}] line:{2} failed.", id, param.ToScriptString(false, Dsl.DelimiterInfo.Default), param.GetLine());
                                LogSystem.Error("{0}", err);
                                throw new Exception(err);
#else
                                LogSystem.Error("[LoadStory] value:{0} line:{1} failed.", id, param.GetLine());
#endif
                            }
                        }
                        return ret;
                    }
                }
            }
        }
        public void Substitute(string id, string substId)
        {
            lock (m_Lock) {
                m_Substitutes[id] = substId;
            }
        }
        public bool TryGetSubstitute(string id, out string substId)
        {
            bool r = false;
            lock (m_Lock) {
                r = m_Substitutes.TryGetValue(id, out substId);
            }
            return r;
        }
        public void ClearSubstitutes()
        {
            lock (m_Lock) {
                m_Substitutes.Clear();
            }
        }

        private IStoryFunctionFactory GetFactory(string id)
        {
            IStoryFunctionFactory handler;
            lock (m_Lock) {
                string substId;
                if (m_Substitutes.TryGetValue(id, out substId)) {
                    id = substId;
                }
                if (!m_FunctionFactories.TryGetValue(id, out handler)) {
                    const ulong c_one = 1;
                    for (int ix = 0; ix < c_MaxFunctionGroupNum; ++ix) {
                        if ((s_ThreadFunctionGroupsMask & (c_one << ix)) != 0 && m_GroupedFunctionFactories[ix].TryGetValue(id, out handler)) {
                            break;
                        }
                    }
                }
            }
            return handler;
        }
        private StoryFunctionManager()
        {
            for (int i = 0; i < c_MaxFunctionGroupNum; ++i) {
                m_GroupedFunctionFactories[i] = new Dictionary<string, IStoryFunctionFactory>();
                m_GroupedFunctionDocs[i] = new SortedList<string, string>();
            }
        }
        private object m_Lock = new object();
        private Dictionary<string, IStoryFunctionFactory> m_FunctionFactories = new Dictionary<string, IStoryFunctionFactory>();
        private SortedList<string, string> m_FunctionDocs = new SortedList<string, string>();
        private Dictionary<string, IStoryFunctionFactory>[] m_GroupedFunctionFactories = new Dictionary<string, IStoryFunctionFactory>[c_MaxFunctionGroupNum];
        private SortedList<string, string>[] m_GroupedFunctionDocs = new SortedList<string, string>[c_MaxFunctionGroupNum];
        private Dictionary<string, string> m_Substitutes = new Dictionary<string, string>();

        public const int c_MaxFunctionGroupNum = (int)StoryFunctionGroupDefine.NUM;
        public static ulong ThreadFunctionGroupsMask
        {
            get { return s_ThreadFunctionGroupsMask; }
            set { s_ThreadFunctionGroupsMask = value; }
        }
        [ThreadStatic]
        private static ulong s_ThreadFunctionGroupsMask = 0;
        public static StoryFunctionManager Instance
        {
            get {
                return s_Instance;
            }
        }
        private static StoryFunctionManager s_Instance = new StoryFunctionManager();
    }
}
