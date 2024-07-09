using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using Lidgren.Network;
using CSharpCenterClient;

using ScriptableFramework;
using GameFrameworkMessage;
using ScriptableFramework.Network;

namespace ScriptableFramework
{
    public enum RoomSrvStatus
    {
        STATUS_INIT = 0,
        STATUS_RUNNING = 1,
        STATUS_STOP = 2,
    }
    public class IOManager
    {
        #region
        private static IOManager s_Instance = new IOManager();
        public static IOManager Instance
        {
            get { return s_Instance; }
        }
        #endregion

        private Thread m_IOThread;
        private NetServer m_NetServer;
        private NetPeerConfiguration m_Config;
        private RoomSrvStatus m_Status = RoomSrvStatus.STATUS_INIT;
        private MessageDispatch m_Dispatch = new MessageDispatch();

        public void Init(int port)
        {
            InitMessageHandler();

            int receiveBufferSize = 64;
            int sendBufferSize = 64;
            StringBuilder sb = new StringBuilder(256);
            if (CenterClientApi.GetConfig("ReceiveBufferSize", sb, 256)) {
                receiveBufferSize = int.Parse(sb.ToString());
            }
            if (CenterClientApi.GetConfig("SendBufferSize", sb, 256)) {
                sendBufferSize = int.Parse(sb.ToString());
            }

            m_Config = new NetPeerConfiguration("RoomServer");
            m_Config.MaximumConnections = 1024;
            m_Config.SocketReceiveBufferSize = receiveBufferSize * 1024 * 1024;
            m_Config.SocketSendBufferSize = sendBufferSize * 1024 * 1024;
            m_Config.Port = port;
            m_Config.DisableMessageType(NetIncomingMessageType.DebugMessage);
            m_Config.DisableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            //m_Config.EnableMessageType(NetIncomingMessageType.DebugMessage);
            //m_Config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.WarningMessage);

            if (m_Config.IsMessageTypeEnabled(NetIncomingMessageType.DebugMessage))
                LogSys.Log(ServerLogType.DEBUG, "Enable NetIncomingMessageType.DebugMessage");
            if (m_Config.IsMessageTypeEnabled(NetIncomingMessageType.VerboseDebugMessage))
                LogSys.Log(ServerLogType.DEBUG, "Enable NetIncomingMessageType.VerboseDebugMessage");
            if (m_Config.IsMessageTypeEnabled(NetIncomingMessageType.ErrorMessage))
                LogSys.Log(ServerLogType.DEBUG, "Enable NetIncomingMessageType.ErrorMessage");
            if (m_Config.IsMessageTypeEnabled(NetIncomingMessageType.WarningMessage))
                LogSys.Log(ServerLogType.DEBUG, "Enable NetIncomingMessageType.WarningMessage");

            m_NetServer = new NetServer(m_Config);
            m_NetServer.Start();
            m_IOThread = new Thread(new ThreadStart(IOHandler));
            m_IOThread.Name = "IOHandler";
            m_IOThread.IsBackground = true;
            m_Status = RoomSrvStatus.STATUS_RUNNING;
            m_IOThread.Start();
            RoomPeerMgr.Instance.Init();
            Console.WriteLine("Init IOManager OK!");
        }

        public void Release()
        {
            m_NetServer.Shutdown("bye for close server");
            m_Status = RoomSrvStatus.STATUS_STOP;
            if (null != m_IOThread) {
                m_IOThread.Join();
            }
        }

        private bool InitMessageHandler()
        {
            RegisterMsgHandler(RoomMessageDefine.Msg_Ping, new MessageDispatch.MsgHandler(MsgPingHandler.Execute));
            return true;
        }

        public void RegisterMsgHandler(RoomMessageDefine id, MessageDispatch.MsgHandler handler)
        {
            m_Dispatch.RegisterHandler(id, handler);
        }

