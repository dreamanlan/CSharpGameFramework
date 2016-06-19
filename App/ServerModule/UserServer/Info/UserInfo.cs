using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using GameFramework;
using GameFrameworkMessage;

namespace GameFramework
{
    internal enum BroadcastType : int
    {
        Chat = 1,//聊天公频
        Roll = 2,//滚动公告
        Both = 3,//聊天加滚动
    }
    internal enum UserState : int
    {
        Online = 0,           //玩家登录，但并未加入游戏
        Room,                 //玩家在多人副本游戏中，PvP或多人PvE
        DropOrOffline,        //掉线或离线状态（逻辑上区分掉线与离线意义不大）
    }
    public sealed partial class UserInfo
    {
        internal const int LifeTimeOfNoHeartbeat = 120000;    //玩家离线，如果心跳停止超过这个值认为玩家离线   
        internal UserInfo()
        {
        }
        internal UserState CurrentState
        {
            get { return m_CurrentState; }
            set { m_CurrentState = value; }
        }
        internal int NextUserSaveCount
        {
            get { return m_NextUserSaveCount; }
            set
            {
                if (m_NextUserSaveCount != DataCacheThread.UltimateSaveCount) {
                    m_NextUserSaveCount = value;
                }
            }
        }
        internal int CurrentUserSaveCount
        {
            get { return m_CurrentUserSaveCount; }
            set
            {
                if (m_NextUserSaveCount == DataCacheThread.UltimateSaveCount) {
                    if (value == DataCacheThread.UltimateSaveCount) {
                        m_CurrentUserSaveCount = value;
                    }
                } else {
                    if (m_CurrentUserSaveCount != DataCacheThread.UltimateSaveCount && m_CurrentUserSaveCount < value) {
                        m_CurrentUserSaveCount = value;
                    }
                }
            }
        }

        //基本数据
        internal string NodeName
        {
            get { return m_NodeName; }
            set { m_NodeName = value; }
        }
        internal uint Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        internal int UserThreadIndex
        {
            get { return m_UserThreadIndex; }
            set { m_UserThreadIndex = value; }
        }
        internal bool IsDisconnected
        {
            get { return m_IsDisconnected; }
            set { m_IsDisconnected = value; }
        }
        internal int CampId
        {
            get { return m_CampId; }
            set { m_CampId = value; }
        }
        internal int LeftLife
        {
            get { return m_LeftLife; }
            set { m_LeftLife = value; }
        }
        internal long LastSaveTime
        {
            get { return m_LastSaveTime; }
            set { m_LastSaveTime = value; }
        }
        internal bool IsRecycled
        {
            get { return m_IsRecycled; }
            set { m_IsRecycled = value; }
        }
        internal object Lock
        {
            get { return m_Lock; }
        }
        internal double LastLoginTime
        {
            get { return m_LastLoginTime; }
            set { m_LastLoginTime = value; }
        }
        internal string ClientInfo
        {
            get { return m_ClientInfo; }
            set { m_ClientInfo = value; }
        }
        internal void ResetMoney()
        {
            m_Money = 0;
        }
        internal void SetMoneyForDB(int money)
        {
            m_Money = money;
        }
        internal void IncreaseMoney(int money)
        {
            if (money > 0) {
                if (Interlocked.Add(ref m_Money, money) < 0) {
                    m_Money = 0;
                }
            }
        }
        internal void DecreaseMoney(int money)
        {
            if (money > 0) {
                if (Interlocked.Add(ref m_Money, -money) < 0) {
                    m_Money = 0;
                }
            }
        }
        internal void ResetGold()
        {
            m_Gold = 0;
        }
        internal void SetGoldForDB(int gold)
        {
            m_Gold = gold;
        }
        internal void IncreaseGold(int gold)
        {
            if (gold > 0) {
                if (Interlocked.Add(ref m_Gold, gold) < 0) {
                    m_Gold = 0;
                }
            }
        }
        internal void DecreaseGold(int gold)
        {
            if (gold > 0) {
                if (Interlocked.Add(ref m_Gold, -gold) < 0) {
                    m_Gold = 0;
                }
            }
        }
        internal bool CheckLevelup()
        {
            return false;
        }
        internal bool CanUseGmCommand
        {
            get
            {
                return GlobalVariables.Instance.IsDebug;
            }
        }
        internal void Reset()
        {
            this.Guid = 0;
            this.Level = 1;
            this.AccountId = string.Empty;
            this.Nickname = string.Empty;
            this.SceneId = 0;

            this.m_LastLoginTime = 0;
            this.m_Money = 0;
            this.m_Gold = 0;

            this.m_ClientInfo = "0";
            this.NodeName = string.Empty;
            this.UserThreadIndex = 0;
            this.Key = 0;
            this.CampId = 0;

            this.IsDisconnected = false;
            this.LeftLife = 0;
            this.CurrentState = UserState.DropOrOffline;            
            this.LastSaveTime = 0;
            this.m_NextUserSaveCount = 1;
            this.m_CurrentUserSaveCount = 0;

            m_MemberInfos.Clear();
            m_FriendInfos.Clear();
            m_ItemBag.Reset();
            m_MailStateInfo.Reset();
        }

        internal List<MemberInfo> MemberInfos
        {
            get { return m_MemberInfos; }
        }
        public List<FriendInfo> FriendInfos
        {
            get { return m_FriendInfos; }
        }
        public ItemBag ItemBag
        {
            get { return m_ItemBag; }
        }
        public MailStateInfo MailStateInfo
        {
            get { return m_MailStateInfo; }
        }

        //==================================================================================

        private List<MemberInfo> m_MemberInfos = new List<MemberInfo>();
        private List<FriendInfo> m_FriendInfos = new List<FriendInfo>();
        private ItemBag m_ItemBag = new ItemBag();
        private MailStateInfo m_MailStateInfo = new MailStateInfo();
        
        private double m_LastLoginTime = 0.0;
        private int m_Money = 0;
        private int m_Gold = 0;
        
        private string m_NodeName;
        private int m_UserThreadIndex = 0;
        private uint m_Key = 0;
        private int m_CampId = 0;
        private string m_ClientInfo = "0";

        private bool m_IsDisconnected = false;
        private int m_LeftLife = 0;
        private bool m_IsRecycled = false;
        private UserState m_CurrentState = UserState.DropOrOffline;
        private long m_LastSaveTime = 0;
        private int m_NextUserSaveCount = 1;
        private int m_CurrentUserSaveCount = 0;

        private object m_Lock = new object();
    }
}

