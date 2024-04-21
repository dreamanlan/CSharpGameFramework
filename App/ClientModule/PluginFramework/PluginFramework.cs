using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GameFramework.GmCommands;
using GameFramework.Story;
using GameFramework.Skill;
using GameFramework.Network;
using GameFrameworkMessage;

namespace GameFramework
{
    public class LockTargetInfo
    {
        public int TargetId;
        public EntityInfo Target;
    }
    public partial class PluginFramework
    {
        #region Singleton
        private static PluginFramework s_Instance = new PluginFramework();
        public static PluginFramework Instance
        {
            get
            {
                return s_Instance;
            }
        }

        #endregion

        #region Subscription Event processing
        private void ResetDsl()
        {
            try {
                GfxSkillSystem.Instance.Reset();
                GfxSkillSystem.Instance.ClearSkillInstancePool();
                SkillSystem.SkillConfigManager.Instance.Clear();

                GfxStorySystem.Instance.Reset();
                GfxStorySystem.Instance.ClearStoryInstancePool();
                StorySystem.StoryConfigManager.Instance.Clear();
                GfxStorySystem.Instance.SceneId = m_SceneId;
                GfxStorySystem.Instance.LoadSceneStories();
                GfxStorySystem.Instance.StartStory("local_main");
                if (IsMainUiScene) {
                    GfxStorySystem.Instance.StartStory("story_main");
                }
                LogSystem.Warn("ResetDsl finish.");
            } catch (Exception ex) {
                LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void ExecScript(string scriptFile)
        {
            try {
                if (string.IsNullOrEmpty(scriptFile)) {
                    scriptFile = "Dsl/gm.dsl";
                }
                string path = scriptFile;
                if (!File.Exists(path)) {
                    path = HomePath.GetAbsolutePath(path);
                }
                if (!File.Exists(path)) {
                    LogSystem.Warn("Can't find {0}.", scriptFile);
                    return;
                }
                ClientGmStorySystem.Instance.Reset();
                ClientGmStorySystem.Instance.LoadStory(path);
                ClientGmStorySystem.Instance.StartStory("main");

                LogSystem.Warn("ExecScript {0} finish.", scriptFile);
            } catch (Exception ex) {
                LogSystem.Error("ExecScript exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void ExecCommand(string cmd)
        {
            try {
                ClientGmStorySystem.Instance.Reset();
                ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes("script(main){onmessage(\"start\"){" + cmd + "}}"));
                ClientGmStorySystem.Instance.StartStory("main");
                LogSystem.Warn("ExecCommand {0} finish.", cmd);
            } catch (Exception ex) {
                LogSystem.Error("ExecCommand exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        #endregion

        public void Init(bool useNetwork)
        {
            m_UseNetwork = useNetwork;
            AttrCalculator.LoadConfig();
            LoadTableConfig();

            ResourceSystem.Instance.Init();

            m_SceneContextInfo.OnHighlightPrompt = (int userId, string dict, object[] args) => {
                PluginFramework.Instance.HighlightPromptWithDict(dict, args);
            };

            m_SceneContextInfo.KdTree = m_KdTree;
            m_SceneContextInfo.BlackBoard = m_BlackBoard;
            m_SceneContextInfo.SceneLogicInfoManager = m_SceneLogicInfoManager;
            m_SceneContextInfo.EntityManager = m_EntityManager;

            m_SceneLogicInfoManager.SetSceneContext(m_SceneContextInfo);
            m_EntityManager.SetSceneContext(m_SceneContextInfo);


            GfxStorySystem.ThreadInitMask();

            ClientGmStorySystem.Instance.Init();
            GfxStorySystem.Instance.Init();
            GfxSkillSystem.Instance.Init();

            m_SceneLogicSystem.SetSceneLogicInfoManager(m_SceneLogicInfoManager);

            if (m_UseNetwork) {
                UserNetworkSystem.Instance.Init(m_AsyncActionProcessor);
                NetworkSystem.Instance.Init();
            }

            SceneLogicViewModelManager.Instance.Init();
            EntityViewModelManager.Instance.Init();
            EntityController.Instance.Init();

            EntityManager.OnDamage += OnDamage;

            Utility.EventSystem.Subscribe("gm_resetdsl", "gm", ResetDsl);
            Utility.EventSystem.Subscribe<string>("gm_execscript", "gm", ExecScript);
            Utility.EventSystem.Subscribe<string>("gm_execcommand", "gm", ExecCommand);

            m_CommandDocs = StorySystem.StoryCommandManager.Instance.GenCommandDocs();
            m_FunctionDocs = StorySystem.StoryFunctionManager.Instance.GenFunctionDocs();
        }
        public void Release()
        {
            if (m_UseNetwork) {
                UserNetworkSystem.Instance.QuitClient();
                NetworkSystem.Instance.QuitClient();
                NetworkSystem.Instance.Release();
            }
            EntityViewModelManager.Instance.Release();
            EntityController.Instance.Release();
        }
        public void Reset()
        {
            GmCommands.ClientGmStorySystem.Instance.Reset();
            GfxStorySystem.Instance.Reset();
            GfxStorySystem.Instance.ClearStoryInstancePool();
            StorySystem.StoryConfigManager.Instance.Clear();

            GfxSkillSystem.Instance.Reset();
            GfxSkillSystem.Instance.ClearSkillInstancePool();

            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info) {
                    EntityViewModelManager.Instance.DestroyEntityView(info.GetId());
                }
            }
            m_EntityManager.Reset();
            m_SceneLogicInfoManager.Reset();
            m_KdTree.Clear();

            m_SceneLogicSystem.Reset();
            m_SceneInfo = null;

            m_LeaderId = 0;
            m_CampId = (int)CampIdEnum.Blue;
        }
        public void Preload()
        {
            GfxStorySystem.Instance.LoadSceneStories();
            GfxStorySystem.Instance.StartStory("local_main");
            if (IsMainUiScene) {
                GfxStorySystem.Instance.StartStory("story_main");
            }

            PreloadUiStories();

            PredefinedSkill.Instance.Preload();
        }
        public void Tick()
        {
            if (m_UseNetwork) {
                UserNetworkSystem.Instance.Tick();
                NetworkSystem.Instance.Tick();
            }
            if (!m_IsSceneLoaded) {
                return;
            }

            m_KdTree.BeginBuild(m_EntityManager.Entities.Count);
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                m_KdTree.AddObjForBuild(info);
            }
            m_KdTree.EndBuild();

            //Handle delayed calls
            m_AsyncActionProcessor.HandleActions(100);

            if (!m_IsSceneLoaded) {
                return;
            }

            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime == 0) {
                m_LastTickTime = curTime;
            }
            long deltaTime = curTime - m_LastTickTime;
            m_LastTickTime = curTime;

            GmCommands.ClientGmStorySystem.Instance.Tick();
            GfxStorySystem.Instance.Tick();
            GfxSkillSystem.Instance.Tick();

            m_SceneLogicSystem.Tick();
            TickAi(curTime, deltaTime);
            TickEntities(curTime, deltaTime);

            EntityViewModelManager.Instance.Tick();
            EntityController.Instance.Tick();
            ResourceSystem.Instance.Tick();
        }
        
        public void TryEnterScene(uint key, string ip, int port, int campId, int sceneId)
        {
            if (!m_IsSceneLoaded) {
                //Wait for the current scene to be successfully entered before switching scenes.
                QueueAction(TryEnterScene, key, ip, port, campId, sceneId);
                return;
            }
            NetworkSystem.Instance.Start(key, ip, port, campId, sceneId);
        }
        
        public void DelayChangeScene(int levelId)
        {
            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(levelId);
            if (null != cfg && (cfg.type == (int)SceneTypeEnum.Story || cfg.type == (int)SceneTypeEnum.Activity)) {
                UserNetworkSystem.Instance.EnterScene(levelId, 0);
            } else {
                QueueAction(ChangeScene, levelId);
            }
        }

        public void ChangeScene(int levelId)
        {
            m_IsSceneLoaded = false;
            m_AsyncActionProcessor.Reset();
            Reset();
            ResourceSystem.Instance.CleanupResourcePool();

            m_SceneInfo = TableConfig.LevelProvider.Instance.GetLevel(levelId);
            if (null != m_SceneInfo && m_SceneInfo.type != (int)SceneTypeEnum.Battle) {
                Utility.SendMessage("GameRoot", "LoadScene", m_SceneInfo);
            }
        }
        public void OnSceneLoaded(TableConfig.Level lvl)
        {
            m_SceneId = lvl.id;
            GfxStorySystem.Instance.SceneId = m_SceneId;
            Preload();

            if (IsMainUiScene) {
                Utility.SendMessage("GameRoot", "OnLoadMainUiComplete", lvl.id);
            } else {
                Utility.SendMessage("GameRoot", "OnLoadSceneComplete", lvl.id);

                GameFrameworkMessage.Msg_CR_Enter build = new GameFrameworkMessage.Msg_CR_Enter();
                NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_Enter, build);
                LogSystem.Warn("send Msg_CR_Enter to roomserver");
            }
            m_IsSceneLoaded = true;
        }
        public void LoadBattle(int levelId)
        {
            m_BattleSceneInfo = TableConfig.LevelProvider.Instance.GetLevel(levelId);
            if (null != m_BattleSceneInfo && m_BattleSceneInfo.type == (int)SceneTypeEnum.Battle) {
                Utility.SendMessage("GameRoot", "LoadBattle", m_BattleSceneInfo);
            }
        }
        public void OnBattleLoaded(TableConfig.Level lvl)
        {
            m_IsBattleState = true;

            m_BattleSceneId = lvl.id;
            GfxStorySystem.Instance.LoadBattleStories(lvl.id);
            GfxStorySystem.Instance.StartStory("battle_main");

            if (lvl.type == (int)SceneTypeEnum.Battle) {
                Utility.SendMessage("GameRoot", "OnLoadBattleComplete", lvl.id);
            }
        }
        public void UnloadBattle(int levelId)
        {
            m_IsBattleState = false;

            //Kill all client NPCs
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (!info.IsServerEntity) {
                    info.Hp = 0;
                }
            }

            var lvl = TableConfig.LevelProvider.Instance.GetLevel(levelId);
            if (null != lvl) {
                Utility.SendMessage("GameRoot", "UnloadBattle", lvl);
            }
        }
        public void OnBattleUnloaded(TableConfig.Level lvl)
        {
            GfxStorySystem.Instance.SendMessage("on_battle_closed", m_RoomObjId, m_RoomUnitId);
        }
                
        public void OnRoomServerDisconnected()
        {
            if (m_IsSceneLoaded) {
                if (NetworkSystem.Instance.ReconnectCount <= 18) {
                } else {
                    //The connection failed after more than 3 minutes.
                    NetworkSystem.Instance.QuitBattlePassive();
                }
            }
        }
        public void OnRoomServerConnected()
        {
            if (m_IsSceneLoaded) {
                if (SceneId == NetworkSystem.Instance.RoomSceneId) {
                    RefreshRoomScene();
                } else {
                    //For multiplayer copies, the network connection and verification must be passed before
                    //switching to the scene to prevent getting stuck.
                    QueueAction(this.ChangeScene, NetworkSystem.Instance.RoomSceneId);
                }
            }
        }
        private void RefreshRoomScene()
        {
            LogSystem.Warn("PluginFramework.RefreshRoomScene Destory Objects...");
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info) {
                    EntityViewModelManager.Instance.DestroyEntityView(info.GetId());
                }
            }
            LogSystem.Warn("PluginFramework.RefreshRoomScene Destory Objects Finish.");

