using System;
using System.Collections.Generic;
using StorySystem;

namespace GameFramework
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
        public void PreloadSceneStories()
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
        public void PreloadNamespacedStory(string _namespace, string file)
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
        public void PreloadAiStory(string _namespace, string file)
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

        public AiStoryInstanceInfo NewAiStoryInstance(string storyId)
        {
            return NewAiStoryInstance(storyId, string.Empty);
        }
        public AiStoryInstanceInfo NewAiStoryInstance(string storyId, string _namespace, params string[] overloadFiles)
        {
            return NewAiStoryInstance(storyId, _namespace, true, overloadFiles);
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
        public Dictionary<string, object> GfxDatas
        {
            get { return m_GfxDatas; }
        }
        public Dictionary<string, object> GlobalVariables
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
        public void StartStory(string storyId)
        {
            StartStory(storyId, string.Empty);
        }
        public void StartStory(string storyId, string _namespace, params string[] overloadFiles)
        {
            StoryInstance inst = NewStoryInstance(storyId, _namespace, true, overloadFiles);
            if (null != inst) {
                m_StoryLogicInfos.Add(inst);
                inst.Context = m_Scene;
                inst.GlobalVariables = m_GlobalVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
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
        public void SendMessage(string msgId, params object[] args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SendMessage(msgId, args);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        infos[ix].m_StoryInstance.SendMessage(msgId, args);
                    }
                }
            }
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
        public void PauseMessageHandler(string msgId, bool pause)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.PauseMessageHandler(msgId, pause);
            }
            foreach (var pair in m_AiStoryInstancePool) {
                var infos = pair.Value;
                int aiCt = infos.Count;
                for (int ix = aiCt - 1; ix >= 0; --ix) {
                    if (infos[ix].m_IsUsed && null != infos[ix].m_StoryInstance) {
                        infos[ix].m_StoryInstance.PauseMessageHandler(msgId, pause);
                    }
                }
            }
        }

        private StoryInstance NewStoryInstance(string storyId, string _namespace, bool logIfNotFound, params string[] overloadFiles)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            StoryInstance instance = GetStoryInstance(storyId);
            if (null == instance) {
                TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(m_Scene.SceneResId);
                if (null != cfg) {
                    string[] filePath;
                    if (overloadFiles.Length <= 0) {
                        int ct1 = cfg.SceneDslFile.Count;
                        int ct2 = cfg.RoomDslFile.Count;
                        filePath = new string[ct1 + ct2];
                        for (int i = 0; i < ct1; i++) {
                            filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + cfg.SceneDslFile[i]);
                        }
                        for (int i = 0; i < ct2; i++) {
                            filePath[ct1 + i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + cfg.RoomDslFile[i]);
                        }
                    } else {
                        int ct = overloadFiles.Length;
                        filePath = new string[ct];
                        for (int i = 0; i < ct; i++) {
                            filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + overloadFiles[i]);
                        }
                    }
                    StoryConfigManager.Instance.LoadStories(m_Scene.SceneResId, _namespace, filePath);
                    instance = StoryConfigManager.Instance.NewStoryInstance(storyId, m_Scene.SceneResId);
                    if (instance == null) {
                        if (logIfNotFound)
                            LogSystem.Error("Can't load story config, story:{0} scene:{1} !", storyId, m_Scene.SceneResId);
                        return null;
                    }
                    for (int ix = 0; ix < filePath.Length; ++ix) {
                        Dictionary<string, StoryInstance> stories = StoryConfigManager.Instance.GetStories(filePath[ix]);
                        if (null != stories) {
                            foreach (KeyValuePair<string, StoryInstance> pair in stories) {
                                if (pair.Key != storyId)
                                    AddStoryInstance(pair.Key, pair.Value.Clone());
                            }
                        }
                    }
                    AddStoryInstance(storyId, instance);
                    return instance;
                } else {
                    if (logIfNotFound)
                        LogSystem.Error("Can't find story config, story:{0} scene:{1} !", storyId, m_Scene.SceneResId);
                    return null;
                }
            } else {
                return instance;
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

        private AiStoryInstanceInfo NewAiStoryInstance(string storyId, string _namespace, bool logIfNotFound, params string[] overloadFiles)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            AiStoryInstanceInfo instInfo = GetUnusedAiStoryInstanceInfoFromPool(storyId);
            if (null == instInfo) {
                int ct;
                string[] filePath;
                ct = overloadFiles.Length;
                filePath = new string[ct];
                for (int i = 0; i < ct; i++) {
                    filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + overloadFiles[i]);
                }
                StoryConfigManager.Instance.LoadStories(m_Scene.SceneResId, _namespace, filePath);
                StoryInstance instance = StoryConfigManager.Instance.NewStoryInstance(storyId, m_Scene.SceneResId);
                if (instance == null) {
                    if (logIfNotFound)
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

        public ServerStorySystem() { }

        private Dictionary<string, object> m_GfxDatas = new Dictionary<string, object>();
        private Dictionary<string, object> m_GlobalVariables = new Dictionary<string, object>();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private Dictionary<string, List<AiStoryInstanceInfo>> m_AiStoryInstancePool = new Dictionary<string, List<AiStoryInstanceInfo>>();
        private Scene m_Scene = null;

        public static void ThreadInitMask()
        {
            StoryCommandManager.ThreadCommandGroupsMask = (ulong)((1 << (int)StoryCommandGroupDefine.GM) + (1 << (int)StoryCommandGroupDefine.GFX));
            StoryValueManager.ThreadValueGroupsMask = (ulong)((1 << (int)StoryValueGroupDefine.GM) + (1 << (int)StoryValueGroupDefine.GFX));
        }
        public static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;

                //注册剧情命令
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "preload", new StoryCommandFactoryHelper<Story.Commands.PreloadCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "startstory", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "stopstory", new StoryCommandFactoryHelper<Story.Commands.StopStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitstory", new StoryCommandFactoryHelper<Story.Commands.WaitStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "pausestory", new StoryCommandFactoryHelper<Story.Commands.PauseStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "resumestory", new StoryCommandFactoryHelper<Story.Commands.ResumeStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "firemessage", new StoryCommandFactoryHelper<Story.Commands.FireMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitallmessage", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "waitallmessagehandler", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "pauseallmessagehandler", new StoryCommandFactoryHelper<Story.Commands.PauseAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "resumeallmessagehandler", new StoryCommandFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendroomstorymessage", new StoryCommandFactoryHelper<StorySystem.CommonCommands.DummyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendserverstorymessage", new StoryCommandFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendclientstorymessage", new StoryCommandFactoryHelper<Story.Commands.SendClientStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "publishgfxevent", new StoryCommandFactoryHelper<Story.Commands.PublishGfxEventCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendgfxmessage", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendgfxmessagewithtag", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sendskillmessage", new StoryCommandFactoryHelper<Story.Commands.SendSkillMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "gfxset", new StoryCommandFactoryHelper<Story.Commands.GfxSetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "gfxclear", new StoryCommandFactoryHelper<Story.Commands.GfxClearCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "addcomponent", new StoryCommandFactoryHelper<StorySystem.CommonCommands.DummyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "openurl", new StoryCommandFactoryHelper<StorySystem.CommonCommands.DummyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "quit", new StoryCommandFactoryHelper<StorySystem.CommonCommands.DummyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "bindui", new StoryCommandFactoryHelper<StorySystem.CommonCommands.DummyCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "activescene", new StoryCommandFactoryHelper<Story.Commands.ActiveSceneCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "changescene", new StoryCommandFactoryHelper<Story.Commands.ChangeSceneCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "changeroomscene", new StoryCommandFactoryHelper<Story.Commands.ChangeRoomSceneCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "notifychangescene", new StoryCommandFactoryHelper<Story.Commands.NotifyChangeSceneCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "createscenelogic", new StoryCommandFactoryHelper<Story.Commands.CreateSceneLogicCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroyscenelogic", new StoryCommandFactoryHelper<Story.Commands.DestroySceneLogicCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "pausescenelogic", new StoryCommandFactoryHelper<Story.Commands.PauseSceneLogicCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "restarttimeout", new StoryCommandFactoryHelper<Story.Commands.RestartTimeoutCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "highlightprompt", new StoryCommandFactoryHelper<Story.Commands.HighlightPromptCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "blackboardset", new StoryCommandFactoryHelper<Story.Commands.BlackboardSetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "blackboardclear", new StoryCommandFactoryHelper<Story.Commands.BlackboardClearCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollow", new StoryCommandFactoryHelper<Story.Commands.CameraFollowCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollowrange", new StoryCommandFactoryHelper<Story.Commands.CameraFollowRangeCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "camerafollowpath", new StoryCommandFactoryHelper<Story.Commands.CameraFollowPathCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "cameralook", new StoryCommandFactoryHelper<Story.Commands.CameraLookCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "lockframe", new StoryCommandFactoryHelper<Story.Commands.LockFrameCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "showdlg", new StoryCommandFactoryHelper<Story.Commands.ShowDlgCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "areadetect", new StoryCommandFactoryHelper<Story.Commands.AreaDetectCommand>());

                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "createnpc", new StoryCommandFactoryHelper<Story.Commands.CreateNpcCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroynpc", new StoryCommandFactoryHelper<Story.Commands.DestroyNpcCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "destroynpcwithobjid", new StoryCommandFactoryHelper<Story.Commands.DestroyNpcWithObjIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcface", new StoryCommandFactoryHelper<Story.Commands.NpcFaceCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcmove", new StoryCommandFactoryHelper<Story.Commands.NpcMoveCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcmovewithwaypoints", new StoryCommandFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcstop", new StoryCommandFactoryHelper<Story.Commands.NpcStopCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcattack", new StoryCommandFactoryHelper<Story.Commands.NpcAttackCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetformation", new StoryCommandFactoryHelper<Story.Commands.NpcSetFormationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcenableai", new StoryCommandFactoryHelper<Story.Commands.NpcEnableAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetai", new StoryCommandFactoryHelper<Story.Commands.NpcSetAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetaitarget", new StoryCommandFactoryHelper<Story.Commands.NpcSetAiTargetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcanimation", new StoryCommandFactoryHelper<Story.Commands.NpcAnimationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcaddimpact", new StoryCommandFactoryHelper<Story.Commands.NpcAddImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcremoveimpact", new StoryCommandFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npccastskill", new StoryCommandFactoryHelper<Story.Commands.NpcCastSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcstopskill", new StoryCommandFactoryHelper<Story.Commands.NpcStopSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcaddskill", new StoryCommandFactoryHelper<Story.Commands.NpcAddSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcremoveskill", new StoryCommandFactoryHelper<Story.Commands.NpcRemoveSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npclisten", new StoryCommandFactoryHelper<Story.Commands.NpcListenCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetcamp", new StoryCommandFactoryHelper<Story.Commands.NpcSetCampCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetsummonerid", new StoryCommandFactoryHelper<Story.Commands.NpcSetSummonerIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "npcsetsummonskillid", new StoryCommandFactoryHelper<Story.Commands.NpcSetSummonSkillIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objface", new StoryCommandFactoryHelper<Story.Commands.ObjFaceCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objmove", new StoryCommandFactoryHelper<Story.Commands.ObjMoveCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objmovewithwaypoints", new StoryCommandFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objstop", new StoryCommandFactoryHelper<Story.Commands.ObjStopCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objattack", new StoryCommandFactoryHelper<Story.Commands.ObjAttackCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetformation", new StoryCommandFactoryHelper<Story.Commands.ObjSetFormationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objenableai", new StoryCommandFactoryHelper<Story.Commands.ObjEnableAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetai", new StoryCommandFactoryHelper<Story.Commands.ObjSetAiCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetaitarget", new StoryCommandFactoryHelper<Story.Commands.ObjSetAiTargetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objanimation", new StoryCommandFactoryHelper<Story.Commands.ObjAnimationCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objaddimpact", new StoryCommandFactoryHelper<Story.Commands.ObjAddImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objremoveimpact", new StoryCommandFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objcastskill", new StoryCommandFactoryHelper<Story.Commands.ObjCastSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objstopskill", new StoryCommandFactoryHelper<Story.Commands.ObjStopSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objaddskill", new StoryCommandFactoryHelper<Story.Commands.ObjAddSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objremoveskill", new StoryCommandFactoryHelper<Story.Commands.ObjRemoveSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objlisten", new StoryCommandFactoryHelper<Story.Commands.ObjListenCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetcamp", new StoryCommandFactoryHelper<Story.Commands.ObjSetCampCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetsummonerid", new StoryCommandFactoryHelper<Story.Commands.ObjSetSummonerIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objsetsummonskillid", new StoryCommandFactoryHelper<Story.Commands.ObjSetSummonSkillIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setvisible", new StoryCommandFactoryHelper<StorySystem.CommonCommands.DummyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "sethp", new StoryCommandFactoryHelper<Story.Commands.SetHpCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setenergy", new StoryCommandFactoryHelper<Story.Commands.SetEnergyCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "objset", new StoryCommandFactoryHelper<Story.Commands.ObjSetCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setlevel", new StoryCommandFactoryHelper<Story.Commands.SetLevelCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setattr", new StoryCommandFactoryHelper<Story.Commands.SetAttrCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setunitid", new StoryCommandFactoryHelper<Story.Commands.SetUnitIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setleaderid", new StoryCommandFactoryHelper<Story.Commands.SetLeaderIdCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "markcontrolbystory", new StoryCommandFactoryHelper<Story.Commands.MarkControlByStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.GFX, "setstorystate", new StoryCommandFactoryHelper<Story.Commands.SetStoryStateCommand>());

                //注册值与函数处理
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "gfxget", new StoryValueFactoryHelper<Story.Values.GfxGetValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "gfxtime", new StoryValueFactoryHelper<Story.Values.GfxTimeValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "gfxtimescale", new StoryValueFactoryHelper<Story.Values.GfxTimeScaleValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "isactive", new StoryValueFactoryHelper<Story.Values.IsActiveValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "isreallyactive", new StoryValueFactoryHelper<Story.Values.IsReallyActiveValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getcomponent", new StoryValueFactoryHelper<Story.Values.GetComponentValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getgameobject", new StoryValueFactoryHelper<Story.Values.GetGameObjectValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getunitytype", new StoryValueFactoryHelper<Story.Values.GetUnityTypeValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getunityuitype", new StoryValueFactoryHelper<Story.Values.GetUnityUiTypeValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getactor", new StoryValueFactoryHelper<StorySystem.CommonValues.DummyValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getentityinfo", new StoryValueFactoryHelper<Story.Values.GetEntityInfoValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getentityview", new StoryValueFactoryHelper<Story.Values.GetEntityViewValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getuserinfo", new StoryValueFactoryHelper<StorySystem.CommonValues.DummyValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "isclient", new StoryValueFactoryHelper<Story.Values.IsClientValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "blackboardget", new StoryValueFactoryHelper<Story.Values.BlackboardGetValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getdialogitem", new StoryValueFactoryHelper<Story.Values.GetDialogItemValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getactoricon", new StoryValueFactoryHelper<Story.Values.GetActorIconValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getmonsterinfo", new StoryValueFactoryHelper<Story.Values.GetMonsterInfoValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getaidata", new StoryValueFactoryHelper<Story.Values.GetAiDataValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "npcidlist", new StoryValueFactoryHelper<Story.Values.NpcIdListValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "combatnpccount", new StoryValueFactoryHelper<Story.Values.CombatNpcCountValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "npccount", new StoryValueFactoryHelper<Story.Values.NpcCountValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "unitid2objid", new StoryValueFactoryHelper<Story.Values.UnitId2ObjIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objid2unitid", new StoryValueFactoryHelper<Story.Values.ObjId2UnitIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "unitid2uniqueid", new StoryValueFactoryHelper<Story.Values.UnitId2UniqueIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objid2uniqueid", new StoryValueFactoryHelper<Story.Values.ObjId2UniqueIdValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "npcgetformation", new StoryValueFactoryHelper<Story.Values.NpcGetFormationValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "npcgetnpctype", new StoryValueFactoryHelper<Story.Values.NpcGetNpcTypeValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "npcgetsummonerid", new StoryValueFactoryHelper<Story.Values.NpcGetSummonerIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "npcgetsummonskillid", new StoryValueFactoryHelper<Story.Values.NpcGetSummonSkillIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "npcfindimpactseqbyid", new StoryValueFactoryHelper<Story.Values.NpcFindImpactSeqByIdValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objgetformation", new StoryValueFactoryHelper<Story.Values.ObjGetFormationValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objgetnpctype", new StoryValueFactoryHelper<Story.Values.ObjGetNpcTypeValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objgetsummonerid", new StoryValueFactoryHelper<Story.Values.ObjGetSummonerIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objfindimpactseqbyid", new StoryValueFactoryHelper<Story.Values.ObjFindImpactSeqByIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objgetsummonskillid", new StoryValueFactoryHelper<Story.Values.ObjGetSummonSkillIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "isenemy", new StoryValueFactoryHelper<Story.Values.IsEnemyValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "isfriend", new StoryValueFactoryHelper<Story.Values.IsFriendValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getposition", new StoryValueFactoryHelper<Story.Values.GetPositionValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getpositionx", new StoryValueFactoryHelper<Story.Values.GetPositionXValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getpositiony", new StoryValueFactoryHelper<Story.Values.GetPositionYValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getpositionz", new StoryValueFactoryHelper<Story.Values.GetPositionZValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getcamp", new StoryValueFactoryHelper<Story.Values.GetCampValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "iscombatnpc", new StoryValueFactoryHelper<Story.Values.IsCombatNpcValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "isvisible", new StoryValueFactoryHelper<StorySystem.CommonValues.DummyValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "gethp", new StoryValueFactoryHelper<Story.Values.GetHpValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getenergy", new StoryValueFactoryHelper<Story.Values.GetEnergyValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getmaxhp", new StoryValueFactoryHelper<Story.Values.GetMaxHpValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getmaxenergy", new StoryValueFactoryHelper<Story.Values.GetMaxEnergyValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "calcoffset", new StoryValueFactoryHelper<Story.Values.CalcOffsetValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "calcdir", new StoryValueFactoryHelper<Story.Values.CalcDirValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "objget", new StoryValueFactoryHelper<Story.Values.ObjGetValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getlinkid", new StoryValueFactoryHelper<Story.Values.GetLinkIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getlevel", new StoryValueFactoryHelper<Story.Values.GetLevelValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getattr", new StoryValueFactoryHelper<Story.Values.GetAttrValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "iscontrolbystory", new StoryValueFactoryHelper<Story.Values.IsControlByStoryValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "cancastskill", new StoryValueFactoryHelper<Story.Values.CanCastSkillValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "isundercontrol", new StoryValueFactoryHelper<Story.Values.IsUnderControlValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getleaderid", new StoryValueFactoryHelper<Story.Values.GetLeaderIdValue>());

                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getleaderlinkid", new StoryValueFactoryHelper<Story.Values.GetLeaderLinkIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getmembercount", new StoryValueFactoryHelper<Story.Values.GetMemberCountValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getmemberlinkid", new StoryValueFactoryHelper<Story.Values.GetMemberLinkIdValue>());
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.GFX, "getmemberlevel", new StoryValueFactoryHelper<Story.Values.GetMemberLevelValue>());

                LoadCustomCommandsAndValues();
            }
        }

        private static void LoadCustomCommandsAndValues()
        {
            string cmdFile = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + "Story/Common/CustomCommands.dsl");
            string valFile = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + "Story/Common/CustomValues.dsl");

            Dsl.DslFile file1 = CustomCommandValueParser.LoadStory(cmdFile);
            Dsl.DslFile file2 = CustomCommandValueParser.LoadStory(valFile);
            CustomCommandValueParser.FirstParse(file1, file2);
            CustomCommandValueParser.FinalParse(file1, file2);
        }
        private static bool s_IsInited = false;
    }
}
