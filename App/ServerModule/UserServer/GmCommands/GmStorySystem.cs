using System;
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
  internal sealed class GmStorySystem
  {
    internal void Init(UserThread userThread)
    {
      StaticInit();
      m_CurUserThread = userThread;
    }

    internal int ActiveStoryCount
    {
      get
      {
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
    
    private List<StoryInstance> m_StoryLogicInfos = new List<StoryInstance>();
    private Dictionary<string, StoryInstance> m_StoryInstancePool = new Dictionary<string, StoryInstance>();
    private UserThread m_CurUserThread = null;

    private StoryConfigManager m_ConfigManager = StoryConfigManager.NewInstance();
    
    internal static void StaticInit()
    {
      if (!s_IsInited) {
        s_IsInited = true;
        //注册Gm命令

        //注册值与函数处理
      }
    }

    private static bool s_IsInited = false;
  }
}
