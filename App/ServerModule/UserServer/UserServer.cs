using System;
using System.IO;
using System.Text;
using CSharpCenterClient;
using Messenger;
using GameFramework;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using GameFrameworkData;
using System.Threading;
using GameFrameworkMessage;

namespace GameFramework
{
    internal partial class UserServer
    {
        #region Singleton
        private static UserServer s_Instance = new UserServer();
        internal static UserServer Instance
        {
            get
            {
                return s_Instance;
            }
        }
        #endregion

        internal Messenger.PBChannel BigworldChannel
        {
            get { return m_BigworldChannel; }
        }
        internal Messenger.PBChannel DataCacheChannel
        {
            get { return m_DataCacheChannel; }
        }
        internal QueueingThread QueueingThread
        {
            get { return m_QueueingThread; }
        }
        internal DataCacheThread DataCacheThread
        {
            get { return m_DataCacheThread; }
        }
        internal GlobalProcessThread GlobalProcessThread
        {
            get { return m_GlobalProcessThread; }
        }
        internal UserProcessScheduler UserProcessScheduler
        {
            get { return m_UserProcessScheduler; }
        }
        internal bool IsUnknownServerOnBigworld(int handle)
        {
            return !IsNodeOnBigworld(handle) && !IsLobbyOnBigworld(handle);
        }
        internal bool IsNodeOnBigworld(int handle)
        {
            return m_NodeHandlesOnBigworld.Contains(handle);
        }
        internal bool IsLobbyOnBigworld(int handle)
        {
            return m_BigworldChannel.DefaultServiceHandle == handle;
        }
        internal bool IsUnknownServer(int handle)
        {
            return !IsNode(handle) && !IsDataCache(handle);
        }
        internal bool IsNode(int handle)
        {
            return m_NodeHandles.Contains(handle);
        }
        internal bool IsDataCache(int handle)
        {
            return m_DataCacheChannel.DefaultServiceHandle == handle;
        }
        internal void HighlightPrompt(UserInfo user, int dictId, params object[] args)
        {
            //0--null 1--int 2--float 3--string      
            GameFrameworkMessage.Msg_CLC_StoryMessage protoData = new GameFrameworkMessage.Msg_CLC_StoryMessage();
            protoData.m_MsgId = string.Format("highlightprompt{0}", args.Length);
            GameFrameworkMessage.Msg_CLC_StoryMessage.MessageArg item0 = new GameFrameworkMessage.Msg_CLC_StoryMessage.MessageArg();
            item0.val_type = LobbyArgType.INT;
            item0.str_val = dictId.ToString();
            protoData.m_Args.Add(item0);
            for (int i = 0; i < args.Length; ++i) {
                GameFrameworkMessage.Msg_CLC_StoryMessage.MessageArg item = new GameFrameworkMessage.Msg_CLC_StoryMessage.MessageArg();
                item.val_type = LobbyArgType.STRING;
                item.str_val = args[i].ToString();
                protoData.m_Args.Add(item);
            }
            NodeMessage msg = new NodeMessage(LobbyMessageDefine.Msg_CLC_StoryMessage, user.Guid);
            msg.m_ProtoData = protoData;
            NodeMessageDispatcher.SendNodeMessage(user.NodeName, msg);
        }
        internal void SendStoryMessage(UserInfo user, string msgId, params object[] args)
        {
            //0--null 1--int 2--float 3--string
            GameFrameworkMessage.Msg_CLC_StoryMessage protoData = new GameFrameworkMessage.Msg_CLC_StoryMessage();
            protoData.m_MsgId = msgId;
            for (int i = 0; i < args.Length; ++i) {
                object arg = args[i];
                GameFrameworkMessage.Msg_CLC_StoryMessage.MessageArg item = new GameFrameworkMessage.Msg_CLC_StoryMessage.MessageArg();
                if (null != arg) {
                    if (arg is int) {
                        item.val_type = LobbyArgType.INT;
                    } else if (arg is float) {
                        item.val_type = LobbyArgType.FLOAT;
                    } else {
                        item.val_type = LobbyArgType.STRING;
                    }
                    item.str_val = arg.ToString();
                } else {
                    item.val_type = LobbyArgType.NULL;
                    item.str_val = "";
                }
                protoData.m_Args.Add(item);
            }
            NodeMessage msg = new NodeMessage(LobbyMessageDefine.Msg_CLC_StoryMessage, user.Guid);
            msg.m_ProtoData = protoData;
            NodeMessageDispatcher.SendNodeMessage(user.NodeName, msg);
        }
        internal void ForwardToBigworld(UserInfo user, byte[] msgData)
        {
            if (null != user && null != msgData) {
                ForwardToBigworld(user.NodeName, msgData);
            }
        }
        internal void ForwardToBigworld(string nodeName, byte[] msgData)
        {
            try {
                if (!string.IsNullOrEmpty(nodeName) && null != msgData) {
                    byte[] data = msgData;
                    Msg_LBL_Message builder = new Msg_LBL_Message();
                    builder.MsgType = Msg_LBL_Message.MsgTypeEnum.Node;
                    builder.TargetName = nodeName;
                    builder.Data = data;
                    m_BigworldChannel.Send(builder);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        internal void TransmitToBigworld(object msg)
        {
            try {
                if (null != msg) {
                    byte[] data = m_BigworldChannel.Encode(msg);
                    Msg_LBL_Message builder = new Msg_LBL_Message();
                    builder.MsgType = Msg_LBL_Message.MsgTypeEnum.Room;
                    builder.TargetName = string.Empty;
                    builder.Data = data;
                    m_BigworldChannel.Send(builder);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        internal bool WaitQuit
        {
            get { return m_WaitQuit; }
        }

        private void Init(string[] args)
        {
            m_NameHandleCallback = this.OnNameHandleChanged;
            m_MsgCallback = this.OnMessage;
            m_MsgResultCallback = this.OnMessageResultCallback;
            m_CmdCallback = this.OnCommand;
            m_LogHandler = this.OnCenterLog;
            CenterHubApi.SetCenterLogHandler(m_LogHandler);
            CenterHubApi.Init("userserver", args.Length, args, m_NameHandleCallback, m_MsgCallback, m_MsgResultCallback, m_CmdCallback);

            LogSys.Init("./config/logconfig.xml");
            UserServerConfig.Init();

            GlobalVariables.Instance.IsClient = false;

            string key = "防君子不防小人";
            byte[] xor = Encoding.UTF8.GetBytes(key);

            ResourceReadProxy.OnReadAsArray = ((string filePath) => {
                byte[] buffer = null;
                try {
                    buffer = File.ReadAllBytes(filePath);
                } catch (Exception e) {
                    LogSys.Log(LOG_TYPE.ERROR, "Exception:{0}\n{1}", e.Message, e.StackTrace);
                    return null;
                }
                return buffer;
            });
            LogSystem.OnOutput += (Log_Type type, string msg) => {
                switch (type) {
                    case Log_Type.LT_Debug:
                        LogSys.Log(LOG_TYPE.DEBUG, msg);
                        break;
                    case Log_Type.LT_Info:
                        LogSys.Log(LOG_TYPE.INFO, msg);
                        break;
                    case Log_Type.LT_Warn:
                        LogSys.Log(LOG_TYPE.WARN, msg);
                        break;
                    case Log_Type.LT_Error:
                    case Log_Type.LT_Assert:
                        LogSys.Log(LOG_TYPE.ERROR, msg);
                        break;
                }
            };

            LoadData();
            LogSys.Log(LOG_TYPE.INFO, "Init Config ...");
            s_Instance = this;
            InstallMessageHandlers();
            LogSys.Log(LOG_TYPE.INFO, "Init Messenger ...");
            m_DataCacheThread.Init(m_DataCacheChannel);
            LogSys.Log(LOG_TYPE.INFO, "Init DataCache ...");
            Start();
            LogSys.Log(LOG_TYPE.INFO, "Start Threads ...");
        }
        private void Loop()
        {
            try {
                while (CenterHubApi.IsRun()) {
                    long curTime = TimeUtility.GetLocalMilliseconds();
                    if (m_LastTickTime != 0) {
                        long elapsedTickTime = curTime - m_LastTickTime;
                        if (elapsedTickTime > c_WarningTickTime) {
                            LogSys.Log(LOG_TYPE.MONITOR, "GameFramework Network Tick:{0}", curTime - m_LastTickTime);
                        }
                    }
                    m_LastTickTime = curTime;

                    CenterHubApi.Tick();
                    Thread.Sleep(10);
                    if (m_WaitQuit) {
                        if (m_GlobalProcessThread.LastSaveFinished && m_UserProcessScheduler.LastSaveFinished && m_QuitFinish == false) {
                            //全局数据和玩家数据存储完毕 
                            int saveReqCount = m_DataCacheThread.CalcSaveRequestCount();
                            LogSys.Log(LOG_TYPE.MONITOR, "QuitStep_1. GlobalData and UserData last save done. SaveRequestCount:{0}", saveReqCount);
                            if (saveReqCount > 0) {
                                //等待5s
                                long startTime = TimeUtility.GetLocalMilliseconds();
                                while (startTime + 5000 > TimeUtility.GetLocalMilliseconds()) {
                                    CenterClientApi.Tick();
                                    Thread.Sleep(10);
                                }
                            } else {
                                //通知关闭DataCache
                                m_QuitFinish = true;
                                LogSys.Log(LOG_TYPE.MONITOR, "QuitStep_2. Notice DataCache to quit.");
                                CenterClientApi.SendCommandByName("DataCache", "QuitDataStore");
                                //等待10s
                                long startTime = TimeUtility.GetLocalMilliseconds();
                                while (startTime + 10000 > TimeUtility.GetLocalMilliseconds()) {
                                    CenterClientApi.Tick();
                                    Thread.Sleep(10);
                                }
                                //关闭GameFramework
                                LogSys.Log(LOG_TYPE.MONITOR, "QuitStep_3. LastSaveDone. GameFramework quit...");
                                CenterClientApi.Quit();
                            }
                        } else {
                            if (curTime - m_LastWaitQuitTime > c_WaitQuitTimeInterval) {
                                m_WaitQuit = false;
                                LogSys.Log(LOG_TYPE.MONITOR, "QuitStep_-1. Reset m_WaitQuit to false.");
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "GameFramework.Loop throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void Release()
        {
            Stop();
            CenterHubApi.Release();
            LogSys.Release();
        }
        private void OnCenterLog(string msg, int size)
        {
            LogSys.Log(LOG_TYPE.INFO, "{0}", msg);
        }
        private void OnNameHandleChanged(int worldId, bool addOrUpdate, string name, int handle)
        {
            try {
                if (worldId == UserServerConfig.WorldId) {
                    m_DataCacheChannel.OnUpdateNameHandle(addOrUpdate, name, handle);
                    if (!addOrUpdate) {
                        m_NodeHandles.Remove(handle);
                    }
                } else {
                    m_BigworldChannel.OnUpdateNameHandle(addOrUpdate, name, handle);
                    if (!addOrUpdate) {
                        m_NodeHandlesOnBigworld.Remove(handle);
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnCommand(int worldId, int src, int dest, string command)
        {
            const string c_QuitLobby = "QuitLobby";
            const string c_ReloadConfig = "ReloadConfig";
            try {
                if (worldId == UserServerConfig.WorldId) {
                    if (0 == command.CompareTo(c_QuitLobby)) {
                        LogSys.Log(LOG_TYPE.MONITOR, "receive {0} command, save data and then quitting ...", command);
                        if (!m_WaitQuit) {
                            //收到关闭服务器指令,退出前保存数据
                            m_UserProcessScheduler.DefaultUserThread.QueueAction(m_UserProcessScheduler.DoLastSaveUserData);
                            m_GlobalProcessThread.QueueAction(m_GlobalProcessThread.DoLastSaveGlobalData);
                            m_LastWaitQuitTime = TimeUtility.GetLocalMilliseconds();
                            m_WaitQuit = true;
                        }
                    } else if (0 == command.CompareTo(c_ReloadConfig)) {
                        CenterHubApi.ReloadConfigScript();
                        UserServerConfig.Init();
                        LogSys.Log(LOG_TYPE.WARN, "receive {0} command.", command);
                    }
                } else {

                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnMessage(int worldId, uint seq, int source_handle, int dest_handle,
            IntPtr data, int len)
        {
            try {
                if (worldId == UserServerConfig.WorldId) {
                    if (IsUnknownServer(source_handle)) {
                        StringBuilder sb = new StringBuilder(256);
                        if (CenterHubApi.TargetName(worldId, source_handle, sb, 256)) {
                            string name = sb.ToString();
                            if (name.StartsWith("NodeJs")) {
                                m_NodeHandles.Add(source_handle);
                            }
                        }
                    }
                    byte[] bytes = new byte[len];
                    Marshal.Copy(data, bytes, 0, len);
                    if (IsNode(source_handle)) {
                        if (!m_WaitQuit) {
                            m_UserProcessScheduler.DispatchJsonMessage(false, seq, source_handle, dest_handle, bytes);
                        }
                    } else if (IsDataCache(source_handle)) {
                        m_DataCacheThread.DispatchAction(m_DataCacheChannel.Dispatch, source_handle, seq, bytes);
                    }
                } else {
                    if (IsUnknownServerOnBigworld(source_handle)) {
                        StringBuilder sb = new StringBuilder(256);
                        if (CenterHubApi.TargetName(worldId, source_handle, sb, 256)) {
                            string name = sb.ToString();
                            if (name.StartsWith("NodeJs")) {
                                m_NodeHandlesOnBigworld.Add(source_handle);
                            }
                        }
                    }
                    byte[] bytes = new byte[len];
                    Marshal.Copy(data, bytes, 0, len);
                    if (IsNodeOnBigworld(source_handle)) {
                        if (!m_WaitQuit) {
                            m_UserProcessScheduler.DispatchJsonMessage(true, seq, source_handle, dest_handle, bytes);
                        }
                    } else if (IsLobbyOnBigworld(source_handle)) {
                        m_GlobalProcessThread.QueueAction(m_BigworldChannel.Dispatch, source_handle, seq, bytes);
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnMessageResultCallback(int worldId, uint seq, int src, int dest, int result)
        {

        }
        private void LoadData()
        {
            try {
                TableConfig.LevelProvider.Instance.LoadForServer();
                TableConfig.ActorProvider.Instance.LoadForServer();
                TableConfig.UserScriptProvider.Instance.LoadForServer();
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void Start()
        {
            m_GlobalProcessThread.Start();
            m_UserProcessScheduler.Start();
            m_QueueingThread.Start();
            m_DataCacheThread.Start();
        }
        private void Stop()
        {
            m_QueueingThread.Stop();
            m_UserProcessScheduler.Stop();
            m_GlobalProcessThread.Stop();
            m_DataCacheThread.Stop();
        }
        private void InstallMessageHandlers()
        {
            m_DataCacheChannel = new PBChannel(DataMessageEnum2Type.Query, DataMessageEnum2Type.Query);
            m_DataCacheChannel.WorldId = UserServerConfig.WorldId;
            m_DataCacheChannel.DefaultServiceName = "DataCache";

            InstallNodeHandlers();
            InstallGmJsonHandlers();
            InstallBigworldHandlers();
        }
        //Const Define
        private const int c_MaxWaitLoginUserNum = 3000;
        private const long c_ChatStatisticInterval = 60000;
        private const long c_WarningTickTime = 1000;
        private const long c_WaitQuitTimeInterval = 600000;      //重置等待退出状态的时间间隔,5mins
        private const long c_FightingScoreStatisticInterval = 60000;
        //
        private bool m_WaitQuit = false;
        private bool m_QuitFinish = false;
        private long m_LastTickTime = 0;
        private long m_LastWaitQuitTime = 0;

        private QueueingThread m_QueueingThread = new QueueingThread();
        private DataCacheThread m_DataCacheThread = new DataCacheThread();
        private UserProcessScheduler m_UserProcessScheduler = new UserProcessScheduler();
        private GlobalProcessThread m_GlobalProcessThread = new GlobalProcessThread();
        private HashSet<int> m_NodeHandlesOnBigworld = new HashSet<int>();
        private HashSet<int> m_NodeHandles = new HashSet<int>();
        private PBChannel m_DataCacheChannel = null;
        private PBChannel m_BigworldChannel = null;

        private CenterHubApi.HandleNameHandleChangedCallback m_NameHandleCallback = null;
        private CenterHubApi.HandleMessageCallback m_MsgCallback = null;
        private CenterHubApi.HandleMessageResultCallback m_MsgResultCallback = null;
        private CenterHubApi.HandleCommandCallback m_CmdCallback = null;
        private CenterHubApi.CenterLogHandler m_LogHandler = null;

        internal static void Main(string[] args)
        {
            UserServer lobby = UserServer.Instance;
            lobby.Init(args);
            lobby.Loop();
            lobby.Release();
        }
    }
}
