using System;
using System.Collections.Generic;
using GameFrameworkMessage;

namespace GameFramework
{
    internal enum SceneState : int
    {
        Sleeping = 0,
        Preloading,
        Running,
    }
    internal sealed partial class Scene
    {
        internal sealed class GameTimeUtil
        {
            internal bool IsGameStart { get { return game_start_ms_ != 0; } }
            internal long StartTime { get { return game_start_ms_; } }
            internal long ElapseMilliseconds { get { return TimeUtility.GetLocalMilliseconds() - game_start_ms_; } }

            internal void Start()
            {
                game_start_ms_ = TimeUtility.GetLocalMilliseconds();
            }

            internal void Reset()
            {
                game_start_ms_ = 0;
            }

            private long game_start_ms_ = 0;
        }

        internal Scene()
        {
            m_SceneContext.OnHighlightPrompt += this.OnHightlightPrompt;

            m_SceneContext.SceneLogicInfoManager = m_SceneLogicInfoMgr;
            m_SceneContext.EntityManager = m_EntityMgr;
            m_SceneContext.KdTree = m_KdTree;
            m_SceneContext.BlackBoard = m_BlackBoard;
            m_SceneContext.CustomData = this;

            m_EntityMgr.SetSceneContext(m_SceneContext);
            m_SceneLogicInfoMgr.SetSceneContext(m_SceneContext);

            MovementSystem.SetEntityManager(m_EntityMgr);
            m_AiSystem.SetEntityManager(m_EntityMgr);
            m_SceneLogicSystem.SetSceneLogicInfoManager(m_SceneLogicInfoMgr);

            m_StorySystem.Init(this);
            m_GmStorySystem.Init(this);
            m_SkillSystem.Init(this);
            //m_SkillSystem.SetSceneContext(m_SceneContext);
            m_EntityController.Init(this, m_EntityMgr);
        }

        internal void Reset()
        {
            m_SceneContext.ResetUniqueId();
            m_EntityMgr.Reset();
            m_SceneLogicInfoMgr.Reset();
            m_SceneLogicSystem.Reset();
            MovementSystem.Reset();
            m_AiSystem.Reset();
            m_GameTime.Reset();
            m_ServerDelayActionProcessor.Reset();
            m_KdTree.Clear();
            m_BlackBoard.Reset();

            m_StorySystem.Reset();
            m_SkillSystem.Reset();
            m_EntityController.Reset();

            m_AirWallInfo.Clear();
            m_LastPreloadingTickTime = 0;
            m_LastTickTimeForTickPerSecond = 0;

            m_SceneState = SceneState.Sleeping;
            m_IsStoryState = false;

            m_ReloadMonstersQueue.Clear();
            m_PreparedReloadMonsterCount = 0;
        }

        internal void SetRoom(Room room)
        {
            m_Room = room;
        }

        internal Room GetRoom()
        {
            return m_Room;
        }

        internal long StartTime
        {
            get { return m_GameTime.StartTime; }
        }

        internal void EnterScene(User newUser)
        {
            Msg_LR_RoomUserInfo lobbyUserData = newUser.LobbyUserData;
            if (null == lobbyUserData)
                return;
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(lobbyUserData.Hero);
            EntityInfo info = m_EntityMgr.AddEntity(0, lobbyUserData.Camp, cfg, (int)AiStateLogicId.Entity_Leader);
            info.SetUnitId(EntityInfo.c_StartUserUnitId + info.GetId());
            info.GetMovementStateInfo().FormationIndex = 0;
            if (null != m_SceneConfig) {
                info.GetMovementStateInfo().SetPosition2D(m_SceneConfig.EnterX + (Helper.Random.NextFloat() - 0.5f) * m_SceneConfig.EnterRadius, m_SceneConfig.EnterY + (Helper.Random.NextFloat() - 0.5f) * m_SceneConfig.EnterRadius);
            }
            newUser.Info = info;

            AttrCalculator.Calc(info);
            if (newUser.HaveHpArmor) {
                info.SetHp(Operate_Type.OT_Absolute, newUser.Hp);
                info.SetEnergy(Operate_Type.OT_Absolute, newUser.Energy);
            } else {
                info.SetHp(Operate_Type.OT_Absolute, newUser.Info.GetActualProperty().HpMax);
                info.SetEnergy(Operate_Type.OT_Absolute, newUser.Info.GetActualProperty().EnergyMax);
            }

            info.SceneContext = m_SceneContext;
            AddCareList(info);
            if (newUser.IsEntered) {
                m_StorySystem.SendMessage("user_enter_scene", info.GetId(), info.GetUnitId(), info.GetCampId(), info.GetMovementStateInfo().PositionX, info.GetMovementStateInfo().PositionZ);
            }
        }

