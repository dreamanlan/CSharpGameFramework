using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class ClientInfo
    {
        public static ClientInfo Instance
        {
            get { return s_Instance; }
        }
        private static ClientInfo s_Instance = new ClientInfo();

        public RoleInfo CurrentRole
        {
            get { return m_CurrentRole; }
            set { m_CurrentRole = value; }
        }
        public int WorldId
        {
            get { return m_WorldId; }
            set { m_WorldId = value; }
        }

        public int PropertyKey
        {
            get { return m_PropertyKey; }
            set { m_PropertyKey = value; }
        }

        private int m_WorldId = 0;
        private int m_PropertyKey = 1;
        private RoleInfo m_CurrentRole = null;              //当前游戏的玩家角色
    }
}
