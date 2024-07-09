using System;
using System.Collections.Generic;
using GameFrameworkMessage;

namespace ScriptableFramework
{
    public class ClientInfo
    {
        public static ClientInfo Instance
        {
            get { return s_Instance; }
        }
        private static ClientInfo s_Instance = new ClientInfo();
        
        public ulong Guid
        {
            get { return m_Guid; }
            set { m_Guid = value; }
        }
        public RoleEnterResult RoleData
        {
            get { return m_RoleData; }
            set { m_RoleData = value; }
        }
        public List<MailInfoForMessage> Mails
        {
            get { return m_Mails; }
            set { m_Mails = value; }
        }
        public int PropertyKey
        {
            get { return m_PropertyKey; }
            set { m_PropertyKey = value; }
        }

        private ulong m_Guid = 0;
        private RoleEnterResult m_RoleData = new RoleEnterResult();
        private List<MailInfoForMessage> m_Mails = new List<MailInfoForMessage>();
        private int m_PropertyKey = 1;
    }
}
