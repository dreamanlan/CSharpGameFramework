using System;
using System.Collections.Generic;
using ScriptRuntime;
using DotnetStoryScript;

namespace ScriptableFramework.GmCommands
{
    /// <summary>
    /// The Gm plot system is a special plot system composed of adding GM commands on top of the game plot system. The commands and values added by the game plot system can be used in the Gm plot script (and vice versa)
    /// </summary>
    /// <remarks>
    /// 1. The commands and values registered in the plot system are shared, that is, the Gm commands and values registered in the Gm plot system can also be used in normal plot scripts!
    /// (This system should be removed from the client when publishing.)
    /// 2. The plot script and the Gm plot script are not a system and have nothing to do with each other.
    /// </remarks>
    internal sealed class GmStorySystem
    {
        internal void Init(UserThread userThread)
        {
            StaticInit();
            m_CurUserThread = userThread;
        }

        internal int ActiveStoryCount
        {
            get {
                return m_StoryLogicInfos.Count;
            }
        }
        internal StrBoxedValueDict GlobalVariables
        {
            get { return m_GlobalVariables; }
        }
        internal void Reset()
        {
            m_GlobalVariables.Clear();
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (null != info) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
            m_StoryLogicInfos.Clear();
        }
        internal void LoadStory(string file)
        {
            m_StoryInstancePool.Clear();
            m_ConfigManager.Clear();
            m_ConfigManager.LoadStory(file, 0, string.Empty);
        }
        internal void LoadStoryText(byte[] bytes)
        {
            m_StoryInstancePool.Clear();
            m_ConfigManager.Clear();
            m_ConfigManager.LoadStoryText(string.Empty, bytes, 0, string.Empty);
        }
        internal StoryInstance GetStory(string storyId)
        {
            return GetStoryInstance(storyId);
        }
        internal void StartStory(string storyId)
        {
            StoryInstance inst = NewStoryInstance(storyId);
            if (null != inst) {
                StopStory(storyId);
                m_StoryLogicInfos.Add(inst);
                inst.Context = m_CurUserThread;
                inst.GlobalVariables = m_GlobalVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
            }
        }
        internal void StopStory(string storyId)
        {
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    m_StoryLogicInfos.RemoveAt(index);
                }
            }
        }
        internal void Tick()
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
        internal void SendMessage(string msgId, params object[] args)
        {
            int ct = m_StoryLogicInfos.Count;
            for (int ix = ct - 1; ix >= 0; --ix) {
                StoryInstance info = m_StoryLogicInfos[ix];
                info.SendMessage(msgId, args);
            }
        }

        private StoryInstance NewStoryInstance(string storyId)
        {
            StoryInstance instance = GetStoryInstance(storyId);
            if (null == instance) {
                instance = m_ConfigManager.NewStoryInstance(storyId, 0);
                if (instance == null) {
                    ScriptableFramework.LogSystem.Error("Can't load story config, story:{0} !", storyId);
                    return null;
                }

                AddStoryInstance(storyId, instance);
                return instance;
            }
            else {
                return instance;
            }
        }
        private void AddStoryInstance(string storyId, StoryInstance info)
        {
            if (!m_StoryInstancePool.ContainsKey(storyId)) {
                m_StoryInstancePool.Add(storyId, info);
            }
            else {
                m_StoryInstancePool[storyId] = info;
            }
        }
        private StoryInstance GetStoryInstance(string storyId)
        {
            StoryInstance info;
            m_StoryInstancePool.TryGetValue(storyId, out info);
            return info;
        }

        private StrBoxedValueDict m_GlobalVariables = new StrBoxedValueDict();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private UserThread m_CurUserThread = null;

        private StoryConfigManager m_ConfigManager = StoryConfigManager.NewInstance();

        internal static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;
                //register GM command

                //register value or functions
            }
        }

        private static bool s_IsInited = false;
    }
}
