using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using GameFramework;
using GameFrameworkMessage;

namespace Lobby
{
    internal enum BroadcastType : int
    {
        Chat = 1,//Chat public channel
        Roll = 2,//rolling announcement
        Both = 3,//chat plus scroll
    }
    internal enum UserState : int
    {
        Online = 0,           //Player is logged in but not joining the game
        Room,                 //Players in multiplayer dungeon games, PvP or multiplayer PvE
        DropOrOffline,        //Offline or offline status (logically, it makes little sense
                              //to distinguish between offline and offline)
    }
    internal class UserInfo
    {
        internal const int c_NextLifeTime = 300000;
        internal const int c_LifeTimeWaitStartGame = 60000;
        internal const int c_LifeTimeWaitOffline = 20000;

        internal UserState CurrentState
        {
            get { return m_CurrentState; }
            set { m_CurrentState = value; }
        }
        //basic data
        internal uint Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        internal int CampId
        {
            get { return m_CampId; }
            set { m_CampId = value; }
        }
        internal DateTime StartServerTime
        {
            get { return m_StartServerTime; }
            set { m_StartServerTime = value; }
        }
        internal string UserSvrName
        {
            get { return m_UserSvrName; }
            set { m_UserSvrName = value; }
        }
        internal string AccountId
        {
            get { return null == m_BaseUserInfo ? "0" : m_BaseUserInfo.AccountId; }
        }
        internal int WorldId
        {
            get { return null == m_BaseUserInfo ? 0 : m_BaseUserInfo.WorldId; }
        }
        internal string NodeName
        {
            get { return null == m_BaseUserInfo ? "" : m_BaseUserInfo.NodeName; }
        }
        internal int SceneId
        {
            get { return null == m_BaseUserInfo ? 0 : m_BaseUserInfo.SceneId; }
            set { if (null != m_BaseUserInfo) m_BaseUserInfo.SceneId = value; }
        }
        internal string ClientInfo
        {
            get { return null == m_BaseUserInfo ? "0" : m_BaseUserInfo.ClientInfo; }
        }
        internal ulong Guid
        {
            get { return null == m_RoomUserInfo ? 0 : m_RoomUserInfo.Guid; }
        }
        internal string Nickname
        {
            get { return null == m_RoomUserInfo ? "Unknown" : m_RoomUserInfo.Nick; }
        }
        internal int HeroId
        {
            get { return null == m_RoomUserInfo ? 0 : m_RoomUserInfo.Hero; }
        }
        //game data
        internal int Level
        {
            get { return null == m_RoomUserInfo ? 0 : m_RoomUserInfo.Level; }
        }

        public Msg_LB_BigworldUserBaseInfo BaseUserInfo
        {
            get { return m_BaseUserInfo; }
            set
            {
                m_BaseUserInfo = value;
                if (null != m_BaseUserInfo) {
                    m_StartServerTime = DateTime.ParseExact(m_BaseUserInfo.StartServerTime, "yyyyMMdd", null);
                    m_UserSvrName = string.Format("UserSvr{0}", m_BaseUserInfo.WorldId);
                }
            }
        }
        internal Msg_LR_RoomUserInfo RoomUserInfo
        {
            get { return m_RoomUserInfo; }
            set { m_RoomUserInfo = value; }
        }
        //Player's current game data
        internal RoomInfo Room
        {
            get
            {
                if (m_Room == null)
                    return null;
                else
                    return m_Room.Target as RoomInfo;
            }
            set
            {
                if (value == null)
                    m_Room = null;
                else
                    m_Room = new WeakReference(value);
            }
        }
        internal int CurrentRoomID
        {
            get { return m_curRoomID; }
            set { m_curRoomID = value; }
        }
        //Other
        internal int LeftLife
        {
            get { return m_LeftLife; }
            set { m_LeftLife = value; }
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
        internal void ResetRoomInfo()
        {
            this.Room = null;
            this.CurrentRoomID = 0;
        }
        internal void Reset()
        {
            this.Key = 0;
            this.CampId = 0;
            this.StartServerTime = DateTime.Now;
            this.UserSvrName = "";

            this.Room = null;
            this.CurrentRoomID = 0;
            this.CurrentState = UserState.DropOrOffline;
            this.LeftLife = 0;
        }

        private uint m_Key = 0;
        private int m_CampId = 0;
        private string m_UserSvrName = "";
        private DateTime m_StartServerTime = DateTime.Now;

        private Msg_LB_BigworldUserBaseInfo m_BaseUserInfo = null;
        private Msg_LR_RoomUserInfo m_RoomUserInfo = null;

        private int m_LeftLife = 0;
        private bool m_IsRecycled = false;
        private object m_Lock = new object();

        private WeakReference m_Room = null;
        private int m_curRoomID = 0;

        private UserState m_CurrentState = UserState.DropOrOffline;
    }
}

