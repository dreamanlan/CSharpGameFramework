using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using GameFrameworkMessage;
using GameFramework;

namespace GameFramework.Network
{
    internal class NetworkSystem
    {
        private class MessageCountInfo
        {
            internal int m_MsgCount;
            internal int m_TotalMsgSize;
        }
        private class MessageInfo : IPoolAllocatedObject<MessageInfo>
        {
            internal int m_MsgId;
            internal object m_Msg;
            internal NetConnection m_Connection;
            internal int m_MsgSize;

            public void InitPool(ObjectPool<MessageInfo> pool)
            {
                m_Pool = pool;
            }
            public MessageInfo Downcast()
            {
                return this;
            }
            private ObjectPool<MessageInfo> m_Pool;
        }

        #region
        private static NetworkSystem s_Instance = new NetworkSystem();
        internal static NetworkSystem Instance
        {
            get { return s_Instance; }
        }
        #endregion

        internal bool Init()
        {
            InitMessageHandler();

            m_NetClientStarted = false;
            m_IsWaitStart = true;
            m_IsQuited = false;
            m_IsConnected = false;
            m_CanSendMessage = false;
            m_ReconnectCount = 0;

            m_Config = new NetPeerConfiguration("RoomServer");
            m_Config.AutoFlushSendQueue = false;
            m_Config.DisableMessageType(NetIncomingMessageType.DebugMessage);
            m_Config.DisableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.WarningMessage);
            m_Config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            m_NetClient = new NetClient(m_Config);
            m_NetThread = new Thread(new ThreadStart(NetworkThread));
            m_NetThread.IsBackground = true;
            m_NetThread.Start();
            return true;
        }

        internal void Start(uint key, string ip, int port, int campId, int sceneId)
        {
            if (m_IsConnected) {
                Disconnect("bye for restart");
            }

            StartNetClient();

            m_Key = key;
            m_Ip = ip;
            m_Port = port;

            m_RoomSceneId = sceneId;

            m_IsWaitStart = false;
            m_IsConnected = false;
            m_CanSendMessage = false;
            m_ReconnectCount = 0;

            LogSystem.Info("NetworkSystem.Start key {0} ip {1} port {2} camp {3} scene {4}", key, ip, port, campId, sceneId);

            m_SendMessageCounts.Clear();
            m_ReceiveMessageCounts.Clear();
        }

