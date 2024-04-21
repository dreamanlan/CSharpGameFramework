using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameFrameworkMessage;
using LitJson;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace GameFramework.Network
{
    internal sealed partial class UserNetworkSystem
    {
        internal void Init(IActionQueue asyncQueue)
        {
            //WebSocket events are not triggered in the current thread,
            //we need to make thread adjustments ourselves
            m_AsyncActionQueue = asyncQueue;

            NodeMessageDispatcher.Init();
            LobbyMessageInit();
            m_IsWaitStart = true;
            m_HasLoggedOn = false;
            m_IsLogining = false;
            m_IsQueueing = false;
            m_LastHeartbeatTime = 0;
            m_LastReceiveHeartbeatTime = 0;
            m_LastQueueingTime = 0;
            m_LastShowQueueingTime = 0;
            m_LastConnectTime = 0;
        }
        internal void SetUrl(string url)
        {
            try {
                m_Url = url;

                if (IsConnected) {
                    Disconnect(true);
                }

                BuildWebSocket();
            } catch (Exception ex) {
                LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        internal void ConnectIfNotOpen()
        {
            if (!IsConnected) {
                m_LastHeartbeatTime = 0;
                m_LastReceiveHeartbeatTime = 0;
                m_LastQueueingTime = 0;
                m_LastShowQueueingTime = 0;
                m_LastConnectTime = TimeUtility.GetLocalMilliseconds();
                TimeUtility.LobbyLastResponseTime = 0;
                LogSystem.Info("ConnectIfNotOpen at time:{0} ServerAddress:{1}", m_LastConnectTime, m_Url);

                m_IsLogining = true;
                m_IsQueueing = false;

                try {
                    m_WebSocket.Open();
                } catch (Exception ex) {
                    LogSystem.Warn("m_WebSocket.Open throw exception {0}\n{1}", ex.Message, ex.StackTrace);
                    BuildWebSocket();
                    //Reconnect immediately next time tick
                    m_LastConnectTime = 0;
                }
            }
        }
        internal void Tick()
        {
            try {
                if (!m_IsWaitStart) {
                    long curTime = TimeUtility.GetLocalMilliseconds();

                    if (!IsConnected) {
                        if (m_LastConnectTime + c_ConnectionTimeout < curTime) {
                            LogSystem.Info("IsConnected == false. CurTime {0} LastConnectTime {1}", curTime, m_LastConnectTime);
                            Disconnect(true);//Prevent the connection from being established later and causing state invalidation
                            ConnectIfNotOpen();
                        }
                    } else {
                        if (m_IsQueueing) {//in queueing
                            if (m_LastQueueingTime + c_GetQueueingCountInterval < curTime) {
                                m_LastQueueingTime = curTime;

                                SendGetQueueingCount();
                            }
                            if (m_LastShowQueueingTime + c_ShowQueueingCountInterval < curTime && m_QueueingNum > 0) {
                                m_LastShowQueueingTime = curTime;

                                PluginFramework.Instance.HighlightPromptWithDict("Tip_QueueingCount", m_QueueingNum);
                            }
                        } else if (m_HasLoggedOn) {//Disconnection and reconnection process and normal game situation
                            if (m_IsLogining) {//logining
                                if (m_LastConnectTime + c_LoginTimeout < curTime) {
                                    LogSystem.Info("Login time out, disconnect and connect again. CurTime {0} LastConnectTime {1}", curTime, m_LastConnectTime);
                                    //disconnect
                                    if (IsConnected) {
                                        Disconnect();
                                    }
                                }
                            } else {
                                if (m_LastHeartbeatTime + c_PingInterval < curTime) {
                                    m_LastHeartbeatTime = curTime;
                                    if (m_LastReceiveHeartbeatTime == 0) {
                                        m_LastReceiveHeartbeatTime = curTime;
                                    }
                                    SendHeartbeat();
                                }
                                if (m_LastReceiveHeartbeatTime > 0 && m_LastReceiveHeartbeatTime + c_PingTimeout < curTime) {
                                    LogSystem.Info("Heartbeat time out, disconnect and connect again. CurTime {0} LastReceiveHeartbeatTime {1} LastHeartbeatTime {2} LastConnectTime {3}", curTime, m_LastReceiveHeartbeatTime, m_LastHeartbeatTime, m_LastConnectTime);
                                    //disconnect
                                    if (IsConnected) {
                                        Disconnect();
                                        m_LastReceiveHeartbeatTime = 0;
                                        TimeUtility.LobbyLastResponseTime = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception e) {
                LogSystem.Error("Exception:{0}\n{1}", e.Message, e.StackTrace);
            }
        }
        internal void Disconnect()
        {
            Disconnect(false);
        }
        internal void Disconnect(bool disconnectImmediately)
        {
            try {
                long curTime = TimeUtility.GetLocalMilliseconds();
                LogSystem.Info("Disconnect({0}). CurTime {1} LastConnectTime {2}", disconnectImmediately, curTime, m_LastConnectTime);
                if (m_WebSocket != null) {
                    m_WebSocket.Close(disconnectImmediately);
                    if (!disconnectImmediately) {
                        //Wait 3 seconds before triggering reconnection
                        m_LastConnectTime = curTime - c_ConnectionTimeout + 3000;
                    }
                }
            } catch (Exception ex) {
                LogSystem.Warn("m_WebSocket.Close throw exception {0}\n{1}", ex.Message, ex.StackTrace);
                BuildWebSocket();
                //Reconnect immediately next time tick
                m_LastConnectTime = 0;
            }
        }
        internal void QuitRoom(bool isQuitTeam)
        {
            if (m_Guid != 0) {
                NodeMessage msg = new NodeMessage(LobbyMessageDefine.QuitRoom);
                msg.SetHeaderWithGuid(m_Guid);
                GameFrameworkMessage.QuitRoom protoMsg = new GameFrameworkMessage.QuitRoom();
                protoMsg.m_IsQuitRoom = isQuitTeam;
                msg.m_ProtoData = protoMsg;
                SendMessage(msg);
            }
        }
        internal void QuitClient()
        {
            if (m_Guid != 0) {
                NodeMessage msg = new NodeMessage(LobbyMessageDefine.Logout, m_Guid);
                SendMessage(msg);
            }
            if (m_AccountId != string.Empty) {
                NodeMessage msg = new NodeMessage(LobbyMessageDefine.AccountLogout, m_AccountId);
                SendMessage(msg);
            }
            m_IsWaitStart = true;
            m_HasLoggedOn = false;

            Disconnect();
        }
        internal bool IsConnected
        {
            get
            {
                bool ret = false;
                if (null != m_WebSocket)
                    ret = (m_WebSocket.State == WebSocket4Net.WebSocketState.Open && m_WebSocket.IsSocketConnected);
                return ret;
            }
        }
        internal int QueueingNum
        {
            get { return m_QueueingNum; }
            set { m_QueueingNum = value; }
        }
        public bool IsQueueing
        {
            get { return m_IsQueueing; }
            set { m_IsQueueing = value; }
        }
        internal bool IsLogining
        {
            get { return m_IsLogining; }
            set { m_IsLogining = value; }
        }
        internal bool HasLoggedOn
        {
            get { return m_HasLoggedOn; }
            set { m_HasLoggedOn = value; }
        }
        internal ulong Guid
        {
            get { return m_Guid; }
        }

        internal bool SendMessage(string msgStr)
        {
            bool ret = false;
            try {
                if (IsConnected) {
                    m_WebSocket.Send(msgStr);
                    ret = true;
                }
            } catch (Exception ex) {
                LogSystem.Error("SendMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
            return ret;
        }

        private void SendGetQueueingCount()
        {
            NodeMessage msg = new NodeMessage(LobbyMessageDefine.GetQueueingCount, m_AccountId);
            SendMessage(msg);
        }

        private void SendHeartbeat()
        {
            NodeMessage msg = new NodeMessage(LobbyMessageDefine.UserHeartbeat, m_Guid);
            SendMessage(msg);
        }

        private void HandleQueueingCountResult(NodeMessage lobbyMsg)
        {
            GameFrameworkMessage.QueueingCountResult protoData = lobbyMsg.m_ProtoData as GameFrameworkMessage.QueueingCountResult;
            if (null != protoData) {
                int queueingCount = protoData.m_QueueingCount;
                if (queueingCount != UserNetworkSystem.Instance.QueueingNum) {
                    UserNetworkSystem.Instance.QueueingNum = protoData.m_QueueingCount;
                }
            }
        }

        private void HandleUserHeartbeat(NodeMessage lobbyMsg)
        {
            m_LastReceiveHeartbeatTime = TimeUtility.GetLocalMilliseconds();
            TimeUtility.LobbyLastResponseTime = m_LastReceiveHeartbeatTime;
            long rtt = m_LastReceiveHeartbeatTime - m_LastHeartbeatTime;
            if (rtt > 0) {
                if (TimeUtility.LobbyAverageRoundtripTime == 0)
                    TimeUtility.LobbyAverageRoundtripTime = rtt;
                else
                    TimeUtility.LobbyAverageRoundtripTime = (long)(TimeUtility.LobbyAverageRoundtripTime * 0.7f + rtt * 0.3f);
            }
        }

        private void RegisterMsgHandler(LobbyMessageDefine id, Type headerType, NodeMessageHandlerDelegate handler)
        {
            NodeMessageDispatcher.RegisterMessageHandler((int)id, headerType, null, handler);
        }
        private void RegisterMsgHandler(LobbyMessageDefine id, Type headerType, Type protoType, NodeMessageHandlerDelegate handler)
        {
            NodeMessageDispatcher.RegisterMessageHandler((int)id, headerType, protoType, handler);
        }
        private void SendMessage(NodeMessage msg)
        {
            try {
                NodeMessageDispatcher.SendMessage(msg);
            } catch (Exception ex) {
                LogSystem.Error("LobbyNetworkSystem.SendMessage throw Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void OnOpened()
        {
            //Verify version first
            NodeMessage versionMsg = new NodeMessage(LobbyMessageDefine.VersionVerify, (uint)0x01010101);
            SendMessage(versionMsg);
        }
        private void OnError(Exception ex)
        {
            if (null != ex) {
                LogSystem.Error("LobbyNetworkSystem.OnError Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                Disconnect();
            }
        }
        private void OnLog(string msg)
        {
            LogSystem.Info("LobbyNetworkSystem.OnLog:{0}", msg);
        }
        private void OnClosed()
        {
            LogSystem.Error("LobbyNetworkSystem.OnClosed");
        }
        private void OnMessageReceived(string msg)
        {
            if (null != msg) {
                NodeMessageDispatcher.HandleNodeMessage(msg);
            }
        }
        private void OnDataReceived(byte[] data)
        {
            LogSystem.Info("Receive Data Message:\n{0}", Helper.BinToHex(data));
        }

        private void BuildWebSocket()
        {
            try {
                m_WebSocket = new WebSocket4Net.WebSocket(m_Url);
                m_WebSocket.AllowUnstrustedCertificate = true;
                m_WebSocket.EnableAutoSendPing = false;
                m_WebSocket.AutoSendPingInterval = 10;
                m_WebSocket.Opened += OnWsOpened;
                m_WebSocket.MessageReceived += OnWsMessageReceived;
                m_WebSocket.DataReceived += OnWsDataReceived;
                m_WebSocket.Error += OnWsError;
                m_WebSocket.Log += OnWsLog;
                m_WebSocket.Closed += OnWsClosed;
            } catch (Exception ex) {
                LogSystem.Warn("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void OnWsOpened(object sender, EventArgs e)
        {
            if (null != m_AsyncActionQueue) {
                m_AsyncActionQueue.QueueActionWithDelegation((MyAction)this.OnOpened);
            }
        }
        private void OnWsError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            if (null != m_AsyncActionQueue) {
                if (null != e) {
                    m_AsyncActionQueue.QueueActionWithDelegation((MyAction<Exception>)this.OnError, e.Exception);
                }
            }
        }
        private void OnWsLog(object sender, string msg)
        {
            m_AsyncActionQueue.QueueActionWithDelegation((MyAction<string>)this.OnLog, msg);
        }
        private void OnWsClosed(object sender, EventArgs e)
        {
            if (null != m_AsyncActionQueue) {
                m_AsyncActionQueue.QueueActionWithDelegation((MyAction)this.OnClosed);
            }
        }
        private void OnWsMessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            if (null != m_AsyncActionQueue) {
                m_AsyncActionQueue.QueueActionWithDelegation((MyAction<string>)this.OnMessageReceived, e.Message);
            }
        }
        private void OnWsDataReceived(object sender, WebSocket4Net.DataReceivedEventArgs e)
        {
            if (null != m_AsyncActionQueue) {
                m_AsyncActionQueue.QueueActionWithDelegation((MyAction<byte[]>)this.OnDataReceived, e.Data);
            }
        }

        private string GetIp()
        {
            string ip = "127.0.0.1";
            try {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in adapters) {
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4)) {
                        UnicastIPAddressInformationCollection uniCast = adapter.GetIPProperties().UnicastAddresses;
                        if (uniCast.Count > 0) {
                            foreach (UnicastIPAddressInformation uni in uniCast) {
                                //Get the IPv4 address. AddressFamily.InterNetwork refers to IPv4
                                if (uni.Address.AddressFamily == AddressFamily.InterNetwork) {
                                    ip = uni.Address.ToString();
                                }
                            }
                        }
                    }
                }
            } catch (System.Exception ex) {
                LogSystem.Error("LobbyNetworkSystem.GetIp {0}", ex.Message);
            }
            return ip;
        }

        private const long c_ConnectionTimeout = 10000;
        private const long c_GetQueueingCountInterval = 5000;
        private const long c_ShowQueueingCountInterval = 1000;
        private const long c_LoginTimeout = 45000;
        private const long c_PingInterval = 5000;
        private const long c_PingTimeout = 20000;

        private bool m_IsWaitStart = true;
        private bool m_IsQueueing = false;
        private bool m_IsLogining = false;
        private bool m_HasLoggedOn = false;
        private long m_LastConnectTime = 0;
        private long m_LastQueueingTime = 0;
        private long m_LastHeartbeatTime = 0;
        private long m_LastReceiveHeartbeatTime = 0;

        private long m_LastShowQueueingTime = 0;
        private int m_QueueingNum = -1;
        private string m_Url;
        private int m_WorldId = -1;       //WorldId of client connection
        private ulong m_Guid;

        private string m_AccountId;
        private string m_Password;
        private string m_ClientInfo = "";

        private WebSocket4Net.WebSocket m_WebSocket;
        private IActionQueue m_AsyncActionQueue;

        internal static UserNetworkSystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static UserNetworkSystem s_Instance = new UserNetworkSystem();
    }
}
