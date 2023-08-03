using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using GameFrameworkData;
using GameFramework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CSharpCenterClient;
using GameFrameworkMessage;

namespace Lobby
{
  /// <summary>
  /// 玩家数据处理调度器，玩家数据请求将被放到并行的若干个线程里进行处理。
  /// 有2类线程：
  /// 1、由DispatchAction调用发起的操作，此时执行线程无法指定。
  /// 2、调度器内部实例化一个线程，用以进行必须在一个线程里进行的操作。（未对外提供接口，目前假定用于为1中操作需要有序的服务。）
  /// </summary>
  /// <remarks>
  /// 这个类采用多线程操作数据，所有成员都不能假定其工作的线程。
  /// 请注意四条约束：
  /// 1、UserInfo一旦实例化，内存不会被释放（只回收到池子里供重用，RoomInfo也是这样）。
  /// 2、对于只操作小于等于机器字长的数据的函数，不加锁（操作本来就是原子的）。
  /// 3、对于操作的数据大于机器字长的并且必须保证事务性更新的，需要加锁，每个UserInfo带有一个Lock属性（mono的读写锁有死锁bug，这里直接用普通锁）。UserInfo上持有的具有复杂结构的属性，
  /// 如果该结构/类里涉及集合操作，应该对该结构/类的数据进行封装并通过内部加锁或lockfree机制保证多线程操作安全。
  /// 4、此类方法除Get开头的方法外通常通过DispatchAction调用发起，具体线程分配考虑如下：
  ///    a、玩家进入房间后基本上只有房间线程会修改玩家数据，故RoomProcessThread会直接修改玩家数据（通常都是简单数据或状态修改）。
  ///    b、玩家在大厅内但没有进入房间时的操作由Node发消息到Lobby，然后经DispatchAction调用各方法进行处理。
  ///    c、玩家在游戏中RoomServer会需要修改玩家数据，此时会发消息到Lobby，然后经DispatchAction调用各方法进行处理。
  /// </remarks>
  internal sealed class UserProcessScheduler : MyServerTaskDispatcher
  {
    internal UserProcessScheduler()
      : base(12, true, 10, 4096)
    {
      m_Thread = new MyServerThread();      
      m_Thread.TickSleepTime = 10;
      m_Thread.ActionNumPerTick = 10240; 
      m_Thread.OnTickEvent += this.OnTick;

      m_NodeMessageManager.Init(3, 10, 4096);
    }
    internal void Start()
    {
      m_Thread.Start();
      m_NodeMessageManager.Start();
    }
    internal void Stop()
    {
      m_NodeMessageManager.Stop();
      StopTaskThreads();
      m_Thread.Stop();
    }
    internal void DispatchJsonMessage(uint seq, ulong sourceHandle, ulong destHandle, byte[] data)
    {
      m_NodeMessageManager.DispatchMessage(seq, sourceHandle, destHandle, data);
    }
    //--------------------------------------------------------------------------------------------------------------------------
    //供外部直接调用的方法，需要保证多线程安全。
    //--------------------------------------------------------------------------------------------------------------------------
    internal void VisitUsers(MyAction<UserInfo> visitor)
    {
      foreach (KeyValuePair<ulong,UserInfo> pair in m_UserInfos) {
        UserInfo userInfo = pair.Value;
        visitor(userInfo);
      }
    }
    //当前在线玩家中根据Nickname查找UserGuid
    internal ulong GetGuidByNickname(string nickname)
    {
      ulong guid = 0;
      m_GuidByNickname.TryGetValue(nickname, out guid);
      return guid;
    }
    internal UserInfo GetUserInfo(ulong guid)
    {
      UserInfo info = null;
      m_UserInfos.TryGetValue(guid, out info);
      return info;
    }
    internal int GetUserCount()
    {
      return m_UserInfos.Count;
    }
    internal void DoCloseServers()
    {
      CenterClientApi.SendCommandByName("NodeJs1", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs2", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs3", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs4", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs5", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs6", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs7", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs8", "QuitNodeJs");
      CenterClientApi.SendCommandByName("NodeJs9", "QuitNodeJs");
      CenterClientApi.SendCommandByName("RoomSvr1", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr2", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr3", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr4", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr5", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr6", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr11", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr12", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr13", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr14", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr15", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr16", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr21", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr22", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr23", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr24", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr25", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr26", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr31", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr32", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr33", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr34", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr35", "QuitRoomServer");
      CenterClientApi.SendCommandByName("RoomSvr36", "QuitRoomServer");
      CenterClientApi.SendCommandByName("GmServer", "QuitGmServer");
    }
    //--------------------------------------------------------------------------------------------------------------------------
    //供外部通过DispatchAction调用的方法，会在不同线程执行，需要保证多线程安全。
    //--------------------------------------------------------------------------------------------------------------------------
    internal void RequestEnterScene(Msg_LB_RequestEnterScene msg)
    {
      //添加/更新玩家数据
      UserInfo info = AddOrUpdateUserInfo(msg.BaseInfo, msg.User, UserInfo.c_LifeTimeWaitStartGame);

      //开始进野外流程
      RoomProcessThread roomProcess = LobbyServer.Instance.RoomProcessThread;
      roomProcess.QueueAction(roomProcess.RequestEnterScene, info.Guid, msg.SceneId, msg.WantRoomId, msg.FromSceneId);
    }

    internal void BroadcastText(ulong guid, BroadcastType type, string content, int roll_ct)
    {
      UserInfo info = GetUserInfo(guid);
      if (null != info) {
        Msg_BL_BroadcastText builder = new Msg_BL_BroadcastText();
        builder.BroadcastType = (int)type;
        builder.Content = content;
        builder.RollCount = roll_ct;
        LobbyServer.Instance.UserChannel.Send(info.UserSvrName, builder);
      }
    }
    //--------------------------------------------------------------------------------------------------------------------------
    //这些方法是一些工具方法，后面需要重新考虑并发相关的处理。
    //--------------------------------------------------------------------------------------------------------------------------
    internal UserInfo AddOrUpdateUserInfo(Msg_LB_BigworldUserBaseInfo baseUserInfo, Msg_LR_RoomUserInfo roomUserInfo, int leftLifeTime)
    {
      ulong guid = roomUserInfo.Guid;
      UserInfo info;
      if (!m_UserInfos.TryGetValue(guid, out info)) {
        info = NewUserInfo();
        info.Key = GenerateKey();
      }
      info.BaseUserInfo = baseUserInfo;
      info.RoomUserInfo = roomUserInfo;

      info.LeftLife = leftLifeTime;
      info.CurrentState = UserState.Online;

      m_UserInfos.AddOrUpdate(guid, info, (g, u) => info);
      m_GuidByNickname.AddOrUpdate(info.Nickname, guid, (n, g) => guid);

      ActivateUserGuid(guid);
      return info;
    }
    private bool IsPveScene(int sceneId)
    {
      bool ret = false;
      TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(sceneId);
      if (null != cfg && (int)SceneTypeEnum.Story == cfg.type) {
        ret = true;
      }
      return ret;
    }

    //--------------------------------------------------------------------------------------------------------------------------
    //后面的方法都是在内部线程执行的方法，不涉及多线程操作，不用加锁，串行执行。
    //--------------------------------------------------------------------------------------------------------------------------
    private void OnTick()
    {
      long curTime = TimeUtility.GetLocalMilliseconds();

      int elapsedTickTime = m_Thread.TickSleepTime;
      if (m_LastTickTime > 0) {
        elapsedTickTime = (int)(curTime - m_LastTickTime);
        if (elapsedTickTime < 0) {
          elapsedTickTime = m_Thread.TickSleepTime;
        }
      }
      m_LastTickTime = curTime;

      if (m_LastLogTime + 60000 < curTime) {
        m_LastLogTime = curTime;

        DebugPoolCount((string msg) => {
          LogSys.Log(ServerLogType.INFO, "UserProcessScheduler.DispatchActionQueue {0}", msg);
        });
        DebugActionCount((string msg) => {
          LogSys.Log(ServerLogType.MONITOR, "UserProcessScheduler.DispatchActionQueue {0}", msg);
        });
        m_Thread.DebugPoolCount((string msg) => {
          LogSys.Log(ServerLogType.INFO, "UserProcessScheduler.ThreadActionQueue {0}", msg);
        });
        LogSys.Log(ServerLogType.MONITOR, "UserProcessScheduler.ThreadActionQueue Current Action {0}", m_Thread.CurActionNum);

        m_NodeMessageManager.TickMonitor();

        LogSys.Log(ServerLogType.MONITOR, "Lobby User Count:{0} ElapsedTickTime:{1}", m_ActiveUserGuids.Count, elapsedTickTime);
      }

      m_DeactiveUserGuids.Clear();
      foreach (var guidPair in m_ActiveUserGuids) {
        ulong guid = guidPair.Key;
        UserInfo user = GetUserInfo(guid);
        if (user == null) {
          m_DeactiveUserGuids.Add(guid);
        } else {
          user.LeftLife -= elapsedTickTime;
          if (user.LeftLife <= 0) {
            if (UserState.Room != user.CurrentState) {
              if (user.Room != null) {
                RoomProcessThread roomProcess = LobbyServer.Instance.RoomProcessThread;
                roomProcess.QueueAction(roomProcess.QuitRoom, guid, true, (ulong)0);
              }
              m_DeactiveUserGuids.Add(guid);
            } else {
              user.LeftLife = UserInfo.c_NextLifeTime;
            }
          }
        }
      }
      int tmpValue = 0;
      foreach (ulong guid in m_DeactiveUserGuids) {
        m_ActiveUserGuids.TryRemove(guid, out tmpValue);
        AddWaitRecycleUser(guid);
      }

      m_DeactiveUserGuids.Clear();
      foreach (ulong guid in m_WaitRecycleUsers) {
        UserInfo user = GetUserInfo(guid);
        if (user == null) {
          m_DeactiveUserGuids.Add(guid);
        } else {       
          FreeKey(user.Key);
          ulong g = 0;
          m_GuidByNickname.TryRemove(user.Nickname, out g);
          UserInfo tmp;
          m_UserInfos.TryRemove(guid, out tmp);
          RecycleUserInfo(user);
          m_DeactiveUserGuids.Add(guid);
        }
      }
      foreach (ulong guid in m_DeactiveUserGuids) {
        m_WaitRecycleUsers.Remove(guid);
      }     
    }
    private void AddWaitRecycleUser(ulong guid)
    {
      if (!m_WaitRecycleUsers.Contains(guid)) {
        m_WaitRecycleUsers.Add(guid);
      }
    }
    //-----------------------------------------------------------------------------
    //这几个方法现在允许在多个线程里同时执行
    //-----------------------------------------------------------------------------
    private UserInfo NewUserInfo()
    {
      UserInfo info = null;
      if (m_UnusedUserInfos.IsEmpty) {
        info = new UserInfo();
      } else {
        if (!m_UnusedUserInfos.TryDequeue(out info)) {
          info = new UserInfo();
        } else {
          info.IsRecycled = false;
        }
      }
      return info;
    }
    private void RecycleUserInfo(UserInfo info)
    {
      info.IsRecycled = true;
      info.Reset();
      m_UnusedUserInfos.Enqueue(info);
    }
    private void ActivateUserGuid(ulong guid)
    {      
      m_ActiveUserGuids.AddOrUpdate(guid, 1, (g, v) => 1);
    }
    private uint GenerateKey()
    {
      uint key = 0;
      for (; ; ) {
        key = (uint)(m_Random.NextDouble() * 0x0fffffff);
        if (!m_Keys.ContainsKey(key)) {
          m_Keys.AddOrUpdate(key, true, (k, v) => true);
          break;
        }
      }
      return key;
    }
    private void FreeKey(uint key)
    {
      bool nouse = false;
      m_Keys.TryRemove(key, out nouse);
    }

    private NodeMessageManager m_NodeMessageManager = new NodeMessageManager();

    private ConcurrentDictionary<ulong, UserInfo> m_UserInfos = new ConcurrentDictionary<ulong, UserInfo>();
    private ConcurrentDictionary<string, ulong> m_GuidByNickname = new ConcurrentDictionary<string, ulong>();
    private ConcurrentQueue<UserInfo> m_UnusedUserInfos = new ConcurrentQueue<UserInfo>();
    private ConcurrentDictionary<uint, bool> m_Keys = new ConcurrentDictionary<uint, bool>();

    private ConcurrentDictionary<ulong, int> m_ActiveUserGuids = new ConcurrentDictionary<ulong, int>();
    private List<ulong> m_DeactiveUserGuids = new List<ulong>();
    private List<ulong> m_WaitRecycleUsers = new List<ulong>();

    private Random m_Random = new Random();
    private MyServerThread m_Thread = null;

    private long m_LastTickTime = 0;
    private long m_LastLogTime = 0;
  }
}