using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

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
                    info.Context = m_Scene;
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
                inst.Context = m_Scene;
                inst.ContextVariables = m_ContextVariables;
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

        private StrBoxedValueDict m_ContextVariables = new StrBoxedValueDict();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private Dictionary<string, List<AiStoryInstanceInfo>> m_AiStoryInstancePool = new Dictionary<string, List<AiStoryInstanceInfo>>();
        private Scene m_Scene = null;

        public static void ThreadInitMask()
        {
            //DslCalculator does not use thread masks
        }
        public static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;

                var registry = DslCalculatorHost.GetSharedApiRegistry();

                //register story commands
                registry.Register("startstory", "startstory(story_id[, multiple])", new ExpressionFactoryHelper<Story.Commands.StartStoryCommand>());
                registry.Register("stopstory", "stopstory(story_id)", new ExpressionFactoryHelper<Story.Commands.StopStoryCommand>());
                registry.Register("waitstory", "waitstory(story_id)", new ExpressionFactoryHelper<Story.Commands.WaitStoryCommand>());
                registry.Register("pausestory", "pausestory(story_id)", new ExpressionFactoryHelper<Story.Commands.PauseStoryCommand>());
                registry.Register("resumestory", "resumestory(story_id)", new ExpressionFactoryHelper<Story.Commands.ResumeStoryCommand>());
                registry.Register("firemessage", "firemessage(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.FireMessageCommand>());
                registry.Register("fireconcurrentmessage", "fireconcurrentmessage(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.FireConcurrentMessageCommand>());
                registry.Register("waitallmessage", "waitallmessage(msg)", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageCommand>());
                registry.Register("waitallmessagehandler", "waitallmessagehandler(msg)", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
                registry.Register("suspendallmessagehandler", "suspendallmessagehandler(msg)", new ExpressionFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
                registry.Register("resumeallmessagehandler", "resumeallmessagehandler(msg)", new ExpressionFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
                registry.Register("sendserverstorymessage", "sendserverstorymessage(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
                registry.Register("sendclientstorymessage", "sendclientstorymessage(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendClientStoryMessageCommand>());
                registry.Register("publishgfxevent", "publishgfxevent(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.PublishGfxEventCommand>());
                registry.Register("sendgfxmessage", "sendgfxmessage(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageCommand>());
                registry.Register("sendgfxmessagewithtag", "sendgfxmessagewithtag(tag, msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());

                registry.Register("activescene", "activescene(scene_id)", new ExpressionFactoryHelper<Story.Commands.ActiveSceneCommand>());
                registry.Register("changescene", "changescene(scene_id)", new ExpressionFactoryHelper<Story.Commands.ChangeSceneCommand>());
                registry.Register("changeroomscene", "changeroomscene(scene_id)", new ExpressionFactoryHelper<Story.Commands.ChangeRoomSceneCommand>());

                registry.Register("createscenelogic", "createscenelogic(logic_id)", new ExpressionFactoryHelper<Story.Commands.CreateSceneLogicCommand>());
                registry.Register("destroyscenelogic", "destroyscenelogic(logic_id)", new ExpressionFactoryHelper<Story.Commands.DestroySceneLogicCommand>());
                registry.Register("pausescenelogic", "pausescenelogic(logic_id)", new ExpressionFactoryHelper<Story.Commands.PauseSceneLogicCommand>());
                registry.Register("restarttimeout", "restarttimeout()", new ExpressionFactoryHelper<Story.Commands.RestartTimeoutCommand>());
                registry.Register("highlightprompt", "highlightprompt(msg, ...)", new ExpressionFactoryHelper<Story.Commands.HighlightPromptCommand>());

                registry.Register("blackboardset", "blackboardset(name, value)", new ExpressionFactoryHelper<Story.Commands.BlackboardSetCommand>());
                registry.Register("blackboardclear", "blackboardclear(name)", new ExpressionFactoryHelper<Story.Commands.BlackboardClearCommand>());
                registry.Register("camerafollow", "camerafollow(objid)", new ExpressionFactoryHelper<Story.Commands.CameraFollowCommand>());
                registry.Register("camerafollowrange", "camerafollowrange(objid, range)", new ExpressionFactoryHelper<Story.Commands.CameraFollowRangeCommand>());
                registry.Register("camerafollowpath", "camerafollowpath(waypoints)", new ExpressionFactoryHelper<Story.Commands.CameraFollowPathCommand>());
                registry.Register("cameralook", "cameralook(objid)", new ExpressionFactoryHelper<Story.Commands.CameraLookCommand>());
                registry.Register("lockframe", "lockframe(is_lock)", new ExpressionFactoryHelper<Story.Commands.LockFrameCommand>());
                registry.Register("showdlg", "showdlg(dlg_id, ...)", new ExpressionFactoryHelper<Story.Commands.ShowDlgCommand>());
                registry.Register("areadetect", "areadetect(area_id, ...)", new ExpressionFactoryHelper<Story.Commands.AreaDetectCommand>());

                registry.Register("createnpc", "createnpc(npc_unit_id, vector3(x,y,z), dir, camp, tableId[, ai, stringlist(\"...\"), leaderId])[objid(\"@objid\")]", new ExpressionFactoryHelper<Story.Commands.CreateNpcCommand>());
                registry.Register("destroynpc", "destroynpc(npc_unit_id)", new ExpressionFactoryHelper<Story.Commands.DestroyNpcCommand>());
                registry.Register("destroynpcwithobjid", "destroynpcwithobjid(npc_obj_id)", new ExpressionFactoryHelper<Story.Commands.DestroyNpcWithObjIdCommand>());
                registry.Register("npcface", "npcface(npc_unit_id, dir)", new ExpressionFactoryHelper<Story.Commands.NpcFaceCommand>());
                registry.Register("npcmove", "npcmove(npc_unit_id, vector3(x,y,z))", new ExpressionFactoryHelper<Story.Commands.NpcMoveCommand>());
                registry.Register("npcmovewithwaypoints", "npcmovewithwaypoints(npc_unit_id, vector3list(\"...\"))", new ExpressionFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
                registry.Register("npcstop", "npcstop(npc_unit_id)", new ExpressionFactoryHelper<Story.Commands.NpcStopCommand>());
                registry.Register("npcattack", "npcattack(npc_unit_id[, target_unit_id])", new ExpressionFactoryHelper<Story.Commands.NpcAttackCommand>());
                registry.Register("npcsetformation", "npcsetformation(npc_unit_id, index)", new ExpressionFactoryHelper<Story.Commands.NpcSetFormationCommand>());
                registry.Register("npcenableai", "npcenableai(npc_unit_id, 1_or_0)", new ExpressionFactoryHelper<Story.Commands.NpcEnableAiCommand>());
                registry.Register("npcsetai", "npcsetai(unitid, ai_logic_id, stringlist(\"...\"))", new ExpressionFactoryHelper<Story.Commands.NpcSetAiCommand>());
                registry.Register("npcsetaitarget", "npcsetaitarget(unitid, targetId)", new ExpressionFactoryHelper<Story.Commands.NpcSetAiTargetCommand>());
                registry.Register("npcanimation", "npcanimation(unit_id, anim)", new ExpressionFactoryHelper<Story.Commands.NpcAnimationCommand>());
                registry.Register("npcaddimpact", "npcaddimpact(unit_id, impactid, ...)[seq(\"@seq\")]", new ExpressionFactoryHelper<Story.Commands.NpcAddImpactCommand>());
                registry.Register("npcremoveimpact", "npcremoveimpact(unit_id, seq)", new ExpressionFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
                registry.Register("npccastskill", "npccastskill(unit_id, skillid, ...)", new ExpressionFactoryHelper<Story.Commands.NpcCastSkillCommand>());
                registry.Register("npcstopskill", "npcstopskill(unit_id)", new ExpressionFactoryHelper<Story.Commands.NpcStopSkillCommand>());
                registry.Register("npcaddskill", "npcaddskill(unit_id, skillid)", new ExpressionFactoryHelper<Story.Commands.NpcAddSkillCommand>());
                registry.Register("npcremoveskill", "npcremoveskill(unit_id, skillid)", new ExpressionFactoryHelper<Story.Commands.NpcRemoveSkillCommand>());
                registry.Register("npclisten", "npclisten(unit_id, message_type, true_or_false)", new ExpressionFactoryHelper<Story.Commands.NpcListenCommand>());
                registry.Register("npcsetcamp", "npcsetcamp(npc_unit_id, camp_id)", new ExpressionFactoryHelper<Story.Commands.NpcSetCampCommand>());
                registry.Register("npcsetsummonerid", "npcsetsummonerid(unit_id, objid)", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonerIdCommand>());
                registry.Register("npcsetsummonskillid", "npcsetsummonskillid(unit_id, objid)", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonSkillIdCommand>());
                registry.Register("objface", "objface(obj_id, dir)", new ExpressionFactoryHelper<Story.Commands.ObjFaceCommand>());
                registry.Register("objmove", "objmove(obj_id, vector3(x,y,z))", new ExpressionFactoryHelper<Story.Commands.ObjMoveCommand>());
                registry.Register("objmovewithwaypoints", "objmovewithwaypoints(obj_id, vector3list(\"...\"))", new ExpressionFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
                registry.Register("objstop", "objstop(obj_id)", new ExpressionFactoryHelper<Story.Commands.ObjStopCommand>());
                registry.Register("objattack", "objattack(obj_id[, target_id])", new ExpressionFactoryHelper<Story.Commands.ObjAttackCommand>());
                registry.Register("objsetformation", "objsetformation(obj_id, index)", new ExpressionFactoryHelper<Story.Commands.ObjSetFormationCommand>());
                registry.Register("objenableai", "objenableai(obj_id, 1_or_0)", new ExpressionFactoryHelper<Story.Commands.ObjEnableAiCommand>());
                registry.Register("objsetai", "objsetai(objid, ai_logic_id, stringlist(\"...\"))", new ExpressionFactoryHelper<Story.Commands.ObjSetAiCommand>());
                registry.Register("objsetaitarget", "objsetaitarget(objid, targetId)", new ExpressionFactoryHelper<Story.Commands.ObjSetAiTargetCommand>());
                registry.Register("objanimation", "objanimation(obj_id, anim)", new ExpressionFactoryHelper<Story.Commands.ObjAnimationCommand>());
                registry.Register("objaddimpact", "objaddimpact(obj_id, impactid, ...)[seq(\"@seq\")]", new ExpressionFactoryHelper<Story.Commands.ObjAddImpactCommand>());
                registry.Register("objremoveimpact", "objremoveimpact(obj_id, seq)", new ExpressionFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
                registry.Register("objcastskill", "objcastskill(obj_id, skillid, ...)", new ExpressionFactoryHelper<Story.Commands.ObjCastSkillCommand>());
                registry.Register("objstopskill", "objstopskill(obj_id)", new ExpressionFactoryHelper<Story.Commands.ObjStopSkillCommand>());
                registry.Register("objaddskill", "objaddskill(obj_id, skillid)", new ExpressionFactoryHelper<Story.Commands.ObjAddSkillCommand>());
                registry.Register("objremoveskill", "objremoveskill(obj_id, skillid)", new ExpressionFactoryHelper<Story.Commands.ObjRemoveSkillCommand>());
                registry.Register("objlisten", "objlisten(obj_id, message_type, true_or_false)", new ExpressionFactoryHelper<Story.Commands.ObjListenCommand>());
                registry.Register("objsetcamp", "objsetcamp(obj_id, camp_id)", new ExpressionFactoryHelper<Story.Commands.ObjSetCampCommand>());
                registry.Register("objsetsummonerid", "objsetsummonerid(obj_id, objid)", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonerIdCommand>());
                registry.Register("objsetsummonskillid", "objsetsummonskillid(obj_id, objid)", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonSkillIdCommand>());
                registry.Register("setvisible", "setvisible(obj_id, visible)", new ExpressionFactoryHelper<Story.Commands.DummyCommand>());
                registry.Register("sethp", "sethp(obj_id, hp)", new ExpressionFactoryHelper<Story.Commands.SetHpCommand>());
                registry.Register("setenergy", "setenergy(obj_id, energy)", new ExpressionFactoryHelper<Story.Commands.SetEnergyCommand>());
                registry.Register("objset", "objset(uniqueId, name, value)", new ExpressionFactoryHelper<Story.Commands.ObjSetCommand>());
                registry.Register("setlevel", "setlevel(obj_id, level)", new ExpressionFactoryHelper<Story.Commands.SetLevelCommand>());
                registry.Register("setattr", "setattr(obj_id, attr_name, value)", new ExpressionFactoryHelper<Story.Commands.SetAttrCommand>());
                registry.Register("setunitid", "setunitid(obj_id, unit_id)", new ExpressionFactoryHelper<Story.Commands.SetUnitIdCommand>());
                registry.Register("setleaderid", "setleaderid(obj_id, leader_id)", new ExpressionFactoryHelper<Story.Commands.SetLeaderIdCommand>());
                registry.Register("markcontrolbystory", "markcontrolbystory(obj_id, flag)", new ExpressionFactoryHelper<Story.Commands.MarkControlByStoryCommand>());

                //register value or functions
                registry.Register("gettime", "gettime()", new ExpressionFactoryHelper<Story.Functions.GetTimeFunction>());
                registry.Register("gettimescale", "gettimescale()", new ExpressionFactoryHelper<Story.Functions.GetTimeScaleFunction>());
                registry.Register("getentityinfo", "getentityinfo(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetEntityInfoFunction>());

                registry.Register("isclient", "isclient()", new ExpressionFactoryHelper<Story.Functions.IsClientFunction>());
                registry.Register("getroomid", "getroomid()", new ExpressionFactoryHelper<Story.Functions.GetRoomIdFunction>());
                registry.Register("getsceneid", "getsceneid()", new ExpressionFactoryHelper<Story.Functions.GetSceneIdFunction>());

                registry.Register("blackboardget", "blackboardget(name[, default])", new ExpressionFactoryHelper<Story.Functions.BlackboardGetFunction>());
                registry.Register("getdialogitem", "getdialogitem(dlg_id, index)", new ExpressionFactoryHelper<Story.Functions.GetDialogItemFunction>());
                registry.Register("getmonsterinfo", "getmonsterinfo(camp_id, index)", new ExpressionFactoryHelper<Story.Functions.GetMonsterInfoFunction>());
                registry.Register("getaidata", "getaidata(obj_id, type_name)", new ExpressionFactoryHelper<Story.Functions.GetAiDataFunction>());

                registry.Register("npcidlist", "npcidlist()", new ExpressionFactoryHelper<Story.Functions.NpcIdListFunction>());
                registry.Register("combatnpccount", "combatnpccount([camp_id])", new ExpressionFactoryHelper<Story.Functions.CombatNpcCountFunction>());
                registry.Register("npccount", "npccount(start_unit_id, end_unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcCountFunction>());
                registry.Register("unitid2objid", "unitid2objid(unit_id)", new ExpressionFactoryHelper<Story.Functions.UnitId2ObjIdFunction>());
                registry.Register("objid2unitid", "objid2unitid(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjId2UnitIdFunction>());
                registry.Register("unitid2uniqueid", "unitid2uniqueid(unit_id)", new ExpressionFactoryHelper<Story.Functions.UnitId2UniqueIdFunction>());
                registry.Register("objid2uniqueid", "objid2uniqueid(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjId2UniqueIdFunction>());

                registry.Register("npcgetformation", "npcgetformation(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetFormationFunction>());
                registry.Register("npcgetnpctype", "npcgetnpctype(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetNpcTypeFunction>());
                registry.Register("npcgetsummonerid", "npcgetsummonerid(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonerIdFunction>());
                registry.Register("npcgetsummonskillid", "npcgetsummonskillid(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonSkillIdFunction>());
                registry.Register("npcfindimpactseqbyid", "npcfindimpactseqbyid(unit_id, impact_id)", new ExpressionFactoryHelper<Story.Functions.NpcFindImpactSeqByIdFunction>());

                registry.Register("objgetformation", "objgetformation(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetFormationFunction>());
                registry.Register("objgetnpctype", "objgetnpctype(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetNpcTypeFunction>());
                registry.Register("objgetsummonerid", "objgetsummonerid(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonerIdFunction>());
                registry.Register("objfindimpactseqbyid", "objfindimpactseqbyid(obj_id, impact_id)", new ExpressionFactoryHelper<Story.Functions.ObjFindImpactSeqByIdFunction>());
                registry.Register("objgetsummonskillid", "objgetsummonskillid(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonSkillIdFunction>());
                registry.Register("isenemy", "isenemy(camp1, camp2)", new ExpressionFactoryHelper<Story.Functions.IsEnemyFunction>());
                registry.Register("isfriend", "isfriend(camp1, camp2)", new ExpressionFactoryHelper<Story.Functions.IsFriendFunction>());

                registry.Register("getposition", "getposition(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionFunction>());
                registry.Register("getpositionx", "getpositionx(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionXFunction>());
                registry.Register("getpositiony", "getpositiony(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionYFunction>());
                registry.Register("getpositionz", "getpositionz(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionZFunction>());
                registry.Register("getcamp", "getcamp(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetCampFunction>());
                registry.Register("iscombatnpc", "iscombatnpc(obj_id)", new ExpressionFactoryHelper<Story.Functions.IsCombatNpcFunction>());

                registry.Register("gethp", "gethp(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetHpFunction>());
                registry.Register("getenergy", "getenergy(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetEnergyFunction>());
                registry.Register("getmaxhp", "getmaxhp(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetMaxHpFunction>());
                registry.Register("getmaxenergy", "getmaxenergy(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetMaxEnergyFunction>());
                registry.Register("calcoffset", "calcoffset(obj_id, target_id, offset)", new ExpressionFactoryHelper<Story.Functions.CalcOffsetFunction>());
                registry.Register("calcdir", "calcdir(obj_id, target_id)", new ExpressionFactoryHelper<Story.Functions.CalcDirFunction>());
                registry.Register("objget", "objget(uniqueId, name[, default])", new ExpressionFactoryHelper<Story.Functions.ObjGetFunction>());
                registry.Register("gettableid", "gettableid(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetTableIdFunction>());
                registry.Register("getlevel", "getlevel(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetLevelFunction>());
                registry.Register("getattr", "getattr(obj_id, attr_name[, default])", new ExpressionFactoryHelper<Story.Functions.GetAttrFunction>());
                registry.Register("iscontrolbystory", "iscontrolbystory(obj_id)", new ExpressionFactoryHelper<Story.Functions.IsControlByStoryFunction>());
                registry.Register("cancastskill", "cancastskill(obj_id[, skill_id])", new ExpressionFactoryHelper<Story.Functions.CanCastSkillFunction>());
                registry.Register("isundercontrol", "isundercontrol(obj_id)", new ExpressionFactoryHelper<Story.Functions.IsUnderControlFunction>());
                registry.Register("getleaderid", "getleaderid([obj_id])", new ExpressionFactoryHelper<Story.Functions.GetLeaderIdFunction>());
                registry.Register("getleadertableid", "getleadertableid(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetLeaderTableIdFunction>());
            }
        }

        private static bool s_IsInited = false;
    }
}
