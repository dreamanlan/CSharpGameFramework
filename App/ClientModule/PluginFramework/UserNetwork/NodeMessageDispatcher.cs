using System;
using System.Text;
using LitJson;
using GameFramework;
using GameFrameworkMessage;

namespace GameFramework.Network
{
    internal class NodeMessageHandlerInfo
    {
        internal Type m_Type = null;
        internal Type m_ProtoType = null;
        internal NodeMessageHandlerDelegate m_Handler = null;
    }
    internal class NodeMessageDispatcher
    {
        internal static void Init()
        {
            if (!s_Inited) {
                s_MessageHandlers = new NodeMessageHandlerInfo[(int)LobbyMessageDefine.MaxNum];
                for (int i = 0; i < (int)LobbyMessageDefine.MaxNum; ++i) {
                    s_MessageHandlers[i] = new NodeMessageHandlerInfo();
                }
                s_Inited = true;
            }
        }

        internal static bool Inited
        {
            get {
                return s_Inited;
            }
        }

        internal static void RegisterMessageHandler(int id, Type type, Type protoType, NodeMessageHandlerDelegate handler)
        {
            if (s_Inited) {
                if (id >= 0 && id < (int)LobbyMessageDefine.MaxNum) {
                    s_MessageHandlers[id].m_Type = type;
                    s_MessageHandlers[id].m_ProtoType = protoType;
                    s_MessageHandlers[id].m_Handler = handler;
                }
            }
        }

        internal static void HandleNodeMessage(string msgStr)
        {
            if (s_Inited) {
                NodeMessage msg = DecodeNodeMessage(msgStr);
                if (null != msg) {
                    HandleNodeMessage(msg);
                }
            }
        }

        internal static void HandleNodeMessage(NodeMessage msg)
        {
            if (s_Inited && msg != null) {
                NodeMessageHandlerInfo info = s_MessageHandlers[(int)msg.m_ID];
                if (info != null && info.m_Handler != null) {
                    info.m_Handler(msg);
                }
            }
        }

