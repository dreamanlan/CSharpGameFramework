using System;
using System.Collections.Generic;
using GameFramework;

namespace StorySystem
{
    /// <summary>
    /// 这个类不加锁，约束条件：所有命令注册必须在程序启动时完成。
    /// </summary>
    public sealed class StoryCommandManager
    {
        public const int c_MaxCommandGroupNum = (int)StoryCommandGroupDefine.NUM;
        public int AllocLocalInfoIndex()
        {
            int index = m_NextLocalInfoIndex;
            System.Threading.Interlocked.Increment(ref m_NextLocalInfoIndex);
            return index;
        }
        public void RegisterCommandFactory(string type, IStoryCommandFactory factory)
        {
            RegisterCommandFactory(type, factory, false);
        }
        public void RegisterCommandFactory(string type, IStoryCommandFactory factory, bool replace)
        {
            lock (m_Lock) {
                if (!m_StoryCommandFactories.ContainsKey(type)) {
                    m_StoryCommandFactories.Add(type, factory);
                }
                else if (replace) {
                    m_StoryCommandFactories[type] = factory;
                }
                else {
                    //error
                }
            }
        }
        public void RegisterCommandFactory(StoryCommandGroupDefine group, string type, IStoryCommandFactory factory)
        {
            RegisterCommandFactory(group, type, factory, false);
        }
        public void RegisterCommandFactory(StoryCommandGroupDefine group, string type, IStoryCommandFactory factory, bool replace)
        {
            lock (m_Lock) {
                int ix = (int)group;
                if (ix >= 0 && ix < c_MaxCommandGroupNum) {
                    Dictionary<string, IStoryCommandFactory> factories = m_GroupedCommandFactories[ix];
                    if (!factories.ContainsKey(type)) {
                        factories.Add(type, factory);
                    }
                    else if (replace) {
                        factories[type] = factory;
                    }
                    else {
                        //error
                    }
                }
            }
        }
        public IStoryCommandFactory FindFactory(string type)
        {
            IStoryCommandFactory factory;
            lock (m_Lock) {
                m_StoryCommandFactories.TryGetValue(type, out factory);
            }
            return factory;
        }
        public IStoryCommandFactory FindFactory(StoryCommandGroupDefine group, string type)
        {
            IStoryCommandFactory factory = null;
            lock (m_Lock) {
                int ix = (int)group;
                if (ix >= 0 && ix < c_MaxCommandGroupNum) {
                    Dictionary<string, IStoryCommandFactory> factories = m_GroupedCommandFactories[ix];
                    factories.TryGetValue(type, out factory);
                }
            }
            return factory;
        }
        public IStoryCommand CreateCommand(Dsl.ISyntaxComponent commandConfig)
        {
            IStoryCommand command = null;
            lock (m_Lock) {
                Dsl.StatementData statementData = commandConfig as Dsl.StatementData;
                if (null != statementData) {
                    Dsl.FunctionData func;
                    if (DslSyntaxTransformer.TryTransformCommandLineLikeSyntax(statementData, out func)) {
                        commandConfig = func;
                    }
                }
                Dsl.FunctionData callData = commandConfig as Dsl.FunctionData;
                if (null != callData) {
                    if (callData.IsHighOrder) {
                        Dsl.FunctionData innerCall = callData.LowerOrderFunction;
                        if (innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS) {
                            if (callData.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS) {
                                //obj.member(a,b,...) or obj[member](a,b,...) or obj.(member)(a,b,...) or obj.[member](a,b,...) or obj.{member}(a,b,...) -> execinstance(obj,member,a,b,...)
                                Dsl.FunctionData newCall = new Dsl.FunctionData();
                                if(innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD)
                                    newCall.Name = new Dsl.ValueData("dotnetexec", Dsl.ValueData.ID_TOKEN);
                                else
                                    newCall.Name = new Dsl.ValueData("collectionexec", Dsl.ValueData.ID_TOKEN);
                                newCall.SetParamClass((int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                                if (innerCall.IsHighOrder) {
                                    newCall.Params.Add(innerCall.LowerOrderFunction);
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
                                return CreateCommand(newCall);
                            }
                        }
                    }
                    else if (callData.GetId() == "=") {
                        Dsl.FunctionData innerCall = callData.GetParam(0) as Dsl.FunctionData;
                        if (null != innerCall && (innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACE ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD_BRACKET ||
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD_PARENTHESIS)) {
                            //obj.property = val  or obj[property] = val or obj.(property) = val or obj.[property] = val or obj.{property} = val -> setinstance(obj,property,val)
                            Dsl.FunctionData newCall = new Dsl.FunctionData();
                            if (innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD)
                                newCall.Name = new Dsl.ValueData("dotnetset", Dsl.ValueData.ID_TOKEN);
                            else
                                newCall.Name = new Dsl.ValueData("collectionset", Dsl.ValueData.ID_TOKEN);
                            newCall.SetParamClass((int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS);
                            if (innerCall.IsHighOrder) {
                                newCall.Params.Add(innerCall.LowerOrderFunction);
                                newCall.Params.Add(ObjectMemberConverter.Convert(innerCall.GetParam(0), innerCall.GetParamClass()));
                                newCall.Params.Add(callData.GetParam(1));
                            }
                            else {
                                newCall.Params.Add(innerCall.Name);
                                newCall.Params.Add(ObjectMemberConverter.Convert(innerCall.GetParam(0), innerCall.GetParamClass()));
                                newCall.Params.Add(callData.GetParam(1));
                            }
                            return CreateCommand(newCall);
                        }
                    }
                }
                string type = commandConfig.GetId();
                if (commandConfig.GetIdType() == Dsl.ValueData.ID_TOKEN && type != "true" && type != "false") {
                    IStoryCommandFactory factory = GetFactory(type);
                    if (null != factory) {
                        try {
                            command = factory.Create();
                            if (!command.Init(commandConfig)) {
                                GameFramework.LogSystem.Error("[LoadStory] command:{0}[{1}] line:{2} failed.", type, commandConfig.ToScriptString(false), commandConfig.GetLine());
                            }
                        }
                        catch (Exception ex) {
                            var msg = string.Format("[LoadStory] command:{0}[{1}] line:{2} failed.", type, commandConfig.ToScriptString(false), commandConfig.GetLine());
                            throw new Exception(msg, ex);
                        }
                    }
                    else {
#if DEBUG
                        string err = string.Format("[LoadStory] CreateCommand failed, line:{0} command:{1}[{2}]", commandConfig.GetLine(), type, commandConfig.ToScriptString(false));
                        GameFramework.LogSystem.Error("{0}", err);
                        throw new Exception(err);
#else
                GameFramework.LogSystem.Error("[LoadStory] CreateCommand failed, type:{0} line:{1}", type, commandConfig.GetLine());
#endif
                    }
                    if (null != command) {
                        //GameFramework.LogSystem.Debug("[LoadStory] CreateCommand, type:{0} command:{1}", type, command.GetType().Name);
                    }
                    else {
#if DEBUG
                        string err = string.Format("[LoadStory] CreateCommand failed, line:{0} command:{1}[{2}]", commandConfig.GetLine(), type, commandConfig.ToScriptString(false));
                        GameFramework.LogSystem.Error("{0}", err);
                        throw new Exception(err);
#else
                GameFramework.LogSystem.Error("[LoadStory] CreateCommand failed, type:{0} line:{1}", type, commandConfig.GetLine());
#endif
                    }
                }
            }
            return command;
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
        private IStoryCommandFactory GetFactory(string type)
        {
            IStoryCommandFactory factory;
            lock (m_Lock) {
                string substType;
                if (m_Substitutes.TryGetValue(type, out substType)) {
                    type = substType;
                }
                if (!m_StoryCommandFactories.TryGetValue(type, out factory)) {
                    const ulong one = 1;
                    for (int ix = 0; ix < c_MaxCommandGroupNum; ++ix) {
                        if ((s_ThreadCommandGroupsMask & (one << ix)) != 0 && m_GroupedCommandFactories[ix].TryGetValue(type, out factory)) {
                            break;
                        }
                    }
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
            RegisterCommandFactory("realtimewait", new StoryCommandFactoryHelper<CommonCommands.RealTimeSleepCommand>());
            RegisterCommandFactory("realtimesleep", new StoryCommandFactoryHelper<CommonCommands.RealTimeSleepCommand>());
            RegisterCommandFactory("storywait", new StoryCommandFactoryHelper<CommonCommands.StorySleepCommand>());
            RegisterCommandFactory("storysleep", new StoryCommandFactoryHelper<CommonCommands.StorySleepCommand>());
            RegisterCommandFactory("storyrealtimewait", new StoryCommandFactoryHelper<CommonCommands.StoryRealTimeSleepCommand>());
            RegisterCommandFactory("storyrealtimesleep", new StoryCommandFactoryHelper<CommonCommands.StoryRealTimeSleepCommand>());
            RegisterCommandFactory("storybreak", new StoryCommandFactoryHelper<CommonCommands.StoryBreakCommand>());
            RegisterCommandFactory("break", new StoryCommandFactoryHelper<CommonCommands.BreakCommand>());
            RegisterCommandFactory("continue", new StoryCommandFactoryHelper<CommonCommands.ContinueCommand>());
            RegisterCommandFactory("return", new StoryCommandFactoryHelper<CommonCommands.ReturnCommand>());
            RegisterCommandFactory("suspend", new StoryCommandFactoryHelper<CommonCommands.SuspendCommand>());
            RegisterCommandFactory("terminate", new StoryCommandFactoryHelper<CommonCommands.TerminateCommand>());
            RegisterCommandFactory("pause", new StoryCommandFactoryHelper<CommonCommands.PauseCommand>());
            RegisterCommandFactory("localmessage", new CommonCommands.LocalMessageCommandFactory());
            RegisterCommandFactory("localconcurrentmessage", new CommonCommands.LocalConcurrentMessageCommandFactory());
            RegisterCommandFactory("storylocalmessage", new CommonCommands.StoryLocalMessageCommandFactory());
            RegisterCommandFactory("storylocalconcurrentmessage", new CommonCommands.StoryLocalConcurrentMessageCommandFactory());
            RegisterCommandFactory("clearmessage", new StoryCommandFactoryHelper<CommonCommands.ClearMessageCommand>());
            RegisterCommandFactory("waitlocalmessage", new StoryCommandFactoryHelper<CommonCommands.WaitLocalMessageCommand>());
            RegisterCommandFactory("waitlocalmessagehandler", new StoryCommandFactoryHelper<CommonCommands.WaitLocalMessageHandlerCommand>());
            RegisterCommandFactory("storywaitlocalmessage", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalMessageCommand>());
            RegisterCommandFactory("storywaitlocalmessagehandler", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalMessageHandlerCommand>());
            RegisterCommandFactory("suspendlocalmessagehandler", new StoryCommandFactoryHelper<CommonCommands.SuspendLocalMessageHandlerCommand>());
            RegisterCommandFactory("resumelocalmessagehandler", new StoryCommandFactoryHelper<CommonCommands.ResumeLocalMessageHandlerCommand>());
            RegisterCommandFactory("localnamespacedmessage", new CommonCommands.LocalNamespacedMessageCommandFactory());
            RegisterCommandFactory("localconcurrentnamespacedmessage", new CommonCommands.LocalConcurrentNamespacedMessageCommandFactory());
            RegisterCommandFactory("storylocalnamespacedmessage", new CommonCommands.StoryLocalNamespacedMessageCommandFactory());
            RegisterCommandFactory("storylocalconcurrentnamespacedmessage", new CommonCommands.StoryLocalConcurrentNamespacedMessageCommandFactory());
            RegisterCommandFactory("clearnamespacedmessage", new StoryCommandFactoryHelper<CommonCommands.ClearNamespacedMessageCommand>());
            RegisterCommandFactory("waitlocalnamespacedmessage", new StoryCommandFactoryHelper<CommonCommands.WaitLocalNamespacedMessageCommand>());
            RegisterCommandFactory("waitlocalnamespacedmessagehandler", new StoryCommandFactoryHelper<CommonCommands.WaitLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("storywaitlocalnamespacedmessage", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalNamespacedMessageCommand>());
            RegisterCommandFactory("storywaitlocalnamespacedmessagehandler", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("suspendlocalnamespacedmessagehandler", new StoryCommandFactoryHelper<CommonCommands.SuspendLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("resumelocalnamespacedmessagehandler", new StoryCommandFactoryHelper<CommonCommands.ResumeLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("while", new StoryCommandFactoryHelper<CommonCommands.WhileCommand>());
            RegisterCommandFactory("if", new StoryCommandFactoryHelper<CommonCommands.IfElseCommand>());
            RegisterCommandFactory("log", new StoryCommandFactoryHelper<CommonCommands.LogCommand>());
            RegisterCommandFactory("listset", new StoryCommandFactoryHelper<CommonCommands.ListSetCommand>());
            RegisterCommandFactory("listadd", new StoryCommandFactoryHelper<CommonCommands.ListAddCommand>());
            RegisterCommandFactory("listremove", new StoryCommandFactoryHelper<CommonCommands.ListRemoveCommand>());
            RegisterCommandFactory("listinsert", new StoryCommandFactoryHelper<CommonCommands.ListInsertCommand>());
            RegisterCommandFactory("listremoveat", new StoryCommandFactoryHelper<CommonCommands.ListRemoveAtCommand>());
            RegisterCommandFactory("listclear", new StoryCommandFactoryHelper<CommonCommands.ListClearCommand>());
            RegisterCommandFactory("dotnetexec", new StoryCommandFactoryHelper<CommonCommands.DotnetExecCommand>());
            RegisterCommandFactory("dotnetset", new StoryCommandFactoryHelper<CommonCommands.DotnetSetCommand>());
            RegisterCommandFactory("collectionexec", new StoryCommandFactoryHelper<CommonCommands.CollectionExecCommand>());
            RegisterCommandFactory("collectionset", new StoryCommandFactoryHelper<CommonCommands.CollectionSetCommand>());
            RegisterCommandFactory("system", new StoryCommandFactoryHelper<CommonCommands.SystemCommand>());
            RegisterCommandFactory("writealllines", new StoryCommandFactoryHelper<CommonCommands.WriteAllLinesCommand>());
            RegisterCommandFactory("writefile", new StoryCommandFactoryHelper<CommonCommands.WriteFileCommand>());
            RegisterCommandFactory("hashtableadd", new StoryCommandFactoryHelper<CommonCommands.HashtableAddCommand>());
            RegisterCommandFactory("hashtableset", new StoryCommandFactoryHelper<CommonCommands.HashtableSetCommand>());
            RegisterCommandFactory("hashtableremove", new StoryCommandFactoryHelper<CommonCommands.HashtableRemoveCommand>());
            RegisterCommandFactory("hashtableclear", new StoryCommandFactoryHelper<CommonCommands.HashtableClearCommand>());
            RegisterCommandFactory("substcmd", new StoryCommandFactoryHelper<CommonCommands.SubstCmdCommand>());
            RegisterCommandFactory("clearcmdsubsts", new StoryCommandFactoryHelper<CommonCommands.ClearCmdSubstsCommand>());
            RegisterCommandFactory("substval", new StoryCommandFactoryHelper<CommonCommands.SubstValCommand>());
            RegisterCommandFactory("clearvalsubsts", new StoryCommandFactoryHelper<CommonCommands.ClearValSubstsCommand>());
            //注册通用值与内部函数
            //object
            StoryValueManager.Instance.RegisterValueFactory("eval", new StoryValueFactoryHelper<CommonValues.EvalValue>());
            StoryValueManager.Instance.RegisterValueFactory("namespace", new StoryValueFactoryHelper<CommonValues.NamespaceValue>());
            StoryValueManager.Instance.RegisterValueFactory("storyid", new StoryValueFactoryHelper<CommonValues.StoryIdValue>());
            StoryValueManager.Instance.RegisterValueFactory("messageid", new StoryValueFactoryHelper<CommonValues.MessageIdValue>());
            StoryValueManager.Instance.RegisterValueFactory("countcommand", new StoryValueFactoryHelper<CommonValues.CountCommandValue>());
            StoryValueManager.Instance.RegisterValueFactory("counthandlercommand", new StoryValueFactoryHelper<CommonValues.CountHandlerCommandValue>());
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
            StoryValueManager.Instance.RegisterValueFactory("array", new StoryValueFactoryHelper<CommonValues.ArrayValue>());
            StoryValueManager.Instance.RegisterValueFactory("toarray", new StoryValueFactoryHelper<CommonValues.ToArrayValue>());
            StoryValueManager.Instance.RegisterValueFactory("list", new StoryValueFactoryHelper<CommonValues.ListValue>());
            StoryValueManager.Instance.RegisterValueFactory("rndfromlist", new StoryValueFactoryHelper<CommonValues.RandomFromListValue>());
            StoryValueManager.Instance.RegisterValueFactory("listget", new StoryValueFactoryHelper<CommonValues.ListGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("listsize", new StoryValueFactoryHelper<CommonValues.ListSizeValue>());
            StoryValueManager.Instance.RegisterValueFactory("listindexof", new StoryValueFactoryHelper<CommonValues.ListIndexOfValue>());
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
            StoryValueManager.Instance.RegisterValueFactory("stringcontains", new StoryValueFactoryHelper<CommonValues.StringContainsValue>());
            StoryValueManager.Instance.RegisterValueFactory("stringnotcontains", new StoryValueFactoryHelper<CommonValues.StringNotContainsValue>());
            StoryValueManager.Instance.RegisterValueFactory("stringcontainsany", new StoryValueFactoryHelper<CommonValues.StringContainsAnyValue>());
            StoryValueManager.Instance.RegisterValueFactory("stringnotcontainsany", new StoryValueFactoryHelper<CommonValues.StringNotContainsAnyValue>());
            StoryValueManager.Instance.RegisterValueFactory("stringtolower", new StoryValueFactoryHelper<CommonValues.StringToLowerValue>());
            StoryValueManager.Instance.RegisterValueFactory("stringtoupper", new StoryValueFactoryHelper<CommonValues.StringToUpperValue>());
            StoryValueManager.Instance.RegisterValueFactory("str2lower", new StoryValueFactoryHelper<CommonValues.Str2LowerValue>());
            StoryValueManager.Instance.RegisterValueFactory("str2upper", new StoryValueFactoryHelper<CommonValues.Str2UpperValue>());
            StoryValueManager.Instance.RegisterValueFactory("str2int", new StoryValueFactoryHelper<CommonValues.Str2IntValue>());
            StoryValueManager.Instance.RegisterValueFactory("str2float", new StoryValueFactoryHelper<CommonValues.Str2FloatValue>());
            StoryValueManager.Instance.RegisterValueFactory("time", new StoryValueFactoryHelper<CommonValues.TimeValue>());
            StoryValueManager.Instance.RegisterValueFactory("isnull", new StoryValueFactoryHelper<CommonValues.IsNullOperator>());
            StoryValueManager.Instance.RegisterValueFactory("gettype", new StoryValueFactoryHelper<CommonValues.GetTypeValue>());
            StoryValueManager.Instance.RegisterValueFactory("dotnetcall", new StoryValueFactoryHelper<CommonValues.DotnetCallValue>());
            StoryValueManager.Instance.RegisterValueFactory("dotnetget", new StoryValueFactoryHelper<CommonValues.DotnetGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("collectioncall", new StoryValueFactoryHelper<CommonValues.CollectionCallValue>());
            StoryValueManager.Instance.RegisterValueFactory("collectionget", new StoryValueFactoryHelper<CommonValues.CollectionGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("changetype", new StoryValueFactoryHelper<CommonValues.ChangeTypeValue>());
            StoryValueManager.Instance.RegisterValueFactory("parseenum", new StoryValueFactoryHelper<CommonValues.ParseEnumValue>());
            StoryValueManager.Instance.RegisterValueFactory("pgrep", new StoryValueFactoryHelper<CommonValues.PgrepValue>());
            StoryValueManager.Instance.RegisterValueFactory("plist", new StoryValueFactoryHelper<CommonValues.PlistValue>());
            StoryValueManager.Instance.RegisterValueFactory("linq", new StoryValueFactoryHelper<CommonValues.LinqValue>());
            StoryValueManager.Instance.RegisterValueFactory("readalllines", new StoryValueFactoryHelper<CommonValues.ReadAllLinesValue>());
            StoryValueManager.Instance.RegisterValueFactory("readfile", new StoryValueFactoryHelper<CommonValues.ReadFileValue>());
            StoryValueManager.Instance.RegisterValueFactory("tojson", new StoryValueFactoryHelper<CommonValues.ToJsonValue>());
            StoryValueManager.Instance.RegisterValueFactory("fromjson", new StoryValueFactoryHelper<CommonValues.FromJsonValue>());
            StoryValueManager.Instance.RegisterValueFactory("hashtable", new StoryValueFactoryHelper<CommonValues.HashtableValue>());
            StoryValueManager.Instance.RegisterValueFactory("hashtableget", new StoryValueFactoryHelper<CommonValues.HashtableGetValue>());
            StoryValueManager.Instance.RegisterValueFactory("hashtablesize", new StoryValueFactoryHelper<CommonValues.HashtableSizeValue>());
            StoryValueManager.Instance.RegisterValueFactory("hashtablekeys", new StoryValueFactoryHelper<CommonValues.HashtableKeysValue>());
            StoryValueManager.Instance.RegisterValueFactory("hashtablevalues", new StoryValueFactoryHelper<CommonValues.HashtableValuesValue>());
            StoryValueManager.Instance.RegisterValueFactory("getcmdsubst", new StoryValueFactoryHelper<CommonValues.GetCmdSubstValue>());
            StoryValueManager.Instance.RegisterValueFactory("getvalsubst", new StoryValueFactoryHelper<CommonValues.GetValSubstValue>());
        }

        private object m_Lock = new object();
        private Dictionary<string, IStoryCommandFactory> m_StoryCommandFactories = new Dictionary<string, IStoryCommandFactory>();
        private Dictionary<string, IStoryCommandFactory>[] m_GroupedCommandFactories = new Dictionary<string, IStoryCommandFactory>[c_MaxCommandGroupNum];
        private Dictionary<string, string> m_Substitutes = new Dictionary<string, string>();
        private int m_NextLocalInfoIndex = 0;

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
    internal static class ObjectMemberConverter
    {
        internal static Dsl.ISyntaxComponent Convert(Dsl.ISyntaxComponent p, int paramClass)
        {
            var pvd = p as Dsl.ValueData;
            if (null != pvd && pvd.IsId() && (paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD
                || paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_POINTER
                || paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_QUESTION_PERIOD)) {
                pvd.SetType(Dsl.ValueData.STRING_TOKEN);
                return pvd;
            }
            else {
                return p;
            }
        }
    }
}
