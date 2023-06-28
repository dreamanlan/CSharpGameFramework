using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using LitJson;
using GameFrameworkMessage;
using GameFramework;

namespace GameFramework
{
    internal sealed class CitySceneInfo
    {
        internal int m_SceneId = 0;
        internal List<HashSet<ulong>> m_Rooms = new List<HashSet<ulong>>();
    }
    internal class CityRoomInfo
    {
        internal int m_RoomIndex = 0;
        internal int m_UserCount = 0;
        internal int m_TotalUserCount = 0;
        internal int m_SceneId = 0;
    }
    internal sealed class QueueingThread : MyServerThread
    {
        internal sealed class LoginInfo
        {
            public string AccountId;
            public string Password;
            public string ClientInfo;
            public string NodeName;
            public int QueueingNum;
        }
        //=========================================================================================
        //同步调用方法部分，其它线程可直接调用(需要考虑多线程安全)。
        //=========================================================================================
        internal int MaxOnlineUserCountPerLogicServer
        {
            get { return m_MaxOnlineUserCountPerLogicServer; }
        }
        internal int GetQueueingNum(string accountId)
        {
            int num = 0;
            LoginInfo info;
            if (m_QueueingInfos.TryGetValue(accountId, out info)) {
                num = info.QueueingNum - GetEnterCount();
            }
            return num;
        }
        internal bool IsQueueingFull()
        {
            return m_QueueingInfos.Count >= m_MaxQueueingCount;
        }
        internal bool NeedQueueing()
        {
            bool ret = false;
            if (IsLobbyFull()) {
                ret = true;
            }
            return ret;
        }
        //=========================================================================================
        //异步调用方法部分，需要通过QueueAction调用。
        //=========================================================================================
        internal void StartQueueing(string accountId, string password, string clientInfo, string nodeName)
        {
            m_LogoutQueueingAccounts.Remove(accountId);
            LoginInfo info;
            if (m_QueueingInfos.TryGetValue(accountId, out info)) {
                info.AccountId = accountId;
                info.Password = password;
                info.ClientInfo = clientInfo;
                info.NodeName = nodeName;
                return;
            } else {
                info = new LoginInfo();
                info.AccountId = accountId;
                info.Password = password;
                info.ClientInfo = clientInfo;
                info.NodeName = nodeName;
                m_QueueingInfos.AddOrUpdate(accountId, info, (k, i) => info);
            }
            if (null != info) {
                m_QueueingAccounts.AddLast(accountId);
                info.QueueingNum = m_QueueingAccounts.Count + GetEnterCount();
            } else {
                //out of memory
            }
        }

        internal void UpdateMaxUserCount(int maxUserCount, int maxUserCountPerLogicServer, int maxQueueingCount, int gowRobotType)
        {
            m_MaxOnlineUserCount = maxUserCount;
            m_MaxOnlineUserCountPerLogicServer = maxUserCountPerLogicServer;
            m_MaxQueueingCount = maxQueueingCount;
            LogSys.Log(ServerLogType.WARN, "UpdateMaxUserCount {0} {1} {2} {3}", maxUserCount, maxUserCountPerLogicServer, maxQueueingCount, gowRobotType);
        }
        internal void TryAddLogoutQueueingAccount(string accountId)
        {
            if (!m_LogoutQueueingAccounts.Contains(accountId) && m_QueueingInfos.ContainsKey(accountId)) {
                m_LogoutQueueingAccounts.Add(accountId);
            }
        }

