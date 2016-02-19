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
        public void PreloadStory(string file)
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
        public void PreloadNamespacedStory(string _namespace, string file)
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
                inst.Context = m_UserThread;
                inst.GlobalVariables = m_GlobalVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
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

        private StoryInstance NewStoryInstance(string storyId, string _namespace, bool logIfNotFound, params string[] overloadFiles)
        {
            if (!string.IsNullOrEmpty(_namespace)) {
                storyId = string.Format("{0}:{1}", _namespace, storyId);
            }
            StoryInstance instance = GetStoryInstance(storyId);
            if (null == instance) {
                string[] filePath;
                if (overloadFiles.Length <= 0) {                    
                    TableConfig.UserScript cfg = TableConfig.UserScriptProvider.Instance.GetUserScript(storyId);
                    if (null != cfg) {
                        filePath = new string[1];
                        filePath[0] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + cfg.DslFile);
                    } else {
                        if (logIfNotFound)
                            LogSystem.Error("Can't find story config, story:{0} !", storyId);
                        return null;
                    }
                } else {
                    int ct = overloadFiles.Length;
                    filePath = new string[ct];
                    for (int i = 0; i < ct; i++) {
                        filePath[i] = HomePath.GetAbsolutePath(FilePathDefine_Server.C_DslPath + overloadFiles[i]);
                    }
                }
                StoryConfigManager.Instance.LoadStories(0, _namespace, filePath);
                instance = StoryConfigManager.Instance.NewStoryInstance(storyId, 0);
                if (instance == null) {
                    if (logIfNotFound)
                        LogSystem.Error("Can't load story config, story:{0} !", storyId);
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

        internal ServerStorySystem() { }

        private Dictionary<string, object> m_GlobalVariables = new Dictionary<string, object>();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private UserThread m_UserThread = null;
        
        internal static void ThreadInitMask()
        {
            StoryCommandManager.ThreadCommandGroupsMask = (ulong)((1 << (int)StoryCommandGroupDefine.GM) + (1 << (int)StoryCommandGroupDefine.USER));
            StoryValueManager.ThreadValueGroupsMask = (ulong)((1 << (int)StoryValueGroupDefine.GM) + (1 << (int)StoryValueGroupDefine.USER));
        }
        internal static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;

                //注册剧情命令
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "startstory", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "stopstory", new StoryCommandFactoryHelper<Story.Commands.StopStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "waitstory", new StoryCommandFactoryHelper<Story.Commands.WaitStoryCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "firemessage", new StoryCommandFactoryHelper<Story.Commands.FireMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "waitallmessage", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "waitallmessagehandler", new StoryCommandFactoryHelper<Story.Commands.WaitAllMessageHandlerCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendserverstorymessage", new StoryCommandFactoryHelper<Story.Commands.SendServerStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendclientstorymessage", new StoryCommandFactoryHelper<Story.Commands.SendClientStoryMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "publishgfxevent", new StoryCommandFactoryHelper<Story.Commands.PublishGfxEventCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendgfxmessage", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory(StoryCommandGroupDefine.USER, "sendgfxmessagewithtag", new StoryCommandFactoryHelper<Story.Commands.SendGfxMessageWithTagCommand>());

                //注册值与函数处理
                StoryValueManager.Instance.RegisterValueFactory(StoryValueGroupDefine.USER, "getuserinfo", new StoryValueFactoryHelper<Story.Values.GetUserInfoValue>());
            }
        }

        private static bool s_IsInited = false;
    }
}
