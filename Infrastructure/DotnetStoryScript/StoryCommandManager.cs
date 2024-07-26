using System;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript
{
    /// <summary>
    /// This class does not lock, constraint:
    /// all commands registration must be completed when the program starts.
    /// </summary>
    public sealed class StoryCommandManager
    {
        public delegate bool CreateFailbackDelegation(Dsl.ISyntaxComponent comp, out IStoryCommand expression);
        public CreateFailbackDelegation OnCreateFailback;
        public int AllocLocalInfoIndex()
        {
            int index = m_NextLocalInfoIndex;
            System.Threading.Interlocked.Increment(ref m_NextLocalInfoIndex);
            return index;
        }
        public void RegisterCommandFactory(string name, string doc, IStoryCommandFactory factory)
        {
            RegisterCommandFactory(name, doc, factory, false);
        }
        public void RegisterCommandFactory(string name, string doc, IStoryCommandFactory factory, bool replace)
        {
            lock (m_Lock) {
                if (!m_StoryCommandFactories.ContainsKey(name)) {
                    m_StoryCommandFactories.Add(name, factory);
                }
                else if (replace) {
                    m_StoryCommandFactories[name] = factory;
                }
                else {
                    //ignore or warning
                }
                if (!m_CommandDocs.ContainsKey(name)) {
                    m_CommandDocs.Add(name, doc);
                }
                else if (replace) {
                    m_CommandDocs[name] = doc;
                }
                else {
                    //ignore or warning
                }
            }
        }
        public void RegisterCommandFactory(StoryCommandGroupDefine group, string name, string doc, IStoryCommandFactory factory)
        {
            RegisterCommandFactory(group, name, doc, factory, false);
        }
        public void RegisterCommandFactory(StoryCommandGroupDefine group, string name, string doc, IStoryCommandFactory factory, bool replace)
        {
            lock (m_Lock) {
                int ix = (int)group;
                if (ix >= 0 && ix < c_MaxCommandGroupNum) {
                    Dictionary<string, IStoryCommandFactory> factories = m_GroupedCommandFactories[ix];
                    if (!factories.ContainsKey(name)) {
                        factories.Add(name, factory);
                    }
                    else if (replace) {
                        factories[name] = factory;
                    }
                    else {
                        //ignore or warning
                    }
                    SortedList<string, string> docs = m_GroupedCommandDocs[ix];
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
        public SortedList<string, string> GenCommandDocs()
        {
            SortedList<string, string> docs;
            lock (m_Lock) {
                docs = new SortedList<string, string>(m_CommandDocs);
                const ulong c_one = 1;
                for (int ix = 0; ix < c_MaxCommandGroupNum; ++ix) {
                    if ((s_ThreadCommandGroupsMask & (c_one << ix)) != 0) {
                        foreach (var pair in m_GroupedCommandDocs[ix]) {
                            if (!docs.ContainsKey(pair.Key)) {
                                docs.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                }
            }
            return docs;
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
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET) {
                            if (callData.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PARENTHESIS) {
                                //obj.member(a,b,...) or obj[member](a,b,...) -> execinstance(obj,member,a,b,...)
                                Dsl.FunctionData newCall = new Dsl.FunctionData();
                                if(innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_PERIOD)
                                    newCall.Name = new Dsl.ValueData("dotnetcall", Dsl.ValueData.ID_TOKEN);
                                else
                                    newCall.Name = new Dsl.ValueData("collectioncall", Dsl.ValueData.ID_TOKEN);
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
                            innerCall.GetParamClass() == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_BRACKET)) {
                            //obj.property = val  or obj[property] = val -> setinstance(obj,property,val)
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
                                LogSystem.Error("[LoadStory] command:{0}[{1}] line:{2} failed.", type, commandConfig.ToScriptString(false), commandConfig.GetLine());
                            }
                        }
                        catch (Exception ex) {
                            var msg = string.Format("[LoadStory] command:{0}[{1}] line:{2} failed.", type, commandConfig.ToScriptString(false), commandConfig.GetLine());
                            throw new Exception(msg, ex);
                        }
                    }
                    else if (null == OnCreateFailback || !OnCreateFailback(commandConfig, out command)) {
#if DEBUG
                        string err = string.Format("[LoadStory] CreateCommand failed, line:{0} command:{1}[{2}]", commandConfig.GetLine(), type, commandConfig.ToScriptString(false));
                        LogSystem.Error("{0}", err);
                        throw new Exception(err);
#else
                LogSystem.Error("[LoadStory] CreateCommand failed, type:{0} line:{1}", type, commandConfig.GetLine());
#endif
                    }
                    if (null != command) {
                        //LogSystem.Debug("[LoadStory] CreateCommand, type:{0} command:{1}", type, command.GetType().Name);
                    }
                    else {
#if DEBUG
                        string err = string.Format("[LoadStory] CreateCommand failed, line:{0} command:{1}[{2}]", commandConfig.GetLine(), type, commandConfig.ToScriptString(false));
                        LogSystem.Error("{0}", err);
                        throw new Exception(err);
#else
                LogSystem.Error("[LoadStory] CreateCommand failed, type:{0} line:{1}", type, commandConfig.GetLine());
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
                    const ulong c_one = 1;
                    for (int ix = 0; ix < c_MaxCommandGroupNum; ++ix) {
                        if ((s_ThreadCommandGroupsMask & (c_one << ix)) != 0 && m_GroupedCommandFactories[ix].TryGetValue(type, out factory)) {
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
                m_GroupedCommandDocs[i] = new SortedList<string, string>();
            }
            //register common command
            RegisterCommandFactory("=", "assignment operator", new StoryCommandFactoryHelper<CommonCommands.AssignCommand>());
            RegisterCommandFactory("assign", "assign(var, val) command", new StoryCommandFactoryHelper<CommonCommands.AssignCommand>());
            RegisterCommandFactory("inc", "inc(var, val) command", new StoryCommandFactoryHelper<CommonCommands.IncCommand>());
            RegisterCommandFactory("dec", "dec(var, val) command", new StoryCommandFactoryHelper<CommonCommands.DecCommand>());
            RegisterCommandFactory("propset", "propset(name, val) command", new StoryCommandFactoryHelper<CommonCommands.PropSetCommand>());
            RegisterCommandFactory("foreach", "foreach statement", new StoryCommandFactoryHelper<CommonCommands.ForeachCommand>());
            RegisterCommandFactory("looplist", "looplist statement", new StoryCommandFactoryHelper<CommonCommands.LoopListCommand>());
            RegisterCommandFactory("loop", "loop statement", new StoryCommandFactoryHelper<CommonCommands.LoopCommand>());
            RegisterCommandFactory("wait", "wait(ms) command", new StoryCommandFactoryHelper<CommonCommands.SleepCommand>());
            RegisterCommandFactory("sleep", "sleep(ms) command", new StoryCommandFactoryHelper<CommonCommands.SleepCommand>());
            RegisterCommandFactory("realtimewait", "realtimewait(ms) command", new StoryCommandFactoryHelper<CommonCommands.RealTimeSleepCommand>());
            RegisterCommandFactory("realtimesleep", "realtimesleep(ms) command", new StoryCommandFactoryHelper<CommonCommands.RealTimeSleepCommand>());
            RegisterCommandFactory("storywait", "storywait(ms) command", new StoryCommandFactoryHelper<CommonCommands.StorySleepCommand>());
            RegisterCommandFactory("storysleep", "storysleep(ms) command", new StoryCommandFactoryHelper<CommonCommands.StorySleepCommand>());
            RegisterCommandFactory("storyrealtimewait", "storyrealtimewait(ms) command", new StoryCommandFactoryHelper<CommonCommands.StoryRealTimeSleepCommand>());
            RegisterCommandFactory("storyrealtimesleep", "storyrealtimesleep(ms) command", new StoryCommandFactoryHelper<CommonCommands.StoryRealTimeSleepCommand>());
            RegisterCommandFactory("storybreak", "storybreak command", new StoryCommandFactoryHelper<CommonCommands.StoryBreakCommand>());
            RegisterCommandFactory("break", "break command", new StoryCommandFactoryHelper<CommonCommands.BreakCommand>());
            RegisterCommandFactory("continue", "continue command", new StoryCommandFactoryHelper<CommonCommands.ContinueCommand>());
            RegisterCommandFactory("return", "return command", new StoryCommandFactoryHelper<CommonCommands.ReturnCommand>());
            RegisterCommandFactory("suspend", "suspend command", new StoryCommandFactoryHelper<CommonCommands.SuspendCommand>());
            RegisterCommandFactory("terminate", "terminate command", new StoryCommandFactoryHelper<CommonCommands.TerminateCommand>());
            RegisterCommandFactory("pause", "pause command", new StoryCommandFactoryHelper<CommonCommands.PauseCommand>());
            RegisterCommandFactory("localmessage", "localmessage(msgid,arg1,arg2,...) command", new CommonCommands.LocalMessageCommandFactory());
            RegisterCommandFactory("localconcurrentmessage", "localconcurrentmessage(msgid,arg1,arg2,...) command", new CommonCommands.LocalConcurrentMessageCommandFactory());
            RegisterCommandFactory("storylocalmessage", "storylocalmessage(msgid,arg1,arg2,...) command", new CommonCommands.StoryLocalMessageCommandFactory());
            RegisterCommandFactory("storylocalconcurrentmessage", "storylocalconcurrentmessage(msgid,arg1,arg2,...) command", new CommonCommands.StoryLocalConcurrentMessageCommandFactory());
            RegisterCommandFactory("clearmessage", "clearmessage(msgid1,msgid2,...) command", new StoryCommandFactoryHelper<CommonCommands.ClearMessageCommand>());
            RegisterCommandFactory("waitlocalmessage", "waitlocalmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.WaitLocalMessageCommand>());
            RegisterCommandFactory("waitlocalmessagehandler", "waitlocalmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.WaitLocalMessageHandlerCommand>());
            RegisterCommandFactory("storywaitlocalmessage", "storywaitlocalmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalMessageCommand>());
            RegisterCommandFactory("storywaitlocalmessagehandler", "storywaitlocalmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalMessageHandlerCommand>());
            RegisterCommandFactory("suspendlocalmessagehandler", "suspendlocalmessagehandler(msgid1,msgid2,...) command", new StoryCommandFactoryHelper<CommonCommands.SuspendLocalMessageHandlerCommand>());
            RegisterCommandFactory("resumelocalmessagehandler", "resumelocalmessagehandler(msgid1,msgid2,...) command", new StoryCommandFactoryHelper<CommonCommands.ResumeLocalMessageHandlerCommand>());
            RegisterCommandFactory("localnamespacedmessage", "localnamespacedmessage(msgid,arg1,arg2,...) command", new CommonCommands.LocalNamespacedMessageCommandFactory());
            RegisterCommandFactory("localconcurrentnamespacedmessage", "localconcurrentnamespacedmessage(msgid,arg1,arg2,...) command", new CommonCommands.LocalConcurrentNamespacedMessageCommandFactory());
            RegisterCommandFactory("storylocalnamespacedmessage", "storylocalnamespacedmessage(msgid,arg1,arg2,...) command", new CommonCommands.StoryLocalNamespacedMessageCommandFactory());
            RegisterCommandFactory("storylocalconcurrentnamespacedmessage", "storylocalconcurrentnamespacedmessage(msgid,arg1,arg2,...) command", new CommonCommands.StoryLocalConcurrentNamespacedMessageCommandFactory());
            RegisterCommandFactory("clearnamespacedmessage", "clearnamespacedmessage(msgid1,msgid2,...) command", new StoryCommandFactoryHelper<CommonCommands.ClearNamespacedMessageCommand>());
            RegisterCommandFactory("waitlocalnamespacedmessage", "waitlocalnamespacedmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.WaitLocalNamespacedMessageCommand>());
            RegisterCommandFactory("waitlocalnamespacedmessagehandler", "waitlocalnamespacedmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.WaitLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("storywaitlocalnamespacedmessage", "storywaitlocalnamespacedmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalNamespacedMessageCommand>());
            RegisterCommandFactory("storywaitlocalnamespacedmessagehandler", "storywaitlocalnamespacedmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)] command", new StoryCommandFactoryHelper<CommonCommands.StoryWaitLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("suspendlocalnamespacedmessagehandler", "suspendlocalnamespacedmessagehandler(msgid1,msgid2,...) command", new StoryCommandFactoryHelper<CommonCommands.SuspendLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("resumelocalnamespacedmessagehandler", "resumelocalnamespacedmessagehandler(msgid1,msgid2,...) command", new StoryCommandFactoryHelper<CommonCommands.ResumeLocalNamespacedMessageHandlerCommand>());
            RegisterCommandFactory("while", "while statement", new StoryCommandFactoryHelper<CommonCommands.WhileCommand>());
            RegisterCommandFactory("if", "if statement", new StoryCommandFactoryHelper<CommonCommands.IfElseCommand>());
            RegisterCommandFactory("log", "log command", new StoryCommandFactoryHelper<CommonCommands.LogCommand>());
            RegisterCommandFactory("listset", "listset(list,index,value) command", new StoryCommandFactoryHelper<CommonCommands.ListSetCommand>());
            RegisterCommandFactory("listadd", "listadd(list,value) command", new StoryCommandFactoryHelper<CommonCommands.ListAddCommand>());
            RegisterCommandFactory("listremove", "listremove(list,value) command", new StoryCommandFactoryHelper<CommonCommands.ListRemoveCommand>());
            RegisterCommandFactory("listinsert", "listinsert(list,index,value) command", new StoryCommandFactoryHelper<CommonCommands.ListInsertCommand>());
            RegisterCommandFactory("listremoveat", "listremoveat(list,index) command", new StoryCommandFactoryHelper<CommonCommands.ListRemoveAtCommand>());
            RegisterCommandFactory("listclear", "listclear(list) command", new StoryCommandFactoryHelper<CommonCommands.ListClearCommand>());
            RegisterCommandFactory("dotnetcall", "dotnetcall command", new StoryCommandFactoryHelper<CommonCommands.DotnetCallCommand>());
            RegisterCommandFactory("dotnetset", "dotnetset command", new StoryCommandFactoryHelper<CommonCommands.DotnetSetCommand>());
            RegisterCommandFactory("collectioncall", "collectioncall command", new StoryCommandFactoryHelper<CommonCommands.CollectionCallCommand>());
            RegisterCommandFactory("collectionset", "collectionset command", new StoryCommandFactoryHelper<CommonCommands.CollectionSetCommand>());
            RegisterCommandFactory("appendformat", "appendformat(sb,fmt,arg1,arg2,...) command", new StoryCommandFactoryHelper<CommonCommands.AppendFormatCommand>());
            RegisterCommandFactory("appendlineformat", "appendformatline(sb,fmt,arg1,arg2,...) command", new StoryCommandFactoryHelper<CommonCommands.AppendFormatLineCommand>());
            RegisterCommandFactory("writealllines", "writealllines(file,lines) command", new StoryCommandFactoryHelper<CommonCommands.WriteAllLinesCommand>());
            RegisterCommandFactory("writefile", "writefile(file,txt) command", new StoryCommandFactoryHelper<CommonCommands.WriteFileCommand>());
            RegisterCommandFactory("hashtableadd", "hashtableadd(hashtable,key,val) command", new StoryCommandFactoryHelper<CommonCommands.HashtableAddCommand>());
            RegisterCommandFactory("hashtableset", "hashtableset(hashtable,key,val) command", new StoryCommandFactoryHelper<CommonCommands.HashtableSetCommand>());
            RegisterCommandFactory("hashtableremove", "hashtableremove(hashtable,key) command", new StoryCommandFactoryHelper<CommonCommands.HashtableRemoveCommand>());
            RegisterCommandFactory("hashtableclear", "hashtableclear(hashtable) command", new StoryCommandFactoryHelper<CommonCommands.HashtableClearCommand>());
            RegisterCommandFactory("substcmd", "substcmd(id,substId) command", new StoryCommandFactoryHelper<CommonCommands.SubstCmdCommand>());
            RegisterCommandFactory("clearcmdsubsts", "clearcmdsubsts() command", new StoryCommandFactoryHelper<CommonCommands.ClearCmdSubstsCommand>());
            RegisterCommandFactory("substfunc", "substfunc(id,substId) command", new StoryCommandFactoryHelper<CommonCommands.SubstFuncCommand>());
            RegisterCommandFactory("clearfuncsubsts", "clearfuncsubsts() command", new StoryCommandFactoryHelper<CommonCommands.ClearFuncSubstsCommand>());
            //register value or internal function
            //object
            StoryFunctionManager.Instance.RegisterFunctionFactory("eval", "eval(exp1,exp2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.EvalFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("namespace", "namespace() function", new StoryFunctionFactoryHelper<CommonFunctions.NamespaceFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("storyid", "storyid() function", new StoryFunctionFactoryHelper<CommonFunctions.StoryIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("messageid", "messageid() function", new StoryFunctionFactoryHelper<CommonFunctions.MessageIdFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("countcommand", "countcommand(level) function", new StoryFunctionFactoryHelper<CommonFunctions.CountCommandFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("counthandlercommand", "counthandlercommand() function", new StoryFunctionFactoryHelper<CommonFunctions.CountHandlerCommandFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("propget", "propget(name[,defval] function", new StoryFunctionFactoryHelper<CommonFunctions.PropGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector2", "vector2(x,y) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector2Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector3", "vector3(x,y,z) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector3Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector4", "vector4(x,y,z,w) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector4Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("quaternion", "quaternion(x,y,z,w) function", new StoryFunctionFactoryHelper<CommonFunctions.QuaternionFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("eular", "eular(x,y,z) function", new StoryFunctionFactoryHelper<CommonFunctions.EularFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("color", "color(r,g,b,a) function", new StoryFunctionFactoryHelper<CommonFunctions.ColorFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("color32", "color32(r,g,b,a) function", new StoryFunctionFactoryHelper<CommonFunctions.Color32Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector2int", "vector2int(x,y) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector2IntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector3int", "vector3int(x,y,z) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector3IntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringlist", "stringlist(str_split_by_sep) function", new StoryFunctionFactoryHelper<CommonFunctions.StringListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("intlist", "intlist(str_split_by_sep) function", new StoryFunctionFactoryHelper<CommonFunctions.IntListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("floatlist", "floatlist(str_split_by_sep) function", new StoryFunctionFactoryHelper<CommonFunctions.FloatListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector2list", "vector2list(str_split_by_sep) function, vector2 per 2 elements", new StoryFunctionFactoryHelper<CommonFunctions.Vector2ListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector3list", "vector3list(str_split_by_sep) function, vector3 per 3 elements", new StoryFunctionFactoryHelper<CommonFunctions.Vector3ListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("array", "array(v1,v2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.ArrayFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("toarray", "toarray(list) function", new StoryFunctionFactoryHelper<CommonFunctions.ToArrayFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("list", "list(v1,v2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.ListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("rndfromlist", "rndfromlist(list[,defval]) function", new StoryFunctionFactoryHelper<CommonFunctions.RandomFromListFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("listget", "listget(list,index[,defval]) function", new StoryFunctionFactoryHelper<CommonFunctions.ListGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("listsize", "listsize(list) function", new StoryFunctionFactoryHelper<CommonFunctions.ListSizeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("listindexof", "listindexof(list,val) function", new StoryFunctionFactoryHelper<CommonFunctions.ListIndexOfFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector2dist", "vector2dist(pt1,pt2) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector2DistanceFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector3dist", "vector3dist(pt1,pt2) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector3DistanceFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector2to3", "vector2to3(pt) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector2To3Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("vector3to2", "vector3to2(pt) function", new StoryFunctionFactoryHelper<CommonFunctions.Vector3To2Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("rndvector3", "rndvector3(pt,radius) function", new StoryFunctionFactoryHelper<CommonFunctions.RandVector3Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("rndvector2", "rndvector2(pt,radius) function", new StoryFunctionFactoryHelper<CommonFunctions.RandVector2Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("+", "add operator", new StoryFunctionFactoryHelper<CommonFunctions.AddOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("-", "sub operator", new StoryFunctionFactoryHelper<CommonFunctions.SubOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("*", "mul operator", new StoryFunctionFactoryHelper<CommonFunctions.MulOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("/", "div operator", new StoryFunctionFactoryHelper<CommonFunctions.DivOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("%", "mod operator", new StoryFunctionFactoryHelper<CommonFunctions.ModOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("&", "bitand operator", new StoryFunctionFactoryHelper<CommonFunctions.BitAndOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("|", "bitor operator", new StoryFunctionFactoryHelper<CommonFunctions.BitOrOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("^", "bitxor operator", new StoryFunctionFactoryHelper<CommonFunctions.BitXorOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("~", "bitnot operator", new StoryFunctionFactoryHelper<CommonFunctions.BitNotOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("<<", "left shift operator", new StoryFunctionFactoryHelper<CommonFunctions.LShiftOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(">>", "right shift operator", new StoryFunctionFactoryHelper<CommonFunctions.RShiftOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("max", "max(v1,v2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.MaxFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("min", "min(v1,v2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.MinFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("abs", "abs(val) function", new StoryFunctionFactoryHelper<CommonFunctions.AbsFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("sin", "sin(val) function", new StoryFunctionFactoryHelper<CommonFunctions.SinFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("cos", "cos(val) function", new StoryFunctionFactoryHelper<CommonFunctions.CosFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("tan", "tan(v) api", new StoryFunctionFactoryHelper<CommonFunctions.TanFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("asin", "asin(v) api", new StoryFunctionFactoryHelper<CommonFunctions.AsinFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("acos", "acos(v) api", new StoryFunctionFactoryHelper<CommonFunctions.AcosFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("atan", "atan(v) api", new StoryFunctionFactoryHelper<CommonFunctions.AtanFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("atan2", "atan2(v1,v2) api", new StoryFunctionFactoryHelper<CommonFunctions.Atan2Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("sinh", "sinh(val) function", new StoryFunctionFactoryHelper<CommonFunctions.SinhFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("cosh", "cosh(val) function", new StoryFunctionFactoryHelper<CommonFunctions.CoshFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("tanh", "tanh(v) api", new StoryFunctionFactoryHelper<CommonFunctions.TanhFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("rndint", "rndint(min,max) function", new StoryFunctionFactoryHelper<CommonFunctions.RandomIntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("rndfloat", "rndfloat() function", new StoryFunctionFactoryHelper<CommonFunctions.RandomFloatFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("pow", "pow(x) or pow(x,y) function, pow(x) = exp(x)", new StoryFunctionFactoryHelper<CommonFunctions.PowFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("sqrt", "sqrt(val) function", new StoryFunctionFactoryHelper<CommonFunctions.SqrtFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("exp", "exp(v) api", new StoryFunctionFactoryHelper<CommonFunctions.ExpFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("exp2", "exp2(v) api", new StoryFunctionFactoryHelper<CommonFunctions.Exp2Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("log", "log(x) or log(x,y) function", new StoryFunctionFactoryHelper<CommonFunctions.LogFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("log2", "log2(v) api", new StoryFunctionFactoryHelper<CommonFunctions.Log2Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("log10", "log10(v) api", new StoryFunctionFactoryHelper<CommonFunctions.Log10Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("floor", "floor(val) function", new StoryFunctionFactoryHelper<CommonFunctions.FloorFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ceiling", "ceiling(val) function", new StoryFunctionFactoryHelper<CommonFunctions.CeilingFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("round", "round(val) function", new StoryFunctionFactoryHelper<CommonFunctions.RoundFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("floortoint", "floortoint(v) api", new StoryFunctionFactoryHelper<CommonFunctions.FloorToIntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ceilingtoint", "ceilingtoint(v) api", new StoryFunctionFactoryHelper<CommonFunctions.CeilingToIntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("roundtoint", "roundtoint(v) api", new StoryFunctionFactoryHelper<CommonFunctions.RoundToIntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("bool", "bool(v) api", new StoryFunctionFactoryHelper<CommonFunctions.BoolFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("sbyte", "sbyte(v) api", new StoryFunctionFactoryHelper<CommonFunctions.SByteFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("byte", "byte(v) api", new StoryFunctionFactoryHelper<CommonFunctions.ByteFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("char", "char(v) api", new StoryFunctionFactoryHelper<CommonFunctions.CharFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("short", "short(v) api", new StoryFunctionFactoryHelper<CommonFunctions.ShortFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ushort", "ushort(v) api", new StoryFunctionFactoryHelper<CommonFunctions.UShortFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("int", "int(v) api", new StoryFunctionFactoryHelper<CommonFunctions.IntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("uint", "uint(v) api", new StoryFunctionFactoryHelper<CommonFunctions.UIntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("long", "long(v) api", new StoryFunctionFactoryHelper<CommonFunctions.LongFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ulong", "ulong(v) api", new StoryFunctionFactoryHelper<CommonFunctions.ULongFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("float", "float(v) api", new StoryFunctionFactoryHelper<CommonFunctions.FloatFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("double", "double(v) api", new StoryFunctionFactoryHelper<CommonFunctions.DoubleFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("decimal", "decimal(v) api", new StoryFunctionFactoryHelper<CommonFunctions.DecimalFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ftoi", "ftoi(v) api", new StoryFunctionFactoryHelper<CommonFunctions.FtoiFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("itof", "itof(v) api", new StoryFunctionFactoryHelper<CommonFunctions.ItofFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ftou", "ftou(v) api", new StoryFunctionFactoryHelper<CommonFunctions.FtouFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("utof", "utof(v) api", new StoryFunctionFactoryHelper<CommonFunctions.UtofFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("dtol", "dtol(v) api", new StoryFunctionFactoryHelper<CommonFunctions.DtolFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ltod", "ltod(v) api", new StoryFunctionFactoryHelper<CommonFunctions.LtodFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("dtou", "dtou(v) api", new StoryFunctionFactoryHelper<CommonFunctions.DtouFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("utod", "utod(v) api", new StoryFunctionFactoryHelper<CommonFunctions.UtodFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("lerp", "lerp(a,b,t) api", new StoryFunctionFactoryHelper<CommonFunctions.LerpFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("lerpunclamped", "lerpunclamped(a,b,t) api", new StoryFunctionFactoryHelper<CommonFunctions.LerpUnclampedFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("lerpangle", "lerpangle(a,b,t) api", new StoryFunctionFactoryHelper<CommonFunctions.LerpAngleFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("smoothstep", "smoothstep(from,to,t) api", new StoryFunctionFactoryHelper<CommonFunctions.SmoothStepFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("clamp01", "clamp01(v) api", new StoryFunctionFactoryHelper<CommonFunctions.Clamp01Function>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("clamp", "clamp(v,v1,v2) api", new StoryFunctionFactoryHelper<CommonFunctions.ClampFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("approximately", "approximately(v1,v2) api", new StoryFunctionFactoryHelper<CommonFunctions.ApproximatelyFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("ispoweroftwo", "ispoweroftwo(v) api", new StoryFunctionFactoryHelper<CommonFunctions.IsPowerOfTwoFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("closestpoweroftwo", "closestpoweroftwo(v) api", new StoryFunctionFactoryHelper<CommonFunctions.ClosestPowerOfTwoFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("nextpoweroftwo", "nextpoweroftwo(v) api", new StoryFunctionFactoryHelper<CommonFunctions.NextPowerOfTwoFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("dist", "dist(x1,y1,x2,y2) api", new StoryFunctionFactoryHelper<CommonFunctions.DistFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("distsqr", "distsqr(x1,y1,x2,y2) api", new StoryFunctionFactoryHelper<CommonFunctions.DistSqrFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(">", "great operator", new StoryFunctionFactoryHelper<CommonFunctions.GreaterThanOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory(">=", "great equal operator", new StoryFunctionFactoryHelper<CommonFunctions.GreaterEqualThanOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("==", "equal operator", new StoryFunctionFactoryHelper<CommonFunctions.EqualOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("!=", "not equal operator", new StoryFunctionFactoryHelper<CommonFunctions.NotEqualOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("<", "less operator", new StoryFunctionFactoryHelper<CommonFunctions.LessThanOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("<=", "less equal operator", new StoryFunctionFactoryHelper<CommonFunctions.LessEqualThanOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("&&", "and operator", new StoryFunctionFactoryHelper<CommonFunctions.AndOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("||", "or operator", new StoryFunctionFactoryHelper<CommonFunctions.OrOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("!", "not operator", new StoryFunctionFactoryHelper<CommonFunctions.NotOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("?", "conditional expression", new StoryFunctionFactoryHelper<CommonFunctions.ConditionalOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("format", "format(fmt[,arg1,...]) function", new StoryFunctionFactoryHelper<CommonFunctions.FormatFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("gettypeassemblyname", "gettypeassemblyname(obj) api", new StoryFunctionFactoryHelper<CommonFunctions.GetTypeAssemblyNameFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("gettypefullname", "gettypefullname(obj) api", new StoryFunctionFactoryHelper<CommonFunctions.GetTypeFullNameFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("gettypename", "gettypename(obj) api", new StoryFunctionFactoryHelper<CommonFunctions.GetTypeNameFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("gettype", "gettype(type_name_str) function", new StoryFunctionFactoryHelper<CommonFunctions.GetTypeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("dotnetcall", "dotnetcall function", new StoryFunctionFactoryHelper<CommonFunctions.DotnetCallFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("dotnetget", "dotnetget function", new StoryFunctionFactoryHelper<CommonFunctions.DotnetGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("collectioncall", "collectioncall function", new StoryFunctionFactoryHelper<CommonFunctions.CollectionCallFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("collectionget", "collectionget function", new StoryFunctionFactoryHelper<CommonFunctions.CollectionGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("changetype", "changetype(obj,type_obj_or_str) function", new StoryFunctionFactoryHelper<CommonFunctions.ChangeTypeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("parseenum", "parseenum(type_obj_or_str,enum_val) function", new StoryFunctionFactoryHelper<CommonFunctions.ParseEnumFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("linq", "linq(obj,method,arg1,arg2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.LinqFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("isnull", "isnull(obj) function", new StoryFunctionFactoryHelper<CommonFunctions.IsNullOperator>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("substring", "substring(str[,start,len]) function", new StoryFunctionFactoryHelper<CommonFunctions.SubstringFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("newstringbuilder", "newstringbuilder() api", new StoryFunctionFactoryHelper<CommonFunctions.NewStringBuilderFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringbuilder_tostring", "stringbuilder_tostring(sb)", new StoryFunctionFactoryHelper<CommonFunctions.StringBuilderToStringFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringjoin", "stringjoin(sep,list) api", new StoryFunctionFactoryHelper<CommonFunctions.StringJoinFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringsplit", "stringsplit(str,sep_list) api", new StoryFunctionFactoryHelper<CommonFunctions.StringSplitFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringtrim", "stringtrim(str) api", new StoryFunctionFactoryHelper<CommonFunctions.StringTrimFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringtrimstart", "stringtrimstart(str) api", new StoryFunctionFactoryHelper<CommonFunctions.StringTrimStartFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringtrimend", "stringtrimend(str) api", new StoryFunctionFactoryHelper<CommonFunctions.StringTrimEndFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringtolower", "stringtolower(str) function", new StoryFunctionFactoryHelper<CommonFunctions.StringToLowerFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringtoupper", "stringtoupper(str) function", new StoryFunctionFactoryHelper<CommonFunctions.StringToUpperFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2lower", "str2lower(str) function, use cache", new StoryFunctionFactoryHelper<CommonFunctions.Str2LowerFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2upper", "str2upper(str) function, use cache", new StoryFunctionFactoryHelper<CommonFunctions.Str2UpperFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringreplace", "stringreplace(str,key,rep_str) api", new StoryFunctionFactoryHelper<CommonFunctions.StringReplaceFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringreplacechar", "stringreplacechar(str,key,char_as_str) api", new StoryFunctionFactoryHelper<CommonFunctions.StringReplaceCharFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("makestring", "makestring(char1_as_str_or_int,char2_as_str_or_int,...) api", new StoryFunctionFactoryHelper<CommonFunctions.MakeStringFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringcontains", "stringcontains(str,str1,str2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.StringContainsFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringnotcontains", "stringnotcontains(str,str1,str2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.StringNotContainsFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringcontainsany", "stringcontainsany(str,str1,str2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.StringContainsAnyFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("stringnotcontainsany", "stringnotcontainsany(str,str1,str2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.StringNotContainsAnyFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2int", "str2int(str) function", new StoryFunctionFactoryHelper<CommonFunctions.Str2IntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2uint", "str2uint(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Str2UintFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2long", "str2long(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Str2LongFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2ulong", "str2ulong(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Str2UlongFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2float", "str2float(str) function", new StoryFunctionFactoryHelper<CommonFunctions.Str2FloatFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("str2double", "str2double(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Str2DoubleFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hex2int", "hex2int(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Hex2IntFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hex2uint", "hex2uint(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Hex2UintFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hex2long", "hex2long(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Hex2LongFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hex2ulong", "hex2ulong(str) api", new StoryFunctionFactoryHelper<CommonFunctions.Hex2UlongFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("datetimestr", "datetimestr(fmt) api", new StoryFunctionFactoryHelper<CommonFunctions.DatetimeStrFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("longdatestr", "longdatestr() api", new StoryFunctionFactoryHelper<CommonFunctions.LongDateStrFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("longtimestr", "longtimestr() api", new StoryFunctionFactoryHelper<CommonFunctions.LongTimeStrFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("shortdatestr", "shortdatestr() api", new StoryFunctionFactoryHelper<CommonFunctions.ShortDateStrFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("shorttimestr", "shorttimestr() api", new StoryFunctionFactoryHelper<CommonFunctions.ShortTimeStrFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("isnullorempty", "isnullorempty(str) api", new StoryFunctionFactoryHelper<CommonFunctions.IsNullOrEmptyFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("time", "time() function", new StoryFunctionFactoryHelper<CommonFunctions.TimeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("readalllines", "readalllines function", new StoryFunctionFactoryHelper<CommonFunctions.ReadAllLinesFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("readfile", "readfile(file) function", new StoryFunctionFactoryHelper<CommonFunctions.ReadFileFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("tojson", "tojson(obj) function", new StoryFunctionFactoryHelper<CommonFunctions.ToJsonFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("fromjson", "fromjson(json_str) function", new StoryFunctionFactoryHelper<CommonFunctions.FromJsonFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hashtable", "hashtable(k1=>v1,k2=>v2,...) function", new StoryFunctionFactoryHelper<CommonFunctions.HashtableFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hashtableget", "hashtableget(hash_obj,key[,defval]) function", new StoryFunctionFactoryHelper<CommonFunctions.HashtableGetFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hashtablesize", "hashtablesize(hash_obj) function", new StoryFunctionFactoryHelper<CommonFunctions.HashtableSizeFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hashtablekeys", "hashtablekeys(hash_obj) function", new StoryFunctionFactoryHelper<CommonFunctions.HashtableKeysFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("hashtablevalues", "hashtablevalues(hash_obj) function", new StoryFunctionFactoryHelper<CommonFunctions.HashtableValuesFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("getcmdsubst", "getcmdsubst(id) function", new StoryFunctionFactoryHelper<CommonFunctions.GetCmdSubstFunction>());
            StoryFunctionManager.Instance.RegisterFunctionFactory("getfuncsubst", "getfuncsubst(id) function", new StoryFunctionFactoryHelper<CommonFunctions.GetFuncSubstFunction>());
        }

        private object m_Lock = new object();
        private Dictionary<string, IStoryCommandFactory> m_StoryCommandFactories = new Dictionary<string, IStoryCommandFactory>();
        private SortedList<string, string> m_CommandDocs = new SortedList<string, string>();
        private Dictionary<string, IStoryCommandFactory>[] m_GroupedCommandFactories = new Dictionary<string, IStoryCommandFactory>[c_MaxCommandGroupNum];
        private SortedList<string, string>[] m_GroupedCommandDocs = new SortedList<string, string>[c_MaxCommandGroupNum];
        private Dictionary<string, string> m_Substitutes = new Dictionary<string, string>();
        private int m_NextLocalInfoIndex = 0;

        public const int c_MaxCommandGroupNum = (int)StoryCommandGroupDefine.NUM;
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
                || paramClass == (int)Dsl.FunctionData.ParamClassEnum.PARAM_CLASS_POINTER)) {
                pvd.SetType(Dsl.ValueData.STRING_TOKEN);
                return pvd;
            }
            else {
                return p;
            }
        }
    }
}
