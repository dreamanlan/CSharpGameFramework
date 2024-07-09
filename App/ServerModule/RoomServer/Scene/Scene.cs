using System;
using System.Collections.Generic;
using GameFrameworkMessage;
using DotnetStoryScript;

namespace ScriptableFramework
{
    public enum SceneState : int
    {
        Sleeping = 0,
        Preloading,
        Running,
    }
    public sealed partial class Scene
    {
        public sealed class GameTimeUtil
        {
            public bool IsGameStart { get { return game_start_ms_ != 0; } }
            public long StartTime { get { return game_start_ms_; } }
            public long ElapseMilliseconds { get { return TimeUtility.GetLocalMilliseconds() - game_start_ms_; } }

            public void Start()
            {
                game_start_ms_ = TimeUtility.GetLocalMilliseconds();
            }

            public void Reset()
            {
                game_start_ms_ = 0;
            }

            private long game_start_ms_ = 0;
        }

        public Scene()
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
            m_SceneLogicSystem.SetSceneLogicInfoManager(m_SceneLogicInfoMgr);

            m_StorySystem.Init(this);
            m_GmStorySystem.Init(this);
            m_EntityController.Init(this, m_EntityMgr);

            m_CommandDocs = StoryCommandManager.Instance.GenCommandDocs();
            m_FunctionDocs = StoryFunctionManager.Instance.GenFunctionDocs();
        }

        public void Reset()
        {
            m_SceneContext.ResetUniqueId();
            m_EntityMgr.Reset();
            m_SceneLogicInfoMgr.Reset();
            m_SceneLogicSystem.Reset();
            MovementSystem.Reset();
            m_GameTime.Reset();
            m_ServerDelayActionProcessor.Reset();
            m_KdTree.Clear();

            m_StorySystem.Reset();
            m_EntityController.Reset();

            m_AirWallInfo.Clear();
            m_LastPreloadingTickTime = 0;
            m_LastTickTimeForTickPerSecond = 0;

            m_SceneState = SceneState.Sleeping;
            m_IsStoryState = false;

            m_ReloadMonstersQueue.Clear();
            m_PreparedReloadMonsterCount = 0;
            m_EntitiesForAi.Clear();
        }

        public void SetRoomUserManager(RoomUserManager roomUserMgr)
        {
            m_RoomInfo = roomUserMgr;
        }

        public RoomUserManager GetRoomUserManager()
        {
            return m_RoomInfo;
        }

        public long StartTime
        {
            get { return m_GameTime.StartTime; }
        }

        public void EnterScene(User newUser)
        {
            Msg_LR_RoomUserInfo lobbyUserData = newUser.LobbyUserData;
            if (null == lobbyUserData)
                return;
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(lobbyUserData.Hero);
            EntityInfo info = m_EntityMgr.AddEntity(0, lobbyUserData.Camp, cfg, string.Empty);
            info.SetUnitId(EntityInfo.c_StartUserUnitId + info.GetId());
            info.GetMovementStateInfo().FormationIndex = 0;
            if (null != m_SceneConfig) {
                info.GetMovementStateInfo().SetPosition2D(m_SceneConfig.EnterX + (Helper.Random.NextFloat() - 0.5f) * m_SceneConfig.EnterRadius, m_SceneConfig.EnterY + (Helper.Random.NextFloat() - 0.5f) * m_SceneConfig.EnterRadius);
            }
            newUser.Info = info;

            AttrCalculator.Calc(info);
            if (newUser.HaveHpArmor) {
                info.Hp = newUser.Hp;
                info.Energy = newUser.Energy;
            } else {
                info.Hp = newUser.Info.HpMax;
                info.Energy = newUser.Info.EnergyMax;
            }

            info.SceneContext = m_SceneContext;
            AddCareList(info);
            if (newUser.IsEntered) {
                var args = m_StorySystem.NewBoxedValueList();
                args.Add(info.GetId());
                args.Add(info.GetUnitId());
                args.Add(info.GetCampId());
                args.Add(info.GetMovementStateInfo().PositionX);
                args.Add(info.GetMovementStateInfo().PositionZ);
                m_StorySystem.SendMessage("user_enter_scene", args);
            }
        }

