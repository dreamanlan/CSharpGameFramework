﻿using System;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;

namespace GameFramework.GmCommands
{
    /// <summary>
    /// Gm剧情系统是在游戏剧情系统之上添加GM命令构成的特殊剧情系统。游戏剧情系统添加的命令与值都可以在Gm剧情脚本里使用（反之亦然）
    /// </summary>
    /// <remarks>
    /// 1、在剧情系统中注册的命令与值是共享的，亦即Gm剧情系统注册的Gm命令与值在正常剧情脚本里也可以使用！
    /// （在发布时此系统应该从客户端移除。）
    /// 2、剧情脚本与Gm剧情脚本不是一套体系，互不相干。
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
                //注册Gm命令
                StoryCommandManager.Instance.RegisterCommandFactory("setposition", "setposition command", new StoryCommandFactoryHelper<SetPositionCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("levelto", "levelto command", new StoryCommandFactoryHelper<LevelToCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("full", "full command", new StoryCommandFactoryHelper<FullCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("clearequipments", "clearequipments command", new StoryCommandFactoryHelper<ClearEquipmentsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("addequipment", "addequipment command", new StoryCommandFactoryHelper<AddEquipmentCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("clearskills", "clearskills command", new StoryCommandFactoryHelper<ClearSkillsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("addskill", "addskill command", new StoryCommandFactoryHelper<AddSkillCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("clearbuffs", "clearbuffs command", new StoryCommandFactoryHelper<ClearBuffsCommand>());
                StoryCommandManager.Instance.RegisterCommandFactory("addbuff", "addbuff command", new StoryCommandFactoryHelper<AddBuffCommand>());

                //注册值与函数处理
            }
        }

        private static bool s_IsInited = false;
    }
}
