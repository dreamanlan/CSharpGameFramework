using System;
using System.Collections.Generic;
using StorySystem;

namespace GameFramework
{
    internal sealed class ServerStorySystem
    {
        internal void Init(UserThread userThread)
        {
            StaticInit();
            m_UserThread = userThread;
        }
        public void Reset()
        {
            m_GlobalVariables.Clear();
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info) {
                    info.Reset();
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
            m_StoryLogicInfos.Clear();
        }
        public void LoadStory(string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(0, string.Empty, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AddStoryInstance(pair.Key, pair.Value.Clone());
                }
            }
        }
        public void LoadStory(string _namespace, string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AddStoryInstance(pair.Key, pair.Value.Clone());
                }
            }
        }
        public void ClearStoryInstancePool()
        {
            m_StoryInstancePool.Clear();
        }
        
        public int ActiveStoryCount
        {
            get
            {
                return m_StoryLogicInfos.Count;
            }
        }
        public StrBoxedValueDict GlobalVariables
        {
            get { return m_GlobalVariables; }
        }
        public StoryInstance GetStory(string storyId)
        {
            return GetStory(storyId, string.Empty);
        }
        public StoryInstance GetStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            return GetStoryInstance(storyId);
        }
        public void StartStories(string storyId)
        {
            StartStories(storyId, string.Empty);
        }
        public void StartStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            foreach(var pair in m_StoryInstancePool) {
                var info = pair.Value;
                if (IsMatch(info.StoryId, storyId)) {
                    StopStory(info.StoryId);
                    m_StoryLogicInfos.Add(info);
                    info.Context = m_UserThread;
                    info.GlobalVariables = m_GlobalVariables;
                    info.Start();

                    LogSystem.Info("StartStory {0}", info.StoryId);
                }
            }
        }
        public void PauseStories(string storyId, bool pause)
        {
            PauseStories(storyId, string.Empty, pause);
        }
        public void PauseStories(string storyId, string _namespace, bool pause)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.IsPaused = pause;
                }
            }
        }
        public void StopStories(string storyId)
        {
            StopStories(storyId, string.Empty);
        }
        public void StopStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        public int CountStories(string storyId)
        {
            return CountStories(storyId, string.Empty);
        }
        public int CountStories(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int ct = 0;
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info && IsMatch(info.StoryId, storyId) && !info.IsInTick) {
                    ++ct;
                }
            }
            return ct;
        }
        public void MarkStoriesTerminated(string storyId)
        {
            MarkStoriesTerminated(storyId, string.Empty);
        }
        public void MarkStoriesTerminated(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.IsTerminated = true;
                }
            }
        }
        public void SkipCurMessageHandlers(string endMsg, string storyId)
        {
            SkipCurMessageHandlers(endMsg, storyId, string.Empty);
        }
        public void SkipCurMessageHandlers(string endMsg, string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (IsMatch(info.StoryId, storyId)) {
                    info.ClearMessage();
                    var enumer = info.GetMessageHandlerEnumerator();
                    while (enumer.MoveNext()) {
                        var handler = enumer.Current;
                        if (handler.IsTriggered && handler.MessageId != endMsg) {
                            handler.CanSkip = true;
                        }
                    }
                    var cenumer = info.GetConcurrentMessageHandlerEnumerator();
                    while (cenumer.MoveNext()) {
                        var handler = cenumer.Current;
                        if (handler.IsTriggered && handler.MessageId != endMsg) {
                            handler.CanSkip = true;
                        }
                    }
                }
            }
        }
        public void StartStory(string storyId)
        {
            StartStory(storyId, string.Empty);
        }
        public void StartStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            StoryInstance inst = GetStoryInstance(storyId);
            if (null != inst) {
                StopStory(storyId);
                m_StoryLogicInfos.Add(inst);
                inst.Context = m_UserThread;
                inst.GlobalVariables = m_GlobalVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
            } else {
                LogSystem.Error("Can't find story, story:{0} !", storyId);
            }
        }
        public void PauseStory(string storyId, bool pause)
        {
            PauseStory(storyId, string.Empty, pause);
        }
        public void PauseStory(string storyId, string _namespace, bool pause)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    info.IsPaused = pause;
                }
            }
        }
        public void StopStory(string storyId)
        {
            StopStory(storyId, string.Empty);
        }
        public void StopStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        public int CountStory(string storyId)
        {
            return CountStory(storyId, string.Empty);
        }
        public int CountStory(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int ct = 0;
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info && info.StoryId == storyId && !info.IsInTick) {
                    ++ct;
                }
            }
            return ct;
        }
        public void MarkStoryTerminated(string storyId)
        {
            MarkStoryTerminated(storyId, string.Empty);
        }
        public void MarkStoryTerminated(string storyId, string _namespace)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    info.IsTerminated = true;
                }
            }
        }
        public void Tick()
        {
            long time = TimeUtility.GetLocalMilliseconds();
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.Tick(time);
                if (info.IsTerminated) {
                    m_StoryLogicInfos.RemoveAt(ix);
                }
            }
        }
        public BoxedValueList NewBoxedValueList()
        {
            var args = m_BoxedValueListPool.Alloc();
            args.Clear();
            return args;
        }
        public void SendMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendMessage(msgId, args);
        }
        public void SendMessage(string msgId, BoxedValueList args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                var newArgs = info.NewBoxedValueList();
                newArgs.AddRange(args);
                info.SendMessage(msgId, newArgs);
            }
            m_BoxedValueListPool.Recycle(args);
        }
        public void SendConcurrentMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendConcurrentMessage(msgId, args);
        }
        public void SendConcurrentMessage(string msgId, BoxedValueList args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                var newArgs = info.NewBoxedValueList();
                newArgs.AddRange(args);
                info.SendConcurrentMessage(msgId, newArgs);
            }
            m_BoxedValueListPool.Recycle(args);
        }
        public int CountMessage(string msgId)
        {
            int sum = 0;
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                sum += info.CountMessage(msgId);
            }
            return sum;
        }
        public void SuspendMessageHandler(string msgId, bool suspend)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SuspendMessageHandler(msgId, suspend);
            }
        }

        private void AddStoryInstance(string storyId, StoryInstance info)
        {
            if (!m_StoryInstancePool.ContainsKey(storyId)) {
                m_StoryInstancePool.Add(storyId, info);
            } else {
                m_StoryInstancePool[storyId] = info;
            }
        }
        private StoryInstance GetStoryInstance(string storyId)
        {
            StoryInstance info;
            m_StoryInstancePool.TryGetValue(storyId, out info);
            return info;
        }
        private bool IsMatch(string realId, string prefixId)
        {
            if (realId == prefixId || realId.Length > prefixId.Length && realId.StartsWith(prefixId) && realId[prefixId.Length] == ':')
                return true;
            return false;
        }

        internal ServerStorySystem() { }

        private StrBoxedValueDict m_GlobalVariables = new StrBoxedValueDict();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private UserThread m_UserThread = null;
        
        internal static void ThreadInitMask()
        {
            StoryCommandManager.ThreadCommandGroupsMask = (ulong)((1 << (int)StoryCommandGroupDefine.GM) + (1 << (int)StoryCommandGroupDefine.USER));
            StoryFunctionManager.ThreadFunctionGroupsMask = (ulong)((1 << (int)StoryFunctionGroupDefine.GM) + (1 << (int)StoryFunctionGroupDefine.USER));
        }
        internal static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;

                //注册剧情命令
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "startstory", "startstory command", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "stopstory", "stopstory command", new StoryCommandFactoryHelper<Story.Commands.StopStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "waitstory", "waitstory command", new StoryCommandFactoryHelper<Story.Commands.WaitStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "pausestory", "pausestory command", new StoryCommandFactoryHelper<Story.Commands.PauseStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "resumestory", "resumestory command", new StoryCommandFactoryHelper<Story.Commands.ResumeStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "firemessage", "firemessage command", new Story.Commands.FireMessageCommandFactory());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "fireconcurrentmessage", "fireconcurrentmessage command", new Story.Commands.FireConcurrentMessageCommandFactory());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "waitallmessage", "waitallmessage command", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "waitallmessagehandler", "waitallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "suspendallmessagehandler", "suspendallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "resumeallmessagehandler", "resumeallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendserverstorymessage", "sendserverstorymessage command", new StoryCommandFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendclientstorymessage", "sendclientstorymessage command", new StoryCommandFactoryHelper<Story.Commands.SendClientStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "publishgfxevent", "publishgfxevent command", new StoryCommandFactoryHelper<Story.Commands.PublishGfxEventCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendgfxmessage", "sendgfxmessage command", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendgfxmessagewithtag", "sendgfxmessagewithtag command", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());
                
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendmail", "sendmail command", new StoryCommandFactoryHelper<Story.Commands.SendMailCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "clearmembers", "clearmembers command", new StoryCommandFactoryHelper<Story.Commands.ClearMembersCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "addmember", "addmember command", new StoryCommandFactoryHelper<Story.Commands.AddMemberCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "removemember", "removemember command", new StoryCommandFactoryHelper<Story.Commands.RemoveMemberCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "syncmembers", "syncmembers command", new StoryCommandFactoryHelper<Story.Commands.SyncMembersCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "clearitems", "clearitems command", new StoryCommandFactoryHelper<Story.Commands.ClearItemsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "additem", "additem command", new StoryCommandFactoryHelper<Story.Commands.AddItemCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "reduceitem", "reduceitem command", new StoryCommandFactoryHelper<Story.Commands.ReduceItemCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "removeitem", "removeitem command", new StoryCommandFactoryHelper<Story.Commands.RemoveItemCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "syncitems", "syncitems command", new StoryCommandFactoryHelper<Story.Commands.SyncItemsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "clearuserdatas", "clearuserdatas command", new StoryCommandFactoryHelper<Story.Commands.ClearUserDatasCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "adduserdata", "adduserdata command", new StoryCommandFactoryHelper<Story.Commands.AddUserDataCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "removeuserdata", "removeuserdata command", new StoryCommandFactoryHelper<Story.Commands.RemoveUserDataCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "clearglobaldatas", "clearglobaldatas command", new StoryCommandFactoryHelper<Story.Commands.ClearGlobalDatasCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "addglobaldata", "addglobaldata command", new StoryCommandFactoryHelper<Story.Commands.AddGlobalDataCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "removeglobaldata", "removeglobaldata command", new StoryCommandFactoryHelper<Story.Commands.RemoveGlobalDataCommand>());

                //注册值与函数处理
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getuserinfo", "getuserinfo function", new StoryFunctionFactoryHelper<Story.Functions.GetUserInfoFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getmembercount", "getmembercount function", new StoryFunctionFactoryHelper<Story.Functions.GetMemberCountFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getmemberinfo", "getmemberinfo function", new StoryFunctionFactoryHelper<Story.Functions.GetMemberInfoFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getfriendcount", "getfriendcount function", new StoryFunctionFactoryHelper<Story.Functions.GetFriendCountFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getfriendinfo", "getfriendinfo function", new StoryFunctionFactoryHelper<Story.Functions.GetFriendInfoFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getitemcount", "getitemcount function", new StoryFunctionFactoryHelper<Story.Functions.GetItemCountFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getiteminfo", "getiteminfo function", new StoryFunctionFactoryHelper<Story.Functions.GetItemInfoFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "finditeminfo", "finditeminfo function", new StoryFunctionFactoryHelper<Story.Functions.FindItemInfoFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "calcitemnum", "calcitemnum function", new StoryFunctionFactoryHelper<Story.Functions.CalcItemNumFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getfreeitemcount", "getfreeitemcount function", new StoryFunctionFactoryHelper<Story.Functions.GetFreeItemCountFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getuserdata", "getuserdata function", new StoryFunctionFactoryHelper<Story.Functions.GetUserDataFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.USER, "getglobaldata", "getglobaldata function", new StoryFunctionFactoryHelper<Story.Functions.GetGlobalDataFunction>());

                LoadCustomCommandsAndFunctions();
            }
        }
        
        private static void LoadCustomCommandsAndFunctions()
        {
            string valFile = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + "Story/Common/CustomFunctions.dsl");
            string cmdFile = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + "Story/Common/CustomCommands.dsl");

            Dsl.DslFile file1 = CustomCommandFunctionParser.LoadStory(valFile);
            Dsl.DslFile file2 = CustomCommandFunctionParser.LoadStory(cmdFile);
            CustomCommandFunctionParser.FirstParse(file1, file2);
            CustomCommandFunctionParser.FinalParse(file1, file2);
        }
        private static bool s_IsInited = false;
    }
}
