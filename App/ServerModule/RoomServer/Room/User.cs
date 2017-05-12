using System;
using System.Collections.Generic;
using GameFramework;
using GameFrameworkMessage;
using ScriptRuntime;

namespace GameFramework
{
    public enum UserControlState : int
    {
        User = 0,
        UserDropped,
        Ai,
        Remove,
    }
    public class User
    {
        public User()
        {
            m_Peer = new RoomPeer();
            m_Dispatcher = new Dispatcher();
            IsEntered = false;
            IsDebug = false;
            IsReady = false;
            m_UserControlState = (int)GameFramework.UserControlState.User;
        }

        public void Reset()
        {
            m_Peer.Reset();
            OwnRoomUserManager = null;
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

        public void Init()
        {
            m_Dispatcher.SetClientDefaultHandler(DefaultMsgHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_Enter, EnterHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_Quit, Msg_CR_QuitHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_UserMoveToPos, Msg_CR_UserMoveToPosHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_Skill, UseSkillHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_StopSkill, Msg_CR_StopSkillHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_SwitchDebug, SwitchDebugHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_DlgClosed, Msg_CR_DlgClosedHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CR_GmCommand, Msg_CR_GmCommandHandler.Execute);
            m_Dispatcher.RegClientMsgHandler(RoomMessageDefine.Msg_CRC_StoryMessage, StoryMessageHandler.Execute);
        }

        public void RegisterObservers(IList<Observer> observers)
        {
            m_Peer.RegisterObservers(observers);
        }

        public bool SetKey(uint key)
        {
            return m_Peer.SetKey(key);
        }
        public uint GetKey()
        {
            return m_Peer.GetKey();
        }

        public bool ReplaceDroppedUser(ulong replacer, uint key)
        {
            if (null != m_LobbyUserData) {
                m_LobbyUserData.Guid = replacer;
                m_Peer.Guid = replacer;
            }
            return m_Peer.UpdateKey(key);
        }

        public RoomPeer GetPeer()
        {
            return m_Peer;
        }

        public bool IsConnected()
        {
            return m_Peer.IsConnected();
        }

        public bool IsTimeout()
        {
            return m_Peer.IsTimeout();
        }

        public void Disconnect()
        {
            m_Peer.Disconnect();
        }

        public long GetElapsedDroppedTime()
        {
            return m_Peer.GetElapsedDroppedTime();
        }

        public void SendMessage(RoomMessageDefine id, object msg)
        {
            m_Peer.SendMessage(id, msg);
        }

        public void BroadCastMsgToCareList(RoomMessageDefine id, object msg, bool exclude_me = true)
        {
            m_Peer.BroadCastMsgToCareList(id, msg, exclude_me);
        }

        public void BroadCastMsgToRoom(RoomMessageDefine id, object msg, bool exclude_me = true)
        {
            m_Peer.BroadCastMsgToRoom(id, msg, exclude_me);
        }

        public void AddSameRoomUser(User user)
        {
            m_Peer.AddSameRoomPeer(user.GetPeer());
        }

        public void RemoveSameRoomUser(User user)
        {
            m_Peer.RemoveSameRoomPeer(user.GetPeer());
        }

        public void ClearSameRoomUser()
        {
            m_Peer.ClearSameRoomPeer();
        }

        public void AddCareMeUser(User user)
        {
            m_Peer.AddCareMePeer(user.GetPeer());
        }

        public void RemoveCareMeUser(User user)
        {
            m_Peer.RemoveCareMePeer(user.GetPeer());
        }

        public void Tick()
        {
            try {
                int id = 0;
                object msg = null;
                while ((msg = m_Peer.PeekLogicMsg(out id)) != null) {
                    m_Dispatcher.HandleClientMsg(id, msg, this);
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public bool LastIsMoving
        {
            get { return m_LastIsMoving; }
            set { m_LastIsMoving = value; }
        }

        public Vector3 LastClientPosition
        {
            get { return m_LastClientPosition; }
        }

        public void SampleMoveData(float x, float z, float velocity, float cosDir, float sinDir)
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

        public bool VerifyMovingPosition(float x, float z, float velocity)
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

        public bool VerifyNotMovingPosition(float x, float z, float maxEnabledDistSqr)
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

        public bool VerifyPosition(float x, float z, float velocity, float maxEnabledNomovingDistSqr)
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

        public uint LocalID { set; get; }
        public bool IsIdle { set; get; }
        public RoomUserManager OwnRoomUserManager { set; get; }
        public ulong Guid
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
                    m_Peer.Guid = m_LobbyUserData.Guid;
                }
            }
        }
        public int RoleId
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
        public long EnterRoomTime
        {
            get
            {
                return m_Peer.EnterRoomTime;
            }
            set
            {
                m_Peer.EnterRoomTime = value;
            }
        }

        public int UserControlState
        {
            get { return m_UserControlState; }
            set { m_UserControlState = value; }
        }
        public bool IsEntered { set; get; }
        public bool IsDebug { set; get; }
        public bool IsReady { set; get; }
        public EntityInfo Info
        {
            get { return m_Info; }
            set
            {
                m_Info = value;
                if (null != m_Info) {
                    m_Info.CustomData = this;
                    m_Peer.RoleId = m_Info.GetId();
                }
            }
        }

        public void SetHpArmor(int hp, int armor)
        {
            m_HaveHpArmor = true;
            m_Hp = hp;
            m_Energy = armor;
        }
        public bool HaveHpArmor
        {
            get { return m_HaveHpArmor; }
        }
        public int Hp
        {
            get { return m_Hp; }
        }
        public int Energy
        {
            get { return m_Energy; }
        }

        public void SetEnterPoint(float x, float y)
        {
            m_HaveEnterPosition = true;
            m_EnterX = x;
            m_EnterY = y;
        }
        public bool HaveEnterPosition
        {
            get { return m_HaveEnterPosition; }
            set { m_HaveEnterPosition = value; }
        }
        public float EnterX
        {
            get { return m_EnterX; }
        }
        public float EnterY
        {
            get { return m_EnterY; }
        }

        public long CharacterCreateTime
        {
            get { return m_CharacterCreateTime; }
            set { m_CharacterCreateTime = value; }
        }
        public int TimeCounter
        {
            get { return m_TimeCounter; }
            set { m_TimeCounter = value; }
        }
        public long LastNotifyUserDropTime
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

        private RoomPeer m_Peer;
        private Dispatcher m_Dispatcher;

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
