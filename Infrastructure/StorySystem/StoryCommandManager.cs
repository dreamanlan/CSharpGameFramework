using System;
using System.Collections.Generic;
using ScriptRuntime;
using GameFramework;

namespace StorySystem
{
    /// <summary>
    /// 这个类不加锁，约束条件：所有命令注册必须在程序启动时完成。
    /// </summary>
    public sealed class StoryCommandManager
    {
        public const int c_MaxCommandGroupNum = (int)StoryCommandGroupDefine.NUM;

        public void RegisterCommandFactory(string type, IStoryCommandFactory factory)
        {
            if (!m_StoryCommandFactories.ContainsKey(type)) {
                m_StoryCommandFactories.Add(type, factory);
            } else {
                //error
            }
        }
        public void RegisterCommandFactory(StoryCommandGroupDefine group, string type, IStoryCommandFactory factory)
        {
            int ix = (int)group;
            if (ix >= 0 && ix < c_MaxCommandGroupNum) {
                Dictionary<string, IStoryCommandFactory> factories = m_GroupedCommandFactories[ix];
                if (!factories.ContainsKey(type)) {
                    factories.Add(type, factory);
                } else {
                    //error
                }
            }
        }
        public IStoryCommand CreateCommand(Dsl.ISyntaxComponent commandConfig)
        {
            Dsl.CallData callData = commandConfig as Dsl.CallData;
            if (null != callData) {
                if (callData.IsHighOrder) {
                    Dsl.CallData innerCall = callData.Call;
                    if (innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                      innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                      innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                      innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                      innerCall.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS) {
                        if (callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS) {
                            //obj.member(a,b,...) or obj[member](a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> execinstance(obj,member,a,b,...)
                            Dsl.CallData newCall = new Dsl.CallData();
                            newCall.Name = new Dsl.ValueData("dotnetexec", Dsl.ValueData.ID_TOKEN);
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
                            return CreateCommand(newCall);
                        }
                    }
                } else if (callData.GetParamClass() == (int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_OPERATOR && callData.GetId() == "=") {
                    Dsl.CallData innerCall = callData.GetParam(0) as Dsl.CallData;
                    if (null != innerCall) {
                        //obj.property = val -> setinstance(obj,property,val)
                        Dsl.CallData newCall = new Dsl.CallData();
                        newCall.Name = new Dsl.ValueData("dotnetset", Dsl.ValueData.ID_TOKEN);
                        newCall.SetParamClass((int)Dsl.CallData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                        if (innerCall.IsHighOrder) {
                            newCall.Params.Add(innerCall.Call);
                            newCall.Params.Add(innerCall.GetParam(0));
                            newCall.Params.Add(callData.GetParam(1));
                        } else {
                            newCall.Params.Add(innerCall.Name);
                            newCall.Params.Add(innerCall.GetParam(0));
                            newCall.Params.Add(callData.GetParam(1));
                        }
                        return CreateCommand(newCall);
                    }
                }
            }
            IStoryCommand command = null;
            string type = commandConfig.GetId();
            IStoryCommandFactory factory = GetFactory(type);
            if (null != factory) {
                command = factory.Create(commandConfig);
            } else {
#if DEBUG
                string err = string.Format("CreateCommand failed, line:{0} command:{1}", commandConfig.GetLine(), commandConfig.ToScriptString());
                throw new Exception(err);
#else
      GameFramework.LogSystem.Error("CreateCommand failed, type:{0}", type, commandConfig.GetLine());
#endif
            }
            if (null != command) {
                GameFramework.LogSystem.Debug("CreateCommand, type:{0} command:{1}", type, command.GetType().Name);
            } else {
#if DEBUG
                string err = string.Format("CreateCommand failed, line:{0} command:{1}", commandConfig.GetLine(), commandConfig.ToScriptString());
                throw new Exception(err);
#else
      GameFramework.LogSystem.Error("CreateCommand failed, type:{0}", type, commandConfig.GetLine());
#endif
            }
            return command;
        }

        private IStoryCommandFactory GetFactory(string type)
        {
            IStoryCommandFactory factory;
            if (m_StoryCommandFactories.TryGetValue(type, out factory)) {
                return factory;
            }
            const ulong one = 1;
            for (int ix = 0; ix < c_MaxCommandGroupNum; ++ix) {
                if ((s_ThreadCommandGroupsMask & (one << ix)) != 0 && m_GroupedCommandFactories[ix].TryGetValue(type, out factory)) {
                    return factory;
                }
            }
            return factory;
        }

        private StoryCommandManager()
        {
            for (int i = 0; i < c_MaxCommandGroupNum; ++i) {
                m_GroupedCommandFactories[i] = new Dictionary<string, IStoryCommandFactory>();
            }
            //注册通用命令
            RegisterCommandFactory("=", new StoryCommandFactoryHelper<CommonCommands.AssignCommand>());
            RegisterCommandFactory("assign", new StoryCommandFactoryHelper<CommonCommands.AssignCommand>());
            RegisterCommandFactory("inc", new StoryCommandFactoryHelper<CommonCommands.IncCommand>());
            RegisterCommandFactory("dec", new StoryCommandFactoryHelper<CommonCommands.DecCommand>());
            RegisterCommandFactory("propset", new StoryCommandFactoryHelper<CommonCommands.PropSetCommand>());
            RegisterCommandFactory("foreach", new StoryCommandFactoryHelper<CommonCommands.ForeachCommand>());
            RegisterCommandFactory("looplist", new StoryCommandFactoryHelper<CommonCommands.LoopListCommand>());
            RegisterCommandFactory("loop", new StoryCommandFactoryHelper<CommonCommands.LoopCommand>());
            RegisterCommandFactory("wait", new StoryCommandFactoryHelper<CommonCommands.SleepCommand>());
            RegisterCommandFactory("sleep", new StoryCommandFactoryHelper<CommonCommands.SleepCommand>());
            RegisterCommandFactory("terminate", new StoryCommandFactoryHelper<CommonCommands.TerminateCommand>());
            RegisterCommandFactory("localmessage", new StoryCommandFactoryHelper<CommonCommands.LocalMessageCommand>());
            RegisterCommandFactory("clearmessage", new StoryCommandFactoryHelper<CommonCommands.ClearMessageCommand>());
            RegisterCommandFactory("waitlocalmessage", new StoryCommandFactoryHelper<CommonCommands.WaitLocalMessageCommand>());
            RegisterCommandFactory("waitlocalmessagehandler", new StoryCommandFactoryHelper<CommonCommands.WaitLocalMessageHandlerCommand>());
            RegisterCommandFactory("localnamespacedmessage", new StoryCommandFactoryHelper<CommonCommands.LocalNamespacedMessageCommand>());
            RegisterCommandFactory("clearnamespacedmessage", new StoryCommandFactoryHelper<CommonCommands.ClearNamespacedMessageCommand>());
            RegisterCommandFactory("waitlocalnamespacedmessage", new StoryCommandFactoryHelper<CommonCommands.WaitLocalNamespacedMessageCommand>());
            RegisterCommandFactory("waitlocalnamespacedmessagehandler", new StoryCommandFactoryHelper<CommonCommands.WaitLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("while", new StoryCommandFactoryHelper<CommonCommands.WhileCommand>());
            RegisterCommandFactory("if", new StoryCommandFactoryHelper<CommonCommands.IfElseCommand>());
            RegisterCommandFactory("log", new StoryCommandFactoryHelper<CommonCommands.LogCommand>());
            RegisterCommandFactory("listset", new StoryCommandFactoryHelper<CommonCommands.ListSetCommand>());
            RegisterCommandFactory("dotnetexec", new StoryCommandFactoryHelper<CommonCommands.DotnetExecCommand>());
            RegisterCommandFactory("dotnetset", new StoryCommandFactoryHelper<CommonCommands.DotnetSetCommand>());
            RegisterCommandFactory("system", new StoryCommandFactoryHelper<CommonCommands.SystemCommand>());
            RegisterCommandFactory("jsonadd", new StoryCommandFactoryHelper<CommonCommands.JsonAddCommand>());
            RegisterCommandFactory("jsonset", new StoryCommandFactoryHelper<CommonCommands.JsonSetCommand>());
            RegisterCommandFactory("jsonremove", new StoryCommandFactoryHelper<CommonCommands.JsonRemoveCommand>());

            //注册通用值与内部函数
            //object
            StoryValueManager.Instance.RegisterValueFactory("namespace", new StoryValueFactoryHelper<CommonValues.NamespaceValue>());
            StoryValueManager.Instance.RegisterValueFactory("propget", new StoryValueFactoryHelper<CommonValues.PropGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("rndint", new StoryValueFactoryHelper<CommonValues.RandomIntValue>());
            StoryValueManager.Instance.RegisterValueFactory("rndfloat", new StoryValueFactoryHelper<CommonValues.RandomFloatValue>());
            StoryValueManager.Instance.RegisterValueFactory("vector2", new StoryValueFactoryHelper<CommonValues.Vector2Value>());
            StoryValueManager.Instance.RegisterValueFactory("vector3", new StoryValueFactoryHelper<CommonValues.Vector3Value>());
            StoryValueManager.Instance.RegisterValueFactory("vector4", new StoryValueFactoryHelper<CommonValues.Vector4Value>());
            StoryValueManager.Instance.RegisterValueFactory("quaternion", new StoryValueFactoryHelper<CommonValues.QuaternionValue>());
            StoryValueManager.Instance.RegisterValueFactory("eular", new StoryValueFactoryHelper<CommonValues.EularValue>());
            StoryValueManager.Instance.RegisterValueFactory("stringlist", new StoryValueFactoryHelper<CommonValues.StringListValue>());
            StoryValueManager.Instance.RegisterValueFactory("intlist", new StoryValueFactoryHelper<CommonValues.IntListValue>());
            StoryValueManager.Instance.RegisterValueFactory("floatlist", new StoryValueFactoryHelper<CommonValues.FloatListValue>());
            StoryValueManager.Instance.RegisterValueFactory("vector2list", new StoryValueFactoryHelper<CommonValues.Vector2ListValue>());
            StoryValueManager.Instance.RegisterValueFactory("vector3list", new StoryValueFactoryHelper<CommonValues.Vector3ListValue>());
            StoryValueManager.Instance.RegisterValueFactory("list", new StoryValueFactoryHelper<CommonValues.ListValue>());
            StoryValueManager.Instance.RegisterValueFactory("rndfromlist", new StoryValueFactoryHelper<CommonValues.RandomFromListValue>());
            StoryValueManager.Instance.RegisterValueFactory("listget", new StoryValueFactoryHelper<CommonValues.ListGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("listsize", new StoryValueFactoryHelper<CommonValues.ListSizeValue>());
            StoryValueManager.Instance.RegisterValueFactory("vector2dist", new StoryValueFactoryHelper<CommonValues.Vector2DistanceValue>());
            StoryValueManager.Instance.RegisterValueFactory("vector3dist", new StoryValueFactoryHelper<CommonValues.Vector3DistanceValue>());
            StoryValueManager.Instance.RegisterValueFactory("vector2to3", new StoryValueFactoryHelper<CommonValues.Vector2To3Value>());
            StoryValueManager.Instance.RegisterValueFactory("vector3to2", new StoryValueFactoryHelper<CommonValues.Vector3To2Value>());
            StoryValueManager.Instance.RegisterValueFactory("rndvector3", new StoryValueFactoryHelper<CommonValues.RandVector3Value>());
            StoryValueManager.Instance.RegisterValueFactory("rndvector2", new StoryValueFactoryHelper<CommonValues.RandVector2Value>());
            StoryValueManager.Instance.RegisterValueFactory("+", new StoryValueFactoryHelper<CommonValues.AddOperator>());
            StoryValueManager.Instance.RegisterValueFactory("-", new StoryValueFactoryHelper<CommonValues.SubOperator>());
            StoryValueManager.Instance.RegisterValueFactory("*", new StoryValueFactoryHelper<CommonValues.MulOperator>());
            StoryValueManager.Instance.RegisterValueFactory("/", new StoryValueFactoryHelper<CommonValues.DivOperator>());
            StoryValueManager.Instance.RegisterValueFactory("%", new StoryValueFactoryHelper<CommonValues.ModOperator>());
            StoryValueManager.Instance.RegisterValueFactory("abs", new StoryValueFactoryHelper<CommonValues.AbsOperator>());
            StoryValueManager.Instance.RegisterValueFactory("floor", new StoryValueFactoryHelper<CommonValues.FloorOperator>());
            StoryValueManager.Instance.RegisterValueFactory("ceiling", new StoryValueFactoryHelper<CommonValues.CeilingOperator>());
            StoryValueManager.Instance.RegisterValueFactory("round", new StoryValueFactoryHelper<CommonValues.RoundOperator>());
            StoryValueManager.Instance.RegisterValueFactory("pow", new StoryValueFactoryHelper<CommonValues.PowOperator>());
            StoryValueManager.Instance.RegisterValueFactory("log", new StoryValueFactoryHelper<CommonValues.LogOperator>());
            StoryValueManager.Instance.RegisterValueFactory("sqrt", new StoryValueFactoryHelper<CommonValues.SqrtOperator>());
            StoryValueManager.Instance.RegisterValueFactory("sin", new StoryValueFactoryHelper<CommonValues.SinOperator>());
            StoryValueManager.Instance.RegisterValueFactory("cos", new StoryValueFactoryHelper<CommonValues.CosOperator>());
            StoryValueManager.Instance.RegisterValueFactory("sinh", new StoryValueFactoryHelper<CommonValues.SinhOperator>());
            StoryValueManager.Instance.RegisterValueFactory("cosh", new StoryValueFactoryHelper<CommonValues.CoshOperator>());
            StoryValueManager.Instance.RegisterValueFactory("min", new StoryValueFactoryHelper<CommonValues.MinOperator>());
            StoryValueManager.Instance.RegisterValueFactory("max", new StoryValueFactoryHelper<CommonValues.MaxOperator>());
            StoryValueManager.Instance.RegisterValueFactory(">", new StoryValueFactoryHelper<CommonValues.GreaterThanOperator>());
            StoryValueManager.Instance.RegisterValueFactory(">=", new StoryValueFactoryHelper<CommonValues.GreaterEqualThanOperator>());
            StoryValueManager.Instance.RegisterValueFactory("==", new StoryValueFactoryHelper<CommonValues.EqualOperator>());
            StoryValueManager.Instance.RegisterValueFactory("!=", new StoryValueFactoryHelper<CommonValues.NotEqualOperator>());
            StoryValueManager.Instance.RegisterValueFactory("<", new StoryValueFactoryHelper<CommonValues.LessThanOperator>());
            StoryValueManager.Instance.RegisterValueFactory("<=", new StoryValueFactoryHelper<CommonValues.LessEqualThanOperator>());
            StoryValueManager.Instance.RegisterValueFactory("&&", new StoryValueFactoryHelper<CommonValues.AndOperator>());
            StoryValueManager.Instance.RegisterValueFactory("||", new StoryValueFactoryHelper<CommonValues.OrOperator>());
            StoryValueManager.Instance.RegisterValueFactory("!", new StoryValueFactoryHelper<CommonValues.NotOperator>());
            StoryValueManager.Instance.RegisterValueFactory("format", new StoryValueFactoryHelper<CommonValues.FormatValue>());
            StoryValueManager.Instance.RegisterValueFactory("substring", new StoryValueFactoryHelper<CommonValues.SubstringValue>());
            StoryValueManager.Instance.RegisterValueFactory("str2int", new StoryValueFactoryHelper<CommonValues.Str2IntValue>());
            StoryValueManager.Instance.RegisterValueFactory("str2float", new StoryValueFactoryHelper<CommonValues.Str2FloatValue>());
            StoryValueManager.Instance.RegisterValueFactory("dictformat", new StoryValueFactoryHelper<CommonValues.DictFormatValue>());
            StoryValueManager.Instance.RegisterValueFactory("dictget", new StoryValueFactoryHelper<CommonValues.DictGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("dictparse", new StoryValueFactoryHelper<CommonValues.DictParseValue>());
            StoryValueManager.Instance.RegisterValueFactory("time", new StoryValueFactoryHelper<CommonValues.TimeValue>());
            StoryValueManager.Instance.RegisterValueFactory("isnull", new StoryValueFactoryHelper<CommonValues.IsNullOperator>());
            StoryValueManager.Instance.RegisterValueFactory("gettype", new StoryValueFactoryHelper<CommonValues.GetTypeValue>());
            StoryValueManager.Instance.RegisterValueFactory("dotnetcall", new StoryValueFactoryHelper<CommonValues.DotnetCallValue>());
            StoryValueManager.Instance.RegisterValueFactory("dotnetget", new StoryValueFactoryHelper<CommonValues.DotnetGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("changetype", new StoryValueFactoryHelper<CommonValues.ChangeTypeValue>());
            StoryValueManager.Instance.RegisterValueFactory("pgrep", new StoryValueFactoryHelper<CommonValues.PgrepValue>());
            StoryValueManager.Instance.RegisterValueFactory("plist", new StoryValueFactoryHelper<CommonValues.PlistValue>());
            StoryValueManager.Instance.RegisterValueFactory("json2str", new StoryValueFactoryHelper<CommonValues.Json2StrValue>());
            StoryValueManager.Instance.RegisterValueFactory("str2json", new StoryValueFactoryHelper<CommonValues.Str2JsonValue>());
            StoryValueManager.Instance.RegisterValueFactory("jsonarray", new StoryValueFactoryHelper<CommonValues.JsonArrayValue>());
            StoryValueManager.Instance.RegisterValueFactory("jsonobject", new StoryValueFactoryHelper<CommonValues.JsonObjectValue>());
            StoryValueManager.Instance.RegisterValueFactory("jsonget", new StoryValueFactoryHelper<CommonValues.JsonGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("jsoncount", new StoryValueFactoryHelper<CommonValues.JsonCountValue>());
            StoryValueManager.Instance.RegisterValueFactory("jsonkeys", new StoryValueFactoryHelper<CommonValues.JsonKeysValue>());
            StoryValueManager.Instance.RegisterValueFactory("jsonvalues", new StoryValueFactoryHelper<CommonValues.JsonValuesValue>());
            StoryValueManager.Instance.RegisterValueFactory("isjsonarray", new StoryValueFactoryHelper<CommonValues.IsJsonArrayValue>());
            StoryValueManager.Instance.RegisterValueFactory("isjsonobject", new StoryValueFactoryHelper<CommonValues.IsJsonObjectValue>());

        }

        private Dictionary<string, IStoryCommandFactory> m_StoryCommandFactories = new Dictionary<string, IStoryCommandFactory>();
        private Dictionary<string, IStoryCommandFactory>[] m_GroupedCommandFactories = new Dictionary<string, IStoryCommandFactory>[c_MaxCommandGroupNum];

        public static ulong ThreadCommandGroupsMask
        {
            get { return s_ThreadCommandGroupsMask; }
            set { s_ThreadCommandGroupsMask = value; }
        }
        [ThreadStatic]
        private static ulong s_ThreadCommandGroupsMask = 0;

        public static StoryCommandManager Instance
        {
            get { return s_Instance; }
        }
        private static StoryCommandManager s_Instance = new StoryCommandManager();
    }
}
