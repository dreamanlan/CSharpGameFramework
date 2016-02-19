using System;

namespace Lobby
{
  internal class RoomServerInfo
  {
    internal RoomServerInfo ()
    {
    }

    internal string RoomServerName
    {
      get { return m_RoomServerName; }
      set { m_RoomServerName = value; }
    }

    internal int IdleRoomNum
    {
      get {
        return m_IdleRoomNum;
      }
      set {
        m_IdleRoomNum = value;
      }
    }

    internal int MaxRoomNum
    {
      get {
        return m_MaxRoomNum;
      }
      set {
        m_MaxRoomNum = value;
      }
    }

    internal int UserNum
    {
      get {
        return m_UserNum;
      }
      set {
        m_UserNum = value;
      }
    }

    internal string ServerIp
    {
      get {
        return m_ServerIp;
      }
      set {
        m_ServerIp = value;
      }
    }

    internal uint ServerPort
    {
      get {
        return m_ServerPort;
      }
      set {
        m_ServerPort = value;
      }
    }

    private string m_RoomServerName = "";
    private int m_IdleRoomNum = 0;
    private int m_MaxRoomNum = 0;
    private int m_UserNum = 0;

    private string m_ServerIp="127.0.0.1";
    private uint m_ServerPort=5321;
  }
}