        internal void LeaveScene(User user)
        {
            EntityInfo info = user.Info;
            RemoveCareList(info);
            m_StorySystem.SendMessage("user_leave_scene", info.GetId(), info.GetUnitId(), info.GetCampId(), info.GetMovementStateInfo().PositionX, info.GetMovementStateInfo().PositionZ);
            user.SetHpArmor(info.Hp, info.Energy);
            user.HaveEnterPosition = false;
            user.IsEntered = false;
            info.NeedDelete = true;
            user.Info = null;
        }

        internal void AddCareList(EntityInfo info)
        {
            User enter_user = info.CustomData as User;
            if (enter_user == null) { return; }
            IList<EntityInfo> users = new List<EntityInfo>();
            foreach (EntityInfo user_info in users) {
                if (user_info.GetId() == info.GetId())
                    continue;
                User user = user_info.CustomData as User;
                if (null == user)
                    continue;
                user.AddCareMeUser(enter_user);
                enter_user.AddCareMeUser(user);
            }
        }

        internal void RemoveCareList(EntityInfo info)
        {
            User leave_user = info.CustomData as User;
            if (leave_user == null) { return; }
            IList<EntityInfo> users = new List<EntityInfo>();
            foreach (EntityInfo user_info in users) {
                if (user_info.GetId() == info.GetId())
                    continue;
                User user = user_info.CustomData as User;
                if (null == user)
                    continue;
                user.RemoveCareMeUser(leave_user);
                leave_user.RemoveCareMeUser(user);
            }
        }