        private void IOHandler()
        {
            while (m_Status == RoomSrvStatus.STATUS_RUNNING) {
                try {
                    m_NetServer.MessageReceivedEvent.WaitOne(1000);
                    long startTime = TimeUtility.GetElapsedTimeUs();
                    NetIncomingMessage im;
                    for (int ct = 0; ct < 1024; ++ct) {
                        try {
                            if ((im = m_NetServer.ReadMessage()) != null) {
                                switch (im.MessageType) {
                                    case NetIncomingMessageType.DebugMessage:
                                    case NetIncomingMessageType.VerboseDebugMessage:
                                        LogSys.Log(ServerLogType.DEBUG, "Debug Message: {0}", im.ReadString());
                                        break;
                                    case NetIncomingMessageType.ErrorMessage:
                                        LogSys.Log(ServerLogType.DEBUG, "Error Message: {0}", im.ReadString());
                                        break;
                                    case NetIncomingMessageType.WarningMessage:
                                        LogSys.Log(ServerLogType.DEBUG, "Warning Message: {0}", im.ReadString());
                                        break;
                                    case NetIncomingMessageType.StatusChanged:
                                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                                        string reason = im.ReadString();
                                        if (null != im.SenderConnection) {
                                            RoomPeer peer = RoomPeerMgr.Instance.GetPeerByConnection(im.SenderConnection);
                                            if (null != peer) {
                                                LogSys.Log(ServerLogType.DEBUG, "Network Status Changed: {0} reason:{1} EndPoint:{2} Key:{3} User:{4}\nStatistic:{5}", status, reason, im.SenderEndPoint.ToString(), peer.GetKey(), peer.Guid, im.SenderConnection.Statistics.ToString());
                                            } else {
                                                LogSys.Log(ServerLogType.DEBUG, "Network Status Changed: {0} reason:{1} EndPoint:{2}\nStatistic:{3}", status, reason, im.SenderEndPoint.ToString(), im.SenderConnection.Statistics.ToString());
                                            }
                                        } else {
                                            LogSys.Log(ServerLogType.DEBUG, "Network Status Changed:{0} reason:{1}", status, reason);
                                        }
                                        break;
                                    case NetIncomingMessageType.Data:
                                        int id = 0;
                                        object msg = null;
                                        byte[] data = null;
                                        try {
                                            data = im.ReadBytes(im.LengthBytes);
                                            msg = Serialize.Decode(data, out id);
                                        } catch {
                                            if (null != im.SenderConnection) {
                                                RoomPeer peer = RoomPeerMgr.Instance.GetPeerByConnection(im.SenderConnection);
                                                if (null != peer) {
                                                    LogSys.Log(ServerLogType.WARN, "room server decode message error !!! from User:{0}({1})", peer.Guid, peer.GetKey());
                                                }
                                            }
                                        }
                                        if (msg != null) {
                                            m_Dispatch.Dispatch(id, msg, im.SenderConnection);
                                        } else {
                                            if (null != im.SenderConnection) {
                                                RoomPeer peer = RoomPeerMgr.Instance.GetPeerByConnection(im.SenderConnection);
                                                if (null != peer) {
                                                    LogSys.Log(ServerLogType.DEBUG, "got unknow message !!! from User:{0}({1})", peer.Guid, peer.GetKey());
                                                } else {
                                                    LogSys.Log(ServerLogType.DEBUG, "got unknow message !!!");
                                                }
                                            } else {
                                                LogSys.Log(ServerLogType.DEBUG, "got unknow message !!!");
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                m_NetServer.Recycle(im);
                            } else {
                                break;
                            }
                        } catch (Exception ex) {
                            LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
                        }
                    }
                    RoomPeerMgr.Instance.Tick();
                    long endTime = TimeUtility.GetElapsedTimeUs();
                    if (endTime - startTime >= 10000) {
                        LogSys.Log(ServerLogType.DEBUG, "Warning, IOHandler() cost {0} us !\nNetPeer Statistic:{1}", endTime - startTime, m_NetServer.Statistics.ToString());
                    }
                } catch (Exception ex) {
                    LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
                }

                Thread.Sleep(10);
            }
        }

        public void SendPeerMessage(RoomPeer peer, RoomMessageDefine id, object msg)
        {
            try {
                NetOutgoingMessage om = m_NetServer.CreateMessage();
                om.Write(Serialize.Encode(msg, (int)id));
                if (null != peer.GetConnection()) {
                    NetSendResult res = m_NetServer.SendMessage(om, peer.GetConnection(), NetDeliveryMethod.ReliableOrdered, 0);
                    if (res == NetSendResult.Dropped) {
                        LogSys.Log(ServerLogType.ERROR, "SendPeerMessage {0} failed:dropped, User:{1}({2})", msg.ToString(), peer.Guid, peer.GetKey());
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void SendMessage(NetConnection conn, RoomMessageDefine id, object msg)
        {
            try {
                NetOutgoingMessage om = m_NetServer.CreateMessage();
                om.Write(Serialize.Encode(msg, (int)id));
                NetSendResult res = m_NetServer.SendMessage(om, conn, NetDeliveryMethod.ReliableOrdered, 0);
                if (res == NetSendResult.Dropped) {
                    RoomPeer peer = RoomPeerMgr.Instance.GetPeerByConnection(conn);
                    if (null != peer) {
                        LogSys.Log(ServerLogType.ERROR, "SendMessage {0} failed:dropped, User:{1}({2})", msg.ToString(), peer.Guid, peer.GetKey());
                    }
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void SendUnconnectedMessage(NetConnection conn, RoomMessageDefine id, object msg)
        {
            try {
                NetOutgoingMessage om = m_NetServer.CreateMessage();
                om.Write(Serialize.Encode(msg, (int)id));
                m_NetServer.SendUnconnectedMessage(om, conn.RemoteEndPoint);
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
