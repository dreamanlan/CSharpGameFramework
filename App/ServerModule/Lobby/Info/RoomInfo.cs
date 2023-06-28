using System;
using System.Collections.Generic;
using Messenger;
using CSharpCenterClient;
using GameFrameworkMessage;
using GameFramework;

namespace Lobby
{
    internal enum RoomState
    {
        Prepare,    //准备
        Game,       //游戏中
        Close,      //房间关闭
        Recycle,    //回收状态(未激活)
    }

    internal class RoomInfo
    {
        internal RoomInfo()
        {
            this.CurrentState = RoomState.Recycle;
        }
        internal void Reset()
        {
            this.CurrentState = RoomState.Recycle;
            m_NextCampId = (int)CampIdEnum.FreedomCamp_Begin;
        }
        internal LobbyInfo LobbyInfo
        {
            get { return m_LobbyInfo; }
            set { m_LobbyInfo = value; }
        }
        internal bool IsEmpty
        {
            get { return UserCount == 0; }
        }
        internal int UserCount
        {
            get { return m_Users.Count; }
        }
        internal int UserCountOfBlue
        {
            get { return m_UserCountOfBlue; }
        }
        internal int UserCountOfRed
        {
            get { return m_UserCountOfRed; }
        }
        internal string RoomServerName
        {
            get { return m_RoomServerName; }
            set { m_RoomServerName = value; }
        }
        internal ulong Creator
        {
            get { return m_Creator; }
            set
            {
                m_Creator = value;
                UserInfo info = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(m_Creator);
                if (null != info) {
                    m_CreatorNick = info.Nickname;
                }
            }
        }
        internal string CreatorNick
        {
            get { return m_CreatorNick; }
        }
        internal int RoomId
        {
            get { return m_RoomId; }
            set { m_RoomId = value; }
        }
        internal int SceneType
        {
            get { return m_SceneType; }
            set { m_SceneType = value; }
        }
        public bool IsField
        {
            get { return m_IsField; }
            set { m_IsField = value; }
        }
        internal int TotalCount
        {
            get { return m_TotalCount; }
            set { m_TotalCount = value; }
        }
        internal float LifeTime
        {
            get { return m_LifeTime; }
            set { m_LifeTime = value; }
        }
        internal Dictionary<ulong, WeakReference> Users
        {
            get { return m_Users; }
        }
        internal RoomState CurrentState
        { get; set; }
        internal DateTime StartTime
        { get; set; }
        internal DateTime EndTime
        { get; set; }

        internal int GenNextCampId()
        {
            return m_NextCampId++;
        }
        internal int CalcUserCount(int campId)
        {
            int ct = 0;
            foreach (KeyValuePair<ulong, WeakReference> pair in m_Users) {
                WeakReference info = pair.Value;
                UserInfo userInfo = info.Target as UserInfo;
                if (userInfo != null) {
                    if (userInfo.CampId == campId)
                        ++ct;
                }
            }
            return ct;
        }

        internal UserInfo GetUserInfo(ulong guid)
        {
            UserInfo info = null;
            WeakReference weakRef;
            if (m_Users.TryGetValue(guid, out weakRef)) {
                info = weakRef.Target as UserInfo;
            }
            return info;
        }

        internal void AddUsers(int camp, params ulong[] users)
        {
            UserProcessScheduler dataProcess = LobbyServer.Instance.UserProcessScheduler;
            foreach (ulong user in users) {
                UserInfo info = dataProcess.GetUserInfo(user);
                if (info != null) {
                    info.LeftLife = UserInfo.c_LifeTimeWaitStartGame;

                    info.Room = this;
                    info.CampId = camp;
                    if (m_Users.ContainsKey(user)) {
                        m_Users[user] = new WeakReference(info);
                    } else {
                        m_Users.Add(user, new WeakReference(info));
                    }
                }
            }
            UpdateUserCount();
        }
        internal void AddMachine(int camp, UserInfo machine)
        {
            if (machine != null) {
                machine.Room = this;
                machine.CampId = camp;
                if (m_Users.ContainsKey(machine.Guid)) {
                    m_Users[machine.Guid] = new WeakReference(machine);
                } else {
                    m_Users.Add(machine.Guid, new WeakReference(machine));
                }
            }
            UpdateUserCount();
        }

        internal void DelUsers(params ulong[] users)
        {
            UserProcessScheduler dataProcess = LobbyServer.Instance.UserProcessScheduler;
            foreach (ulong user in users) {
                UserInfo info = dataProcess.GetUserInfo(user);
                if (null != info) {
                    info.ResetRoomInfo();
                    LogSys.Log(ServerLogType.DEBUG, "LeaveRoom:{0}", user);
                }
                m_Users.Remove(user);
            }
            UpdateUserCount();
        }
        
        internal void Tick()
        {
            //清除不在游戏中的玩家数据
            if (UserCount > 0) {
                m_RecycledGuids.Clear();
                foreach (KeyValuePair<ulong, WeakReference> pair in m_Users) {
                    ulong guid = pair.Key;
                    UserInfo info = pair.Value.Target as UserInfo;
                    if (info == null || info.IsRecycled || info.CurrentState == UserState.DropOrOffline || info.Guid != guid) {
                        if (null != info)
                            info.ResetRoomInfo();
                        m_RecycledGuids.Add(guid);
                        LogSys.Log(ServerLogType.DEBUG, "Room {0} has a exception user {1} !!!", m_RoomId, guid);
                    }
                }
                if (m_RecycledGuids.Count > 0) {
                    foreach (ulong guid in m_RecycledGuids) {
                        m_Users.Remove(guid);
                    }
                    UpdateUserCount();
                }
            }
            if (IsField) {
                if (CurrentState != RoomState.Game)
                    CurrentState = RoomState.Game;
            }
        }
        
        internal void CloseRoom()
        {
            //清空房间内玩家数据 
            foreach (KeyValuePair<ulong, WeakReference> pair in m_Users) {
                WeakReference info = pair.Value;
                UserInfo user = info.Target as UserInfo;
                if (user != null) {
                    if (user.CurrentState == UserState.Room) {
                        user.CurrentState = UserState.Online;
                        user.LeftLife = UserInfo.c_LifeTimeWaitOffline;
                    }
                    user.ResetRoomInfo();
                }
            }
            m_Users.Clear();
            this.CurrentState = RoomState.Close;  //房间进入关闭状态
            LogSys.Log(ServerLogType.INFO, "Lobby Room Close, roomID:{0}", RoomId);
        }

        private string m_RoomServerName = "";
        private ulong m_Creator = 0;
        private string m_CreatorNick = "";
        private int m_RoomId = 0;
        private bool m_IsField = false;
        private int m_SceneType = 0;
        private int m_TotalCount = 0;
        private float m_LifeTime = 0;
        private int m_UserCountOfBlue = 0;
        private int m_UserCountOfRed = 0;
        private int m_NextCampId = (int)CampIdEnum.FreedomCamp_Begin;

        private HashSet<ulong> m_RecycledGuids = new HashSet<ulong>();
        private LobbyInfo m_LobbyInfo = null;

        private Dictionary<ulong, WeakReference> m_Users = new Dictionary<ulong, WeakReference>();

        GameFramework.Network.ProtoNetEncoding m_Encoding = new GameFramework.Network.ProtoNetEncoding();

        private void UpdateUserCount()
        {
            m_UserCountOfBlue = CalcUserCount((int)CampIdEnum.Blue);
            m_UserCountOfRed = CalcUserCount((int)CampIdEnum.Red);
        }
    }
}

