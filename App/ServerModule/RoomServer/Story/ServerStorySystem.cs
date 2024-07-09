using System;
using System.Collections.Generic;
using DotnetStoryScript;

namespace ScriptableFramework
{
    public sealed class ServerStorySystem
    {
        public void Init(Scene scene)
        {
            StaticInit();
            m_Scene = scene;
        }
        public void Reset()
        {
            LoadCustomCommandsAndFunctions();

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
        public void LoadSceneStories()
        {
            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(m_Scene.SceneResId);
            if (null != cfg) {
                string[] filePath;
                int ct1 = cfg.SceneDslFile.Count;
                int ct2 = cfg.RoomDslFile.Count;
                filePath = new string[ct1 + ct2];
                for (int i = 0; i < ct1; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + cfg.SceneDslFile[i]);
                }
                for (int i = 0; i < ct2; i++) {
                    filePath[ct1 + i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + cfg.RoomDslFile[i]);
                }
                StoryConfigManager.Instance.LoadStories(m_Scene.SceneResId, string.Empty, filePath);
                Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(m_Scene.SceneResId);
                if (null != stories) {
                    foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                        AddStoryInstance(pair.Key, pair.Value.Clone());
                    }
                }
            }
        }
        public void LoadStory(string _namespace, string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(m_Scene.SceneResId, _namespace, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AddStoryInstance(pair.Key, pair.Value.Clone());
                }
            }
        }
        public void LoadAiStory(string _namespace, string file)
        {
            string filePath = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + file);
            StoryConfigManager.Instance.LoadStories(m_Scene.SceneResId, _namespace, filePath);
            Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath);
            if (null != stories) {
                foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                    AiStoryInstanceInfo info = new AiStoryInstanceInfo();
                    info.m_StoryInstance = pair.Value.Clone();
                    info.m_IsUsed = false;
                    AddAiStoryInstanceInfoToPool(pair.Key, info);
                }
            }
        }
        public void ClearStoryInstancePool()
        {
            m_StoryInstancePool.Clear();
            m_AiStoryInstancePool.Clear();
        }

        public AiStoryInstanceInfo NewAiStoryInstance(string storyId, string _namespace, params string[] aiFiles)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            AiStoryInstanceInfo instInfo = GetUnusedAiStoryInstanceInfoFromPool(storyId);
            if (null == instInfo) {
                int ct;
                string[] filePath;
                ct = aiFiles.Length;
                filePath = new string[ct];
                for (int i = 0; i < ct; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + aiFiles[i]);
                }
                StoryConfigManager.Instance.LoadStories(m_Scene.SceneResId, _namespace, filePath);
                StoryInstance instance = StoryConfigManager.Instance.NewStoryInstance(storyId, m_Scene.SceneResId);
                if (instance == null) {
                    LogSystem.Error("Can't load story config, story:{0} scene:{1} !", storyId, m_Scene.SceneResId);
                    return null;
                }
                for (int ix = 0; ix < filePath.Length; ++ix) {
                    Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath[ix]);
                    if (null != stories) {
                        foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                            if (pair.Key != storyId) {
                                AiStoryInstanceInfo info = new AiStoryInstanceInfo();
                                info.m_StoryInstance = pair.Value.Clone();
                                info.m_IsUsed = false;
                                AddAiStoryInstanceInfoToPool(pair.Key, info);
                            }
                        }
                    }
                }
                AiStoryInstanceInfo res = new AiStoryInstanceInfo();
                res.m_StoryInstance = instance;
                res.m_IsUsed = true;

                AddAiStoryInstanceInfoToPool(storyId, res);
                return res;
            } else {
                instInfo.m_IsUsed = true;
                return instInfo;
            }
        }
        public void RecycleAiStoryInstance(AiStoryInstanceInfo info)
        {
            info.m_StoryInstance.Reset();
            info.m_IsUsed = false;
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
                    info.Context = m_Scene;
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
                inst.Context = m_Scene;
                inst.GlobalVariables = m_GlobalVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
            } else {
                LogSystem.Error("Can't find story, story:{0} scene:{1} !", storyId, m_Scene.SceneResId);
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
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        sum += infos[ix].m_StoryInstance.CountMessage(msgId);
                    }
                }
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
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        infos[ix].m_StoryInstance.SuspendMessageHandler(msgId, suspend);
                    }
                }
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
        private void AddAiStoryInstanceInfoToPool(string storyId, AiStoryInstanceInfo info)
        {
            List<AiStoryInstanceInfo> infos;
            if (m_AiStoryInstancePool.TryGetValue(storyId, out infos)) {
                infos.Add(info);
            } else {
                infos = new List<AiStoryInstanceInfo>();
                infos.Add(info);
                m_AiStoryInstancePool.Add(storyId, infos);
            }
        }
        private AiStoryInstanceInfo GetUnusedAiStoryInstanceInfoFromPool(string storyId)
        {
            AiStoryInstanceInfo info = null;
            List<AiStoryInstanceInfo> infos;
            if (m_AiStoryInstancePool.TryGetValue(storyId, out infos)) {
                int ct = infos.Count;
                for (int ix = 0; ix < ct; ++ix) {
                    if (!infos[ix].m_IsUsed) {
                        info = infos[ix];
                        break;
                    }
                }
            }
            return info;
        }
        private bool IsMatch(string realId, string prefixId)
        {
            if (realId == prefixId || realId.Length > prefixId.Length && realId.StartsWith(prefixId) && realId[prefixId.Length] == ':')
                return true;
            return false;
        }

        public ServerStorySystem() { }

        private StrBoxedValueDict m_GlobalVariables = new StrBoxedValueDict();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private Dictionary<string, List<AiStoryInstanceInfo>> m_AiStoryInstancePool = new Dictionary<string, List<AiStoryInstanceInfo>>();
        private Scene m_Scene = null;

        public static void ThreadInitMask()
        {
            StoryCommandManager.ThreadCommandGroupsMask = (ulong)((1 << (int)StoryCommandGroupDefine.GM) + (1 << (int)StoryCommandGroupDefine.GFX));
            StoryFunctionManager.ThreadFunctionGroupsMask = (ulong)((1 << (int)StoryFunctionGroupDefine.GM) + (1 << (int)StoryFunctionGroupDefine.GFX));
        }
        public static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;

                //register story commands
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "startstory", "startstory command", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "stopstory", "stopstory command", new StoryCommandFactoryHelper<Story.Commands.StopStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitstory", "waitstory command", new StoryCommandFactoryHelper<Story.Commands.WaitStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "pausestory", "pausestory command", new StoryCommandFactoryHelper<Story.Commands.PauseStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "resumestory", "resumestory command", new StoryCommandFactoryHelper<Story.Commands.ResumeStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "firemessage", "firemessage command", new Story.Commands.FireMessageCommandFactory());
            	StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "fireconcurrentmessage", "fireconcurrentmessage command", new Story.Commands.FireConcurrentMessageCommandFactory());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitallmessage", "waitallmessage command", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitallmessagehandler", "waitallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "suspendallmessagehandler", "suspendallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "resumeallmessagehandler", "resumeallmessagehandler command", new StoryCommandFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendserverstorymessage", "sendserverstorymessage command", new StoryCommandFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendclientstorymessage", "sendclientstorymessage command", new StoryCommandFactoryHelper<Story.Commands.SendClientStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "publishgfxevent", "publishgfxevent command", new StoryCommandFactoryHelper<Story.Commands.PublishGfxEventCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendgfxmessage", "sendgfxmessage command", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendgfxmessagewithtag", "sendgfxmessagewithtag command", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "activescene", "activescene command", new StoryCommandFactoryHelper<Story.Commands.ActiveSceneCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "changescene", "changescene command", new StoryCommandFactoryHelper<Story.Commands.ChangeSceneCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "changeroomscene", "changeroomscene command", new StoryCommandFactoryHelper<Story.Commands.ChangeRoomSceneCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "createscenelogic", "createscenelogic command", new StoryCommandFactoryHelper<Story.Commands.CreateSceneLogicCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroyscenelogic", "destroyscenelogic command", new StoryCommandFactoryHelper<Story.Commands.DestroySceneLogicCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "pausescenelogic", "pausescenelogic command", new StoryCommandFactoryHelper<Story.Commands.PauseSceneLogicCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "restarttimeout", "restarttimeout command", new StoryCommandFactoryHelper<Story.Commands.RestartTimeoutCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "highlightprompt", "highlightprompt command", new StoryCommandFactoryHelper<Story.Commands.HighlightPromptCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "blackboardset", "blackboardset command", new StoryCommandFactoryHelper<Story.Commands.BlackboardSetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "blackboardclear", "blackboardclear command", new StoryCommandFactoryHelper<Story.Commands.BlackboardClearCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollow", "camerafollow command", new StoryCommandFactoryHelper<Story.Commands.CameraFollowCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollowrange", "camerafollowrange command", new StoryCommandFactoryHelper<Story.Commands.CameraFollowRangeCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollowpath", "camerafollowpath command", new StoryCommandFactoryHelper<Story.Commands.CameraFollowPathCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralook", "cameralook command", new StoryCommandFactoryHelper<Story.Commands.CameraLookCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "lockframe", "lockframe command", new StoryCommandFactoryHelper<Story.Commands.LockFrameCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "showdlg", "showdlg command", new StoryCommandFactoryHelper<Story.Commands.ShowDlgCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "areadetect", "areadetect command", new StoryCommandFactoryHelper<Story.Commands.AreaDetectCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "createnpc", "createnpc command", new StoryCommandFactoryHelper<Story.Commands.CreateNpcCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroynpc", "destroynpc command", new StoryCommandFactoryHelper<Story.Commands.DestroyNpcCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroynpcwithobjid", "destroynpcwithobjid command", new StoryCommandFactoryHelper<Story.Commands.DestroyNpcWithObjIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcface", "npcface command", new StoryCommandFactoryHelper<Story.Commands.NpcFaceCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcmove", "npcmove command", new StoryCommandFactoryHelper<Story.Commands.NpcMoveCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcmovewithwaypoints", "npcmovewithwaypoints command", new StoryCommandFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcstop", "npcstop command", new StoryCommandFactoryHelper<Story.Commands.NpcStopCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcattack", "npcattack command", new StoryCommandFactoryHelper<Story.Commands.NpcAttackCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetformation", "npcsetformation command", new StoryCommandFactoryHelper<Story.Commands.NpcSetFormationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcenableai", "npcenableai command", new StoryCommandFactoryHelper<Story.Commands.NpcEnableAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetai", "npcsetai command", new StoryCommandFactoryHelper<Story.Commands.NpcSetAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetaitarget", "npcsetaitarget command", new StoryCommandFactoryHelper<Story.Commands.NpcSetAiTargetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcanimation", "npcanimation command", new StoryCommandFactoryHelper<Story.Commands.NpcAnimationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcaddimpact", "npcaddimpact command", new StoryCommandFactoryHelper<Story.Commands.NpcAddImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcremoveimpact", "npcremoveimpact command", new StoryCommandFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npccastskill", "npccastskill command", new StoryCommandFactoryHelper<Story.Commands.NpcCastSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcstopskill", "npcstopskill command", new StoryCommandFactoryHelper<Story.Commands.NpcStopSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcaddskill", "npcaddskill command", new StoryCommandFactoryHelper<Story.Commands.NpcAddSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcremoveskill", "npcremoveskill command", new StoryCommandFactoryHelper<Story.Commands.NpcRemoveSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npclisten", "npclisten command", new StoryCommandFactoryHelper<Story.Commands.NpcListenCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetcamp", "npcsetcamp command", new StoryCommandFactoryHelper<Story.Commands.NpcSetCampCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetsummonerid", "npcsetsummonerid command", new StoryCommandFactoryHelper<Story.Commands.NpcSetSummonerIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetsummonskillid", "npcsetsummonskillid command", new StoryCommandFactoryHelper<Story.Commands.NpcSetSummonSkillIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objface", "objface command", new StoryCommandFactoryHelper<Story.Commands.ObjFaceCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objmove", "objmove command", new StoryCommandFactoryHelper<Story.Commands.ObjMoveCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objmovewithwaypoints", "objmovewithwaypoints command", new StoryCommandFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objstop", "objstop command", new StoryCommandFactoryHelper<Story.Commands.ObjStopCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objattack", "objattack command", new StoryCommandFactoryHelper<Story.Commands.ObjAttackCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetformation", "objsetformation command", new StoryCommandFactoryHelper<Story.Commands.ObjSetFormationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objenableai", "objenableai command", new StoryCommandFactoryHelper<Story.Commands.ObjEnableAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetai", "objsetai command", new StoryCommandFactoryHelper<Story.Commands.ObjSetAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetaitarget", "objsetaitarget command", new StoryCommandFactoryHelper<Story.Commands.ObjSetAiTargetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objanimation", "objanimation command", new StoryCommandFactoryHelper<Story.Commands.ObjAnimationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objaddimpact", "objaddimpact command", new StoryCommandFactoryHelper<Story.Commands.ObjAddImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objremoveimpact", "objremoveimpact command", new StoryCommandFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objcastskill", "objcastskill command", new StoryCommandFactoryHelper<Story.Commands.ObjCastSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objstopskill", "objstopskill command", new StoryCommandFactoryHelper<Story.Commands.ObjStopSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objaddskill", "objaddskill command", new StoryCommandFactoryHelper<Story.Commands.ObjAddSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objremoveskill", "objremoveskill command", new StoryCommandFactoryHelper<Story.Commands.ObjRemoveSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objlisten", "objlisten command", new StoryCommandFactoryHelper<Story.Commands.ObjListenCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetcamp", "objsetcamp command", new StoryCommandFactoryHelper<Story.Commands.ObjSetCampCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetsummonerid", "objsetsummonerid command", new StoryCommandFactoryHelper<Story.Commands.ObjSetSummonerIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetsummonskillid", "objsetsummonskillid command", new StoryCommandFactoryHelper<Story.Commands.ObjSetSummonSkillIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setvisible", "setvisible command", new StoryCommandFactoryHelper<DotnetStoryScript.CommonCommands.DummyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sethp", "sethp command", new StoryCommandFactoryHelper<Story.Commands.SetHpCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setenergy", "setenergy command", new StoryCommandFactoryHelper<Story.Commands.SetEnergyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objset", "objset command", new StoryCommandFactoryHelper<Story.Commands.ObjSetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setlevel", "setlevel command", new StoryCommandFactoryHelper<Story.Commands.SetLevelCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setattr", "setattr command", new StoryCommandFactoryHelper<Story.Commands.SetAttrCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setunitid", "setunitid command", new StoryCommandFactoryHelper<Story.Commands.SetUnitIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setleaderid", "setleaderid command", new StoryCommandFactoryHelper<Story.Commands.SetLeaderIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "markcontrolbystory", "markcontrolbystory command", new StoryCommandFactoryHelper<Story.Commands.MarkControlByStoryCommand>());

                //register value or functions
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gettime", "gettime function", new StoryFunctionFactoryHelper<Story.Functions.GetTimeFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gettimescale", "gettimescale function", new StoryFunctionFactoryHelper<Story.Functions.GetTimeScaleFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getentityinfo", "getentityinfo function", new StoryFunctionFactoryHelper<Story.Functions.GetEntityInfoFunction>());

                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isclient", "isclient function", new StoryFunctionFactoryHelper<Story.Functions.IsClientFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getroomid", "getroomid function", new StoryFunctionFactoryHelper<Story.Functions.GetRoomIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getsceneid", "getsceneid function", new StoryFunctionFactoryHelper<Story.Functions.GetSceneIdFunction>());

                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "blackboardget", "blackboardget function", new StoryFunctionFactoryHelper<Story.Functions.BlackboardGetFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getdialogitem", "getdialogitem function", new StoryFunctionFactoryHelper<Story.Functions.GetDialogItemFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmonsterinfo", "getmonsterinfo function", new StoryFunctionFactoryHelper<Story.Functions.GetMonsterInfoFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getaidata", "getaidata function", new StoryFunctionFactoryHelper<Story.Functions.GetAiDataFunction>());

                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcidlist", "npcidlist function", new StoryFunctionFactoryHelper<Story.Functions.NpcIdListFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "combatnpccount", "combatnpccount function", new StoryFunctionFactoryHelper<Story.Functions.CombatNpcCountFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npccount", "npccount function", new StoryFunctionFactoryHelper<Story.Functions.NpcCountFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "unitid2objid", "unitid2objid function", new StoryFunctionFactoryHelper<Story.Functions.UnitId2ObjIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objid2unitid", "objid2unitid function", new StoryFunctionFactoryHelper<Story.Functions.ObjId2UnitIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "unitid2uniqueid", "unitid2uniqueid function", new StoryFunctionFactoryHelper<Story.Functions.UnitId2UniqueIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objid2uniqueid", "objid2uniqueid function", new StoryFunctionFactoryHelper<Story.Functions.ObjId2UniqueIdFunction>());

                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetformation", "npcgetformation function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetFormationFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetnpctype", "npcgetnpctype function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetNpcTypeFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetsummonerid", "npcgetsummonerid function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetSummonerIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcgetsummonskillid", "npcgetsummonskillid function", new StoryFunctionFactoryHelper<Story.Functions.NpcGetSummonSkillIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "npcfindimpactseqbyid", "npcfindimpactseqbyid function", new StoryFunctionFactoryHelper<Story.Functions.NpcFindImpactSeqByIdFunction>());

                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetformation", "objgetformation function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetFormationFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetnpctype", "objgetnpctype function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetNpcTypeFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetsummonerid", "objgetsummonerid function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetSummonerIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objfindimpactseqbyid", "objfindimpactseqbyid function", new StoryFunctionFactoryHelper<Story.Functions.ObjFindImpactSeqByIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objgetsummonskillid", "objgetsummonskillid function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetSummonSkillIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isenemy", "isenemy function", new StoryFunctionFactoryHelper<Story.Functions.IsEnemyFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isfriend", "isfriend function", new StoryFunctionFactoryHelper<Story.Functions.IsFriendFunction>());

                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getposition", "getposition function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getpositionx", "getpositionx function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionXFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getpositiony", "getpositiony function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionYFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getpositionz", "getpositionz function", new StoryFunctionFactoryHelper<Story.Functions.GetPositionZFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getcamp", "getcamp function", new StoryFunctionFactoryHelper<Story.Functions.GetCampFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "iscombatnpc", "iscombatnpc function", new StoryFunctionFactoryHelper<Story.Functions.IsCombatNpcFunction>());

                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gethp", "gethp function", new StoryFunctionFactoryHelper<Story.Functions.GetHpFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getenergy", "getenergy function", new StoryFunctionFactoryHelper<Story.Functions.GetEnergyFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmaxhp", "getmaxhp function", new StoryFunctionFactoryHelper<Story.Functions.GetMaxHpFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getmaxenergy", "getmaxenergy function", new StoryFunctionFactoryHelper<Story.Functions.GetMaxEnergyFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "calcoffset", "calcoffset function", new StoryFunctionFactoryHelper<Story.Functions.CalcOffsetFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "calcdir", "calcdir function", new StoryFunctionFactoryHelper<Story.Functions.CalcDirFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "objget", "objget function", new StoryFunctionFactoryHelper<Story.Functions.ObjGetFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "gettableid", "gettableid function", new StoryFunctionFactoryHelper<Story.Functions.GetTableIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getlevel", "getlevel function", new StoryFunctionFactoryHelper<Story.Functions.GetLevelFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getattr", "getattr function", new StoryFunctionFactoryHelper<Story.Functions.GetAttrFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "iscontrolbystory", "iscontrolbystory function", new StoryFunctionFactoryHelper<Story.Functions.IsControlByStoryFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "cancastskill", "cancastskill function", new StoryFunctionFactoryHelper<Story.Functions.CanCastSkillFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "isundercontrol", "isundercontrol function", new StoryFunctionFactoryHelper<Story.Functions.IsUnderControlFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getleaderid", "getleaderid function", new StoryFunctionFactoryHelper<Story.Functions.GetLeaderIdFunction>());
                StoryFunctionManager.Instance.RegisterFunctionFactory(StoryFunctionGroupDefine.GFX, "getleadertableid", "getleadertableid function", new StoryFunctionFactoryHelper<Story.Functions.GetLeaderTableIdFunction>());

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
