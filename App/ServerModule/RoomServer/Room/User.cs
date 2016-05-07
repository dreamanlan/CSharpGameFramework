using System;
using System.Collections.Generic;
using RoomServer;
using GameFrameworkMessage;
using ScriptRuntime;

namespace GameFramework
{
    internal enum UserControlState : int
    {
        User = 0,
        UserDropped,
        Ai,
        Remove,
    }
    internal class User
    {
        internal User()
        {
            peer_ = new RoomPeer();
            dispatcher_ = new Dispatcher();
            IsEntered = false;
            IsDebug = false;
            IsReady = false;
            m_UserControlState = (int)GameFramework.UserControlState.User;
        }

        internal void Reset()
        {
            peer_.Reset();
            OwnRoom = null;
            IsEntered = false;
            IsReady = false;
            IsDebug = false;
            m_UserControlState = (int)GameFramework.UserControlState.User;

            m_HaveHpArmor = false;
            m_Hp = 0;
            m_Energy = 0;

            m_HaveEnterPosition = false;
            m_EnterX = 0;
            m_EnterY = 0;

            m_LastIsMoving = false;
            m_LastSampleTime = 0;
            m_LastClientPosition = Vector3.Zero;
            m_LastMoveVelocity = 0;
            m_LastMoveDirCosAngle = 1;
            m_LastMoveDirSinAngle = 0;
            m_TimeCounter = 0;
            m_CharacterCreateTime = 0;
        }

        internal void Init()
        {
            dispatcher_.SetClientDefaultHandler(DefaultMsgHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_Enter, EnterHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_Quit, Msg_CR_QuitHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_UserMoveToPos, Msg_CR_UserMoveToPosHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_Skill, UseSkillHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_StopSkill, Msg_CR_StopSkillHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_OperateMode, Msg_CR_OperateModeHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_GiveUpBattle, Msg_CR_GiveUpBattleHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_SwitchDebug, SwitchDebugHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_DlgClosed, Msg_CR_DlgClosedHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CR_GmCommand, Msg_CR_GmCommandHandler.Execute);
            dispatcher_.RegClientMsgHandler(RoomMessageDefine.Msg_CRC_StoryMessage, StoryMessageHandler.Execute);
        }

        internal void RegisterObservers(IList<Observer> observers)
        {
            peer_.RegisterObservers(observers);
        }

        internal bool SetKey(uint key)
        {
            return peer_.SetKey(key);
        }
        internal uint GetKey()
        {
            return peer_.GetKey();
        }

        internal bool ReplaceDroppedUser(ulong replacer, uint key)
        {
            if (null != m_LobbyUserData) {
                m_LobbyUserData.Guid = replacer;
                peer_.Guid = replacer;
            }
            return peer_.UpdateKey(key);
        }

        internal RoomPeer GetPeer()
        {
            return peer_;
        }

        internal bool IsConnected()
        {
            return peer_.IsConnected();
        }

        internal bool IsTimeout()
        {
            return peer_.IsTimeout();
        }

        internal void Disconnect()
        {
            peer_.Disconnect();
        }

        internal long GetElapsedDroppedTime()
        {
            return peer_.GetElapsedDroppedTime();
        }

        internal void SendMessage(RoomMessageDefine id, object msg)
        {
            peer_.SendMessage(id, msg);
        }

        internal void BroadCastMsgToCareList(RoomMessageDefine id, object msg, bool exclude_me = true)
        {
            peer_.BroadCastMsgToCareList(id, msg, exclude_me);
        }

        internal void BroadCastMsgToRoom(RoomMessageDefine id, object msg, bool exclude_me = true)
        {
            peer_.BroadCastMsgToRoom(id, msg, exclude_me);
        }

        internal void AddSameRoomUser(User user)
        {
            peer_.AddSameRoomPeer(user.GetPeer());
        }

        internal void RemoveSameRoomUser(User user)
        {
            peer_.RemoveSameRoomPeer(user.GetPeer());
        }

