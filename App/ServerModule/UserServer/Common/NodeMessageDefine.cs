using System;
using System.IO;
using LitJson;
using ScriptableFramework;
using GameFrameworkMessage;

namespace ScriptableFramework
{
    //----------------------------------------------------------------------------------------------
    // node messages from client
    //----------------------------------------------------------------------------------------------
    internal sealed class NodeMessage
    {
        internal int m_ID = -1;
        internal object m_NodeHeader = null;
        internal object m_ProtoData = null;
        internal byte[] m_OriginalMsgData = null;

        internal NodeMessage(int id)
        {
            m_ID = id;
        }
        internal NodeMessage(LobbyMessageDefine id)
        {
            m_ID = (int)id;
        }
        internal NodeMessage(LobbyMessageDefine id, string account)
        {
            m_ID = (int)id;
            SetHeaderWithAccount(account);
        }
        internal NodeMessage(LobbyMessageDefine id, ulong guid)
        {
            m_ID = (int)id;
            SetHeaderWithGuid(guid);
        }
        internal NodeMessage(LobbyMessageDefine id, string account, ulong guid)
        {
            m_ID = (int)id;
            SetHeaderWithAccountAndGuid(account, guid);
        }
        internal void SetHeaderWithAccount(string account)
        {
            NodeMessageWithAccount header = m_NodeHeader as NodeMessageWithAccount;
            if (null == header) {
                header = new NodeMessageWithAccount();
                m_NodeHeader = header;
            }
            header.m_Account = account;
        }
        internal void SetHeaderWithGuid(ulong guid)
        {
            NodeMessageWithGuid header = m_NodeHeader as NodeMessageWithGuid;
            if (null == header) {
                header = new NodeMessageWithGuid();
                m_NodeHeader = header;
            }
            header.m_Guid = guid;
        }
        internal void SetHeaderWithAccountAndGuid(string account, ulong guid)
        {
            NodeMessageWithAccountAndGuid header = m_NodeHeader as NodeMessageWithAccountAndGuid;
            if (null == header) {
                header = new NodeMessageWithAccountAndGuid();
                m_NodeHeader = header;
            }
            header.m_Account = account;
            header.m_Guid = guid;
        }
    }
    internal delegate bool NodeMessageFilterDelegate(NodeMessage msg, ulong handle, uint seq);
    internal delegate void NodeMessageHandlerDelegate(NodeMessage msg, ulong handle, uint seq);

    //----------------------------------------------------------------------------------------------
    // node messages from GM tools
    //----------------------------------------------------------------------------------------------
    internal sealed class JsonMessage
    {
        internal int m_ID = -1;
        internal JsonData m_JsonData = null;
        internal object m_ProtoData = null;
        internal byte[] m_OriginalMsgData = null;

        internal JsonMessage(int id)
        {
            m_ID = id;
        }
        internal JsonMessage(LobbyGmMessageDefine id)
        {
            m_ID = (int)id;
            m_JsonData = new JsonData();
        }
        internal JsonMessage(LobbyGmMessageDefine id, string account)
        {
            m_ID = (int)id;
            m_JsonData = new JsonData();
            m_JsonData["m_Account"] = account;
        }
        internal string Account
        {
            get
            {
                return (string)m_JsonData["m_Account"];
            }
            set
            {
                m_JsonData["m_Account"] = value;
            }
        }
    }

    internal delegate bool JsonMessageFilterDelegate(JsonMessage msg, ulong handle, uint seq);
    internal delegate void JsonMessageHandlerDelegate(JsonMessage msg, ulong handle, uint seq);
}