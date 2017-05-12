using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using GameFramework;
using GameFrameworkData;

namespace GameFramework
{
    internal enum AccountState : int
    {
        Unloaded,     //账号数据未加载    
        Online,       //账号在线
        Dropped,      //账号掉线
        Offline,      //账号离线
    }
    /// <summary>
    /// 玩家账号数据
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
        //玩家账号数据    
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
