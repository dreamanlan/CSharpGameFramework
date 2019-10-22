using System;
using System.Collections.Generic;
using ScriptRuntime;
using GameFramework;
using LitJson;

namespace StorySystem
{
    public interface IStoryValueFactory
    {
        IStoryValue Build();
    }
    public sealed class StoryValueFactoryHelper<C> : IStoryValueFactory where C : IStoryValue, new()
    {
        public IStoryValue Build()
        {
            C c = new C();
            return c;
        }
    }
    /// <summary>
    /// 这个类不加锁，约束条件：所有值注册必须在程序启动时完成。
    /// </summary>
    public class StoryValueManager
    {
        public const int c_MaxValueGroupNum = (int)StoryValueGroupDefine.NUM;
        public void RegisterValueFactory(string name, IStoryValueFactory factory)
        {
            RegisterValueFactory(name, factory, false);
        }
        public void RegisterValueFactory(string name, IStoryValueFactory factory, bool replace)
        {
            lock (m_Lock) {
                if (!m_ValueFactories.ContainsKey(name)) {
                    m_ValueFactories.Add(name, factory);
                }
                else if (replace) {
                    m_ValueFactories[name] = factory;
                }
                else {
                    //error
                }
            }
        }
        public void RegisterValueFactory(StoryValueGroupDefine group, string name, IStoryValueFactory factory)
        {
            RegisterValueFactory(group, name, factory, false);
        }
        public void RegisterValueFactory(StoryValueGroupDefine group, string name, IStoryValueFactory factory, bool replace)
        {
            lock (m_Lock) {
                int ix = (int)group;
                if (ix >= 0 && ix < c_MaxValueGroupNum) {
                    Dictionary<string, IStoryValueFactory> handlers = m_GroupedValueFactories[ix];
                    if (!handlers.ContainsKey(name)) {
                        handlers.Add(name, factory);
                    }
                    else if (replace) {
                        handlers[name] = factory;
                    }
                    else {
                        //error
                    }
                }
            }
        }
        public IStoryValueFactory FindFactory(string type)
        {
            IStoryValueFactory factory;
            lock (m_Lock) {
                m_ValueFactories.TryGetValue(type, out factory);
            }
            return factory;
        }
        public IStoryValueFactory FindFactory(StoryValueGroupDefine group, string type)
        {
            IStoryValueFactory factory = null;
            lock (m_Lock) {
                int ix = (int)group;
                if (ix >= 0 && ix < c_MaxValueGroupNum) {
                    Dictionary<string, IStoryValueFactory> factories = m_GroupedValueFactories[ix];
                    factories.TryGetValue(type, out factory);
                }
            }
            return factory;
        }
        public IStoryValue CalcValue(Dsl.ISyntaxComponent param)
        {
            lock (m_Lock) {
                Dsl.CallData callData = param as Dsl.CallData;
                if (null != callData && callData.IsValid() && callData.GetId().Length == 0 && !callData.IsHighOrder && (callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS || callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                    //处理圆括弧与方括弧
                    switch (callData.GetParamClass()) {
                        case (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS:
                            if (callData.GetParamNum() > 0) {
                                int ct = callData.GetParamNum();
                                return CalcValue(callData.GetParam(ct - 1));
                            }
                            else {
                                return null;
                            }
                        case (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET: {
                                IStoryValue ret = null;
                                IStoryValueFactory factory = GetFactory("array");
                                if (null != factory) {
                                    try {
                                        ret = factory.Build();
                                        ret.InitFromDsl(param);
                                    }
                                    catch (Exception ex) {
                                        var msg = string.Format("[LoadStory] value:{0} line:{1} failed.", param.ToScriptString(false), param.GetLine());
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
                        //处理大括弧
                        callData = funcData.Call;
                        if (null == callData || !callData.HaveParam()) {
                            IStoryValue ret = null;
                            IStoryValueFactory factory = GetFactory("hashtable");
                            if (null != factory) {
                                try {
                                    ret = factory.Build();
                                    ret.InitFromDsl(param);
                                }
                                catch (Exception ex) {
                                    var msg = string.Format("[LoadStory] value:{0} line:{1} failed.", param.ToScriptString(false), param.GetLine());
                                    throw new Exception(msg, ex);
                                }
                            }
                            return ret;
                        }
                        else {
                            //不支持的语法
                            return null;
                        }
                    }
                    else {
                        if (null != callData) {
                            Dsl.CallData innerCall = callData.Call;
                            if (callData.IsHighOrder && callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS && (
                                innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                                innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                                innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                                innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                                innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS
                                )) {
                                //obj.member(a,b,...) or obj[member](a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> dotnetcall(obj,member,a,b,...)
                                string method = innerCall.GetParamId(0);
                                string apiName;
                                if (method == "orderby" || method == "orderbydesc" || method == "where" || method == "top") {
                                    apiName = "linq";
                                }
                                else {
                                    apiName = "dotnetcall";
                                }
                                Dsl.CallData newCall = new Dsl.CallData();
                                newCall.Name = new Dsl.ValueData(apiName, Dsl.ValueData.ID_TOKEN);
                                newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                if (innerCall.IsHighOrder) {
                                    newCall.Params.Add(innerCall.Call);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(innerCall.GetParam(0), innerCall.GetParamClass()));
                                    for (int i = 0; i < callData.GetParamNum(); ++i) {
                                        Dsl.ISyntaxComponent p = callData.Params[i];
                                        newCall.Params.Add(p);
                                    }
                                }
                                else {
                                    newCall.Params.Add(innerCall.Name);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(innerCall.GetParam(0), innerCall.GetParamClass()));
                                    for (int i = 0; i < callData.GetParamNum(); ++i) {
                                        Dsl.ISyntaxComponent p = callData.Params[i];
                                        newCall.Params.Add(p);
                                    }
                                }
                                return CalcValue(newCall);
                            }
                            else if (callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                              callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                              callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                              callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                              callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS) {
                                //obj.property or obj[property] or obj.(property) or obj.[property] or obj.{property} -> dotnetget(obj,property)
                                Dsl.CallData newCall = new Dsl.CallData();
                                newCall.Name = new Dsl.ValueData("dotnetget", Dsl.ValueData.ID_TOKEN);
                                newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                if (callData.IsHighOrder) {
                                    newCall.Params.Add(callData.Call);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(callData.GetParam(0), callData.GetParamClass()));
                                }
                                else {
                                    newCall.Params.Add(callData.Name);
                                    newCall.Params.Add(ObjectMemberConverter.Convert(callData.GetParam(0), callData.GetParamClass()));
                                }
                                return CalcValue(newCall);
                            }
                        }
                        IStoryValue ret = null;
                        string id = param.GetId();
                        if (param.GetIdType() == Dsl.ValueData.ID_TOKEN) {
                            IStoryValueFactory factory = GetFactory(id);
                            if (null != factory) {
                                try {
                                    ret = factory.Build();
                                    ret.InitFromDsl(param);
                                }
                                catch (Exception ex) {
                                    var msg = string.Format("[LoadStory] value:{0}[{1}] line:{2} failed.", id, param.ToScriptString(false), param.GetLine());
                                    throw new Exception(msg, ex);
                                }
                            }
                            else {
#if DEBUG
                                string err = string.Format("[LoadStory] value:{0}[{1}] line:{2} failed.", id, param.ToScriptString(false), param.GetLine());
                                GameFramework.LogSystem.Error("{0}", err);
                                throw new Exception(err);
#else
                                GameFramework.LogSystem.Error("[LoadStory] value:{0} line:{1} failed.", id, param.GetLine());
#endif
                            }
                        }
                        return ret;
                    }
                }
            }
        }
        private IStoryValueFactory GetFactory(string id)
        {
            IStoryValueFactory handler;
            lock (m_Lock) {
                if (!m_ValueFactories.TryGetValue(id, out handler)) {
                    const ulong one = 1;
                    for (int ix = 0; ix < c_MaxValueGroupNum; ++ix) {
                        if ((s_ThreadValueGroupsMask & (one << ix)) != 0 && m_GroupedValueFactories[ix].TryGetValue(id, out handler)) {
                            break;
                        }
                    }
                }
            }
            return handler;
        }
        private StoryValueManager()
        {
            for (int i = 0; i < c_MaxValueGroupNum; ++i) {
                m_GroupedValueFactories[i] = new Dictionary<string, IStoryValueFactory>();
            }
        }
        private object m_Lock = new object();
        private Dictionary<string, IStoryValueFactory> m_ValueFactories = new Dictionary<string, IStoryValueFactory>();
        private Dictionary<string, IStoryValueFactory>[] m_GroupedValueFactories = new Dictionary<string, IStoryValueFactory>[c_MaxValueGroupNum];
        public static ulong ThreadValueGroupsMask
        {
            get { return s_ThreadValueGroupsMask; }
            set { s_ThreadValueGroupsMask = value; }
        }
        [ThreadStatic]
        private static ulong s_ThreadValueGroupsMask = 0;
        public static StoryValueManager Instance
        {
            get {
                return s_Instance;
            }
        }
        private static StoryValueManager s_Instance = new StoryValueManager();
    }
}
