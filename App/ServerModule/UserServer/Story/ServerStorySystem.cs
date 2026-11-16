using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

namespace ScriptableFramework
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
            m_ContextVariables.Clear();
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
        public StrBoxedValueDict ContextVariables
        {
            get { return m_ContextVariables; }
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
                    info.ContextVariables = m_ContextVariables;
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
                inst.ContextVariables = m_ContextVariables;
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

        private StrBoxedValueDict m_ContextVariables = new StrBoxedValueDict();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private UserThread m_UserThread = null;
        
        internal static void ThreadInitMask()
        {
        }
        internal static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;

                var registry = DslCalculatorHost.GetSharedApiRegistry();

                //register story commands
                registry.Register("start_story", "start_story command", new ExpressionFactoryHelper<Story.Commands.StartStoryCommand>());
                registry.Register("stop_story", "stop_story command", new ExpressionFactoryHelper<Story.Commands.StopStoryCommand>());
                registry.Register("wait_story", "wait_story command", new ExpressionFactoryHelper<Story.Commands.WaitStoryCommand>());
                registry.Register("pause_story", "pause_story command", new ExpressionFactoryHelper<Story.Commands.PauseStoryCommand>());
                registry.Register("resume_story", "resume_story command", new ExpressionFactoryHelper<Story.Commands.ResumeStoryCommand>());
                registry.Register("fire_message", "fire_message command", new ExpressionFactoryHelper<Story.Commands.FireMessageCommand>());
                registry.Register("fire_concurrent_message", "fire_concurrent_message command", new ExpressionFactoryHelper<Story.Commands.FireConcurrentMessageCommand>());
                registry.Register("wait_all_message", "wait_all_message command", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageCommand>());
                registry.Register("wait_all_message_handler", "wait_all_message_handler command", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
                registry.Register("suspend_all_message_handler", "suspend_all_message_handler command", new ExpressionFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
                registry.Register("resume_all_message_handler", "resume_all_message_handler command", new ExpressionFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
                registry.Register("send_server_story_message", "send_server_story_message command", new ExpressionFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
                registry.Register("send_client_story_message", "send_client_story_message command", new ExpressionFactoryHelper<Story.Commands.SendClientStoryMessageCommand>());
                registry.Register("publish_gfx_event", "publish_gfx_event command", new ExpressionFactoryHelper<Story.Commands.PublishGfxEventCommand>());
                registry.Register("send_gfx_message", "send_gfx_message command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageCommand>());
                registry.Register("send_gfx_message_with_tag", "send_gfx_message_with_tag command", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());

                registry.Register("send_mail", "send_mail command", new ExpressionFactoryHelper<Story.Commands.SendMailCommand>());
                registry.Register("clear_members", "clear_members command", new ExpressionFactoryHelper<Story.Commands.ClearMembersCommand>());
                registry.Register("add_member", "add_member command", new ExpressionFactoryHelper<Story.Commands.AddMemberCommand>());
                registry.Register("remove_member", "remove_member command", new ExpressionFactoryHelper<Story.Commands.RemoveMemberCommand>());
                registry.Register("sync_members", "sync_members command", new ExpressionFactoryHelper<Story.Commands.SyncMembersCommand>());
                registry.Register("clear_items", "clear_items command", new ExpressionFactoryHelper<Story.Commands.ClearItemsCommand>());
                registry.Register("add_item", "add_item command", new ExpressionFactoryHelper<Story.Commands.AddItemCommand>());
                registry.Register("reduce_item", "reduce_item command", new ExpressionFactoryHelper<Story.Commands.ReduceItemCommand>());
                registry.Register("remove_item", "remove_item command", new ExpressionFactoryHelper<Story.Commands.RemoveItemCommand>());
                registry.Register("sync_items", "sync_items command", new ExpressionFactoryHelper<Story.Commands.SyncItemsCommand>());
                registry.Register("clear_user_datas", "clear_user_datas command", new ExpressionFactoryHelper<Story.Commands.ClearUserDatasCommand>());
                registry.Register("add_user_data", "add_user_data command", new ExpressionFactoryHelper<Story.Commands.AddUserDataCommand>());
                registry.Register("remove_user_data", "remove_user_data command", new ExpressionFactoryHelper<Story.Commands.RemoveUserDataCommand>());
                registry.Register("clear_global_datas", "clear_global_datas command", new ExpressionFactoryHelper<Story.Commands.ClearGlobalDatasCommand>());
                registry.Register("add_global_data", "add_global_data command", new ExpressionFactoryHelper<Story.Commands.AddGlobalDataCommand>());
                registry.Register("remove_global_data", "remove_global_data command", new ExpressionFactoryHelper<Story.Commands.RemoveGlobalDataCommand>());

                //register value or functions
                registry.Register("get_user_info", "get_user_info function", new ExpressionFactoryHelper<Story.Functions.GetUserInfoFunction>());
                registry.Register("get_member_count", "get_member_count function", new ExpressionFactoryHelper<Story.Functions.GetMemberCountFunction>());
                registry.Register("get_member_info", "get_member_info function", new ExpressionFactoryHelper<Story.Functions.GetMemberInfoFunction>());
                registry.Register("get_friend_count", "get_friend_count function", new ExpressionFactoryHelper<Story.Functions.GetFriendCountFunction>());
                registry.Register("get_friend_info", "get_friend_info function", new ExpressionFactoryHelper<Story.Functions.GetFriendInfoFunction>());
                registry.Register("get_item_count", "get_item_count function", new ExpressionFactoryHelper<Story.Functions.GetItemCountFunction>());
                registry.Register("get_item_info", "get_item_info function", new ExpressionFactoryHelper<Story.Functions.GetItemInfoFunction>());
                registry.Register("find_item_info", "find_item_info function", new ExpressionFactoryHelper<Story.Functions.FindItemInfoFunction>());
                registry.Register("calc_item_num", "calc_item_num function", new ExpressionFactoryHelper<Story.Functions.CalcItemNumFunction>());
                registry.Register("get_free_item_count", "get_free_item_count function", new ExpressionFactoryHelper<Story.Functions.GetFreeItemCountFunction>());
                registry.Register("get_user_data", "get_user_data function", new ExpressionFactoryHelper<Story.Functions.GetUserDataFunction>());
                registry.Register("get_global_data", "get_global_data function", new ExpressionFactoryHelper<Story.Functions.GetGlobalDataFunction>());
            }
        }
        private static bool s_IsInited = false;
    }
}
