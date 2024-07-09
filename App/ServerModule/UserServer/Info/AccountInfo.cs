using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using ScriptableFramework;
using GameFrameworkData;

namespace ScriptableFramework
{
    internal enum AccountState : int
    {
        Unloaded,     //Account data not loaded
        Online,       //Account is online
        Dropped,      //Account is dropped
        Offline,      //Account is offline
    }
    /// <summary>
    /// Player account data
    /// </summary>
    public partial class AccountInfo
    {
        internal string ClientInfo
        {
            get { return m_ClientInfo; }
            set { m_ClientInfo = value; }
        }
        internal string NodeName
        {
            get { return m_NodeName; }
            set { m_NodeName = value; }
        }
        internal double LastLoginTime
        {
            get { return m_LastLoginTime; }
            set { m_LastLoginTime = value; }
        }
        internal AccountState CurrentState
        {
            get { return m_CurrentState; }
            set { m_CurrentState = value; }
        }
        //Player account data
        private string m_AccountKey = string.Empty;
        private string m_ClientInfo = string.Empty;
        private AccountState m_CurrentState = AccountState.Offline;
        private double m_LastLoginTime = 0;
        private string m_NodeName;
    }

    internal class AccountSystem
    {
        internal void AddAccountById(string accountId, AccountInfo accountInfo)
        {
            m_AccountById.AddOrUpdate(accountId, accountInfo, (g, u) => accountInfo);
        }
        internal AccountInfo FindAccountById(string accountId)
        {
            AccountInfo accountInfo = null;
            m_AccountById.TryGetValue(accountId, out accountInfo);
            return accountInfo;
        }
        internal void RemoveAccountById(string accountId)
        {
            AccountInfo tmpAi = null;
            m_AccountById.TryRemove(accountId, out tmpAi);
        }

        private ConcurrentDictionary<string, AccountInfo> m_AccountById = new ConcurrentDictionary<string, AccountInfo>();
    }
}
