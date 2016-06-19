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
    internal enum RoomSrvStatus
    {
        STATUS_INIT = 0,
        STATUS_RUNNING = 1,
        STATUS_STOP = 2,
    }

    /// <remarks>
    /// 注意这个类的消息处理的逻辑里调用的其它方法，都要检查跨线程调用是否安全！！！
    /// </remarks>
    internal sealed partial class RoomServer
    {
        internal void ChangeRoomScene(int roomid, int sceneId)
        {
            room_mgr_.ChangeRoomScene(roomid, sceneId);
        }
        internal void PlayerRequestActiveRoom(int targetSceneId, params ulong[] guids)
        {
            room_mgr_.PlayerRequestActiveRoom(targetSceneId, guids);
        }
        internal void PlayerRequestChangeRoom(int targetSceneId, params ulong[] guids)
        {
            room_mgr_.PlayerRequestChangeRoom(targetSceneId, guids);
        }

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

            last_tick_time_ = TimeUtility.GetLocalMilliseconds();
            last_send_roominfo_time_ = last_tick_time_;
            is_continue_register_ = true;
            channel_ = new PBChannel(BigworldAndRoomServerMessageEnum2Type.Query, BigworldAndRoomServerMessageEnum2Type.Query);
            channel_.DefaultServiceName = "Lobby";
            lobby_connector_ = new Connector(channel_);

            server_ip_ = "127.0.0.1";
            server_port_ = 9528;

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

            LogSys.Log(LOG_TYPE.DEBUG, "room server init ip: {0}  port: {1}", server_ip_, server_port_);

            uint tick_interval = 33;
            room_mgr_ = new RoomManager(1280, c_thread_count, c_per_thread_room_count, tick_interval, lobby_connector_);
            room_mgr_.Init(room_server_name_);
            IOManager.Instance.Init((int)server_port_);
            room_mgr_.StartRoomThread();
            AiViewManager.Instance.Init();
            SceneLogicViewManager.Instance.Init();

            ServerStorySystem.StaticInit();
            GameFramework.GmCommands.GmStorySystem.StaticInit();

            channel_.Register<Msg_LR_ReplyRegisterRoomServer>(HandleReplyRegisterRoomServer);
            room_mgr_.RegisterMsgHandler(channel_);

            LogSys.Log(LOG_TYPE.DEBUG, "room server init ok.");
        }
        private void InitConfig()
        {
            try {
                StringBuilder sb = new StringBuilder(256);
                if (CenterClientApi.GetConfig("name", sb, 256)) {
                    string str = sb.ToString();
                    if (null == room_server_name_ || 0 != room_server_name_.CompareTo(str))
                        room_server_name_ = str;
                }
                if (CenterClientApi.GetConfig("ServerIp", sb, 256)) {
                    string str = sb.ToString();
                    if (null == server_ip_ || 0 != server_ip_.CompareTo(str))
                        server_ip_ = str;
                }
                if (CenterClientApi.GetConfig("ServerPort", sb, 256)) {
                    uint port = uint.Parse(sb.ToString());
                    if (server_port_ != port)
                        server_port_ = port;
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
            room_mgr_.StopRoomThread();
            CenterClientApi.Release();
            IOManager.Instance.Release();
            LogSys.Release();
        }
        private void Tick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (last_tick_time_ + c_tick_interval_ms < curTime) {
                last_tick_time_ = curTime;

                if (is_continue_register_) {
                    SendRegisterRoomServer();
                }
            }
            if (!is_continue_register_)
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
                    is_continue_register_ = true;
                }
                channel_.OnUpdateNameHandle(addOrUpdate, name, handle);
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
                channel_.Dispatch(source_handle, seq, bytes);
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
            rrsBuilder.ServerName = room_server_name_;
            rrsBuilder.MaxRoomNum = 1024;
            rrsBuilder.ServerIp = server_ip_;
            rrsBuilder.ServerPort = server_port_;
            channel_.Send(rrsBuilder);
            LogSys.Log(LOG_TYPE.DEBUG, "register room server to Lobby.");
        }

        private void SendRoomServerUpdateInfo()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            int ts = (int)(curTime - last_send_roominfo_time_);
            if (ts >= c_send_interval_ms) {
                last_send_roominfo_time_ = curTime;
                Msg_RL_RoomServerUpdateInfo msgBuilder = new Msg_RL_RoomServerUpdateInfo();
                msgBuilder.ServerName = room_server_name_;
                msgBuilder.IdleRoomNum = room_mgr_.GetIdleRoomCount();
                msgBuilder.UserNum = room_mgr_.GetUserCount();
                channel_.Send(msgBuilder);
                //LogSys.Log(LOG_TYPE.DEBUGI, "send room info to Lobby, Name:{0} IdleRoomNum:{1} UserNum:{2}.", room_server_name_, room_mgr_.GetIdleRoomCount(), room_mgr_.GetUserCount());
            }
        }

        private void HandleReplyRegisterRoomServer(Msg_LR_ReplyRegisterRoomServer msg, PBChannel channel, int handle, uint seq)
        {
            if (msg.IsOk == true) {
                is_continue_register_ = false;
            }
        }

        private const int c_tick_interval_ms = 5000;           // tick间隔
        private const int c_send_interval_ms = 1000;           // 发送间隔
        private const long c_WarningTickTime = 1000;
        private long m_LastTickTime = 0;

        private RoomManager room_mgr_;
        private long last_tick_time_;
        private long last_send_roominfo_time_;// 上一次发送房间信息的时间
        private uint thread_count_;
        private uint per_thread_room_count_;
        private bool is_continue_register_;
        private string server_ip_;
        private uint server_port_;
        private Connector lobby_connector_;
        private PBChannel channel_;

        private string room_server_name_;

        private CenterClientApi.HandleNameHandleChangedCallback m_NameHandleCallback = null;
        private CenterClientApi.HandleMessageCallback m_MsgCallback = null;
        private CenterClientApi.HandleMessageResultCallback m_MsgResultCallback = null;
        private CenterClientApi.HandleCommandCallback m_CmdCallback = null;
        private CenterClientApi.CenterLogHandler m_LogHandler = null;

        private const int c_thread_count = 8;
        private const int c_per_thread_room_count = 32;

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