            m_EntityManager.Reset();

            GameFrameworkMessage.Msg_CR_Enter build = new GameFrameworkMessage.Msg_CR_Enter();
            NetworkSystem.Instance.SendMessage(RoomMessageDefine.Msg_CR_Enter, build);
            LogSystem.Warn("RefreshRoomScene send Msg_CR_Enter to roomserver");
        }

        public int GetBossCount()
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.EntityType == (int)EntityTypeEnum.Boss) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount()
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc()) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount(EntityInfo src, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && EntityInfo.GetRelation(src, info) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount(int campId, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && EntityInfo.GetRelation(campId, info.GetCampId()) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount(int campId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && info.GetCampId() == campId) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetDyingBattleNpcCount(int campId, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && info.IsDead() && info.DeadTime != 0 && info.IsCombatNpc() && EntityInfo.GetRelation(campId, info.GetCampId()) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetDyingBattleNpcCount(int campId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && info.IsDead() && info.DeadTime != 0 && info.IsCombatNpc() && info.GetCampId() == campId) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetNpcCount(int startUnitId, int endUnitId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;                
                if (null != info && !info.IsDead()) {
                    int unitId = info.GetUnitId();
                    if (unitId >= startUnitId && unitId <= endUnitId) {
                        ++ct;
                    }
                }
            }
            return ct;
        }

        public EntityInfo GetEntityById(int id)
        {
            EntityInfo obj = null;
            if (null != m_EntityManager)
                obj = m_EntityManager.GetEntityInfo(id);
            return obj;
        }
        public EntityInfo GetEntityByUnitId(int unitId)
        {
            EntityInfo obj = null;
            if (null != m_EntityManager)
                obj = m_EntityManager.GetEntityInfoByUnitId(unitId);
            return obj;
        }
        public void DestroyEntityById(int id)
        {
            if (m_EntityManager.Entities.Contains(id)) {
                m_EntityManager.RemoveEntity(id);
            }
        }
        public int CreateEntity(int unitId, float x, float y, float z, float dir, int camp, int tableId)
        {
            int objId = 0;
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(tableId);
            if (null != cfg) {
                EntityInfo entity = m_EntityManager.AddEntity(unitId, camp, cfg, string.Empty);
                if (null != entity) {
                    entity.IsServerEntity = false;
                    entity.GetMovementStateInfo().SetPosition(x, y, z);
                    entity.GetMovementStateInfo().SetFaceDir(dir);
                    EntityViewModelManager.Instance.CreateEntityView(entity.GetId());
                    objId = entity.GetId();
                    OnCreateEntity(entity);
                }
            }
            return objId;
        }
        public int CreateEntity(int unitId, float x, float y, float z, float dir, int camp, int tableId, string ai, params string[] aiParams)
        {
            int objId = 0;
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(tableId);
            if (null != cfg) {
                EntityInfo entity = m_EntityManager.AddEntity(unitId, camp, cfg, ai, aiParams);
                if (null != entity) {
                    entity.IsServerEntity = false;
                    entity.GetMovementStateInfo().SetPosition(x, y, z);
                    entity.GetMovementStateInfo().SetFaceDir(dir);
                    EntityViewModelManager.Instance.CreateEntityView(entity.GetId());
                    objId = entity.GetId();
                    OnCreateEntity(entity);
                }
            }
            return objId;
        }
        public EntityInfo CreateEntity(int objId, int unitId, float x, float y, float z, float dir, int camp, int tableId)
        {
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(tableId);
            if (null != cfg) {
                EntityInfo entity = m_EntityManager.AddEntity(objId, unitId, camp, cfg, string.Empty);
                if (null != entity) {
                    entity.IsServerEntity = true;
                    entity.GetMovementStateInfo().SetPosition(x, y, z);
                    entity.GetMovementStateInfo().SetFaceDir(dir);
                    EntityViewModelManager.Instance.CreateEntityView(entity.GetId());
                    OnCreateEntity(entity);
                    return entity;
                }
            }
            return null;
        }
        public EntityInfo CreateEntity(int objId, int unitId, float x, float y, float z, float dir, int camp, int tableId, string ai, params string[] aiParams)
        {
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(tableId);
            if (null != cfg) {
                EntityInfo entity = m_EntityManager.AddEntity(objId, unitId, camp, cfg, ai, aiParams);
                if (null != entity) {
                    entity.IsServerEntity = true;
                    entity.GetMovementStateInfo().SetPosition(x, y, z);
                    entity.GetMovementStateInfo().SetFaceDir(dir);
                    EntityViewModelManager.Instance.CreateEntityView(entity.GetId());
                    OnCreateEntity(entity);
                    return entity;
                }
            }
            return null;
        }
        public int CreateSceneLogic(int configId, int logicId, params string[] args)
        {
            int id = 0;
            SceneLogicConfig cfg = new SceneLogicConfig();
            cfg.m_ConfigId = configId;
            cfg.m_LogicId = logicId;
            cfg.m_Params = args;
            SceneLogicInfo logicInfo = m_SceneLogicInfoManager.AddSceneLogicInfo(cfg);
            if (null != logicInfo) {
                id = logicInfo.GetId();
            }
            return id;
        }
        public SceneLogicInfo CreateSceneLogic(int infoId, int configId, int logicId, params string[] args)
        {
            SceneLogicConfig cfg = new SceneLogicConfig();
            cfg.m_ConfigId = configId;
            cfg.m_LogicId = logicId;
            cfg.m_Params = args;
            SceneLogicInfo logicInfo = m_SceneLogicInfoManager.AddSceneLogicInfo(infoId, cfg);
            return logicInfo;
        }
        public void DestroySceneLogic(int id)
        {
            m_SceneLogicInfoManager.RemoveSceneLogicInfo(id);
        }
        public void DestroySceneLogicByConfigId(int configId)
        {
            SceneLogicInfo info = m_SceneLogicInfoManager.GetSceneLogicInfoByConfigId(configId);
            if (null != info) {
                m_SceneLogicInfoManager.RemoveSceneLogicInfo(info.GetId());
            }
        }
        public SceneLogicInfo GetSceneLogicInfo(int id)
        {
            SceneLogicInfo info = m_SceneLogicInfoManager.GetSceneLogicInfo(id);
            return info;
        }
        public SceneLogicInfo GetSceneLogicInfoByConfigId(int configId)
        {
            SceneLogicInfo info = m_SceneLogicInfoManager.GetSceneLogicInfoByConfigId(configId);
            return info;
        }
        public void HighlightPromptWithDict(string id, params object[] args)
        {
            string info = Dict.Format(id, args);
            HighlightPrompt(info);
        }
        public void HighlightPrompt(string txt)
        {
            Utility.EventSystem.Publish("ui_highlight_prompt", "ui", txt);
        }
        public EntityInfo GetLeaderEntityInfo()
        {
            return m_EntityManager.GetEntityInfo(m_LeaderId);
        }

        private void TickEntities(long curTime, long deltaTime)
        {
            while (m_NpcOnAttackedToDeadQueue.Count > 0) {
                int npcId = m_NpcOnAttackedToDeadQueue.Dequeue();
                NpcOnAttackedToDead(npcId);
            }
            m_DeletedEntities.Clear();
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                info.RetireAttackerInfos(10000);
                //Ai hooking and switching
                var aiStateInfo = info.GetAiStateInfo();
                if (string.IsNullOrEmpty(aiStateInfo.AiLogic)) {
                    if (null != aiStateInfo.AiStoryInstanceInfo) {
                        DetachAiLogic(info);
                    }
                } else {
                    if (null == aiStateInfo.AiStoryInstanceInfo) {
                        AttachAiLogic(info);
                    }
                }
                //Property calculation
                if (info.LevelChanged || info.GetSkillStateInfo().BuffChanged) {
                    AttrCalculator.Calc(info);
                    info.LevelChanged = false;
                    info.GetSkillStateInfo().BuffChanged = false;
                }
                //buff time check
                var impacts = info.GetSkillStateInfo().GetAllImpact();
                for (int i = impacts.Count - 1; i >= 0; --i) {
                    var impact = impacts[i];
                    if (impact.ConfigData.type == (int)SkillOrImpactType.Buff) {
                        if (impact.ConfigData.ImpactCooldown <= 0) {
                            if (impact.StartTime + impact.DurationTime < curTime) {
                                GfxSkillSystem.Instance.StopSkill(info.GetId(), impact.ImpactId, impact.Seq, false);
                            }
                        }
                    }
                }
                //Birth and Death Processing
                if (info.IsBorning) {
                    if (info.BornTime <= 0) {
                        SkillInfo skillInfo = info.GetSkillStateInfo().GetSkillInfoById(info.BornSkillId);
                        if (info.BornSkillId > 0 && null != skillInfo) {
                            info.BornTime = TimeUtility.GetLocalMilliseconds();
                            GfxSkillSystem.Instance.StartSkill(info.GetId(), skillInfo.ConfigData, 0);
                        } else {
                            info.IsBorning = false;
                            info.BornTime = 0;
                            info.SetAIEnable(true);
                            info.RemoveState(CharacterPropertyEnum.x3009_无敌);
                        }
                    } else if (info.BornTime + info.BornTimeout < TimeUtility.GetLocalMilliseconds()) {
                        info.IsBorning = false;
                        info.BornTime = 0;
                        info.SetAIEnable(true);
                        info.RemoveState(CharacterPropertyEnum.x3009_无敌);
                    }
                }
                if (info.IsDead() && !info.NeedDelete) {
                    if (info.CanDead) {
                        if (info.DeadTime <= 0) {
                            SkillInfo skillInfo = info.GetSkillStateInfo().GetSkillInfoById(info.DeadSkillId);
                            if (info.DeadSkillId > 0 && null != skillInfo) {
                                info.DeadTime = TimeUtility.GetLocalMilliseconds();
                                GfxSkillSystem.Instance.StopAllSkill(info.GetId(), true, false, true);
                                GfxSkillSystem.Instance.StartSkill(info.GetId(), skillInfo.ConfigData, 0);
                                OnEntityKilled(info);
                                EntityDrop(info); // fall
                            } else {
                                info.DeadTime = 0;
                                info.NeedDelete = true;
                                OnEntityKilled(info);
                                EntityDrop(info); // fall
                            }
                        } else if (info.DeadTime + info.DeadTimeout < TimeUtility.GetLocalMilliseconds()) {
                            info.DeadTime = 0;
                            info.NeedDelete = true;
                        }
                    } else {
                        info.CanDead = true;
                    }
                } else {
                    //CanDead is reset with each tick. When you need to kill corpses in a skill,
                    //you should use a trigger to mark the target as non-deadable (keeplive)
                    //every frame.
                    info.CanDead = true;
                }
                if (info.NeedDelete) {
                    m_DeletedEntities.Add(info);
                }
            }
            if (m_DeletedEntities.Count > 0) {
                int enemyCt = 0;
                int friendCt = 0;
                for (int i = 0; i < m_DeletedEntities.Count; ++i) {
                    EntityInfo ni = m_DeletedEntities[i];
                    if (CharacterRelation.RELATION_ENEMY==EntityInfo.GetRelation(CampId, ni.GetCampId())) {
                        ++enemyCt;
                    } else if (CharacterRelation.RELATION_FRIEND == EntityInfo.GetRelation(CampId, ni.GetCampId())) {
                        ++friendCt;
                    }
                    DestroyEntity(ni);
                }
                TryAllKilledOrAllDied(enemyCt > 0, friendCt > 0);
            }
        }

        private void EntityDrop(EntityInfo info)
        {
        }
        private void DestroyEntity(EntityInfo ni)
        {
            GfxSkillSystem.Instance.StopAllSkill(ni.GetId(), true, true, true);
            ni.GetSkillStateInfo().RemoveAllImpact();
            OnDestroyEntity(ni);
            if (ni.IsCombatNpc()) {
                ni.DeadTime = 0;
            }
            EntityViewModelManager.Instance.DestroyEntityView(ni.GetId());
            DestroyEntityById(ni.GetId());
        }
        private void OnEntityKilled(EntityInfo ni)
        {
            if (ni.GetMovementStateInfo().IsMoving) {
                ni.GetMovementStateInfo().IsMoving = false;
            }
            int leftEnemyCt = GetBattleNpcCount(CampId, CharacterRelation.RELATION_ENEMY);
            int leftFriendCt = GetBattleNpcCount(CampId);

            GfxStorySystem.Instance.SendMessage("obj_killed", ni.GetId(), leftEnemyCt, leftFriendCt);
            GfxStorySystem.Instance.SendMessage("npc_killed:" + ni.GetUnitId(), ni.GetId(), leftEnemyCt, leftFriendCt);
        }
        private void TryAllKilledOrAllDied(bool tryAllKilled, bool tryAllDied)
        {
            if (tryAllKilled) {
                int leftEnemyCt = GetBattleNpcCount(CampId, CharacterRelation.RELATION_ENEMY) + GetDyingBattleNpcCount(CampId, CharacterRelation.RELATION_ENEMY);
                if (leftEnemyCt <= 0) {
                    GfxStorySystem.Instance.SendMessage("all_killed");
                }
            }
            if (tryAllDied) {
                int leftFriendCt = GetBattleNpcCount(CampId) + GetDyingBattleNpcCount(CampId);
                if (leftFriendCt <= 0) {
                    GfxStorySystem.Instance.SendMessage("all_died");
                }
            }
        }
        private void NpcOnAttackedToDead(int id)
        {
            for (LinkedListNode<EntityInfo> linkNode = m_EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next)
            {
                EntityInfo info = linkNode.Value;
                var impacts = info.GetSkillStateInfo().GetAllImpact();
                for (int i = 0; i < impacts.Count; ++i)
                {
                    var impact = impacts[i];
                    if (null != impact && null != impact.ConfigData.impactData)
                    {
                        if (id == impact.ImpactSenderId)
                        {
                            GfxSkillSystem.Instance.StopSkill(info.GetId(), impact.ImpactId, impact.Seq, false);
                        }
                    }
                }
            }
        }
        private void OnCreateEntity(EntityInfo entity)
        {
            if (null != entity) {
                OnAiInitDslLogic(entity);
                GfxStorySystem.Instance.SendMessage("obj_created", entity.GetId());
                GfxStorySystem.Instance.SendMessage(string.Format("npc_created:{0}", entity.GetUnitId()), entity.GetId());

                bool isGreen = CharacterRelation.RELATION_FRIEND == EntityInfo.GetRelation(entity.GetCampId(), CampId);
                Utility.EventSystem.Publish("ui_add_actor", "ui", entity.GetId(), isGreen, entity.ConfigData);
            }
        }
        private void OnDestroyEntity(EntityInfo entity)
        {
            if (null != entity) {
                OnAiDestroy(entity);
                if (null != m_SelectedTarget && entity == m_SelectedTarget.Target) {
                    SetLockTarget(0);
                }
                Utility.EventSystem.Publish("ui_remove_actor", "ui", entity.GetId());
            }
        }

        private void OnDamage(int receiver, int caster, bool isNormalDamage, bool isCritical, int hpDamage, int npDamage)
        {
            if (receiver == LeaderId && caster > 0) {
                bool newSelect = true;
                if (null != SelectedTarget) {
                    EntityInfo curTarget = GetEntityById(PluginFramework.Instance.SelectedTarget.TargetId);
                    if (curTarget == SelectedTarget.Target) {
                        newSelect = false;
                    }
                }
                if (newSelect) {
                    SetLockTarget(caster);
                }
            }
            EntityInfo entity = GetEntityById(receiver);
            EntityInfo casterNpc = GetEntityById(caster);
            while (null != casterNpc && casterNpc.SummonerId > 0) {
                casterNpc = GetEntityById(casterNpc.SummonerId);
                if (null != casterNpc) {
                    caster = casterNpc.GetId();
                }
            }
            if (null != entity) {
                if (hpDamage != 0) {
                    float hp = (float)entity.Hp / entity.HpMax;
                    Utility.EventSystem.Publish("ui_actor_hp", "ui", entity.GetId(), hp);
                    //if (receiver == LeaderId || caster == LeaderId) {
                        Utility.EventSystem.Publish("ui_show_hp_num", "ui", entity.GetId(), -hpDamage);
                    //}
                    if (caster == LeaderId) {
                        EntityViewModel view = EntityController.Instance.GetEntityViewById(receiver);
                        if (view != null) {
                            view.SetRedEdge(1.0f);
                        }
                    }
                }
                if (npDamage != 0) {
                    float mp = (float)entity.Energy / entity.EnergyMax;
                    Utility.EventSystem.Publish("ui_actor_mp", "ui", entity.GetId(), mp, -npDamage);
                    //PluginFrameworkUtility.EventSystem.Publish("ui_show_mp_num", "ui", entity.GetId(), -npDamage);
                }
            }
        }
        private void PreloadUiStories()
        {
            if (null != m_SceneInfo) {
                TableConfig.Level level = m_SceneInfo;
                int ct = level.SceneUi.Count;
                for (int ix = 0; ix < ct; ++ix) {
                    int uiId = level.SceneUi[ix];
                    TableConfig.UI ui = TableConfig.UIProvider.Instance.GetUI(uiId);
                    if (null != ui) {
                        if (!string.IsNullOrEmpty(ui.dsl) && !string.IsNullOrEmpty(ui.name)) {
                            GfxStorySystem.Instance.LoadStory(ui.name, ui.dsl);
                        }
                    }
                }
            }
        }
        private void OnSelectedTargetChange(int oldSelect, int newSelect)
        {
            if (null == m_SelectedEffect) {
                m_SelectedEffect = ResourceSystem.Instance.NewObject("Effects/Select") as UnityEngine.GameObject;
            }
            if (null == m_SelectedEffect) {
                return;
            }
            m_SelectedEffect.transform.SetParent(null);
            if (newSelect > 0) {
                EntityViewModel viewModel = EntityController.Instance.GetEntityViewById(newSelect);
                if (null != viewModel && null != viewModel.Actor) {
                    m_SelectedEffect.transform.SetParent(viewModel.Actor.transform);
                    m_SelectedEffect.transform.localPosition = UnityEngine.Vector3.zero;
                    m_SelectedEffect.transform.localRotation = UnityEngine.Quaternion.identity;
                    m_SelectedEffect.SetActive(true);
                } else {
                    m_SelectedEffect.SetActive(false);
                }
            } else {
                m_SelectedEffect.SetActive(false);
            }
            Utility.EventSystem.Publish("ui_lock_target", "ui", newSelect);
        }

        public int SceneId
        {
            get { return m_SceneId; }
        }
        public TableConfig.Level SceneInfo
        {
            get { return m_SceneInfo; }
        }
        public int BattleSceneId
        {
            get { return m_BattleSceneId; }
        }
        public TableConfig.Level BattleSceneInfo
        {
            get { return m_BattleSceneInfo; }
        }
        public bool IsMainUiScene
        {
            get
            {
                bool ret = false;
                if (null != m_SceneInfo) {
                    ret = m_SceneInfo.type == (int)SceneTypeEnum.MainUi;
                }
                return ret;
            }
        }
        public bool IsBattleState
        {
            get { return m_IsBattleState; }
        }
        public LockTargetInfo SelectedTarget
        {
            get { return m_SelectedTarget; }
        }
        public int RoomObjId
        {
            get { return m_RoomObjId; }
            internal set { m_RoomObjId = value; }
        }
        public int RoomUnitId
        {
            get { return m_RoomUnitId; }
            internal set { m_RoomUnitId = value; }
        }
        public int LeaderId
        {
            get { return m_LeaderId; }
            set { m_LeaderId = value; }
        }
        public int CampId
        {
            get { return m_CampId; }
            set { m_CampId = value; }
        }

        public EntityManager EntityManager
        {
            get { return m_EntityManager; }
        }
        public SceneLogicInfoManager SceneLogicInfoManager
        {
            get { return m_SceneLogicInfoManager; }
        }
        public BlackBoard BlackBoard
        {
            get { return m_BlackBoard; }
        }
        public KdObjectTree KdTree
        {
            get { return m_KdTree; }
        }
        public SceneContextInfo SceneContext
        {
            get { return m_SceneContextInfo; }
        }
        public SortedList<string, string> CommandDocs
        {
            get { return m_CommandDocs; }
        }
        public SortedList<string, string> FunctionDocs
        {
            get { return m_FunctionDocs; }
        }

        #region delay action process (In order not to trigger jit compilation, repackage it here)
        public void QueueAction(MyAction action)
        {
            m_AsyncActionProcessor.QueueAction(action);
        }
        public void QueueAction<T1>(MyAction<T1> action, T1 t1)
        {
            QueueActionWithDelegation(action, t1);
        }
        public void QueueAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
        {
            QueueActionWithDelegation(action, t1, t2);
        }
        public void QueueAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            QueueActionWithDelegation(action, t1, t2, t3);
        }
        public void QueueAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4);
        }
        public void QueueAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6>(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7>(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }
        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }
        public void QueueFunc<R>(MyFunc<R> action)
        {
            QueueActionWithDelegation(action);
        }
        public void QueueFunc<T1, R>(MyFunc<T1, R> action, T1 t1)
        {
            QueueActionWithDelegation(action, t1);
        }
        public void QueueFunc<T1, T2, R>(MyFunc<T1, T2, R> action, T1 t1, T2 t2)
        {
            QueueActionWithDelegation(action, t1, t2);
        }
        public void QueueFunc<T1, T2, T3, R>(MyFunc<T1, T2, T3, R> action, T1 t1, T2 t2, T3 t3)
        {
            QueueActionWithDelegation(action, t1, t2, t3);
        }
        public void QueueFunc<T1, T2, T3, T4, R>(MyFunc<T1, T2, T3, T4, R> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, R>(MyFunc<T1, T2, T3, T4, T5, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, R>(MyFunc<T1, T2, T3, T4, T5, T6, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }
        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            QueueActionWithDelegation(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }
        public void QueueActionWithDelegation(Delegate action, params object[] args)
        {
            if (null != m_AsyncActionProcessor) {
                m_AsyncActionProcessor.QueueActionWithDelegation(action, args);
            }
        }
        #endregion

        private long m_LastTickTime = 0;
        private int m_SceneId;
        private TableConfig.Level m_SceneInfo;
        private int m_BattleSceneId;
        private TableConfig.Level m_BattleSceneInfo;

        private KdObjectTree m_KdTree = new KdObjectTree();
        private BlackBoard m_BlackBoard = new BlackBoard();
        private EntityManager m_EntityManager = new EntityManager(256);
        private SceneLogicInfoManager m_SceneLogicInfoManager = new SceneLogicInfoManager(256);
        private SceneContextInfo m_SceneContextInfo = new SceneContextInfo();

        private SceneLogicSystem m_SceneLogicSystem = new SceneLogicSystem();
        private List<EntityInfo> m_DeletedEntities = new List<EntityInfo>();
        private ClientAsyncActionProcessor m_AsyncActionProcessor = new ClientAsyncActionProcessor();
        private bool m_IsSceneLoaded = true;

        private int m_RoomObjId;
        private int m_RoomUnitId;
        private int m_LeaderId;
        private int m_CampId = (int)CampIdEnum.Blue;
        private string m_Nick = string.Empty;
        private bool m_UseNetwork = true;
        private bool m_IsBattleState = false;
        private LockTargetInfo m_SelectedTarget = null;
        private UnityEngine.GameObject m_SelectedEffect = null;
        private Queue<int> m_NpcOnAttackedToDeadQueue = new Queue<int>();

        private List<EntityInfo> m_EntitiesForAi = new List<EntityInfo>();
        private SortedList<string, string> m_CommandDocs;
        private SortedList<string, string> m_FunctionDocs;
    }
}
