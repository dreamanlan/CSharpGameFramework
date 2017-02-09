using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using GameFramework;
using CSharpCenterClient;
using Messenger;
using GameFrameworkMessage;

namespace RoomServer
{
    /// <remarks>
    /// 注意这个类的消息处理的逻辑里调用的其它方法，都要检查跨线程调用是否安全！！！
    /// </remarks>
    internal sealed partial class RoomServer
    {
        private void Init(string[] args)
        {
            m_NameHandleCallback = this.OnNameHandleChanged;
            m_MsgCallback = this.OnMessage;
            m_MsgResultCallback = this.OnMessageResultCallback;
            m_CmdCallback = this.OnCommand;
            m_LogHandler = this.OnCenterLog;
            CenterClientApi.SetCenterLogHandler(m_LogHandler);
            CenterClientApi.Init("roomserver", args.Length, args, m_NameHandleCallback, m_MsgCallback, m_MsgResultCallback, m_CmdCallback);

            Console.WriteLine("begin init roomserver...");
            HomePath.InitHomePath();

            bool ret = LogSys.Init("./config/logconfig.xml");
            System.Diagnostics.Debug.Assert(ret);

            m_LastTickTimeForSend = TimeUtility.GetLocalMilliseconds();
            m_LastSendRoomInfoTime = m_LastTickTimeForSend;
            m_IsContinueRegister = true;
            m_Channel = new PBChannel(BigworldAndRoomServerMessageEnum2Type.Query, BigworldAndRoomServerMessageEnum2Type.Query);
            m_Channel.DefaultServiceName = "Lobby";
            m_LobbyConnector = new Connector(m_Channel);

            m_ServerIp = "127.0.0.1";
            m_ServerPort = 9528;

            InitConfig();

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
            PluginAssembly.Instance.Init();

            LogSys.Log(LOG_TYPE.DEBUG, "room server init ip: {0}  port: {1}", m_ServerIp, m_ServerPort);

            uint tick_interval = 33;
            m_RoomMgr = new RoomManager(1280, c_ThreadCount, c_PerThreadRoomCount, tick_interval, m_LobbyConnector);
            m_RoomMgr.Init(m_RoomServerName);
            IOManager.Instance.Init((int)m_ServerPort);
            m_RoomMgr.StartRoomThread();
            SceneLogicViewManager.Instance.Init();

            ServerStorySystem.StaticInit();
            GameFramework.GmCommands.GmStorySystem.StaticInit();

            m_Channel.Register<Msg_LR_ReplyRegisterRoomServer>(HandleReplyRegisterRoomServer);
            m_RoomMgr.RegisterMsgHandler(m_Channel);

            LogSys.Log(LOG_TYPE.DEBUG, "room server init ok.");
        }
        private void InitConfig()
        {
            try {
                StringBuilder sb = new StringBuilder(256);
                if (CenterClientApi.GetConfig("name", sb, 256)) {
                    string str = sb.ToString();
                    if (null == m_RoomServerName || 0 != m_RoomServerName.CompareTo(str))
                        m_RoomServerName = str;
                }
                if (CenterClientApi.GetConfig("ServerIp", sb, 256)) {
                    string str = sb.ToString();
                    if (null == m_ServerIp || 0 != m_ServerIp.CompareTo(str))
                        m_ServerIp = str;
                }
                if (CenterClientApi.GetConfig("ServerPort", sb, 256)) {
                    uint port = uint.Parse(sb.ToString());
                    if (m_ServerPort != port)
                        m_ServerPort = port;
                }
                if (CenterClientApi.GetConfig("Debug", sb, 256)) {
                    int debug = int.Parse(sb.ToString());
                    if (debug != 0) {
                        GlobalVariables.Instance.IsDebug = true;
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "RoomServer.InitConfig throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void Loop()
        {
            try {
                while (CenterClientApi.IsRun()) {
                    long curTime = TimeUtility.GetLocalMilliseconds();
                    if (m_LastTickTime != 0) {
                        long elapsedTickTime = curTime - m_LastTickTime;
                        if (elapsedTickTime > c_WarningTickTime) {
                            LogSys.Log(LOG_TYPE.MONITOR, "RoomServer Network Tick:{0}", curTime - m_LastTickTime);
                        }
                    }
                    m_LastTickTime = curTime;

                    CenterClientApi.Tick();
                    Tick();
                    Thread.Sleep(10);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "RoomServer.Loop throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void Release()
        {
            m_RoomMgr.StopRoomThread();
            CenterClientApi.Release();
            IOManager.Instance.Release();
            LogSys.Release();
        }
        private void Tick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTimeForSend + c_TickIntervalMs < curTime) {
                m_LastTickTimeForSend = curTime;

                if (m_IsContinueRegister) {
                    SendRegisterRoomServer();
                }
            }
            if (!m_IsContinueRegister)
                SendRoomServerUpdateInfo();
        }

        private void OnCenterLog(string msg, int size)
        {
            LogSys.Log(LOG_TYPE.INFO, "{0}", msg);
        }
        private void OnNameHandleChanged(bool addOrUpdate, string name, int handle)
        {
            try {
                if (name.CompareTo("Lobby") == 0 && !addOrUpdate) {
                    m_IsContinueRegister = true;
                }
                m_Channel.OnUpdateNameHandle(addOrUpdate, name, handle);
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnCommand(int src, int dest, string command)
        {
            const string c_QuitRoomServer = "QuitRoomServer";
            const string c_ReloadConfig = "ReloadConfig";
            try {
                if (0 == command.CompareTo(c_QuitRoomServer)) {
                    LogSys.Log(LOG_TYPE.MONITOR, "receive {0} command, quit", command);
                    CenterClientApi.Quit();
                } else if (0 == command.CompareTo(c_ReloadConfig)) {
                    CenterClientApi.ReloadConfigScript();
                    InitConfig();
                    LogSys.Log(LOG_TYPE.WARN, "receive {0} command.", command);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void OnMessage(uint seq, int source_handle,
            int dest_handle,
            IntPtr data, int len)
        {
            try {
                byte[] bytes = new byte[len];
                Marshal.Copy(data, bytes, 0, len);
                m_Channel.Dispatch(source_handle, seq, bytes);
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void OnMessageResultCallback(uint seq, int src, int dest, int result)
        {

        }
        
        private void SendRegisterRoomServer()
        {
            Msg_RL_RegisterRoomServer rrsBuilder = new Msg_RL_RegisterRoomServer();
            rrsBuilder.ServerName = m_RoomServerName;
            rrsBuilder.MaxRoomNum = 1024;
            rrsBuilder.ServerIp = m_ServerIp;
            rrsBuilder.ServerPort = m_ServerPort;
            m_Channel.Send(rrsBuilder);
            LogSys.Log(LOG_TYPE.DEBUG, "register room server to Lobby.");
        }

        private void SendRoomServerUpdateInfo()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            int ts = (int)(curTime - m_LastSendRoomInfoTime);
            if (ts >= c_SendIntervalMs) {
                m_LastSendRoomInfoTime = curTime;
                Msg_RL_RoomServerUpdateInfo msgBuilder = new Msg_RL_RoomServerUpdateInfo();
                msgBuilder.ServerName = m_RoomServerName;
                msgBuilder.IdleRoomNum = m_RoomMgr.GetIdleRoomCount();
                msgBuilder.UserNum = m_RoomMgr.GetUserCount();
                m_Channel.Send(msgBuilder);
                //LogSys.Log(LOG_TYPE.DEBUGI, "send room info to Lobby, Name:{0} IdleRoomNum:{1} UserNum:{2}.", room_server_name_, room_mgr_.GetIdleRoomCount(), room_mgr_.GetUserCount());
            }
        }

        private void HandleReplyRegisterRoomServer(Msg_LR_ReplyRegisterRoomServer msg, PBChannel channel, int handle, uint seq)
        {
            if (msg.IsOk == true) {
                m_IsContinueRegister = false;
            }
        }

        private const int c_TickIntervalMs = 5000;           // tick间隔
        private const int c_SendIntervalMs = 1000;           // 发送间隔
        private const long c_WarningTickTime = 1000;
        private long m_LastTickTimeForSend = 0;

        private RoomManager m_RoomMgr;
        private long m_LastTickTime;
        private long m_LastSendRoomInfoTime;// 上一次发送房间信息的时间
        private bool m_IsContinueRegister;
        private string m_ServerIp;
        private uint m_ServerPort;
        private Connector m_LobbyConnector;
        private PBChannel m_Channel;

        private string m_RoomServerName;

        private CenterClientApi.HandleNameHandleChangedCallback m_NameHandleCallback = null;
        private CenterClientApi.HandleMessageCallback m_MsgCallback = null;
        private CenterClientApi.HandleMessageResultCallback m_MsgResultCallback = null;
        private CenterClientApi.HandleCommandCallback m_CmdCallback = null;
        private CenterClientApi.CenterLogHandler m_LogHandler = null;

        private const int c_ThreadCount = 8;
        private const int c_PerThreadRoomCount = 32;

        internal static RoomServer Instance
        {
            get { return s_Instance; }
        }
        private static RoomServer s_Instance;

        internal static void Main(string[] args)
        {
            s_Instance = new RoomServer();
            s_Instance.Init(args);
            s_Instance.Loop();
            s_Instance.Release();
        }        
    }
}