        internal void ClearSameRoomUser()
        {
            peer_.ClearSameRoomPeer();
        }

        internal void AddCareMeUser(User user)
        {
            peer_.AddCareMePeer(user.GetPeer());
        }

        internal void RemoveCareMeUser(User user)
        {
            peer_.RemoveCareMePeer(user.GetPeer());
        }

        internal void Tick()
        {
            try {
                int id = 0;
                object msg = null;
                while ((msg = peer_.PeekLogicMsg(out id)) != null) {
                    dispatcher_.HandleClientMsg(id, msg, this);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        internal bool LastIsMoving
        {
            get { return m_LastIsMoving; }
            set { m_LastIsMoving = value; }
        }

        internal Vector3 LastClientPosition
        {
            get { return m_LastClientPosition; }
        }

        internal void SampleMoveData(float x, float z, float velocity, float cosDir, float sinDir)
        {
            m_LastClientPosition.X = x;
            m_LastClientPosition.Y = 0;
            m_LastClientPosition.Z = z;
            m_LastMoveVelocity = velocity;
            m_LastMoveDirCosAngle = cosDir;
            m_LastMoveDirSinAngle = sinDir;
            m_LastSampleTime = TimeUtility.GetLocalMilliseconds();

            //LogSys.Log(LOG_TYPE.WARN, "SampleMoveData user:{0}({1},{2},{3}) x:{4} z:{5} v:{6} cos:{7} sin:{8} time:{9}", RoleId, GetKey(), Guid, Name, x, z, velocity, cosDir, sinDir, time);
        }

        internal bool VerifyMovingPosition(float x, float z, float velocity)
        {
            bool ret = true;
            if (m_LastSampleTime > 0) {
                long time = TimeUtility.GetLocalMilliseconds();
                Vector3 pos = new Vector3(x, 0, z);
                float distSqr = Geometry.DistanceSquare(pos, m_LastClientPosition);
                float v = Geometry.Max(velocity, m_LastMoveVelocity);
                float t = (time - m_LastSampleTime) / 1000.0f;
                float enableDist = v * t;
                float enableDistSqr = enableDist * enableDist;
                if (distSqr > 1 && (distSqr > enableDistSqr * 2 + 1/* || distSqr < enableDistSqr / 2 - 1*/)) {
                    ret = false;
                    float sx = m_LastClientPosition.X + enableDist * m_LastMoveDirSinAngle;
                    float sz = m_LastClientPosition.Z + enableDist * m_LastMoveDirCosAngle;

                    LogSys.Log(LOG_TYPE.ERROR, "VerifyMoveData user:{0}({1},{2},{3}) t:{4} v:{5} x:{6} z:{7} sx:{8} sz:{9} distSqr:{10} enableDistSqr:{11}", RoleId, GetKey(), Guid, Name, t, v, x, z, sx, sz, distSqr, enableDistSqr);
                }
            }
            return ret;
        }

        internal bool VerifyNotMovingPosition(float x, float z, float maxEnabledDistSqr)
        {
            bool ret = true;
            if (m_LastSampleTime > 0) {
                Vector3 pos = new Vector3(x, 0, z);
                float distSqr = Geometry.DistanceSquare(pos, m_LastClientPosition);
                if (distSqr > maxEnabledDistSqr) {
                    ret = false;

                    LogSys.Log(LOG_TYPE.ERROR, "VerifyNoMoveData user:{0}({1},{2},{3}) x:{4} z:{5} sx:{6} sz:{7}", RoleId, GetKey(), Guid, Name, x, z, m_LastClientPosition.X, m_LastClientPosition.Z);
                }
            }
            return ret;
        }

        internal bool VerifyPosition(float x, float z, float velocity, float maxEnabledNomovingDistSqr)
        {
            bool ret = true;
            float ox = x;
            float oz = z;
            if (LastIsMoving) {
                if (!VerifyMovingPosition(x, z, velocity)) {
                    ret = false;
                }
            } else {
                if (!VerifyNotMovingPosition(x, z, maxEnabledNomovingDistSqr)) {
                    ret = false;
                }
            }
            return ret;
        }

        internal uint LocalID { set; get; }
        internal bool IsIdle { set; get; }
        internal Room OwnRoom { set; get; }
        internal ulong Guid
        {
            get
            {
                ulong guid = 0;
                if (null != m_LobbyUserData) {
                    guid = m_LobbyUserData.Guid;
                }
                return guid;
            }
        }
        public string Name
        {
            get
            {
                string name = string.Empty;
                if (null != m_LobbyUserData) {
                    name = m_LobbyUserData.Nick;
                }
                return name;
            }
        }
        public Msg_LR_RoomUserInfo LobbyUserData
        {
            get { return m_LobbyUserData; }
            set
            {
                m_LobbyUserData = value;
                if (null != m_LobbyUserData) {
                    peer_.Guid = m_LobbyUserData.Guid;
                }
            }
        }
        internal int RoleId
        {
            get
            {
                if (null != m_Info) {
                    return m_Info.GetId();
                } else {
                    return 0;
                }
            }
        }
        internal long EnterRoomTime
        {
            get
            {
                return peer_.EnterRoomTime;
            }
            set
            {
                peer_.EnterRoomTime = value;
            }
        }

        internal int UserControlState
        {
            get { return m_UserControlState; }
            set { m_UserControlState = value; }
        }
        internal bool IsEntered { set; get; }
        internal bool IsDebug { set; get; }
        internal bool IsReady { set; get; }
        internal EntityInfo Info
        {
            get { return m_Info; }
            set
            {
                m_Info = value;
                m_Info.CustomData = this;
                if (null != m_Info) {
                    peer_.RoleId = m_Info.GetId();
                }
            }
        }

        internal void SetHpArmor(int hp, int armor)
        {
            m_HaveHpArmor = true;
            m_Hp = hp;
            m_Energy = armor;
        }
        internal bool HaveHpArmor
        {
            get { return m_HaveHpArmor; }
        }
        internal int Hp
        {
            get { return m_Hp; }
        }
        internal int Energy
        {
            get { return m_Energy; }
        }

        internal void SetEnterPoint(float x, float y)
        {
            m_HaveEnterPosition = true;
            m_EnterX = x;
            m_EnterY = y;
        }
        internal bool HaveEnterPosition
        {
            get { return m_HaveEnterPosition; }
        }
        internal float EnterX
        {
            get { return m_EnterX; }
        }
        internal float EnterY
        {
            get { return m_EnterY; }
        }

        internal long CharacterCreateTime
        {
            get { return m_CharacterCreateTime; }
            set { m_CharacterCreateTime = value; }
        }
        internal int TimeCounter
        {
            get { return m_TimeCounter; }
            set { m_TimeCounter = value; }
        }
        internal long LastNotifyUserDropTime
        {
            get { return m_LastNotifyUserDropTime; }
            set { m_LastNotifyUserDropTime = value; }
        }

        private bool m_HaveHpArmor = false;
        private int m_Hp = 0;
        private int m_Energy = 0;

        private bool m_HaveEnterPosition = false;
        private float m_EnterX = 0;
        private float m_EnterY = 0;

        private Msg_LR_RoomUserInfo m_LobbyUserData = null;

        private RoomPeer peer_;
        private Dispatcher dispatcher_;

        private EntityInfo m_Info;
        private int m_UserControlState;
        private long m_LastNotifyUserDropTime;

        //移动校验数据
        private Vector3 m_LastClientPosition = Vector3.Zero;
        private float m_LastMoveVelocity = 0;
        private float m_LastMoveDirCosAngle = 0;
        private float m_LastMoveDirSinAngle = 0;
        private long m_LastSampleTime = 0;
        private bool m_LastIsMoving = false;
        private long m_CharacterCreateTime = 0;
        private int m_TimeCounter = 0;
    }
} // namespace 
