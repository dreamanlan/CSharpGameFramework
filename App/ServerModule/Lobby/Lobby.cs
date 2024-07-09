using System;
using System.IO;
using System.Text;
using CSharpCenterClient;
using Messenger;
using Lobby;
using ScriptableFramework;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using GameFrameworkData;
using System.Threading;
using GameFrameworkMessage;

namespace Lobby
{
    internal partial class LobbyServer
    {
        #region Singleton
        private static LobbyServer s_Instance = new LobbyServer();
        internal static LobbyServer Instance
        {
            get
            {
                return s_Instance;
            }
        }
        #endregion

        internal Messenger.PBChannel UserChannel
        {
            get { return m_UserChannel; }
        }
        internal Messenger.PBChannel RoomSvrChannel
        {
            get { return m_RoomSvrChannel; }
        }
        internal Messenger.PBChannel DataCacheChannel
        {
            get { return m_DataCacheChannel; }
        }
        internal RoomProcessThread RoomProcessThread
        {
            get { return m_RoomProcessThread; }
        }
        internal GlobalProcessThread GlobalProcessThread
        {
            get { return m_GlobalProcessThread; }
        }
        internal UserProcessScheduler UserProcessScheduler
        {
            get { return m_UserProcessScheduler; }
        }
        internal bool IsUnknownServer(ulong handle)
        {
            return !IsNode(handle) && !IsRoomServer(handle) && !IsUserServer(handle) && !IsDataCache(handle);
        }
        internal bool IsNode(ulong handle)
        {
            return m_NodeHandles.Contains(handle);
        }
        internal bool IsRoomServer(ulong handle)
        {
            return m_RoomSvrHandles.Contains(handle);
        }
        internal bool IsUserServer(ulong handle)
        {
            return m_UserHandles.Contains(handle);
        }
        internal bool IsDataCache(ulong handle)
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
            TransmitToWorld(user, msg);
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
            TransmitToWorld(user, msg);
        }
        internal void TransmitToWorld(UserInfo user, NodeMessage msg)
        {
            if (null != user && null != msg) {
                TransmitToWorld(user.UserSvrName, user.NodeName, msg);
            }
        }
        internal void TransmitToWorld(string userSvrName, string nodeName, NodeMessage msg)
        {
            if (!string.IsNullOrEmpty(userSvrName) && !string.IsNullOrEmpty(nodeName) && null != msg) {
                ulong lobbyHandle = CenterClientApi.TargetHandle(userSvrName);
                if (lobbyHandle > 0) {
                    TransmitToWorld(lobbyHandle, nodeName, msg);
                }
            }
        }
        internal void TransmitToWorld(ulong userSvrHandle, string nodeName, NodeMessage msg)
        {
            try {
                if (!string.IsNullOrEmpty(nodeName) && null != msg) {
                    byte[] data = NodeMessageDispatcher.BuildNodeMessage(msg);
                    if (null != data) {
                        Msg_LBL_Message builder = new Msg_LBL_Message();
                        builder.MsgType = Msg_LBL_Message.MsgTypeEnum.Node;
                        builder.TargetName = nodeName;
                        builder.Data = data;
                        m_UserChannel.Send(userSvrHandle, builder);
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        internal void ForwardToWorld(string userSvrName, object msg)
        {
            if (!string.IsNullOrEmpty(userSvrName) && null != msg) {
                ulong lobbyHandle = CenterClientApi.TargetHandle(userSvrName);
                if (lobbyHandle > 0) {
                    ForwardToWorld(lobbyHandle, msg);
                }
            }
        }
        internal void ForwardToWorld(ulong userSvrHandle, object msg)
        {
            try {
                if (null != msg) {
                    byte[] data = m_UserChannel.Encode(msg);
                    if (null != data) {
                        Msg_LBL_Message builder = new Msg_LBL_Message();
                        builder.MsgType = Msg_LBL_Message.MsgTypeEnum.Room;
                        builder.TargetName = "UserSvr";
                        builder.Data = data;
                        m_UserChannel.Send(userSvrHandle, builder);
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void Init(string[] args)
        {
            m_NameHandleCallback = this.OnNameHandleChanged;
            m_MsgCallback = this.OnMessage;
            m_MsgResultCallback = this.OnMessageResultCallback;
            m_CmdCallback = this.OnCommand;
            m_LogHandler = this.OnCenterLog;
            CenterClientApi.SetCenterLogHandler(m_LogHandler);
            CenterClientApi.Init("lobby", args.Length, args, m_NameHandleCallback, m_MsgCallback, m_MsgResultCallback, m_CmdCallback);

            LogSys.Init("./config/logconfig.xml");
            LobbyConfig.Init();

            if (LobbyConfig.IsDebug) {
                GlobalVariables.Instance.IsDebug = true;
            }

            GlobalVariables.Instance.IsClient = false;

            ResourceReadProxy.OnReadAsArray = ((string filePath) => {
                byte[] buffer = null;
                try {
                    buffer = File.ReadAllBytes(filePath);
                } catch (Exception e) {
                    LogSys.Log(ServerLogType.ERROR, "Exception:{0}\n{1}", e.Message, e.StackTrace);
                    return null;
                }
                return buffer;
            });
            LogSystem.OnOutput += (GameLogType type, string msg) => {
                switch (type) {
                    case GameLogType.Debug:
                        LogSys.Log(ServerLogType.DEBUG, msg);
                        break;
                    case GameLogType.Info:
                        LogSys.Log(ServerLogType.INFO, msg);
                        break;
                    case GameLogType.Warn:
                        LogSys.Log(ServerLogType.WARN, msg);
                        break;
                    case GameLogType.Error:
                    case GameLogType.Assert:
                        LogSys.Log(ServerLogType.ERROR, msg);
                        break;
                }
            };

            LoadData();
            LogSys.Log(ServerLogType.INFO, "Init Config ...");
            s_Instance = this;
            InstallMessageHandlers();
            LogSys.Log(ServerLogType.INFO, "Init Messenger ...");
            m_RoomProcessThread.Init();
            LogSys.Log(ServerLogType.INFO, "Init RoomProcessThread ...");
            Start();
            LogSys.Log(ServerLogType.INFO, "Start Threads ...");
        }
        private void Loop()
        {
            try {
                while (CenterClientApi.IsRun()) {
                    long curTime = TimeUtility.GetLocalMilliseconds();
                    if (m_LastTickTime != 0) {
                        long elapsedTickTime = curTime - m_LastTickTime;
                        if (elapsedTickTime > c_WarningTickTime) {
                            LogSys.Log(ServerLogType.MONITOR, "Lobby Network Tick:{0}", curTime - m_LastTickTime);
                        }
                    }
                    m_LastTickTime = curTime;

                    CenterClientApi.Tick();
                    Thread.Sleep(10);
                    if (m_WaitQuit) {
                        //wait 10s
                        long startTime = TimeUtility.GetLocalMilliseconds();
                        while (startTime + 10000 > TimeUtility.GetLocalMilliseconds()) {
                            CenterClientApi.Tick();
                            Thread.Sleep(10);
                        }
                        //close Lobby
                        LogSys.Log(ServerLogType.MONITOR, "QuitStep_3. LastSaveDone. Lobby quit...");
                        CenterClientApi.Quit();
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Lobby.Loop throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void Release()
        {
            Stop();
            CenterClientApi.Release();
            LogSys.Release();
        }
        private void OnCenterLog(string msg, int size)
        {
            LogSys.Log(ServerLogType.INFO, "{0}", msg);
        }
        private void OnNameHandleChanged(bool addOrUpdate, string name, ulong handle)
        {
            try {
                m_DataCacheChannel.OnUpdateNameHandle(addOrUpdate, name, handle);
                if (!addOrUpdate) {
                    m_NodeHandles.Remove(handle);
                    m_RoomSvrHandles.Remove(handle);
                    m_UserHandles.Remove(handle);
                }
                if (0 == name.CompareTo("Lobby")) {
                    if (addOrUpdate) {
                        m_MyHandle = handle;
                    } else {
                        m_MyHandle = 0;
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnCommand(ulong src, ulong dest, string command)
        {
            const string c_QuitLobby = "QuitLobby";
            const string c_ReloadConfig = "ReloadConfig";
            try {
                if (0 == command.CompareTo(c_QuitLobby)) {
                    LogSys.Log(ServerLogType.MONITOR, "receive {0} command, save data and then quitting ...", command);
                    if (!m_WaitQuit) {
                        m_UserProcessScheduler.DispatchAction(m_UserProcessScheduler.DoCloseServers);
                        m_LastWaitQuitTime = TimeUtility.GetLocalMilliseconds();
                        m_WaitQuit = true;
                    }
                } else if (0 == command.CompareTo(c_ReloadConfig)) {
                    CenterClientApi.ReloadConfigScript();
                    LobbyConfig.Init();
                    LogSys.Log(ServerLogType.WARN, "receive {0} command.", command);
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnMessage(uint seq, ulong source_handle, ulong dest_handle,
            IntPtr data, int len)
        {
            try {
                if (IsUnknownServer(source_handle)) {
                    StringBuilder sb = new StringBuilder(256);
                    if (CenterClientApi.TargetName(source_handle, sb, 256)) {
                        string name = sb.ToString();
                        if (name.StartsWith("NodeJs")) {
                            m_NodeHandles.Add(source_handle);
                        } else if (name.StartsWith("RoomSvr")) {
                            m_RoomSvrHandles.Add(source_handle);
                        } else if (name.StartsWith("UserSvr")) {
                            m_UserHandles.Add(source_handle);
                        }
                    }
                }
                byte[] bytes = new byte[len];
                Marshal.Copy(data, bytes, 0, len);
                if (IsNode(source_handle)) {
                    if (!m_WaitQuit) {
                        m_UserProcessScheduler.DispatchJsonMessage(seq, source_handle, dest_handle, bytes);
                    }
                } else if (IsUserServer(source_handle)) {
                    DispatchUserMessage(source_handle, seq, bytes);
                } else if (IsRoomServer(source_handle)) {
                    m_RoomProcessThread.QueueAction(m_RoomSvrChannel.Dispatch, source_handle, seq, bytes);
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnMessageResultCallback(uint seq, ulong src, ulong dest, int result)
        {

        }
        private void LoadData()
        {
            try {
                TableConfig.LevelProvider.Instance.LoadForServer();
                TableConfig.ActorProvider.Instance.LoadForServer();
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void SetModuleLevelLock()
        {
            //const int pvp_unlock_index = 18;
        }
        private void Start()
        {
            m_GlobalProcessThread.Start();
            m_UserProcessScheduler.Start();
            m_RoomProcessThread.Start();
        }
        private void Stop()
        {
            m_RoomProcessThread.Stop();
            m_UserProcessScheduler.Stop();
            m_GlobalProcessThread.Stop();
        }
        private void InstallMessageHandlers()
        {
            m_DataCacheChannel = new PBChannel(DataMessageEnum2Type.Query,
                                DataMessageEnum2Type.Query);
            m_DataCacheChannel.DefaultServiceName = "DataCache";

            InstallUserHandlers();
            InstallServerHandlers();
            InstallNodeHandlers();
        }

        private const int c_MaxWaitLoginUserNum = 3000;
        private bool m_WaitQuit = false;

        private const long c_WarningTickTime = 1000;
        private long m_LastTickTime = 0;
        private const long c_WaitQuitTimeInterval = 300000;      //Reset the time interval for waiting for exit status,5mins
        private long m_LastWaitQuitTime = 0;

        private UserProcessScheduler m_UserProcessScheduler = new UserProcessScheduler();
        private GlobalProcessThread m_GlobalProcessThread = new GlobalProcessThread();
        private RoomProcessThread m_RoomProcessThread = new RoomProcessThread();

        private HashSet<ulong> m_NodeHandles = new HashSet<ulong>();
        private HashSet<ulong> m_RoomSvrHandles = new HashSet<ulong>();
        private HashSet<ulong> m_UserHandles = new HashSet<ulong>();
        private PBChannel m_RoomSvrChannel = null;
        private PBChannel m_UserChannel = null;
        private PBChannel m_DataCacheChannel = null;

        private ulong m_MyHandle = 0;

        private CenterClientApi.HandleNameHandleChangedCallback m_NameHandleCallback = null;
        private CenterClientApi.HandleMessageCallback m_MsgCallback = null;
        private CenterClientApi.HandleMessageResultCallback m_MsgResultCallback = null;
        private CenterClientApi.HandleCommandCallback m_CmdCallback = null;
        private CenterClientApi.CenterLogHandler m_LogHandler = null;

        internal static void Main(string[] args)
        {
            LobbyServer lobby = LobbyServer.Instance;
            lobby.Init(args);
            lobby.Loop();
            lobby.Release();
        }
    }
}