        public void LeaveScene(User user)
        {
            user.HaveEnterPosition = false;
            user.IsEntered = false;
            EntityInfo info = user.Info;
            if (null != info) {
                RemoveCareList(info);
                var args = m_StorySystem.NewBoxedValueList();
                args.Add(info.GetId());
                args.Add(info.GetUnitId());
                args.Add(info.GetCampId());
                args.Add(info.GetMovementStateInfo().PositionX);
                args.Add(info.GetMovementStateInfo().PositionZ);
                m_StorySystem.SendMessage("user_leave_scene", args);
                user.SetHpArmor(info.Hp, info.Energy);
                info.NeedDelete = true;
                user.Info = null;
            }
        }

        public void AddCareList(EntityInfo info)
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

        public void RemoveCareList(EntityInfo info)
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

        public void Init(int resId)
        {
            m_SceneResId = resId;
            m_SceneContext.SceneResId = resId;
            m_SceneContext.IsRunWithRoomServer = true;
            m_SceneConfig = TableConfig.LevelProvider.Instance.GetLevel(m_SceneResId);
        }

        public void LoadData(int resId)
        {
            try {
                //Read scene-related data such as blocking

                LogSys.Log(ServerLogType.DEBUG, "Scene {0} start Preloading.", resId);
                                
                m_SceneState = SceneState.Preloading;
            } catch (Exception ex) {
                LogSystem.Error("Scene.LoadData throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void Tick()
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

        public void NotifyAllObserver(RoomMessageDefine id, object msg)
        {
            foreach (Observer observer in m_RoomInfo.RoomObservers) {
                if (null != observer && !observer.IsIdle) {
                    observer.SendMessage(id, msg);
                }
            }
        }

        public void NotifyEnteredUser(RoomMessageDefine id, object msg)
        {
            foreach (User us in m_RoomInfo.RoomUsers) {
                if (us.IsEntered) {
                    us.SendMessage(id, msg);
                }
            }
            NotifyAllObserver(id, msg);
        }

        public void NotifyAllUser(RoomMessageDefine id, object msg)
        {
            foreach (User us in m_RoomInfo.RoomUsers) {
                us.SendMessage(id, msg);
            }
            NotifyAllObserver(id, msg);
        }

        public void NotifyAllUser(RoomMessageDefine id, object msg, int exceptId)
        {
            foreach (User us in m_RoomInfo.RoomUsers) {
                if (us.RoleId != exceptId) {
                    us.SendMessage(id, msg);
                }
            }
            NotifyAllObserver(id, msg);
        }

        public void SyncForNewUser(User user)
        {
            if (null != user) {
                EntityInfo userInfo = user.Info;
                RoomUserManager roomUserMgr = GetRoomUserManager();
                if (null != userInfo && null != roomUserMgr && null != roomUserMgr.ActiveScene) {
                    //Send camp to yourself
                    Msg_RC_CampChanged msg = new Msg_RC_CampChanged();
                    msg.obj_id = 0;
                    msg.camp_id = user.LobbyUserData.Camp;
                    user.SendMessage(RoomMessageDefine.Msg_RC_CampChanged, msg);
                    //Synchronize scene data to yourself
                    SyncSceneObjectsToUser(user);
                    SyncUserObjectToOtherUsers(user);
                }
            }
        }

        public void SyncForNewObserver(Observer observer)
        {
            if (null != observer) {
                RoomUserManager roomUserMgr = GetRoomUserManager();
                if (null != roomUserMgr && null != roomUserMgr.ActiveScene) {
                    //Synchronize scene data to observers
                    for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo npc = linkNode.Value;
                        if (null != npc) {
                            Msg_RC_CreateNpc bder = DataSyncUtility.BuildCreateNpcMessage(npc);
                            observer.SendMessage(RoomMessageDefine.Msg_RC_CreateNpc, bder);
                        }
                    }
                }
            }
        }

        public bool IsAllPlayerEntered()
        {
            foreach (User us in m_RoomInfo.RoomUsers) {
                if (!us.IsEntered && !us.IsTimeout()) {
                    return false;
                }
            }
            return true;
        }
        public Queue<TableConfig.LevelMonster> ReloadMonstersQueue
        {
            get { return m_ReloadMonstersQueue; }
        }
        public MovementSystem MovementSystem
        {
            get { return m_MovementSystem; }
        }
        public SceneLogicSystem SceneLogicSystem
        {
            get { return m_SceneLogicSystem; }
        }
        public SceneContextInfo SceneContext
        {
            get { return m_SceneContext; }
        }

        public SceneLogicInfoManager SceneLogicInfoMgr
        {
            get { return m_SceneLogicInfoMgr; }
        }
        public EntityManager EntityManager
        {
            get { return m_EntityMgr; }
        }
        public BlackBoard BlackBoard
        {
            get
            {
                return m_BlackBoard;
            }
        }
        public SortedList<string, string> CommandDocs
        {
            get { return m_CommandDocs; }
        }
        public SortedList<string, string> FunctionDocs
        {
            get { return m_FunctionDocs; }
        }

        public int SceneResId
        {
            get { return m_SceneResId; }
        }
        public TableConfig.Level SceneConfig
        {
            get { return m_SceneConfig; }
        }
        public ServerDelayActionProcessor DelayActionProcessor
        {
            get { return m_ServerDelayActionProcessor; }
        }
        public GameTimeUtil GameTime
        {
            get
            {
                return m_GameTime;
            }
        }
        public SceneState SceneState
        {
            get { return m_SceneState; }
        }
        public SceneProfiler SceneProfiler
        {
            get { return m_SceneProfiler; }
        }
        public ServerStorySystem StorySystem
        {
            get { return m_StorySystem; }
        }
        public EntityController EntityController
        {
            get { return m_EntityController; }
        }
        public KdObjectTree KdTree
        {
            get { return m_KdTree; }
        }
        public GmCommands.GmStorySystem GmStorySystem
        {
            get { return m_GmStorySystem; }
        }
        public bool IsStoryState
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
        private long m_LastTickTimeForAi = 0;

        private int m_SceneResId = 0;
        private TableConfig.Level m_SceneConfig = null;
        private Dictionary<string, bool> m_AirWallInfo = new Dictionary<string, bool>();
        private ServerDelayActionProcessor m_ServerDelayActionProcessor = new ServerDelayActionProcessor();

        private List<EntityInfo> m_DeletedEntities = new List<EntityInfo>();
        private EntityManager m_EntityMgr = new EntityManager(1024);
        private EntityController m_EntityController = new EntityController();
        private MovementSystem m_MovementSystem = new MovementSystem();
        private SceneLogicInfoManager m_SceneLogicInfoMgr = new SceneLogicInfoManager(1024);
        private SceneLogicSystem m_SceneLogicSystem = new SceneLogicSystem();
        private KdObjectTree m_KdTree = new KdObjectTree();
        private BlackBoard m_BlackBoard = new BlackBoard();
        private SceneContextInfo m_SceneContext = new SceneContextInfo();

        private ServerStorySystem m_StorySystem = new ServerStorySystem();

        private GmCommands.GmStorySystem m_GmStorySystem = new GmCommands.GmStorySystem();
        private bool m_IsStoryState = false;
        private SortedList<string, string> m_CommandDocs;
        private SortedList<string, string> m_FunctionDocs;

        private RoomUserManager m_RoomInfo = null;
        private GameTimeUtil m_GameTime = new GameTimeUtil();

        private SceneState m_SceneState = SceneState.Sleeping;
        private SceneProfiler m_SceneProfiler = new SceneProfiler();

        private Queue<TableConfig.LevelMonster> m_ReloadMonstersQueue = new Queue<TableConfig.LevelMonster>();
        private TableConfig.LevelMonster[] m_PreparedReloadMonsters = new TableConfig.LevelMonster[c_MaxReloadMonsterNum];
        private int m_PreparedReloadMonsterCount = 0;
        
        private List<EntityInfo> m_EntitiesForAi = new List<EntityInfo>();

        private const int c_MaxReloadMonsterNum = 32;
    }
}
