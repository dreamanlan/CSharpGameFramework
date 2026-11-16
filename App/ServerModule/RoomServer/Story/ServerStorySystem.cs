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
                registry.Register("start_story", "start_story(story_id[, multiple])", new ExpressionFactoryHelper<Story.Commands.StartStoryCommand>());
                registry.Register("stop_story", "stop_story(story_id)", new ExpressionFactoryHelper<Story.Commands.StopStoryCommand>());
                registry.Register("wait_story", "wait_story(story_id)", new ExpressionFactoryHelper<Story.Commands.WaitStoryCommand>());
                registry.Register("pause_story", "pause_story(story_id)", new ExpressionFactoryHelper<Story.Commands.PauseStoryCommand>());
                registry.Register("resume_story", "resume_story(story_id)", new ExpressionFactoryHelper<Story.Commands.ResumeStoryCommand>());
                registry.Register("fire_message", "fire_message(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.FireMessageCommand>());
                registry.Register("fire_concurrent_message", "fire_concurrent_message(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.FireConcurrentMessageCommand>());
                registry.Register("wait_all_message", "wait_all_message(msg)", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageCommand>());
                registry.Register("wait_all_message_handler", "wait_all_message_handler(msg)", new ExpressionFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
                registry.Register("suspend_all_message_handler", "suspend_all_message_handler(msg)", new ExpressionFactoryHelper<Story.Commands.SuspendAllMessageHandlerCommand>());
                registry.Register("resume_all_message_handler", "resume_all_message_handler(msg)", new ExpressionFactoryHelper<Story.Commands.ResumeAllMessageHandlerCommand>());
                registry.Register("send_server_story_message", "send_server_story_message(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
                registry.Register("send_client_story_message", "send_client_story_message(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendClientStoryMessageCommand>());
                registry.Register("publish_gfx_event", "publish_gfx_event(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.PublishGfxEventCommand>());
                registry.Register("send_gfx_message", "send_gfx_message(msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageCommand>());
                registry.Register("send_gfx_message_with_tag", "send_gfx_message_with_tag(tag, msg, arg1, arg2, ...)", new ExpressionFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());

                registry.Register("active_scene", "active_scene(scene_id)", new ExpressionFactoryHelper<Story.Commands.ActiveSceneCommand>());
                registry.Register("change_scene", "change_scene(scene_id)", new ExpressionFactoryHelper<Story.Commands.ChangeSceneCommand>());
                registry.Register("change_room_scene", "change_room_scene(scene_id)", new ExpressionFactoryHelper<Story.Commands.ChangeRoomSceneCommand>());

                registry.Register("create_scene_logic", "create_scene_logic(logic_id)", new ExpressionFactoryHelper<Story.Commands.CreateSceneLogicCommand>());
                registry.Register("destroy_scene_logic", "destroy_scene_logic(logic_id)", new ExpressionFactoryHelper<Story.Commands.DestroySceneLogicCommand>());
                registry.Register("pause_scene_logic", "pause_scene_logic(logic_id)", new ExpressionFactoryHelper<Story.Commands.PauseSceneLogicCommand>());
                registry.Register("restart_timeout", "restart_timeout()", new ExpressionFactoryHelper<Story.Commands.RestartTimeoutCommand>());
                registry.Register("highlight_prompt", "highlight_prompt(msg, ...)", new ExpressionFactoryHelper<Story.Commands.HighlightPromptCommand>());

                registry.Register("blackboard_set", "blackboard_set(name, value)", new ExpressionFactoryHelper<Story.Commands.BlackboardSetCommand>());
                registry.Register("blackboard_clear", "blackboard_clear(name)", new ExpressionFactoryHelper<Story.Commands.BlackboardClearCommand>());
                registry.Register("camera_follow", "camera_follow(objid)", new ExpressionFactoryHelper<Story.Commands.CameraFollowCommand>());
                registry.Register("camera_follow_range", "camera_follow_range(objid, range)", new ExpressionFactoryHelper<Story.Commands.CameraFollowRangeCommand>());
                registry.Register("camera_follow_path", "camera_follow_path(waypoints)", new ExpressionFactoryHelper<Story.Commands.CameraFollowPathCommand>());
                registry.Register("camera_look", "camera_look(objid)", new ExpressionFactoryHelper<Story.Commands.CameraLookCommand>());
                registry.Register("lock_frame", "lock_frame(is_lock)", new ExpressionFactoryHelper<Story.Commands.LockFrameCommand>());
                registry.Register("show_dlg", "show_dlg(dlg_id, ...)", new ExpressionFactoryHelper<Story.Commands.ShowDlgCommand>());
                registry.Register("area_detect", "area_detect(area_id, ...)", new ExpressionFactoryHelper<Story.Commands.AreaDetectCommand>());

                registry.Register("create_npc", "create_npc(npc_unit_id, vector_3(x,y,z), dir, camp, tableId[, ai, string_list(\"...\"), leaderId])[objid(\"@objid\")]", new ExpressionFactoryHelper<Story.Commands.CreateNpcCommand>());
                registry.Register("destroy_npc", "destroy_npc(npc_unit_id)", new ExpressionFactoryHelper<Story.Commands.DestroyNpcCommand>());
                registry.Register("destroy_npc_with_obj_id", "destroy_npc_with_obj_id(npc_obj_id)", new ExpressionFactoryHelper<Story.Commands.DestroyNpcWithObjIdCommand>());
                registry.Register("npc_face", "npc_face(npc_unit_id, dir)", new ExpressionFactoryHelper<Story.Commands.NpcFaceCommand>());
                registry.Register("npc_move", "npc_move(npc_unit_id, vector_3(x,y,z))", new ExpressionFactoryHelper<Story.Commands.NpcMoveCommand>());
                registry.Register("npc_move_with_waypoints", "npc_move_with_waypoints(npc_unit_id, vector_3_list(\"...\"))", new ExpressionFactoryHelper<Story.Commands.NpcMoveWithWaypointsCommand>());
                registry.Register("npc_stop", "npc_stop(npc_unit_id)", new ExpressionFactoryHelper<Story.Commands.NpcStopCommand>());
                registry.Register("npc_attack", "npc_attack(npc_unit_id[, target_unit_id])", new ExpressionFactoryHelper<Story.Commands.NpcAttackCommand>());
                registry.Register("npc_set_formation", "npc_set_formation(npc_unit_id, index)", new ExpressionFactoryHelper<Story.Commands.NpcSetFormationCommand>());
                registry.Register("npc_enable_ai", "npc_enable_ai(npc_unit_id, 1_or_0)", new ExpressionFactoryHelper<Story.Commands.NpcEnableAiCommand>());
                registry.Register("npc_set_ai", "npc_set_ai(unitid, ai_logic_id, string_list(\"...\"))", new ExpressionFactoryHelper<Story.Commands.NpcSetAiCommand>());
                registry.Register("npc_set_ai_target", "npc_set_ai_target(unitid, targetId)", new ExpressionFactoryHelper<Story.Commands.NpcSetAiTargetCommand>());
                registry.Register("npc_animation", "npc_animation(unit_id, anim)", new ExpressionFactoryHelper<Story.Commands.NpcAnimationCommand>());
                registry.Register("npc_add_impact", "npc_add_impact(unit_id, impactid, ...)[seq(\"@seq\")]", new ExpressionFactoryHelper<Story.Commands.NpcAddImpactCommand>());
                registry.Register("npc_remove_impact", "npc_remove_impact(unit_id, seq)", new ExpressionFactoryHelper<Story.Commands.NpcRemoveImpactCommand>());
                registry.Register("npc_cast_skill", "npc_cast_skill(unit_id, skillid, ...)", new ExpressionFactoryHelper<Story.Commands.NpcCastSkillCommand>());
                registry.Register("npc_stop_skill", "npc_stop_skill(unit_id)", new ExpressionFactoryHelper<Story.Commands.NpcStopSkillCommand>());
                registry.Register("npc_add_skill", "npc_add_skill(unit_id, skillid)", new ExpressionFactoryHelper<Story.Commands.NpcAddSkillCommand>());
                registry.Register("npc_remove_skill", "npc_remove_skill(unit_id, skillid)", new ExpressionFactoryHelper<Story.Commands.NpcRemoveSkillCommand>());
                registry.Register("npc_listen", "npc_listen(unit_id, message_type, true_or_false)", new ExpressionFactoryHelper<Story.Commands.NpcListenCommand>());
                registry.Register("npc_set_camp", "npc_set_camp(npc_unit_id, camp_id)", new ExpressionFactoryHelper<Story.Commands.NpcSetCampCommand>());
                registry.Register("npc_set_summoner_id", "npc_set_summoner_id(unit_id, objid)", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonerIdCommand>());
                registry.Register("npc_set_summon_skill_id", "npc_set_summon_skill_id(unit_id, objid)", new ExpressionFactoryHelper<Story.Commands.NpcSetSummonSkillIdCommand>());
                registry.Register("obj_face", "obj_face(obj_id, dir)", new ExpressionFactoryHelper<Story.Commands.ObjFaceCommand>());
                registry.Register("obj_move", "obj_move(obj_id, vector_3(x,y,z))", new ExpressionFactoryHelper<Story.Commands.ObjMoveCommand>());
                registry.Register("obj_move_with_waypoints", "obj_move_with_waypoints(obj_id, vector_3_list(\"...\"))", new ExpressionFactoryHelper<Story.Commands.ObjMoveWithWaypointsCommand>());
                registry.Register("obj_stop", "obj_stop(obj_id)", new ExpressionFactoryHelper<Story.Commands.ObjStopCommand>());
                registry.Register("obj_attack", "obj_attack(obj_id[, target_id])", new ExpressionFactoryHelper<Story.Commands.ObjAttackCommand>());
                registry.Register("obj_set_formation", "obj_set_formation(obj_id, index)", new ExpressionFactoryHelper<Story.Commands.ObjSetFormationCommand>());
                registry.Register("obj_enable_ai", "obj_enable_ai(obj_id, 1_or_0)", new ExpressionFactoryHelper<Story.Commands.ObjEnableAiCommand>());
                registry.Register("obj_set_ai", "obj_set_ai(objid, ai_logic_id, string_list(\"...\"))", new ExpressionFactoryHelper<Story.Commands.ObjSetAiCommand>());
                registry.Register("obj_set_ai_target", "obj_set_ai_target(objid, targetId)", new ExpressionFactoryHelper<Story.Commands.ObjSetAiTargetCommand>());
                registry.Register("obj_animation", "obj_animation(obj_id, anim)", new ExpressionFactoryHelper<Story.Commands.ObjAnimationCommand>());
                registry.Register("obj_add_impact", "obj_add_impact(obj_id, impactid, ...)[seq(\"@seq\")]", new ExpressionFactoryHelper<Story.Commands.ObjAddImpactCommand>());
                registry.Register("obj_remove_impact", "obj_remove_impact(obj_id, seq)", new ExpressionFactoryHelper<Story.Commands.ObjRemoveImpactCommand>());
                registry.Register("obj_cast_skill", "obj_cast_skill(obj_id, skillid, ...)", new ExpressionFactoryHelper<Story.Commands.ObjCastSkillCommand>());
                registry.Register("obj_stop_skill", "obj_stop_skill(obj_id)", new ExpressionFactoryHelper<Story.Commands.ObjStopSkillCommand>());
                registry.Register("obj_add_skill", "obj_add_skill(obj_id, skillid)", new ExpressionFactoryHelper<Story.Commands.ObjAddSkillCommand>());
                registry.Register("obj_remove_skill", "obj_remove_skill(obj_id, skillid)", new ExpressionFactoryHelper<Story.Commands.ObjRemoveSkillCommand>());
                registry.Register("obj_listen", "obj_listen(obj_id, message_type, true_or_false)", new ExpressionFactoryHelper<Story.Commands.ObjListenCommand>());
                registry.Register("obj_set_camp", "obj_set_camp(obj_id, camp_id)", new ExpressionFactoryHelper<Story.Commands.ObjSetCampCommand>());
                registry.Register("obj_set_summoner_id", "obj_set_summoner_id(obj_id, objid)", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonerIdCommand>());
                registry.Register("obj_set_summon_skill_id", "obj_set_summon_skill_id(obj_id, objid)", new ExpressionFactoryHelper<Story.Commands.ObjSetSummonSkillIdCommand>());
                registry.Register("set_visible", "set_visible(obj_id, visible)", new ExpressionFactoryHelper<Story.Commands.DummyCommand>());
                registry.Register("set_hp", "set_hp(obj_id, hp)", new ExpressionFactoryHelper<Story.Commands.SetHpCommand>());
                registry.Register("set_energy", "set_energy(obj_id, energy)", new ExpressionFactoryHelper<Story.Commands.SetEnergyCommand>());
                registry.Register("obj_set", "obj_set(uniqueId, name, value)", new ExpressionFactoryHelper<Story.Commands.ObjSetCommand>());
                registry.Register("set_level", "set_level(obj_id, level)", new ExpressionFactoryHelper<Story.Commands.SetLevelCommand>());
                registry.Register("set_attr", "set_attr(obj_id, attr_name, value)", new ExpressionFactoryHelper<Story.Commands.SetAttrCommand>());
                registry.Register("set_unit_id", "set_unit_id(obj_id, unit_id)", new ExpressionFactoryHelper<Story.Commands.SetUnitIdCommand>());
                registry.Register("set_leader_id", "set_leader_id(obj_id, leader_id)", new ExpressionFactoryHelper<Story.Commands.SetLeaderIdCommand>());
                registry.Register("mark_control_by_story", "mark_control_by_story(obj_id, flag)", new ExpressionFactoryHelper<Story.Commands.MarkControlByStoryCommand>());

                //register value or functions
                registry.Register("get_time", "get_time()", new ExpressionFactoryHelper<Story.Functions.GetTimeFunction>());
                registry.Register("get_time_scale", "get_time_scale()", new ExpressionFactoryHelper<Story.Functions.GetTimeScaleFunction>());
                registry.Register("get_entity_info", "get_entity_info(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetEntityInfoFunction>());

                registry.Register("is_client", "is_client()", new ExpressionFactoryHelper<Story.Functions.IsClientFunction>());
                registry.Register("get_room_id", "get_room_id()", new ExpressionFactoryHelper<Story.Functions.GetRoomIdFunction>());
                registry.Register("get_scene_id", "get_scene_id()", new ExpressionFactoryHelper<Story.Functions.GetSceneIdFunction>());

                registry.Register("blackboard_get", "blackboard_get(name[, default])", new ExpressionFactoryHelper<Story.Functions.BlackboardGetFunction>());
                registry.Register("get_dialog_item", "get_dialog_item(dlg_id, index)", new ExpressionFactoryHelper<Story.Functions.GetDialogItemFunction>());
                registry.Register("get_monster_info", "get_monster_info(camp_id, index)", new ExpressionFactoryHelper<Story.Functions.GetMonsterInfoFunction>());
                registry.Register("get_ai_data", "get_ai_data(obj_id, type_name)", new ExpressionFactoryHelper<Story.Functions.GetAiDataFunction>());

                registry.Register("npc_id_list", "npc_id_list()", new ExpressionFactoryHelper<Story.Functions.NpcIdListFunction>());
                registry.Register("combat_npc_count", "combat_npc_count([camp_id])", new ExpressionFactoryHelper<Story.Functions.CombatNpcCountFunction>());
                registry.Register("npc_count", "npc_count(start_unit_id, end_unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcCountFunction>());
                registry.Register("unit_id_2_obj_id", "unit_id_2_obj_id(unit_id)", new ExpressionFactoryHelper<Story.Functions.UnitId2ObjIdFunction>());
                registry.Register("obj_id_2_unit_id", "obj_id_2_unit_id(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjId2UnitIdFunction>());
                registry.Register("unit_id_2_unique_id", "unit_id_2_unique_id(unit_id)", new ExpressionFactoryHelper<Story.Functions.UnitId2UniqueIdFunction>());
                registry.Register("obj_id_2_unique_id", "obj_id_2_unique_id(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjId2UniqueIdFunction>());

                registry.Register("npc_get_formation", "npc_get_formation(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetFormationFunction>());
                registry.Register("npc_get_npc_type", "npc_get_npc_type(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetNpcTypeFunction>());
                registry.Register("npc_get_summoner_id", "npc_get_summoner_id(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonerIdFunction>());
                registry.Register("npc_get_summon_skill_id", "npc_get_summon_skill_id(unit_id)", new ExpressionFactoryHelper<Story.Functions.NpcGetSummonSkillIdFunction>());
                registry.Register("npc_find_impact_seq_by_id", "npc_find_impact_seq_by_id(unit_id, impact_id)", new ExpressionFactoryHelper<Story.Functions.NpcFindImpactSeqByIdFunction>());

                registry.Register("obj_get_formation", "obj_get_formation(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetFormationFunction>());
                registry.Register("obj_get_npc_type", "obj_get_npc_type(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetNpcTypeFunction>());
                registry.Register("obj_get_summoner_id", "obj_get_summoner_id(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonerIdFunction>());
                registry.Register("obj_find_impact_seq_by_id", "obj_find_impact_seq_by_id(obj_id, impact_id)", new ExpressionFactoryHelper<Story.Functions.ObjFindImpactSeqByIdFunction>());
                registry.Register("obj_get_summon_skill_id", "obj_get_summon_skill_id(obj_id)", new ExpressionFactoryHelper<Story.Functions.ObjGetSummonSkillIdFunction>());
                registry.Register("is_enemy", "is_enemy(camp1, camp2)", new ExpressionFactoryHelper<Story.Functions.IsEnemyFunction>());
                registry.Register("is_friend", "is_friend(camp1, camp2)", new ExpressionFactoryHelper<Story.Functions.IsFriendFunction>());

                registry.Register("get_position", "get_position(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionFunction>());
                registry.Register("get_position_x", "get_position_x(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionXFunction>());
                registry.Register("get_position_y", "get_position_y(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionYFunction>());
                registry.Register("get_position_z", "get_position_z(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetPositionZFunction>());
                registry.Register("get_camp", "get_camp(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetCampFunction>());
                registry.Register("is_combat_npc", "is_combat_npc(obj_id)", new ExpressionFactoryHelper<Story.Functions.IsCombatNpcFunction>());

                registry.Register("get_hp", "get_hp(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetHpFunction>());
                registry.Register("get_energy", "get_energy(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetEnergyFunction>());
                registry.Register("get_max_hp", "get_max_hp(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetMaxHpFunction>());
                registry.Register("get_max_energy", "get_max_energy(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetMaxEnergyFunction>());
                registry.Register("calc_offset", "calc_offset(obj_id, target_id, offset)", new ExpressionFactoryHelper<Story.Functions.CalcOffsetFunction>());
                registry.Register("calc_dir", "calc_dir(obj_id, target_id)", new ExpressionFactoryHelper<Story.Functions.CalcDirFunction>());
                registry.Register("obj_get", "obj_get(uniqueId, name[, default])", new ExpressionFactoryHelper<Story.Functions.ObjGetFunction>());
                registry.Register("get_table_id", "get_table_id(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetTableIdFunction>());
                registry.Register("get_level", "get_level(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetLevelFunction>());
                registry.Register("get_attr", "get_attr(obj_id, attr_name[, default])", new ExpressionFactoryHelper<Story.Functions.GetAttrFunction>());
                registry.Register("is_control_by_story", "is_control_by_story(obj_id)", new ExpressionFactoryHelper<Story.Functions.IsControlByStoryFunction>());
                registry.Register("can_cast_skill", "can_cast_skill(obj_id[, skill_id])", new ExpressionFactoryHelper<Story.Functions.CanCastSkillFunction>());
                registry.Register("is_under_control", "is_under_control(obj_id)", new ExpressionFactoryHelper<Story.Functions.IsUnderControlFunction>());
                registry.Register("get_leader_id", "get_leader_id([obj_id])", new ExpressionFactoryHelper<Story.Functions.GetLeaderIdFunction>());
                registry.Register("get_leader_table_id", "get_leader_table_id(obj_id)", new ExpressionFactoryHelper<Story.Functions.GetLeaderTableIdFunction>());
            }
        }

        private static bool s_IsInited = false;
    }
}