        internal static NodeMessage DecodeNodeMessage(string msgStr)
        {
            NodeMessage msg = null;
            if (s_Inited) {
                try {
                    byte[] msgData = Convert.FromBase64String(msgStr);
                    if (msgData.Length >= 4) {
                        int first = msgData[0];
                        int second = msgData[1];
                        int headerFirst = msgData[2];
                        int headerSecond = msgData[3];
                        int id = first + (second << 8);
                        int headerSize = headerFirst + (headerSecond << 8);

                        LogSystem.Info("DecodeNodeMessage:{0} {1}", id, msgStr);

                        msg = new NodeMessage(id);
                        Type type = GetMessageType(id);
                        if (null != type) {
                            msg.m_NodeHeader = Encoding.Decode(type, msgData, 4, headerSize);
                        }
                        Type proto = GetMessageProtoType(id);
                        if (null != proto) {
                            msg.m_ProtoData = Encoding.Decode(proto, msgData, 4 + headerSize);
                        }
                    }
                }
                catch (Exception ex) {
                    if (null == msgStr) {
                        LogSystem.Error("[Exception] DecodeJsonMessage:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                    else {
                        LogSystem.Error("[Exception] DecodeJsonMessage:{0} {1}\n{2}", msgStr, ex.Message, ex.StackTrace);
                    }
                }
            }
            return msg;
        }

        internal static Type GetMessageType(int id)
        {
            Type type = null;
            if (id >= 0 && id < (int)LobbyMessageDefine.MaxNum) {
                type = s_MessageHandlers[id].m_Type;
            }
            return type;
        }

        internal static Type GetMessageProtoType(int id)
        {
            Type type = null;
            if (id >= 0 && id < (int)LobbyMessageDefine.MaxNum) {
                type = s_MessageHandlers[id].m_ProtoType;
            }
            return type;
        }

        internal static bool SendMessage(NodeMessage msg)
        {
            string msgStr = BuildNodeMessage(msg);

            LogSystem.Info("SendToLobby:{0} {1}", msg.m_ID, msgStr);
            /* 
            /// Test message package
            if (msg.m_ID == (int)LobbyMessageDefine.GetMorrowReward
              || msg.m_ID == (int)LobbyMessageDefine.SellItem
              || msg.m_ID == (int)LobbyMessageDefine.DiscardItem
              || msg.m_ID == (int)LobbyMessageDefine.MountEquipment
              || msg.m_ID == (int)LobbyMessageDefine.UnmountEquipment
              || msg.m_ID == (int)LobbyMessageDefine.UpgradeSkill
              || msg.m_ID == (int)LobbyMessageDefine.CompoundEquip
              || msg.m_ID == (int)LobbyMessageDefine.CompoundTalentCard
              || msg.m_ID == (int)LobbyMessageDefine.BuyEliteCount
              || msg.m_ID == (int)LobbyMessageDefine.StartPartnerBattle
              || msg.m_ID == (int)LobbyMessageDefine.EndPartnerBattle
              || msg.m_ID == (int)LobbyMessageDefine.BuyPartnerCombatTicket
              || msg.m_ID == (int)LobbyMessageDefine.RefreshPartnerCombatResult
              || msg.m_ID == (int)LobbyMessageDefine.UpgradeItem
              || msg.m_ID == (int)LobbyMessageDefine.StageClear
              || msg.m_ID == (int)LobbyMessageDefine.SweepStage
              || msg.m_ID == (int)LobbyMessageDefine.LiftSkill
              || msg.m_ID == (int)LobbyMessageDefine.BuyStamina
              || msg.m_ID == (int)LobbyMessageDefine.FinishMission
              || msg.m_ID == (int)LobbyMessageDefine.UpgradeLegacy
              || msg.m_ID == (int)LobbyMessageDefine.AddXSoulExperience
              || msg.m_ID == (int)LobbyMessageDefine.XSoulChangeShowModel
              || msg.m_ID == (int)LobbyMessageDefine.FinishExpedition
              || msg.m_ID == (int)LobbyMessageDefine.ExpeditionAward
              || msg.m_ID == (int)LobbyMessageDefine.MidasTouch
              || msg.m_ID == (int)LobbyMessageDefine.ExchangeGoods
              || msg.m_ID == (int)LobbyMessageDefine.SecretAreaTrial
              || msg.m_ID == (int)LobbyMessageDefine.SecretAreaFightingInfo
              || msg.m_ID == (int)LobbyMessageDefine.RequestRefreshExchange
              || msg.m_ID == (int)LobbyMessageDefine.SelectPartner
              || msg.m_ID == (int)LobbyMessageDefine.UpgradePartnerLevel
              || msg.m_ID == (int)LobbyMessageDefine.UpgradePartnerStage
              || msg.m_ID == (int)LobbyMessageDefine.CompoundPartner
              || msg.m_ID == (int)LobbyMessageDefine.SignInAndGetReward
              || msg.m_ID == (int)LobbyMessageDefine.WeeklyLoginReward
              || msg.m_ID == (int)LobbyMessageDefine.GetOnlineTimeReward
              || msg.m_ID == (int)LobbyMessageDefine.RequestMpveAward
              || msg.m_ID == (int)LobbyMessageDefine.EquipTalent
              || msg.m_ID == (int)LobbyMessageDefine.AddTalentExperience
              || msg.m_ID == (int)LobbyMessageDefine.GainVipReward
              || msg.m_ID == (int)LobbyMessageDefine.GainFirstPayReward
              || msg.m_ID == (int)LobbyMessageDefine.RequestEnhanceEquipmentStar
              || msg.m_ID == (int)LobbyMessageDefine.UpgradeEquipBatch
              || msg.m_ID == (int)LobbyMessageDefine.BuyFashion
              || msg.m_ID == (int)LobbyMessageDefine.LuckyDraw
              || msg.m_ID == (int)LobbyMessageDefine.EquipmentStrength) {

              /// Test codes
              /// Send 32 packets each time to test multi-thread deadlock or other bugs
              for (int i = 0; i < 32; ++i) {
                LobbyNetworkSystem.Instance.SendMessage(msgStr);
              }
              return true;
            }
            */
            return UserNetworkSystem.Instance.SendMessage(msgStr);
        }
        internal static bool SendMessageTest(NodeMessage msg)
        {
            string msgStr = BuildNodeMessage(msg);

            /// Test codes
            /// Send 32 packets each time to test multi-thread deadlock or other bugs
            for (int i = 0; i < 32; ++i) {
                UserNetworkSystem.Instance.SendMessage(msgStr);
            }

            return true;
        }

        private static string BuildNodeMessage(NodeMessage msg)
        {
            byte[] msgData = Encoding.Encode(msg.m_ID, msg.m_NodeHeader, msg.m_ProtoData);
            return Convert.ToBase64String(msgData);
        }

        private static ProtoNetEncoding Encoding
        {
            get {
                if (null == s_Encoding) {
                    s_Encoding = new ProtoNetEncoding();
                }
                return s_Encoding;
            }
        }

        private static bool s_Inited = false;
        private static NodeMessageHandlerInfo[] s_MessageHandlers = null;
        [ThreadStatic]
        private static ProtoNetEncoding s_Encoding = null;
    }
}

