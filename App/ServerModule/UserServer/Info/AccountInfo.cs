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
    internal class AccountInfo
    {
        internal string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        internal string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }
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
        internal ulong UserGuid
        {
            get { return m_UserGuid; }
            set { m_UserGuid = value; }
        }
        internal TableAccountWrap TableAccount
        {
            get { return m_TableAccount; }
        }
        //玩家账号数据    
        private string m_AccountKey = string.Empty;       //玩家账号标识，玩家当前设备标识
        private string m_AccountId = string.Empty;        //玩家账号ID，由畅游账号平台返回
        private string m_Password = string.Empty;              //玩家账号在第三方平台的ID
        private string m_ClientInfo = string.Empty;
        private AccountState m_CurrentState = AccountState.Offline;
        private ulong m_UserGuid = 0;
        private double m_LastLoginTime = 0;
        private string m_NodeName;

        private TableAccountWrap m_TableAccount = new TableAccountWrap();   //账号信息
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
