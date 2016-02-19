using System;
using GameFramework;
using GameFrameworkMessage;

namespace Lobby
{
  //----------------------------------------------------------------------------------------------
  // 来自客户端的node消息
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
  internal delegate bool NodeMessageFilterDelegate(NodeMessage msg, int handle, uint seq);
  internal delegate void NodeMessageHandlerDelegate(NodeMessage msg, int handle, uint seq);
}