        internal void Tick()
        {
            try {
                if (m_NetClient == null)
                    return;
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_IsConnected) {
                    if (m_WaitShakeHands) {
                        if (m_LastShakeHandsTime > 0 && curTime - m_LastShakeHandsTime >= c_ShakeHandTimeout) {
                            Disconnect("bye for shakehands timeout");

                            LogSystem.Info("ShakeHands time out, CurTime {0} LastShakeHandsTime {1}", curTime, m_LastShakeHandsTime);
                        }
                    }
                    if (m_CanSendMessage) {
                        if (curTime - m_LastPingTime >= c_PingInterval) {
                            InternalPing();
                        }
                        if (m_LastReceivePongTime > 0 && curTime - m_LastReceivePongTime > c_PingTimeout) {
                            Disconnect("bye for timeout");

                            LogSystem.Info("PingPong time out, CurTime {0} LastReceivePongTime {1} LastPingTime {2}", curTime, m_LastReceivePongTime, m_LastPingTime);
                        }
                    }
                }
                ProcessMsg();
            } catch (Exception e) {
                LogSystem.Error("Exception:{0}\n{1}", e.Message, e.StackTrace);
            }
        }

        internal bool IsWaitStart
        {
            get { return m_IsWaitStart; }
        }

        internal bool IsQuited
        {
            get
            {
                return m_IsQuited;
            }
        }

        internal void Resume()
        {
            StartNetClient();
            m_IsWaitStart = false;
            m_IsConnected = false;
            m_CanSendMessage = false;
            m_ReconnectCount = 0;
        }

        internal void Pause()
        {
            ShutdownNetClient();
            m_IsWaitStart = true;
            m_IsConnected = false;
            m_CanSendMessage = false;
            m_ReconnectCount = 0;
        }

        internal void Disconnect(string reason)
        {
            m_NetClient.Disconnect(reason);
            m_LastReceivePongTime = 0;
            m_ReconnectCount = 0;
            TimeUtility.LastResponseTime = 0;
        }

        internal void QuitBattle(bool is_force)
        {
            m_IsWaitStart = true;

            if (m_IsConnected) {
                Msg_CR_Quit msg = new Msg_CR_Quit();
                msg.is_force = is_force;
                SendMessage(RoomMessageDefine.Msg_CR_Quit, msg);

                Thread.Sleep(1000);
                m_NetClient.Disconnect("bye for quit");
                Thread.Sleep(1000);
            }

            ClearArgs();
            ClientModule.Instance.QueueAction(this.ShutdownNetClient);
        }

        internal void QuitBattlePassive()
        {
            m_IsWaitStart = true;

            if (m_IsConnected) {
                m_NetClient.Disconnect("bye for quit passive");
                Thread.Sleep(1000);
            }

            ClearArgs();
            ClientModule.Instance.QueueAction(this.ShutdownNetClient);
        }

        internal void OnRoomServerWaitStart()
        {
            //网络被关闭又没有正常结束，有可能是服务器副本已经关闭，直接尝试结算
            GameFramework.Story.GfxStorySystem.Instance.SendMessage("try_stag_clear", 0);
            ClearArgs();
            ClientModule.Instance.QueueAction(this.ShutdownNetClient);
        }

        internal void QuitClient()
        {
            if (m_IsConnected) {
                m_NetClient.Disconnect("bye for quit passive");
                Thread.Sleep(1000);
            }
            m_IsQuited = true;
        }

        internal void SendMessage(RoomMessageDefine id, object msg)
        {
            try {
                if (!m_IsConnected) {
                    return;
                }
                NetOutgoingMessage om = m_NetClient.CreateMessage();
                byte[] bt = Serialize.Encode(msg, (int)id);
                IncSendMessageCount((int)id, bt.Length);
                om.Write(bt);
                NetSendResult result = m_NetClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
                if (result == NetSendResult.FailedNotConnected) {
                    m_IsConnected = false;
                    m_CanSendMessage = false;
                    LogSystem.Info("SendMessage FailedNotConnected");
                } else if (result == NetSendResult.Dropped) {
                    LogSystem.Error("SendMessage {0} Dropped", msg.ToString());
                }
                m_NetClient.FlushSendQueue();
            } catch (Exception ex) {
                LogSystem.Error("NetworkSystem.SendMessage throw Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        internal void Release()
        {
            ShutdownNetClient();
        }

        internal void OnPong(long time, long sendPingTime, long sendPongTime)
        {
            if (time < sendPingTime) return;
            ++m_PingPongNumber;
            m_LastReceivePongTime = time;
            TimeUtility.LastResponseTime = time;

            long rtt = time - sendPingTime;
            if (TimeUtility.AverageRoundtripTime == 0)
                TimeUtility.AverageRoundtripTime = rtt;
            else
                TimeUtility.AverageRoundtripTime = (long)(TimeUtility.AverageRoundtripTime * 0.7f + rtt * 0.3f);

            //LogSystem.Info("RoundtripTime:{0} AverageRoundtripTime:{1}", rtt, TimeUtility.AverageRoundtripTime);

            long diff = sendPongTime + rtt / 2 - time;
            TimeUtility.RemoteTimeOffset = (TimeUtility.RemoteTimeOffset * (m_PingPongNumber - 1) + diff) / m_PingPongNumber;
        }
        
        internal void SyncPlayerMoveToPos(ScriptRuntime.Vector3 target_pos)
        {
            EntityInfo userInfo = ClientModule.Instance.GetEntityById(ClientModule.Instance.LeaderID);
            if (null != userInfo) {
                MovementStateInfo msi = userInfo.GetMovementStateInfo();

                Msg_CR_UserMoveToPos builder = new Msg_CR_UserMoveToPos();
                builder.target_pos = ProtoHelper.EncodePosition2D(target_pos.X, target_pos.Z);
                SendMessage(RoomMessageDefine.Msg_CR_UserMoveToPos, builder);
            }
        }

        internal void SyncPlayerStopMove()
        {
            EntityInfo userInfo = ClientModule.Instance.GetEntityById(ClientModule.Instance.LeaderID);
            if (null != userInfo) {
                MovementStateInfo msi = userInfo.GetMovementStateInfo();

                Msg_CR_UserMoveToPos builder = new Msg_CR_UserMoveToPos();
                builder.target_pos = ProtoHelper.EncodePosition2D(msi.PositionX, msi.PositionZ);
                builder.is_stop = true;
                SendMessage(RoomMessageDefine.Msg_CR_UserMoveToPos, builder);
            }
        }

        internal void SyncPlayerSkill(EntityInfo entity, int skillId, int targetId, float faceDir)
        {
            if (entity.IsHaveStateFlag(CharacterState_Type.CST_Sleep)) {
                return;
            }
            Msg_CR_Skill bd = new Msg_CR_Skill();
            bd.role_id = entity.GetId();
            bd.skill_id = skillId;
            if (targetId > 0) {
                bd.target_id = targetId;
            } else {
                bd.target_dir = ProtoHelper.EncodeFloat(faceDir);
            }
            SendMessage(RoomMessageDefine.Msg_CR_Skill, bd);

        }

        internal void SyncPlayerStopSkill(EntityInfo entity)
        {
            if (entity.IsHaveStateFlag(CharacterState_Type.CST_Sleep)) {
                return;
            }
            Msg_CR_StopSkill bd = new Msg_CR_StopSkill();
            SendMessage(RoomMessageDefine.Msg_CR_StopSkill, bd);

        }

        internal void SyncOperateMode(bool bAuto)
        {
            Msg_CR_OperateMode bd = new Msg_CR_OperateMode();
            bd.isauto = bAuto;
            SendMessage(RoomMessageDefine.Msg_CR_OperateMode, bd);
        }

        internal void SyncGiveUpCombat()
        {
            Msg_CR_GiveUpBattle msg = new Msg_CR_GiveUpBattle();
            SendMessage(RoomMessageDefine.Msg_CR_GiveUpBattle, msg);
        }

        private void ClearArgs()
        {
            m_Key = 0;
            m_Ip = "";
            m_Port = 0;
        }
        private void RegisterMsgHandler(RoomMessageDefine msgid, MessageDispatch.MsgHandler handler)
        {
            m_Dispatch.RegisterHandler(msgid, handler);
        }
        private void InitMessageHandler()
        {
            RegisterMsgHandler(RoomMessageDefine.Msg_Pong, new MessageDispatch.MsgHandler(MsgPongHandler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_ShakeHands_Ret, new MessageDispatch.MsgHandler(MsgShakeHandsRetHandler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_CreateNpc, new MessageDispatch.MsgHandler(Msg_RC_CreateNpc_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_NpcDead, new MessageDispatch.MsgHandler(Msg_RC_NpcDead_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_DestroyNpc, new MessageDispatch.MsgHandler(Msg_RC_DestroyNpc_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_NpcMove, new MessageDispatch.MsgHandler(Msg_RC_NpcMove_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_NpcFace, new MessageDispatch.MsgHandler(Msg_RC_NpcFace_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_NpcSkill, new MessageDispatch.MsgHandler(Msg_RC_NpcSkill_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_NpcStopSkill, new MessageDispatch.MsgHandler(Msg_RC_NpcStopSkill_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_AddImpact, new MessageDispatch.MsgHandler(Msg_RC_AddImpact_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_RemoveImpact, new MessageDispatch.MsgHandler(Msg_RC_RemoveImpact_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_AddSkill, new MessageDispatch.MsgHandler(Msg_RC_AddSkill_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_RemoveSkill, new MessageDispatch.MsgHandler(Msg_RC_RemoveSkill_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_AdjustPosition, new MessageDispatch.MsgHandler(Msg_RC_AdjustPosition_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_SyncProperty, new MessageDispatch.MsgHandler(Msg_RC_SyncProperty_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_ImpactDamage, new MessageDispatch.MsgHandler(Msg_RC_ImpactDamage_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_ChangeScene, new MessageDispatch.MsgHandler(Msg_RC_ChangeScene_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_SyncNpcOwnerId, new MessageDispatch.MsgHandler(Msg_RC_SyncNpcOwnerId_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_CampChanged, new MessageDispatch.MsgHandler(Msg_RC_CampChanged_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_DebugSpaceInfo, new MessageDispatch.MsgHandler(Msg_RC_DebugSpaceInfo_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_CRC_StoryMessage, new MessageDispatch.MsgHandler(Msg_CRC_StoryMessage_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_PublishEvent, new MessageDispatch.MsgHandler(Msg_RC_PublishEvent_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_SendGfxMessage, new MessageDispatch.MsgHandler(Msg_RC_SendGfxMessage_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_HighlightPrompt, new MessageDispatch.MsgHandler(Msg_RC_HighlightPrompt_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_ShowDlg, new MessageDispatch.MsgHandler(Msg_RC_ShowDlg_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_LockFrame, new MessageDispatch.MsgHandler(Msg_RC_LockFrame_Handler.Execute));
            RegisterMsgHandler(RoomMessageDefine.Msg_RC_PlayAnimation, new MessageDispatch.MsgHandler(Msg_RC_PlayAnimation_Handler.Execute));
        }
        private void NetworkThread()
        {
            while (!m_IsQuited) {
                try {
                    if (m_IsWaitStart) {
                        Thread.Sleep(1000);
                    } else {
                        while (!m_IsQuited && !m_IsConnected && !m_IsWaitStart) {
                            LogSystem.Info("Connect ip:{0} port:{1} key:{2}\nNetPeer Statistic:{3}", m_Ip, m_Port, Key, m_NetClient.Statistics.ToString());
                            try {
                                m_LastPingTime = 0;
                                m_LastReceivePongTime = 0;
                                m_PingPongNumber = 0;
                                m_WaitShakeHands = false;
                                m_LastShakeHandsTime = 0;
                                ++m_ReconnectCount;
                                TimeUtility.LastResponseTime = 0;
                                m_NetClient.Connect(m_Ip, m_Port);
                            } catch {
                            }
                            for (int ct = 0; ct < c_ConnectionTimeout / 1000 && !m_IsConnected; ++ct) {
                                OnRecvMessage();
                                LogSystem.Info("Wait NetConnectionStatus.Connected ...");
                            }
                            if (!m_IsConnected) {
                                m_NetClient.Disconnect("bye for reconnect");
                                ClientModule.Instance.QueueAction(ClientModule.Instance.OnRoomServerDisconnected);
                            }
                        }
                        OnRecvMessage();
                    }
                } catch (Exception e) {
                    LogSystem.Error("Exception:{0}\n{1}", e.Message, e.StackTrace);
                }
            }
        }
        private void OnConnected(NetConnection conn)
        {
            m_Connection = conn;
            m_IsConnected = true;
            m_ReconnectCount = 0;

            Msg_CR_ShakeHands bd = new Msg_CR_ShakeHands();
            bd.auth_key = Key;
            SendMessage(RoomMessageDefine.Msg_CR_ShakeHands, bd);

            m_LastShakeHandsTime = TimeUtility.GetLocalMilliseconds();
            m_WaitShakeHands = true;
        }
        private void OnRecvMessage()
        {
            try {
                m_NetClient.MessageReceivedEvent.WaitOne(1000);
                NetIncomingMessage im;
                while ((im = m_NetClient.ReadMessage()) != null) {
                    switch (im.MessageType) {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                            LogSystem.Info("Debug Message: {0}", im.ReadString());
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            LogSystem.Info("Error Message: {0}", im.ReadString());
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            LogSystem.Info("Warning Message: {0}", im.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                            string reason = im.ReadString();
                            if (null != im.SenderConnection) {
                                LogSystem.Info("Network Status Changed:{0} Reason:{1}\nStatistic:{2}", status, reason, im.SenderConnection.Statistics.ToString());
                                LogMessageCount();
                                if (NetConnectionStatus.Disconnected == status) {
                                    m_IsConnected = false;
                                    m_CanSendMessage = false;
                                    if (reason.CompareTo("disconnect") == 0) {
                                        if (!m_IsWaitStart) {
                                            m_IsWaitStart = true;
                                            m_NetClient.Disconnect("bye for disconnect");
                                            ClientModule.Instance.QueueAction(this.OnRoomServerWaitStart);
                                        }
                                    }
                                } else if (NetConnectionStatus.Connected == status) {
                                    OnConnected(im.SenderConnection);
                                }
                            } else {
                                LogSystem.Info("Network Status Changed:{0} reason:{1}", status, reason);
                            }
                            break;
                        case NetIncomingMessageType.Data:
                        case NetIncomingMessageType.UnconnectedData:
                            if (!m_IsConnected && NetIncomingMessageType.Data == im.MessageType) {
                                break;
                            }
                            try {
                                byte[] data = im.ReadBytes(im.LengthBytes);
                                int id;
                                object msg = Serialize.Decode(data, out id);
                                if (msg != null) {
                                    PushMsg(id, msg, data.Length, im.SenderConnection);
                                }
                            } catch (Exception ex) {
                                LogSystem.Error("Decode Message exception:{0}\n{1}", ex.Message, ex.StackTrace);
                            }
                            break;
                        default:
                            break;
                    }
                    m_NetClient.Recycle(im);
                }
            } catch (Exception e) {
                LogSystem.Error("Exception:{0}\n{1}", e.Message, e.StackTrace);
            }
        }
        private bool PushMsg(int id, object msg, int msgSize, NetConnection conn)
        {
            lock (m_Lock) {
                MessageInfo info = m_MessageInfoPool.Alloc();
                info.m_MsgId = id;
                info.m_Msg = msg;
                info.m_MsgSize = msgSize;
                info.m_Connection = conn;
                m_QueuePair.Enqueue(info);
                return true;
            }
        }
        private int ProcessMsg()
        {
            lock (m_Lock) {
                if (m_QueuePair.Count <= 0)
                    return -1;
                foreach (MessageInfo msgInfo in m_QueuePair) {
                    try {
                        IncReceiveMessageCount(msgInfo.m_MsgId, msgInfo.m_MsgSize);
                        m_Dispatch.Dispatch(msgInfo.m_MsgId, msgInfo.m_Msg, msgInfo.m_Connection);
                    } catch (Exception ex) {
                        LogSystem.Error("ProcessMsg Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    } finally {
                        m_MessageInfoPool.Recycle(msgInfo);
                    }
                }
                m_QueuePair.Clear();
                return 0;
            }
        }

        private void StartNetClient()
        {
            if (m_NetClient != null) {
                if (!m_NetClientStarted) {
                    m_NetClient.Start();
                    m_NetClientStarted = true;
                }
            }
        }
        private void ShutdownNetClient()
        {
            if (m_NetClient != null) {
                if (m_NetClientStarted) {
                    m_NetClient.Shutdown("bye");
                    m_NetClientStarted = false;

                    m_IsConnected = false;
                }
            }
        }
        private void InternalPing()
        {
            if (m_CanSendMessage) {
                Msg_Ping builder = new Msg_Ping();
                m_LastPingTime = TimeUtility.GetLocalMilliseconds();
                builder.send_ping_time = (int)m_LastPingTime;
                SendMessage(RoomMessageDefine.Msg_Ping, builder);
            }
        }
        private void IncSendMessageCount(int id, int size)
        {
            MessageCountInfo ctInfo;
            if (m_SendMessageCounts.TryGetValue(id, out ctInfo)) {
                ++ctInfo.m_MsgCount;
                ctInfo.m_TotalMsgSize += size;
            } else {
                ctInfo = new MessageCountInfo();
                ctInfo.m_MsgCount = 1;
                ctInfo.m_TotalMsgSize = size;
                m_SendMessageCounts.Add(id, ctInfo);
            }
        }
        private void IncReceiveMessageCount(int id, int size)
        {
            MessageCountInfo ctInfo;
            if (m_ReceiveMessageCounts.TryGetValue(id, out ctInfo)) {
                ++ctInfo.m_MsgCount;
                ctInfo.m_TotalMsgSize += size;
            } else {
                ctInfo = new MessageCountInfo();
                ctInfo.m_MsgCount = 1;
                ctInfo.m_TotalMsgSize = size;
                m_ReceiveMessageCounts.Add(id, ctInfo);
            }
        }
        private void LogMessageCount()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Message Count]:");
            sb.AppendLine("[Send]:");
            foreach (KeyValuePair<int, MessageCountInfo> pair in m_SendMessageCounts) {
                Type t = RoomMessageDefine2Type.Query(pair.Key);
                if (null != t) {
                    sb.AppendFormat("=>{0}:{1} {2} {3}", pair.Key, pair.Value.m_MsgCount, pair.Value.m_TotalMsgSize, t.ToString());
                } else {
                    sb.AppendFormat("=>{0}:{1} {2}", pair.Key, pair.Value.m_MsgCount, pair.Value.m_TotalMsgSize);
                }
                sb.AppendLine();
            }
            sb.AppendLine("[Receive]:");
            foreach (KeyValuePair<int, MessageCountInfo> pair in m_ReceiveMessageCounts) {
                Type t = RoomMessageDefine2Type.Query(pair.Key);
                if (null != t) {
                    sb.AppendFormat("=>{0}:{1} {2} {3}", pair.Key, pair.Value.m_MsgCount, pair.Value.m_TotalMsgSize, t.ToString());
                } else {
                    sb.AppendFormat("=>{0}:{1} {2}", pair.Key, pair.Value.m_MsgCount, pair.Value.m_TotalMsgSize);
                }
                sb.AppendLine();
            }
            LogSystem.Info("{0}", sb.ToString());
        }

        internal int ReconnectCount
        {
            get { return m_ReconnectCount; }
        }
        internal bool WaitShakeHands
        {
            get { return m_WaitShakeHands; }
            set { m_WaitShakeHands = value; }
        }
        internal bool CanSendMessage
        {
            get { return m_CanSendMessage; }
            set { m_CanSendMessage = value; }
        }
        internal uint Key
        {
            get { return m_Key; }
        }
        internal int RoomSceneId
        {
            get { return m_RoomSceneId; }
        }

        private NetworkSystem() { }

        private const int c_ConnectionTimeout = 10000;
        private const long c_WaitDisconnectTimeout = 5000;
        private const long c_PingInterval = 5000;
        private const long c_PingTimeout = 20000;
        private const long c_ShakeHandTimeout = 10000;

        private bool m_WaitShakeHands = false;
        private long m_LastShakeHandsTime = 0;

        private long m_PingPongNumber = 0;
        private long m_LastPingTime = 0;
        private long m_LastReceivePongTime = 0;

        private NetPeerConfiguration m_Config;
        private NetClient m_NetClient;
        private NetConnection m_Connection;
        private Thread m_NetThread;
        private bool m_NetClientStarted = false;

        private string m_Ip;
        private int m_Port;
        private bool m_IsConnected = false;
        private bool m_IsWaitStart = true;
        private int m_ReconnectCount = 0;

        private bool m_IsQuited = false;
        private bool m_CanSendMessage = false;
        private MessageDispatch m_Dispatch = new MessageDispatch();

        private Queue<MessageInfo> m_QueuePair = new Queue<MessageInfo>();
        private ObjectPool<MessageInfo> m_MessageInfoPool = new ObjectPool<MessageInfo>();
        private object m_Lock = new object();
        private uint m_Key = 0;
        private int m_RoomSceneId = 0;

        //消息统计
        private Dictionary<int, MessageCountInfo> m_SendMessageCounts = new Dictionary<int, MessageCountInfo>();
        private Dictionary<int, MessageCountInfo> m_ReceiveMessageCounts = new Dictionary<int, MessageCountInfo>();
    }
}
