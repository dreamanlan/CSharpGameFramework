using System;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;

namespace GameFramework.GmCommands
{
    /// <summary>
    /// The Gm plot system is a special plot system composed of adding GM commands on top of the game plot system. The commands and values added by the game plot system can be used in the Gm plot script (and vice versa)
    /// </summary>
    /// <remarks>
    /// 1. The commands and values registered in the plot system are shared, that is, the Gm commands and values registered in the Gm plot system can also be used in normal plot scripts!
    /// (This system should be removed from the client when publishing.)
    /// 2. The plot script and the Gm plot script are not a system and have nothing to do with each other.
    /// </remarks>
    public sealed class GmStorySystem
    {
        public void Init(Scene scene)
        {
            StaticInit();
            m_CurScene = scene;
        }

        public int ActiveStoryCount
        {
            get {
                return m_StoryLogicInfos.Count;
            }
        }
        public StrBoxedValueDict GlobalVariables
        {
            get { return m_GlobalVariables; }
        }
        public void Reset()
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
        public void LoadStory(string file)
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
        public void StartStory(string storyId)
        {
            StoryInstance inst = NewStoryInstance(storyId);
            if (null != inst) {
                StopStory(storyId);
                m_StoryLogicInfos.Add(inst);
                inst.Context = m_CurScene;
                inst.GlobalVariables = m_GlobalVariables;
                inst.Start();

                LogSystem.Info("StartStory {0}", storyId);
            }
        }
        public void StopStory(string storyId)
        {
            int count = m_StoryLogicInfos.Count;
            for (int index = count - 1; index >= 0; --index) {
                StoryInstance info = m_StoryLogicInfos[index];
                if (info.StoryId == storyId) {
                    m_StoryLogicInfos.RemoveAt(index);
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
        public void SendMessage(string msgId, params object[] args)
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
                    GameFramework.LogSystem.Error("Can't load story config, story:{0} !", storyId);
                    return null;
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

        private StrBoxedValueDict m_GlobalVariables = new StrBoxedValueDict();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();

        private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
        private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
        private Scene m_CurScene = null;

        private StoryConfigManager m_ConfigManager = StoryConfigManager.NewInstance();

        public static void StaticInit()
        {
            if (!s_IsInited) {
                s_IsInited = true;
                //register GM commands
                StoryCommandManager.Instance.RegisterCommandFactory("setposition", "setposition command", new StoryCommandFactoryHelper<SetPositionCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("levelto", "levelto command", new StoryCommandFactoryHelper<LevelToCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("full", "full command", new StoryCommandFactoryHelper<FullCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("clearequipments", "clearequipments command", new StoryCommandFactoryHelper<ClearEquipmentsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("addequipment", "addequipment command", new StoryCommandFactoryHelper<AddEquipmentCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("clearskills", "clearskills command", new StoryCommandFactoryHelper<ClearSkillsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("addskill", "addskill command", new StoryCommandFactoryHelper<AddSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("clearbuffs", "clearbuffs command", new StoryCommandFactoryHelper<ClearBuffsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("addbuff", "addbuff command", new StoryCommandFactoryHelper<AddBuffCommand>());

                //register value or functions
            }
        }

        private static bool s_IsInited = false;
    }
}