        protected override void OnStart()
        {
            TickSleepTime = 100;
            ActionNumPerTick = 1024;
        }
        protected override void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime != 0) {
                long elapsedTickTime = curTime - m_LastTickTime;
                if (elapsedTickTime > c_WarningTickTime) {
                    LogSys.Log(ServerLogType.MONITOR, "QueueingThread Tick:{0}", elapsedTickTime);
                }
            }
            m_LastTickTime = curTime;

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;
                DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "QueueingThread.ActionQueue {0}", msg);
                });
                LogSys.Log(ServerLogType.MONITOR, "QueueingThread.ActionQueue Current Action {0}", this.CurActionNum);
            }

            const int c_MaxIterationPerTick = 10;
            if (IsLobbyFull() || GetTotalQueueingCount() <= 0) {
                //大厅已经满或者没有排队的玩家，多休息1秒
                System.Threading.Thread.Sleep(1000);
            } else {
                UserProcessScheduler dataProcess = UserServer.Instance.UserProcessScheduler;
                for (int i = 0; i < c_MaxIterationPerTick; ++i) {
                    if (m_QueueingAccounts.Count > 0 && null != m_QueueingAccounts.First && CanEnter()) {
                        string accountId = m_QueueingAccounts.First.Value;
                        m_QueueingAccounts.RemoveFirst();
                        m_LogoutQueueingAccounts.Remove(accountId);
                        IncEnterCount();
                        LoginInfo info;
                        if (m_QueueingInfos.TryRemove(accountId, out info)) {
                            dataProcess.DefaultUserThread.QueueAction(dataProcess.DoAccountLoginWithoutQueueing, accountId, info.Password, info.ClientInfo, info.NodeName);
                            ++i;
                        }
                    }
                }

                if (m_LastCleanTickTime + c_CleanTickInterval < curTime) {
                    m_LastCleanTickTime = curTime;

                    if (m_QueueingAccounts.Count > 0) {
                        int ct = 0;
                        int removeCount = 0;
                        for (LinkedListNode<string> node = m_QueueingAccounts.First; null != node && m_QueueingAccounts.Count > 0; ) {
                            ++ct;
                            if (ct > m_MaxQueueingCount) {
                                LogSys.Log(ServerLogType.WARN, "QueueingThread.Tick maybe deadlock {0}", m_QueueingAccounts.Count);
                                break;
                            }
                            string accountId = node.Value;
                            LoginInfo info;
                            if (m_LogoutQueueingAccounts.Contains(accountId)) {
                                m_LogoutQueueingAccounts.Remove(accountId);
                                m_QueueingInfos.TryRemove(accountId, out info);

                                ++removeCount;
                                LinkedListNode<string> delNode = node;
                                node = node.Next;
                                m_QueueingAccounts.Remove(delNode);
                            } else if (m_QueueingInfos.TryGetValue(accountId, out info)) {
                                info.QueueingNum -= removeCount;
                                node = node.Next;
                            }
                        }
                    }
                }
            }
        }

        private int GetTotalQueueingCount()
        {
            //注意这个函数不要跨线程调用
            int ct = m_QueueingAccounts.Count;
            return ct;
        }
        private int GetEnterCount()
        {
            return m_EnterCount;
        }
        private void IncEnterCount()
        {
            System.Threading.Interlocked.Increment(ref m_EnterCount);
        }
        private bool IsLobbyFull()
        {
            int totalUserCount = UserServer.Instance.UserProcessScheduler.GetUserCount();
            return totalUserCount >= m_MaxOnlineUserCount;
        }
        private bool CanEnter()
        {
            bool ret = true;
            return ret;
        }

        private ConcurrentDictionary<string, LoginInfo> m_QueueingInfos = new ConcurrentDictionary<string, LoginInfo>();
        private int m_EnterCount = 0;
        private LinkedList<string> m_QueueingAccounts = new LinkedList<string>();

        private HashSet<string> m_LogoutQueueingAccounts = new HashSet<string>();

        private const int c_ReservedCountForRoleSelect = 256;
        private int m_MaxOnlineUserCount = 12000;
        private int m_MaxOnlineUserCountPerLogicServer = 3000;
        private int m_MaxQueueingCount = 2000;

        private const long c_WarningTickTime = 2000;
        private const long c_CleanTickInterval = 3000;
        private long m_LastTickTime = 0;
        private long m_LastCleanTickTime = 0;
        private long m_LastLogTime = 0;
    }
}
