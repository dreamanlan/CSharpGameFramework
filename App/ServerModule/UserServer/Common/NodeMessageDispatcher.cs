using System;
using System.Text;
using LitJson;
using CSharpCenterClient;
using ScriptableFramework;
using GameFrameworkMessage;
using ScriptableFramework.Network;

namespace ScriptableFramework
{
    /// <summary>
    /// node message format(Binary form, finally encoded into a string using base64):
    /// 2 byte id
    /// 2 byte node message header size
    /// node message header
    /// proto message data
    /// </summary>
    internal class NodeMessageHandlerInfo
    {
        internal Type m_Type = null;
        internal Type m_ProtoType = null;
        internal NodeMessageHandlerDelegate m_Handler = null;
    }
    /// <summary>
    /// json message format:
    /// 'id'|'json message string'|proto-buf message (base64 encoded into a string)
    /// </summary>
    internal class JsonMessageHandlerInfo
    {
        internal Type m_ProtoType = null;
        internal JsonMessageHandlerDelegate m_Handler = null;
    }
    internal class NodeMessageDispatcher
    {
        internal static void Init(int worldId)
        {
            if (!s_Inited) {
                s_WorldId = worldId;
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

        internal static void SetMessageFilter(NodeMessageFilterDelegate filter)
        {
            s_MessageFilter = filter;
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

        internal static void HandleNodeMessage(
            uint seq,
            ulong source_handle,
            ulong dest_handle,
            byte[] data)
        {
            if (s_Inited) {
                NodeMessage msg = DecodeNodeMessage(data);
                if (null != msg) {
                    bool isContinue = true;
                    if (null != s_MessageFilter) {
                        isContinue = s_MessageFilter(msg, source_handle, seq);
                    }
                    if (isContinue) {
                        HandleNodeMessage(msg, source_handle, seq);
                    }
                }
            }
        }

        internal static void HandleNodeMessage(NodeMessage msg, ulong handle, uint session)
        {
            if (s_Inited && msg != null) {

                //LogSys.Log(LOG_TYPE.DEBUG, "Handle Json Message:{0}={1}", msg.m_ID, msg.GetType().Name);

                NodeMessageHandlerInfo info = s_MessageHandlers[(int)msg.m_ID];
                if (info != null && info.m_Handler != null) {
                    info.m_Handler(msg, handle, session);
                }
            }
        }

        internal static NodeMessage DecodeNodeMessage(byte[] data)
        {
            NodeMessage msg = null;
            if (s_Inited) {
                string msgStr = null;
                try {
                    msgStr = System.Text.Encoding.ASCII.GetString(data, 0, data.Length - 1);
                    byte[] msgData = Convert.FromBase64String(msgStr);
                    if (msgData.Length >= 4) {
                        int first = msgData[0];
                        int second = msgData[1];
                        int headerFirst = msgData[2];
                        int headerSecond = msgData[3];
                        int id = first + (second << 8);
                        int headerSize = headerFirst + (headerSecond << 8);
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
                        LogSys.Log(ServerLogType.ERROR, "[Exception] DecodeJsonMessage:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                    else {
                        LogSys.Log(ServerLogType.ERROR, "[Exception] DecodeJsonMessage:{0} {1}\n{2}", msgStr, ex.Message, ex.StackTrace);
                    }
                }
            }
            if (null != msg) {
                msg.m_OriginalMsgData = data;
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

        internal static void SendNodeMessage(ulong handle, NodeMessage msg)
        {
            if (s_Inited) {
                byte[] data = BuildNodeMessage(msg);
                if (null != data) {
                    CenterHubApi.SendByHandle(s_WorldId, handle, data, data.Length);
                }
            }
        }

        internal static void SendNodeMessage(string name, NodeMessage msg)
        {
            if (s_Inited) {
                byte[] data = BuildNodeMessage(msg);
                if (null != name && null != data) {
                    CenterHubApi.SendByName(s_WorldId, name, data, data.Length);
                }
            }
        }

        internal static void ForwardMessage(ulong handle, byte[] data)
        {
            if (s_Inited) {
                if (null != data) {
                    CenterHubApi.SendByHandle(s_WorldId, handle, data, data.Length);
                }
            }
        }

        internal static void ForwardMessage(string name, byte[] data)
        {
            if (s_Inited) {
                if (null != name && null != data) {
                    CenterHubApi.SendByName(s_WorldId, name, data, data.Length);
                }
            }
        }

        internal static byte[] BuildNodeMessage(NodeMessage msg)
        {
            byte[] msgData = Encoding.Encode(msg.m_ID, msg.m_NodeHeader, msg.m_ProtoData);
            StringBuilder sb = new StringBuilder();
            sb.Append(Convert.ToBase64String(msgData));
            sb.Append('\0');
            byte[] data = System.Text.Encoding.ASCII.GetBytes(sb.ToString());
            return data;
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

        private static int s_WorldId = 0;
        private static bool s_Inited = false;
        private static NodeMessageFilterDelegate s_MessageFilter = null;
        private static NodeMessageHandlerInfo[] s_MessageHandlers = null;
        [ThreadStatic]
        private static ProtoNetEncoding s_Encoding = null;
    }
    internal class JsonGmMessageDispatcher
    {
        internal static void Init(int worldId)
        {
            if (!s_Inited) {
                s_WorldId = worldId;
                s_MessageHandlers = new JsonMessageHandlerInfo[(int)LobbyGmMessageDefine.MaxNum];
                for (int i = 0; i < (int)LobbyGmMessageDefine.MaxNum; ++i) {
                    s_MessageHandlers[i] = new JsonMessageHandlerInfo();
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

        internal static void RegisterMessageHandler(int id, Type protoType, JsonMessageHandlerDelegate handler)
        {
            if (s_Inited) {
                if (id >= 0 && id < (int)LobbyGmMessageDefine.MaxNum) {
                    s_MessageHandlers[id].m_ProtoType = protoType;
                    s_MessageHandlers[id].m_Handler = handler;
                }
            }
        }

        internal static void HandleNodeMessage(
            uint seq,
            ulong source_handle,
            ulong dest_handle,
            byte[] data)
        {
            if (s_Inited) {
                JsonMessage msg = DecodeJsonMessage(data);
                if (null != msg) {
                    HandleNodeMessage(msg, source_handle, seq);
                }
            }
        }

        internal static void HandleNodeMessage(JsonMessage msg, ulong handle, uint session)
        {
            if (s_Inited && msg != null) {
                //LogSys.Log(LOG_TYPE.DEBUG, "Handle Json Message:{0}={1}", msg.m_ID, msg.GetType().Name);

                JsonMessageHandlerInfo info = s_MessageHandlers[(int)msg.m_ID];
                if (info != null && info.m_Handler != null) {
                    info.m_Handler(msg, handle, session);
                }
            }
        }

        internal static JsonMessage DecodeJsonMessage(byte[] data)
        {
            JsonMessage msg = null;
            if (s_Inited) {
                string msgStr = null;
                try {
                    msgStr = System.Text.Encoding.UTF8.GetString(data);
                    int ix = msgStr.IndexOf('|');
                    if (ix > 0) {
                        int id = int.Parse(msgStr.Substring(0, ix));
                        int ix2 = msgStr.IndexOf('|', ix + 1);
                        msg = new JsonMessage(id);
                        if (ix2 > 0) {
                            string jsonStr = msgStr.Substring(ix + 1, ix2 - ix - 1);
                            string protoStr = msgStr.Substring(ix2 + 1);
                            msg.m_JsonData = JsonMapper.ToObject(jsonStr);
                            Type t = s_MessageHandlers[id].m_ProtoType;
                            if (null != t) {
                                byte[] bytes = Convert.FromBase64String(protoStr);
                                msg.m_ProtoData = Encoding.Decode(t, bytes);
                            }
                        }
                        else {
                            string jsonStr = msgStr.Substring(ix + 1);
                            msg.m_JsonData = JsonMapper.ToObject(jsonStr);
                        }
                    }
                }
                catch (Exception ex) {
                    if (null == msgStr) {
                        LogSys.Log(ServerLogType.ERROR, "[Exception] DecodeJsonMessage:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                    else {
                        LogSys.Log(ServerLogType.ERROR, "[Exception] DecodeJsonMessage:{0} {1}\n{2}", msgStr, ex.Message, ex.StackTrace);
                    }
                }
            }
            return msg;
        }

        internal static Type GetMessageProtoType(int id)
        {
            Type type = null;
            if (id >= 0 && id < (int)LobbyGmMessageDefine.MaxNum) {
                type = s_MessageHandlers[id].m_ProtoType;
            }
            return type;
        }

        internal static void SendNodeMessage(ulong handle, JsonMessage msg)
        {
            if (s_Inited) {
                byte[] data = BuildNodeMessage(msg);
                if (null != data) {
                    CenterHubApi.SendByHandle(s_WorldId, handle, data, data.Length);
                }
            }
        }

        internal static void SendNodeMessage(string name, JsonMessage msg)
        {
            if (s_Inited) {
                byte[] data = BuildNodeMessage(msg);
                if (null != name && null != data) {
                    CenterHubApi.SendByName(s_WorldId, name, data, data.Length);
                }
            }
        }

        internal static void ForwardMessage(ulong handle, byte[] data)
        {
            if (s_Inited) {
                if (null != data) {
                    CenterHubApi.SendByHandle(s_WorldId, handle, data, data.Length);
                }
            }
        }

        internal static void ForwardMessage(string name, byte[] data)
        {
            if (s_Inited) {
                if (null != name && null != data) {
                    CenterHubApi.SendByName(s_WorldId, name, data, data.Length);
                }
            }
        }

        internal static byte[] BuildNodeMessage(JsonMessage msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(msg.m_ID);
            sb.Append('|');
            sb.Append(JsonMapper.ToJson(msg.m_JsonData));
            if (null != msg.m_ProtoData) {
                byte[] bytes = Encoding.Encode(msg.m_ProtoData);
                sb.Append('|');
                sb.Append(Convert.ToBase64String(bytes));
            }
            return System.Text.Encoding.UTF8.GetBytes(sb.ToString());
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

        private static int s_WorldId = 0;
        private static bool s_Inited = false;
        private static JsonMessageHandlerInfo[] s_MessageHandlers = null;

        [ThreadStatic]
        private static ProtoNetEncoding s_Encoding = null;
    }
}

