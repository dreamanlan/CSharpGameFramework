using System;
using System.Collections.Generic;
using ScriptRuntime;
using GameFramework;
using LitJson;

namespace StorySystem
{
    public interface IStoryValueFactory
    {
        IStoryValue<object> Build(Dsl.ISyntaxComponent param);
    }
    public sealed class StoryValueFactoryHelper<C> : IStoryValueFactory where C : IStoryValue<object>, new()
    {
        public IStoryValue<object> Build(Dsl.ISyntaxComponent param)
        {
            C c = new C();
            c.InitFromDsl(param);
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
            if (!m_ValueFactories.ContainsKey(name)) {
                m_ValueFactories.Add(name, factory);
            } else {
                //error
            }
        }
        public void RegisterValueFactory(StoryValueGroupDefine group, string name, IStoryValueFactory factory)
        {
            int ix = (int)group;
            if (ix >= 0 && ix < c_MaxValueGroupNum) {
                Dictionary<string, IStoryValueFactory> handlers = m_GroupedValueFactories[ix];
                if (!handlers.ContainsKey(name)) {
                    handlers.Add(name, factory);
                } else {
                    //error
                }
            }
        }
        public IStoryValue<object> CalcValue(Dsl.ISyntaxComponent param)
        {
            if (param.IsValid() && param.GetId().Length == 0) {
                //处理括弧
                Dsl.CallData callData = param as Dsl.CallData;
                if (null != callData) {
                    switch (callData.GetParamClass()) {
                        case (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS:
                            if (callData.GetParamNum() > 0) {
                                int ct = callData.GetParamNum();
                                return CalcValue(callData.GetParam(ct - 1));
                            } else {
                                //不支持的语法
                                return null;
                            }
                        case (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET: {
                                IStoryValue<object> ret = null;
                                IStoryValueFactory factory = GetFactory("jsonarray");
                                if (null != factory) {
                                    ret = factory.Build(param);
                                }
                                return ret;
                            }
                        default:
                            return null;
                    }
                } else {
                    Dsl.FunctionData funcData = param as Dsl.FunctionData;
                    if (null != funcData && funcData.HaveStatement()) {
                        callData = funcData.Call;
                        if (null == callData || !callData.HaveParam()) {
                            IStoryValue<object> ret = null;
                            IStoryValueFactory factory = GetFactory("jsonobject");
                            if (null != factory) {
                                ret = factory.Build(param);
                            }
                            return ret;
                        } else {
                            //不支持的语法
                            return null;
                        }
                    } else {
                        //不支持的语法
                        return null;
                    }
                }
            } else {
                Dsl.CallData callData = param as Dsl.CallData;
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
                        Dsl.CallData newCall = new Dsl.CallData();
                        newCall.Name = new Dsl.ValueData("dotnetcall", Dsl.ValueData.ID_TOKEN);
                        newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                        if (innerCall.IsHighOrder) {
                            newCall.Params.Add(innerCall.Call);
                            newCall.Params.Add(innerCall.GetParam(0));
                            for (int i = 0; i < callData.GetParamNum(); ++i) {
                                Dsl.ISyntaxComponent p = callData.Params[i];
                                newCall.Params.Add(p);
                            }
                        } else {
                            newCall.Params.Add(innerCall.Name);
                            newCall.Params.Add(innerCall.GetParam(0));
                            for (int i = 0; i < callData.GetParamNum(); ++i) {
                                Dsl.ISyntaxComponent p = callData.Params[i];
                                newCall.Params.Add(p);
                            }
                        }
                        return CalcValue(newCall);
                    } else if (callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
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
                            newCall.Params.Add(callData.GetParam(0));
                        } else {
                            newCall.Params.Add(callData.Name);
                            newCall.Params.Add(callData.GetParam(0));
                        }
                        return CalcValue(newCall);
                    }
                }
                IStoryValue<object> ret = null;
                string id = param.GetId();
                IStoryValueFactory factory = GetFactory(id);
                if (null != factory) {
                    ret = factory.Build(param);
                }
                return ret;
            }
        }

        private IStoryValueFactory GetFactory(string id)
        {
            IStoryValueFactory handler;
            if (m_ValueFactories.TryGetValue(id, out handler)) {
                return handler;
            }
            const ulong one = 1;
            for (int ix = 0; ix < c_MaxValueGroupNum; ++ix) {
                if ((s_ThreadValueGroupsMask & (one << ix)) != 0 && m_GroupedValueFactories[ix].TryGetValue(id, out handler)) {
                    return handler;
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
            get
            {
                return s_Instance;
            }
        }
        private static StoryValueManager s_Instance = new StoryValueManager();
    }
}