        internal void LoadData(int resId)
        {
            try {
                m_SceneResId = resId;
                m_SceneContext.SceneResId = resId;
                m_SceneContext.IsRunWithRoomServer = true;

                LogSys.Log(LOG_TYPE.DEBUG, "Scene {0} start Preloading.", resId);

                m_SceneConfig = TableConfig.LevelProvider.Instance.GetLevel(m_SceneResId);
                
                m_SceneState = SceneState.Preloading;
            } catch (Exception ex) {
                LogSystem.Error("Scene.LoadData throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        internal void Tick()
        {
            switch (m_SceneState) {
                case SceneState.Preloading:
                    TickPreloading();
                    break;
                case SceneState.Running:
                    TickRunning();
                    break;
            }
        }

        internal void NotifyAllObserver(RoomMessageDefine id, object msg)
        {
            foreach (Observer observer in m_Room.RoomObservers) {
                if (null != observer && !observer.IsIdle) {
                    observer.SendMessage(id, msg);
                }
            }
        }

        internal void NotifyEnteredUser(RoomMessageDefine id, object msg)
        {
            foreach (User us in m_Room.RoomUsers) {
                if (us.IsEntered) {
                    us.SendMessage(id, msg);
                }
            }
            NotifyAllObserver(id, msg);
        }

        internal void NotifyAllUser(RoomMessageDefine id, object msg)
        {
            foreach (User us in m_Room.RoomUsers) {
                us.SendMessage(id, msg);
            }
            NotifyAllObserver(id, msg);
        }

        internal void NotifyAllUser(RoomMessageDefine id, object msg, int exceptId)
        {
            foreach (User us in m_Room.RoomUsers) {
                if (us.RoleId != exceptId) {
                    us.SendMessage(id, msg);
                }
            }
            NotifyAllObserver(id, msg);
        }

        internal void SyncForNewUser(User user)
        {
            if (null != user) {
                EntityInfo userInfo = user.Info;
                Room room = GetRoom();
                if (null != userInfo && null != room && null != room.ActiveScene) {
                    //发阵营给自己
                    Msg_RC_CampChanged msg = new Msg_RC_CampChanged();
                    msg.obj_id = 0;
                    msg.camp_id = user.LobbyUserData.Camp;
                    user.SendMessage(RoomMessageDefine.Msg_RC_CampChanged, msg);
                    //同步场景数据给自己
                    SyncSceneObjectsToUser(user);
                    SyncUserObjectToOtherUsers(user);
                }
            }
        }

        internal void SyncForNewObserver(Observer observer)
        {
            if (null != observer) {
                Room room = GetRoom();
                if (null != room && null != room.ActiveScene) {
                    //同步场景数据给观察者
                    for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo npc = linkNode.Value;
                        if (null != npc) {
                            Msg_RC_CreateNpc bder = DataSyncUtility.BuildCreateNpcMessage(npc);
                            observer.SendMessage(RoomMessageDefine.Msg_RC_CreateNpc, bder);
                        }
                    }
                }
            }
        }

        internal bool IsAllPlayerEntered()
        {
            foreach (User us in m_Room.RoomUsers) {
                if (!us.IsEntered && !us.IsTimeout()) {
                    return false;
                }
            }
            return true;
        }

        internal Queue<TableConfig.LevelMonster> ReloadMonstersQueue
        {
            get { return m_ReloadMonstersQueue; }
        }

        internal MovementSystem MovementSystem
        {
            get { return m_MovementSystem; }
        }
        internal AiSystem AiSystem
        {
            get { return m_AiSystem; }
        }
        internal SceneLogicSystem SceneLogicSystem
        {
            get { return m_SceneLogicSystem; }
        }
        internal SceneContextInfo SceneContext
        {
            get { return m_SceneContext; }
        }

        internal SceneLogicInfoManager SceneLogicInfoMgr
        {
            get { return m_SceneLogicInfoMgr; }
        }
        internal EntityManager EntityManager
        {
            get { return m_EntityMgr; }
        }
        internal BlackBoard BlackBoard
        {
            get
            {
                return m_BlackBoard;
            }
        }

        internal int SceneResId
        {
            get { return m_SceneResId; }
        }
        internal TableConfig.Level SceneConfig
        {
            get { return m_SceneConfig; }
        }
        internal ServerDelayActionProcessor DelayActionProcessor
        {
            get { return m_ServerDelayActionProcessor; }
        }
        internal GameTimeUtil GameTime
        {
            get
            {
                return m_GameTime;
            }
        }
        internal SceneState SceneState
        {
            get { return m_SceneState; }
        }
        internal SceneProfiler SceneProfiler
        {
            get { return m_SceneProfiler; }
        }
        internal ServerStorySystem StorySystem
        {
            get { return m_StorySystem; }
        }
        internal ServerSkillSystem SkillSystem
        {
            get { return m_SkillSystem; }
        }
        internal EntityController EntityController
        {
            get { return m_EntityController; }
        }
        public KdObjectTree KdTree
        {
            get { return m_KdTree; }
        }
        internal GmCommands.GmStorySystem GmStorySystem
        {
            get { return m_GmStorySystem; }
        }
        internal bool IsStoryState
        {
            get { return m_IsStoryState; }
            set { m_IsStoryState = value; }
        }
        
        private const long c_PreloadingTickInterval = 1000;
        private long m_LastPreloadingTickTime = 0;

        private const long c_IntervalPerSecond = 5000;
        private long m_LastTickTimeForTickPerSecond = 0;
        private const long c_IntervalPer5s = 5000;
        private long m_LastTickTimeForTickPer5s = 0;
        private int m_SceneResId = 0;
        private TableConfig.Level m_SceneConfig = null;
        private Dictionary<string, bool> m_AirWallInfo = new Dictionary<string, bool>();
        private ServerDelayActionProcessor m_ServerDelayActionProcessor = new ServerDelayActionProcessor();

        private List<EntityInfo> m_DeletedEntities = new List<EntityInfo>();
        private EntityManager m_EntityMgr = new EntityManager(1024);
        private EntityController m_EntityController = new EntityController();
        private MovementSystem m_MovementSystem = new MovementSystem();
        private AiSystem m_AiSystem = new AiSystem();
        private SceneLogicInfoManager m_SceneLogicInfoMgr = new SceneLogicInfoManager(1024);
        private SceneLogicSystem m_SceneLogicSystem = new SceneLogicSystem();
        private KdObjectTree m_KdTree = new KdObjectTree();
        private BlackBoard m_BlackBoard = new BlackBoard();
        private SceneContextInfo m_SceneContext = new SceneContextInfo();

        private ServerStorySystem m_StorySystem = new ServerStorySystem();
        private ServerSkillSystem m_SkillSystem = new ServerSkillSystem();

        private GmCommands.GmStorySystem m_GmStorySystem = new GmCommands.GmStorySystem();
        private bool m_IsStoryState = false;

        private Room m_Room = null;
        private GameTimeUtil m_GameTime = new GameTimeUtil();

        private SceneState m_SceneState = SceneState.Sleeping;
        private SceneProfiler m_SceneProfiler = new SceneProfiler();

        private Queue<TableConfig.LevelMonster> m_ReloadMonstersQueue = new Queue<TableConfig.LevelMonster>();
        private TableConfig.LevelMonster[] m_PreparedReloadMonsters = new TableConfig.LevelMonster[c_MaxReloadMonsterNum];
        private int m_PreparedReloadMonsterCount = 0;

        private const int c_MaxReloadMonsterNum = 32;
    }
}
